using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BLL;
using model;

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
                AgreementBLL con = new AgreementBLL();
                AgreementModel model = new AgreementModel();
                model.Title = Title.Text.Trim();
                model.Describe = Describe.Text.Trim();
                model.GameID = Convert.ToInt32(Request.QueryString["gameid"]);
                if (con.Add(model) > 0)
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
