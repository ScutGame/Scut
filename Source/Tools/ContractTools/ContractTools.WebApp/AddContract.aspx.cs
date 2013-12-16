using System;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;

namespace ContractTools.WebApp
{
    /// <summary>
    /// 增加修改协议
    /// </summary>
    public partial class AddContract : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlAgreement.Items.Clear();
                var agreementList = DbDataLoader.GetAgreement(SlnID);
                ddlAgreement.DataSource = agreementList;
                ddlAgreement.DataTextField = "Title";
                ddlAgreement.DataValueField = "AgreementID";
                ddlAgreement.DataBind();
               
                if(ddlAgreement.Items.Count==0)
                {
                    ddlAgreement.Items.Add(new ListItem("无接口分类","0"));
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
            try
            {
                
                ContractModel model = new ContractModel();
                model.ID = Convert.ToInt32((string) txtID.Text.Trim());
                model.Descption = txtDescption.Text.Trim();
                model.ParentID = 1;
                model.SlnID = SlnID;
                model.AgreementID = ddlAgreement.SelectedValue.ToInt();
                if (DbDataLoader.Add(model) > 0)
                {
                    Response.Redirect(String.Format("index.aspx?ID={0}&slnID={1}", model.ID, model.SlnID));
                    return;
                }

            }
            catch(Exception ex)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");          
            }
        }
    }
}
