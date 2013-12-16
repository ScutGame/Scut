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
                ddlAgreement.Items.Clear();
                var agreementList = DbDataLoader.GetAgreement(SlnID);
                ddlAgreement.DataSource = agreementList;
                ddlAgreement.DataTextField = "Title";
                ddlAgreement.DataValueField = "AgreementID";
                ddlAgreement.DataBind();
               
                if(ddlAgreement.Items.Count==0)
                {
                    ddlAgreement.Items.Add(new ListItem("无接口分类","0"));
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

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                
                ContractModel model = new ContractModel();
                model.ID = Convert.ToInt32((string) txtID.Text.Trim());
                model.Descption = txtDescption.Text.Trim();
                model.ParentID = 1;
                model.SlnID = SlnID;
                model.AgreementID = ddlAgreement.SelectedValue.ToInt();
                if (DbDataLoader.Add(model) > 0)
                {
                    Response.Redirect(String.Format("index.aspx?ID={0}&slnID={1}", model.ID, model.SlnID));
                    return;
                }

            }
            catch(Exception ex)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");          
            }
        }
    }
}