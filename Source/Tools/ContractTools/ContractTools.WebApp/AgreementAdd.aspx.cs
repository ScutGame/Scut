using System;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;

namespace ZyGames.ContractTools
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
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");          
            }
        }

    }
}
