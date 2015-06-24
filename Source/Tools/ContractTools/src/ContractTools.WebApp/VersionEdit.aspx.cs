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
    public partial class VersionEdit : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected int SlnId
        {
            get
            {
                return Request["slnId"].ToInt();
            }
        }

        private void BindData()
        {
            this.GridView1.DataSource = DbDataLoader.GetVersion(SlnId);
            this.GridView1.DataBind();
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                VersionMode model = new VersionMode();
                model.Title = txtTitle.Text.Trim();
                model.SlnID = SlnId;
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
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
                string title = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("itTitle")).Text;

                VersionMode model = new VersionMode();
                model.ID = id;
                model.Title = title;
                model.SlnID = SlnId;
                if (DbDataLoader.Update(model))
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

                if (DbDataLoader.Delete(new VersionMode() { ID = id }))
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