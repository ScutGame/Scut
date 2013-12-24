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
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;

namespace ContractTools.WebApp
{
    /// <summary>
    /// 增加字段
    /// </summary>
    public partial class AddParamInfo : System.Web.UI.Page
    {
        public string UID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
            UID = LabType.Text;
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
        /// <summary>
        /// 初始化加载
        /// </summary>
        public void Bind()
        {


            if (!Request.QueryString["ID"].Equals(""))
            {
                string ID = Request.QueryString["ID"];
                LabType.Text = ID;

            }
            else
            {
                Response.Redirect("index.aspx");
            }

        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            txtField.Text = "";
            txtDescption.Text = "";
            txtFieldValue.Text = "";
            txtRemark.Text = "";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ParamInfoModel mode = new ParamInfoModel();
            mode.Field = txtField.Text.Trim();
            mode.FieldValue = txtFieldValue.Text.Trim();
            mode.Remark = txtRemark.Text.Trim();
            mode.ContractID = ContractID;
            mode.FieldType = droFieldType.SelectedValue.ToEnum<FieldType>();
            mode.ParamType = Convert.ToInt32((string)droParamType.SelectedValue);
            mode.Required = Convert.ToBoolean((string)droRrequired.SelectedValue);
            mode.Descption = txtDescption.Text.Trim();
            mode.SlnID = SlnID;
            mode.MinValue = Convert.ToInt32((string)txtMinValue.Text.Trim());
            mode.MaxValue = Convert.ToInt32((string)txtMaxValue.Text.Trim());
            mode.CreateDate = DateTime.Now;

            var paramList = DbDataLoader.GetParamInfo(SlnID, ContractID, mode.ParamType).OrderBy(p => p.SortID).ToList();
            if (paramList.Count == 0 || paramList[0].SortID == 0)
            {
                mode.SortID = 1;
            }
            else
            {
                int SortID = paramList[paramList.Count - 1].SortID;
                SortID++;
                mode.SortID = SortID;

            }

            if (DbDataLoader.Add(mode) > 0)
            {
                Response.Redirect(String.Format("index.aspx?ID={0}&slnID={1}", ContractID, mode.SlnID));
            }
            else
            {
                Response.Write("<script language=javascript>alert('增加失败！')</script>");
            }

        }
    }
}