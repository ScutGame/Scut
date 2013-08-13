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
            if (!Request.QueryString["ID"].Equals(""))
            {
                int ID = Convert.ToInt32(Request.QueryString["ID"]);
                int slnID = Convert.ToInt32(Request.QueryString["slnID"]);
                ContractBLL BLL = new ContractBLL();
                ContractModel model = new ContractModel();
                model = BLL.GetModel(ContractID, SlnID);
                txtDescption.Text = model.Descption;
            }
        }
    }
}
