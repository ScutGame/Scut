using System;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;

namespace ContractTools.WebApp
{
    public partial class SolutionsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            this.GridView1.DataSource = DbDataLoader.GetSolution();
            this.GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1; 
            BindData();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((string) GridView1.DataKeys[e.RowIndex].Values[0].ToString());
                string SlnName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("SlnName")).Text;
                string Namespace = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Namespace")).Text;
                string RefNamespace = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("RefNamespace")).Text;
                string url = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Url")).Text;
                string gameid = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("gameid")).Text;

                SolutionModel mode = new SolutionModel();
                mode.SlnID = id;
                mode.SlnName = SlnName;
                mode.Namespace = Namespace;
                mode.RefNamespace = RefNamespace;
                mode.Url = url;
                mode.GameID = Convert.ToInt32(gameid);
                if (DbDataLoader.Update(mode))
                {
                    GridView1.EditIndex = -1;
                    BindData();
                }
            }

            catch (Exception erro)
            {
                Response.Write("错误信息:" + erro.Message);
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((string) GridView1.DataKeys[e.RowIndex].Values[0].ToString());

                if (DbDataLoader.Delete(new SolutionModel(){SlnID = id}))
                {
                    GridView1.EditIndex = -1;
                    BindData();
                }
            }

            catch (Exception erro)
            {
                Response.Write("错误信息:" + erro.Message);
            }
        }

        protected string FormatToHtml(object value)
        {
            return value.ToString().Replace("\r\n","<br>");
        }
    }
}
