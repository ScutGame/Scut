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
using ZyGames.Framework.Common;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.Message;
using ZyGames.Framework.Model;
using ProtoBuf;
using System.Runtime.Serialization;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model.Enum;


namespace ZyGames.Tianjiexing.Model
{
    /// <summary>
    /// @periodTime:设置生存周期(秒)
    /// @personalName: 映射UserId对应的字段名,默认为"UserId"
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(DbConfig.Data, "UserMail", DbConfig.PeriodTime, DbConfig.PersonalName, Condition = "IsRemove=0")]
    public class UserMail : MailMessage
    {

        /// <summary>
        /// </summary>
        public UserMail()
        {
        }

        /// <summary>
        /// </summary>
        public UserMail(Guid MailID)
            : base(MailID)
        {
            this._MailID = MailID;
        }

        #region 自动生成属性


        private Boolean _IsGuide;

        /// <summary>
        /// </summary>        
        [ProtoMember(114)]
        [EntityField("IsGuide")]
        public Boolean IsGuide
        {
            get
            {
                return _IsGuide;
            }
            set
            {
                SetChange("IsGuide", value);
            }
        }

        private bool _isReply;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(115)]
        [EntityField("IsReply")]
        public bool IsReply
        {
            get
            {
                return _isReply;
            }
            set
            {
                SetChange("IsReply", value);
            }
        }
        private short _replyStatus;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(116)]
        [EntityField("ReplyStatus")]
        public short ReplyStatus
        {
            get
            {
                return _replyStatus;
            }
            set
            {
                SetChange("ReplyStatus", value);
            }
        }
        private int _counterattackUserID;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(117)]
        [EntityField("CounterattackUserID")]
        public int CounterattackUserID
        {
            get
            {
                return _counterattackUserID;
            }
            set
            {
                SetChange("CounterattackUserID", value);
            }
        }

        private string _combatProcess;

        /// <summary>
        /// 战斗过程
        /// </summary>        
        [ProtoMember(118)]
        [EntityField("CombatProcess")]
        public string CombatProcess
        {
            get
            {
                return _combatProcess;
            }
            set
            {
                SetChange("CombatProcess", value);
            }
        }

        private bool _isWin;

        /// <summary>
        /// 战斗结果：1、胜利 0、失败
        /// </summary>        
        [ProtoMember(119)]
        [EntityField("IsWin")]
        public bool IsWin
        {
            get
            {
                return _isWin;
            }
            set
            {
                SetChange("IsWin", value);
            }
        }

        private int _gameCoin;

        /// <summary>
        /// 竞技场金币
        /// </summary>
        [ProtoMember(120)]
        [EntityField("GameCoin")]
        public int GameCoin
        {
            get { return _gameCoin; }
            set
            {
                SetChange("GameCoin", value);
            }
        }

        
        private int _obtion;
        /// <summary>
        /// 竞技场获得声望
        /// </summary>
        [ProtoMember(121)]
        [EntityField("Obtion")]
        public int Obtion
        {
            get { return _obtion; }
            set
            {
                SetChange("Obtion", value);
            }
        }

        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "MailID": return MailID;
                    case "UserId": return UserId;
                    case "MailType": return MailType;
                    case "FromUserId": return FromUserId;
                    case "FromUserName": return FromUserName;
                    case "ToUserID": return ToUserID;
                    case "ToUserName": return ToUserName;
                    case "Title": return Title;
                    case "Content": return Content;
                    case "SendDate": return SendDate;
                    case "IsRead": return IsRead;
                    case "IsRemove": return IsRemove;
                    case "RemoveDate": return RemoveDate;
                    case "IsGuide": return IsGuide;
                    case "IsReply": return _isReply;
                    case "ReplyStatus": return _replyStatus;
                    case "CounterattackUserID": return _counterattackUserID;
                    case "CombatProcess": return _combatProcess;
                    case "IsWin": return _isWin;
                    case "GameCoin": return _gameCoin;
                    case "Obtion": return _obtion;
                    default: throw new ArgumentException(string.Format("UserMail index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "MailID":
                        _MailID = (Guid)value;

                        break;
                    case "UserId":
                        _UserId = value.ToInt();
                        break;
                    case "MailType":
                        _MailType = value.ToInt();
                        break;
                    case "FromUserId":
                        _FromUserId = value.ToInt();
                        break;
                    case "FromUserName":
                        _FromUserName = value.ToNotNullString();
                        break;
                    case "ToUserID":
                        _ToUserID = value.ToInt();
                        break;
                    case "ToUserName":
                        _ToUserName = value.ToNotNullString();
                        break;
                    case "Title":
                        _Title = value.ToNotNullString();
                        break;
                    case "Content":
                        _Content = value.ToNotNullString();
                        break;
                    case "SendDate":
                        _SendDate = value.ToDateTime();
                        break;
                    case "IsRead":
                        _IsRead = value.ToBool();
                        break;
                    case "IsRemove":
                        _IsRemove = value.ToBool();
                        break;
                    case "RemoveDate":
                        _RemoveDate = value.ToDateTime();
                        break;
                    case "IsGuide":
                        _IsGuide = value.ToBool();
                        break;
                    case "IsReply":
                        _isReply = value.ToBool();
                        break;
                    case "ReplyStatus":
                        _replyStatus = value.ToShort();
                        break;
                    case "CounterattackUserID":
                        _counterattackUserID = value.ToInt();
                        break;
                    case "CombatProcess":
                        _combatProcess = value.ToNotNullString();
                        break;
                    case "IsWin":
                        _isWin = value.ToBool();
                        break;
                    case "GameCoin":
                        _gameCoin = value.ToInt();
                        break;
                    case "Obtion":
                        _obtion = value.ToInt();
                        break;
                    default: throw new ArgumentException(string.Format("UserMail index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion

        protected override int GetIdentityId()
        {
            return UserId;
        }


        public override int CompareTo(BaseEntity other)
        {
            return ((UserMail)other).SendDate.CompareTo(this.SendDate);
        }
    }
}