using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BLL;
using System.Text;
using System.IO;
using model;
using System.Collections;

namespace ZyGames.ContractTools
{
    public class Mainfun
    {
        public static Hashtable LoadEnumApplication(int slnid, bool clean)
        {
            if (System.Web.HttpContext.Current.Application[slnid.ToString()] == null || clean)
            {
                Hashtable ht = new Hashtable();
                EnuminfoBLL dal = new EnuminfoBLL();
                DataTable dt = dal.GetList(slnid).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    string key = "【" + dr["enumName"].ToString() + "】";
                    if (!ht.Contains(key))
                    {
                        ht.Add(key, string.Format("{0}\n\n{1}", dr["enumDescription"].ToString(),
                            dr["enumValueInfo"].ToString()));
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
        /// <param name="mainDt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string FromatTempto(string content, DataSet mainDt, string title)
        {
            string[] expressList = new string[] { "##ID##", "##Description##", "##Parameter##", "##Fixed##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));
                if (fieldBuilder.ToString() == "ID")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(mainDt.Tables[0].Rows[0]["ContractID"].ToString());
                }
                else if (fieldBuilder.ToString() == "Description")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(title);
                }
                else if (fieldBuilder.ToString() == "Parameter")
                {
                    DataTable dt = mainDt.Tables[0];
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Type = dt.Rows[i]["ParamType"].ToString();
                        if (Convert.ToInt32(Type) == 1)
                        {
                            fieldBuilder.Append(",");
                            fieldBuilder.Append(dt.Rows[i]["Field"].ToString());
                        }


                    }
                }
                else if (fieldBuilder.ToString() == "Fixed")
                {
                    DataTable dt = mainDt.Tables[0];
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Type = dt.Rows[i]["ParamType"].ToString();
                        if (Convert.ToInt32(Type) == 1)
                        {
                            fieldBuilder.Append("ZyWriter:writeString(\"");
                            fieldBuilder.Append(dt.Rows[i]["Field"].ToString());
                            fieldBuilder.Append("\",");
                            fieldBuilder.Append(dt.Rows[i]["Field"].ToString());
                            fieldBuilder.Append(")");
                            fieldBuilder.Append(Environment.NewLine);
                            fieldBuilder.Append("        ");
                        }
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return ReplaceJudgeto(content, mainDt);
        }
        public static string GetspaceIndent(int n)
        {
            return new String(' ', n);

        }
        protected static string ReplaceJudgeto(string content, DataSet dt)
        {
            string field = "##Judge##";
            int zz = dt.Tables[0].Rows.Count;
            StringBuilder strTemp = new StringBuilder();
            DataView dv = dt.Tables[0].DefaultView;
            dv.RowFilter = " ParamType = 2";
            dv.Sort = "SortID";
            bool isInRecord = false;
            int n = 8;
            for (int i = 0; i < dv.Count; i++)
            {
                FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dv[i]["FieldType"]);
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
                                strTemp.Append(dv[i]["Field"].ToString());
                                strTemp.Append("= ZyReader:getInt()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Short:
                                strTemp.Append(GetspaceIndent(n * 2 - 1));
                                strTemp.Append(" mRecordTabel_1.");
                                strTemp.Append(dv[i]["Field"].ToString());
                                strTemp.Append("= ZyReader:getWORD()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.String:
                                strTemp.Append(GetspaceIndent(n * 2 - 1));
                                strTemp.Append(" mRecordTabel_1.");
                                strTemp.Append(dv[i]["Field"].ToString());
                                strTemp.Append("= ZyReader:readString()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Byte:
                                strTemp.Append(GetspaceIndent(n * 2 - 1));
                                strTemp.Append(" mRecordTabel_1.");
                                strTemp.Append(dv[i]["Field"].ToString());
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
                                strTemp.Append(dv[i]["Field"].ToString());
                                strTemp.Append("= ZyReader:getInt()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Short:
                                strTemp.Append(GetspaceIndent(n));
                                strTemp.Append("DataTabel.");
                                strTemp.Append(dv[i]["Field"].ToString());
                                strTemp.Append("= ZyReader:getWORD()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.String:
                                strTemp.Append(GetspaceIndent(n));
                                strTemp.Append("DataTabel.");
                                strTemp.Append(dv[i]["Field"].ToString());
                                strTemp.Append("= ZyReader:readString()");
                                strTemp.Append(Environment.NewLine);

                                break;
                            case FieldType.Byte:
                                strTemp.Append(GetspaceIndent(n));
                                strTemp.Append("DataTabel.");
                                strTemp.Append(dv[i]["Field"].ToString());
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

        public static string FormatActionDefineTemp(string content, DataSet mainDt, DataSet slnDS)
        {
            DataRow slnRecord = null;
            if (slnDS.Tables.Count > 0 && slnDS.Tables[0].Rows.Count > 0)
            {
                slnRecord = slnDS.Tables[0].Rows[0];
            }
            string[] expressList = new string[] { "##FieldList##", "##Namespace##", "##RefNamespace##" };
            int n = 8;
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));

                if (fieldBuilder.ToString() == "Namespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord["Namespace"].ToString());
                }
                else if (fieldBuilder.ToString() == "RefNamespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord["RefNamespace"].ToString());
                }
                else if (fieldBuilder.ToString() == "FieldList")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    DataTable dt = mainDt.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        fieldBuilder.Append("///<summary>");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));

                        fieldBuilder.AppendFormat("///{0}", dt.Rows[i]["Descption"].ToString());
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));

                        fieldBuilder.Append("///</summary>");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));

                        fieldBuilder.AppendFormat("public const Int16 Cst_Action{0} = {0};", dt.Rows[i]["ID"].ToString());
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());
            }
            return content;
        }
        internal static string FormatPython(string content, DataSet mainDt, DataSet slnRecord, string title)
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
                            DataTable dt = mainDt.Tables[0];
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dt.Rows[i]["FieldType"]);
                                if (FieldType.Record.Equals(fieldType) || FieldType.End.Equals(fieldType))
                                {
                                    continue;

                                }
                                string paramType = dt.Rows[i]["ParamType"].ToString();
                                if (paramType == "1")
                                {
                                    fieldBuilder.Append(GetspaceIndent(n * 2));
                                    if (fieldType == FieldType.Byte
                                   || fieldType == FieldType.Int
                                   || fieldType == FieldType.Short
                                   )
                                    {
                                        fieldBuilder.Append(string.Format("self.{0} = 0", dt.Rows[i]["Field"].ToString()));
                                    }
                                    else if (fieldType == FieldType.String)
                                    {
                                        fieldBuilder.Append(string.Format("self.{0} = ''", dt.Rows[i]["Field"].ToString()));
                                    }
                                    fieldBuilder.Append(Environment.NewLine);
                                }

                            }
                        }
                        break;

                    case "##getUrlElement##":
                        ReplacePythonJudge(fieldBuilder, mainDt);
                        break;
                    case "##actionResult##":
                        ReplacePythonAction(fieldBuilder, mainDt);
                        break;
                    case "##buildPacket##":
                        ReplacebuildPacket(fieldBuilder, mainDt);
                        break;
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            return content;
        }

        private static void ReplacePythonAction(StringBuilder builder, DataSet ds)
        {
            //todo
            int n = 4;
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "paramType = 2";
            dv.Sort = "SortID";
            int depth = 1;
            string currentVar = "self";
            string itemVar = "DsItemCollect";

            for (int i = 0; i < dv.Count; i++)
            {
                FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dv[i]["FieldType"]);
                string fieldValue = dv[i]["Field"].ToString();
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

        protected static void ReplacePythonJudge(StringBuilder stMust, DataSet ds)
        {
            StringBuilder stNotMust = new StringBuilder();
            DataView dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "paramType = 1";
            dv.Sort = "SortID";
            int n = 4;
            for (int i = 0; i < dv.Count; i++)
            {
                StringBuilder strTemp = new StringBuilder();
                if (dv[i]["ParamType"].ToString().Equals("1"))
                {
                    FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dv[i]["FieldType"]);
                    int minValue = Convert.ToInt32(dv[i]["MinValue"]);
                    int maxValue = Convert.ToInt32(dv[i]["MaxValue"]);
                    stNotMust.Append(GetspaceIndent(n * 2));
                    switch (fieldType)
                    {
                        case FieldType.Int:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetIntValue(\"{0}\")", dv[i]["Field"].ToString());
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetIntValue(\"{0}\"{1})", dv[i]["Field"].ToString(), minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.String:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{0}\")", dv[i]["Field"].ToString());
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetStringValue(\"{0}\"{1})", dv[i]["Field"].ToString(), minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.Short:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetWordValue(\"{0}\")", dv[i]["Field"].ToString());
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetWordValue(\"{0}\"{1})", dv[i]["Field"].ToString(), minandMaxValue);
                                }
                            }
                            break;
                        case FieldType.Byte:
                            {
                                string minandMaxValue = SetValueRange(minValue, maxValue);
                                if (minandMaxValue.Length == 0)
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetByteValue(\"{0}\")", dv[i]["Field"].ToString());
                                }
                                else
                                {
                                    stNotMust.AppendFormat("urlParam.{0} = httpGet.GetByteValue(\"{0}\"{1})", dv[i]["Field"].ToString(), minandMaxValue);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    stNotMust.Append(Environment.NewLine);

                    if (Convert.ToBoolean(dv[i]["Required"]))
                    {
                        if (stMust.ToString().Length == 0)
                        {
                            stMust.Append(GetspaceIndent(n));
                            stMust.AppendFormat("if httpGet.Contains(\"{0}\")", dv[i]["Field"].ToString());
                        }
                        else
                        {
                            stMust.Append("\\");
                            stMust.Append(Environment.NewLine);
                            stMust.Append(GetspaceIndent(n));
                            stMust.AppendFormat("and httpGet.Contains(\"{0}\")", dv[i]["Field"].ToString());
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

        protected static void ReplacebuildPacket(StringBuilder strTemp, DataSet dt)
        {
            int n = 4;
            DataView dv = dt.Tables[0].DefaultView;
            dv.RowFilter = "paramType = 2";
            dv.Sort = "SortID";
            int depth = 0;
            string currentVar = "actionResult";
            string itemVar = "dsItem";
            string preItemVar = "writer";

            for (int i = 0; i < dv.Count; i++)
            {
                FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dv[i]["FieldType"]);
                string fieldValue = dv[i]["Field"].ToString();
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
        /// <param name="mainDt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string FormatTemp(string content, DataSet mainDt, DataSet slnDS, string title)
        {
            DataRow slnRecord = null;
            if (slnDS.Tables.Count > 0 && slnDS.Tables[0].Rows.Count > 0)
            {
                slnRecord = slnDS.Tables[0].Rows[0];
            }

            int n = 8;
            string[] expressList = new string[] { "##ID##", "##Description##", "##Field##", "##Namespace##", "##RefNamespace##" };
            foreach (string exp in expressList)
            {
                StringBuilder fieldBuilder = new StringBuilder();
                fieldBuilder.Append(exp.Replace("##", ""));

                if (fieldBuilder.ToString() == "ID")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(mainDt.Tables[0].Rows[0]["ContractID"].ToString());

                }
                else if (fieldBuilder.ToString() == "Description")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(title);
                }
                else if (fieldBuilder.ToString() == "Namespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord["Namespace"].ToString());
                }
                else if (fieldBuilder.ToString() == "RefNamespace")
                {
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    fieldBuilder.Append(slnRecord == null ? "" : slnRecord["RefNamespace"].ToString());
                }

                else if (fieldBuilder.ToString() == "Field")
                {

                    DataTable dt = mainDt.Tables[0];
                    fieldBuilder.Remove(0, fieldBuilder.Length);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dt.Rows[i]["FieldType"]);
                        if (FieldType.Record.Equals(fieldType) || FieldType.End.Equals(fieldType))
                        {
                            continue;

                        }
                        fieldBuilder.Append("private ");

                        fieldBuilder.Append(fieldType.ToString().ToLower());
                        fieldBuilder.Append(" ");
                        fieldBuilder.Append(dt.Rows[i]["Field"].ToString());
                        fieldBuilder.Append(";");
                        fieldBuilder.Append(Environment.NewLine);
                        fieldBuilder.Append(GetspaceIndent(n));
                    }
                }
                content = content.Replace(exp, fieldBuilder.ToString());

            }
            content = ReplaceJudge(content, mainDt);
            content = Replacejohc(content, mainDt);
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

        protected static string ReplaceJudge(string content, DataSet dt)
        {
            string field = "##Judge##";
            StringBuilder stMust = new StringBuilder();
            StringBuilder stNotMust = new StringBuilder();

            int n = 8;
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                StringBuilder strTemp = new StringBuilder();
                if (dt.Tables[0].Rows[i]["ParamType"].ToString().Equals("1"))
                {
                    FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dt.Tables[0].Rows[i]["FieldType"]);
                    int minValue = Convert.ToInt32(dt.Tables[0].Rows[i]["MinValue"]);
                    int maxValue = Convert.ToInt32(dt.Tables[0].Rows[i]["MaxValue"]);
                    switch (fieldType)
                    {
                        case FieldType.Int:
                            strTemp.Append("httpGet.GetInt(\"");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append("\", ref ");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.String:
                            strTemp.Append("httpGet.GetString(\"");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append("\", ref ");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.Short:
                            strTemp.Append("httpGet.Short(\"");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append("\", ref ");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        case FieldType.Byte:
                            strTemp.Append("httpGet.Byte(\"");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append("\", ref ");
                            strTemp.Append(dt.Tables[0].Rows[i]["Field"].ToString());
                            strTemp.Append(SetValueRange(minValue, maxValue));
                            strTemp.Append(")");
                            break;
                        default:
                            break;
                    }

                    if (Convert.ToBoolean(dt.Tables[0].Rows[i]["Required"]))
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

        protected static string Replacejohc(string content, DataSet dt)
        {
            int n = 8;
            string field = "##johc##";
            StringBuilder strTemp = new StringBuilder();
            DataView dv = dt.Tables[0].DefaultView;
            dv.RowFilter = "paramType = 2";
            dv.Sort = "SortID";
            int depth = 0;
            string currentVar = "this";
            string itemVar = "dsItem";

            for (int i = 0; i < dv.Count; i++)
            {
                FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dv[i]["FieldType"]);
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
                strTemp.Append(dv[i]["Field"].ToString());
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

        internal static string GetExampleUrl(DataSet ds, string contractID)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.AppendFormat("ActionID={0}", contractID);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), dt.Rows[i]["FieldType"]);
                int paramType = Convert.ToInt32(dt.Rows[i]["ParamType"]);
                bool required = Convert.ToBoolean(dt.Rows[i]["Required"]);
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
                urlBuilder.AppendFormat("{0}={1}", dt.Rows[i]["Field"], dt.Rows[i]["FieldValue"]);
            }
            return urlBuilder.ToString();
        }


    }
}
