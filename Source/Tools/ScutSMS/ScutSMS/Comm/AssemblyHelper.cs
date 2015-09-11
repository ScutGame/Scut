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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using ProtoBuf;

namespace Scut.SMS.Comm
{

    public class AssemblyHelper
    {
        public static Assembly LoadDynamicAssembly(ICollection<string> paths, out string error)
        {
            List<string> codeList = new List<string>();
            var sb1 = new StringBuilder();
            var ass = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZyGames.Framework.dll"));

            sb1.AppendLine("using System;");
            sb1.AppendLine("using ProtoBuf;");
            sb1.AppendLine("using System.Collections.Generic;");
            sb1.AppendLine("using System.Collections.Concurrent;");
            sb1.AppendLine("using ZyGames.Framework.Common;");
            sb1.AppendLine("using ZyGames.Framework;");
            sb1.AppendLine("namespace DynamicCodeGenerate");
            sb1.AppendLine("{");
            sb1.Append(MappingCode(ass));
            sb1.AppendLine(@"public class HashDict<TValue> : Dictionary<string, TValue>
                {
                    public HashDict()
                        : base(StringComparer.InvariantCultureIgnoreCase)
                    {

                    }
            }");
            sb1.AppendLine("}");
            codeList.Add(sb1.ToString());

            var sb = new StringBuilder();
            int index = 1;
            foreach (var path in paths)
            {
                sb.AppendLine("using System;");
                sb.AppendLine("using ProtoBuf;");
                sb.AppendLine("using ZyGames.Framework.Common;");
                sb.AppendLine("using ZyGames.Framework;");
                sb.AppendLine("using ZyGames.Framework.Data;");
                sb.AppendLine("using ZyGames.Framework.Cache.Generic;");
                sb.AppendLine("using ZyGames.Framework.Model;");
                sb.AppendLine("using ZyGames.Framework.Event;");
                sb.AppendLine("using ZyGames.Framework.Game;");
                sb.AppendLine("using ZyGames.Framework.Game.Cache;");
                sb.AppendLine("using ZyGames.Framework.Game.Model;");
                sb.AppendLine("using ZyGames.Framework.Game.Message;");
                sb.AppendLine("using ZyGames.Framework.Game.Com.Model;");
                sb.AppendLine("using ZyGames.Framework.Game.Service;");
                sb.AppendLine("using ZyGames.Framework.Game.Context;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Collections.Concurrent;");
                sb.AppendLine("namespace DynamicCodeGenerate" + index);
                sb.AppendLine("{");
                var assembly = Assembly.LoadFile(path);
                var sb2 = MappingCode(assembly);
                sb.Append(sb2);
                sb.Append(Environment.NewLine);
                sb.Append("}");
                index++;
            }
            codeList.Add(sb.ToString());
            return Compile(codeList.ToArray(), out error);
        }

        private static Assembly Compile(string[] scriptCode, out string result)
        {
            result = "";
            CompilerResults cr;
            if (!Compile(scriptCode, out cr))
            {
                result += "Error:\r\n";
                foreach (CompilerError err in cr.Errors)
                {
                    result += err.Line + "\t" + err.ErrorText + "\r\n";
                }
                return null;
            }
            return cr.CompiledAssembly;

        }

        private static bool Compile(string[] scriptCode, out CompilerResults result)
        {
            // 1.CSharpCodePrivoder
            var objCSharpCodePrivoder = new CSharpCodeProvider();

            // 2.ICodeComplier
            //var objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            // 3.CompilerParameters
            var objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.ReferencedAssemblies.Add("protobuf-net.dll");
            objCompilerParameters.ReferencedAssemblies.Add("ZyGames.Framework.Common.dll");
            objCompilerParameters.ReferencedAssemblies.Add("ZyGames.Framework.dll");
            objCompilerParameters.ReferencedAssemblies.Add("ZyGames.Framework.Game.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;

            // 4.CompilerResults
            result = objCSharpCodePrivoder.CompileAssemblyFromSource(objCompilerParameters, scriptCode);
            return !result.Errors.HasErrors;
        }

        private static StringBuilder MappingCode(Assembly ass)
        {
            var dict = new Dictionary<string, Type>();
            var sb = new StringBuilder();
            sb.AppendLine();

            #region 非泛型类
            var types = ass.GetTypes().Where(p => p.GetCustomAttributes(typeof(ProtoContractAttribute), true).Count() > 0).ToList();
            for (int i = 0; i < types.Count; i++)
            {
                var myEntity = types[i];

                //重命名的类型
                if (dict.ContainsKey(myEntity.Name)) continue;
                dict[myEntity.Name] = myEntity.GetType();


                sb.AppendLine();
                sb.Append("[ProtoContract]");
                sb.AppendLine();
                sb.Append("public class ");
                sb.Append(GetTinyTypeName(myEntity));
                sb.AppendLine();
                sb.Append("{");
                //获得所有属性
                var Properties = myEntity.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Where(p => p.GetCustomAttributes(typeof(ProtoMemberAttribute), true).Count() > 0).ToList();
                var Fields = myEntity.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Where(p => p.GetCustomAttributes(typeof(ProtoMemberAttribute), true).Count() > 0).ToList();
                for (int j = 0; j < Properties.Count; j++)
                {
                    var tag = ((ProtoMemberAttribute)(Properties[j].GetCustomAttributes(typeof(ProtoMemberAttribute), true)[0])).Tag;
                    sb.AppendLine();
                    sb.AppendFormat("[ProtoMember({0})]", tag);
                    sb.AppendLine();
                    sb.Append("public ");
                    sb.Append(GetTinyTypeName(Properties[j].PropertyType));
                    sb.Append(" ");
                    sb.Append(Properties[j].Name);
                    sb.Append("{get;set;} ");
                }
                sb.AppendLine();
                sb.Append("}");
            }
            #endregion

            return sb;
        }

        private static string GetTinyTypeName(Type type)
        {
            var r = type.FullName;
            var typecode = Type.GetTypeCode(type);
            switch (typecode)
            {
                case TypeCode.Boolean: r = "bool"; break;
                case TypeCode.Byte:
                case TypeCode.SByte:
                    r = "byte"; break;
                case TypeCode.UInt16:
                case TypeCode.Int16:
                    r = "short"; break;
                case TypeCode.UInt32:
                case TypeCode.Int32:
                    r = "int"; break;
                case TypeCode.UInt64:
                case TypeCode.Int64:
                    r = "long"; break;
                case TypeCode.DateTime:
                    r = "DateTime"; break;
                case TypeCode.String:
                    r = "string"; break;
                case TypeCode.Single:
                    r = "float"; break;
                case TypeCode.Double:
                    r = "double"; break;
                case TypeCode.Decimal:
                    r = "decimal"; break;
                case TypeCode.Object:
                    if (type.IsGenericType)
                    {
                        var t = type.Name.Split('`');
                        var genericArguments = "";
                        for (int i = 0; i < int.Parse(t[1]); i++)
                        {
                            genericArguments += GetTinyTypeName(type.GetGenericArguments()[i]) + ",";
                        }
                        r = string.Format("{0}<{1}>", t[0], genericArguments.TrimEnd(','));
                    }
                    else
                    {
                        r = type.Name;
                    }
                    break;
            }
            return r;
        }


    }
}