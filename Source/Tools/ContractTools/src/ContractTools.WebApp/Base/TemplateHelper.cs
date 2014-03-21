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
                            fieldBuilder.Append("ZyWriter:writeString(\"");
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

        /// <summary>
        /// 缩进字符
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="preIndent"></param>
        /// <returns></returns>
        public static string GetSpaceIndent(int indent, int preIndent)
        {
            int num = (preIndent + indent) * IndentWordNum;
            return new String(' ', num);
        }

        protected static string ReplaceClientLuaCallback(string content, List<ParamInfoModel> paramList)
        {
            string field = "##Judge##";
            int depth = 0;
            int preIndent = 2;
            int indent = 0;
            int recordIndex = 0;
            string subNumVar = "subRecordCount";
            string subTableVar = "subTabel";
            string subRecordVar = "subRecord";
            string currTableVar = subTableVar;
            StringBuilder strTemp = new StringBuilder();
            int[] indexList = new int[forVarChars.Length];

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record))
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
                    strTemp.AppendFormat("{0}local {1} = ZyReader:getInt()", currIndent, subNumVar);
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
                    strTemp.AppendFormat("{0}ZyReader:recordBegin()", currIndent);
                    strTemp.AppendLine();

                    currTableVar = subRecordVar;
                    depth++;
                }
                else if (fieldType.Equals(FieldType.End))
                {
                    string currIndent = GetSpaceIndent(depth + indent, preIndent);

                    strTemp.AppendFormat("{0}ZyReader:recordEnd()", currIndent);
                    strTemp.AppendLine();
                    strTemp.AppendFormat("{0}ZyTable.push_back({1}, {2})", currIndent, subTableVar, subRecordVar);
                    strTemp.AppendLine();

                    indent--;
                    currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendFormat("{0}end", currIndent);
                    strTemp.AppendLine();
                    currIndent = GetSpaceIndent(depth + indent - 1, preIndent);
                    strTemp.AppendFormat("{0}end", currIndent);
                    strTemp.AppendLine();

                    string tempTableVar = subTableVar;

                    depth--;
                    if (depth > 0)
                    {
                        subNumVar = subNumVar.Substring(0, subNumVar.LastIndexOf('_'));
                        subTableVar = subTableVar.Substring(0, subTableVar.LastIndexOf('_'));
                        subRecordVar = subRecordVar.Substring(0, subRecordVar.LastIndexOf('_'));
                    }
                    else
                    {
                        subTableVar = "subTabel";
                    }
                    strTemp.AppendFormat("{0}{1}.Children{2} = {3}",
                        currIndent,
                        depth > 0 ? subTableVar : "DataTabel",
                        "_" + recordIndex,
                        tempTableVar);
                    strTemp.AppendLine();
                    currTableVar = subTableVar;
                }
                else if (fieldType.Equals(FieldType.Head))
                {
                    continue;
                }
                else
                {
                    string currIndent = GetSpaceIndent(depth + indent, preIndent);
                    strTemp.AppendFormat("{0}{1}.{2}", currIndent, currTableVar, paramInfo.Field);

                    switch (fieldType)
                    {
                        case FieldType.Int:
                            strTemp.Append(" = ZyReader:getInt()");
                            break;
                        case FieldType.Short:
                            strTemp.Append(" = ZyReader:getWORD()");
                            break;
                        case FieldType.String:
                            strTemp.Append(" = ZyReader:readString()");
                            break;
                        case FieldType.Byte:
                            strTemp.Append(" = ZyReader:getByte()");
                            break;
                        default:
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
                                if (FieldType.Record.Equals(fieldType) || FieldType.End.Equals(fieldType))
                                {
                                    continue;

                                }
                                if (paramInfo.ParamType == 1)
                                {
                                    fieldBuilder.Append(currIndent);
                                    if (fieldType == FieldType.Byte
                                   || fieldType == FieldType.Int
                                   || fieldType == FieldType.Short
                                   )
                                    {
                                        fieldBuilder.Append(string.Format("self.{0} = 0", ToMemberVarName(paramInfo.Field)));
                                    }
                                    else if (fieldType == FieldType.String)
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
                if (fieldType.Equals(FieldType.Record))
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
                    builder.AppendFormat("{0}.{1} = {2}", currentVar, listVar, "None");
                    builder.Append(Environment.NewLine);
                }
                else if (fieldType.Equals(FieldType.End))
                {
                    listVar = listVar.Substring(0, listVar.LastIndexOf('_'));
                    depth--;
                }
                else if (fieldType.Equals(FieldType.Head))
                {
                }
                else if (depth == 0)
                {
                    string proValue = "''";
                    if (fieldType == FieldType.Byte ||
                        fieldType == FieldType.Short ||
                        fieldType == FieldType.Int)
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
                StringBuilder strTemp = new StringBuilder();
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
                        case FieldType.String:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{1}\")", varName, fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{1}\"{2})", varName, fieldName, minandMaxValue);
                                }
                            }
                            break;
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
                if (fieldType.Equals(FieldType.Record))
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
                    depth++;
                    string currIndent = GetSpaceIndent(depth, 0);
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("{0}.PushIntoStack(len(actionResult.{1}))", preItemVar, listVar);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("for {0} in actionResult.{1}:", enumVar, listVar);
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
                if (fieldType.Equals(FieldType.Head))
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
                    int recordIndex = 0;
                    int[] indexList = new int[forVarChars.Length];
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    var list = new List<ParamInfoModel>(reqParams);
                    list.AddRange(paramList);

                    foreach (var paramInfo in list)
                    {
                        FieldType fieldType = paramInfo.FieldType;
                        if (FieldType.Record.Equals(fieldType))
                        {
                            if (depth < indexList.Length)
                            {
                                recordIndex = indexList[depth];
                                recordIndex++;
                                indexList[depth] = recordIndex;
                            }
                            listVar = listVar + "_" + recordIndex;
                            fieldBuilder.AppendFormat("private List<Object> {0};", listVar);
                            fieldBuilder.Append(Environment.NewLine);
                            fieldBuilder.Append(GetSpaceIndent(0, 2));
                            depth++;
                            continue;
                        }
                        if (FieldType.End.Equals(fieldType))
                        {
                            listVar = listVar.Substring(0, listVar.LastIndexOf('_'));
                            depth--;
                            continue;
                        }
                        if (depth > 0)
                        {
                            continue;
                        }
                        fieldBuilder.Append("private ");

                        fieldBuilder.Append(fieldType.ToString().ToLower());
                        fieldBuilder.Append(" ");
                        fieldBuilder.Append(ToMemberVarName(paramInfo.Field));
                        fieldBuilder.Append(";");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetSpaceIndent(0, 2));
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            content = ReplaceCheckRequest(content, reqParams);
            content = ReplaceBuildPack(content, paramList);
            return content;
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
            string subIndent = GetSpaceIndent(indent + 1, 2);

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
                        case FieldType.Int:
                            strTemp.Append("httpGet.GetInt(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(varName);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.String:
                            strTemp.Append("httpGet.GetString(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(varName);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.Short:
                            strTemp.Append("httpGet.Short(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(varName);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.Byte:
                            strTemp.Append("httpGet.Byte(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(varName);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        default:
                            break;
                    }

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
            else if (reqParams.Count > 0)
            {
                stMust.Append(currIndent);
                stMust.Append("if (true)");
                stMust.Append(Environment.NewLine);
                stMust.Append(currIndent);
                stMust.Append("{");
                stMust.Append(Environment.NewLine);
            }
            if (reqParams.Count > 0)
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
            string currentVar = "this";
            string itemVar = "dsItem";
            string enumVar = "item";
            string listVar = "_dsItemList";
            int recordIndex = 0;
            int[] indexList = new int[forVarChars.Length];

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record))
                {
                    if (depth < indexList.Length)
                    {
                        recordIndex = indexList[depth];
                        recordIndex++;
                        indexList[depth] = recordIndex;
                    }
                    if (depth > 0)
                    {
                        currentVar = itemVar;
                        itemVar = itemVar + recordIndex;
                        enumVar = enumVar + "_" + recordIndex;
                    }
                    listVar = listVar + "_" + recordIndex;
                    string currIndent = GetSpaceIndent(indent + depth, 2);
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("{0}.PushIntoStack({1}.Length);", currentVar, listVar);
                    strTemp.AppendLine();
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("foreach (var {0} in {1})", enumVar, listVar);
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
                else if (fieldType.Equals(FieldType.Head))
                {
                }
                else
                {
                    string currIndent = GetSpaceIndent(indent + depth, 2);
                    strTemp.Append(currIndent);
                    strTemp.AppendFormat("{0}.PushIntoStack({1}", depth == 0 ? currentVar : itemVar, FormatFieldType(fieldType));
                    strTemp.Append(depth > 0 ? enumVar + "." + paramInfo.Field : ToMemberVarName(paramInfo.Field));
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


    }
}