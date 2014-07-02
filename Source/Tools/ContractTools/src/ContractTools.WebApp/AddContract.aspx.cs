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
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;

namespace ContractTools.WebApp
{
    /// <summary>
    /// 增加修改协议
    /// </summary>
    public partial class AddContract : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    ddlAgreement.Items.Clear();
                    var agreementList = DbDataLoader.GetAgreement(SlnID);
                    ddlAgreement.DataSource = agreementList;
                    ddlAgreement.DataTextField = "Title";
                    ddlAgreement.DataValueField = "AgreementID";
                    ddlAgreement.DataBind();

                    if (ddlAgreement.Items.Count == 0)
                    {
                        ddlAgreement.Items.Add(new ListItem("选择分类", "0"));
                    }

                    ddVersion.Items.Clear();
                    var versiontList = DbDataLoader.GetVersion(SlnID);
                    versiontList.Insert(0, new VersionMode() { ID = 0, SlnID = SlnID, Title = "选择版本" });
                    ddVersion.DataSource = versiontList;
                    ddVersion.DataTextField = "Title";
                    ddVersion.DataValueField = "ID";
                    ddVersion.DataBind();
                    ddVersion.SelectedValue = VerID.ToString();
                }
                catch (Exception)
                {
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
        protected int VerID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["VerID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request["VerID"]);
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
            try
            {

                ContractModel model = new ContractModel();
                model.ID = Convert.ToInt32((string)txtID.Text.Trim());
                model.Descption = txtDescption.Text.Trim();
                model.ParentID = 1;
                model.SlnID = SlnID;
                model.VerID = Convert.ToInt32(ddVersion.Text.Trim());
                model.AgreementID = ddlAgreement.SelectedValue.ToInt();
                if (DbDataLoader.Add(model) > 0)
                {
                    Response.Redirect(String.Format("index.aspx?ID={0}&slnID={1}&VerID={2}", model.ID, model.SlnID, model.VerID));
                    return;
                }

            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");
            }
        }
    }
}