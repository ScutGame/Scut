using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;

namespace ContractTools.WebApp
{
    public partial class ContractEdit : BasePage
    {
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
        protected int VerID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["VerID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request["VerID"]);
            }
        }
        protected int AgreementID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["AgreementID"]))
                {
                    return 0;
                }
                return Convert.ToInt32(Request["AgreementID"]);
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

        protected bool IsModify
        {
            get
            {
                return Request["modify"].ToBool();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int slnId = SlnID;
                    var agreementList = DbDataLoader.GetAgreement(slnId);
                    ddlAgreement.DataSource = agreementList;
                    ddlAgreement.DataTextField = "Title";
                    ddlAgreement.DataValueField = "AgreementID";
                    ddlAgreement.DataBind();

                    if (ddlAgreement.Items.Count == 0)
                    {
                        ddlAgreement.Items.Add(new ListItem("选择类别", "0"));
                    }


                    var versiontList = DbDataLoader.GetVersion(slnId);
                    versiontList.Insert(0, new VersionMode() { ID = 0, SlnID = slnId, Title = "选择版本" });
                    ddVersion.DataSource = versiontList;
                    ddVersion.DataTextField = "Title";
                    ddVersion.DataValueField = "ID";
                    ddVersion.DataBind();

                    if (IsModify)
                    {
                        ContractModel model = DbDataLoader.GetContract(slnId, ContractID, 0);
                        if (model != null)
                        {
                            txtID.Text = model.ID.ToString();
                            txtDescption.Text = model.Descption;
                            ddlAgreement.SelectedValue = model.AgreementID.ToString();
                            ddVersion.SelectedValue = model.VerID.ToString();
                            btnDelete.Visible = true;
                        }
                    }
                    else
                    {
                        ddlAgreement.SelectedValue = AgreementID.ToString();
                        btnDelete.Visible = false;
                        ddVersion.SelectedValue = VerID.ToString();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ContractModel model = new ContractModel();
                model.ID = Convert.ToInt32((string)txtID.Text.Trim());
                model.Descption = txtDescption.Text.Trim();
                model.ParentID = 1;
                model.SlnID = SlnID;
                model.VerID = Convert.ToInt32(ddVersion.Text.Trim());
                model.AgreementID = ddlAgreement.SelectedValue.ToInt();
                if (IsModify)
                {
                    DbDataLoader.Update(model);
                }
                else
                {
                    DbDataLoader.Add(model);
                }
                Response.Redirect(string.Format("Default.aspx?edit=true&ID={0}&slnID={1}&VerID={2}&GameID={1}&AgreementID={3}", model.ID, model.SlnID, model.VerID, model.AgreementID));
            }
            catch (Exception ex)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('添加失败,填写重复！')</script>");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var contractModel = new ContractModel() { ID = txtID.Text.ToInt(), SlnID = SlnID };
                if (DbDataLoader.Delete(contractModel))
                {
                    Response.Write("<script language=javascript>alert('删除成功！')</script>");
                    Response.Redirect("Default.aspx?edit=true");
                }
                else
                {
                    Response.Write("<script language=javascript>alert('删除失败！')</script>");
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("delete contract error:{0}", ex);
            }
        }


    }
}