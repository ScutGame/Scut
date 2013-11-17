using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.Data.OleDb;
using System.Text;
using ZyGames.Core.Data;
using ZyGames.SimpleManager.Service.Common;

/// <summary>
/// ExcelHelp 的摘要说明
/// </summary>
public class ExcelHelp
{
    public ExcelHelp()
    {
    }

    private string errorInfo;
    public string ErrorInfo
    {
        get { return errorInfo; }
        private set { errorInfo = value; }
    }

    /// <summary>
    /// 导入数据到数据库
    /// </summary>
    /// <param name="table">导入的数据表</param>
    /// <param name="tableName">数据库表名</param>
    public ImportState ImportSql(DataTable table, string tableName)
    {
        if (!TableExist(tableName)) //表名是否存在  
            return ImportState.TableNameError;

        if (table == null)
        {
            return ImportState.ExcelFormatError;
        }
        ArrayList tableField = GetTableField(tableName);   //表格的列名称 

        StringBuilder columnName = new StringBuilder();
        for (int i = 0; i < table.Columns.Count; i++)
        {
            columnName.Append(table.Columns[i].ColumnName + ",");
            string currentColumn = table.Columns[i].ToString().ToUpper(); //当前列名  
            for (int j = 0; j < tableField.Count; j++)
            {
                if (tableField[j].ToString().ToUpper() == table.Columns[i].ToString().ToUpper())
                    break;   //跳出本层和上一层循环，continue是跳出本层循环，如果用continue，会继续执行j++   Excel里的字段必须在Sql中都有  
                if ((tableField[j].ToString().ToUpper() != table.Columns[i].ToString().ToUpper()) && j == tableField.Count - 1)
                    return ImportState.FieldMatchError;
            }
        }
        columnName.Remove(columnName.Length - 1, 1);    //移除最后一个逗号
        StringBuilder value = null;
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            try
            {
                string strSql = string.Empty;
                for (int h = 0; h < table.Rows.Count; h++)
                {
                    value = new StringBuilder();
                    for (int k = 0; k < table.Columns.Count; k++) //根据列名得到值  
                    {
                        value.Append("'" + table.Rows[h][k].ToString() + "',");
                    }
                    value.Remove(value.Length - 1, 1);    //移除最后一个逗号

                    strSql = "insert into " + tableName + "(" + columnName.ToString() + ") values(" + value + ")";
                    SqlHelper.ExecuteNonQuery(con, CommandType.Text, strSql);   
                }
                //sqlTran.Commit();
                return ImportState.Success;
            }
            catch (Exception ex)
            {
                ErrorInfo = ex.Message;
                //sqlTran.Rollback();
                return ImportState.DataTypeError;
            }
    }

    /// <summary>
    /// 导入数据到数据库
    /// </summary>
    /// <param name="excelPath">excel文件完整路径</param>
    /// <param name="tableName">数据库表名</param>
    public ImportState ImportSql(string excelPath, string tableName)
    {
        DataSet ds = ExcelToDataTable(excelPath);
        if (ds == null)
        {
            return ImportState.ExcelFormatError;
        }
        return ImportSql(ds.Tables[0], tableName);
    }

    /// <summary>
    /// 把Excel里的数据转换为DataTable，并返回DataTable
    /// </summary>
    /// <param name="excelPath">excel文件完整路径</param>
    public DataSet ExcelToDataTable(string excelPath)
    {
        string strCon = string.Empty;
        if (excelPath.ToLower().EndsWith("xls"))
            strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelPath + ";Extended Properties='Excel 8.0;IMEX=1'";
        else
            strCon = "Provider= Microsoft.Ace.OleDB.12.0;Data Source=" + excelPath + ";Extended Properties=Excel 12.0;";

        OleDbConnection oleDBConn = null;
        OleDbDataAdapter oleAdMaster = null;
        DataTable m_tableName = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            oleDBConn = new OleDbConnection(strCon);
            oleDBConn.Open();
            m_tableName = oleDBConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (m_tableName != null && m_tableName.Rows.Count > 0)
            {
                m_tableName.TableName = m_tableName.Rows[0]["TABLE_NAME"].ToString();
            }
            string sqlMaster;
            sqlMaster = " SELECT *  FROM [" + m_tableName.TableName + "]";
            oleAdMaster = new OleDbDataAdapter(sqlMaster, oleDBConn);
            oleAdMaster.Fill(ds, "m_tableName");
            return ds;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (oleAdMaster != null)
                oleAdMaster.Dispose();
            if (oleDBConn != null)
            {
                oleDBConn.Close();
                oleDBConn.Dispose();
            }
        }
    }

    /// <summary>
    /// 查看数据库里是否有此表名
    /// </summary>
    /// <param name="tableName"></param>
    public bool TableExist(string tableName)
    {
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string table = tableName.Substring(0, tableName.LastIndexOf('.') + 1);
            tableName = tableName.Substring(tableName.LastIndexOf('.') + 1).ToUpper();
            string sql = "select name from " + table + "sysobjects where type='u'";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(con, CommandType.Text, sql))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0).ToUpper() == tableName)
                            return true;
                    }
                }
            }
            return false;

    }

    public ArrayList GetTableField(string tableName)  //得到数据库某一个表中的所有字段  
    {
        ArrayList al = new ArrayList();
        string con = ConfigContext.GetInstance().DataBaseSettingProvider.SimpleManagerConnstring;
            string table = tableName.Substring(0, tableName.LastIndexOf('.') + 1);
            tableName = tableName.Substring(tableName.LastIndexOf('.') + 1);
            string sql = "SELECT b.name FROM " + table + "sysobjects a INNER JOIN " + table + "syscolumns b ON a.id = b.id WHERE (a.name = '" + tableName + "')";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(con, CommandType.Text, sql))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        al.Add(reader.GetString(0));
                    }
                }
            }

        return al; //返回的是表中的字段  
    }

    public enum ImportState
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 表名不存在
        /// </summary>
        TableNameError = 2,
        /// <summary>
        /// Excel里的字段和数据库表里的字段不匹配
        /// </summary>
        FieldMatchError = 3,
        /// <summary>
        /// 转换数据类型时发生错误
        /// </summary>
        DataTypeError = 4,
        /// <summary>
        /// Excel格式不能读取
        /// </summary>
        ExcelFormatError = 5
    }

}
