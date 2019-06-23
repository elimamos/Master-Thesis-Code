using System;

namespace ExpressionEval.MethodState
{
    /// <summary>
    /// Interface to an expression compiler.  In a shared assembly so can be marshaled across an AppDomain.
    /// </summary>
    public interface IExpressionCompiler
    {
        /// <summary>
        /// Compiles the expression into an assembly and returns the method code for it.
        /// It should compile the method into a class that inherits from the functionType.
        /// </summary>
        /// <param name="expression">expression to be compiled</param>
        /// <param name="functionType">Type of the function class to use</param>
        /// <param name="returnType">Return type of the method to create</param>
        /// <returns>DynamicMethodState - A serialized version of the method code</returns>
        DynamicMethodState CompileExpression(string expression, Type functionType, Type returnType);
    }
}
