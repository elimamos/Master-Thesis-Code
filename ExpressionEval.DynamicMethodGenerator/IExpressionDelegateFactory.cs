using ExpressionEval.MethodState;

namespace ExpressionEval.DynamicMethodGenerator
{
    /// <summary>
    /// Delegate returned by an expression delegate factory.
    /// </summary>
    /// <typeparam name="R">The return type of the method built on the expression</typeparam>
    /// <typeparam name="C">The type of the function class</typeparam>
    /// <param name="functionClass">An instance of the function class the method is built against</param>
    /// <returns>R - an instance of the return type for the expression</returns>
    public delegate R ExecuteExpression<R,C>(C functionClass);

    /// <summary>
    /// Defines the methods an expression delegate factory needs to provide.
    /// </summary>
    public interface IExpressionDelegateFactory
    {
        /// <summary>
        /// Compiles an expression and returns a delegate to the compiled code.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>ExecuteExpression&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        ExecuteExpression<R,C> CreateExpressionDelegate<R,C>(string expression);

        /// <summary>
        /// Compiles a DynamicMethodState and returns a delegate.
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="methodState">The serialized version of a method on the functionClass</param>
        /// <returns>ExecuteExpression&lt;R, C&gt; - a delegate that calls the compiled expression</returns>
        ExecuteExpression<R, C> CreateExpressionDelegate<R, C>(DynamicMethodState methodState);

        /// <summary>
        /// Compiles an expression and returns a DynamicMethodState
        /// </summary>
        /// <typeparam name="R">The return type of the expression</typeparam>
        /// <typeparam name="C">The type of the function class</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>DynamicMethodState - serialized version of the compiled expression</returns>
        DynamicMethodState CreateExpressionMethodState<R, C>(string expression);
    }
}
