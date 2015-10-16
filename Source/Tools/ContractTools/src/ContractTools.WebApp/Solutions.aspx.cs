using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;

namespace ContractTools.WebApp
{
    public partial class Solutions : System.Web.UI.Page
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
        protected void butSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SolutionModel model = new SolutionModel();
                model.SlnName = txtDescption.Text.Trim();
                model.Namespace = txtNamespace.Text.Trim();
                model.RefNamespace = txtRefNamespace.Text.Trim();
                model.GameID = Convert.ToInt32((string)txtGameID.Text);
                model.Url = txtUrl.Text.Trim();
                model.SerUseScript = ddSerUseScript.Text;
                model.CliUseScript = ddCliUseScript.Text;
                model.RespContentType = ddResponseContentType.Text.ToInt();
                model.IsDParam = ckIsDParam.Checked;
                if (DbDataLoader.Add(model) > 0)
                {
                    BindData();
                }

            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");
            }
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
                int id = Convert.ToInt32((string)GridView1.DataKeys[e.RowIndex].Values[0].ToString());
                string SlnName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("SlnName")).Text;
                string Namespace = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Namespace")).Text;
                string RefNamespace = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("RefNamespace")).Text;
                string url = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Url")).Text;
                string gameid = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("gameid")).Text;

                string SerUseScript = ((DropDownList)GridView1.Rows[e.RowIndex].FindControl("SerUseScript")).Text;
                string CliUseScript = ((DropDownList)GridView1.Rows[e.RowIndex].FindControl("CliUseScript")).Text;
                string RespContentType = ((DropDownList)GridView1.Rows[e.RowIndex].FindControl("RespContentType")).Text;
                bool IsDParam = ((CheckBox)GridView1.Rows[e.RowIndex].FindControl("IsDParam")).Checked;

                SolutionModel mode = new SolutionModel();
                mode.SlnID = id;
                mode.SlnName = SlnName;
                mode.Namespace = Namespace;
                mode.RefNamespace = RefNamespace;
                mode.Url = url;
                mode.GameID = Convert.ToInt32(gameid);
                mode.SerUseScript = SerUseScript;
                mode.CliUseScript = CliUseScript;
                mode.RespContentType = RespContentType.ToInt();
                mode.IsDParam = IsDParam;
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
                int id = Convert.ToInt32((string)GridView1.DataKeys[e.RowIndex].Values[0].ToString());

                if (DbDataLoader.Delete(new SolutionModel() { SlnID = id }))
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
    }
}