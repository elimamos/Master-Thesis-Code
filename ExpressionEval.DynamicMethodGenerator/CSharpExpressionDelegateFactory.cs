#region Header

/*
{*******************************************************************}
{                                                                   }
{       Archer Technologies Archer.Engine.Generic Library           }
{       The Archer SmartSuite Framework(tm)                         }
{                                                                   }
{       Copyright © 2003-2007 Archer Technologies, LLC.             }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF ARCHER TECHNOLOGIES, LLC.                            }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM ARCHER TECHNOLOGIES, LLC.                   }
{                                                                   }
{   CONSULT THE LICENSE AGREEMENT FOR INFORMATION ON                }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/

#endregion

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text;
using ArcherFusion.MethodState;
using ArcherFusion.MsilConversion;

namespace ArcherFusion.DynamicMethodGenerator
{
    public class CSharpExpressionDelegateFactory : IExpressionDelegateFactory
    {

        public ExecuteExpression<T> CreateExpressionDelegate<T>(string expression, object functionClass)
        {
            ExecuteExpression<T> expressionDelegate = null;
            DynamicMethodState methodState;

            IExpressionCompiler compiler;

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            AppDomainSetup loSetup = new AppDomainSetup();
            loSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain loAppDomain = AppDomain.CreateDomain("CompilerDomain", null, loSetup);

            compiler = (IExpressionCompiler)loAppDomain.CreateInstanceFromAndUnwrap("ArcherFusion.ExpressionCompiler.dll", "ArcherFusion.ExpressionCompiler.CSharpExpressionCompiler");

            try
            {
                methodState = compiler.CompileExpression(expression, functionClass.GetType(), typeof(T));
            }
            catch (CompileException e)
            {
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
                AppDomain.Unload(loAppDomain);
            }

            stopWatch.Stop();

            if (methodState != null && methodState.CodeBytes != null)
            {
                expressionDelegate = CreateExpressionDelegate<T>(methodState, functionClass);
            }

            return expressionDelegate;
        }

        public ExecuteExpression<T> CreateExpressionDelegate<T>(DynamicMethodState methodState, object functionClass)
        {
            ExecuteExpression<T> expressionDelegate;
            Stopwatch stopWatch = new Stopwatch();

            Trace.WriteLine("Dynamic dll creation - " + stopWatch.ElapsedMilliseconds.ToString());

            stopWatch = new Stopwatch();

            stopWatch.Start();

            //create 
            DynamicMethod dynamicMethod = new DynamicMethod("", typeof(T), new Type[] { functionClass.GetType() }, functionClass.GetType());

            DynamicILInfo dynamicInfo = dynamicMethod.GetDynamicILInfo();

            dynamicMethod.InitLocals = methodState.InitLocals;

            SignatureHelper locals = SignatureHelper.GetLocalVarSigHelper();
            foreach (int localIndex in methodState.LocalVariables.Keys)
            {
                LocalVariable localVar = methodState.LocalVariables[localIndex];
                locals.AddArgument(localVar.LocalType, localVar.IsPinned);
            }

            dynamicInfo.SetLocalSignature(locals.GetSignature());

            IlTokenResolver tokenResolver = new IlTokenResolver(methodState.TokenOffset.Fields, methodState.TokenOffset.Methods, methodState.TokenOffset.Members, methodState.TokenOffset.Types, methodState.TokenOffset.LiteralStrings);

            methodState.CodeBytes = tokenResolver.ResolveCodeTokens(methodState.CodeBytes, dynamicInfo);

            dynamicInfo.SetCode(methodState.CodeBytes, methodState.MaxStackSize);

            expressionDelegate = (ExecuteExpression<T>)dynamicMethod.CreateDelegate(typeof(ExecuteExpression<T>), functionClass);

            stopWatch.Stop();

            Trace.WriteLine("Dynamic Method Creation - " + stopWatch.ElapsedMilliseconds.ToString());

            return expressionDelegate;
        }

    }
}
