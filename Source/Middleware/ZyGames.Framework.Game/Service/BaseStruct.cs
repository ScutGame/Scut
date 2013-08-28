using System;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Locking;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Framework.Game.Service
{
    public abstract class BaseStruct : GameStruct
    {
        /// <summary>
        /// 
        /// </summary>
        public static string PublishType = ConfigUtils.GetSetting("PublishType");

        /// <summary>
        /// 本次登录SessionID句柄
        /// </summary>
        protected string Sid;
        /// <summary>
        /// 提交Action用户唯一ID
        /// </summary>
        protected string Uid;

        private int _userId;
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            get
            {
                return _userId;
            }
            set { _userId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Func<int, BaseUser> UserFactory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GameContext Current { get; private set; }

        /// <summary>
        /// url分解器
        /// </summary>
        protected HttpGet httpGet;
        /// <summary>
        /// 是否是压力测试
        /// </summary>
        protected bool IsRunLoader
        {
            get
            {
                return CheckRunloader(httpGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsRealse { get { return PublishType.Equals("Release"); } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpGet"></param>
        /// <returns></returns>
        public static bool CheckRunloader(HttpGet httpGet)
        {
            return !IsRealse && httpGet.ContainsKey("rl");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aActionId"></param>
        /// <param name="httpGet"></param>
        protected BaseStruct(short aActionId, HttpGet httpGet) :
            base(aActionId)
        {
            actionId = aActionId;
            this.httpGet = httpGet;
        }

        /// <summary>
        /// 子类实现
        /// </summary>
        protected virtual void InitChildAction()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoInit()
        {
            Uid = "0";
            httpGet.GetString("uid", ref Uid);
            httpGet.GetInt("uid", ref _userId);

            if (!IsPush)
            {
                httpGet.GetInt("MsgId", ref MsgId);
            }
            string st = httpGet.GetStringValue("St");
            if (!string.IsNullOrEmpty(st))
            {
                St = st;
            }
            if (!IsIgnoreUid())
            {
                InitContext(actionId, UserId);
            }
            InitAction();
            InitChildAction();
        }

        internal protected void InitContext(int actionId, int userId)
        {
            if (userId > 0)
            {
                Current = GameContext.GetInstance(actionId, userId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ILocking RequestLock()
        {
            ILocking strategy = null;
            if (Current != null)
            {
                var tempLock = Current.MonitorLock.Lock();
                if (tempLock != null && tempLock.TryEnterLock())
                {
                    strategy = tempLock;
                }
                else
                {
                    ErrorCode = LanguageHelper.GetLang().ErrorCode;
                    if (!IsRealse) ErrorInfo = LanguageHelper.GetLang().ServerBusy;
                    TraceLog.WriteError("Action-{0} Uid:{1} locked timeout.", actionId, UserId);
                }
            }
            return strategy;
        }

        /// <summary>
        /// 子类实现Action处理
        /// </summary>
        /// <returns></returns>
        public abstract bool TakeAction();

        /// <summary>
        /// 处理结束执行
        /// </summary>
        public virtual void TakeActionAffter(bool state)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckAction()
        {
            return true;
        }

        /// <summary>
        /// 执行Action处理
        /// </summary>
        /// <returns></returns>
        public override bool DoAction()
        {
            bool result;
            try
            {
                if (!CheckAction())
                {
                    return false;
                }
                result = TakeAction();
                TakeActionAffter(result);
            }
            catch (Exception ex)
            {
                SaveLog(ex);
                ErrorCode = LanguageHelper.GetLang().ErrorCode;
                if (!IsRealse)
                {
                    ErrorInfo = LanguageHelper.GetLang().ServerBusy;
                }
                return false;
            }
            return result;
        }

        /// <summary>
        /// 读取Url参数
        /// </summary>
        /// <returns></returns>
        public override bool ReadUrlElement()
        {
            //调整加锁位置
            bool result = false;
            httpGet.GetString("Sid", ref Sid);
            if (GetUrlElement())
            {
                if (ValidateElement())
                {
                    result = true;
                }
                else if (ErrorCode == 0)
                {
                    ErrorCode = LanguageHelper.GetLang().ValidateCode;
                    if (!IsRealse)
                    {
                        ErrorInfo = LanguageHelper.GetLang().ValidateError;
                    }
                }
            }
            else
            {
                if (ErrorCode == 0)
                {
                    ErrorCode = LanguageHelper.GetLang().ErrorCode;
                    if (!IsRealse)
                    {
                        ErrorInfo = LanguageHelper.GetLang().UrlElement + httpGet.ErrorMsg;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 较验参数
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateElement()
        {
            return true;
        }

        /// <summary>
        /// 是否此请求忽略UID参数
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsIgnoreUid()
        {
            return false;
        }


        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="actionStat"></param>
        /// <param name="cont"></param>
        protected override void SaveActionLogToDB(LogActionStat actionStat, string cont)
        {
            try
            {
                ActionCount.ActionVisit(actionId, actionStat);

            }
            catch (Exception ex)
            {
                SaveLog(ex);
            }
        }
    }
}
