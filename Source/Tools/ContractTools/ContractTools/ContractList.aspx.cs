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

namespace ZyGames.ContractTools
{
    public partial class ContractList : System.Web.UI.Page
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
            DataSet dssln = new SolutionBLL().GetList("");
            ddlSolution.DataSource = dssln;
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();

            var slnTable = dssln.Tables[0];

            slnTable.PrimaryKey = new DataColumn[] { slnTable.Columns["SlnID"] };
            var row = slnTable.Rows.Find(slnID);
            if (row != null)
            {
                lblSlnName.Text = row["SlnName"].ToString();
            }

            ddContract.Items.Clear();
            ContractBLL BLL = new ContractBLL();
            DataSet ds = BLL.GetList("SlnID=" + slnID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddContract.DataSource = ds;
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

            ContractBLL con = new ContractBLL();
            if (con.Copy(int.Parse(txtSlnID.Text), int.Parse(ddContract.Text), int.Parse(ddlSolution.Text), int.Parse(txtCopyID.Text)))
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
