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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9105_加入黑名单列表接口
    /// </summary>
    public class Action9105 : BaseAction
    {
        private string _mailId = string.Empty;
        private string _friendId = string.Empty;
        private int _ops;

        public Action9105(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9105, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("MailId", ref _mailId)
                 && httpGet.GetString("FriendId", ref _friendId)

                 && httpGet.GetInt("Ops", ref _ops)
                 )
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cacheSet = new ShareCacheStruct<UserFriends>();
            if (_friendId != "" && _friendId != ContextUser.UserID)
            {
                //找到本玩家的数据
                List<UserFriends> friendArray = cacheSet.FindAll(m => m.UserID == ContextUser.UserID);
                int friendNum = ConfigEnvSet.GetInt("UserFriends.MaxFriendNum");
                //添加的好友上限
                if (friendArray.Count >= friendNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St9103_TheMaximumReachedAFriend;
                    return false;
                }

                //查看是否在user库中有该玩家
                GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(_friendId);
                if (userInfo == null)
                {
                    UserCacheGlobal.LoadOffline(_friendId);
                    userInfo = new GameDataCacheSet<GameUser>().FindKey(_friendId);
                }
                if (userInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St9103_NotFriendsUserID;
                    return false;
                }

                //在好友表中查找本玩家 和添加好友的关系
                var userFriend = cacheSet.FindKey(ContextUser.UserID, _friendId);
                var userFriend1 = cacheSet.FindKey(_friendId, ContextUser.UserID);
                //同意
                if (_ops == 1)
                {
                   //有信息表 没信息表
                   if (userFriend == null)
                   {
                       //创建新的数据 并且添加成关注类型
                       var friends = new UserFriends
                       {
                           UserID = ContextUser.UserID,
                           FriendID = _friendId,
                           FriendType = FriendType.Friend
                       };
                       cacheSet.Add(friends);
                       //todo test
                       friends.ChatTime = DateTime.Now;

                   }else
                   {
                       //玩家原来就有数据
                       //判断两个玩家的关系
                       if (userFriend.FriendType == FriendType.Friend)
                       {
                           ErrorCode = LanguageManager.GetLang().ErrorCode;
                           ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasAFriendIn;
                           return false;
                       }
                       userFriend.FriendType = FriendType.Friend;
                   }

                   //判断对方是否有和本玩家的数据 如果没有创建 有改状态
                   if (userFriend1 == null)
                   {
                       var friends2 = new UserFriends
                       {
                           UserID = _friendId,
                           FriendID = ContextUser.UserID,
                           FriendType = FriendType.Friend,
                       };
                       cacheSet.Add(friends2);
                       //todo test
                       friends2.ChatTime = DateTime.Now;
                   }
                   else
                   {
                         userFriend1.FriendType = FriendType.Friend;
                    }

                    //加为好友成功后发送一条邮件   
                   try
                   {
                       Guid newGuid = Guid.NewGuid();
                       UserMail userMail = new UserMail(newGuid);
                       userMail.UserId = Int32.Parse(_friendId);
                       userMail.MailType = (int)MailType.Friends;
                       userMail.Title = LanguageManager.GetLang().St_AskFirendMailTitle;
                       userMail.Content = string.Format(LanguageManager.GetLang().St_FirendNotice, ContextUser.NickName);
                       userMail.SendDate = DateTime.Now;
                       userMail.FromUserId = Int32.Parse(ContextUser.UserID);
                       userMail.FromUserName = ContextUser.NickName;
                       TjxMailService mailService = new TjxMailService(ContextUser);
                       mailService.Send(userMail);

                       var noticeMail = mailService.ReadMail(ContextUser.UserID, _mailId);
                       noticeMail.ReplyStatus = 1;
     

                   }
                   catch (Exception)
                   {

                   }

                }else
                {

                    //不同意
                    TjxMailService mailService = new TjxMailService(ContextUser);
                    var noticeMail = mailService.ReadMail(ContextUser.UserID, _mailId);
                    noticeMail.ReplyStatus = 1;

                }
            }
            return true;
        }
    }
}