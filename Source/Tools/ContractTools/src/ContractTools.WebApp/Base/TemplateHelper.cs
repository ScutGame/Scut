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
using System.Data;
using System.Text;
using System.Linq;
using System.IO;
using ContractTools.WebApp.Model;
using System.Collections;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.Model;

namespace ContractTools.WebApp.Base
{
    public class TemplateInfo : MemoryEntity
    {
        public string Content { get; set; }

    }

    public class TemplateHelper
    {
        private const int IndentWordNum = 4;
        private static char[] forVarChars = new[] { 'i', 'r', 'j', 'k', 'n', 'm', 'l' };
        private static MemoryCacheStruct<TemplateInfo> _tempCacheSet = new MemoryCacheStruct<TemplateInfo>();

        public static string ToFistWordCase(string str, bool isLower = true)
        {
            if (string.IsNullOrEmpty(str)) return "";
            string first = str[0].ToString();
            return (isLower ? first.ToLower() : first.ToUpper()) + str.Substring(1);
        }

        /// <summary>
        /// 转换为成员变量名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToMemberVarName(string name)
        {
            return string.Format("_{0}", ToFistWordCase(name));
        }

        public static Hashtable LoadEnumApplication(int slnid, bool clean)
        {
            if (System.Web.HttpContext.Current.Application[slnid.ToString()] == null || clean)
            {
                Hashtable ht = new Hashtable();
                var list = DbDataLoader.GetEnumInfo(slnid);
                foreach (var dr in list)
                {
                    string key = "【" + dr.enumName + "】";
                    if (!ht.Contains(key))
                    {
                        ht.Add(key, string.Format("{0}\n\n{1}", dr.enumDescription,
                            dr.enumValueInfo));
                    }
                }
                System.Web.HttpContext.Current.Application[slnid.ToString()] = ht;
                return ht;
            }
            else
            {
                return (Hashtable)System.Web.HttpContext.Current.Application[slnid.ToString()];
            }
        }
        /// <summary>
        /// 开启通用方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadTemp(string fileName)
        {
            string code = Path.GetFileName(fileName);
            TemplateInfo tempInfo;
            if (!_tempCacheSet.TryGet(code, out tempInfo))
            {
                tempInfo = new TemplateInfo();
                tempInfo.Content = File.ReadAllText(fileName, Encoding.UTF8);
                _tempCacheSet.TryAdd(code, tempInfo);

            }
            return tempInfo.Content;
        }

