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
using System.Text;
using System.Web;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.RPC.IO;

namespace ContractTools.WebApp
{
    public partial class UnitTest : System.Web.UI.Page
    {
        public const int ErrorCode = 10000;
        private string GetCookesKey(string str)
        {
            return string.Format("{0}_{1}", SlnID, str);
        }
        public byte[] _buffer;

        private void SetCookies(string key, string value)
        {
            Response.Cookies[GetCookesKey(key)].Value = value;
            Response.Cookies[GetCookesKey(key)].Expires = DateTime.Now.AddMonths(1);
        }
        private string GetCookies(string key)
        {
            if (Request.Cookies[GetCookesKey(key)] != null)
            {
                return Request.Cookies[GetCookesKey(key)].Value;
            }
            else
            {
                return string.Empty;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.txtServerUrl.Text = GetCookies(string.Empty);
                SolutionModel solutionMode = DbDataLoader.GetSolution(SlnID);
                if (solutionMode != null)
                {
                    if (this.txtServerUrl.Text.Length == 0)
                    {
                        this.txtServerUrl.Text = solutionMode.Url;
                    }
                    string gameId = solutionMode.GameID.ToString();
                    gameId = string.IsNullOrEmpty(gameId) ? "0" : gameId;
                    this.lbtGmeID.Text = gameId;
                }
                BindContract();
            }
        }
        protected int SlnID
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["slnID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request.Params["slnID"]);
            }
        }
        protected int ContractID
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request.QueryString["ID"]);
            }
        }



        private void BindContract()
        {
            ddlContract.Items.Clear();
            var list = DbDataLoader.GetContract(SlnID);
            if (list.Count > 0)
            {
                ddlContract.DataSource = list;
                ddlContract.DataTextField = "uname";
                ddlContract.DataValueField = "ID";
                ddlContract.DataBind();
                ddlContract.SelectedValue = ContractID.ToString();
                ddlContract_SelectedIndexChanged(null, null);
            }
        }
        protected void btnTest_Click(object sender, EventArgs e)
        {
            DoRequest(false);
        }

        protected void socketBtn_Click(object sender, EventArgs e)
        {
            DoRequest(true);
        }

        private void DoRequest(bool isSocket)
        {
            SetCookies(ddlContract.SelectedValue, ParamListTextBox.Text);
            SetCookies(string.Empty, txtServerUrl.Text);

            try
            {
                string serverUrl = txtServerUrl.Text.Trim();
                string sid = txtSessionID.Text.Trim();
                string uid = txtUserID.Text.Trim();
                int contractId = ddlContract.Text.Trim().ToInt();
                string pid = txtPassport.Text.TrimEnd();
                string pwd = txtPassword.Text.TrimEnd();
                string respSID = "";
                string respUID = "";
                string responseStr = "";
                string requestParams = "";
                string[] contractList = txtMoreContrats.Text.Trim().Split(',');
                int gameId = lbtGmeID.Text.ToInt();
                int serverId = txtServerID.Text.ToInt();

                foreach (var tempId in contractList)
                {
                    int conId = tempId.Trim().ToInt();
                    if (conId == 0) continue;

                    requestParams = GetRequestParams(sid, uid, conId, SlnID, pid, pwd, gameId, serverId);
                    responseStr += PostGameServer(conId, SlnID, serverUrl, requestParams, isSocket, pid, out respSID, out respUID);
                    if (!string.IsNullOrEmpty(respSID))
                    {
                        txtSessionID.Text = respSID;
                        txtUserID.Text = respUID;
                        sid = respSID;
                        uid = respUID;
                    }
                }
                requestParams = GetRequestParams(sid, uid, contractId, SlnID, pid, pwd, gameId, serverId, ParamListTextBox.Text);
                LinkUrlLiteral.Text = requestParams;
                if (this.ckResponse.Checked)
                {
                    responseStr += PostGameServer(contractId, SlnID, serverUrl, requestParams, isSocket, pid, out respSID, out respUID);
                }
                else
                {
                    responseStr = PostGameServer(contractId, SlnID, serverUrl, requestParams, isSocket, pid, out respSID, out respUID);
                }
                if (!string.IsNullOrEmpty(respSID))
                {
                    txtSessionID.Text = respSID;
                    txtUserID.Text = respUID;
                }
                this.lblResponse.Text = responseStr;
            }
            catch (Exception ex)
            {
                lblResponse.Text = ex.ToString().Replace("\r\n", "<br>").Replace("\n", "<br>");
            }
        }

        private static string GetRequestParams(string sid, string uid, int contractId, int slnId, string pid, string pwd, int gameId, int serverId, string paramList = "")
        {
            string[] paramArray = null;
            if (paramList != null)
            {
                paramArray = paramList.Split(new char[] { ',', '，', ' ', ';' });
            }

            StringBuilder requestParams = new StringBuilder();
            requestParams.AppendFormat("MsgId=1&Sid={0}&Uid={1}&ActionID={2}", sid, uid, contractId);
            if (gameId > 0)
            {
                requestParams.AppendFormat("&GameType={0}", gameId);
            }
            if (serverId > 0)
            {
                requestParams.AppendFormat("&ServerID={0}", serverId);
            }
            int paramType = 1;
            var paramRecords = DbDataLoader.GetParamInfo(slnId, contractId, paramType);

            int i = 0;
            foreach (var record in paramRecords)
            {
                if (requestParams.Length > 0)
                {
                    requestParams.Append("&");
                }
                string fieldName = record.Field;
                string fieldValue = record.FieldValue;

                if (contractId == NetHelper.LoginActionId)
                {
                    if (fieldName.ToLower().Equals("pid"))
                    {
                        fieldValue = string.IsNullOrEmpty(pid)
                                         ? fieldValue
                                         : pid;
                    }
                    else if (fieldName.ToLower().Equals("pwd"))
                    {
                        fieldValue = string.IsNullOrEmpty(pwd)
                                         ? fieldValue
                                         : pwd;
                        fieldValue = new DESAlgorithmNew().EncodePwd(fieldValue, NetHelper.ClientDesDeKey);
                        fieldValue = HttpUtility.UrlEncode(fieldValue, Encoding.UTF8);
                    }
                }
                else
                {
                    if (paramArray != null && i < paramArray.Length && !string.IsNullOrEmpty(paramArray[i]))
                    {
                        fieldValue = paramArray[i];
                    }
                }
                requestParams.AppendFormat("{0}={1}", fieldName, fieldValue);
                i++;
            }
            return requestParams.ToString();
        }

        private static List<ParamInfoModel> GetResponseFields(int contractId, int slnId)
        {
            int paramType = 2;
            return DbDataLoader.GetParamInfo(slnId, contractId, paramType);
        }


        private static string PostGameServer(int contractId, int slnId, string serverUrl, string requestParams, bool isSocket, string pid, out string sid, out string uid)
        {
            sid = "";
            uid = "";
            StringBuilder respContent = new StringBuilder();
            MessageHead msg = new MessageHead();
            MessageStructure msgReader = NetHelper.Create(serverUrl, requestParams, out msg, isSocket, contractId, pid);
            if (msgReader != null)
            {
                ProcessResult(contractId, slnId, respContent, msg, msgReader, out sid, out uid);
            }
            else
            {
                ResponseHead(contractId, respContent, ErrorCode, "请求超时");
            }
            return respContent.ToString();
        }

        private static void ProcessResult(int contractId, int slnId, StringBuilder respContent, MessageHead msg, MessageStructure msgReader, out string sid, out string uid)
        {
            sid = "";
            uid = "";
            ResponseHead(contractId, respContent, msg.ErrorCode, msg.ErrorInfo);


            if (msg.ErrorCode != ErrorCode)
            {
                var respRecords = GetResponseFields(contractId, slnId);

                //消息体
                respContent.AppendFormat("<h3>{0}-{1}</h3>", contractId, "返回结果");
                respContent.Append("<table style=\"width:90%; border-color:#999\" border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
                respContent.Append("<tr><td style=\"width:15%;\"><strong>参数</strong></td><td style=\"width:10%;\"><strong>类型</strong></td>");
                respContent.Append("<td style=\"width:75%;\"><strong>参数值</strong></td></tr>");
                int loopDepth = 0;//循环深度
                List<ParamInfoModel> recordQueue = new List<ParamInfoModel>();
                #region 循环体
                foreach (var record in respRecords)
                {
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
            respContent.AppendFormat("<h3>{0}-{1}</h3>", contractId, "基本消息");
            respContent.Append("<table style=\"width:90%; border-color:#999\" border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
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
            respContent.Append("<!--子表开始--><table style=\"width:98%; border-color:#999\" border=\"1\" cellpadding=\"2\" cellspacing=\"0\">");
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
                                    "<!--子表开始--><table style=\"width:95%; border-color:#999\" border=\"1\" cellpadding=\"2\" cellspacing=\"0\">");
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
                                    builderContent.AppendFormat("<td align=\"center\">&nbsp;{0}</td>", fieldValue);
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

        protected void ddlContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParamListTextBox.Text = GetCookies(ddlContract.SelectedValue);
        }

        protected void ddlServerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParamListTextBox.Text = GetCookies(ddlContract.SelectedValue);
        }
    }
}