using System;
using System.Web.UI.WebControls;
using System.Data;
using ContractTools.WebApp.Base;

namespace ContractTools.WebApp
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
                var list = DbDataLoader.GetContract(f =>
                {
                    f.Condition = string.Format("({0} OR {1}) AND {2}",
                        f.FormatExpression("Descption", "LIKE", "Arg"),
                        f.FormatExpression("ID", "LIKE", "Arg"),
                        f.FormatExpression("SlnID"));
                    f.AddParam("Arg", string.Format("%{0}%",SearchTextBox.Text));
                    f.AddParam("SlnID", SlnID);
                });
                if (list.Count == 0)
                {
                    ResultLiteral.Visible = true;
                    ResultLiteral.Text = "查不到任何结果";
                }
                else
                {
                    ResultLiteral.Visible = false;
                    GridView.DataSource = list;
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
