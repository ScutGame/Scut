/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ZyGames.Framework.Common.Reflect
{
    /// <summary>
    /// 
    /// </summary>
    internal static class FastActivator
    {
        private static Dictionary<Type, Func<object[], object>> factoryCache = new Dictionary<Type, Func<object[], object>>();


        /// <summary>
        /// Creates an instance of the specified type using a generated factory to avoid using Reflection.
        /// </summary>
        /// <param Name="type">The type to be created.</param>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns>The newly created instance.</returns>
        public static object Create(Type type, params object[] args)
        {
            Func<object[], object> f;

            if (!factoryCache.TryGetValue(type, out f))
                lock (factoryCache)
                    if (!factoryCache.TryGetValue(type, out f))
                    {
                        Type[] typeArray = args.Select(obj => obj.GetType()).ToArray();
                        factoryCache[type] = f = BuildDeletgateObj(type, typeArray);
                    }

            return f(args);
        }

        private static Func<object[], object> BuildDeletgateObj(Type type, Type[] typeList)
        {
            ConstructorInfo constructor = type.GetConstructor(typeList);
            ParameterExpression paramExp = Expression.Parameter(typeof(object[]), "args_");
            Expression[] expList = GetExpressionArray(typeList, paramExp);
            NewExpression newExp = Expression.New(constructor, expList);
            Expression<Func<object[], object>> expObj = Expression.Lambda<Func<object[], object>>(newExp, paramExp);
            return expObj.Compile();
        }

        private static Expression[] GetExpressionArray(Type[] typeList, ParameterExpression paramExp)
        {
            List<Expression> expList = new List<Expression>();
            for (int i = 0; i < typeList.Length; i++)
            {
                var paramObj = Expression.ArrayIndex(paramExp, Expression.Constant(i));
                var expObj = Expression.Convert(paramObj, typeList[i]);
                expList.Add(expObj);
            }

            return expList.ToArray();
        }
    }
}