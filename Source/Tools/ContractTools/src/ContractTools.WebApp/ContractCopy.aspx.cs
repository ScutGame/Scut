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
    public partial class ContractCopy : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSlnID.Text = SlnID.ToString();
                txtCopyID.Text = ContractID.ToString();
                txtVerID.Text = VerID.ToString();
                Bind(SlnID, ContractID, SlnID, ContractID, VerID);
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

        private void Bind(int slnID, int contractID, int newSlnId, int newContractID, int verId)
        {
            ddlSolution.Items.Clear();
            var slnList = DbDataLoader.GetSolution();
            ddlSolution.DataSource = slnList;
            ddlSolution.DataTextField = "SlnName";
            ddlSolution.DataValueField = "SlnID";
            ddlSolution.DataBind();
            ddlSolution.SelectedValue = newSlnId.ToString();

            var slnModel = slnList.Where(p => p.SlnID == slnID).FirstOrDefault();
            if (slnModel != null)
            {
                lblSlnName.Text = slnModel.SlnName;
            }

            ddContract.Items.Clear();
            var contractList = DbDataLoader.GetContract(slnID, verId);
            if (contractList.Count > 0)
            {
                ddContract.DataSource = contractList;
                ddContract.DataTextField = "uname";
                ddContract.DataValueField = "ID";
                ddContract.DataBind();

                ddContract.SelectedValue = contractID.ToString();
            }
            List<ParamInfoModel> requestParams;
            List<ParamInfoModel> responseParams;
            GetParamInfo(slnID, contractID, verId, out requestParams, out responseParams);
            int paramtype = 2;
            BindResponseParams(paramtype == 1 ? requestParams : responseParams);

            GetParamInfo(newSlnId, newContractID, 0, out requestParams, out responseParams);
            BindNewResponseParams(paramtype == 1 ? requestParams : responseParams);
        }
        private void BindResponseParams(List<ParamInfoModel> list)
        {
            ddParamCopyFrom.DataSource = list;
            ddParamCopyFrom.DataTextField = "ComboxDescp";
            ddParamCopyFrom.DataValueField = "SortID";
            ddParamCopyFrom.DataBind();

            ddParamCopyTo.DataSource = list;
            ddParamCopyTo.DataTextField = "ComboxDescp";
            ddParamCopyTo.DataValueField = "SortID";
            ddParamCopyTo.DataBind();
        }

        private void BindNewResponseParams(List<ParamInfoModel> list)
        {
            ddResponseParams.DataSource = list;
            ddResponseParams.DataTextField = "ComboxDescp";
            ddResponseParams.DataValueField = "SortID";
            ddResponseParams.DataBind();
            if (list.Count > 0)
            {
                ddResponseParams.Items.Insert(0, new ListItem("<First>", "0"));
                ddResponseParams.SelectedValue = (list[list.Count - 1].SortID).ToString();
            }
        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {
            if (txtSlnID.Text.Trim() == ddlSolution.Text.Trim() &&
                txtCopyID.Text.Trim() == ddContract.Text)
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('不能复制至相同项目方案！')</script>");
                return;
            }

            if (DbDataLoader.CopyContract(int.Parse(txtSlnID.Text), int.Parse(ddContract.Text), int.Parse(ddlSolution.Text), int.Parse(txtCopyID.Text)))
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('复制成功！')</script>");
            }
            else
            {
                Page.RegisterStartupScript("", "<script language=javascript>alert('复制失败！')</script>");
            }
        }

        protected void ddContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind(txtSlnID.Text.ToInt(), ddContract.Text.ToInt(), ddlSolution.Text.ToInt(), txtCopyID.Text.ToInt(), txtVerID.Text.ToInt());

        }

        protected void btnRefesh_Click(object sender, EventArgs e)
        {
            Bind(txtSlnID.Text.ToInt(), ddContract.Text.ToInt(), ddlSolution.Text.ToInt(), txtCopyID.Text.ToInt(), txtVerID.Text.ToInt());

        }

        protected void btnCopyParam_Click(object sender, EventArgs e)
        {
            try
            {
                int sortFrom = ddParamCopyFrom.Text.ToInt();
                int sortTo = ddParamCopyTo.Text.ToInt();
                if (sortFrom > sortTo) return;

                int paramType = 2;
                int insertPos = ddResponseParams.Text.ToInt();
                int copySlnId = txtSlnID.Text.ToInt();
                int copyContractId = ddContract.Text.ToInt();
                int verId = txtVerID.Text.ToInt();
                var copyParamList = DbDataLoader.GetParamInfo(copySlnId, copyContractId, paramType, verId);
                var copyList = copyParamList.FindAll(t => t.SortID >= sortFrom && t.SortID <= sortTo);
                int sortId = insertPos + copyList.Count;

                int slnId = ddlSolution.Text.ToInt();
                int contractId = txtCopyID.Text.ToInt();
                var paramList = DbDataLoader.GetParamInfo(slnId, contractId, paramType, 0);
                paramList = paramList.FindAll(t => t.SortID >= insertPos);

                foreach (var param in paramList)
                {
                    if (param.SortID > insertPos)
                    {
                        sortId++;
                        DbDataLoader.UpdateParamSort(param.ID, sortId);
                    }
                }

                sortId = insertPos;
                foreach (var param in copyList)
                {
                    sortId++;
                    param.SlnID = slnId;
                    param.ContractID = contractId;
                    param.SortID = sortId;
                    param.VerID = verId;
                    param.ModifyDate = DateTime.MinValue;
                    param.CreateDate = DateTime.Now;
                    DbDataLoader.Add(param);
                }

            }
            catch (Exception ex)
            {
                TraceLog.WriteError("Default ParamCopy error:{0}", ex);
            }
        }

    }
}