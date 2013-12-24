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
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common.Log;

namespace ContractTools.WebApp
{
    /// <summary>
    /// 增加修改协议
    /// </summary>
    public partial class AgreementAdd : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                AgreementModel model = new AgreementModel();
                model.Title = Title.Text.Trim();
                model.Describe = Describe.Text.Trim();
                model.GameID = Convert.ToInt32(Request.QueryString["gameid"]);
                if (DbDataLoader.Add(model) > 0)
                {
                    Page.RegisterStartupScript("", "<script language=javascript>alert('添加成功！')</script>");      
                }

            }
            catch(Exception ex)
            {
                TraceLog.WriteError("AgreementAdd:{0}", ex);
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");          
            }
        }

    }
}