using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using BLL;
using model;

namespace ZyGames.ContractTools
{
    public partial class UnitTest : System.Web.UI.Page
    {
        public const int ErrorCode = 10000;
        private static bool IsSocket = false;
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
                if (this.txtServerUrl.Text.Length == 0)
                {
                    this.txtServerUrl.Text = new SolutionBLL().GetUrl(SlnID);
                }
                SolutionModel solutionModel = new SolutionModel();
                string gameId = new SolutionBLL().GetGameID(SlnID);
                gameId = string.IsNullOrEmpty(gameId) ? "0" : gameId;
                this.lbtGmeID.Text = gameId;

                ddlServerID.Items.Clear();
                DataSet ds = new SolutionBLL().GetServerList(" GameID=" + lbtGmeID.Text);
                ddlServerID.DataSource = ds;
                ddlServerID.DataTextField = "ServerName";
                ddlServerID.DataValueField = "ID";
                ddlServerID.DataBind();
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
            ContractBLL BLL = new ContractBLL();
            DataSet ds = BLL.GetList("SlnID=" + SlnID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlContract.DataSource = ds;
                ddlContract.DataTextField = "uname";
                ddlContract.DataValueField = "ID";
                ddlContract.DataBind();
                ddlContract.SelectedValue = ContractID.ToString();
                ddlContract_SelectedIndexChanged(null, null);
            }
        }
        protected void btnTest_Click(object sender, EventArgs e)
        {
            SetCookies(ddlContract.SelectedValue, ParamListTextBox.Text);
            SetCookies(string.Empty, txtServerUrl.Text);
            IsSocket = false;
            try
            {
                if (string.IsNullOrEmpty(txtPassport.Text.TrimEnd()))
                {
                    Response.Write("<script language=javascript>alert('PassportID为空！')</script>");
                    return;
                }
                if (string.IsNullOrEmpty(txtPassword.Text.TrimEnd()))
                {
                    Response.Write("<script language=javascript>alert('Password为空！')</script>");
                    return;
                }
                string serverUrl = txtServerUrl.Text.Trim();
                string sid = txtSessionID.Text.Trim();
                string uid = txtUserID.Text.Trim();
                string contractId = ddlContract.Text;
                string pid = txtPassport.Text.TrimEnd();
                string pwd = txtPassword.Text.TrimEnd();
                string respSID = "";
                string respUID = "";
                string responseStr = "";
                string requestParams = "";
                string[] contractList = txtMoreContrats.Text.Trim().Split(',');

                foreach (var tempId in contractList)
                {
                    if (string.IsNullOrEmpty(tempId)) continue;

                    requestParams = GetRequestParams(sid, uid, tempId, SlnID, pid, pwd, null);
                    responseStr += PostGameServer(tempId, SlnID, serverUrl, requestParams, out respSID, out respUID);
                    if (!string.IsNullOrEmpty(respSID))
                    {
                        txtSessionID.Text = respSID;
                        txtUserID.Text = respUID;
                        sid = respSID;
                        uid = respUID;
                    }
                }
                requestParams = GetRequestParams(sid, uid, contractId, SlnID, pid, pwd, ParamListTextBox.Text);
                LinkUrlLiteral.Text = requestParams;
                if (this.ckResponse.Checked)
                {
                    responseStr += PostGameServer(contractId, SlnID, serverUrl, requestParams, out respSID, out respUID);
                }
                else
                {
                    responseStr = PostGameServer(contractId, SlnID, serverUrl, requestParams, out respSID, out respUID);
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
        protected void socketBtn_Click(object sender, EventArgs e)
        {
            SetCookies(ddlContract.SelectedValue, ParamListTextBox.Text);
            SetCookies(string.Empty, txtServerUrl.Text);
            try
            {
                IsSocket = true;
                if (string.IsNullOrEmpty(txtPassport.Text.TrimEnd()))
                {
                    Response.Write("<script language=javascript>alert('PassportID为空！')</script>");
                    return;
                }
                if (string.IsNullOrEmpty(txtPassword.Text.TrimEnd()))
                {
                    Response.Write("<script language=javascript>alert('Password为空！')</script>");
                    return;
                }
                string serverUrl = txtServerUrl.Text.Trim();
                string sid = txtSessionID.Text.Trim();
                string uid = txtUserID.Text.Trim();
                string contractId = ddlContract.Text;
                string pid = txtPassport.Text.TrimEnd();
                string pwd = txtPassword.Text.TrimEnd();
                string respSID = "";
                string respUID = "";
                string responseStr = "";
                string requestParams = "";
                string[] contractList = txtMoreContrats.Text.Trim().Split(',');

                foreach (var tempId in contractList)
                {
                    if (string.IsNullOrEmpty(tempId)) continue;

                    requestParams = GetRequestParams(sid, uid, tempId, SlnID, pid, pwd, null);
                    requestParams += string.Format("&MsgId=1&GameType={0}&ServerID={1}", lbtGmeID.Text, ddlServerID.SelectedValue);
                    responseStr += PostGameServer(tempId, SlnID, serverUrl, requestParams, out respSID, out respUID);
                    if (!string.IsNullOrEmpty(respSID))
                    {
                        txtSessionID.Text = respSID;
                        txtUserID.Text = respUID;
                        sid = respSID;
                        uid = respUID;
                    }
                }
                requestParams = GetRequestParams(sid, uid, contractId, SlnID, pid, pwd, ParamListTextBox.Text);
                //requestParams += string.Format("&GameType={0}&ServerID={1}", lbtGmeID.Text, ddlServerID.SelectedValue);
                LinkUrlLiteral.Text = requestParams;
                if (this.ckResponse.Checked)
                {
                    requestParams += string.Format("&MsgId=1&GameType={0}&ServerID={1}", lbtGmeID.Text, ddlServerID.SelectedValue);
                    responseStr += PostGameServer(contractId, SlnID, serverUrl, requestParams, out respSID, out respUID);
                }
                else
                {
                    requestParams += string.Format("&MsgId=1&GameType={0}&ServerID={1}", lbtGmeID.Text, ddlServerID.SelectedValue);
                    responseStr = PostGameServer(contractId, SlnID, serverUrl, requestParams, out respSID, out respUID);
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


        private static string GetRequestParams(string sid, string uid, string contractId, int slnId, string pid, string pwd, string paramList)
        {
            string[] paramArray = null;
            if (paramList != null)
            {
                paramArray = paramList.Split(new char[] { ',', '，', ' ', ';' });
            }

            StringBuilder requestParams = new StringBuilder();
            requestParams.AppendFormat("Sid={0}&Uid={1}&ActionID={2}", sid, uid, contractId);

            DataSet reqParamList = new ParamInfoBLL().GetList(string.Format("ContractID={0} and SlnID={1} and ParamType=1", contractId, slnId));
            DataRowCollection paramRecords = reqParamList.Tables[0].Rows;

            int i = 0;
            foreach (DataRow record in paramRecords)
            {
                if (requestParams.Length > 0)
                {
                    requestParams.Append("&");
                }
                string fieldName = record["Field"].ToString();
                string fieldValue = record["FieldValue"].ToString();

                if (contractId == "1004")
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
                        fieldValue = new ZyGames.DesSecurity.DESAlgorithmNew().EncodePwd(fieldValue, "n7=7=7dk");
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

        private static DataRowCollection GetResponseFields(string contractId, int slnId)
        {
            DataSet respParamList = new ParamInfoBLL().GetList(string.Format("ContractID={0} and SlnID={1} and ParamType=2", contractId, slnId));
            DataRowCollection respRecords = respParamList.Tables[0].Rows;
            return respRecords;
        }


        private static string PostGameServer(string contractId, int slnId, string serverUrl, string requestParams, out string sid, out string uid)
        {
            sid = "";
            uid = "";
            StringBuilder respContent = new StringBuilder();
            Message msg = new Message();
            MessageReader msgReader = MessageReader.Create(serverUrl, requestParams,ref msg, IsSocket);
            if (msgReader != null)
            {
                try
                {
                    ProcessResult(contractId, slnId, respContent, msg, msgReader, out sid, out uid);
                }
                finally
                {
                    msgReader.Dispose();
                }
            }
            return respContent.ToString();
        }

        private static void ProcessResult(string contractId, int slnId, StringBuilder respContent, Message msg, MessageReader msgReader, out string sid, out string uid)
        {
            sid = "";
            uid = "";
            //头部消息
            respContent.AppendFormat("<h3>{0}-{1}</h3>", contractId, "基本消息");
            respContent.Append("<table style=\"width:90%; border-color:#999\" border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
            respContent.Append("<tr><td style=\"width:25%;\"><strong>状态值</strong></td>");
            respContent.Append("<td style=\"width:75%;\"><strong>描述</strong></td></tr>");
            respContent.AppendFormat("<tr><td>{0}</td>", msg.ErrorCode);
            respContent.AppendFormat("<td>{0}&nbsp;</td></tr>", msg.ErrorInfo);
            respContent.Append("</table>");


            if (msg.ErrorCode != ErrorCode)
            {
                DataRowCollection respRecords = GetResponseFields(contractId, slnId);

                //消息体
                respContent.AppendFormat("<h3>{0}-{1}</h3>", contractId, "返回结果");
                respContent.Append("<table style=\"width:90%; border-color:#999\" border=\"1\" cellpadding=\"3\" cellspacing=\"0\">");
                respContent.Append("<tr><td style=\"width:15%;\"><strong>参数</strong></td><td style=\"width:10%;\"><strong>类型</strong></td>");
                respContent.Append("<td style=\"width:75%;\"><strong>参数值</strong></td></tr>");
                int loopDepth = 0;//循环深度
                List<DataRow> recordQueue = new List<DataRow>();
                #region 循环体
                foreach (DataRow record in respRecords)
                {
                    string fieldName = record["Field"].ToString();
                    FieldType fieldType = (FieldType)Enum.Parse(typeof(FieldType), record["FieldType"].ToString());
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
                            if (msgReader.GetFieldValue(fieldType, ref fieldValue))
                            {
                                //自动登录
                                if ("1004".Equals(contractId))
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

        /// <summary>
        /// 处理循环记录
        /// </summary>
        /// <param name="respContent"></param>
        /// <param name="recordQueue"></param>
        /// <param name="msgReader"></param>
        private static void ProcessLoopRocord(StringBuilder respContent, List<DataRow> queue, MessageReader reader)
        {
            StringBuilder headContent = new StringBuilder();
            StringBuilder builderContent = new StringBuilder();
            int recordCount = 0;
            try
            {
                recordCount = reader.RecordCount();
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
                    MessageReader msgReader = reader;
                    int loopDepth = 0; //循环深度
                    List<DataRow> recordQueue = new List<DataRow>();

                    headContent.Append("<tr><!--头开始tr-->");
                    builderContent.Append("<tr><!--内容开始tr-->");
                    int columnNum = 0;

                    #region

                    for (int r = 1; r < queue.Count - 1; r++)
                    {
                        DataRow record = queue[r];
                        string fieldName = record["Field"].ToString();
                        FieldType fieldType = (FieldType)Enum.Parse(typeof(FieldType), record["FieldType"].ToString());
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
                                if (msgReader.GetFieldValue(fieldType, ref fieldValue))
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
