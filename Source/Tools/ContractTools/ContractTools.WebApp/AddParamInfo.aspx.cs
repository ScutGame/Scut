using System;
using System.Data;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;

namespace ContractTools.WebApp
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
            mode.Field = txtField.Text.Trim();
            mode.FieldValue = txtFieldValue.Text.Trim();
            mode.Remark = txtRemark.Text.Trim();
            mode.ContractID = ContractID;
            mode.FieldType = droFieldType.SelectedValue.ToEnum<FieldType>();
            mode.ParamType = Convert.ToInt32((string)droParamType.SelectedValue);
            mode.Required = Convert.ToBoolean((string)droRrequired.SelectedValue);
            mode.Descption = txtDescption.Text.Trim();
            mode.SlnID = SlnID;
            mode.MinValue = Convert.ToInt32((string)txtMinValue.Text.Trim());
            mode.MaxValue = Convert.ToInt32((string)txtMaxValue.Text.Trim());

            var paramList = DbDataLoader.GetParamInfo(SlnID, ContractID, mode.ParamType);
            if (paramList.Count == 0 || paramList[0].SortID == 0)
            {
                mode.SortID = 1;
            }
            else
            {
                int SortID = paramList[0].SortID;
                SortID++;
                mode.SortID = SortID;

            }

            if (DbDataLoader.Add(mode) > 0)
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
