using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ContractTools.WebApp
{
    public partial class Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
            else
            {
                var target = Request["__EVENTTARGET"];
                if ("btnSerach".Equals(target))
                {
                    var argument = Request["txtSearch"];
                    btnSerach_Click(argument);
                }
            }
        }

        #region param
        public bool IsEdit
        {
            get { return !string.IsNullOrEmpty(Request["edit"]); }
        }

        protected int VerID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["VerID"]))
                {
                    string val = GetCookies(ddlSolution.Text + "_Ver");
                    int verId;
                    if (int.TryParse(val, out verId))
                    {
                        return verId;
                    }
                    return 0;
                }

                SetCookies(ddlSolution.Text + "_Ver", Request["VerID"]);
                return Convert.ToInt32(Request["VerID"]);
            }
        }

        protected int AgreementID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["AgreementID"]))
                {
                    string val = GetCookies(ddlSolution.Text + "_Agreement");
                    int verId;
                    if (int.TryParse(val, out verId))
                    {
                        return verId;
                    }
                    return 0;
                }

                SetCookies(ddlSolution.Text + "_Agreement", Request["AgreementID"]);
                return Convert.ToInt32(Request["AgreementID"]);
            }
        }

        protected string ContractID
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    if (string.IsNullOrEmpty(GetCookies(ddlSolution.Text)))
                    {
                        return "0";
                    }
                    else
                    {
                        return GetCookies(ddlSolution.Text);
                    }
                }
                else
                {
                    SetCookies(ddlSolution.Text, Request.QueryString["ID"]);
                    return Request.QueryString["ID"];
                }
            }
        }

        protected string SlnID
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["slnID"]))
                {
                    if (string.IsNullOrEmpty(GetCookies(string.Empty)))
                    {
                        return "0";
                    }
                    else
                    {
                        return GetCookies(string.Empty);
                    }
                }
                else
                {
                    SetCookies(string.Empty, Request.QueryString["slnID"]);
                    return Request.QueryString["slnID"];
                }
            }
        }
        #endregion

        #region bind data

        public void Bind()
        {
            try
            {
                int slnId = BindSolution(SlnID.ToInt());
                BindEnumInfo(slnId);
                int verId = BindVersion(slnId, VerID);
                int agreementId = BindAgreement(slnId, AgreementID);
                int contractId = BindContract(slnId, verId, agreementId, ContractID.ToInt());
                BindResult(slnId, verId, contractId);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Default bind error:{0}", ex);
            }
        }

        private int BindSolution(int slnId)
        {
            var list = DbDataLoader.GetSolution();
            ddlSolution.DataSource = list;
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();
            ddlSolution.SelectedValue = slnId.ToString();

            SolutionModel model = list.Find(t => t.SlnID == slnId);
            if (model != null)
            {
                ddServerCodeType.Text = model.SerUseScript;
                ddClientCodeType.Text = model.CliUseScript;
            }
            return ddlSolution.SelectedValue.ToInt();
        }
        private int BindAgreement(int slnId, int agreementId)
        {
            ddlAgreement.DataSource = DbDataLoader.GetAgreement(slnId);
            ddlAgreement.DataTextField = "Title";
            ddlAgreement.DataValueField = "AgreementID";
            ddlAgreement.DataBind();
            ddlAgreement.Items.Insert(0, new ListItem("All", "0"));
            ddlAgreement.SelectedValue = agreementId.ToString();
            return ddlAgreement.SelectedValue.ToInt();
        }
        private int BindVersion(int slnId, int verId)
        {
            ddVersion.DataSource = DbDataLoader.GetVersion(slnId);
            ddVersion.DataTextField = "Title";
            ddVersion.DataValueField = "ID";
            ddVersion.DataBind();
            ddVersion.Items.Insert(0, new ListItem("None", "0"));
            ddVersion.SelectedValue = verId.ToString();
            return ddVersion.SelectedValue.ToInt();
        }

        private int BindContract(int slnId, int versionId, int agreementId, int contractId)
        {
            ddContract.DataSource = DbDataLoader.GetContractByAgreement(slnId, agreementId, versionId);
            ddContract.DataTextField = "Uname";
            ddContract.DataValueField = "ID";
            ddContract.DataBind();
            if (contractId > 0)
            {
                ddContract.SelectedValue = contractId.ToString();
            }
            return ddContract.SelectedValue.ToInt();
        }

        private void BindFieldType(int paramtype)
        {
            var data = paramtype - 1 < FieldTypeMaps.Length ? FieldTypeMaps[paramtype - 1] : FieldTypeMaps[0];
            ddFieldType.DataSource = data;
            ddFieldType.DataTextField = "Value";
            ddFieldType.DataValueField = "Key";
            ddFieldType.DataBind();
            if (data.Length > 0)
            {
                ddFieldType.SelectedValue = data.Length > 1 ? "1" : "0";
            }
            List<ParamInfoModel> requestParams;
            List<ParamInfoModel> responseParams;
            GetParamInfo(ddlSolution.Text.ToInt(), ddContract.Text.ToInt(), ddVersion.Text.ToInt(), out requestParams, out responseParams);
            BindResponseParams(paramtype == 1 ? requestParams : responseParams);
        }

        private void BindResponseParams(List<ParamInfoModel> list)
        {
            ddResponseParams.DataSource = list;
            ddResponseParams.DataTextField = "ComboxDescp";
            ddResponseParams.DataValueField = "SortID";
            ddResponseParams.DataBind();
            if (list.Count > 0)
            {
                ddResponseParams.Items.Insert(0, new ListItem("<First>", "0"));
                ddResponseParams.SelectedValue = (list[list.Count - 1].SortID).ToString();
            }

            ddParamCopyFrom.DataSource = list;
            ddParamCopyFrom.DataTextField = "ComboxDescp";
            ddParamCopyFrom.DataValueField = "SortID";
            ddParamCopyFrom.DataBind();

            ddParamCopyTo.DataSource = list;
            ddParamCopyTo.DataTextField = "ComboxDescp";
            ddParamCopyTo.DataValueField = "SortID";
            ddParamCopyTo.DataBind();
        }

        private void BindGrid()
        {
            BindGrid(ddlSolution.Text.ToInt(), ddVersion.Text.ToInt(), ddContract.Text.ToInt());
        }

        private void BindGrid(int slnId, int versionId, int contractId)
        {
            //grid bind

            List<ParamInfoModel> requestParams;
            List<ParamInfoModel> responseParams;
            GetParamInfo(slnId, contractId, versionId, out requestParams, out responseParams);

            BindResponseParams(ddParamType.Text.ToInt() == 1 ? requestParams : responseParams);

            bool isEdit = IsEdit;
            gvReqParams.Columns[gvReqParams.Columns.Count - 1].Visible = isEdit;
            gvReqParams.Columns[gvReqParams.Columns.Count - 2].Visible = isEdit;
            gvReqParams.DataKeyNames = new[] { "ID", "ParamType" };
            gvReqParams.DataSource = requestParams;
            gvReqParams.DataBind();

            gvRespParams.Columns[gvRespParams.Columns.Count - 1].Visible = isEdit;
            gvRespParams.Columns[gvRespParams.Columns.Count - 2].Visible = isEdit;
            gvRespParams.Columns[gvRespParams.Columns.Count - 3].Visible = isEdit;
            gvRespParams.DataKeyNames = new[] { "ID", "ParamType" };
            gvRespParams.DataSource = responseParams;
            gvRespParams.DataBind();

            BindSourceCode(slnId, versionId, contractId, requestParams, responseParams);

        }

        private void BindSourceCode(int slnId, int versionId, int contractId, List<ParamInfoModel> requestParams, List<ParamInfoModel> responseParams)
        {
            try
            {
                var modol = DbDataLoader.GetContract(slnId, contractId, 0);
                var tileName = GetTileName(contractId, modol != null ? modol.Descption : null);
                bool isSelfAction = ckSelfAction.Checked;

                string clientTemp = string.Empty;
                string serverTemp = string.Empty;

                if (ddClientCodeType.Text == "Lua")
                {
                    clientTemp = Path.Combine(Server.MapPath("~"), "Template/ClientLuaCode.txt");
                    txtClientCode.Text = TemplateHelper.FromatClientLuaTemp(TemplateHelper.ReadTemp(clientTemp), contractId, responseParams, requestParams, tileName);

                }
                else if (ddClientCodeType.Text == "C#")
                {
                    clientTemp = Path.Combine(Server.MapPath("~"),
                        isSelfAction ? "Template/ClientCsharpSelfCode.txt" : "Template/ClientCsharpCode.txt");
                    txtClientCode.Text = TemplateHelper.FromatClientCsharpTemp(TemplateHelper.ReadTemp(clientTemp),
                        contractId, responseParams, requestParams, tileName);
                }
                else if (ddClientCodeType.Text == "Quick")
                {
                    var clientSendTemp = Path.Combine(Server.MapPath("~"), "Template/ClientQuickCode-S.txt");
                    var clientReceiveTemp = Path.Combine(Server.MapPath("~"), "Template/ClientQuickCode-R.txt");
                    var codeBuild = new StringBuilder();
                    codeBuild.AppendLine(TemplateHelper.FromatClientQuickSendTemp(TemplateHelper.ReadTemp(clientSendTemp), contractId, responseParams, requestParams, tileName));
                    codeBuild.AppendLine(TemplateHelper.FromatClientQuickReceiveTemp(TemplateHelper.ReadTemp(clientReceiveTemp), contractId, responseParams, requestParams, tileName));
                    txtClientCode.Text = codeBuild.ToString();
                }
                else
                {
                    txtClientCode.Text = "Not supported code.";
                }


                var slnRecord = DbDataLoader.GetSolution(slnId);
                if (ddServerCodeType.Text == "C#")
                {
                    serverTemp = Path.Combine(Server.MapPath("~"), isSelfAction ? "Template/ServerCsharpSelfCode.txt" : "Template/ServerCsharpCode.txt");
                    txtServerCode.Text = TemplateHelper.FormatTemp(TemplateHelper.ReadTemp(serverTemp), contractId, responseParams, requestParams, slnRecord, tileName);
                }
                else if (ddServerCodeType.Text == "Python")
                {
                    serverTemp = Path.Combine(Server.MapPath("~"), "Template/ServerPythonCode.txt");
                    txtServerCode.Text = TemplateHelper.FormatPython(TemplateHelper.ReadTemp(serverTemp), responseParams, requestParams, slnRecord, tileName);
                }
                else if (ddServerCodeType.Text == "Lua")
                {
                    //todo not
                    serverTemp = Path.Combine(Server.MapPath("~"), "Template/ServerLuaCode.txt");
                    txtServerCode.Text = TemplateHelper.FormatLua(TemplateHelper.ReadTemp(serverTemp), contractId, responseParams, requestParams, slnRecord, tileName);
                }
                else
                {
                    txtServerCode.Text = "Not supported code.";
                }

            }
            catch (Exception ex)
            {
                txtServerCode.Text = ex.ToString();
            }
        }

        private static string GetTileName(int contractId, string descption)
        {
            string tileName = descption ?? "Action " + contractId;
            int index = tileName.IndexOf("【", System.StringComparison.Ordinal);
            if (index != -1)
            {
                tileName = tileName.Substring(0, index);
            }
            return tileName;
        }

        private void BindResult()
        {
            BindResult(ddlSolution.Text.ToInt(), ddVersion.Text.ToInt(), ddContract.Text.ToInt());
        }

        private string GetUrl()
        {
            int slnId = ddlSolution.Text.ToInt();
            int versionId = ddVersion.Text.ToInt();
            int contractId = ddContract.Text.ToInt();
            return "Default.aspx?" + (!IsEdit ? "" : "edit=true&") + string.Format("ID={0}&slnID={1}&VerID={2}&GameID={1}", contractId, slnId, versionId);
        }
        private void BindResult(int slnId, int versionId, int contractId)
        {
            hlTopEdit.NavigateUrl = "Default.aspx" + (IsEdit ? "" : "?edit=true");
            string get = string.Format("?ID={0}&slnID={1}&VerID={2}&GameID={1}", contractId, slnId, versionId);
            hlSolution.NavigateUrl = "Solutions.aspx";
            hlVersion.NavigateUrl = "VersionEdit.aspx" + get;
            hlEnum.NavigateUrl = "EnumEdit.aspx" + get;
            hlAgreement.NavigateUrl = "AgreementEdit.aspx" + get;
            hlContract.NavigateUrl = "ContractEdit.aspx" + get + "&AgreementID=" + ddlAgreement.Text;
            hlContractEdit.NavigateUrl = "ContractEdit.aspx" + get + "&modify=true";
            hlContractCopy.NavigateUrl = "ContractCopy.aspx" + get;

            ifrTest.Src = "ContractDebug.aspx" + get;
            //ifrClientConfig.Src = "ClientConfigInfo.aspx" + get;

            BindFieldType(ddParamType.SelectedValue.ToInt());
            BindGrid(slnId, versionId, contractId);

        }

        private void BindEnumInfo(int slnId)
        {
            //enum bind
            StringBuilder enumBuilder = new StringBuilder();
            var list = DbDataLoader.GetEnumInfo(slnId);
            if (list.Count == 0)
            {
                enumBuilder.AppendFormat("No enum value.");
            }
            foreach (var dr in list)
            {
                enumBuilder.AppendFormat("<div id=\"tabEnum_{0}\">【{0}】－{1}<br>", dr.enumName, dr.enumDescription);
                enumBuilder.AppendFormat("┗{0}<br><br></div>", dr.enumValueInfo.Replace("\r\n", "<br>┗").Replace("\n", "<br>┗").TrimEnd('┗'));
            }
            lblEnumDescp.InnerHtml = enumBuilder.ToString();
        }

        private string FormatTips(string text)
        {
            int i, j;
            int slnid = 0;
            try
            {
                slnid = int.Parse(ddlSolution.Text);
            }
            catch
            {
            }
            Hashtable ht = TemplateHelper.LoadEnumApplication(slnid, false);
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                i = text.IndexOfAny(new char[] { '[', '【' });
                if (i != -1)
                {
                    j = text.IndexOfAny(new char[] { ']', '】' });
                    if (j != -1)
                    {
                        sb.Append(text.Substring(0, i));
                        string innertext = text.Substring(i, j - i + 1);
                        text = text.Substring(j + 1);
                        innertext = innertext.TrimEnd().Replace('[', '【').Replace(']', '】');
                        if (ht.Contains(innertext))
                        {
                            string enumName = innertext.Substring(1, innertext.Length - 2);
                            string enumValuses = ht[innertext].ToString().Replace("\r", "").Replace("\n", "<br>");
                            sb.Append(string.Format("<a href='#tabEnum_{2}' onmouseover='ShowPrompt(event, \"{1}\")'>{0}</a>", innertext, enumValuses, enumName));
                        }
                        else
                        {
                            sb.Append(string.Format("<a href='#' onmouseover='ShowPrompt(event, \"{1}\")'>{0}</a>", innertext, "暂无此枚举信息"));
                        }
                    }
                    else
                    {
                        sb.Append(text);
                        break;
                    }
                }
                else
                {
                    sb.Append(text);
                    break;
                }
            }
            return sb.ToString();
        }
        #endregion

        #region event

        protected void OnSolutionChanged(object sender, EventArgs e)
        {
            try
            {
                SetCookies(string.Empty, ddlSolution.Text);
                int slnId = ddlSolution.Text.ToInt();
                BindEnumInfo(slnId);
                int verId = BindVersion(slnId, 0);
                int agreementId = BindAgreement(slnId, AgreementID);
                int contractId = BindContract(slnId, verId, agreementId, 0);
                BindResult(slnId, verId, contractId);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnSolutionChanged {0}", ex);
            }
        }
        protected void OnVersionChanged(object sender, EventArgs e)
        {
            try
            {
                SetCookies(ddlSolution.Text + "_Ver", ddVersion.Text);
                int slnId = ddlSolution.Text.ToInt();
                int verId = ddVersion.Text.ToInt();
                int agreementId = ddlAgreement.Text.ToInt();
                int contractId = BindContract(slnId, verId, agreementId, 0);
                BindResult(slnId, verId, contractId);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnVersionChanged {0}", ex);
            }
        }

        protected void OnParamTypeChanged(object sender, EventArgs e)
        {
            BindFieldType(ddParamType.SelectedValue.ToInt());
        }

        protected void OnAgreementChanged(object sender, EventArgs e)
        {
            SetCookies(ddlSolution.Text + "_Agreement", ddlAgreement.Text);
            int slnId = ddlSolution.Text.ToInt();
            int verId = ddVersion.Text.ToInt();
            int agreementId = ddlAgreement.Text.ToInt();
            int contractId = BindContract(slnId, verId, agreementId, 0);
            BindResult(slnId, verId, contractId);
        }
        protected void OnContractChanged(object sender, EventArgs e)
        {
            SetCookies(ddlSolution.Text, ddContract.Text);
            int slnId = ddlSolution.Text.ToInt();
            int verId = ddVersion.Text.ToInt();
            int contractId = ddContract.Text.ToInt();
            BindResult(slnId, verId, contractId);
        }

        protected void OnSelfActionChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void OnGridRowEditing(object sender, GridViewEditEventArgs e)
        {
            var gv = sender as GridView;
            if (gv == null) return;
            gv.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnGridRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gv = sender as GridView;
            if (gv == null) return;
            gv.EditIndex = -1;
            BindGrid();
        }

        protected void OnGridRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                var gv = sender as GridView;
                if (gv == null) return;
                var keys = gv.DataKeys[e.RowIndex];
                TableCell cell = gv.Rows[e.RowIndex].Cells[0];
                ParamInfoModel mode = new ParamInfoModel();
                mode.ID = keys.Value.ToInt();
                mode.ContractID = ddContract.Text.ToInt();
                mode.ParamType = keys.Values[1].ToInt();
                mode.Field = ((TextBox)cell.FindControl("txtField")).Text.Trim();
                mode.FieldType = ((DropDownList)cell.FindControl("droFieldType")).SelectedValue.ToEnum<FieldType>();
                mode.Remark = JoinArray(',', ((TextBox)cell.FindControl("hiDescption")).Text.Trim(),
                    ((TextBox)cell.FindControl("txtDescption")).Text.Trim());//合并到Remark字段
                mode.Descption = "";
                var fieldVlue = ((TextBox)cell.FindControl("txtFieldValue"));
                if (fieldVlue != null)
                {
                    mode.FieldValue = fieldVlue.Text.Trim();
                }
                var conRequired = (DropDownList)cell.FindControl("droRequired");
                if (conRequired != null)
                {
                    mode.Required = Convert.ToBoolean(conRequired.Text);
                }
                mode.SortID = -1;
                mode.ModifyDate = MathUtils.Now;
                if (DbDataLoader.Update(mode))
                {
                    gv.EditIndex = -1;
                    BindResult();
                }
            }

            catch (Exception erro)
            {
                TraceLog.WriteError("RowUpdating:{0}", erro);
                Response.Write("错误信息:" + erro.Message);
            }

        }

        private string JoinArray(char split, params string[] arr)
        {
            string str = string.Empty;
            foreach (var s in arr)
            {
                if (string.IsNullOrEmpty(s)) continue;
                str += s + split;
            }
            return str.TrimEnd(split);
        }

        protected void OnGridRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var control = (Label)e.Row.FindControl("LabFieldType");
                    if (control != null)
                    {
                        control.Text = control.Text.ToEnum<FieldType>().ToString();
                        var paramInfo = e.Row.DataItem as ParamInfoModel;//定义一个DataRowView的实例

                        if (paramInfo != null)
                        {
                            if (paramInfo.FieldType == FieldType.Record ||
                                paramInfo.FieldType == FieldType.SigleRecord ||
                                paramInfo.FieldType == FieldType.End)
                            {
                                e.Row.Font.Bold = true;
                            }
                            if (paramInfo.FieldType == FieldType.Void)
                            {
                                e.Row.Font.Strikeout = true;
                            }
                            if (paramInfo.VerID < ddVersion.Text.ToInt())
                            {
                                e.Row.CssClass = "grid-row-old";
                            }
                            if (DateTime.Now - paramInfo.ModifyDate < TimeSpan.FromDays(3))
                            {
                                e.Row.CssClass = "grid-row-change";
                            }
                        }
                    }
                    Label lblDescption = (Label)e.Row.FindControl("LabDescption");
                    if (lblDescption != null)
                    {
                        lblDescption.Text = FormatTips(lblDescption.Text);
                    }
                    Label lblRemark = (Label)e.Row.FindControl("LabRemark");
                    if (lblRemark != null)
                    {
                        lblRemark.Text = FormatTips(lblRemark.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("bind grid error:{0}", ex);
            }
        }


        protected void btnSortAsc_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split(new char[] { ',' });
            int currID = Convert.ToInt32(args[0]);
            int paramType = Convert.ToInt32(args[1]);
            int currSortID = Convert.ToInt32(args[2]);
            LinkButton btnSortAsc = (LinkButton)sender;
            GridViewRow gridRow = (GridViewRow)btnSortAsc.NamingContainer;
            int rowIndex = gridRow.RowIndex;
            int sortID = rowIndex + 1;
            if (sortID != currSortID)
            {
                sortID = currSortID;
            }
            if (rowIndex > 0)
            {
                var preRow = gvRespParams.DataKeys[rowIndex - 1];
                if (preRow == null) return;

                int preParamType = preRow.Values[1].ToInt();
                int preID = preRow.Values[0].ToInt();
                if (paramType == preParamType)
                {
                    DbDataLoader.UpdateParamSort(preID, sortID);
                    DbDataLoader.UpdateParamSort(currID, sortID - 1);
                    BindResult();
                }
            }
        }
        protected void btnSortDes_Command(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split(new char[] { ',' });
            int currID = Convert.ToInt32(args[0]);
            int paramType = Convert.ToInt32(args[1]);
            int currSortID = Convert.ToInt32(args[2]);
            LinkButton btnSortDes = (LinkButton)sender;
            GridViewRow gridRow = (GridViewRow)btnSortDes.NamingContainer;
            int rowIndex = gridRow.RowIndex;
            int sortID = rowIndex + 1;
            if (sortID != currSortID)
            {
                sortID = currSortID;
            }
            if (rowIndex >= 0)
            {
                if (rowIndex >= gvRespParams.Rows.Count - 1) return;

                var nextRow = gvRespParams.DataKeys[rowIndex + 1];
                if (nextRow == null) return;
                int preParamType = nextRow.Values[1].ToInt();
                int nextID = nextRow.Values[0].ToInt();
                if (paramType == preParamType)
                {
                    DbDataLoader.UpdateParamSort(nextID, sortID);
                    DbDataLoader.UpdateParamSort(currID, sortID + 1);
                    BindResult();
                }
            }

        }

        protected void btnRecordSortAsc_Command(object sender, CommandEventArgs e)
        {
            //当前不是Record或End, 移动到上一个Record前
            string[] args = e.CommandArgument.ToString().Split(new char[] { ',' });
            int currID = Convert.ToInt32(args[0]);
            int fieldType = Convert.ToInt32(args[1]);
            int currSortID = Convert.ToInt32(args[2]);
            if (fieldType == (int)FieldType.Record || fieldType == (int)FieldType.SigleRecord || fieldType == (int)FieldType.End)
            {
                return;
            }
            LinkButton btnSortAsc = (LinkButton)sender;
            GridViewRow gridRow = (GridViewRow)btnSortAsc.NamingContainer;
            int rowIndex = gridRow.RowIndex;
            int sortID = -1;
            var keyList = new List<int>();
            while (rowIndex > 0)
            {
                rowIndex--;
                var row = gvRespParams.Rows[rowIndex];
                var dataKey = gvRespParams.DataKeys[rowIndex];
                if (dataKey == null || dataKey.Values == null) return;
                int keyId = dataKey.Values[0].ToInt();
                keyList.Add(keyId);

                var input = row.FindControl("LabFieldType") as Label;
                if (input != null && (input.Text == FieldType.Record.ToString() || input.Text == FieldType.SigleRecord.ToString()))
                {
                    var t = row.FindControl("txtSortID") as Label;
                    sortID = t != null ? t.Text.ToInt() : rowIndex + 1;
                    break;
                }
            }
            if (sortID > -1)
            {
                DbDataLoader.UpdateParamSort(currID, sortID);
                for (int i = keyList.Count - 1; i >= 0; i--)
                {
                    sortID++;
                    DbDataLoader.UpdateParamSort(keyList[i], sortID);
                }
                BindResult();
            }
        }

        protected void btnRecordSortDes_Command(object sender, CommandEventArgs e)
        {
            //当前不是Record或End, 移动到下一个End后
            string[] args = e.CommandArgument.ToString().Split(new char[] { ',' });
            int currID = Convert.ToInt32(args[0]);
            int fieldType = Convert.ToInt32(args[1]);
            int currSortID = Convert.ToInt32(args[2]);
            if (fieldType == (int)FieldType.Record || fieldType == (int)FieldType.SigleRecord || fieldType == (int)FieldType.End)
            {
                return;
            }
            LinkButton btnSortAsc = (LinkButton)sender;
            GridViewRow gridRow = (GridViewRow)btnSortAsc.NamingContainer;
            int rowIndex = gridRow.RowIndex;
            int sortID = -1;
            var keyList = new List<int>();
            while (rowIndex < gvRespParams.Rows.Count - 1)
            {
                rowIndex++;
                var row = gvRespParams.Rows[rowIndex];
                var dataKey = gvRespParams.DataKeys[rowIndex];
                if (dataKey == null || dataKey.Values == null) return;
                int keyId = dataKey.Values[0].ToInt();
                keyList.Add(keyId);

                var input = row.FindControl("LabFieldType") as Label;
                if (input != null && input.Text == FieldType.End.ToString())
                {
                    var t = row.FindControl("txtSortID") as Label;
                    sortID = t != null ? t.Text.ToInt() : rowIndex + 1;
                    break;
                }
            }
            if (sortID < 0)
            {
                //未找到End，移动下结尾
                sortID = gvRespParams.Rows.Count - 1;
            }
            if (sortID > -1)
            {
                DbDataLoader.UpdateParamSort(currID, sortID);
                for (int i = keyList.Count - 1; i >= 0; i--)
                {
                    sortID--;
                    DbDataLoader.UpdateParamSort(keyList[i], sortID);
                }
                BindResult();
            }
        }
        protected void OnRespGridDelete(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            if (DbDataLoader.Delete(new ParamInfoModel() { ID = id }))
            {
                Alert("删除成功！", GetUrl());
            }
            else
            {
                Alert("删除失败！", GetUrl());
            }
            BindResult();
        }

        protected void OnNoCompaltedClick(object sender, EventArgs e)
        {
            UpdateStatus(false);
        }

        protected void OnCompaltedClick(object sender, EventArgs e)
        {
            UpdateStatus(true);
        }

        private void UpdateStatus(bool complated)
        {
            if (DbDataLoader.UpdateContractStatus(ddContract.Text.ToInt(), ddlSolution.Text.ToInt(), complated))
            {
                Bind();
            }
        }


        protected void btnParamCopy_Click(object sender, EventArgs e)
        {
            try
            {
                int sortFrom = ddParamCopyFrom.Text.ToInt();
                int sortTo = ddParamCopyTo.Text.ToInt();
                if (sortFrom > sortTo) return;

                int insertPos = ddResponseParams.Text.ToInt();
                int slnId = ddlSolution.Text.ToInt();
                int verId = ddVersion.Text.ToInt();
                int contractId = ddContract.Text.ToInt();
                int paramType = ddParamType.Text.ToInt();

                var paramList = DbDataLoader.GetParamInfo(slnId, contractId, paramType, verId);
                var copyList = paramList.FindAll(t => t.SortID >= sortFrom && t.SortID <= sortTo);
                int sortId = insertPos + copyList.Count;

                foreach (var param in paramList)
                {
                    if (param.SortID > insertPos)
                    {
                        sortId++;
                        DbDataLoader.UpdateParamSort(param.ID, sortId);
                    }
                }
                sortId = insertPos;
                foreach (var param in copyList)
                {
                    sortId++;
                    param.SortID = sortId;
                    param.VerID = verId;
                    param.ModifyDate = DateTime.MinValue;
                    param.CreateDate = DateTime.Now;
                    DbDataLoader.Add(param);
                }

                BindGrid(slnId, verId, contractId);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Default ParamCopy error:{0}", ex);
            }
        }

        protected void btnSerach_Click(string argument)
        {
            try
            {
                int slnId = ddlSolution.Text.ToInt();
                int verId = ddVersion.Text.ToInt();
                int contractId = ddContract.Text.ToInt();
                var list = DbDataLoader.GetContract(slnId, argument);
                ddContract.DataSource = list;
                ddContract.DataTextField = "Uname";
                ddContract.DataValueField = "ID";
                ddContract.DataBind();
                if (list.Count > 0)
                {
                    contractId = list[0].ID;
                }
                BindResult(slnId, verId, contractId);

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnSolutionChanged {0}", ex);
            }
        }

        protected void btnParamAdd_Click(object sender, EventArgs e)
        {
            try
            {

                ParamInfoModel mode = new ParamInfoModel();
                mode.Field = txtField.Text.Trim();
                mode.FieldValue = "";
                mode.Remark = txtRemark.Text.Trim();
                mode.ContractID = ddContract.Text.ToInt();
                mode.FieldType = ddFieldType.Text.ToEnum<FieldType>();
                mode.ParamType = ddParamType.Text.ToInt();
                mode.Required = ckRequired.Checked;
                mode.Descption = "";
                mode.SlnID = ddlSolution.Text.ToInt();
                mode.MinValue = 0;
                mode.MaxValue = 0;
                mode.CreateDate = DateTime.Now;
                mode.VerID = ddVersion.Text.ToInt();

                var paramList = DbDataLoader.GetParamInfo(mode.SlnID, mode.ContractID, mode.ParamType, mode.VerID);
                int sortID = 0;
                if (!string.IsNullOrEmpty(ddResponseParams.Text))
                {
                    sortID = ddResponseParams.Text.ToInt() + 1;
                }
                else
                {
                    sortID = paramList.Count == 0 ? 1 : paramList.Max(t => t.SortID) + 1;
                }
                mode.SortID = sortID;

                foreach (var param in paramList)
                {
                    if (param.SortID >= mode.SortID)
                    {
                        sortID++;
                        DbDataLoader.UpdateParamSort(param.ID, sortID);
                    }
                }
                if (DbDataLoader.Add(mode) > 0)
                {
                    BindGrid(mode.SlnID, mode.VerID, mode.ContractID);
                    txtField.Text = "";
                    txtRemark.Text = "";
                }
                else
                {
                    Alert("增加失败！", GetUrl());
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Default ParamAdd error:{0}", ex);
            }
        }


        protected void btnParamRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int sortFrom = ddParamCopyFrom.Text.ToInt();
                int sortTo = ddParamCopyTo.Text.ToInt();
                if (sortFrom > sortTo) return;

                int slnId = ddlSolution.Text.ToInt();
                int verId = ddVersion.Text.ToInt();
                int contractId = ddContract.Text.ToInt();
                int paramType = ddParamType.Text.ToInt();

                var paramList = DbDataLoader.GetParamInfo(slnId, contractId, paramType, verId);
                var removeList = paramList.FindAll(t => t.SortID >= sortFrom && t.SortID <= sortTo);

                foreach (var param in removeList)
                {
                    DbDataLoader.Delete(param);
                }
                BindGrid(slnId, verId, contractId);
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Default Param Remove error:{0}", ex);
            }
        }

        protected void OnServerCodeTypeChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void OnClientCodeTypeChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void OnExportSererCode(object sender, EventArgs e)
        {
            if (ddServerCodeType.Text == "C#")
            {
                SaveAsAttachment(txtServerCode.Text, String.Format("Action{0}.cs", ddContract.Text), true);
            }
            else if (ddServerCodeType.Text == "Python")
            {
                SaveAsAttachment(txtServerCode.Text, String.Format("action{0}.py", ddContract.Text), true);
            }
            else if (ddServerCodeType.Text == "Lua")
            {
                SaveAsAttachment(txtServerCode.Text, String.Format("action{0}.lua", ddContract.Text), true);
            }
        }

        protected void OnExportClientCode(object sender, EventArgs e)
        {
            if (ddClientCodeType.Text == "Lua")
            {
                SaveAsAttachment(txtClientCode.Text, String.Format("Action{0}.lua", ddContract.Text));
            }
            else if (ddClientCodeType.Text == "Quick")
            {
                SaveAsAttachment(txtClientCode.Text, String.Format("Action{0}.lua", ddContract.Text));
            }
            else if (ddClientCodeType.Text == "C#")
            {
                SaveAsAttachment(txtClientCode.Text, String.Format("Action{0}.cs", ddContract.Text));
            }
        }

        protected void OnExportAllSererCode(object sender, EventArgs e)
        {
            try
            {
                string type = ddServerCodeType.Text;
                int slnId = ddlSolution.Text.ToInt();
                int agreementId = ddlAgreement.Text.ToInt();
                int versionId = ddVersion.Text.ToInt();
                bool isSelfAction = ckSelfAction.Checked;

                var slnRecord = DbDataLoader.GetSolution(slnId);
                List<ZipFileInfo> zipFileList = null;
                List<ContractModel> contractList = DbDataLoader.GetContractByAgreement(slnId, agreementId, versionId);

                string serverTemp = string.Empty;
                if (type == "C#")
                {
                    serverTemp = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"), isSelfAction ? "Template/ServerCsharpSelfCode.txt" : "Template/ServerCsharpCode.txt"));
                }
                else if (type == "Python")
                {
                    serverTemp = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"), "Template/ServerPythonCode.txt"));
                }
                else if (type == "Lua")
                {
                    serverTemp = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"), "Template/ServerLuaCode.txt"));
                }
                else
                {
                    return;
                }

                ZipFileInfo zipFile;
                zipFileList = new List<ZipFileInfo>();
                string fileExt;
                string content;

                foreach (var model in contractList)
                {
                    int contractId = model.ID;
                    var tileName = GetTileName(model.ID, model.Descption);
                    List<ParamInfoModel> requestParams;
                    List<ParamInfoModel> responseParams;
                    GetParamInfo(slnId, contractId, versionId, out requestParams, out responseParams);
                    if (type == "C#")
                    {
                        fileExt = ".cs";
                        content = TemplateHelper.FormatTemp(serverTemp, contractId, responseParams, requestParams, slnRecord, tileName);
                    }
                    else if (type == "Python")
                    {
                        fileExt = ".py";
                        content = TemplateHelper.FormatPython(serverTemp, responseParams, requestParams, slnRecord, tileName);
                    }
                    else if (type == "Lua")
                    {
                        fileExt = ".lua";
                        content = TemplateHelper.FormatLua(serverTemp, contractId, responseParams, requestParams, slnRecord, tileName);
                    }
                    else
                    {
                        break;
                    }
                    zipFile = new ZipFileInfo() { Name = string.Format("Action{0}{1}", contractId, fileExt) };
                    zipFile.Content = content;
                    zipFileList.Add(zipFile);
                }
                if (zipFileList.Count > 0)
                {
                    SaveAsAttachment(string.Format("{0}Action{1}.zip", type, DateTime.Now.ToString("HHmmss")), zipFileList, true);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnExportAllSererCode error:{0}", ex);
            }

        }

        protected void OnExportAllClientCode(object sender, EventArgs e)
        {
            try
            {
                string type = ddClientCodeType.Text;
                int slnId = ddlSolution.Text.ToInt();
                int agreementId = ddlAgreement.Text.ToInt();
                int versionId = ddVersion.Text.ToInt();
                bool isSelfAction = ckSelfAction.Checked;

                List<ZipFileInfo> zipFileList = null;
                List<ContractModel> contractList = DbDataLoader.GetContractByAgreement(slnId, agreementId, versionId);
                if (type == "Lua")
                {
                    var codeBuild = new StringBuilder("");
                    zipFileList = new List<ZipFileInfo>();
                    var clientTemp = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"), "Template/ClientLuaCode.txt"));
                    foreach (var model in contractList)
                    {
                        if (!model.Complated) continue;
                        int contractId = model.ID;
                        var tileName = GetTileName(model.ID, model.Descption);
                        List<ParamInfoModel> requestParams;
                        List<ParamInfoModel> responseParams;
                        GetParamInfo(slnId, contractId, versionId, out requestParams, out responseParams);
                        codeBuild.AppendLine(TemplateHelper.FromatClientLuaTemp(clientTemp, contractId, responseParams, requestParams, tileName));
                        codeBuild.AppendLine();
                    }
                    zipFileList.Add(new ZipFileInfo() { Name = "actionLayer.lua", Content = codeBuild.ToString() });
                }
                else if (type == "Quick")
                {
                    var sendCodeBuild = new StringBuilder("");
                    var receiveCodeBuild = new StringBuilder();
                    sendCodeBuild.AppendLine(@"
local Request = class(""Request"")
function Request:ctor()
    
end
");
                    receiveCodeBuild.AppendLine("local Response = {}");
                    receiveCodeBuild.AppendLine("Response.Success = 0");
                    var clientSendTemp = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"), "Template/ClientQuickCode-S.txt"));
                    var clientReceiveTemp = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"), "Template/ClientQuickCode-R.txt"));
                    foreach (var model in contractList)
                    {
                        if (!model.Complated) continue;
                        int contractId = model.ID;
                        var tileName = GetTileName(model.ID, model.Descption);
                        List<ParamInfoModel> requestParams;
                        List<ParamInfoModel> responseParams;
                        GetParamInfo(slnId, contractId, versionId, out requestParams, out responseParams);
                        sendCodeBuild.AppendLine(TemplateHelper.FromatClientQuickSendTemp(clientSendTemp, contractId, responseParams, requestParams, tileName));
                        receiveCodeBuild.AppendLine(TemplateHelper.FromatClientQuickReceiveTemp(clientReceiveTemp, contractId, responseParams, requestParams, tileName));

                    }
                    sendCodeBuild.AppendLine("return Request");
                    receiveCodeBuild.AppendLine("return Response");
                    zipFileList = new List<ZipFileInfo>();
                    zipFileList.Add(new ZipFileInfo() { Name = "Request.lua", Content = sendCodeBuild.ToString() });
                    zipFileList.Add(new ZipFileInfo() { Name = "Response.lua", Content = receiveCodeBuild.ToString() });
                }
                else if (type == "C#")
                {
                    var clientTemp = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"),
                          isSelfAction ? "Template/ClientCsharpSelfCode.txt" : "Template/ClientCsharpCode.txt"));
                    ZipFileInfo zipFile;
                    zipFileList = new List<ZipFileInfo>();

                    foreach (var model in contractList)
                    {
                        if (!model.Complated) continue;
                        int contractId = model.ID;
                        zipFile = new ZipFileInfo() { Name = string.Format("Action{0}.cs", contractId) };
                        var tileName = GetTileName(model.ID, model.Descption);
                        List<ParamInfoModel> requestParams;
                        List<ParamInfoModel> responseParams;
                        GetParamInfo(slnId, contractId, versionId, out requestParams, out responseParams);
                        zipFile.Content = TemplateHelper.FromatClientCsharpTemp(clientTemp, contractId, responseParams, requestParams, tileName);

                        zipFileList.Add(zipFile);
                    }
                }
                if (zipFileList != null)
                {
                    SaveAsAttachment(string.Format("{0}Action{1}.zip", type, DateTime.Now.ToString("HHmmss")), zipFileList);
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("OnExportAllClientCode error:{0}", ex);
            }
        }


        #endregion

    }
}