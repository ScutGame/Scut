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
    public partial class AddContract : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlAgreement.Items.Clear();
                DataSet ds = SlnID == 0 ? new AgreementBLL().GetList("", "0") : new AgreementBLL().GetList(" gameid=" + SlnID, SlnID.ToString());
                ddlAgreement.DataSource = ds;
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
                
                ContractBLL con = new ContractBLL();
                ContractModel model = new ContractModel();
                model.ID = Convert.ToInt32(txtID.Text.Trim());
                model.Descption = txtDescption.Text.Trim();
                model.ParentID = 1;
                model.SlnID = SlnID;
                //model.AgreementID = Convert.ToInt32(ddlAgreement.SelectedValue);
                if (con.Add(model))
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
