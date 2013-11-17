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
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

/// <summary>
/// Formula 的摘要说明
/// </summary>
public class Formula
{
    
    private int _formulaId;
    private string _formulaName;
    private int _formulaType;
    private string _formulaDesc;
    private int _MustFactoryLevel;

    public int formulaId 
    {
        get { return _formulaId; } 
    }

    public string Name
    {
        get { return _formulaName; }
        set 
        {
            _formulaName = value;
        }
    }

    public int fType
    {
        get { return _formulaType; }

    }
    public string Desc
    {
        get { return _formulaDesc; }
    }

    public int MustFactoryLevel
    {
        get { return _MustFactoryLevel; }
    }

	public Formula(int aFormulaId)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this._formulaId = aFormulaId;
	}

    public bool GetFormula()
    {
        bool bHasFormula = false;
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select * from ConfigFormula where formulaid=@aformulaid";
            SqlParameter[] paramsGet = new SqlParameter[1];
            paramsGet[0] = SqlParamHelper.MakeInParam("@aformulaid", SqlDbType.Int, 0, _formulaId);
            using (SqlDataReader aReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql, paramsGet))
            {
                if (aReader == null) throw new Exception();
                if (aReader.HasRows)
                {
                    bHasFormula = true;
                    aReader.Read();
                    this.InitData(aReader);
                }               
            }

            if (bHasFormula)
            {

            }

        return bHasFormula;
    }

    public void InitData(SqlDataReader aReader)
    {
        _formulaDesc = Convert.ToString(aReader["formulaDesc"]);
        _formulaId = Convert.ToInt32(aReader["formulaId"]);
        _formulaName = Convert.ToString(aReader["formulaName"]);
        _formulaType = Convert.ToInt32(aReader["formulaType"]);
        _MustFactoryLevel = Convert.ToInt32(aReader["MustFactoryLevel"]);
    }
}
