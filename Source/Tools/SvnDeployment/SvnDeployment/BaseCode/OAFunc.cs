using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Func 的摘要说明
/// </summary>
public class OAFunc
{
	public OAFunc()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 获取当前月份的第一天的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime getMonthFirstDay()
    {
        DateTime tmpDate = DateTime.Now;
        string sFirstDate = tmpDate.Year + "-" + tmpDate.Month + "-01";
        return Convert.ToDateTime(sFirstDate);
    }

    /// <summary>
    /// 获取明天的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime getTomorrow()
    {
        return DateTime.Now.AddDays(1);
    }
    /// <summary>
    /// 获取昨天的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime getYesterday()
    {
        return DateTime.Now.AddDays(-1);
    }

    /// <summary>
    /// 获取明天的日期
    /// </summary>
    /// <returns></returns>
    public static DateTime getNextDay(string dateValue)
    {
        DateTime dateTime = DateTime.Parse(dateValue);
        return dateTime.AddDays(1);        
    }
}
