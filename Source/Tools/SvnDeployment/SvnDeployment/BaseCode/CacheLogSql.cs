using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using CitGame;
/// <summary>
/// CacheLogSql 的摘要说明
/// </summary>
public class CacheLogSql:BaseCache
{
    private const string cacheKey = "LCacheLogSql";
     
	public CacheLogSql():base(cacheKey)
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
        this.cachekey = cacheKey;
	}

    public void addLogList(LogSql ologSql)
    {
        ActiveLog oActiveLog = (ActiveLog)this.getCache();
        oActiveLog.addLogList(ologSql);
    }

    protected override bool InitCache()
    {
        try
        {
            ActiveLog oActiveLog = new ActiveLog();
            this.addCache(oActiveLog);
            return true;
        }
        catch
        {
            return false;
        }
    }
}


public class ActiveLog
{
    private System.Timers.Timer otime;
    private BaseLog oBaselog;
    private List<LogSql> _listSql;
    public ActiveLog()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        this.oBaselog = new BaseLog("LActiveTest");
        this.otime = new System.Timers.Timer();
        this.otime.Interval = 10000;
        this.otime.Elapsed += new System.Timers.ElapsedEventHandler(otime_Elapsed);
        this.otime.Start();
        this._listSql = new List<LogSql>();
    }
    public void addLogList(LogSql oLogSql)
    {        
        this._listSql.Add(oLogSql);
    }
    void otime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (this._listSql.Count >=10)
        {
            lock (_listSql)
            {
                for (int i = 0; i < _listSql.Count; i++)
                {
                    this.oBaselog.SaveLog(_listSql[i].ExecSql + "\r\n" + _listSql[i].ExecParams);
                }
                _listSql.Clear();
            }
        }
    }
}


public class LogSql
{
    private string _ExecSql;
    private string _ExecParams;
    private DateTime _dateTime;

    public string ExecParams { get { return _ExecParams; } }
    public string ExecSql { get { return _ExecSql; } }
    public DateTime ExecDateTime { get { return _dateTime; } }

    public LogSql()
    {

    }

    public void SetLog(string commandText)
    {

    }

    public void SetLog(string commandText, SqlParameter[] commandParams)
    {
        if (commandParams == null)
        {
            this._ExecParams = "";
        }
        else
        {
          //  this._ExecParams = "";
            
            for (int i = 0; i < commandParams.Length; i++)
            {
                if (commandParams[i] == null) continue;
                this._ExecParams += commandParams[i].ParameterName + "=" + commandParams[i].Value + ";";
            }
        }

        this._ExecSql = commandText;
        _dateTime = DateTime.Now;
    }
}