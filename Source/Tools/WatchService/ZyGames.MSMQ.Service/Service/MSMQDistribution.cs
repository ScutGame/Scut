using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;
using ZyGames.Core.Data;
using ZyGames.Core.Util;
using ZyGames.GameService.BaseService.LogService;
using ZyGames.MSMQ.Model;
using ZyGames.MSMQ.Service.Common;
using ZyGames.MSMQ.Service.Util;

namespace ZyGames.MSMQ.Service.Service
{
    /// <summary>
    /// 上下文对象
    /// </summary>
    public class MSMQContext
    {
        public MSMQContext(MessageConfig msgConfig, int index)
        {
            Index = index;
            MsgConfig = msgConfig;
        }
        public int Index { get; set; }
        public MessageConfig MsgConfig { get; set; }
    }
    /// <summary>
    /// 消息队列分发器
    /// </summary>
    public class MSMQDistribution
    {
        private const int SleepSeconds = 3;
        private static int BufferWait;
        private static int BufferMaxCount;
        private static MessageQueue _errorMQ;
        private static readonly object syncRoot = new object();
        private Dictionary<int, SafedQueue<SqlMessageQueue>> _distribeCollection;

        static MSMQDistribution()
        {
            string temp = ConfigHelper.GetSetting("messageDistribeBufferCount", "1000");
            BufferMaxCount = int.Parse(temp);
            string waitTemp = ConfigHelper.GetSetting("messageDistribeBufferWait", "100");
            BufferWait = int.Parse(waitTemp);
        }

