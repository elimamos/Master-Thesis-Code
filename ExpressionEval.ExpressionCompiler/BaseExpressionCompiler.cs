using System;
using System.Collections.Generic;
using System.Reflection;
using ExpressionEval.MsilConversion;
using ExpressionEval.MethodState;

namespace ExpressionEval.ExpressionCompiler
{
    /// <summary>
    /// A base expression compiler.  MarshalByRef so it can be used across AppDomains.
    /// </summary>
    public abstract class BaseExpressionCompiler : MarshalByRefObject
    {
        /// <summary>
        /// Converts a MethodInfo into a serialized version of it.
        /// </summary>
        /// <param name="dynamicMethod">The method for which to create a DynamicMethod for</param>
        /// <returns>DynamicMethodState - serialized version of a method.</returns>
        protected DynamicMethodState GetMethodState(MethodInfo dynamicMethod)
        {
            DynamicMethodState methodState = new DynamicMethodState();

            //IL info from method
            MethodBody methodIlCode = dynamicMethod.GetMethodBody();

            //get code bytes and other method properties
            methodState.CodeBytes = methodIlCode.GetILAsByteArray();
            methodState.InitLocals = methodIlCode.InitLocals;
            methodState.MaxStackSize = methodIlCode.MaxStackSize;

            //get any local variable information
            IDictionary<int, LocalVariable> locals = new SortedList<int, LocalVariable>();

            foreach (LocalVariableInfo localInfo in methodIlCode.LocalVariables)
            {
                locals.Add(localInfo.LocalIndex, new LocalVariable(localInfo.IsPinned, localInfo.LocalType.TypeHandle));
            }

            methodState.LocalVariables = locals;

            TokenOffset tokenOffset = new TokenOffset();

            //get metadata token offsets
            IlReader reader = new IlReader(methodState.CodeBytes, dynamicMethod.Module);

            tokenOffset.Fields = reader.Fields;
            tokenOffset.Methods = reader.Methods;
            tokenOffset.Types = reader.Types;
            tokenOffset.LiteralStrings = reader.LiteralStrings;

            methodState.TokenOffset = tokenOffset;

            return methodState;
        }
    }
}
