using System;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;

namespace ContractTools.WebApp
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
                SolutionModel model = new SolutionModel();
                model.SlnName = txtDescption.Text.Trim();
                model.Namespace = txtNamespace.Text.Trim();
                model.RefNamespace = txtRefNamespace.Text.Trim();
                model.GameID = Convert.ToInt32((string) txtGameID.Text);
                model.Url = txtUrl.Text.Trim();
                if (DbDataLoader.Add(model) > 0)
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
