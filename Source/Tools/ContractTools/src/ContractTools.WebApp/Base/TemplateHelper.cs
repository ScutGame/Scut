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

namespace ContractTools.WebApp.Base
{
    public class TemplateHelper
    {
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
            string temp = string.Empty;
            using (FileStream fs = File.Open(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                temp = sr.ReadToEnd();
            }
            return temp;
        }
        /// <summary>
        /// 赋值客户端模板
        /// </summary>
        /// <param name="content"></param>
        /// <param name="respParam"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string FromatTempto(string content, int contractId, List<ParamInfoModel> respParam, string title)
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
                    foreach (var paramInfo in respParam)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append(",");
                            fieldBuilder.Append(paramInfo.Field);
                        }


                    }
                }
                else if (fieldBuilder.ToString() == "Fixed")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    foreach (var paramInfo in respParam)
                    {
                        if (paramInfo.ParamType == 1)
                        {
                            fieldBuilder.Append("ZyWriter:writeString(\"");
                            fieldBuilder.Append(paramInfo.Field);
                            fieldBuilder.Append("\",");
                            fieldBuilder.Append(paramInfo.Field);
                            fieldBuilder.Append(")");
                            fieldBuilder.Append(Environment.NewLine);
                            fieldBuilder.Append("        ");
                        }
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return ReplaceJudgeto(content, respParam);
        }
        public static string GetspaceIndent(int n)
        {
            return new String(' ', n);

        }
        protected static string ReplaceJudgeto(string content, List<ParamInfoModel> paramList)
        {
            string field = "##Judge##";
            StringBuilder strTemp = new StringBuilder();
            bool isInRecord = false;
            int n = 8;
            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record))
                {

                    strTemp.Append(GetspaceIndent(n));
                    strTemp.Append("local RecordNums_1=ZyReader:getInt()");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n + 1));
                    strTemp.Append("local RecordTabel_1={}");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n + 2));
                    strTemp.Append("if RecordNums_1~=0 then");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n + 4));
                    strTemp.Append("for k=1,RecordNums_1 do");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n + 5));
                    strTemp.Append("local mRecordTabel_1={}");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n + 5));
                    strTemp.Append("ZyReader:recordBegin()");
                    strTemp.Append(Environment.NewLine);
                    isInRecord = true;

                }
                else if (fieldType.Equals(FieldType.End))
                {
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.Append("ZyReader:recordEnd()");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.Append("ZyTable.push_back(RecordTabel_1,mRecordTabel_1)");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * 2 - 2));
                    strTemp.Append("end");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n));
                    strTemp.Append("end");
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.Append("DataTabel.RecordTabel = RecordTabel_1;");
                    strTemp.Append(Environment.NewLine);
                    isInRecord = false;
                }
                else
                {
                    if (isInRecord)
                    {
                        switch (fieldType)
                        {
                            case FieldType.Int:
                                strTemp.Append(GetspaceIndent(n * 2 - 1));
                                strTemp.Append(" mRecordTabel_1.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:getInt()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Short:
                                strTemp.Append(GetspaceIndent(n * 2 - 1));
                                strTemp.Append(" mRecordTabel_1.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:getWORD()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.String:
                                strTemp.Append(GetspaceIndent(n * 2 - 1));
                                strTemp.Append(" mRecordTabel_1.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:readString()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Byte:
                                strTemp.Append(GetspaceIndent(n * 2 - 1));
                                strTemp.Append(" mRecordTabel_1.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:getByte()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {

                        switch (fieldType)
                        {
                            case FieldType.Int:
                                strTemp.Append(GetspaceIndent(n));
                                strTemp.Append("DataTabel.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:getInt()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Short:
                                strTemp.Append(GetspaceIndent(n));
                                strTemp.Append("DataTabel.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:getWORD()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.String:
                                strTemp.Append(GetspaceIndent(n));
                                strTemp.Append("DataTabel.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:readString()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Byte:
                                strTemp.Append(GetspaceIndent(n));
                                strTemp.Append("DataTabel.");
                                strTemp.Append(paramInfo.Field);
                                strTemp.Append("= ZyReader:getByte()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            default:
                                break;
                        }
                    }

                }
            }
            content = content.Replace(field, strTemp.ToString());
            return content;
        }

        public static string FormatActionDefineTemp(string content, List<ContractModel> contractList, SolutionModel slnRecord)
        {
            string[] expressList = new string[] { "##FieldList##", "##Namespace##", "##RefNamespace##" };
            int n = 8;
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
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    foreach (var contract in contractList)
                    {
                        fieldBuilder.Append("///<summary>");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));

                        fieldBuilder.AppendFormat("///{0}", contract.Descption);
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));

                        fieldBuilder.Append("///</summary>");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));

                        fieldBuilder.AppendFormat("public const Int16 Cst_Action{0} = {0};", contract.ID);
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());
            }
            return content;
        }
        internal static string FormatPython(string content, List<ParamInfoModel> paramList, List<ParamInfoModel> reqParams, SolutionModel slnRecord, string title)
        {
            int n = 4;
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
                            foreach (var paramInfo in paramList)
                            {
                                FieldType fieldType = paramInfo.FieldType;
                                if (FieldType.Record.Equals(fieldType) || FieldType.End.Equals(fieldType))
                                {
                                    continue;

                                }
                                if (paramInfo.ParamType == 1)
                                {
                                    fieldBuilder.Append(GetspaceIndent(n * 2));
                                    if (fieldType == FieldType.Byte
                                   || fieldType == FieldType.Int
                                   || fieldType == FieldType.Short
                                   )
                                    {
                                        fieldBuilder.Append(string.Format("self.{0} = 0", paramInfo.Field));
                                    }
                                    else if (fieldType == FieldType.String)
                                    {
                                        fieldBuilder.Append(string.Format("self.{0} = ''", paramInfo.Field));
                                    }
                                    fieldBuilder.Append(Environment.NewLine);
                                }

                            }
                        }
                        break;

                    case "##getUrlElement##":
                        ReplacePythonJudge(fieldBuilder, reqParams);
                        break;
                    case "##actionResult##":
                        ReplacePythonAction(fieldBuilder, paramList);
                        break;
                    case "##buildPacket##":
                        ReplacebuildPacket(fieldBuilder, paramList);
                        break;
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return content;
        }

        private static void ReplacePythonAction(StringBuilder builder, List<ParamInfoModel> paramList)
        {
            int n = 4;
            int depth = 1;
            string currentVar = "self";
            string itemVar = "DsItemCollect";
            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                string fieldValue = paramInfo.Field;
                if (fieldType.Equals(FieldType.Record))
                {
                    builder.Append(GetspaceIndent(n * (depth + 1)));
                    builder.AppendFormat("{0}.{1} = {2}", currentVar, itemVar, "None");
                    builder.Append(Environment.NewLine);
                    break;
                }
                if (fieldType.Equals(FieldType.Head))
                {
                    continue;
                }
                string proValue = "''";
                if (fieldType == FieldType.Byte ||
                    fieldType == FieldType.Short ||
                    fieldType == FieldType.Int)
                {
                    proValue = "0";
                }
                builder.Append(GetspaceIndent(n * (depth + 1)));
                builder.AppendFormat("{0}.{1} = {2}", currentVar, fieldValue, proValue);
                builder.Append(Environment.NewLine);
            }
        }

        protected static void ReplacePythonJudge(StringBuilder stMust, List<ParamInfoModel> reqParams)
        {
            StringBuilder stNotMust = new StringBuilder();
            int n = 4;
            foreach (var paramInfo in reqParams)
            {
                StringBuilder strTemp = new StringBuilder();
                if (paramInfo.ParamType == 1)
                {
                    FieldType fieldType = paramInfo.FieldType;
                    int minValue = paramInfo.MinValue;
                    int maxValue = paramInfo.MaxValue;
                    stNotMust.Append(GetspaceIndent(n * 2));
                    string fieldName = paramInfo.Field;
                    switch (fieldType)
                    {
                        case FieldType.Int:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetIntValue(\"{0}\")", fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetIntValue(\"{0}\"{1})", fieldName, minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.String:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{0}\")", fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{0}\"{1})", fieldName, minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.Short:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetWordValue(\"{0}\")", fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetWordValue(\"{0}\"{1})", fieldName, minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.Byte:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetByteValue(\"{0}\")", fieldName);
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetByteValue(\"{0}\"{1})", fieldName, minandMaxValue);
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
                            stMust.Append(GetspaceIndent(n));
                            stMust.AppendFormat("if httpGet.Contains(\"{0}\")", fieldName);
                        }
                        else
                        {
                            stMust.Append("\\");
                            stMust.Append(Environment.NewLine);
                            stMust.Append(GetspaceIndent(n));
                            stMust.AppendFormat("and httpGet.Contains(\"{0}\")", fieldName);
                        }
                    }
                }

            }
            if (stMust.ToString().Length > 0)
            {
                stMust.Append(":");
                stMust.Append(Environment.NewLine);
            }
            else
            {
                stMust.Append(GetspaceIndent(n));
                stMust.Append("if True:");
                stMust.Append(Environment.NewLine);
                stMust.Append(GetspaceIndent(n * 2));
                stMust.Append("urlParam.Result = True");
                stMust.Append(Environment.NewLine);
            }
            stMust.Append(stNotMust);
            stMust.Append(GetspaceIndent(n));
            stMust.Append("else:\n");
            stMust.Append(GetspaceIndent(n * 2));
            stMust.Append("urlParam.Result = False");
            stMust.Append(Environment.NewLine);
        }

        protected static void ReplacebuildPacket(StringBuilder strTemp, List<ParamInfoModel> paramList)
        {
            int n = 4;
            int depth = 0;
            string currentVar = "actionResult";
            string itemVar = "dsItem";
            string preItemVar = "writer";
            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                string fieldValue = paramInfo.Field;
                if (fieldType.Equals(FieldType.Record))
                {
                    if (depth > 0)
                    {
                        preItemVar = itemVar;
                        currentVar = itemVar;
                        itemVar = itemVar + depth;
                    }
                    strTemp.Append(GetspaceIndent(n * (depth + 1)));
                    strTemp.AppendFormat("{0}.PushIntoStack(len(actionResult.{1}Collect))", preItemVar, itemVar);
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * (depth + 1)));
                    strTemp.AppendFormat("for info in actionResult.{0}Collect:", itemVar);
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * (depth + 2)));
                    strTemp.AppendFormat("{0} = DataStruct()", itemVar);
                    strTemp.Append(Environment.NewLine);
                    depth++;
                    continue;
                }
                if (fieldType.Equals(FieldType.End))
                {
                    strTemp.Append(GetspaceIndent(n * (depth + 1)));
                    strTemp.AppendFormat("{0}.PushIntoStack({1})", preItemVar, itemVar);
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(Environment.NewLine);
                    depth--;
                    if (depth > 0)
                    {
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
                    continue;
                }
                if (fieldType.Equals(FieldType.Head))
                {
                    continue;
                }

                strTemp.Append(GetspaceIndent(n * (depth + 1)));
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
                    strTemp.AppendFormat("writer.{2}({0}.{1})", currentVar, fieldValue, putMethod);
                }
                else
                {
                    strTemp.AppendFormat("{0}.{2}(info.{1})", itemVar, fieldValue, putMethod);
                }
                strTemp.Append(Environment.NewLine);
            }
        }

        private static string PythonGetUrlElement(string field, string key)
        {
            return string.Format("{1}if httpGet.Contains(\"{0}\"):\n{1}{1}urlParam.{0} = httpGet.Get{2}(\"{0}\")\n{1}else:\n{1}{1}urlParam.Result = False\n{1}{1}return urlParam", field, GetspaceIndent(4), key);
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
            int n = 8;
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

                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    foreach (var paramInfo in paramList)
                    {

                        FieldType fieldType = paramInfo.FieldType;
                        if (FieldType.Record.Equals(fieldType) || FieldType.End.Equals(fieldType))
                        {
                            continue;

                        }
                        fieldBuilder.Append("private ");

                        fieldBuilder.Append(fieldType.ToString().ToLower());
                        fieldBuilder.Append(" ");
                        fieldBuilder.Append(paramInfo.Field);
                        fieldBuilder.Append(";");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            content = ReplaceJudge(content, reqParams);
            content = Replacejohc(content, paramList);
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

        protected static string ReplaceJudge(string content, List<ParamInfoModel> reqParams)
        {
            string field = "##Judge##";
            StringBuilder stMust = new StringBuilder();
            StringBuilder stNotMust = new StringBuilder();

            int n = 8;
            foreach (var paramInfo in reqParams)
            {

                StringBuilder strTemp = new StringBuilder();
                if (paramInfo.ParamType == 1)
                {
                    FieldType fieldType = paramInfo.FieldType;
                    int minValue = paramInfo.MinValue;
                    int maxValue = paramInfo.MaxValue;
                    string fieldname = paramInfo.Field;
                    switch (fieldType)
                    {
                        case FieldType.Int:
                            strTemp.Append("httpGet.GetInt(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(fieldname);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.String:
                            strTemp.Append("httpGet.GetString(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(fieldname);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.Short:
                            strTemp.Append("httpGet.Short(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(fieldname);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.Byte:
                            strTemp.Append("httpGet.Byte(\"");
                            strTemp.Append(fieldname);
                            strTemp.Append("\", ref ");
                            strTemp.Append(fieldname);
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        default:
                            break;
                    }

                    if (paramInfo.Required)
                    {
                        stMust.Append(GetspaceIndent(n + 4));
                        if (stMust.ToString().Trim() == "")
                        {
                            stMust.Append("if (");
                        }
                        else
                        {
                            stMust.Append(Environment.NewLine);
                            stMust.Append(GetspaceIndent(n * 2));
                            stMust.Append(" &&");
                        }
                        stMust.Append(strTemp);

                    }
                    else
                    {
                        stNotMust.Append(GetspaceIndent(n * 2));
                        stNotMust.Append(strTemp).Append(";");
                        stNotMust.Append(Environment.NewLine);
                    }
                }
            }
            if (stMust.ToString().Length > 0)
            {
                stMust.Append(")");
                stMust.Append(Environment.NewLine);
                stMust.Append(GetspaceIndent(n + 4));
                stMust.Append("{");
                stMust.Append(Environment.NewLine);
            }
            else
            {
                stMust.Append(GetspaceIndent(n + 4));
                stMust.Append("if (true)");
                stMust.Append(Environment.NewLine);
                stMust.Append(GetspaceIndent(n + 4));
                stMust.Append("{");
                stMust.Append(Environment.NewLine);
            }
            stMust.Append(stNotMust);
            stMust.Append(GetspaceIndent(n * 2));
            stMust.Append("return true;");
            stMust.Append(Environment.NewLine);
            stMust.Append(GetspaceIndent(n + 4));
            stMust.Append("}");
            return content.Replace(field, stMust.ToString());
        }

        protected static string Replacejohc(string content, List<ParamInfoModel> paramList)
        {
            int n = 8;
            string field = "##johc##";
            StringBuilder strTemp = new StringBuilder();
            int depth = 0;
            string currentVar = "this";
            string itemVar = "dsItem";

            foreach (var paramInfo in paramList)
            {
                FieldType fieldType = paramInfo.FieldType;
                if (fieldType.Equals(FieldType.Record))
                {
                    if (depth > 0)
                    {
                        currentVar = itemVar;
                        itemVar = itemVar + depth;
                    }
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.AppendFormat("{0}.PushIntoStack({2}{1}Collection.Length);", currentVar, itemVar, FormatFieldType(fieldType));
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.AppendFormat("foreach (var item in {0}Collection )", itemVar);
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.Append("{");
                    strTemp.Append(Environment.NewLine);
                    n = n + 2;
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.AppendFormat("DataStruct {0} = new DataStruct();", itemVar);
                    strTemp.Append(Environment.NewLine);

                    depth++;
                    continue;
                }
                if (fieldType.Equals(FieldType.End))
                {
                    strTemp.Append(Environment.NewLine);
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.AppendFormat("{0}.PushIntoStack({1});", currentVar, itemVar);
                    strTemp.Append(Environment.NewLine);
                    n = n - 2;
                    strTemp.Append(GetspaceIndent(n * 2));
                    strTemp.Append("}");
                    strTemp.Append(Environment.NewLine);

                    depth--;
                    if (depth > 0)
                    {
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
                    continue;
                }
                if (fieldType.Equals(FieldType.Head))
                {
                    continue;
                }

                strTemp.Append(GetspaceIndent(n * 2));
                strTemp.AppendFormat("{0}.PushIntoStack({1}", depth == 0 ? currentVar : itemVar, FormatFieldType(fieldType));
                strTemp.Append(paramInfo.Field);
                strTemp.Append(");");
                strTemp.Append(Environment.NewLine);


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