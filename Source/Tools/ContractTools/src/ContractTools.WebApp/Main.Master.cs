using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZyGames.Framework.Common;

namespace ContractTools.WebApp
{
    public partial class Main : System.Web.UI.MasterPage
    {
        private string GetCookesKey(string key)
        {
            return string.Format("__Contract{0}", key);
        }
        protected void SetCookies(string key, string value)
        {
            Response.Cookies[GetCookesKey(key)].Value = value;
            Response.Cookies[GetCookesKey(key)].Expires = DateTime.Now.AddMonths(1);
        }
        protected string GetCookies(string key)
        {
            if (Request.Cookies[GetCookesKey(key)] != null)
            {
                return Request.Cookies[GetCookesKey(key)].Value;
            }
            else
            {
                return string.Empty;
            }
        }
        protected int VerID
        {
            get
            {
                string slnId = SlnID;
                if (string.IsNullOrEmpty(Request["VerID"]))
                {
                    string val = GetCookies(slnId + "_Ver");
                    int verId;
                    if (int.TryParse(val, out verId))
                    {
                        return verId;
                    }
                    return 0;
                }
                return Convert.ToInt32(Request["VerID"]);
            }
        }

        protected string ContractID
        {
            get
            {
                string slnId = SlnID;
                if (string.IsNullOrEmpty(Request["ID"]))
                {
                    if (string.IsNullOrEmpty(GetCookies(slnId)))
                    {
                        return "0";
                    }
                    return GetCookies(slnId);
                }
                return Request.QueryString["ID"];
            }
        }

        protected string SlnID
        {
            get
            {
                if (string.IsNullOrEmpty(Request["slnID"]))
                {
                    if (string.IsNullOrEmpty(GetCookies(string.Empty)))
                    {
                        return "0";
                    }
                    return GetCookies(string.Empty);
                }
                return Request["slnID"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind(SlnID.ToInt(), VerID, ContractID.ToInt());
            }
        }
        private void Bind(int slnId, int versionId, int contractId)
        {
            string get = string.Format("?ID={0}&slnID={1}&VerID={2}&GameID={1}", contractId, slnId, versionId);
            hlSolution.NavigateUrl = "Solutions.aspx";
            hlVersion.NavigateUrl = "VersionEdit.aspx" + get;
            hlEnum.NavigateUrl = "EnumEdit.aspx" + get;
            hlAgreement.NavigateUrl = "AgreementEdit.aspx" + get;
            hlContract.NavigateUrl = "ContractEdit.aspx" + get;
            hlContractEdit.NavigateUrl = "ContractEdit.aspx" + get + "&modify=true";
            hlContractCopy.NavigateUrl = "ContractCopy.aspx" + get;
        }
    }
}