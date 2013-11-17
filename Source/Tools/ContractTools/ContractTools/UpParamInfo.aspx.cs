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
    /// 修改协议
    /// </summary>
    public partial class UpParamInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    Response.Redirect("index.aspx");
                }
                else
                {
                    IdLabel.Text = Request.QueryString["ID"];
                    Bind();
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
            ContractBLL BLL = new ContractBLL();
            ContractModel model = new ContractModel();
            model.ID = ContractID;
            model.SlnID = SlnID;
            model.Descption = txtDescption.Text.Trim();
            model.AgreementID = Convert.ToInt32(ddlAgreement.SelectedValue);
            if (BLL.Update(model))
            {
                Response.Write("<script language=javascript>alert('修改成功！')</script>");
            }
        }
        /// <summary>
        /// 初始化加载
        /// </summary>
        public void Bind()
        {
            ddlAgreement.Items.Clear();
            DataSet ds = SlnID == 0 ? new AgreementBLL().GetList("", "0") : new AgreementBLL().GetList(" gameid=" + SlnID, SlnID.ToString());
            ddlAgreement.DataSource = ds;
            ddlAgreement.DataTextField = "Title";
            ddlAgreement.DataValueField = "AgreementID";
            ddlAgreement.DataBind();

            if (ddlAgreement.Items.Count == 0)
            {
                ddlAgreement.Items.Add(new ListItem("无接口分类", "0"));
            }
            if (!Request.QueryString["ID"].Equals(""))
            {
                int ID = Convert.ToInt32(Request.QueryString["ID"]);
                int slnID = Convert.ToInt32(Request.QueryString["slnID"]);
                ContractBLL BLL = new ContractBLL();
                ContractModel model = new ContractModel();
                model = BLL.GetModel(ContractID, SlnID);
                txtDescption.Text = model.Descption;
                ddlAgreement.SelectedValue = model.AgreementID.ToString();
            }


        }
    }
}
