using System;
using ExpressionEval.MethodState;

namespace ExpressionEval.ExpressionEvaluation
{
    /// <summary>
    /// Generic delegate  returned by an expression evaluator.
    /// </summary>
    /// <typeparam name="R">The return type of the method built on the expression</typeparam>
    /// <typeparam name="C">The type of the function class</typeparam>
    /// <param name="functionClass">An instance of the function class the method is built against</param>
    /// <returns>R - an instance of the return type for the expression</returns>
    public delegate R CompiledCode<R,C>(C functionClass);

    /// <summary>
    /// Defines the methods an evaluator needs to provide.
    /// </summary>
    public interface ICodeCompiler
    {
        /// <summary>
        /// Evaluates an expression and returns the RESULT VALUE.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <param name="functionClass">An instance of the function class the method is built against</param>
        /// <returns>R - an instance of the return type for the expression</returns>
        R CompileToResult<R, C>(string expression, C functionClass);

        /// <summary>
        /// Compiles an expression and returns a delegate to the COMPILED CODE.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>CompiledCode&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        CompiledCode<R, C> GetDelegate<R, C>(string expression);

        /// <summary>
        /// Compiles an expression and returns a DynamicMethodState
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>DynamicMethodState - serialized version of the compiled expression</returns>
        DynamicMethodState GetMethodState<R, C>(string expression);

        /// <summary>
        /// Compiles a DynamicMethodState and returns a delegate.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="methodState">The serialized version of a method on the functionClass</param>
        /// <returns>CompiledCode&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        CompiledCode<R, C> GetDelegate<R, C>(DynamicMethodState methodState);
    }
}
