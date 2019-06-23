using System;
using System.Collections.Generic;
using ExpressionEval.DynamicMethodGenerator;
using System.Reflection;
using ExpressionEval.MethodState;
using System.Text.RegularExpressions;

namespace ExpressionEval.ExpressionEvaluation
{
    /// <summary>
    /// Implementation of IExpressionEvaluator
    /// </summary>
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private ExpressionLanguage m_language;

        /// <summary>
        /// Constructor for an expression language
        /// </summary>
        /// <param name="language"></param>
        public ExpressionEvaluator(ExpressionLanguage language)
        {
            m_language = language;
        }

        /// <summary>
        /// Evaluates an expression and returns the result value.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <param name="functionClass">An instance of the function class the method is built against</param>
        /// <returns>R - an instance of the return type for the expression</returns>
        public R Evaluate<R, C>(string expression, C functionClass)
        {
            R result = default(R);
            EvalExpression<R, C> methodDelegate;

            //get delegate for expression
            methodDelegate = GetDelegate<R, C>(expression);

            if (methodDelegate != null)
            {
                //get result
                result = methodDelegate(functionClass);
            }
            
            return result;
        }

        /// <summary>
        /// Compiles an expression and returns a delegate to the compiled code.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>EvalExpression&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        public EvalExpression<R, C> GetDelegate<R, C>(string expression)
        {
            DynamicMethodState methodState;
            EvalExpression<R, C> methodDelegate = null;

            //get compiled method
            methodState = GetMethodState<R, C>(expression);

            if (methodState != null)
            {
                //Get delegate for method state
                methodDelegate = GetDelegate<R, C>(methodState);
            }

            return methodDelegate;
        }

        /// <summary>
        /// Compiles an expression and returns a DynamicMethodState
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>DynamicMethodState - serialized version of the compiled expression</returns>
        public DynamicMethodState GetMethodState<R, C>(string expression)
        {
            DynamicMethodState methodState;

            //get delegate factory
            IExpressionDelegateFactory delegateFactory = new ExpressionDelegateFactory(m_language);

            //check the function class type to be sure it can be inherited
            Type functionType = typeof(C);

            if (!functionType.IsClass || functionType.IsSealed)
            {
                throw new ApplicationException("Function Type must be a class that is not sealed.");
            }

            //the implementation of the compiler requires an empty constructor
            ConstructorInfo[] infos = functionType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            bool hasEmptyConstructor = false;
            foreach (ConstructorInfo info in infos)
            {
                if (info.GetParameters().Length == 0 && info.IsStatic == false && info.IsPrivate == false)
                {
                    hasEmptyConstructor = true;
                }
            }

            if (!hasEmptyConstructor)
            {
                throw new ApplicationException("Function Type must be have a parameterless constructor defined.");
            }

            //get the method state
            methodState = delegateFactory.CreateExpressionMethodState<R, C>(expression);

            return methodState;
        }

        /// <summary>
        /// Compiles a DynamicMethodState and returns a delegate.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="methodState">The serialized version of a method on the functionClass</param>
        /// <returns>EvalExpression&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        public EvalExpression<R, C> GetDelegate<R, C>(DynamicMethodState methodState)
        {
            ExecuteExpression<R, C> methodDelegate = null;

            //get delegate factory
            IExpressionDelegateFactory delegateFactory = new ExpressionDelegateFactory(m_language);

            if (methodState != null && methodState.CodeBytes != null)
            {
                //get delegate from factory
                methodDelegate = delegateFactory.CreateExpressionDelegate<R, C>(methodState);
            }

            //return an eval delegate based on the delegate from the factory
            return new EvalExpression<R, C>(methodDelegate);
        }
    }
}
