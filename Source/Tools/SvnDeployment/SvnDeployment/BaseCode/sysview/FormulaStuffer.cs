using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// FormulaStuffer 的摘要说明
/// </summary>
public class FormulaStuffer
{
    private int _StufferListId;
    private int _Formulaid;
    private int _StufferShopId;
    private int _StufferNum;

	public FormulaStuffer()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public void InitData(SqlDataReader aReader)
    {
        _Formulaid = Convert.ToInt32(aReader["formulaid"]);
        _StufferListId = Convert.ToInt32(aReader["stufferlistid"]);
        _StufferNum = Convert.ToInt32(aReader["stufferNum"]);
        _StufferShopId = Convert.ToInt32(aReader["shopid"]);
    }
}
