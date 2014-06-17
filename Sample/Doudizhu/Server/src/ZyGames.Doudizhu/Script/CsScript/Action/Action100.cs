using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Contract;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{
    /// <summary>
    /// 客户端注册Socket接口
    /// </summary>
    public class Action100 : BaseAction
    {
        private MemoryCacheStruct<UserConnection> _roomStruct;
        private int ops;

        public Action100(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action100, httpGet)
        {
            _roomStruct = new MemoryCacheStruct<UserConnection>();
        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            httpGet.GetInt("ops", ref ops);
            return true;

        }

        public override bool TakeAction()
        {
            //string host = httpGet.RemoteAddress ?? "";
            //string bindHost = ContextUser != null ? ContextUser.RemoteAddress : "";
            //if (!string.IsNullOrEmpty(host) && !host.Equals(bindHost))
            //{
            //    UserConnection con;
            //    if (_roomStruct.TryGet(host, out con))
            //    {
            //        var tempUser = new GameDataCacheSet<GameUser>().FindKey(Uid);
            //        if (tempUser != null)
            //        {
            //            Console.WriteLine("玩家:{0}-{1} Socket通道被解绑", tempUser.UserId, tempUser.NickName);
            //        }
            //        con.UserId = UserId;
            //    }
            //    else
            //    {
            //        _roomStruct.TryAdd(host, new UserConnection() { UserId = UserId, LocalHost = host });
            //    }
            //    if (ContextUser != null)
            //    {
            //        ContextUser.RemoteAddress = host;
            //    }
            //}
            //Console.WriteLine(httpGet.RemoteAddress);
            return true;
        }

    }
}
