using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.CodeDom.Compiler;

namespace ExpressionEval.MethodState
{
    /// <summary>
    /// A custom exception that will be thrown when there is a compile exception when using CodeDom
    /// </summary>
    [Serializable]
    public class CompileException : ApplicationException, ISerializable
    {
        private CompilerErrorCollection m_errors;

        protected CompileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            m_errors = (CompilerErrorCollection)info.GetValue("Errors", typeof(CompilerErrorCollection));
        }

        public CompileException(string message) : base(message)
        {
        }

        public CompileException(CompilerErrorCollection errors) : base()
        {
            m_errors = errors;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            
            info.AddValue("Errors", m_errors);
        }

        public CompilerErrorCollection CompileErrors
        {
            get { return m_errors; }
        }
    }
}
