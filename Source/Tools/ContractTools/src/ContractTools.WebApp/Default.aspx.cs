using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
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
                int verId = BindVersion(slnId);
                int agreementId = BindAgreement(slnId);
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
            ddlSolution.DataSource = DbDataLoader.GetSolution();
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();
            ddlSolution.SelectedValue = slnId.ToString();
            return ddlSolution.SelectedValue.ToInt();
        }
        private int BindAgreement(int slnId)
        {
            ddlAgreement.DataSource = DbDataLoader.GetAgreement(slnId);
            ddlAgreement.DataTextField = "Title";
            ddlAgreement.DataValueField = "AgreementID";
            ddlAgreement.DataBind();
            ddlAgreement.Items.Insert(0, new ListItem("All", "0"));
            ddlAgreement.SelectedValue = "0";
            return ddlAgreement.SelectedValue.ToInt();
        }
        private int BindVersion(int slnId)
        {
            ddVersion.DataSource = DbDataLoader.GetVersion(slnId);
            ddVersion.DataTextField = "Title";
            ddVersion.DataValueField = "ID";
            ddVersion.DataBind();
            ddVersion.Items.Insert(0, new ListItem("None", "0"));
            ddVersion.SelectedValue = "0";
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

        private void BindGrid()
        {
            BindGrid(ddlSolution.Text.ToInt(), ddVersion.Text.ToInt(), ddContract.Text.ToInt());
        }
        private void BindGrid(int slnId, int versionId, int contractId)
        {
            //grid bind
            var paramList = DbDataLoader.GetParamInfo(slnId, contractId, versionId);
            var pairs = paramList.GroupBy(t => t.ParamType);
            List<ParamInfoModel> requestParams = new List<ParamInfoModel>();
            List<ParamInfoModel> responseParams = new List<ParamInfoModel>();
            bool isEdit = IsEdit;
            foreach (var pair in pairs)
            {
                switch (pair.Key)
                {
                    case 1:
                        requestParams = pair.ToList();
                        break;
                    case 2:
                        responseParams = pair.ToList();
                        break;
                    default:
                        break;
                }
            }

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
            var modol = DbDataLoader.GetContract(slnId, contractId, 0);
            string tileName = modol != null ? modol.Descption : "Action " + contractId;
            int index = tileName.IndexOf("【", System.StringComparison.Ordinal);
            if (index != -1)
            {
                tileName = tileName.Substring(0, index);
            }
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
                clientTemp = Path.Combine(Server.MapPath("~"), isSelfAction ? "Template/ClientCsharpSelfCode.txt" : "Template/ClientCsharpCode.txt");
                txtClientCode.Text = TemplateHelper.FromatClientCsharpTemp(TemplateHelper.ReadTemp(clientTemp), contractId, responseParams, requestParams, tileName);
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
        }

        private void BindResult()
        {
            BindResult(ddlSolution.Text.ToInt(), ddVersion.Text.ToInt(), ddContract.Text.ToInt());
        }

        private void BindResult(int slnId, int versionId, int contractId)
        {
            hlTopEdit.NavigateUrl = "Default.aspx" + (IsEdit ? "" : "?edit=true");
            string get = string.Format("?ID={0}&slnID={1}&VerID={2}&GameID={1}", contractId, slnId, versionId);
            hlSolution.NavigateUrl = "Solutions.aspx";
            hlVersion.NavigateUrl = "VersionEdit.aspx" + get;
            hlEnum.NavigateUrl = "EnumEdit.aspx" + get;
            hlAgreement.NavigateUrl = "AgreementEdit.aspx" + get;
            hlContract.NavigateUrl = "ContractEdit.aspx" + get;
            hlContractEdit.NavigateUrl = "ContractEdit.aspx" + get + "&modify=true";
            hlContractCopy.NavigateUrl = "ContractCopy.aspx" + get;

            ifrTest.Src = "ContractDebug.aspx" + get;
            ifrClientConfig.Src = "ClientConfigInfo.aspx" + get;
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
            SetCookies(string.Empty, ddlSolution.Text);
            int slnId = ddlSolution.Text.ToInt();
            BindEnumInfo(slnId);
            int verId = BindVersion(slnId);
            int agreementId = BindAgreement(slnId);
            int contractId = BindContract(slnId, verId, agreementId, 0);
            BindResult(slnId, verId, contractId);
        }
        protected void OnVersionChanged(object sender, EventArgs e)
        {
            int slnId = ddlSolution.Text.ToInt();
            int verId = ddVersion.Text.ToInt();
            int agreementId = ddlAgreement.Text.ToInt();
            int contractId = BindContract(slnId, verId, agreementId, 0);
            BindResult(slnId, verId, contractId);

        }
        protected void OnAgreementChanged(object sender, EventArgs e)
        {
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label control = (Label)e.Row.FindControl("LabFieldType");
                if (control != null)
                {
                    control.Text = control.Text.ToEnum<FieldType>().ToString();
                    var paramInfo = e.Row.DataItem as ParamInfoModel;//定义一个DataRowView的实例

                    if (paramInfo != null)
                    {
                        if (paramInfo.FieldType == FieldType.Record || paramInfo.FieldType == FieldType.End)
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
            if (rowIndex > 0)
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

        protected void OnRespGridDelete(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            if (DbDataLoader.Delete(new ParamInfoModel() { ID = id }))
            {
                Response.Write("<script language=javascript>alert('删除成功！')</script>");
            }
            else
            {
                Response.Write("<script language=javascript>alert('删除失败！')</script>");
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
            else
            {
                Response.Write("<script language=javascript>alert('提交失败！')</script>");
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

                var paramList = DbDataLoader.GetParamInfo(mode.SlnID, mode.ContractID, mode.ParamType, mode.VerID).OrderBy(p => p.SortID).ToList();
                if (paramList.Count == 0 || paramList[0].SortID == 0)
                {
                    mode.SortID = 1;
                }
                else
                {
                    int SortID = paramList[paramList.Count - 1].SortID;
                    SortID++;
                    mode.SortID = SortID;

                }

                if (DbDataLoader.Add(mode) > 0)
                {
                    BindGrid(mode.SlnID, mode.VerID, mode.ContractID);
                    txtField.Text = "";
                    txtRemark.Text = "";
                }
                else
                {
                    Response.Write("<script language=javascript>alert('增加失败！')</script>");
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Default ParamAdd error:{0}", ex);
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

        #endregion
    }
}