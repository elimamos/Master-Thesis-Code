using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace ExpressionEval.MethodState
{
    /// <summary>
    /// A serialized version of a method.  Holds everything to create a DynamicMethod.
    /// </summary>
    [Serializable]
    public class DynamicMethodState
    {
        /// <summary>
        /// The IL bytes from compilation
        /// </summary>
        public byte[] codeBytes;

        /// <summary>
        /// Whether or not local variables are initialized
        /// </summary>
        public bool initLocals;

        /// <summary>
        /// The maximum size of the stack required for this method
        /// </summary>
        public int maxStackSize;

        /// <summary>
        /// Local variables defined for the method
        /// </summary>
        public IDictionary<int,LocalVariable> localVariables;

        /// <summary>
        /// Definition of tokens that need to be resolved
        /// </summary>
        public TokenOffset tokenOffset;

    }

    /// <summary>
    /// Defines a local variable for a method
    /// </summary>
    [Serializable]
    public class LocalVariable
    {
        /// <summary>
        /// Whether or not the variable is pinned in memory, used for working with unmanaged memory
        /// </summary>
        public bool isPinned;

        /// <summary>
        /// Type of the variable
        /// Represents a type using an internal metadata token.
        /// </summary>
        public RuntimeTypeHandle localType;

        public LocalVariable(bool isPinned, RuntimeTypeHandle localType)
        {
            this.isPinned = isPinned;
            this.localType = localType;
        }


    }

    /// <summary>
    /// A definition of tokens to resolve for given offsets in IL code bytes
    /// </summary>
    [Serializable]
    public class TokenOffset
    {
        /// <summary>
        /// fields to be resolved
        /// </summary>
        public IDictionary<int, RuntimeFieldHandle> fields;

        /// <summary>
        /// Method to be resolved
        /// </summary>
        public IDictionary<int, MethodBase> methods;

        /// <summary>
        /// types to be resolved
        /// </summary>
        public IDictionary<int, RuntimeTypeHandle> types;

        /// <summary>
        /// Literal strings to resolve
        /// </summary>
        public IDictionary<int, string> literalStrings;
    }
}
