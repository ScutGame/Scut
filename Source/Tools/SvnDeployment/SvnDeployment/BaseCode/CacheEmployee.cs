using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CitGame;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

/// <summary>
/// CacheEmployee 的摘要说明
/// </summary>
public class CacheEmployee:BaseCache
{
    private const string cstCacheKey = "LOACacheEmployee";
	public CacheEmployee():base(cstCacheKey)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this.cachekey = cstCacheKey;
	}

    public BaseEmployee GetEmployee(int aUserId)
    {
        List<BaseEmployee> listBase = (List<BaseEmployee>)this.getCache();
        if (listBase != null)
        {
            for (int i = 0; i < listBase.Count; i++)
            {
                if (listBase[i].UserId == aUserId)
                {
                    return listBase[i];
                }
            }
        }
        return null;
    }


    public List<BaseEmployee> getAllEmployee()
    {
        return (List<BaseEmployee>)this.getCache();
    }

    protected override bool InitCache()
    {
        try
        {
            string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string sGetSql = "select [userid],[username],[userpassword],[useremail],[userinfo],[usergroupid],[userCName] from GameOa.dbo.oa_user ";
                using (SqlDataReader aReader = SqlHelper.ExecuteReader(con, CommandType.Text, sGetSql))
                {
                    if (aReader == null) throw new Exception();
                    List<BaseEmployee> listBaseEmp = new List<BaseEmployee>();
                    if (aReader.HasRows)
                    {
                        while (aReader.Read())
                        {
                            BaseEmployee oTmpEmployee = new BaseEmployee(Convert.ToInt32(aReader["userid"]));
                            oTmpEmployee.InitData(aReader);
                            listBaseEmp.Add(oTmpEmployee);
                        }
                    }
                    this.addCache(listBaseEmp);
                    return true;
                }

        }
        catch (Exception ex)
        {
            this.SaveLog(ex);
            return false;
        }
    }
}
