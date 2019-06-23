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
using System.Collections.Generic;
using ArcherFusion.DynamicMethodGenerator;

namespace ArcherFusion.ExpressionEvaluator
{
    public class CSharpExpressionEvaluator : IExpressionEvaluator
    {
        private static Dictionary<int, Delegate> m_delegateCache = new Dictionary<int, Delegate>();

        public T Evaluate<T>(string expression, object functionClass)
        {
            T result = default(T);
            EvalExpression<T> methodDelegate;

            methodDelegate = GetDelegate<T>(expression, functionClass);

            if (methodDelegate != null)
            {
                result = methodDelegate();
            }
            
            return result;
        }

        public EvalExpression<T> GetDelegate<T>(string expression, object functionClass)
        {
            ExecuteExpression<T> methodDelegate;
            int expressionHashCode = expression.GetHashCode();

            if (m_delegateCache.ContainsKey(expressionHashCode))
            {
                methodDelegate = (ExecuteExpression<T>)m_delegateCache[expressionHashCode];
            }
            else
            {
                IExpressionDelegateFactory delegateFactory = new CSharpExpressionDelegateFactory();

                methodDelegate = delegateFactory.CreateExpressionDelegate<T>(expression, functionClass);

                if (methodDelegate != null)
                {
                    m_delegateCache.Add(expressionHashCode, methodDelegate);
                }
            }

            return new EvalExpression<T>(methodDelegate);
        }

    }
}
