using System;
using System.Collections.Generic;
using System.Text;

namespace ArcherFusion.MethodState
{
    public interface IExpressionCompilerFactory
    {
        IExpressionCompiler CreateCompiler();
    }
}
