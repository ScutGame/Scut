using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
using System.Drawing;

namespace ZyGames.ContractTools
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
            DataSet ds = new SolutionBLL().GetList("");
            ddlSolution.DataSource = ds;
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();
            ddlSolution.SelectedValue = SlnID;
            string gameId = string.IsNullOrEmpty(ddlSolution.SelectedValue) ? "0" : ddlSolution.SelectedValue;

            ddlAgreement.Items.Clear();
            ddlAgreement.Items.Add(new ListItem("全部", "0"));
            DataSet ds2 = new AgreementBLL().GetList(" gameid=" + gameId, gameId);
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                ddlAgreement.Items.Add(new ListItem(ds2.Tables[0].Rows[i]["Title"].ToString(), ds2.Tables[0].Rows[i]["AgreementID"].ToString()));
            }
            //ddlAgreement.DataSource = ds2;
            //ddlAgreement.DataTextField = "Title";
            //ddlAgreement.DataValueField = "AgreementID";
            //ddlAgreement.DataBind();
            
            ddlAgreement.SelectedValue = "0";
            DataSet ds3 = new AgreementBLL().GetList(" gameid=" + gameId, gameId);
            DataSet ds4 = new ContractBLL().GetList(" SlnID=" + gameId);
            TreeList.Nodes.Clear();
            leftStyle = "display:none;";
            for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
            {
                leftStyle = "";
                TreeNode node = new TreeNode();
                node.Text = ds3.Tables[0].Rows[i]["Title"].ToString();
                node.Value = ds3.Tables[0].Rows[i]["AgreementID"].ToString();
                node.Target = "0";
                node.Expanded = false;
                TreeList.Nodes.Add(node);
                //TreeList(0, node);
                TreeList.DataBind();
            }
            for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(ds4.Tables[0].Rows[i]["AgreementID"]) > 0)
                {
                    for (int j = 0; j < TreeList.Nodes.Count; j++)
                    {
                        TreeNode node = TreeList.Nodes[j];
                        if (ds4.Tables[0].Rows[i]["AgreementID"].ToString() == node.Value)
                        {
                            TreeNode nodes = new TreeNode();
                            nodes.Text = ds4.Tables[0].Rows[i]["uname"].ToString();
                            nodes.Value = ds4.Tables[0].Rows[i]["ID"].ToString();
                            node.ChildNodes.Add(nodes);

                        }
                    }
                }
            }

            //TreeList.Attributes.Add("onclick", "HandleCheckEvent()"); 
            //  if (!string.IsNullOrEmpty(hi_SlnId.Value))
            //  {
            
            //   }

            if (ddlSolution.Text.Length > 0)
            {
                BindContract(Convert.ToInt32(ddlSolution.Text));
            }
        }

        private void BindContract(int slnID)
        {
            DropGetList.Items.Clear();
            ContractBLL BLL = new ContractBLL();
            DataSet ds = BLL.GetList("SlnID=" + slnID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropGetList.DataSource = ds;
                DropGetList.DataTextField = "uname";
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
                DataSet ds = BindList();
                string parameter = string.Format("?ID={0}&slnID={1}", DropGetList.Text, ddlSolution.Text);
                UnitTestLink.NavigateUrl = "UnitTest.aspx" + parameter;
                AddRecordLink.NavigateUrl = "AddParamInfo.aspx" + parameter;
                AddProtocolLink.NavigateUrl = "AddContract.aspx" + parameter;
                UPRecordLink.NavigateUrl = "UpParamInfo.aspx" + parameter;
                btnCopyContract.NavigateUrl = "ContractList.aspx" + parameter;
                AddEnumLink.NavigateUrl = "addenum.aspx" + parameter;
                SearchLink.NavigateUrl = "search.aspx" + parameter;

                if (gvGetlist.Rows.Count != 0)
                {
                    txtContent.Visible = true;
                    txtContentto.Visible = true;
                    btnCopy.Visible = true;
                    btnCopyto.Visible = true;

                    string name = Path.Combine(Server.MapPath("~"), "CustomerModel.txt");
                    string tempContentto = Mainfun.ReadTemp(name);
                    txtContentto.Text = Mainfun.FromatTempto(tempContentto, ds, DropGetList.SelectedItem.Text);
                    DataSet slnRecord = new SolutionBLL().GetList("SlnID=" + ddlSolution.Text);
                    if (LangDropDownList.SelectedValue == "C#")
                    {
                        string fileName = Path.Combine(Server.MapPath("~"), "model.txt");
                        string tempContent = Mainfun.ReadTemp(fileName);
                        string tempActionDefine = Mainfun.ReadTemp(Path.Combine(Server.MapPath("~"), "ActionIDDefine.txt"));

                        txtContent.Text = Mainfun.FormatTemp(tempContent, ds, slnRecord, DropGetList.SelectedItem.Text);
                        DataSet contractDs = new ContractBLL().GetList("slnID=" + ddlSolution.Text);
                        txtActionDefine.Text = Mainfun.FormatActionDefineTemp(tempActionDefine, contractDs, slnRecord);
                        txtActionDefine.Visible = true;
                    }
                    else
                    {
                        string fileName = Path.Combine(Server.MapPath("~"), "pythonmodel.txt");
                        string tempContent = Mainfun.ReadTemp(fileName);

                        txtContent.Text = Mainfun.FormatPython(tempContent, ds, slnRecord, DropGetList.SelectedItem.Text);
                        txtActionDefine.Text = string.Empty;
                        txtActionDefine.Visible = false;
                    };


                    lblExample.Text = Mainfun.GetExampleUrl(ds, DropGetList.Text);
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
                Response.Write("错误信息:" + ex.Message);
            }
        }

        private DataSet BindList()
        {
            DataSet ds = GetParamInfo();
            DataTable dt = ds.Tables[0];
            gvGetlist.DataSource = ds;
            gvGetlist.DataBind();
            return ds;

        }

        private DataSet GetParamInfo()
        {
            ParamInfoBLL BLL = new ParamInfoBLL();
            DataSet ds = BLL.GetList(string.Format("ContractID={0} and SlnID={1}", DropGetList.Text.Length == 0 ? "0" : DropGetList.Text, ddlSolution.Text.Length == 0 ? "0" : ddlSolution.Text));
            return ds;
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
                mode.ContractID = Convert.ToInt32(DropGetList.Text);
                mode.ParamType = Convert.ToInt32(((DropDownList)cell.FindControl("droParamType")).SelectedValue);
                mode.Field = ((TextBox)cell.FindControl("txtField")).Text.ToString().Trim();
                mode.FieldType = Convert.ToInt32(((DropDownList)cell.FindControl("droFieldType")).SelectedValue.ToString().Trim());
                mode.Descption = ((TextBox)cell.FindControl("txtDescption")).Text.ToString().Trim();
                mode.Required = Convert.ToBoolean(((DropDownList)cell.FindControl("droRequired")).SelectedValue);
                mode.Remark = ((TextBox)cell.FindControl("txtRemark")).Text.ToString().Trim();
                mode.SortID = Convert.ToInt32(((TextBox)cell.FindControl("txtSortID")).Text.ToString().Trim());
                mode.FieldValue = ((TextBox)cell.FindControl("txtFieldValue")).Text.ToString().Trim();
                mode.MinValue = Convert.ToInt32(((TextBox)cell.FindControl("txtMinValue")).Text.ToString().Trim());
                mode.MaxValue = Convert.ToInt32(((TextBox)cell.FindControl("txtMaxValue")).Text.ToString().Trim());
                mode.ModifyDate = DateTime.Now;
                ParamInfoBLL Pinfo = new ParamInfoBLL();
                if (Pinfo.Update(mode))
                {
                    gvGetlist.EditIndex = -1;
                    QueryResult();
                }
            }

            catch (Exception erro)
            {
                Response.Write("错误信息:" + erro.Message);
            }
            finally
            {

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
            ParamInfoBLL pinfo = new ParamInfoBLL();
            if (pinfo.Delete(id))
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
                DataSet ds = GetParamInfo();
                DataRow preRow = ds.Tables[0].Rows[rowIndex - 1];
                int preParamType = Convert.ToInt32(preRow["ParamType"]);
                int preID = Convert.ToInt32(preRow["ID"]);
                if (paramType == preParamType)
                {
                    new ParamInfoBLL().UpdateSort(preID, sortID);
                    new ParamInfoBLL().UpdateSort(currID, sortID - 1);
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
                DataSet ds = GetParamInfo();
                if (rowIndex >= ds.Tables[0].Rows.Count - 1) return;

                DataRow nextRow = ds.Tables[0].Rows[rowIndex + 1];
                int preParamType = Convert.ToInt32(nextRow["ParamType"]);
                int nextID = Convert.ToInt32(nextRow["ID"]);
                int nextSortID = Convert.ToInt32(nextRow["SortID"]);
                if (paramType == preParamType)
                {
                    new ParamInfoBLL().UpdateSort(nextID, sortID);
                    new ParamInfoBLL().UpdateSort(currID, sortID + 1);
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
                    DataRowView view = e.Row.DataItem as DataRowView;//定义一个DataRowView的实例

                    if (view != null)
                    {
                        DateTime modifyTime = (DateTime)view["ModifyDate"];
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
            Hashtable ht = Mainfun.LoadEnumApplication(slnid, false);
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
            ContractBLL BLL = new ContractBLL();
            if (!DropGetList.Text.Trim().Equals(""))
            {
                if (BLL.Delete(Convert.ToInt32(DropGetList.Text), Convert.ToInt32(ddlSolution.Text)))
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
                SaveasAttachment(txtContent.Text, String.Format("Action{0}.cs", DropGetList.Text));
            }
            else
            {
                SaveasAttachment(txtContent.Text, String.Format("action{0}.py", DropGetList.Text));
            }
        }

        private void SaveasAttachment(string txtContent, string filename)
        {
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            byte[] o = Encoding.GetEncoding("gb2312").GetBytes(txtContent);
            Response.OutputStream.Write(o, 0, o.Length);
            Response.End();
        }

        protected void btnCopy1_Click(object sender, EventArgs e)
        {
            SaveasAttachment(txtContentto.Text, String.Format("Action{0}.lua", DropGetList.Text));
        }

        protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCookies(string.Empty, ddlSolution.Text);
            BindContract(Convert.ToInt32(ddlSolution.SelectedValue));
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
            ddlAgreement.Items.Clear();
            ddlAgreement.Items.Add(new ListItem("全部", "0"));

            string gameId = string.IsNullOrEmpty(ddlSolution.SelectedValue) ? "0" : ddlSolution.SelectedValue;
            DataSet ds2 = new AgreementBLL().GetList(" gameid=" + gameId, gameId);
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                ddlAgreement.Items.Add(new ListItem(ds2.Tables[0].Rows[i]["Title"].ToString(), ds2.Tables[0].Rows[i]["AgreementID"].ToString()));
            }
            DataSet ds3 = new AgreementBLL().GetList(" gameid=" + gameId, gameId);
            DataSet ds4 = new ContractBLL().GetList(" SlnID=" + gameId);
            TreeList.Nodes.Clear();
            for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
            {


                TreeNode node = new TreeNode();

                node.Text = ds3.Tables[0].Rows[i]["Title"].ToString();

                node.Value = ds3.Tables[0].Rows[i]["AgreementID"].ToString();
                node.Target = "0";
                node.Expanded = false;

                TreeList.Nodes.Add(node);

                //TreeList(0, node);

                TreeList.DataBind();
            }
            for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToInt32(ds4.Tables[0].Rows[i]["AgreementID"]) > 0)
                {
                    for (int j = 0; j < TreeList.Nodes.Count; j++)
                    {
                        TreeNode node = TreeList.Nodes[j];
                        if (ds4.Tables[0].Rows[i]["AgreementID"].ToString() == node.Value)
                        {
                            TreeNode nodes = new TreeNode();
                            nodes.Text = ds4.Tables[0].Rows[i]["uname"].ToString();
                            nodes.Value = ds4.Tables[0].Rows[i]["ID"].ToString();
                            node.ChildNodes.Add(nodes);

                        }
                    }
                }
            }
        }
        protected void ddlAgreement_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContractBLL BLL = new ContractBLL();

            string gameId = string.IsNullOrEmpty(ddlSolution.SelectedValue) ? "0" : ddlSolution.SelectedValue;
            DataSet ds = ddlAgreement.SelectedValue == "0" 
                ? BLL.GetList(" SlnID=" + gameId + " and AgreementID>=0") 
                : BLL.GetList(" SlnID=" + gameId + " and  AgreementID=" + ddlAgreement.SelectedValue);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DropGetList.DataSource = ds;
                DropGetList.DataTextField = "uname";
                DropGetList.DataValueField = "ID";
                DropGetList.DataBind();
            }
            else
            {
                DropGetList.Items.Clear();
                DropGetList.Items.Add(new ListItem("无接口","0"));
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
            ContractBLL BLL = new ContractBLL();
            if (BLL.UpdateStatus(Convert.ToInt32(DropGetList.Text), Convert.ToInt32(ddlSolution.Text), complated))
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
                ContractBLL BLL = new ContractBLL();
                DataSet ds = ddlAgreement.SelectedValue == "0" ? BLL.GetList("AgreementID>=0") : BLL.GetList("AgreementID=" + ddlAgreement.SelectedValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DropGetList.DataSource = ds;
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
