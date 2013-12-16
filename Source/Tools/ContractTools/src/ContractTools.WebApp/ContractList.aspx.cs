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
using ContractTools.WebApp.Base;

namespace ContractTools.WebApp
{
    public partial class ContractList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSlnID.Text = SlnID.ToString();
                txtCopyID.Text = ContractID.ToString();
                Bind(SlnID);
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

        private void Bind(int slnID)
        {
            ddlSolution.Items.Clear();
            var slnList = DbDataLoader.GetSolution();
            ddlSolution.DataSource = slnList;
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();

            var slnModel = slnList.Where(p => p.SlnID == slnID).FirstOrDefault();
            if (slnModel != null)
            {
                lblSlnName.Text = slnModel.SlnName;
            }

            ddContract.Items.Clear();
            var contractList = DbDataLoader.GetContract(slnID);
            if (contractList.Count > 0)
            {
                ddContract.DataSource = contractList;
                ddContract.DataTextField = "uname";
                ddContract.DataValueField = "ID";
                ddContract.DataBind();

                ddContract.SelectedValue = ContractID.ToString();
            }
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            if (txtSlnID.Text.Trim() == ddlSolution.Text.Trim() &&
                txtCopyID.Text.Trim() == ddContract.Text)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('不能复制至相同项目方案！')</script>");
                return;
            }

            if (DbDataLoader.CopyContract(int.Parse(txtSlnID.Text), int.Parse(ddContract.Text), int.Parse(ddlSolution.Text), int.Parse(txtCopyID.Text)))
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('复制成功！')</script>");
            }
            else
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('复制失败！')</script>");
            }
        }

        protected void ddContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtCopyID.Text = ddContract.Text;
        }
    }
}