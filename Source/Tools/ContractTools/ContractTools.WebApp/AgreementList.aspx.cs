using System;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;

namespace ContractTools.WebApp
{
    public partial class AgreementList : System.Web.UI.Page
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
            if (ddlSolution.Items.Count <= 0)
            {
                ddlSolution.Items.Clear();
                ddlSolution.DataSource = DbDataLoader.GetSolution(null);
                ddlSolution.DataTextField = "SlnName";
                ddlSolution.DataValueField = "SlnID";
                ddlSolution.DataBind();
            }
            string GameID = string.Empty;
            if (Request.QueryString["GameID"] == null || Request.QueryString["GameID"] == "")
            {
                GameID = ddlSolution.SelectedValue;
            }
            else
            {
                GameID = Request.QueryString["GameID"];
            }
            this.GridView1.DataSource = DbDataLoader.GetAgreement(GameID.ToInt());
            this.GridView1.DataBind();

            this.ddlSolution.SelectedValue = GameID;
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
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0].ToString());
                string Title = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Title")).Text;
                string Describe = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Describe")).Text;


                AgreementModel mode = new AgreementModel();
                mode.AgreementID = id;
                mode.Title = Title;
                mode.Describe = Describe;

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
                int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0].ToString());

                if (DbDataLoader.Delete(new AgreementModel() { AgreementID = id }))
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
            return value.ToString().Replace("\r\n", "<br>");
        }

        protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
