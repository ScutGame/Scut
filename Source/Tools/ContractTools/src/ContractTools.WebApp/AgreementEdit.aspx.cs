using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ContractTools.WebApp
{
    public partial class AgreementEdit : BasePage
    {
        protected string SlnID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["slnID"]))
                {
                    if (string.IsNullOrEmpty(GetCookies(string.Empty)))
                    {
                        return "0";
                    }
                    return GetCookies(string.Empty);
                }
                return Request["slnID"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
        protected void butSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                AgreementModel model = new AgreementModel();
                model.Title = txtTitle.Text.Trim();
                model.Describe = txtDescribe.Text.Trim();
                model.GameID = Convert.ToInt32(Request.QueryString["gameid"]);
                if (DbDataLoader.Add(model) > 0)
                {
                    BindData();
                    Page.RegisterStartupScript("", "<script language=javascript>alert('添加成功！')</script>");
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("AgreementAdd:{0}", ex);
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");
            }
        }

        private void BindData()
        {
            this.GridView1.DataSource = DbDataLoader.GetAgreement(SlnID.ToInt());
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

    }
}