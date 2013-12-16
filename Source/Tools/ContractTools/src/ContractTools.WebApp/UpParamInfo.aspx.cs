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
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;

namespace ContractTools.WebApp
{
    /// <summary>
    /// 修改协议
    /// </summary>
    public partial class UpParamInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    Response.Redirect("index.aspx");
                }
                else
                {
                    IdLabel.Text = Request.QueryString["ID"];
                    Bind();
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
            ContractModel model = new ContractModel();
            model.ID = ContractID;
            model.SlnID = SlnID;
            model.Descption = txtDescption.Text.Trim();
            model.AgreementID = Convert.ToInt32((string) ddlAgreement.SelectedValue);
            if (DbDataLoader.Update(model))
            {
                Response.Write("<script language=javascript>alert('修改成功！')</script>");
            }
        }
        /// <summary>
        /// 初始化加载
        /// </summary>
        public void Bind()
        {
            ddlAgreement.Items.Clear();
            var list = DbDataLoader.GetAgreement(SlnID);
            ddlAgreement.DataSource = list;
            ddlAgreement.DataTextField = "Title";
            ddlAgreement.DataValueField = "AgreementID";
            ddlAgreement.DataBind();

            if (ddlAgreement.Items.Count == 0)
            {
                ddlAgreement.Items.Add(new ListItem("无接口分类", "0"));
            }
            if (!Request.QueryString["ID"].Equals(""))
            {
                ContractModel model = DbDataLoader.GetContract(SlnID, ContractID);
                if (model != null)
                {
                    txtDescption.Text = model.Descption;
                    ddlAgreement.SelectedValue = model.AgreementID.ToString();
                }
            }


        }
    }
}