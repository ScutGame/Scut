using System;

namespace ZyGames.MSMQ.Model
{
    /// <summary>
    /// 消息基础类
    /// <remarks>CIT所有消息都继承此类</remarks>
    /// </summary>
    public abstract class BaseMSMQ
    {
        /// <summary>
        /// 消息发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }

    /// <summary>
    /// SQL执行参数信息体
    /// </summary>
    public class pramsBody
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string paramName;
        /// <summary>
        /// 数据字段类型
        /// </summary>
        public int iDBTypeValue;
        /// <summary>
        /// 数据字段大小
        /// </summary>
        public int isize;
        /// <summary>
        /// 数据值
        /// </summary>
        public object ivalue;
    }
}
