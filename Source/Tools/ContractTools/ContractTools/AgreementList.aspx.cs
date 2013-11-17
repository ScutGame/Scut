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
using model;

namespace ZyGames.ContractTools
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
                DataSet ds = new SolutionBLL().GetList("");
                ddlSolution.DataSource = ds;
                ddlSolution.DataTextField = "SlnName";
                ddlSolution.DataValueField = "SlnID";
                ddlSolution.DataBind();
            }
            string GameID = string.Empty;
            if (Request.QueryString["GameID"]==null||Request.QueryString["GameID"]=="")
            {
                GameID = ddlSolution.SelectedValue;
            }
            else
            {
                GameID = Request.QueryString["GameID"];
            }
            this.GridView1.DataSource = new AgreementBLL().GetList("  GameID=" + GameID, GameID);
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


                AgreementBLL Pinfo = new AgreementBLL();
                if (Pinfo.Update(mode))
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

                AgreementBLL Pinfo = new AgreementBLL();
                if (Pinfo.Delete(id))
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

        protected void ddlSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
