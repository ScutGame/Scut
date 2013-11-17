using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Data.SqlClient;
/// <summary>
/// Author:匆匆  Blog:http://www.cnblogs.com/huangjianhuakarl/
/// 数据库操作类
/// </summary>
public class Common
{
    private static SqlConnection conn = new SqlConnection();
    private static SqlCommand comm = new SqlCommand();
	public Common()
	{ 
        
	}
    /// <summary>
    /// 打开连接
    /// </summary>
    private static void openConnection()
    {
        if (conn.State == ConnectionState.Closed)
        {
            try
            {
                conn.ConnectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["CitGameDB"].ConnectionString;
                comm.Connection = conn;
                conn.Open();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
    /// <summary>
    /// 关闭连接
    /// </summary>
    private static void closeConnection()
    {
        if (conn.State == ConnectionState.Open)
        {
            conn.Close();
        }
        conn.Dispose();
        comm.Dispose();
    }
    /// <summary>
    /// 执行一条sql语句
    /// </summary>
    /// <param name="sqlStr">sql语句</param>
    public static void ExecuteSql(string sqlStr)
    {
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlStr;
            comm.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
    }
    /// <summary>
    /// 返回一个数据集
    /// </summary>
    /// <param name="sqlStr">sql语句</param>
    /// <returns></returns>
    public static DataSet dataSet(string sqlStr)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlStr;
            da.SelectCommand = comm;
            da.Fill(ds);
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
        return ds;
    }
    /// <summary>
    /// 返回一个数据视图
    /// </summary>
    /// <param name="sqlStr">sql语句</param>
    /// <returns></returns>
    public static DataView dataView(string sqlStr)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataView dv = new DataView();
        DataSet ds = new DataSet();
        try
        {
            openConnection();
            comm.CommandType = CommandType.Text;
            comm.CommandText = sqlStr;
            da.SelectCommand = comm;
            da.Fill(ds);
            dv = ds.Tables[0].DefaultView;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            closeConnection();
        }
        return dv;
    }
}
