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
    public partial class SolutionAdd : System.Web.UI.Page
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
                SolutionBLL con = new SolutionBLL();
                SolutionModel model = new SolutionModel();
                model.SlnName = txtDescption.Text.Trim();
                model.Namespace = txtNamespace.Text.Trim();
                model.RefNamespace = txtRefNamespace.Text.Trim();
                model.GameID = Convert.ToInt32(txtGameID.Text);
                model.Url = txtUrl.Text.Trim();
                if (con.Add(model) > 0)
                {
                    Response.Redirect("SolutionsList.aspx");
                }

            }
            catch(Exception ex)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");          
            }
        }

    }
}
