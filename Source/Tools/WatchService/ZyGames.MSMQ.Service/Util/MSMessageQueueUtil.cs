using System;
using System.Messaging;

namespace ZyGames.MSMQ.Service.Util
{
    /// <summary>
    /// 消息队列通用类
    /// </summary>
    public class MSMessageQueueUtil : IDisposable
    {
        /// <summary>
        /// 消息机制类型
        /// </summary>
        protected MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;
        /// <summary>
        /// 消息队列实体
        /// </summary>
        protected MessageQueue queue;
        /// <summary>
        /// 消息超时时间
        /// </summary>
        protected TimeSpan timeout;

        /// <summary>
        /// 消息描述信息
        /// </summary>
        public string MessageLabel = string.Empty;

        /// <summary>
        /// 消息编号
        /// </summary>
        public string MessageID = string.Empty;

        /// <summary>
        /// 接收必要参数的构造器
        /// </summary>
        /// <param name="queuePath">消息队列存放路径（建议服务器上统一修改消息队列存储磁盘）</param>
        /// <param name="timeoutSeconds">消息队列超时时间（以秒为单位）</param>
        public MSMessageQueueUtil(string queuePath, int timeoutSeconds)
        {
            queue = new MessageQueue(queuePath);
            timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeoutSeconds));
            queue.DefaultPropertiesToSend.AttachSenderId = false;
            queue.DefaultPropertiesToSend.UseAuthentication = false;
            queue.DefaultPropertiesToSend.UseEncryption = false;
            queue.DefaultPropertiesToSend.AcknowledgeType = AcknowledgeTypes.None;
            queue.DefaultPropertiesToSend.UseJournalQueue = false;
        }

        /// <summary>
        /// 消息队列内容接收，并返回消息实体
        /// </summary>
        public virtual object Receive()
        {
            try
            {
                using (Message message = queue.Receive(timeout, transactionType))
                {
                    MessageLabel = message.Label;
                    MessageID = message.Id;
                    return message;
                }
            }
            catch (MessageQueueException mqex)
            {
                if (mqex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                    throw new TimeoutException();

                throw;
            }
        }

        /// <summary>
        /// 消息队列内容写入，写入对象由外部定义
        /// 建议调用者要进行异常保护，消息写入异常要有异常修复机制
        /// </summary>
        public virtual void Send(object msg)
        {
            queue.Send(msg, transactionType);
        }

        /// <summary>
        /// 创建消息队列
        /// </summary>
        /// <param name="path">消息存放路径（可以是专用队列，可以是公共队列）</param>
        /// <returns></returns>
        public static MessageQueue Create(string path)
        {
            if (false == MessageQueue.Exists(path))
            {
                MessageQueue q = MessageQueue.Create(path);
                q.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
                return q;
            }
            else
            {
                return new MessageQueue(path);
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            queue.Dispose();
        }
        #endregion
    }
}
