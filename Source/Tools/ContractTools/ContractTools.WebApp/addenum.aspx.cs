using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using model;
using System.Data;
using System.Collections;
namespace ZyGames.ContractTools
{
    public partial class addenum : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
                if (!string.IsNullOrEmpty(Request["enum"]))
                {
                    string enumName = Request["enum"].Trim(new char[]{'【','】'});
                    txtName.Text = enumName;
                    EnuminfoBLL dal = new EnuminfoBLL();
                    EnumInfoModel info = dal.GetModel(enumName, SlnID.ToString());
                    if (info != null)
                    {
                        txtDescription.Text = info.enumDescription;
                        txtValueInfo.Text = info.enumValueInfo;
                        AddorEditMode(true);
                        EditKey.Text = info.ID.ToString();
                    }
                    else
                    {
                        txtName.Text = enumName;
                        AddorEditMode(false);
                    }
                }
            }
        }

        private void Bind()
        {
            EnuminfoBLL dal = new EnuminfoBLL();
            GridView.DataSource = dal.GetList(SlnID);
            GridView.DataBind();
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

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EditKey.Text))
            {
                try
                {
                    EnuminfoBLL con = new EnuminfoBLL();
                    EnumInfoModel model = new EnumInfoModel();
                    model.enumName = txtName.Text;
                    model.enumDescription = txtDescription.Text;
                    model.enumValueInfo = txtValueInfo.Text;
                    model.SlnID = SlnID;
                    if (con.Add(model))
                    {
                        Bind();
                        btCancelButton_Click(null, null);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败！')</script>");
                }
            }
            else
            {
                try
                {

                    EnuminfoBLL con = new EnuminfoBLL();
                    EnumInfoModel model = new EnumInfoModel();
                    model.enumName = txtName.Text;
                    model.enumDescription = txtDescription.Text;
                    model.enumValueInfo = txtValueInfo.Text;
                    model.SlnID = SlnID;
                    model.ID = int.Parse(EditKey.Text);
                    if (con.Update(model))
                    {
                        Bind();
                        btCancelButton_Click(null, null);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Page.RegisterStartupScript("", "<script language=javascript>alert('编辑失败！')</script>");
                }
            }
        }



        private void AddorEditMode(bool IsEdit)
        {
            if (IsEdit)
            {
                btAddEnum.Text = "编辑";
                btCancelButton.Visible = true;
                txtName.ReadOnly = true;
            }
            else
            {
                btAddEnum.Text = "增加";
                btCancelButton.Visible = false;
                txtName.ReadOnly = false;
            }
        }

        protected void btCancelButton_Click(object sender, EventArgs e)
        {
            AddorEditMode(false);
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtValueInfo.Text = string.Empty;
            EditKey.Text = string.Empty;
        }

        protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "sel":
                    {
                        EnuminfoBLL con = new EnuminfoBLL();
                        EnumInfoModel info = con.GetModel(id);
                        txtName.Text = info.enumName;
                        txtDescription.Text = info.enumDescription;
                        txtValueInfo.Text = info.enumValueInfo;
                        AddorEditMode(true);
                        EditKey.Text = e.CommandArgument.ToString();
                    }
                    break;
                case "del":
                    {
                        EnuminfoBLL con = new EnuminfoBLL();
                        con.Delete(id);
                        Bind();
                    }
                    break;
            }
        }

        protected void btRefreshCache_Click(object sender, EventArgs e)
        {
            Mainfun.LoadEnumApplication(SlnID,true);
        }
    }
}
