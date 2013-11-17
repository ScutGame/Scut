using System;
using System.Data;
using System.Data.SqlClient;

namespace ZyGames.MSMQ.Model
{
    /// <summary>
    /// 数据库执行语句消息队列对象
    /// </summary>
    public class SqlMessageQueue : BaseMSMQ
    {
        /// <summary>
        /// 标识ID（UserID）将队列水平划分
        /// </summary>
        public int IdentityID { get; set; }
        /// <summary>
        /// 数据库连接串设置
        /// </summary>
        public string ConnectionString = string.Empty;
        /// <summary>
        /// SQL执行类型
        /// </summary>
        public CommandType commandType;
        /// <summary>
        /// SQL执行命令
        /// </summary>
        public String commandText = string.Empty;
        /// <summary>
        /// SQL执行参数列表
        /// </summary>
        public pramsBody[] paramters = new pramsBody[0];
        /// <summary>
        /// 构造执行SQL的参数信息
        /// </summary>
        public SqlParameter[] paramsAction = new SqlParameter[0];

        private string _dataBase;
        private string _commandTable;

        /// <summary>
        /// 缺省构造器
        /// </summary>
        public SqlMessageQueue()
            : base()
        {

        }

        public string DataBase
        {
            get
            {
                if (string.IsNullOrEmpty(_dataBase))
                {
                    try
                    {
                        string[] tempList = ConnectionString.Split(';');
                        foreach (var temp in tempList)
                        {
                            if (temp.TrimEnd().Length == 0) continue;
                            if (temp.ToLower().StartsWith("database"))
                            {
                                _dataBase = temp.ToLower().Replace("database", "").Replace("=", "");
                            }
                        }
                        if (string.IsNullOrEmpty(_dataBase))
                        {
                            _dataBase = "None";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return _dataBase;
            }
        }

        public string CommandTable
        {
            get
            {
                if (string.IsNullOrEmpty(_commandTable))
                {
                    try
                    {
                        //INSERT INTO UserSportsCombat(
                        var tempList = commandText.Split(' ');
                        if (tempList.Length >= 3)
                        {
                            var list = tempList[2].Split('(');
                            string tempStr = list.Length > 0 ? list[0] : "";
                            _commandTable = string.Join("_", tempList, 0, 2) + "_" + tempStr;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (string.IsNullOrEmpty(_commandTable))
                    {
                        _commandTable = "None";
                    }
                }
                return _commandTable;
            }
        }
    }
}
