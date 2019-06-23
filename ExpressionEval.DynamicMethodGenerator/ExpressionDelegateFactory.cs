using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text;
using ExpressionEval.MethodState;
using ExpressionEval.MsilConversion;

namespace ExpressionEval.DynamicMethodGenerator
{
    /// <summary>
    /// Implements a Delegate Factory for compiled expressions
    /// </summary>
    public class ExpressionDelegateFactory : IExpressionDelegateFactory
    {
        private ExpressionLanguage m_language;

        /// <summary>
        /// Delegate Factory for a Language
        /// </summary>
        /// <param name="language"></param>
        public ExpressionDelegateFactory(ExpressionLanguage language)
        {
            m_language = language;
        }

        /// <summary>
        /// Compiles an expression and returns a delegate to the compiled code.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>ExecuteExpression&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        public ExecuteExpression<R, C> CreateExpressionDelegate<R, C>(string expression)
        {
            ExecuteExpression<R, C> expressionDelegate = null;
            DynamicMethodState methodState;

            //create the compiled expression
            methodState = CreateExpressionMethodState<R, C>(expression);

            if (methodState != null && methodState.CodeBytes != null)
            {
                //get a dynamic method delegate from the method state
                expressionDelegate = CreateExpressionDelegate<R, C>(methodState);
            }

            return expressionDelegate;
        }

        /// <summary>
        /// Compiles a DynamicMethodState and returns a delegate.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="methodState">The serialized version of a method on the functionClass</param>
        /// <returns>ExecuteExpression&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        public ExecuteExpression<R, C> CreateExpressionDelegate<R, C>(DynamicMethodState methodState)
        {
            ExecuteExpression<R, C> expressionDelegate;

            //create a dynamic method
            DynamicMethod dynamicMethod = new DynamicMethod("_" + Guid.NewGuid().ToString("N"), typeof(R), new Type[] { typeof(C) }, typeof(C));

            //get the IL writer for it
            DynamicILInfo dynamicInfo = dynamicMethod.GetDynamicILInfo();

            //set the properties gathered from the compiled expression
            dynamicMethod.InitLocals = methodState.InitLocals;

            //set local variables
            SignatureHelper locals = SignatureHelper.GetLocalVarSigHelper();
            foreach (int localIndex in methodState.LocalVariables.Keys)
            {
                LocalVariable localVar = methodState.LocalVariables[localIndex];
                locals.AddArgument(Type.GetTypeFromHandle(localVar.LocalType), localVar.IsPinned);
            }

            dynamicInfo.SetLocalSignature(locals.GetSignature());

            //resolve any metadata tokens
            IlTokenResolver tokenResolver = new IlTokenResolver(methodState.TokenOffset.Fields, methodState.TokenOffset.Methods, methodState.TokenOffset.Types, methodState.TokenOffset.LiteralStrings);
            methodState.CodeBytes = tokenResolver.ResolveCodeTokens(methodState.CodeBytes, dynamicInfo);

            //set the IL code for the dynamic method
            dynamicInfo.SetCode(methodState.CodeBytes, methodState.MaxStackSize);

            //create a delegate for fast execution
            expressionDelegate = (ExecuteExpression<R, C>)dynamicMethod.CreateDelegate(typeof(ExecuteExpression<R, C>));

            return expressionDelegate;
        }

        /// <summary>
        /// Compiles an expression and returns a DynamicMethodState
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>DynamicMethodState - serialized version of the compiled expression</returns>
        public DynamicMethodState CreateExpressionMethodState<R, C>(string expression)
        {
            DynamicMethodState methodState;
            IExpressionCompiler compiler;

            //create an AppDomain
            AppDomainSetup loSetup = new AppDomainSetup();
            loSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain loAppDomain = AppDomain.CreateDomain("CompilerDomain", null, loSetup);

            //get the compiler to use based on the language
            string className = null;
            switch(m_language)
            {
                case ExpressionLanguage.CSharp:
                    className = "CSharpExpressionCompiler";
                    break;
                case ExpressionLanguage.VisualBasic:
                    className = "VisualBasicExpressionCompiler";
                    break;
                case ExpressionLanguage.JScript:
                    className = "JScriptExpressionCompiler";
                    break;
            }

            //create an instance of a compiler
            compiler = (IExpressionCompiler)loAppDomain.CreateInstanceFromAndUnwrap("ExpressionEval.ExpressionCompiler.dll", "ExpressionEval.ExpressionCompiler." + className);

            try
            {
                //compile the expression
                methodState = compiler.CompileExpression(expression, typeof(C), typeof(R));
            }
            catch (CompileException e)
            {
                //catch any compile errors and throw an overall exception
                StringBuilder exceptionMessage = new StringBuilder();

                foreach (CompilerError error in e.CompileErrors)
                {
                    exceptionMessage.Append("Error# ").Append(error.ErrorNumber);
                    exceptionMessage.Append(", column ").Append(error.Column);
                    exceptionMessage.Append(", ").Append(error.ErrorText);
                    exceptionMessage.Append(Environment.NewLine);
                }

                throw new ApplicationException(exceptionMessage.ToString());
            }
            finally
            {
                //unload the AppDomain
                AppDomain.Unload(loAppDomain);
            }

            //if for some reason the code byte were not sent then return null
            if (methodState != null && methodState.CodeBytes == null)
            {
                methodState = null;
            }

            return methodState;
        }

    }
}
