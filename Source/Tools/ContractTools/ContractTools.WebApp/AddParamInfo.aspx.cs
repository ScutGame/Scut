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
    /// 增加字段
    /// </summary>
    public partial class AddParamInfo : System.Web.UI.Page
    {
        public string UID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ViewState["slnID"] = Request.Params["slnID"];
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
            ParamInfoBLL BLL = new ParamInfoBLL();
            mode.Field = txtField.Text.Trim();
            mode.FieldValue = txtFieldValue.Text.Trim();
            mode.Remark = txtRemark.Text.Trim();
            mode.ContractID = ContractID;
            mode.FieldType = Convert.ToInt32(droFieldType.SelectedValue);
            mode.ParamType = Convert.ToInt32(droParamType.SelectedValue);
            mode.Required = Convert.ToBoolean(droRrequired.SelectedValue);
            mode.Descption = txtDescption.Text.Trim();
            mode.SlnID = SlnID;
            mode.MinValue = Convert.ToInt32(txtMinValue.Text.Trim());
            mode.MaxValue = Convert.ToInt32(txtMaxValue.Text.Trim());

            DataSet ds = BLL.GetID(string.Format("ContractID={0} and slnid={1} and ParamType={2}", ContractID, SlnID, mode.ParamType));
            if (ds.Tables[0].Rows[0]["SortID"].ToString() == "")
            {
                mode.SortID = 1;
            }
            else
            {
                int SortID = Convert.ToInt32(ds.Tables[0].Rows[0]["SortID"]);
                SortID++;
                mode.SortID = SortID;

            }

            if (BLL.Add(mode) != 0)
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
