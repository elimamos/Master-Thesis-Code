using System;
using System.Reflection;
using System.CodeDom.Compiler;
using ExpressionEval.MethodState;


namespace ExpressionEval.ExpressionCompiler
{
    /// <summary>
    /// Expression compiler using the C# language
    /// </summary>
    public class CSharpExpressionCompiler : BaseExpressionCompiler, IExpressionCompiler
    {

        /// <summary>
        /// Compiles the expression into an assembly and returns the method code for it.
        /// It should compile the method into a class that inherits from the functionType.
        /// </summary>
        /// <param name="expression">expression to be compiled</param>
        /// <param name="functionType">Type of the function class to use</param>
        /// <param name="returnType">Return type of the method to create</param>
        /// <returns>DynamicMethodState - A serialized version of the method code</returns>
        public DynamicMethodState CompileExpression(string expression, Type functionType, Type returnType)
        {
            DynamicMethodState methodState;

            //use CodeDom to compile using C#
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

            CompilerParameters loParameters = new CompilerParameters();

            //add assemblies
            loParameters.ReferencedAssemblies.Add("System.dll");
            loParameters.ReferencedAssemblies.Add(functionType.Assembly.Location);
            //don't generate assembly on disk and treat warnings as errors
            loParameters.GenerateInMemory = true;
            loParameters.TreatWarningsAsErrors = true;

            //set namespace of dynamic class
            string dynamicNamespace = "ExpressionEval.Functions.Dynamic";

            //set source for inherited class - need to change to use CodeDom objects instead
            string source = @"
using System;
using {5};

namespace {6}
{{
    public class {0} : {1}
    {{
        public {2} {3}()
        {{
            {4};
        }}
    }}
}}
";

            //set source code replacements
            string className = "Class_" + Guid.NewGuid().ToString("N");
            string methodName = "Method_" + Guid.NewGuid().ToString("N");
            string returnTypeName = returnType.FullName;

            //check for generic type for return
            if (returnType.IsGenericType)
            {
                //check for nullable
                Type genericType = returnType.GetGenericTypeDefinition();
                if (genericType == typeof(Nullable<>))
                {
                    //nullable so add ?
                    Type nullableType = Nullable.GetUnderlyingType(returnType);
                    returnTypeName = nullableType.FullName + "?";
                }
                else
                {
                    //not nullable but is generic so get the list of types
                    Type[] genericArgTypes = returnType.GetGenericArguments();

                    //get type name without the last 2 characters for generic type names
                    returnTypeName = genericType.FullName.Substring(0, genericType.FullName.Length - 2) + "<";

                    //loop through type arguments and build out return type
                    foreach (Type genericArgType in genericArgTypes)
                    {
                        returnTypeName += genericArgType.FullName;
                    }

                    //add ending generic operator
                    returnTypeName += ">";
                }
            }

            //format codestring with replacements
            string codeString = string.Format(source, className, functionType.FullName, returnTypeName, methodName, expression, functionType.Namespace, dynamicNamespace);

            //compile the code
            CompilerResults results = codeProvider.CompileAssemblyFromSource(loParameters, codeString);

            if (results.Errors.Count > 0)
            {
                //throw an exception for any errors
                 throw new CompileException(results.Errors);
                //methodState = null;
            }
            else
            {
                //get the type that was compiled
                Type dynamicType = results.CompiledAssembly.GetType(dynamicNamespace + "." + className);

                //get the MethodInfo for the compiled expression
                MethodInfo dynamicMethod = dynamicType.GetMethod(methodName);

                //get the compiled expression as serializable object
                methodState = GetMethodState(dynamicMethod);

            }

            return methodState;
        }
    }
}
