/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Linq;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;

namespace ContractTools.WebApp
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
                    string enumName = Request["enum"].Trim(new char[] { '【', '】' });
                    txtName.Text = enumName;
                    EnumInfoModel info = DbDataLoader.GetEnumInfo(SlnID, enumName).FirstOrDefault();
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
            GridView.DataSource = DbDataLoader.GetEnumInfo(SlnID);
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
                    EnumInfoModel model = new EnumInfoModel();
                    model.enumName = txtName.Text;
                    model.enumDescription = txtDescription.Text;
                    model.enumValueInfo = txtValueInfo.Text;
                    model.SlnID = SlnID;
                    if (DbDataLoader.Add(model) > 0)
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

                    EnumInfoModel model = new EnumInfoModel();
                    model.enumName = txtName.Text;
                    model.enumDescription = txtDescription.Text;
                    model.enumValueInfo = txtValueInfo.Text;
                    model.SlnID = SlnID;
                    model.ID = int.Parse(EditKey.Text);
                    if (DbDataLoader.Update(model))
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
                        EnumInfoModel info = DbDataLoader.GetEnumInfo(f =>
                        {
                            f.Condition = f.FormatExpression("ID");
                            f.AddParam("ID", id);
                        }).FirstOrDefault();
                        txtName.Text = info.enumName;
                        txtDescription.Text = info.enumDescription;
                        txtValueInfo.Text = info.enumValueInfo;
                        AddorEditMode(true);
                        EditKey.Text = e.CommandArgument.ToString();
                    }
                    break;
                case "del":
                    {
                        DbDataLoader.Delete(new EnumInfoModel() { ID = id });
                        Bind();
                    }
                    break;
            }
        }

        protected void btRefreshCache_Click(object sender, EventArgs e)
        {
            TemplateHelper.LoadEnumApplication(SlnID, true);
        }
    }
}