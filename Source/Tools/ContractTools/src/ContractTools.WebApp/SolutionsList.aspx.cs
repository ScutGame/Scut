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

namespace ContractTools.WebApp
{
    public partial class SolutionsList : System.Web.UI.Page
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
                int id = Convert.ToInt32((string) GridView1.DataKeys[e.RowIndex].Values[0].ToString());
                string SlnName = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("SlnName")).Text;
                string Namespace = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Namespace")).Text;
                string RefNamespace = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("RefNamespace")).Text;
                string url = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("Url")).Text;
                string gameid = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("gameid")).Text;

                SolutionModel mode = new SolutionModel();
                mode.SlnID = id;
                mode.SlnName = SlnName;
                mode.Namespace = Namespace;
                mode.RefNamespace = RefNamespace;
                mode.Url = url;
                mode.GameID = Convert.ToInt32(gameid);
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
                int id = Convert.ToInt32((string) GridView1.DataKeys[e.RowIndex].Values[0].ToString());

                if (DbDataLoader.Delete(new SolutionModel(){SlnID = id}))
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
            return value.ToString().Replace("\r\n","<br>");
        }
    }
}