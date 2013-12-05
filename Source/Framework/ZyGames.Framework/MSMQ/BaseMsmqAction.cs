/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using MySql.Data.MySqlClient;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.SyncThreading;

namespace ZyGames.Framework.MSMQ
{

    /// <summary>
    /// 
    /// </summary>
    public enum MSMQCmd
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        SendSQLCmd
    }

    /// <summary>
    /// 队列的基类
    /// </summary>
    public abstract class BaseMSMQ
    {
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime sendTime;
    }

    /// <summary>
    /// Sql执行语句的传入参数类
    /// </summary>
    public class pramsBody
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string paramName;
        /// <summary>
        /// 参数类型
        /// </summary>
        public int iDBTypeValue;
        /// <summary>
        /// 长度大小限制
        /// </summary>
        public int isize;
        /// <summary>
        /// 参数值
        /// </summary>
        public object ivalue;
    }
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        public CommandType commandType;
        /// <summary>
        /// 
        /// </summary>
        public String commandText;
        /// <summary>
        /// 数据驱动连接提供者类型
        /// </summary>
        public string ProviderType;
        /// <summary>
        /// 
        /// </summary>
        public pramsBody[] paramters;
        /// <summary>
        /// 
        /// </summary>
        public SqlMessageQueue()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityID">标识ID</param>
        /// <param name="providerType">数据驱动连接提供者类型</param>
        /// <param name="aConnectionStr"></param>
        /// <param name="aCommandType"></param>
        /// <param name="aCommandText"></param>
        /// <param name="aParamters"></param>
        public void CreateBody(int identityID, string providerType, string aConnectionStr, CommandType aCommandType, String aCommandText, DbParameter[] aParamters)
        {
            IdentityID = identityID;
            this.sendTime = DateTime.Now;
            this.ProviderType = providerType;
            this.commandType = aCommandType;
            this.commandText = aCommandText;
            this.ConnectionString = aConnectionStr;

            if (aParamters != null)
            {
                List<pramsBody> paramsCmd = new List<pramsBody>();
                foreach (var param in aParamters)
                {
                    if (param == null)
                    {
                        continue;
                    }
                    pramsBody otmp = new pramsBody();
                    if (param is SqlParameter)
                    {
                        otmp.iDBTypeValue = Convert.ToInt32(((SqlParameter)param).SqlDbType);
                    }
                    else if(param is MySqlParameter)
                    {
                        otmp.iDBTypeValue = Convert.ToInt32(((MySqlParameter)param).MySqlDbType);
                    }
                    else
                    {
                        otmp.iDBTypeValue = Convert.ToInt32(param.DbType);
                    }
                    otmp.isize = param.Size;
                    otmp.paramName = param.ParameterName;
                    otmp.ivalue = param.Value;
                    paramsCmd.Add(otmp);
                }

                paramters = paramsCmd.ToArray();
            }

        }

    }
    /// <summary>
    ///消息队列添加类
    /// </summary>
    public abstract class BaseMsmqAction
    {

        private static MessageQueueTransactionType trantype = MessageQueueTransactionType.Automatic;
        /// <summary>
        /// 
        /// </summary>
        protected string sMessagePath = "";
        /// <summary>
        /// 
        /// </summary>
        protected List<MessageQueue> mqList;
        /// <summary>
        /// 
        /// </summary>
        protected SqlMessageQueue oSqlMessageQueue;
        /// <summary>
        /// 
        /// </summary>
        protected MSMQCmd msmqCmd;
        /// <summary>
        /// 
        /// </summary>
        protected int _messageQueueNum;
        /// <summary>
        /// 创建队列的构造函数，传入队列路径名称
        /// </summary>
        /// <param name="messagePath"></param>
        /// <param name="messageQueueNum">开启队列数</param>
        public BaseMsmqAction(string messagePath, int messageQueueNum)
        {
            sMessagePath = messagePath;
            _messageQueueNum = messageQueueNum;
            mqList = new List<MessageQueue>(messageQueueNum);
            for (int i = 0; i < messageQueueNum; i++)
            {
                string msmqPath = messageQueueNum > 1
                    ? string.Format("{0}_{1}", sMessagePath, i)
                    : sMessagePath;
                if (IsCreateQueue(msmqPath))
                {
#if DEBUG
                    Trace.WriteLine(string.Format("创建队列{0}/{1},path:{2}", i, messageQueueNum, msmqPath));
#endif
                    mqList.Add(CreateMessageQueue(msmqPath));
                }
            }
        }


        private static MessageQueue CreateMessageQueue(string msmqPath)
        {
            var mq = new MessageQueue(msmqPath, QueueAccessMode.Send);
            mq.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
            return mq;
        }

        /// <summary>
        /// 检查队列是否存在，若不存在，则创建队列目录
        /// </summary>
        private static bool IsCreateQueue(string msmqPath)
        {
            try
            {
                if (!MessageQueue.Exists(msmqPath))
                {
                    MessageQueue.Create(msmqPath, false);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryMQ(SqlMessageQueue sqlmq, out MessageQueue mq)
        {
            mq = null;
            if (sqlmq != null && _messageQueueNum > 0)
            {
                int index = 0;
                if (sqlmq.IdentityID > 0)
                {
                    index = sqlmq.IdentityID % _messageQueueNum;
                }
                if (mqList.Count > 0 && index < mqList.Count)
                {
                    mq = mqList[index];
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SendMessage()
        {
            SendMessage(msmqCmd, oSqlMessageQueue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aCmd"></param>
        /// <param name="aObjectBody"></param>
        public void SendMessage(MSMQCmd aCmd, object aObjectBody)
        {
            MessageQueue mq = null;
            Message message = new Message();
            message.Label = Convert.ToString(aCmd);
            switch (aCmd)
            {
                case MSMQCmd.SendSQLCmd:
                    SqlMessageQueue sqlmq = (SqlMessageQueue)aObjectBody;
                    TryMQ(sqlmq, out mq);
                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(SqlMessageQueue) });
                    message.Body = sqlmq;
                    break;
            }
            if (mq != null)
            {
                mq.Send(message, trantype);
            }
            else
            {
                throw new Exception("MessageQueue is null.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityID">标识ID</param>
        /// <param name="providerType">数据驱动连接提供者类型</param>
        /// <param name="connectingStr"></param>
        /// <param name="aCommandType"></param>
        /// <param name="aCommandText"></param>
        /// <param name="aParamters"></param>
        public void SendSqlCmd(int identityID, string providerType, string connectingStr, CommandType aCommandType, String aCommandText, DbParameter[] aParamters)
        {
            try
            {
                if (System.Threading.Monitor.TryEnter(SourceText.StLockMsmq, 3000))
                {

                    try
                    {
                        this.msmqCmd = MSMQCmd.SendSQLCmd;
                        this.oSqlMessageQueue = new SqlMessageQueue();
                        this.oSqlMessageQueue.CreateBody(identityID, providerType, connectingStr, aCommandType, aCommandText, aParamters);
                        this.SendMessage(MSMQCmd.SendSQLCmd, this.oSqlMessageQueue);
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(SourceText.StLockMsmq);
                    }
                }
                else
                {
                    throw new Exception("SendTimeOut");
                }
            }
            catch (Exception ex)
            {
                BaseLog baselog = new BaseLog("ActionMsmq");
                baselog.SaveLog(aCommandText, ex);
            }
        }
    }
}