        /// <summary>
        /// 赋值客户端模板
        /// </summary>
        /// <param name="content"></param>
        /// <param name="paramList"></param>
        /// <param name="reqParams"></param>
        /// <param name="title"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public static string FromatClientLuaTemp(string content, int contractId, List<ParamInfoModel> paramList, List<ParamInfoModel> reqParams, string title)
        {
            string[] expressList = new string[] { "##ID##", "##Description##", "##Parameter##", "##Fixed##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));
                if (fieldBuilder.ToString() == "ID")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(contractId);
                }
                else if (fieldBuilder.ToString() == "Description")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(title);
                }
                else if (fieldBuilder.ToString() == "Parameter")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    foreach (var paramInfo in reqParams)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append(", ");
                            fieldBuilder.Append(ToFistWordCase(paramInfo.Field));
                        }
                    }
                }
                else if (fieldBuilder.ToString() == "Fixed")
                {
                    //write to request param
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    string currIndent = GetSpaceIndent(1, 0);
                    foreach (var paramInfo in reqParams)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append(Environment.NewLine);
                            fieldBuilder.Append(currIndent);
                            switch (paramInfo.FieldType)
                            {
                                case FieldType.UInt:
                                case FieldType.Int:
                                    fieldBuilder.Append("ZyWriter:writeInt32(\"");
                                    break;
                                case FieldType.UShort:
                                case FieldType.Short:
                                    fieldBuilder.Append("ZyWriter:writeWord(\"");
                                    break;
                                case FieldType.DateTime:
                                case FieldType.ULong:
                                case FieldType.Long:
                                    fieldBuilder.Append("ZyWriter:writeInt64(\"");
                                    break;
                                case FieldType.Float:
                                    fieldBuilder.Append("ZyWriter:writeFloat(\"");
                                    break;
                                default:
                                    fieldBuilder.Append("ZyWriter:writeString(\"");
                                    break;
                            }
                            fieldBuilder.Append(paramInfo.Field);
                            fieldBuilder.Append("\", ");
                            fieldBuilder.Append(ToFistWordCase(paramInfo.Field));
                            fieldBuilder.Append(")");
                        }
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return ReplaceClientLuaCallback(content, paramList);
        }

        public static string FromatClientQuickSendTemp(string content, int contractId, List<ParamInfoModel> paramList, List<ParamInfoModel> reqParams, string title)
        {
            string[] expressList = new string[] { "##ID##", "##Description##", "##Parameter##", "##Fixed##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));
                if (fieldBuilder.ToString() == "ID")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(contractId);
                }
                else if (fieldBuilder.ToString() == "Description")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(title);
                }
                else if (fieldBuilder.ToString() == "Parameter")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    foreach (var paramInfo in reqParams)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append(ToFistWordCase(paramInfo.Field));
                            fieldBuilder.Append(", ");
                        }
                    }
                }
                else if (fieldBuilder.ToString() == "Fixed")
                {
                    //write to request param
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    string currIndent = GetSpaceIndent(1, 0);
                    foreach (var paramInfo in reqParams)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append(Environment.NewLine);
                            fieldBuilder.Append(currIndent);
                            fieldBuilder.Append("url:add(\"");
                            fieldBuilder.Append(paramInfo.Field);
                            fieldBuilder.Append("\", ");
                            fieldBuilder.Append(ToFistWordCase(paramInfo.Field));
                            fieldBuilder.Append(")");
                        }
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return content;//ReplaceClientLuaCallback(content, paramList);
        }

        public static string FromatClientQuickReceiveTemp(string content, int contractId, List<ParamInfoModel> paramList, List<ParamInfoModel> reqParams, string title)
        {
            string[] expressList = new string[] { "##ID##", "##Description##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));
                if (fieldBuilder.ToString() == "ID")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(contractId);
                }
                else if (fieldBuilder.ToString() == "Description")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(title);
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return ReplaceClientLuaCallback(content, paramList, true);
        }


        public static string FromatClientCsharpTemp(string content, int contractId, List<ParamInfoModel> responseParams, List<ParamInfoModel> requestParams, string title)
        {
            string[] expressList = new string[] { "##ID##", "##Description##", "##Parameter##", "##Fixed##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));
                if (fieldBuilder.ToString() == "ID")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(contractId);
                }
                else if (fieldBuilder.ToString() == "Description")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(title);
                }
                else if (fieldBuilder.ToString() == "Parameter")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    foreach (var paramInfo in requestParams)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append(", ");
                            fieldBuilder.Append(ToFistWordCase(paramInfo.Field));
                        }
                    }
                }
                else if (fieldBuilder.ToString() == "Fixed")
                {
                    //write to request param
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    string currIndent = GetSpaceIndent(2, 0);
                    foreach (var paramInfo in requestParams)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append(Environment.NewLine);
                            fieldBuilder.Append(currIndent);
                            switch (paramInfo.FieldType)
                            {
                                case FieldType.Void:
                                    continue;
                                    break;
                                case FieldType.UInt:
                                case FieldType.Int:
                                    fieldBuilder.Append("writer.writeInt32(\"");
                                    break;
                                case FieldType.UShort:
                                case FieldType.Short:
                                    fieldBuilder.Append("writer.writeWord(\"");
                                    break;
                                case FieldType.DateTime:
                                case FieldType.ULong:
                                case FieldType.Long:
                                    fieldBuilder.Append("writer.writeInt64(\"");
                                    break;
                                case FieldType.Bool:
                                case FieldType.Byte:
                                    fieldBuilder.Append("writer.getByte(\"");
                                    break;
                                case FieldType.Float:
                                    fieldBuilder.Append("writer.writeFloat(\"");
                                    break;
                                default:
                                    fieldBuilder.Append("writer.writeString(\"");
                                    break;
                            }
                            fieldBuilder.Append(paramInfo.Field);
                            fieldBuilder.Append("\", ");
                            fieldBuilder.AppendFormat("actionParam.Get<{1}>(\"{0}\")",
                                ToFistWordCase(paramInfo.Field),
                                paramInfo.FieldType == FieldType.Password ? "string" : paramInfo.FieldType.ToString().ToLower());
                            fieldBuilder.Append(");");
                        }
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return ReplaceClientCshparCallback(content, responseParams);
        }
        /// <summary>
        /// 缩进字符
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="preIndent">上次缩进</param>
        /// <returns></returns>
        public static string GetSpaceIndent(int indent, int preIndent)
        {
            int num = (preIndent + indent) * IndentWordNum;
            return new String(' ', num > 0 ? num : 1);
        }

        protected static string ReplaceClientLuaCallback(string content, List<ParamInfoModel> paramList, bool isQuick = false)
        {

            string field = "##Judge##";
            int depth = 0;
            int preIndent = 2;
            int indent = 0;
            string subNumVar = "subRecordCount";
            string subTableVar = "subTable";
            string subRecordVar = "subRecord";
            string currTableVar = "DataTable";
            string readerVar = isQuick ? "rd" : "ZyReader";
            StringBuilder strTemp = new StringBuilder();
            int[] indexList = new int[forVarChars.Length];

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record) || fieldType.Equals(FieldType.SigleRecord))
                {
                    int recordIndex = 0;
                    if (depth < indexList.Length)
                    {
                        recordIndex = indexList[depth];
                        recordIndex++;
                        indexList[depth] = recordIndex;
                    }
                    if (depth > 0)
                    {
                        subNumVar = subNumVar + "_" + recordIndex;
                        subTableVar = subTableVar + "_" + recordIndex;
                        subRecordVar = subRecordVar + "_" + recordIndex;
                    }
                    currTableVar = subTableVar;
                    string currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}local {1} = {2}:getInt()", currIndent, subNumVar, readerVar);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}local {1} = {{}}", currIndent, currTableVar);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}if {1} ~= 0 then", currIndent, subNumVar);
                    strTemp.AppendLine();
                    indent++;
                    currIndent = GetSpaceIndent(depth + indent, preIndent);
                    var enumVar = depth < forVarChars.Length
                        ? forVarChars[depth]
                        : forVarChars[forVarChars.Length - 1];
                    strTemp.AppendFormat("{0}for {1}=1, {2} do", currIndent, enumVar, subNumVar);
                    strTemp.AppendLine();
                    currIndent = GetSpaceIndent(depth + indent + 1, preIndent);
                    strTemp.AppendFormat("{0}local {1} = {{}}", currIndent, subRecordVar);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}{1}:recordBegin()", currIndent, readerVar);
                    strTemp.AppendLine();

                    currTableVar = subRecordVar;
                    depth++;
                }
                else if (fieldType.Equals(FieldType.End))
                {

                    string currIndent = GetSpaceIndent(depth + indent, preIndent);

                    strTemp.AppendFormat("{0}{1}:recordEnd()", currIndent, readerVar);
                    strTemp.AppendLine();
                    if (isQuick)
                    {
                        strTemp.AppendFormat("{0}table.insert({1}, {2})", currIndent, subTableVar, subRecordVar);
                    }
                    else
                    {
                        strTemp.AppendFormat("{0}ZyTable.push_back({1}, {2})", currIndent, subTableVar, subRecordVar);
                    }
                    strTemp.AppendLine();

                    indent--;
                    currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendFormat("{0}end", currIndent);
                    strTemp.AppendLine();
                    currIndent = GetSpaceIndent(depth + indent - 1, preIndent);
                    strTemp.AppendFormat("{0}end", currIndent);
                    strTemp.AppendLine();

                    string tempTableVar = subTableVar;

                    indexList[depth] = 0;//子层级编号重置
                    depth--;
                    if (depth > 0)
                    {
                        subNumVar = subNumVar.Substring(0, subNumVar.LastIndexOf('_'));
                        subTableVar = subTableVar.Substring(0, subTableVar.LastIndexOf('_'));
                        subRecordVar = subRecordVar.Substring(0, subRecordVar.LastIndexOf('_'));
                    }
                    int recordIndex = depth < indexList.Length ? indexList[depth] : 0;
                    currTableVar = depth > 0 ? subRecordVar : "DataTable";
                    strTemp.AppendFormat("{0}{1}.Children{2} = {3}",
                        currIndent,
                        currTableVar,
                        "_" + recordIndex,
                        tempTableVar);
                    strTemp.AppendLine();
                }
                else if (fieldType.Equals(FieldType.Void))
                {
                    continue;
                }
                else
                {
                    string currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendFormat("{0}{1}.{2}", currIndent, currTableVar, paramInfo.Field);

                    switch (fieldType)
                    {
                        case FieldType.DateTime:
                        case FieldType.Long:
                        case FieldType.ULong:
                            strTemp.AppendFormat(" = {0}:getInt64()", readerVar);
                            break;
                        case FieldType.Int:
                            strTemp.AppendFormat(" = {0}:getInt()", readerVar);
                            break;
                        case FieldType.UShort:
                        case FieldType.Short:
                            strTemp.AppendFormat(" = {0}:getWORD()", readerVar);
                            break;
                        case FieldType.Password:
                        case FieldType.String:
                            strTemp.AppendFormat(" = {0}:readString()", readerVar);
                            break;
                        case FieldType.Bool:
                        case FieldType.Byte:
                            strTemp.AppendFormat(" = {0}:getByte()", readerVar);
                            break;
                        case FieldType.Float:
                            strTemp.AppendFormat(" = {0}:getFloat()", readerVar);
                            break;
                        case FieldType.Double:
                            strTemp.AppendFormat(" = {0}:getDouble()", readerVar);
                            break;
                        case FieldType.UInt:
                            strTemp.AppendFormat(" = {0}:getDWORD()", readerVar);
                            break;
                        default:
                            strTemp.AppendFormat(" = {0}:get{1}()", readerVar, fieldType.ToString());
                            break;
                    }
                    strTemp.AppendLine();
                }
            }
            content = content.Replace(field, strTemp.ToString());
            return content;
        }
        protected static string ReplaceClientCshparCallback(string content, List<ParamInfoModel> paramList)
        {
            string field = "##Judge##";
            int depth = 0;
            int preIndent = 2;
            int indent = 0;
            int recordIndex = 0;
            string subNumVar = "subRecordCount";
            string subTableVar = "subTable";
            string subRecordVar = "subRecord";
            string currTableVar = "actionResult";
            StringBuilder strTemp = new StringBuilder();
            int[] indexList = new int[forVarChars.Length];
            char enumVar = forVarChars[depth];
            //ArrayList RecordFieldList = new ArrayList(); 
            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record) || fieldType.Equals(FieldType.SigleRecord))
                {
                    if (depth < indexList.Length)
                    {
                        recordIndex = indexList[depth];
                        recordIndex++;
                        indexList[depth] = recordIndex;
                    }
                    if (depth > 0)
                    {
                        subNumVar = subNumVar + "_" + recordIndex;
                        subTableVar = subTableVar + "_" + recordIndex;
                        subRecordVar = subRecordVar + "_" + recordIndex;
                    }
                    currTableVar = subTableVar;
                    string currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}int {1} = reader.getInt();", currIndent, subNumVar);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}var {1} = new ActionResult[{2}];", currIndent, currTableVar, subNumVar);
                    strTemp.AppendLine();
                    currIndent = GetSpaceIndent(depth + indent, preIndent);
                    enumVar = depth < forVarChars.Length
                        ? forVarChars[depth]
                        : forVarChars[forVarChars.Length - 1];
                    strTemp.AppendFormat("{0}for(int {1} = 0; {1} < {2}; {1}++)", currIndent, enumVar, subNumVar);
                    strTemp.AppendLine("{");
                    currIndent = GetSpaceIndent(depth + indent + 1, preIndent);
                    strTemp.AppendFormat("{0}var {1} = new ActionResult();", currIndent, subRecordVar);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}reader.recordBegin();", currIndent);
                    strTemp.AppendLine();

                    currTableVar = subRecordVar;
                    //RecordFieldList.Add(paramInfo.Field.Length > 0 ? paramInfo.Field : string.Format("Children_{0}", recordIndex));
                    depth++;
                }
                else if (fieldType.Equals(FieldType.End))
                {
                    string currIndent = GetSpaceIndent(depth + indent, preIndent);

                    strTemp.AppendFormat("{0}reader.recordEnd();", currIndent);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}{1}[{3}] = {2};", currIndent, subTableVar, subRecordVar, enumVar);
                    strTemp.AppendLine();

                    depth--;
                    //string RecordField = RecordFieldList[depth] as string;
                    currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendFormat("{0}", currIndent);
                    strTemp.AppendLine("}");

                    string tempTableVar = subTableVar;

                    if (depth > 0)
                    {
                        subNumVar = subNumVar.Substring(0, subNumVar.LastIndexOf('_'));
                        subTableVar = subTableVar.Substring(0, subTableVar.LastIndexOf('_'));
                        subRecordVar = subRecordVar.Substring(0, subRecordVar.LastIndexOf('_'));
                    }
                    currTableVar = depth > 0 ? subTableVar : "actionResult";
                    strTemp.AppendFormat("{0}{1}[\"Children{2}\"] = {3};",
                         currIndent,
                         currTableVar,
                         "_" + recordIndex,
                         tempTableVar);
                    strTemp.AppendLine();
                }
                else if (fieldType.Equals(FieldType.Void))
                {
                    continue;
                }
                else
                {
                    string currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendFormat("{0}{1}[\"{2}\"]", currIndent, currTableVar, paramInfo.Field);

                    switch (fieldType)
                    {
                        case FieldType.Void:
                            break;
                        case FieldType.Int:
                            strTemp.Append(" = reader.getInt();");
                            break;
                        case FieldType.Short:
                            strTemp.Append(" = reader.getShort();");
                            break;
                        case FieldType.Password:
                        case FieldType.String:
                            strTemp.Append(" = reader.readString();");
                            break;
                        case FieldType.Bool:
                            strTemp.Append(" = reader.getByte() == 0 ? false : true;");
                            break;
                        case FieldType.Byte:
                            strTemp.Append(" = reader.getByte();");
                            break;
                        default:
                            strTemp.Append(" = reader.get" + fieldType + "();");
                            break;
                    }
                    strTemp.AppendLine();
                }
            }
            content = content.Replace(field, strTemp.ToString());
            return content;
        }

        public static string FormatActionDefineTemp(string content, List<ContractModel> contractList, SolutionModel slnRecord)
        {
            string[] expressList = new string[] { "##FieldList##", "##Namespace##", "##RefNamespace##" };

            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));

                if (fieldBuilder.ToString() == "Namespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord.Namespace);
                }
                else if (fieldBuilder.ToString() == "RefNamespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord.RefNamespace);
                }
                else if (fieldBuilder.ToString() == "FieldList")
                {
                    string currIndent = GetSpaceIndent(2, 0);
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    foreach (var contract in contractList)
                    {
                        fieldBuilder.Append("///<summary>");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(currIndent);

                        fieldBuilder.AppendFormat("///{0}", contract.Descption);
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(currIndent);

                        fieldBuilder.Append("///</summary>");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(currIndent);

                        fieldBuilder.AppendFormat("public const Int16 Cst_Action{0} = {0};", contract.ID);
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(currIndent);
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());
            }
            return content;
        }

        internal static string FormatPython(string content, List<ParamInfoModel> paramList, List<ParamInfoModel> reqParams, SolutionModel slnRecord, string title)
        {
            string[] expressList = new string[] { "##Description##", "##UrlParam##", "##getUrlElement##", "##actionResult##", "##buildPacket##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                switch (exp)
                {
                    case "##Description##":
                        fieldBuilder.Append(title);
                        break;
                    case "##UrlParam##":
                        {
                            string currIndent = GetSpaceIndent(2, 0);
                            foreach (var paramInfo in paramList)
                            {
                                FieldType fieldType = paramInfo.FieldType;
                                if (FieldType.Record.Equals(fieldType) || 
                                    FieldType.SigleRecord.Equals(fieldType) ||
                                    FieldType.End.Equals(fieldType))
                                {
                                    continue;

                                }
                                if (paramInfo.ParamType == 1)
                                {
                                    fieldBuilder.Append(currIndent);
                                    if (fieldType == FieldType.Byte
                                   || fieldType == FieldType.Int
                                   || fieldType == FieldType.Short
                                   || fieldType == FieldType.Long
                                   || fieldType == FieldType.UInt
                                   || fieldType == FieldType.UShort
                                   || fieldType == FieldType.ULong
                                   )
                                    {
                                        fieldBuilder.Append(string.Format("self.{0} = 0", ToMemberVarName(paramInfo.Field)));
                                    }
                                    else if (fieldType == FieldType.String || fieldType == FieldType.Password)
                                    {
                                        fieldBuilder.Append(string.Format("self.{0} = ''", ToMemberVarName(paramInfo.Field)));
                                    }
                                    fieldBuilder.Append(Environment.NewLine);
                                }

                            }
                        }
                        break;

                    case "##getUrlElement##":
                        ReplacePythonCheckRequest(fieldBuilder, reqParams);
                        break;
                    case "##actionResult##":
                        ReplacePythonAction(fieldBuilder, paramList);
                        break;
                    case "##buildPacket##":
                        ReplacePythonBuildPacket(fieldBuilder, paramList);
                        break;
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return content;
        }

        private static void ReplacePythonAction(StringBuilder builder, List<ParamInfoModel> paramList)
        {
            string currentVar = "self";
            string listVar = "dsItemList";
            int recordIndex = 0, depth = 0;
            int[] indexList = new int[forVarChars.Length];
            string currIndent = GetSpaceIndent(2, 0);

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                string fieldValue = ToMemberVarName(paramInfo.Field);
                if (fieldType.Equals(FieldType.Record) || fieldType.Equals(FieldType.SigleRecord))
                {
                    if (depth < indexList.Length)
                    {
                        recordIndex = indexList[depth];
                        recordIndex++;
                        indexList[depth] = recordIndex;
                    }
                    listVar = listVar + "_" + recordIndex;
                    depth++;
                    builder.Append(currIndent);
                    builder.AppendFormat("{0}.{1} = {2}", currentVar, (fieldValue.Length > 0 ? paramInfo.Field : listVar), "None");
                    builder.Append(Environment.NewLine);
                }
                else if (fieldType.Equals(FieldType.End))
                {
                    listVar = listVar.Substring(0, listVar.LastIndexOf('_'));
                    depth--;
                }
                else if (fieldType.Equals(FieldType.Void))
                {
                }
                else if (depth == 0)
                {
                    string proValue = "''";
                    if (fieldType == FieldType.Byte ||
                        fieldType == FieldType.Short ||
                        fieldType == FieldType.Int ||
                        fieldType == FieldType.Long ||
                        fieldType == FieldType.UShort ||
                        fieldType == FieldType.UInt ||
                        fieldType == FieldType.ULong)
                    {
                        proValue = "0";
                    }
                    builder.Append(currIndent);
                    builder.AppendFormat("{0}.{1} = {2}", currentVar, fieldValue, proValue);
                    builder.Append(Environment.NewLine);
                }
            }
        }

        protected static void ReplacePythonCheckRequest(StringBuilder stMust, List<ParamInfoModel> reqParams)
        {
            string currIndent = GetSpaceIndent(2, 0);
            string parentIndent = GetSpaceIndent(1, 0);
            StringBuilder stNotMust = new StringBuilder();
            foreach (var paramInfo in reqParams)
            {
                if (paramInfo.ParamType == 1)
                {
                    FieldType fieldType = paramInfo.FieldType;
                    int minValue = paramInfo.MinValue;
                    int maxValue = paramInfo.MaxValue;
                    stNotMust.Append(currIndent);
                    string fieldName = paramInfo.Field;
                    string varName = ToMemberVarName(fieldName);
                    switch (fieldType)
                    {
                        case FieldType.ULong:
                        case FieldType.Long:
                        case FieldType.UInt:
                        case FieldType.Int:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetIntValue(\"{1}\")", varName, fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetIntValue(\"{1}\"{2})", varName, fieldName, minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.UShort:
                        case FieldType.Short:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetWordValue(\"{1}\")", varName, fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetWordValue(\"{1}\"{2})", varName, fieldName, minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.Byte:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetByteValue(\"{1}\")", varName, fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetByteValue(\"{1}\"{2})", varName, fieldName, minandMaxValue);
                                }
                            }
                            break;
                        default:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{1}\")", varName,
                                        fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{1}\"{2})", varName,
                                        fieldName, minandMaxValue);
                                }
                            }
                            break;
                    }
                    stNotMust.Append(Environment.NewLine);

                    if (paramInfo.Required)
                    {
                        if (stMust.ToString().Length == 0)
                        {
                            stMust.Append(parentIndent);
                            stMust.AppendFormat("if httpGet.Contains(\"{0}\")", fieldName);
                        }
                        else
                        {
                            stMust.Append("\\");
                            stMust.Append(Environment.NewLine);
                            stMust.Append(parentIndent);
                            stMust.AppendFormat("  and httpGet.Contains(\"{0}\")", fieldName);
                        }
                    }
                }

            }
            if (stMust.ToString().Length > 0)
            {
                stMust.Append(":");
                stMust.Append(Environment.NewLine);
            }
            else if (reqParams.Count > 0)
            {
                stMust.Append(parentIndent);
                stMust.Append("if True:");
                stMust.Append(Environment.NewLine);
                stMust.Append(currIndent);
                stMust.Append("urlParam.Result = True");
                stMust.Append(Environment.NewLine);
            }
            if (reqParams.Count > 0)
            {
                stMust.Append(stNotMust);
                stMust.Append(parentIndent);
                stMust.Append("else:\n");
                stMust.Append(currIndent);
                stMust.Append("urlParam.Result = False");
                stMust.Append(Environment.NewLine);
            }
            else
            {
                stMust.Append(parentIndent);
                stMust.Append("urlParam.Result = True");
            }
        }

        protected static void ReplacePythonBuildPacket(StringBuilder strTemp, List<ParamInfoModel> paramList)
        {
            int depth = 0;
            string currentVar = "actionResult";
            string itemVar = "dsItem";
            string preItemVar = "writer";
            string enumVar = "info";
            string listVar = "dsItemList";
            int recordIndex = 0;
            int[] indexList = new int[forVarChars.Length];

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                string fieldValue = paramInfo.Field;
                if (fieldType.Equals(FieldType.Record) || fieldType.Equals(FieldType.SigleRecord))
                {
                    if (depth < indexList.Length)
                    {
                        recordIndex = indexList[depth];
                        recordIndex++;
                        indexList[depth] = recordIndex;
                    }
                    if (depth > 0)
                    {
                        preItemVar = itemVar;
                        currentVar = itemVar;
                        itemVar = itemVar + recordIndex;
                        enumVar = enumVar + "_" + recordIndex;
                    }
                    //RecordFieldList.Add(fieldValue.Length > 0 ? paramInfo.Field : string.Format("Children_{0}", recordIndex));
                    listVar = listVar + "_" + recordIndex;
                    string varString = (fieldValue.Length > 0 ? paramInfo.Field : listVar);
                    depth++;
                    string currIndent = GetSpaceIndent(depth, 0);
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("{0}.PushIntoStack(len(actionResult.{1}))", preItemVar, varString);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("for {0} in actionResult.{1}:", enumVar, varString);
                    strTemp.AppendLine();
                    strTemp.Append(GetSpaceIndent(depth + 1, 0));
                    strTemp.AppendFormat("{0} = DataStruct()", itemVar);
                    strTemp.AppendLine();
                    continue;
                }
                if (fieldType.Equals(FieldType.End))
                {
                    listVar = listVar.Substring(0, listVar.LastIndexOf('_'));

                    strTemp.Append(GetSpaceIndent(depth + 1, 0));
                    strTemp.AppendFormat("{0}.PushIntoStack({1})", preItemVar, itemVar);
                    strTemp.AppendLine();
                    strTemp.AppendLine();
                    depth--;
                    if (depth > 0)
                    {
                        enumVar = enumVar.Substring(0, enumVar.LastIndexOf('_'));
                        itemVar = currentVar;
                        if (currentVar.Length == 6)
                        {
                            currentVar = "actionResult";
                        }
                        else
                        {
                            currentVar = currentVar.Substring(0, itemVar.Length - 1);
                        }
                        if (itemVar == "dsItem")
                        {
                            preItemVar = "writer";
                        }
                    }
                    else
                    {
                        enumVar = "info";
                    }
                    continue;
                }
                if (fieldType.Equals(FieldType.Void))
                {
                    continue;
                }

                strTemp.Append(GetSpaceIndent(depth + 1, 0));
                string putMethod = "PushIntoStack";
                if (fieldType == FieldType.Byte)
                {
                    putMethod = "PushByteIntoStack";
                }
                else if (fieldType == FieldType.Short)
                {
                    putMethod = "PushShortIntoStack";
                }
                if (depth == 0)
                {
                    strTemp.AppendFormat("writer.{0}({1}.{2})", putMethod, currentVar, ToMemberVarName(fieldValue));
                }
                else
                {
                    strTemp.AppendFormat("{0}.{1}({2}.{3})", itemVar, putMethod, enumVar, fieldValue);
                }
                strTemp.AppendLine();
            }
        }

        private static string PythonGetUrlElement(string field, string key)
        {
            return string.Format("{1}if httpGet.Contains(\"{0}\"):\n{1}{1}urlParam.{0} = httpGet.Get{2}(\"{0}\")\n{1}else:\n{1}{1}urlParam.Result = False\n{1}{1}return urlParam", field, GetSpaceIndent(4, 0), key);
        }


        public static string FormatLua(string content, int contractId, List<ParamInfoModel> responseParams, List<ParamInfoModel> requestParams, SolutionModel slnRecord, string title)
        {
            string[] expressList = new string[] { "##ID##", "##Description##", "##getUrlElement##", "##buildPacket##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                switch (exp)
                {
                    case "##ID##":
                        fieldBuilder.Append(contractId);
                        break;
                    case "##Description##":
                        fieldBuilder.Append(title);
                        break;
                    case "##getUrlElement##":
                        ReplaceLuaCheckRequest(fieldBuilder, requestParams);
                        break;
                    case "##buildPacket##":
                        ReplaceLuaBuildPacket(fieldBuilder, responseParams);
                        break;
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return content;
        }

        protected static void ReplaceLuaCheckRequest(StringBuilder stMust, List<ParamInfoModel> reqParams)
        {
            string currIndent = GetSpaceIndent(2, 0);
            string parentIndent = GetSpaceIndent(1, 0);
            StringBuilder stNotMust = new StringBuilder();
            foreach (var paramInfo in reqParams)
            {
                if (paramInfo.ParamType == 1)
                {
                    FieldType fieldType = paramInfo.FieldType;
                    stNotMust.Append(currIndent);
                    string fieldName = paramInfo.Field;
                    string varName = ToMemberVarName(fieldName);
                    switch (fieldType)
                    {
                        case FieldType.Int:
                            stNotMust.AppendFormat("urlParam.{0} = ReadNumberParam(actionGetter, \"{1}\")", varName, fieldName);
                            break;
                        default:
                            stNotMust.AppendFormat("urlParam.{0} = ReadStringParam(actionGetter, \"{1}\")", varName, fieldName);
                            break;
                    }
                    stNotMust.Append(Environment.NewLine);

                    if (paramInfo.Required)
                    {
                        if (stMust.ToString().Length == 0)
                        {
                            stMust.Append(parentIndent);
                            stMust.AppendFormat("if ContainsParam(actionGetter, \"{0}\")", fieldName);
                        }
                        else
                        {
                            stMust.Append(Environment.NewLine);
                            stMust.Append(parentIndent);
                            stMust.AppendFormat("  and ContainsParam(actionGetter, \"{0}\")", fieldName);
                        }
                    }
                }

            }
            if (stMust.ToString().Length > 0)
            {
                stMust.AppendLine(" then");
            }
            else if (reqParams.Count > 0)
            {
                stMust.Append(parentIndent);
                stMust.AppendLine("if true then");
                stMust.Append(currIndent);
                stMust.AppendLine("urlParam.Result = true");
            }
            if (reqParams.Count > 0)
            {
                stMust.Append(stNotMust);
                stMust.Append(parentIndent);
                stMust.AppendLine("else");
                stMust.Append(currIndent);
                stMust.AppendLine("urlParam.Result = false");
                stMust.Append(parentIndent);
                stMust.AppendLine("end");
            }
            else
            {
                stMust.Append(parentIndent);
                stMust.Append("urlParam.Result = true");
            }
        }


        protected static void ReplaceLuaBuildPacket(StringBuilder strTemp, List<ParamInfoModel> paramList)
        {
            int depth = 0;
            string currentVar = "actionResult";
            string itemVar = "dsItem";
            string preItemVar = "writer";
            string enumVar = "info";
            string listVar = "dsItemList";
            int recordIndex = 0;
            int[] indexList = new int[forVarChars.Length];

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                string fieldValue = paramInfo.Field;
                if (fieldType.Equals(FieldType.Record) || fieldType.Equals(FieldType.SigleRecord))
                {
                    if (depth < indexList.Length)
                    {
                        recordIndex = indexList[depth];
                        recordIndex++;
                        indexList[depth] = recordIndex;
                    }
                    if (depth > 0)
                    {
                        preItemVar = itemVar;
                        currentVar = itemVar;
                        itemVar = itemVar + recordIndex;
                        enumVar = enumVar + "_" + recordIndex;
                    }
                    listVar = listVar + "_" + recordIndex;
                    string varString = (fieldValue.Length > 0 ? paramInfo.Field : listVar);
                    depth++;
                    string currIndent = GetSpaceIndent(depth, 0);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("PushLenIntoStack({0}, actionResult.{1})", preItemVar, varString);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("local len = {0}.{1}.Count", preItemVar, varString);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("for {0}=1, len, 1 do", enumVar);
                    strTemp.AppendLine();
                    strTemp.Append(GetSpaceIndent(depth + 1, 0));
                    strTemp.AppendFormat("local {0} = CreateDataStruct()", itemVar);
                    strTemp.AppendLine();
                    continue;
                }
                if (fieldType.Equals(FieldType.End))
                {
                    listVar = listVar.Substring(0, listVar.LastIndexOf('_'));

                    strTemp.Append(GetSpaceIndent(depth + 1, 0));
                    strTemp.AppendFormat("PushIntoStack({0}, {1})", preItemVar, itemVar);
                    strTemp.AppendLine();
                    strTemp.Append(GetSpaceIndent(depth, 0));
                    strTemp.AppendLine("end");
                    depth--;
                    if (depth > 0)
                    {
                        enumVar = enumVar.Substring(0, enumVar.LastIndexOf('_'));
                        itemVar = currentVar;
                        if (currentVar.Length == 6)
                        {
                            currentVar = "actionResult";
                        }
                        else
                        {
                            currentVar = currentVar.Substring(0, itemVar.Length - 1);
                        }
                        if (itemVar == "dsItem")
                        {
                            preItemVar = "writer";
                        }
                    }
                    else
                    {
                        enumVar = "info";
                    }
                    continue;
                }
                if (fieldType.Equals(FieldType.Void))
                {
                    continue;
                }

                strTemp.Append(GetSpaceIndent(depth + 1, 0));
                string putMethod = "PushIntoStack";
                if (depth == 0)
                {
                    strTemp.AppendFormat("{0}(writer, {1}.{2})", putMethod, currentVar, ToMemberVarName(fieldValue));
                }
                else
                {
                    strTemp.AppendFormat("{1}({0}, {2}.{3})", itemVar, putMethod, enumVar, fieldValue);
                }
                strTemp.AppendLine();
            }
        }

        /// <summary>
        /// 赋值模板
        /// </summary>
        /// <param name="content"></param>
        /// <param name="paramList"></param>
        /// <param name="slnRecord"></param>
        /// <param name="title"></param>
        /// <param name="reqParams"></param>
        /// <returns></returns>
        public static string FormatTemp(string content, int contractId, List<ParamInfoModel> paramList, List<ParamInfoModel> reqParams, SolutionModel slnRecord, string title)
        {
            string[] expressList = new string[] { "##ID##", "##Description##", "##Field##", "##Namespace##", "##RefNamespace##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));

                if (fieldBuilder.ToString() == "ID")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(contractId);

                }
                else if (fieldBuilder.ToString() == "Description")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(title);
                }
                else if (fieldBuilder.ToString() == "Namespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord.Namespace);
                }
                else if (fieldBuilder.ToString() == "RefNamespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord.RefNamespace);
                }

                else if (fieldBuilder.ToString() == "Field")
                {
                    int depth = 0;
                    string listVar = "_dsItemList";
                    string className;
                    int recordIndex = 0;
                    int[] indexList = new int[forVarChars.Length];
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    var list = new List<ParamInfoModel>(reqParams.Where(t => t.FieldType != FieldType.Void));
                    list.AddRange(paramList.Where(t => t.FieldType != FieldType.Void));

                    var classList = new List<StringBuilder>();
                    var classStack = new Stack<StringBuilder>();
                    StringBuilder respPacketBuilder = new StringBuilder();
                    BuildClassCode(respPacketBuilder, GetSpaceIndent(2, 0), "Main Body", "ResponsePacket");
                    respPacketBuilder.Append(GetSpaceIndent(1, 0));
                    StringBuilder memberBuilder = new StringBuilder();

                    foreach (var paramInfo in list)
                    {
                        #region item
                        string descp = paramInfo.Descption + paramInfo.Remark;
                        string spaceString = GetSpaceIndent(2, depth + 1);
                        FieldType fieldType = paramInfo.FieldType;
                        if (fieldType == FieldType.Void) continue;

                        if (FieldType.Record.Equals(fieldType) || FieldType.SigleRecord.Equals(fieldType))
                        {
                            if (depth < indexList.Length)
                            {
                                recordIndex = indexList[depth];
                                recordIndex++;
                                indexList[depth] = recordIndex;
                            }
                            string memberName = string.IsNullOrEmpty(paramInfo.Field) ? listVar + "_" + recordIndex : paramInfo.Field + "List";
                            listVar = listVar + "_" + recordIndex;
                            className = string.IsNullOrEmpty(paramInfo.Field)
                                ? listVar.Replace("_dsItemList", "Class")
                                : paramInfo.Field;
                            //add class's member
                            if (depth > 0)
                            {
                                var classMemberBuilder = classStack.Peek();
                                BuildMemberCodeByList(classMemberBuilder, GetSpaceIndent(2, depth), className, descp, memberName, true);
                            }
                            else
                            {
                                BuildMemberCodeByList(respPacketBuilder, spaceString, className, descp, memberName, true);
                            }
                            var classBuilder = new StringBuilder();
                            BuildClassCode(classBuilder, GetSpaceIndent(2, 0), descp, className);
                            classBuilder.Append(GetSpaceIndent(1, 0));
                            classStack.Push(classBuilder);
                            depth++;
                            continue;
                        }
                        if (FieldType.End.Equals(fieldType))
                        {
                            listVar = listVar.Substring(0, listVar.LastIndexOf('_'));

                            var classMemberBuilder = classStack.Pop();
                            classMemberBuilder.AppendLine("");
                            classMemberBuilder.Append(GetSpaceIndent(2, 0));
                            classMemberBuilder.AppendLine("}");
                            classList.Add(classMemberBuilder);
                            depth--;
                            continue;
                        }
                        if (depth > 0)
                        {
                            var classMemberBuilder = classStack.Peek();
                            BuildMemberCode(classMemberBuilder, GetSpaceIndent(2, 1), paramInfo, true);
                            continue;
                        }
                        //放到主结构体
                        if (paramInfo.ParamType == 2)
                        {
                            BuildMemberCode(respPacketBuilder, spaceString, paramInfo, true);
                        }
                        else
                        {
                            BuildMemberCode(memberBuilder, GetSpaceIndent(2, depth), paramInfo);
                        }

                        #endregion
                    }
                    string space = GetSpaceIndent(2, 0);
                    fieldBuilder.AppendLine();
                    fieldBuilder.Append(space);
                    fieldBuilder.AppendLine("#region class object");
                    foreach (var builder in classList)
                    {
                        fieldBuilder.Append(builder);
                        fieldBuilder.AppendLine();
                    }
                    fieldBuilder.Append(respPacketBuilder);
                    fieldBuilder.AppendLine();
                    fieldBuilder.Append(space);
                    fieldBuilder.AppendLine("}");
                    fieldBuilder.Append(space);
                    fieldBuilder.AppendLine("#endregion");
                    fieldBuilder.AppendLine();
                    fieldBuilder.Append(space);
                    fieldBuilder.AppendLine("/// <summary>");
                    fieldBuilder.Append(space);
                    fieldBuilder.AppendLine("/// 响应数据包");
                    fieldBuilder.Append(space);
                    fieldBuilder.AppendLine("/// </summary>");
                    fieldBuilder.Append(space);
                    fieldBuilder.AppendFormat("private ResponsePacket _packet = new ResponsePacket();");
                    fieldBuilder.AppendLine();
                    fieldBuilder.Append(space);
                    fieldBuilder.Append(memberBuilder);
                    fieldBuilder.Append(space);
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            content = ReplaceCheckRequest(content, reqParams);
            content = ReplaceBuildPack(content, paramList);
            return content;
        }

        private static void BuildClassCode(StringBuilder classBuilder, string spaceString, string descp, string className)
        {
            classBuilder.Append(spaceString);
            classBuilder.AppendLine("/// <summary>");
            classBuilder.Append(spaceString);
            classBuilder.AppendLine("/// " + descp.Replace("\r\n", ","));
            classBuilder.Append(spaceString);
            classBuilder.AppendLine("/// </summary>");
            classBuilder.Append(spaceString);
            classBuilder.AppendFormat("class {0}", className);
            classBuilder.AppendLine("");
            classBuilder.Append(spaceString);
            classBuilder.AppendLine("{");
            classBuilder.Append(spaceString);
        }

        private static void BuildMemberCodeByList(StringBuilder memberBuilder, string spaceString, string className, string descp, string listVar, bool isClassMember = false)
        {
            memberBuilder.AppendLine("/// <summary>");
            memberBuilder.Append(spaceString);
            memberBuilder.AppendLine("/// " + descp.Replace("\r\n", ","));
            memberBuilder.Append(spaceString);
            memberBuilder.AppendLine("/// </summary>");
            memberBuilder.Append(spaceString);
            if (isClassMember)
            {
                memberBuilder.AppendFormat("public List<{1}> {0}", listVar, className);
                memberBuilder.AppendLine(" { get; set; }");
            }
            else
            {
                memberBuilder.AppendFormat("private List<{1}> {0} = new List<{1}>()", listVar, className);
                memberBuilder.AppendLine(";");
            }
            memberBuilder.Append(spaceString);
        }

        private static void BuildMemberCode(StringBuilder memberBuilder, string spaceString, ParamInfoModel paramInfo, bool isClassMember = false)
        {
            string descp = paramInfo.Descption + paramInfo.Remark;
            memberBuilder.AppendLine("/// <summary>");
            memberBuilder.Append(spaceString);
            memberBuilder.AppendLine("/// " + descp.Replace("\r\n", ","));
            memberBuilder.Append(spaceString);
            memberBuilder.AppendLine("/// </summary>");
            memberBuilder.Append(spaceString);
            if (isClassMember)
            {
                memberBuilder.AppendFormat("public {0} {1}",
                    paramInfo.FieldType == FieldType.Password ? "string" : paramInfo.FieldType.ToString().ToLower(),
                    paramInfo.Field);
                memberBuilder.AppendLine(" { get; set; }");
            }
            else
            {
                memberBuilder.AppendFormat("private {0} {1}",
                    paramInfo.FieldType == FieldType.Password ? "string" : paramInfo.FieldType.ToString().ToLower(),
                    ToMemberVarName(paramInfo.Field));
                memberBuilder.AppendLine(";");
            }
            memberBuilder.Append(spaceString);
        }

        private static string SetValueRange(int minValue, int maxValue)
        {
            if (minValue >= 0 && maxValue > 0)
            {
                return string.Format(", {0}, {1} ", minValue, maxValue);
            }
            return string.Empty;
        }

        protected static string ReplaceCheckRequest(string content, List<ParamInfoModel> reqParams)
        {
            string field = "##Judge##";
            StringBuilder stMust = new StringBuilder();
            StringBuilder stNotMust = new StringBuilder();

            int indent = 1;
            string currIndent = GetSpaceIndent(indent, 2);
            bool hasRequired = reqParams.Exists(t => t.Required);
            string subIndent = GetSpaceIndent(hasRequired ? indent + 1 : indent, 2);

            foreach (var paramInfo in reqParams)
            {

                StringBuilder strTemp = new StringBuilder();
                if (paramInfo.ParamType == 1)
                {
                    FieldType fieldType = paramInfo.FieldType;
                    int minValue = paramInfo.MinValue;
                    int maxValue = paramInfo.MaxValue;
                    string fieldname = paramInfo.Field;
                    string varName = ToMemberVarName(fieldname);
                    switch (fieldType)
                    {
                        case FieldType.ULong:
                        case FieldType.Long:
                            strTemp.Append("httpGet.GetLong(\"");
                            break;
                        case FieldType.UInt:
                        case FieldType.Int:
                            strTemp.Append("httpGet.GetInt(\"");
                            break;
                        case FieldType.UShort:
                        case FieldType.Short:
                            strTemp.Append("httpGet.GetWord(\"");
                            break;
                        case FieldType.Byte:
                            strTemp.Append("httpGet.GetByte(\"");
                            break;
                        case FieldType.Bool:
                            strTemp.Append("httpGet.GetBool(\"");
                            break;
                        default:
                            strTemp.Append("httpGet.GetString(\"");
                            break;
                    }
                    strTemp.Append(fieldname);
                    strTemp.Append("\", ref ");
                    strTemp.Append(varName);
                    strTemp.Append(SetValueRange(minValue, maxValue));
                    strTemp.Append(")");

                    if (paramInfo.Required)
                    {
                        stMust.Append(currIndent);
                        if (stMust.ToString().Trim() == "")
                        {
                            stMust.Append("if (");
                        }
                        else
                        {
                            stMust.Append(Environment.NewLine);
                            stMust.Append(subIndent);
                            stMust.Append("&& ");
                        }
                        stMust.Append(strTemp);

                    }
                    else
                    {
                        stNotMust.Append(subIndent);
                        stNotMust.Append(strTemp).Append(";");
                        stNotMust.Append(Environment.NewLine);
                    }
                }
            }
            if (stMust.ToString().Length > 0)
            {
                stMust.Append(")");
                stMust.Append(Environment.NewLine);
                stMust.Append(currIndent);
                stMust.Append("{");
                stMust.Append(Environment.NewLine);
            }
            //else if (reqParams.Count > 0)
            //{
            //    stMust.Append(currIndent);
            //    stMust.Append("if (true)");
            //    stMust.Append(Environment.NewLine);
            //    stMust.Append(currIndent);
            //    stMust.Append("{");
            //    stMust.Append(Environment.NewLine);
            //}
            if (!hasRequired)
            {
                stMust.Append(stNotMust);
                stMust.Append(subIndent);
                stMust.Append("return true;");
            }
            else if (reqParams.Count > 0)
            {
                stMust.Append(stNotMust);
                stMust.Append(subIndent);
                stMust.Append("return true;");
                stMust.Append(Environment.NewLine);
                stMust.Append(currIndent);
                stMust.Append("}");
                stMust.Append(Environment.NewLine);
                stMust.Append(currIndent);
                stMust.Append("return false;");
            }
            else
            {
                stMust.Append(currIndent);
                stMust.Append("return true;");
            }
            return content.Replace(field, stMust.ToString());
        }

        protected static string ReplaceBuildPack(string content, List<ParamInfoModel> paramList)
        {
            string field = "##johc##";
            StringBuilder strTemp = new StringBuilder();
            int depth = 0;
            int indent = 1;
            string parentEnumVar = "";
            string currentVar = "this";
            string itemVar = "dsItem";
            string enumVar = "item";
            string listVar = "_dsItemList";
            string parentVar = "_packet";
            int recordIndex = 0;
            int[] indexList = new int[forVarChars.Length];

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record) || fieldType.Equals(FieldType.SigleRecord))
                {
                    if (depth < indexList.Length)
                    {
                        recordIndex = indexList[depth];
                        recordIndex++;
                        indexList[depth] = recordIndex;
                    }
                    parentEnumVar = "";
                    if (depth > 0)
                    {
                        parentEnumVar = enumVar + ".";
                        currentVar = itemVar;
                        itemVar = itemVar + recordIndex;
                        enumVar = enumVar + "_" + recordIndex;
                    }
                    listVar = listVar + "_" + recordIndex;
                    string memberName = string.IsNullOrEmpty(paramInfo.Field) ? listVar : paramInfo.Field + "List";
                    if (depth == 0)
                    {
                        memberName = string.Format("{0}.{1}", parentVar, memberName);
                    }
                    string currIndent = GetSpaceIndent(indent + depth, 2);
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("{0}.PushIntoStack({1}{2}.Count);", currentVar, parentEnumVar, memberName);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("foreach (var {0} in {1}{2})", enumVar, parentEnumVar, memberName);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.Append("{");
                    strTemp.AppendLine();
                    currIndent = GetSpaceIndent(indent + depth + 1, 2);
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("DataStruct {0} = new DataStruct();", itemVar);
                    strTemp.AppendLine();

                    depth++;
                }
                else if (fieldType.Equals(FieldType.End))
                {
                    listVar = listVar.Substring(0, listVar.LastIndexOf('_'));
                    string currIndent = GetSpaceIndent(indent + depth, 2);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("{0}.PushIntoStack({1});", currentVar, itemVar);
                    strTemp.AppendLine();
                    currIndent = GetSpaceIndent(indent + depth - 1, 2);
                    strTemp.Append(currIndent);
                    strTemp.Append("}");
                    strTemp.AppendLine();

                    depth--;
                    if (depth > 0)
                    {
                        enumVar = enumVar.Substring(0, enumVar.LastIndexOf('_'));
                        itemVar = currentVar;
                        if (currentVar.Length == 6)
                        {
                            currentVar = "this";
                        }
                        else
                        {
                            currentVar = currentVar.Substring(0, itemVar.Length - 1);
                        }
                    }
                    else
                    {
                        enumVar = "item";
                    }
                }
                else if (fieldType.Equals(FieldType.Void))
                {
                }
                else
                {
                    string memberName = string.Format("{0}.{1}", depth > 0 ? enumVar : parentVar, paramInfo.Field);
                    string currIndent = GetSpaceIndent(indent + depth, 2);
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("{0}.PushIntoStack({1}", depth == 0 ? currentVar : itemVar, FormatFieldType(fieldType));
                    strTemp.Append(memberName);
                    strTemp.Append(");");
                    strTemp.AppendLine();
                }


            }
            return content.Replace(field, strTemp.ToString());
        }

        private static string FormatFieldType(FieldType fieldType)
        {
            if (fieldType == FieldType.Short || fieldType == FieldType.Byte)
            {
                return string.Format("({0})", fieldType.ToString().ToLower());
            }
            return "";
        }

        internal static string GetExampleUrl(List<ParamInfoModel> paramList, string contractID)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.AppendFormat("ActionID={0}", contractID);
            foreach (var paramInfo in paramList)
            {

                FieldType fieldType = paramInfo.FieldType;
                int paramType = paramInfo.ParamType;
                bool required = paramInfo.Required;
                if (FieldType.Record.Equals(fieldType) || 
                    FieldType.SigleRecord.Equals(fieldType) ||
                    FieldType.End.Equals(fieldType) ||
                    paramType != 1 ||
                    !required)
                {
                    continue;

                }
                if (urlBuilder.Length > 0)
                {
                    urlBuilder.Append("&");
                }
                urlBuilder.AppendFormat("{0}={1}", paramInfo.Field, paramInfo.FieldValue);
            }
            return urlBuilder.ToString();
        }


        public static List<ParamInfoModel> InitParamDepth(List<ParamInfoModel> paramList)
        {
            int depth = 0;
            foreach (var paramInfo in paramList)
            {
                if (paramInfo.ParamType != 2) continue;

                paramInfo.Depth = depth;
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record) || fieldType.Equals(FieldType.SigleRecord))
                {
                    paramInfo.Depth = depth;
                    depth++;
                    continue;
                }
                if (fieldType.Equals(FieldType.End))
                {
                    depth--;
                    paramInfo.Depth = depth;
                    continue;
                }
                if (fieldType.Equals(FieldType.Void))
                {
                    continue;
                }
            }
            return paramList;
        }
    }
}