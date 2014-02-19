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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 9103_添加好友接口
    /// </summary>
    public class Action9103 : BaseAction
    {
        private string _friendId = string.Empty;
        private string _friendName = string.Empty;
        private int _isRe;
        private int _isSuccess;

        public Action9103(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action9103, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_isSuccess);
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("FriendID", ref _friendId))
            {
                httpGet.GetString("FriendName", ref _friendName);;
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            var cacheSet = new ShareCacheStruct<UserFriends>();
            _isSuccess = 1;
            if (_friendId != "")
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

                if (userFriend == null)
                {
                    //创建新的数据 并且添加成关注类型
                    var friends = new UserFriends
                                      {
                                          UserID = ContextUser.UserID,
                                          FriendID = _friendId,
                                          FriendType = FriendType.Attention
                                      };
                    cacheSet.Add(friends);
                    //todo test
                    friends.ChatTime = DateTime.Now;

                }
                //如果玩家数据不为空
                else
                {
                    //判断两个玩家的关系
                    if (userFriend.FriendType == FriendType.Friend)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasAFriendIn;
                        return false;
                    }

                    ////如果已经发送请求就不在继续发
                    //if (userFriend.FriendType == FriendType.Attention)
                    //{
                    //    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    //    ErrorInfo = LanguageManager.GetLang().St_FirendNoticeTip;
                    //    return false;
                    //}

                    //加好友都是变为关注 从仇敌那里也能转换为关注
                    if (userFriend.FriendType != FriendType.Friend)
                    {
                        userFriend.FriendType = FriendType.Attention;
                    }

                }

                //判断对方是否有和本玩家的数据 如果没有创建 有改状态
                if (userFriend1 == null)
                {
                    var friends2 = new UserFriends
                    {
                        UserID = _friendId,
                        FriendID = ContextUser.UserID,
                        FriendType = FriendType.Fans,
                    };
                    cacheSet.Add(friends2);
                    //todo test
                    friends2.ChatTime = DateTime.Now;
                }

                // 发送系统信件       
                try
                {
                    Guid newGuid = Guid.NewGuid();
                    UserMail userMail = new UserMail(newGuid);
                    userMail.UserId = Int32.Parse(_friendId);
                    userMail.MailType = (int)MailType.Friends;
                    userMail.Title = LanguageManager.GetLang().St_AskFirendMailTitle;
                    userMail.Content = string.Format(LanguageManager.GetLang().St_AskFirendTip, ContextUser.NickName);
                    userMail.SendDate = DateTime.Now;
                    userMail.IsReply = true;
                    userMail.ReplyStatus = 0;
                    userMail.FromUserId = Int32.Parse(ContextUser.UserID);
                    userMail.FromUserName = ContextUser.NickName;
                    TjxMailService mailService = new TjxMailService(ContextUser);
                    mailService.Send(userMail);
                }
                catch (Exception)
                {

                }
            }
            //上传的好友名字不为空
            else if (_friendName != null)
            {
                List<UserFriends> friendArray = cacheSet.FindAll(m => m.UserID == ContextUser.UserID);
                int friendNum = ConfigEnvSet.GetInt("UserFriends.MaxFriendNum");
                if (friendArray.Count >= friendNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St9103_TheMaximumReachedAFriend;
                    return false;
                }
                GameUser friend = null;
                new GameDataCacheSet<GameUser>().Foreach((personalId, key, user) =>
                {
                    if (user.NickName == _friendName)
                    {
                        friend = user;
                        return false;
                    }
                    return true;
                });
                if (friend != null)
                {
                    GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(friend.UserID);
                    UserFriends userFriend = cacheSet.FindKey(ContextUser.UserID, _friendId);
                    if (userFriend != null)
                    {
                        if (userFriend.FriendType == FriendType.Fans)
                        {
                            this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                            this.ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasTheFansIn;
                            return false;
                        }
                        else if (userFriend.FriendType == FriendType.Blacklist)
                        {
                            this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                            this.ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasTheBlacklist;
                            return false;
                        }
                        else
                        {
                            this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                            this.ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasAFriendIn;
                            return false;
                        }
                    }
                    UserFriends friends = new UserFriends()
                    {
                        UserID = ContextUser.UserID,
                        FriendID = gameUser.UserID,
                        FriendType = FriendType.Attention
                    };
                    cacheSet.Add(friends);

                    UserFriends friends2 = new UserFriends()
                    {
                        UserID = gameUser.UserID,
                        FriendID = ContextUser.UserID,
                        FriendType = FriendType.Fans,
                    };
                    cacheSet.Add(friends2);
                }
                else
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St9103_DoesNotExistTheUser;
                    return false;
                }
            }
            return true;
        }
    }
}