using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Base.PayCenter.Cache;
using ZyGames.Base.PayCenter.NetDirServer;
using ZyGames.Framework.RPC.IO;

namespace BLL
{
    public class SocketAction : GameSocketClient
    {
        public MessageStructure result { get; set; }
        public MessageHead _head { get; set; }
        public override void Request(string action)
        {

        }
        public void DoSocket(string serverUrl, string requestParams )
        {
            DoRequest(serverUrl, requestParams);
        }
        protected override void SuccessCallback(MessageStructure writer, MessageHead head)
        {
            result = writer;
            _head = head;
        }
    }
}
