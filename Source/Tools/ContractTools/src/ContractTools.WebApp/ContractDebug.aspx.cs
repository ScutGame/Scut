using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.RPC.IO;

namespace ContractTools.WebApp
{
    public partial class ContractDebug : BasePage
    {

        public const int ErrorCode = 10000;
        protected int SlnID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["slnID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request["slnID"]);
            }
        }
        protected int VerID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["VerID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request["VerID"]);
            }
        }
        protected int ContractID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["ID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request["ID"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {
                    var list = DbDataLoader.GetContract(SlnID, VerID);
                    if (list.Count > 0)
                    {
                        ddlContract.DataSource = list;
                        ddlContract.DataTextField = "uname";
                        ddlContract.DataValueField = "ID";
                        ddlContract.DataBind();
                        ddlContract.SelectedValue = ContractID.ToString();
                    }

                    Bind();
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Contract debug error:{0}", ex);
            }
        }

        private void Bind()
        {
            int slnId = SlnID;
            int verId = VerID;
            int contractId = ddlContract.Text.ToInt();

            SolutionModel solutionMode = DbDataLoader.GetSolution(slnId);
            if (solutionMode != null)
            {
                if (this.txtServerUrl.Text.Length == 0)
                {
                    this.txtServerUrl.Text = solutionMode.Url;
                }
                int msgId = txtMsgId.Text.ToInt();
                txtParams.Text = BuildRequestParam(msgId, txtSid.Text, txtUid.Text, contractId, slnId, verId, solutionMode.GameID);
            }
        }

        protected void OnContractSelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        protected void OnRefresh(object sender, EventArgs e)
        {
            Bind();
        }
        protected void OnSendClick(object sender, EventArgs e)
        {
            try
            {
                int contractId = ddlContract.Text.ToInt();
                string pid = Session["pid"] == null ? Session.SessionID : Session["pid"].ToString();
                Session["pid"] = pid;
                var paramRecords = DbDataLoader.GetParamInfo(SlnID, contractId, VerID);
                string postParam = FormatPostParam(paramRecords.Where(t => t.ParamType == 1).ToList(), txtParams.Text);
                txtPostParam.Text = string.Format("?d={0}", NetHelper.GetSign(postParam));
                dvResult.InnerHtml = "正在请求,请稍候...";
                string sid;
                string uid;
                dvResult.InnerHtml = PostGameServer(paramRecords.Where(t => t.ParamType == 2).ToList(), contractId, txtServerUrl.Text, postParam, pid, out sid, out uid);
                if (!string.IsNullOrEmpty(sid))
                {
                    txtSid.Text = sid;
                    txtUid.Text = uid;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Contract debug error:{0}", ex);
            }
        }

        private string FormatPostParam(List<ParamInfoModel> paramList, string text)
        {
            StringBuilder requestParams = new StringBuilder();
            string[] line = text.Split('\r', '\n');
            foreach (var str in line)
            {
                string[] paramArray = str.Split('=');
                if (paramArray.Length > 1)
                {
                    string name = paramArray[0].Trim();
                    string value = paramArray.Length > 2 ? string.Join("=", paramArray, 1, paramArray.Length - 1) : paramArray[1];
                    value = value.Trim();
                    var record = paramList.Find(t => string.Equals(t.Field, name, StringComparison.CurrentCultureIgnoreCase));
                    if (record != null && record.FieldType == FieldType.Password)
                    {
                        value = new DESAlgorithmNew().EncodePwd(value, NetHelper.ClientDesDeKey);
                    }
                    value = HttpUtility.UrlEncode(value, Encoding.UTF8);
                    requestParams.AppendFormat("{0}={1}&", name, value);
                }
            }
            return requestParams.ToString().TrimEnd('&');
        }

        private string BuildRequestParam(int msgId, string sid, string uid, int contractId, int slnId, int versionId, int gameId, int serverId = 0)
        {
            StringBuilder requestParams = new StringBuilder();
            requestParams.AppendLine("MsgId=" + msgId);
            requestParams.AppendLine("Sid=" + sid);
            requestParams.AppendLine("Uid=" + uid);
            requestParams.AppendLine("ActionID=" + contractId);
            if (serverId > 0)
            {
                requestParams.AppendLine("GameType=" + gameId);
                requestParams.AppendLine("ServerID=" + serverId);
            }

            var paramRecords = DbDataLoader.GetParamInfo(slnId, contractId, 1, versionId);

            foreach (var record in paramRecords)
            {
                string fieldName = record.Field;
                string fieldValue = record.FieldValue;
                requestParams.AppendLine(fieldName + "=" + fieldValue);
            }
            return requestParams.ToString();
        }


        private static string PostGameServer(List<ParamInfoModel> paramList, int contractId, string serverUrl, string requestParams, string pid, out string sid, out string uid)
        {
            bool isSocket = !serverUrl.StartsWith("http://");
            sid = "";
            uid = "";
            StringBuilder respContent = new StringBuilder();
            MessageHead msg = new MessageHead();
            MessageStructure msgReader = NetHelper.Create(serverUrl, requestParams, out msg, isSocket, contractId, pid);
            if (msgReader != null)
            {
                ProcessResult(paramList, contractId, respContent, msg, msgReader, out sid, out uid);
            }
            else
            {
                ResponseHead(contractId, respContent, ErrorCode, "请求超时");
            }
            return respContent.ToString();
        }

        private static void ProcessResult(List<ParamInfoModel> paramList, int contractId, StringBuilder respContent, MessageHead msg, MessageStructure msgReader, out string sid, out string uid)
        {
            sid = "";
            uid = "";
            ResponseHead(contractId, respContent, msg.ErrorCode, msg.ErrorInfo);


            if (msg.ErrorCode < ErrorCode)
            {
                //消息体
                respContent.AppendFormat("<h3>{0}-{1}</h3>", contractId, "返回结果");
                respContent.Append("<table style=\"width:99%; border-color:#f0f0f0\" border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
                respContent.Append("<tr><td style=\"width:15%;\"><strong>参数</strong></td><td style=\"width:10%;\"><strong>类型</strong></td>");
                respContent.Append("<td style=\"width:75%;\"><strong>参数值</strong></td></tr>");
                int loopDepth = 0;//循环深度
                List<ParamInfoModel> recordQueue = new List<ParamInfoModel>();
                #region 循环体
                foreach (var record in paramList)
                {
                    if (record.FieldType == FieldType.Void)
                    {
                        continue;
                    }
                    string fieldName = record.Field;
                    FieldType fieldType = record.FieldType;
                    string fieldValue = "";
                    try
                    {
                        if (loopDepth > 0 && fieldType == FieldType.End)
                        {
                            loopDepth--;
                            recordQueue.Add(record);
                        }
                        if (loopDepth == 0 && recordQueue.Count > 0)
                        {
                            //处理循环记录
                            ProcessLoopRocord(respContent, recordQueue, msgReader);
                            recordQueue.Clear();
                        }

                        if (loopDepth == 0)
                        {
                            if (NetHelper.GetFieldValue(msgReader, fieldType, ref fieldValue))
                            {
                                //自动登录
                                if (NetHelper.LoginActionId == contractId)
                                {
                                    if ("SessionID".Equals(fieldName)) sid = fieldValue;
                                    if ("UserID".Equals(fieldName)) uid = fieldValue;
                                }
                                respContent.Append("<tr>");
                                respContent.AppendFormat("<td>&nbsp;{0}</td>", fieldName);
                                respContent.AppendFormat("<td>&nbsp;{0}</td>", fieldType);
                                respContent.AppendFormat("<td>&nbsp;{0}</td>", fieldValue);
                                respContent.Append("</tr>");
                            }
                            if (fieldType == FieldType.Record)
                            {
                                loopDepth++;
                                recordQueue.Add(record);
                            }
                        }
                        else if (fieldType != FieldType.End)
                        {
                            if (fieldType == FieldType.Record)
                            {
                                loopDepth++;
                            }
                            recordQueue.Add(record);
                        }
                    }
                    catch (Exception ex)
                    {
                        respContent.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", fieldName, fieldType, ex.ToString());
                    }

                }
                #endregion

                respContent.Append("</table>");

            }
        }

        private static void ResponseHead(int contractId, StringBuilder respContent, int errorCode, string errorInfo)
        {
            //头部消息
            respContent.AppendFormat("<h3>{0}-{1}</h3>", contractId, "返回头部消息");
            respContent.Append("<table style=\"width:99%; border-color:#f0f0f0\" border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
            respContent.Append("<tr><td style=\"width:25%;\"><strong>状态值</strong></td>");
            respContent.Append("<td style=\"width:75%;\"><strong>描述</strong></td></tr>");
            respContent.AppendFormat("<tr><td>{0}</td>", errorCode);
            respContent.AppendFormat("<td>{0}&nbsp;</td></tr>", errorInfo);
            respContent.Append("</table>");
        }

        /// <summary>
        /// 处理循环记录
        /// </summary>
        /// <param name="respContent"></param>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        private static void ProcessLoopRocord(StringBuilder respContent, List<ParamInfoModel> queue, MessageStructure reader)
        {
            StringBuilder headContent = new StringBuilder();
            StringBuilder builderContent = new StringBuilder();
            int recordCount = 0;
            try
            {
                recordCount = reader.ReadInt();
            }
            catch (Exception ex)
            {
            }
            respContent.Append("<tr>");
            respContent.Append("<td style=\"width:25%;\" align=\"left\">Record(N)</td>");
            respContent.Append("<td style=\"width:20%;\" align=\"left\">Record</td>");
            respContent.AppendFormat("<td style=\"width:50%;\" align=\"left\">{0}</td>", recordCount);
            respContent.Append("</tr>");

            respContent.Append("<tr><td colspan=\"3\" align=\"center\">");
            respContent.Append("<!--子表开始--><table style=\"width:98%; border-color:#f0f0f0\" border=\"1\" cellpadding=\"2\" cellspacing=\"0\">");
            if (recordCount == 0)
            {
                builderContent.Append("<tr><td align=\"center\">空数据</td></tr>");
            }


            for (int i = 0; i < recordCount; i++)
            {
                try
                {
                    reader.RecordStart();
                    MessageStructure msgReader = reader;
                    int loopDepth = 0; //循环深度
                    List<ParamInfoModel> recordQueue = new List<ParamInfoModel>();

                    headContent.Append("<tr><!--头开始tr-->");
                    builderContent.Append("<tr><!--内容开始tr-->");
                    int columnNum = 0;

                    #region

                    for (int r = 1; r < queue.Count - 1; r++)
                    {
                        var record = queue[r];
                        string fieldName = record.Field;
                        FieldType fieldType = record.FieldType;
                        string fieldValue = "";
                        try
                        {
                            if (loopDepth > 0 && fieldType == FieldType.End)
                            {
                                loopDepth--;
                                recordQueue.Add(record);
                            }
                            if (loopDepth == 0 && recordQueue.Count > 0)
                            {
                                builderContent.Append("</tr><tr>");
                                builderContent.AppendFormat("<td colspan=\"{0}\" align=\"right\">", columnNum);
                                builderContent.Append(
                                    "<!--子表开始--><table style=\"width:95%; border-color:#f0f0f0\" border=\"1\" cellpadding=\"2\" cellspacing=\"0\">");
                                //处理循环记录
                                ProcessLoopRocord(builderContent, recordQueue, msgReader);

                                builderContent.Append("</table><!--子表结束-->");
                                builderContent.Append("</td>");
                                recordQueue.Clear();
                            }

                            if (loopDepth == 0)
                            {
                                if (NetHelper.GetFieldValue(msgReader, fieldType, ref fieldValue))
                                {
                                    if (i == 0)
                                        headContent.AppendFormat("<td align=\"center\"><strong>{0}</strong>({1})</td>",
                                                                 fieldName, fieldType);
                                    builderContent.AppendFormat("<td align=\"center\">&nbsp;{0}</td>", (fieldValue ?? "").Replace("{", "%7B").Replace("}", "%7D"));
                                    columnNum++;
                                }
                                if (fieldType == FieldType.Record)
                                {
                                    loopDepth++;
                                    recordQueue.Add(record);
                                }
                            }
                            else if (fieldType != FieldType.End)
                            {
                                if (fieldType == FieldType.Record)
                                {
                                    loopDepth++;
                                }
                                recordQueue.Add(record);
                            }
                        }
                        catch (Exception ex)
                        {
                            builderContent.AppendFormat("<td align=\"center\">{0}列出错{1}</td>", fieldName, ex.Message);
                        }

                    }

                    #endregion

                    headContent.Append("</tr><!--头结束tr-->");
                    builderContent.Append("</tr><!--内容结束tr-->");
                    //读取行结束
                    reader.RecordEnd();
                }
                catch (Exception ex)
                {
                    builderContent.AppendFormat("<tr><td align=\"left\">{0}行出错{1}</td></tr>", (i + 1), ex.Message);
                    break; //读流出错，直接退出
                }
            }

            respContent.Append(headContent.ToString());
            respContent.Append(builderContent.ToString());
            respContent.Append("</table><!--子表结束-->");
            respContent.Append("</td></tr>");
            respContent.Append("<tr>");
            respContent.Append("<td colspan=\"3\" align=\"left\">End</td>");
            respContent.Append("</tr>");
        }

    }
}