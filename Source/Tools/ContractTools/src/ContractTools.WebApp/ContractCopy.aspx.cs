using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;

namespace ContractTools.WebApp
{
    public partial class ContractCopy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSlnID.Text = SlnID.ToString();
                txtCopyID.Text = ContractID.ToString();
                Bind(SlnID);
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
                if (string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request.QueryString["ID"]);
            }
        }

        private void Bind(int slnID)
        {
            ddlSolution.Items.Clear();
            var slnList = DbDataLoader.GetSolution();
            ddlSolution.DataSource = slnList;
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();

            var slnModel = slnList.Where(p => p.SlnID == slnID).FirstOrDefault();
            if (slnModel != null)
            {
                lblSlnName.Text = slnModel.SlnName;
            }

            ddContract.Items.Clear();
            var contractList = DbDataLoader.GetContract(slnID, VerID);
            if (contractList.Count > 0)
            {
                ddContract.DataSource = contractList;
                ddContract.DataTextField = "uname";
                ddContract.DataValueField = "ID";
                ddContract.DataBind();

                ddContract.SelectedValue = ContractID.ToString();
            }
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            if (txtSlnID.Text.Trim() == ddlSolution.Text.Trim() &&
                txtCopyID.Text.Trim() == ddContract.Text)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('不能复制至相同项目方案！')</script>");
                return;
            }

            if (DbDataLoader.CopyContract(int.Parse(txtSlnID.Text), int.Parse(ddContract.Text), int.Parse(ddlSolution.Text), int.Parse(txtCopyID.Text)))
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('复制成功！')</script>");
            }
            else
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('复制失败！')</script>");
            }
        }

        protected void ddContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtCopyID.Text = ddContract.Text;
        }
    }
}