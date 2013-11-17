using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;

namespace ZyGames.ContractTools
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["search"]))
                {
                    SearchTextBox.Text = Request["search"];
                }
                BindData();
            }
        }

        private void BindData()
        {
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                ContractBLL BLL = new ContractBLL();
                DataSet ds = BLL.Search(SlnID, SearchTextBox.Text);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    ResultLiteral.Visible = true;
                    ResultLiteral.Text = "查不到任何结果";
                }
                else
                {
                    ResultLiteral.Visible = false;
                    GridView.DataSource = ds;
                    GridView.DataBind();
                }
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

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
