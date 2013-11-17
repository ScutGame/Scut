using System;
using BLL;
using System.Text;
using System.Data;

namespace ZyGames.ContractTools
{
    public partial class ClientConfigInfo : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                this.txtServerUrl.Text = new SolutionBLL().GetUrl(SlnID);
                ddlContract.Items.Clear();
                ContractBLL BLL = new ContractBLL();
                DataSet ds = BLL.GetList("SlnID=" + SlnID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlContract.DataSource = ds;
                    ddlContract.DataTextField = "uname";
                    ddlContract.DataValueField = "ID";
                    ddlContract.DataBind();
                    ddlContract.SelectedValue = ContractID.ToString();
                }
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            int contractID = int.Parse(ddlContract.Text);
            Message msg = new Message();

            string requestParams = GetRequestParams("", "0", contractID, SlnID, txtVersion.Text);
            //StringBuilder requestParams = new StringBuilder();
            //requestParams.AppendFormat("Sid={0}&Uid={1}&ActionID={2}&ClientVersion={3}&rl=1", string.Empty, string.Empty, contractID, txtVersion.Text);
            string serverUrl = txtServerUrl.Text;
            string[] keyNames = txtKeyName.Text.Split(new char[] { ',' });

            MessageReader msgReader = MessageReader.Create(serverUrl, requestParams, ref msg, false);
            if (msgReader != null)
            {
                try
                {
                    if (msg.ErrorCode != 0)
                    {
                        txtResponse.Text = msg.ErrorInfo;
                    }
                    else
                    {
                        txtResponse.Text = new ParamInfoBLL().LuaConfig(SlnID, contractID, keyNames, msgReader);
                    }
                }
                catch (Exception ex)
                {
                    txtResponse.Text = ex.ToString();
                }
                finally
                {
                    msgReader.Dispose();
                }
            }
        }

        private static string GetRequestParams(string sid, string uid, int contractId, int slnId, string clientVersion)
        {
            StringBuilder requestParams = new StringBuilder();
            requestParams.AppendFormat("Sid={0}&Uid={1}&ActionID={2}&ClientVersion={3}&rl=1", sid, uid, contractId, clientVersion);

            DataSet reqParamList = new ParamInfoBLL().GetList(string.Format("ContractID={0} and SlnID={1} and ParamType=1", contractId, slnId));
            DataRowCollection paramRecords = reqParamList.Tables[0].Rows;

            int i = 0;
            foreach (DataRow record in paramRecords)
            {
                if (requestParams.Length > 0)
                {
                    requestParams.Append("&");
                }
                string fieldName = record["Field"].ToString();
                string fieldValue = record["FieldValue"].ToString();

                requestParams.AppendFormat("{0}={1}", fieldName, fieldValue);
                i++;
            }
            return requestParams.ToString();
        }

    }
}