        public void Receive(object item)
        {
            MessageConfig msgConfig = item as MessageConfig;
            if (msgConfig == null) return;
            try
            {
                InitQueue(msgConfig);



                while (true)
                {
                    SqlMessageQueue sqlMsgQueue = null;
                    try
                    {
                        var sqlMessageQueue = new MSMessageQueueExtension<SqlMessageQueue>(msgConfig.ManagerMessagePath, msgConfig.ManagerThreadTimeOut);
                        sqlMsgQueue = sqlMessageQueue.Receive();
                        PutQueue(sqlMsgQueue);
                    }
                    catch (TimeoutException outEx)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(SleepSeconds));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteException(string.Format("Receive messages error: content: {0}; parameters：{1}",
                            (sqlMsgQueue != null ? sqlMsgQueue.commandText : string.Empty),
                            GetMessagQueueParamterString(sqlMsgQueue)), ex);
                        Thread.Sleep(TimeSpan.FromSeconds(SleepSeconds));
                    }
                }
            }
            catch (Exception error)
            {
                LogHelper.WriteException(string.Format("Queue {0} error:", msgConfig.ManagerMessagePath), error);
            }
        }

        private void TraceWriteLine(string msg)
        {
#if DEBUG
            new BaseLog().SaveDebuLog(msg);
            Console.WriteLine(msg);
            Thread.Sleep(500);
#else
            //new BaseLog().SaveDebuLog(msg);
#endif
        }

        private void StartWriteThread(object item)
        {
            MSMQContext context = item as MSMQContext;
            if (context == null) return;

            try
            {
                SafedQueue<SqlMessageQueue> msmqQueue = GetQueue(context.Index);
                while (true)
                {
                    try
                    {
                        while (msmqQueue.Count == 0)
                        {
                            Thread.Sleep(BufferWait);
                        }
                        List<SqlMessageQueue> smqBuffer = new List<SqlMessageQueue>(BufferMaxCount);
                        while (smqBuffer.Count < BufferMaxCount)
                        {
                            if (msmqQueue.Count == 0) break;
                            SqlMessageQueue sqlMsgQueue;
                            if (msmqQueue.TryDequeue(out sqlMsgQueue))
                            {
                                smqBuffer.Add(sqlMsgQueue);
                            }
                            else
                            {
                                Thread.Sleep(SleepSeconds);
                            }
                        }
                        Interlocked.Exchange(ref writeCount, writeCount - smqBuffer.Count);
                        TraceWriteLine(string.Format("Queue [{0} _ {1}] has buffered {2} / {3}, remaining: {4}", context.MsgConfig.ManagerMessagePath, context.Index, smqBuffer.Count, BufferMaxCount, writeCount));

                        foreach (SqlMessageQueue itemQueue in smqBuffer)
                        {
                            if (itemQueue == null) continue;
                            int runTimes = 0;
                            if (!DoSqlExecute(itemQueue, runTimes))
                            {
                                //失败重执行次数
                                ReDoSqlExecute(context, itemQueue, runTimes);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteException(string.Format("Queue write {0} error:", context.MsgConfig.ManagerMessagePath), e);
                    }
                }
            }
            catch (Exception error)
            {
                LogHelper.WriteException(string.Format("Queue write {0} error:", context.MsgConfig.ManagerMessagePath), error);
            }
        }

        private void ReDoSqlExecute(MSMQContext context, SqlMessageQueue messageQueue, int runTimes)
        {
            try
            {
                for (int i = 0; i < ConfigContext.GetInstance().ReDoExecuteCount; i++)
                {
                    runTimes++;
                    if (DoSqlExecute(messageQueue, runTimes))
                    {
                        LogHelper.WriteDebug(string.Format("Message queue [{0} times] re-execute Sql: {1} successfully!", runTimes, messageQueue.commandText));
                        return;
                    }
                }

                WriteErrorMSMQ(context.MsgConfig, messageQueue);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(string.Format("Queue reexecute {0} error:", context.MsgConfig.ManagerMessagePath), ex);
            }
        }

        private void InitQueue(MessageConfig msgConfig)
        {
            if (msgConfig == null) return;

            if (msgConfig.ManagerThreadNumber == 0)
            {
                throw new Exception("ManagerThreadNumber is error.");
            }
            _distribeCollection = new Dictionary<int, SafedQueue<SqlMessageQueue>>(msgConfig.ManagerThreadNumber);
            for (int i = 0; i < msgConfig.ManagerThreadNumber; i++)
            {
                if (!_distribeCollection.ContainsKey(i))
                {
                    _distribeCollection.Add(i, new SafedQueue<SqlMessageQueue>());
                }
                else
                {
                    _distribeCollection[i] = new SafedQueue<SqlMessageQueue>();
                }
                new Thread(StartWriteThread).Start(new MSMQContext(msgConfig, i));
            }
        }

        private SafedQueue<SqlMessageQueue> GetQueue(int index)
        {
            if (_distribeCollection.ContainsKey(index))
            {
                return _distribeCollection[index];
            }
            return null;
        }

        private void SetQueue(int index, SafedQueue<SqlMessageQueue> msQueue)
        {
            if (msQueue != null && _distribeCollection.ContainsKey(index))
            {
                _distribeCollection[index] = msQueue;
            }

        }

        private int writeCount = 0;

        private void PutQueue(SqlMessageQueue sqlmq)
        {
            if (sqlmq == null) return;
            if (_distribeCollection.Count > 0)
            {
                int index = sqlmq.IdentityID % _distribeCollection.Count;
                SafedQueue<SqlMessageQueue> msQueue = GetQueue(index);

                while (msQueue != null && !msQueue.TryEnqueue(sqlmq))
                {
                    Thread.Sleep(SleepSeconds);
                }
                SetQueue(index, msQueue);
            }
            Interlocked.Exchange(ref writeCount, writeCount + 1);
            //TraceWriteLine(string.Format("写入ID:{0}队列数{1}", sqlmq.IdentityID, writeCount));
        }

        /// <summary>
        /// 执行消息队列SQL语句
        /// </summary>
        /// <param name="messageQueue">消息队列对象</param>
        public bool DoSqlExecute(SqlMessageQueue messageQueue, int reDoingCount)
        {
            if (messageQueue == null || messageQueue.paramters == null) return false;
            try
            {
                SqlParameter[] paramsAction = new SqlParameter[messageQueue.paramters.Length];
                for (int i = 0; i < messageQueue.paramters.Length; i++)
                {
                    if (messageQueue.paramters[i] == null)
                    {
                        continue;
                    }
                    SqlDbType dbtype = (SqlDbType)Enum.Parse(typeof(SqlDbType), messageQueue.paramters[i].iDBTypeValue.ToString(), true);
                    paramsAction[i] = SqlParamHelper.MakeInParam(messageQueue.paramters[i].paramName, dbtype, messageQueue.paramters[i].isize, messageQueue.paramters[i].ivalue);
                }
                int result = 0;
                if (string.IsNullOrEmpty(messageQueue.ConnectionString))
                {
                    result = SqlHelper.ExecuteNonQuery(ConfigContext.GetInstance().ConnctionString, messageQueue.commandType, messageQueue.commandText, paramsAction);
                }
                else
                {
                    result = SqlHelper.ExecuteNonQuery(messageQueue.ConnectionString, messageQueue.commandType, messageQueue.commandText, paramsAction);
                }
                if (result > 1)
                {
                    LogHelper.WriteDebug(string.Format("Message queue [{0} times] perform impact {1} line Sql：{2}", reDoingCount, result, messageQueue.commandText + "：parameter：" + messageQueue.commandType + GetMessagQueueParamterString(messageQueue)));
                }
                return true;
            }
            catch (SqlException sqlex)
            {
                LogHelper.WriteException(string.Format("Message queue [{0} times] to execute SQL statements SQLEX：{1};{2}", reDoingCount, sqlex.Message, messageQueue.commandText + "：parameter：" + messageQueue.commandType + GetMessagQueueParamterString(messageQueue)), sqlex);
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(string.Format("Message queue [{0} times] SQL statement error：{1};{2}", reDoingCount, ex.Message, messageQueue.commandText + "：parameter：" + messageQueue.commandType + GetMessagQueueParamterString(messageQueue)), ex);
            }
            return false;
        }

        /// <summary>
        /// 写入异常处理队列
        /// </summary>
        private void WriteErrorMSMQ(MessageConfig msgConfig, SqlMessageQueue messageQueue)
        {
            try
            {
                if (msgConfig.IsErrorQueue)
                {
                    if (messageQueue != null)
                    {
                        SaveToLog(messageQueue);
                    }
                    return;
                }
                MessageQueue mq = CreateErrorMQ(msgConfig);

                Message message = new Message();
                message.Label = Convert.ToString(MSMQCmd.SendSQLCmd);
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(SqlMessageQueue) });
                message.Body = messageQueue;
                mq.Send(message, MessageQueueTransactionType.Automatic);

            }
            catch (Exception ex)
            {
                LogHelper.WriteException(string.Format("Write error exception handling queue：{0}", ex.Message), ex);
            }
        }

        private void SaveToLog(SqlMessageQueue messageQueue)
        {
            string dirPath = messageQueue.DataBase;
            string jsonStr = ZyGames.Base.Common.JsonSerializer.Serialize(messageQueue);
            string root = @"D:\NLog\ErrorMSMQLog";
            try
            {
                root = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (Exception ex)
            {
            }
            string path = Path.Combine(root, "MSMQLog");
            if (!string.IsNullOrEmpty(dirPath)) path = Path.Combine(path, dirPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, string.Format("{0}_{1}.txt", messageQueue.CommandTable, Guid.NewGuid().ToString()));
            using (StreamWriter stream = File.CreateText(path))
            {
                stream.Write(jsonStr);
                stream.Flush();
            }
        }

        private MessageQueue CreateErrorMQ(MessageConfig msgConfig)
        {
            if (_errorMQ == null)
            {
                lock (syncRoot)
                {
                    if (_errorMQ == null)
                    {
                        if (!MessageQueue.Exists(msgConfig.ManagerErrorPath))
                        {
                            MessageQueue.Create(msgConfig.ManagerErrorPath, false);
                        }
                        _errorMQ = new MessageQueue(msgConfig.ManagerErrorPath, QueueAccessMode.Send);
                        _errorMQ.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
                    }

                }
            }

            return _errorMQ;
        }

        private static string GetMessagQueueParamterString(SqlMessageQueue messageQueue)
        {
            StringBuilder sb = new StringBuilder();
            if (messageQueue == null) return sb.ToString();
            if (messageQueue.paramters == null) return sb.ToString();
            for (int i = 0; i < messageQueue.paramters.Length; i++)
            {
                if (messageQueue.paramters[i] == null)
                {
                    continue;
                }
                if (sb.Length > 0) sb.Append(" and ");
                sb.AppendFormat("{0}={1}", messageQueue.paramters[i].paramName, messageQueue.paramters[i].ivalue);
            }
            return sb.ToString();
        }

    }
}
