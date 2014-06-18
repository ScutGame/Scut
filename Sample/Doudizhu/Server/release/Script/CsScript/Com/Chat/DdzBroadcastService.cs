using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Game.Model;

namespace ZyGames.Doudizhu.Bll.Com.Chat
{
    /// <summary>
    /// 广播中间件
    /// </summary>
    public class DdzBroadcastService : BroadcastService
    {
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="content"></param>
        /// <param name="times">发送次数</param>
        /// <param name="secondInvertal">间隔发送</param>
        public static void SendTimer(string content, int times, int secondInvertal)
        {
            var broadcastService = new DdzBroadcastService(null);
            var msg = broadcastService.Create(NoticeType.System, content);
            string startTime = DateTime.Now.ToString("HH:mm:ss");
            string endTime = DateTime.Now.AddSeconds(times * secondInvertal).ToString("HH:mm:ss");
            broadcastService.SendTimer(msg, startTime, endTime, true, secondInvertal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public static void Send(string content)
        {
            var broadcastService = new DdzBroadcastService(null);
            var msg = broadcastService.Create(NoticeType.System, content);
            broadcastService.Send(msg);
        }

        private readonly GameUser _user;
        public const int MaxCount = 50;
        private SensitiveWordService _wordServer;

        /// <summary>
        /// 
        /// </summary>
        public DdzBroadcastService(GameUser user)
        {
            _user = user;
            _wordServer = new SensitiveWordService();
        }

        protected override void SetVersionId(int versionId)
        {
            if (_user != null)
            {
                _user.Property.BroadcastVesion = versionId;
            }
        }

        public bool HasMessage()
        {
            return HasMessage(_user.Property.BroadcastVesion);
        }

        public List<NoticeMessage> GetMessages()
        {
            return GetMessages(_user.Property.BroadcastVesion, MaxCount);
        }

        protected override string FilterMessage(string message)
        {
            return _wordServer.Filter(message);
        }
    }
}
