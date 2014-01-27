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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Drawing;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ContractTools.WebApp
{
    /// <summary>
    /// 首页生成页面
    /// </summary>
    public partial class index : System.Web.UI.Page
    {
        public string leftStyle = "";
        private string GetCookesKey(string key)
        {
            return string.Format("index{0}", key);
        }
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
            if (!IsPostBack)
            {
                //try
                //{
                //    hi_SlnId.Value = SlnID;
                //}
                //catch
                //{
                //}
                Bind();
                try
                {
                    DropGetList.SelectedValue = ContractID;
                }
                catch
                {
                }
                QueryResult();
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
        /// <summary>
        /// 初始化加载
        /// </summary>
        public void Bind()
        {
            ddlSolution.Items.Clear();
            var list = DbDataLoader.GetSolution();
            ddlSolution.DataSource = list;
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();
            ddlSolution.SelectedValue = SlnID;

            int gameId = ddlSolution.SelectedValue.ToInt();
            BindAgreement(gameId);
            BindContractTree(gameId);


            if (ddlSolution.Text.Length > 0)
            {
                BindContract(Convert.ToInt32((string)ddlSolution.Text));
            }
        }

        private void BindContractTree(int gameId)
        {
            var contractList = DbDataLoader.GetContract(gameId);
            foreach (var contract in contractList)
            {
                if (contract.AgreementID > 0)
                {
                    for (int j = 0; j < TreeList.Nodes.Count; j++)
                    {
                        TreeNode node = TreeList.Nodes[j];
                        if (contract.AgreementID.ToString() == node.Value)
                        {
                            TreeNode nodes = new TreeNode();
                            nodes.Text = contract.Uname;
                            nodes.Value = contract.ID.ToString();
                            node.ChildNodes.Add(nodes);

                        }
                    }
                }
            }
        }

        private void BindAgreement(int gameId)
        {
            ddlAgreement.Items.Clear();
            ddlAgreement.Items.Add(new ListItem("全部", "0"));
            var agreementList = DbDataLoader.GetAgreement(gameId);
            TreeList.Nodes.Clear();
            leftStyle = "display:none;";
            foreach (var agreement in agreementList)
            {
                ddlAgreement.Items.Add(new ListItem(agreement.Title, agreement.AgreementID.ToString()));

                leftStyle = "";
                TreeNode node = new TreeNode();
                node.Text = agreement.Title;
                node.Value = agreement.AgreementID.ToString();
                node.Target = "0";
                node.Expanded = false;
                TreeList.Nodes.Add(node);
            }
            TreeList.DataBind();
            ddlAgreement.SelectedValue = "0";
        }

        private void BindContract(int slnID)
        {
            DropGetList.Items.Clear();
            var contractList = DbDataLoader.GetContract(slnID);
            if (contractList.Count > 0)
            {
                DropGetList.DataSource = contractList;
                DropGetList.DataTextField = "Uname";
                DropGetList.DataValueField = "ID";
                DropGetList.DataBind();
            }
            string conId = ContractID;
            if (!string.IsNullOrEmpty(conId) && conId != "0")
            {
                try
                {
                    DropGetList.SelectedValue = conId;
                }
                catch
                {
                }
            }
        }

        protected void QueryResult()
        {
            try
            {
                gvGetlist.EditIndex = -1;
                var paramList = BindList();
                var reqParams = paramList.Where(p => p.ParamType == 1).OrderBy(p => p.SortID).ToList();
                var respParams = paramList.Where(p => p.ParamType == 2).OrderBy(p => p.SortID).ToList();
                int contractId = DropGetList.Text.ToInt();
                string parameter = string.Format("?ID={0}&slnID={1}", contractId, ddlSolution.Text);
                UnitTestLink.NavigateUrl = "UnitTest.aspx" + parameter;
                AddRecordLink.NavigateUrl = "AddParamInfo.aspx" + parameter;
                AddProtocolLink.NavigateUrl = "AddContract.aspx" + parameter;
                UPRecordLink.NavigateUrl = "UpParamInfo.aspx" + parameter;
                btnCopyContract.NavigateUrl = "ContractList.aspx" + parameter;
                AddEnumLink.NavigateUrl = "addenum.aspx" + parameter;
                SearchLink.NavigateUrl = "search.aspx" + parameter;
                syncLink.Target = "_blank";
                syncLink.NavigateUrl = "SyncModelInfo.aspx" + parameter;
                btnTestCase.NavigateUrl = "TestCase.aspx" + parameter;
                btnTestCase.Target = "_blank";

                int slnId = ddlSolution.Text.ToInt();
                if (gvGetlist.Rows.Count != 0)
                {
                    txtContent.Visible = true;
                    txtContentto.Visible = true;
                    btnCopy.Visible = true;
                    btnCopyto.Visible = true;
                    string name = Path.Combine(Server.MapPath("~"), "Template/CustomerModel.txt");
                    string tempContentto = TemplateHelper.ReadTemp(name);
                    txtContentto.Text = TemplateHelper.FromatTempto(tempContentto, contractId, respParams, reqParams, DropGetList.SelectedItem.Text);
                    var slnRecord = DbDataLoader.GetSolution(slnId);
                    if (LangDropDownList.SelectedValue == "C#")
                    {
                        string fileName = Path.Combine(Server.MapPath("~"), "Template/model.txt");
                        string tempContent = TemplateHelper.ReadTemp(fileName);
                        string tempActionDefine = TemplateHelper.ReadTemp(Path.Combine(Server.MapPath("~"), "Template/ActionIDDefine.txt"));

                        txtContent.Text = TemplateHelper.FormatTemp(tempContent, contractId, respParams, reqParams, slnRecord, DropGetList.SelectedItem.Text);
                        var contractDs = DbDataLoader.GetContract(slnId);
                        txtActionDefine.Text = TemplateHelper.FormatActionDefineTemp(tempActionDefine, contractDs, slnRecord);
                        txtActionDefine.Visible = true;
                    }
                    else
                    {
                        string fileName = Path.Combine(Server.MapPath("~"), "Template/pythonmodel.txt");
                        string tempContent = TemplateHelper.ReadTemp(fileName);

                        txtContent.Text = TemplateHelper.FormatPython(tempContent, respParams, reqParams, slnRecord, DropGetList.SelectedItem.Text);
                        txtActionDefine.Text = string.Empty;
                        txtActionDefine.Visible = false;
                    };


                    lblExample.Text = TemplateHelper.GetExampleUrl(respParams, DropGetList.Text);
                }
                else
                {
                    txtContent.Text = string.Empty;
                    txtContentto.Text = string.Empty;
                    txtContent.Visible = false;
                    btnCopy.Visible = false;
                    btnCopyto.Visible = false;
                    txtContentto.Visible = false;
                }
            }
            catch (Exception ex)
            {
                TraceLog.WriteError("QueryResult:{0}", ex);
                Response.Write("错误信息:" + ex.Message);
            }
        }

        private List<ParamInfoModel> BindList()
        {
            var ds = GetParamInfo();
            gvGetlist.DataSource = ds;
            gvGetlist.DataBind();
            return ds;

        }

        private List<ParamInfoModel> GetParamInfo()
        {
            int slnId = ddlSolution.Text.ToInt();
            int conId = DropGetList.Text.ToInt();
            return DbDataLoader.GetParamInfo(slnId, conId);
        }

        protected void gvGetlist_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvGetlist.EditIndex = e.NewEditIndex;
            BindList();
        }

        protected void gvGetlist_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TableCell cell = gvGetlist.Rows[e.RowIndex].Cells[0];
                ParamInfoModel mode = new ParamInfoModel();
                mode.ID = Convert.ToInt32(((Label)cell.FindControl("IDLabel")).Text.ToString().Trim());
                mode.ContractID = Convert.ToInt32((string)DropGetList.Text);
                mode.ParamType = Convert.ToInt32(((DropDownList)cell.FindControl("droParamType")).SelectedValue);
                mode.Field = ((TextBox)cell.FindControl("txtField")).Text.ToString().Trim();
                mode.FieldType = ((DropDownList)cell.FindControl("droFieldType")).SelectedValue.ToEnum<FieldType>();
                mode.Descption = ((TextBox)cell.FindControl("txtDescption")).Text.ToString().Trim();
                mode.Required = Convert.ToBoolean(((DropDownList)cell.FindControl("droRequired")).SelectedValue);
                mode.Remark = ((TextBox)cell.FindControl("txtRemark")).Text.ToString().Trim();
                mode.SortID = Convert.ToInt32(((TextBox)cell.FindControl("txtSortID")).Text.ToString().Trim());
                mode.FieldValue = ((TextBox)cell.FindControl("txtFieldValue")).Text.ToString().Trim();
                mode.MinValue = Convert.ToInt32(((TextBox)cell.FindControl("txtMinValue")).Text.ToString().Trim());
                mode.MaxValue = Convert.ToInt32(((TextBox)cell.FindControl("txtMaxValue")).Text.ToString().Trim());
                mode.ModifyDate = MathUtils.Now;
                if (DbDataLoader.Update(mode))
                {
                    gvGetlist.EditIndex = -1;
                    QueryResult();
                }
            }

            catch (Exception erro)
            {
                TraceLog.WriteError("RowUpdating:{0}", erro);
                Response.Write("错误信息:" + erro.Message);
            }

        }

        protected void gvGetlist_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvGetlist.EditIndex = -1;
            BindList();
        }

        protected void LinkButton1_Command(object sender, CommandEventArgs e)
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
            QueryResult();
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
                var ds = GetParamInfo();
                var preRow = ds[rowIndex - 1];
                int preParamType = preRow.ParamType;
                int preID = preRow.ID;
                if (paramType == preParamType)
                {
                    DbDataLoader.UpdateParamSort(preID, sortID);
                    DbDataLoader.UpdateParamSort(currID, sortID - 1);
                    QueryResult();
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
                var ds = GetParamInfo();
                if (rowIndex >= ds.Count - 1) return;

                var nextRow = ds[rowIndex + 1];
                int preParamType = nextRow.ParamType;
                int nextID = nextRow.ID;
                int nextSortID = nextRow.SortID;
                if (paramType == preParamType)
                {
                    DbDataLoader.UpdateParamSort(nextID, sortID);
                    DbDataLoader.UpdateParamSort(currID, sortID + 1);
                    QueryResult();
                }
            }
        }

        protected string UPFieldType(int txt)
        {
            FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), txt);
            switch (fieldType)
            {
                case FieldType.Int:
                    fieldType = FieldType.Int;
                    break;
                case FieldType.String:
                    fieldType = FieldType.String;
                    break;
                case FieldType.Short:
                    fieldType = FieldType.Short;
                    break;
                case FieldType.Byte:
                    fieldType = FieldType.Byte;
                    break;
                default:
                    break;
            }
            return fieldType.ToString();
        }

        protected string UPlblParamType(int paramType)
        {
            string stu = string.Empty;
            if (paramType == 1)
            {
                stu = ParamType.sts;
            }
            else
            {
                stu = ParamType.stc;
            }
            return stu;
        }

        protected void gvGetlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRowID = (Label)e.Row.FindControl("lblRowID");
                lblRowID.Text = (e.Row.RowIndex + 1).ToString();
                Label control = (Label)e.Row.FindControl("LabFieldType");
                if (control != null)
                {
                    control.Text = UPFieldType(Convert.ToInt32(control.Text));
                    if (control.Text == FieldType.Record.ToString() || control.Text == FieldType.End.ToString())
                    {
                        control.Font.Bold = true;
                    }
                    var paramInfo = e.Row.DataItem as ParamInfoModel;//定义一个DataRowView的实例

                    if (paramInfo != null)
                    {
                        DateTime modifyTime = (DateTime)paramInfo.ModifyDate;
                        if (DateTime.Now - modifyTime < TimeSpan.FromDays(3))
                        {
                            e.Row.BackColor = Color.Yellow;
                        }
                    }
                    Label paType = (Label)e.Row.FindControl("lblParamType");
                    paType.Text = UPlblParamType(Convert.ToInt32(paType.Text));

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
                        innertext = innertext.Replace('[', '【').Replace(']', '】');
                        if (ht.Contains(innertext))
                        {
                            sb.Append(string.Format("<a href='addenum.aspx?enum={2}&id={0}&slnid={1}' onmouseover='ShowPrompt(event, \"{3}\")'>{2}</a>", DropGetList.Text, ddlSolution.Text, innertext,
                                ht[innertext].ToString().Replace("\r", "").Replace("\n", "<br>")));
                        }
                        else
                        {
                            sb.Append(string.Format("<a href='addenum.aspx?enum={2}&id={0}&slnid={1}' onmouseover='ShowPrompt(event, \"{3}\")'>{2}</a>", DropGetList.Text, ddlSolution.Text, innertext,
                                "暂无此枚举信息，请点击连接进行定义"));
                        }
                    }
                    else
                    {
                        sb.Append(text);
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
        protected void DropGetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCookies(ddlSolution.Text, DropGetList.Text);
            QueryResult();
        }

        protected void btnDeldte_Click(object sender, EventArgs e)
        {
            if (!DropGetList.Text.Trim().Equals(""))
            {
                var contractModel = new ContractModel() { ID = DropGetList.Text.ToInt(), SlnID = ddlSolution.Text.ToInt() };
                if (DbDataLoader.Delete(contractModel))
                {
                    Response.Write("<script language=javascript>alert('删除成功！')</script>");
                    QueryResult();
                    Bind();
                }
                else
                {

                    Response.Write("<script language=javascript>alert('删除失败！')</script>");
                }
            }

        }

        protected void btnCopy0_Click(object sender, EventArgs e)
        {
            if (LangDropDownList.SelectedValue == "C#")
            {
                SaveasAttachment(txtContent.Text, String.Format("Action{0}.cs", (object)DropGetList.Text));
            }
            else
            {
                SaveasAttachment(txtContent.Text, String.Format("action{0}.py", (object)DropGetList.Text));
            }
        }

        private void SaveasAttachment(string txtContent, string filename)
        {
            Response.ContentType = "text/plain";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AppendHeader("Content-Encoding", "UTF-8");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            byte[] o = Encoding.UTF8.GetBytes(txtContent);
            Response.OutputStream.Write(o, 0, o.Length);
            Response.End();
        }

        protected void btnCopy1_Click(object sender, EventArgs e)
        {
            SaveasAttachment(txtContentto.Text, String.Format("Action{0}.lua", (object)DropGetList.Text));
        }

        protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCookies(string.Empty, ddlSolution.Text);
            BindContract(Convert.ToInt32((string)ddlSolution.SelectedValue));
            try
            {
                if (!string.IsNullOrEmpty(GetCookies(ddlSolution.Text)))
                {
                    DropGetList.SelectedValue = GetCookies(ddlSolution.Text);
                }
                else
                {
                    DropGetList.SelectedValue = ContractID;
                }
            }
            catch
            {

            }
            QueryResult();

            int gameId = ddlSolution.SelectedValue.ToInt();
            BindAgreement(gameId);
            BindContractTree(gameId);
        }
        protected void ddlAgreement_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = DbDataLoader.GetContractByAgreement(ddlSolution.SelectedValue.ToInt(), ddlAgreement.SelectedValue.ToInt());
            if (list.Count > 0)
            {
                DropGetList.DataSource = list;
                DropGetList.DataTextField = "uname";
                DropGetList.DataValueField = "ID";
                DropGetList.DataBind();
            }
            else
            {
                DropGetList.Items.Clear();
                DropGetList.Items.Add(new ListItem("无接口", "0"));
            }
        }
        protected void btnConfig_Click(object sender, EventArgs e)
        {
            string url = string.Format("ClientConfigInfo.aspx?ID={0}&SlnID={1}", DropGetList.Text.Length == 0 ? "0" : DropGetList.Text, ddlSolution.Text.Length == 0 ? "0" : ddlSolution.Text);
            Response.Redirect(url, false);
        }

        protected void LangDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryResult();
        }

        protected void btnNoCompalte_Click(object sender, EventArgs e)
        {
            UpdateStatus(false);
        }

        protected void btnCompalte_Click(object sender, EventArgs e)
        {
            UpdateStatus(true);
        }

        private void UpdateStatus(bool complated)
        {
            if (DbDataLoader.UpdateContractStatus(Convert.ToInt32((string)DropGetList.Text), Convert.ToInt32((string)ddlSolution.Text), complated))
            {
                //Response.Write("<script language=javascript>alert('提交成功！');</script>");
                QueryResult();
                Bind();
            }
            else
            {
                Response.Write("<script language=javascript>alert('提交失败！')</script>");
            }
        }

        protected void TreeList_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (TreeList.SelectedNode.Target != "0")
            {
                ddlAgreement.SelectedValue = TreeList.SelectedNode.Parent.Value;
                //ContractBLL BLL = new ContractBLL();
                int agreementId = ddlAgreement.SelectedValue.ToInt();
                var list = DbDataLoader.GetContract(m =>
                {
                    if (agreementId > 0)
                    {
                        m.Condition = m.FormatExpression("AgreementID");
                        m.AddParam("AgreementID", agreementId);
                    }
                });
                if (list.Count > 0)
                {
                    DropGetList.DataSource = list;
                    DropGetList.DataTextField = "uname";
                    DropGetList.DataValueField = "ID";
                    DropGetList.DataBind();
                }
                else
                {
                    DropGetList.Items.Clear();
                    DropGetList.Items.Add(new ListItem("无接口", "0"));
                }
                TreeList.SelectedNode.Checked = true;
                TreeList.SelectedNode.Selected = true;

                DropGetList.SelectedValue = TreeList.SelectedValue;
                SetCookies(ddlSolution.Text, DropGetList.Text);
                QueryResult();
            }
        }

    }
}