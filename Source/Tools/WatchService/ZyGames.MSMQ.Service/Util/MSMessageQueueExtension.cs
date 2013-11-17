using System;
using System.Messaging;
using ZyGames.MSMQ.Model;

namespace ZyGames.MSMQ.Service.Util
{
    /// <summary>
    /// 消息队列扩展对象
    /// <remarks>可以接收外部任意对象</remarks>
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    public class MSMessageQueueExtension<T> : MSMessageQueueUtil where T : new()
    {
        /// <summary>
        /// 接收必要参数的构造器
        /// </summary>
        /// <param name="path">消息队列存放路径（建议服务器上统一修改消息队列存储磁盘）</param>
        /// <param name="timeOut">消息队列超时时间（以秒为单位）</param>
        public MSMessageQueueExtension(string path, int timeOut)
            : base(path, timeOut)
        {
            //这里我目前不是很清楚你们的格式化规则，这里暂用这种格式化方式，具体格式化方式，可以由外部构造
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(SqlMessageQueue) });
        }

        /// <summary>
        /// 消息队列内容接收，并返回消息泛型实体
        /// </summary>
        /// <returns></returns>
        public new T Receive()
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            return (T)((Message)base.Receive()).Body;
        }

        /// <summary>
        /// 消息队列内容接收，并返回消息泛型实体,接收超时时间
        /// </summary>
        /// <param name="timeOut">消息队列超时时间（以秒为单位）</param>
        /// <returns></returns>
        public T Receive(int timeOut)
        {
            base.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeout));
            return Receive();
        }

        /// <summary>
        /// 消息队列内容写入，写入对象由外部定义
        /// 建议调用者要进行异常保护，消息写入异常要有异常修复机制
        /// </summary>
        /// <param name="message">消息泛型对象</param>
        public void Send(T message)
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            base.Send(message);
        }

    }
}
