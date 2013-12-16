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
using System.Data;
using ContractTools.WebApp.Base;

namespace ContractTools.WebApp
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["search"]))
                {
                    SearchTextBox.Text = Request["search"];
                }
                BindData();
            }
        }

        private void BindData()
        {
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                var list = DbDataLoader.GetContract(f =>
                {
                    f.Condition = string.Format("({0} OR {1}) AND {2}",
                        f.FormatExpression("Descption", "LIKE", "Arg"),
                        f.FormatExpression("ID", "LIKE", "Arg"),
                        f.FormatExpression("SlnID"));
                    f.AddParam("Arg", string.Format("%{0}%",SearchTextBox.Text));
                    f.AddParam("SlnID", SlnID);
                });
                if (list.Count == 0)
                {
                    ResultLiteral.Visible = true;
                    ResultLiteral.Text = "查不到任何结果";
                }
                else
                {
                    ResultLiteral.Visible = false;
                    GridView.DataSource = list;
                    GridView.DataBind();
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

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}