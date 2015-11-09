using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZyGames.Framework.Profile
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfileSummary
    {
        /// <summary>
        /// 
        /// </summary>
        public ProfileSummary()
        {
            ChangeAutoMQ = new Dictionary<string, EntitySummary>();
            PostMQ = new Dictionary<string, EntitySummary>();
            ProcessMQ = new Dictionary<string, EntitySummary>();

        }
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 间隔采集时间
        /// </summary>
        public double IntervalSecond { get; set; }
        /// <summary>
        /// 修改属性由事件自动触发的计数
        /// </summary>
        public Dictionary<string, EntitySummary> ChangeAutoMQ { get; set; }
        /// <summary>
        /// Post的计数
        /// </summary>
        public Dictionary<string,EntitySummary> PostMQ { get; set; }
        /// <summary>
        /// 总计数可能与Post的总计数不相等，原因Process与Post是不同的线程处理，会出现Post 4次Process分两批处理
        /// </summary>
        public Dictionary<string, EntitySummary> ProcessMQ { get; set; }
        /// <summary>
        /// 未被处理的计数
        /// </summary>
        public long TotalNoProcess { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EntitySummary
    {
        /// <summary>
        /// 总计数
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// 总对象计数
        /// </summary>
        public long TotalObjectCount { get; set; }

        /// <summary>
        /// 单个采集点计数
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// 单个采集点对数计数
        /// </summary>
        public long ObjectCount { get; set; }
    }
}
