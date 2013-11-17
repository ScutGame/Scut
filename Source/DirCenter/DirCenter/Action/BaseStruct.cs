using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;using ZyGames.GameService.BaseService.StructService;
using ZyGames.Core.Util;
using ZyGames.Core.Data;
using ZyGames.DirCenter.BLL.Lang;

namespace ZyGames.DirCenter.Action
{
    /// <summary>
    /// BaseStruct 的摘要说明
    /// </summary>
    public abstract class BaseStruct : GameStruct
    {
        protected int GameID = 0;
        /// <summary>
        /// url分解器
        /// </summary>
        protected HttpGet httpGet;       
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aActionId"></param>
        public BaseStruct(short aActionId, HttpGet httpGet) :
            base(System.Web.HttpContext.Current.Response, aActionId)
        {
            this.aAction = aActionId;
            this.httpGet = httpGet;
            InitAction(System.Web.HttpContext.Current.Response);
        }

        public abstract bool TakeAction();

        public override bool DoAction()
        {
            if (GameID == 0)
            {
                TakeAction();
            }
            else
            {
                ulong lock_cas = 0;
                string lock_key = string.Format(ZyLock.FmtLockKey, this.aAction, this.GameID);
                try
                {
                    if (ZyLock.Instance().LockKey(lock_key, out lock_cas))
                    {
                        try
                        {
                            TakeAction();
                        }
                        catch (Exception ex)
                        {
                            this.SaveLog(ex);
                            this.ErrorInfo = zh_cs.ServerBusy;
                        }
                        finally
                        {
                            ZyLock.Instance().UnLockKey(lock_key, lock_cas);
                        }
                    }
                    else
                    {
                        this.ErrorInfo = zh_cs.ServerBusy;
#if LoadRunner
                        this.SaveLog("LockKey" + this.ErrorInfo, null);
#endif
                    }
                }
                catch (Exception ex)
                {
                    this.SaveLog(ex);
                    this.ErrorInfo = zh_cs.ServerBusy;
                }

            }
            return true;
        }

        public override bool ReadUrlElement()
        {
            if (this.GetUrlElement())
            {
                return true;
            }
            else
            {
                this.ErrorInfo = zh_cs.UrlElement;
                return false;
            }
        }

     /*   protected void SendSqlCmd(string connection, CommandType type, string sql, SqlParameter[] para)
        {
#if DEBUG
            SqlHelper.ExecuteNonQuery(connection, type, sql, para);
#else
            、、ActionMSMQ.Instance().SendSqlCmd(connection, type, sql, para);
#endif
        }*/

        protected override void SaveActionLogToDB(GameStruct.LogActionStat actionStat, string cont)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                this.SaveLog(ex);
            }
        }
    }
}