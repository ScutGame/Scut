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
using System.Diagnostics;
using System.Web;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;

namespace ZyGames.Framework.Game.Service
{

    /// <summary>
    /// BaseStruct 的摘要说明
    /// </summary>
    public abstract class GameStruct
    {
		/// <summary>
		/// The action time out.
		/// </summary>
        public static int ActionTimeOut = 500;

        static GameStruct()
        {
            int timeOut = ConfigUtils.GetSetting("ActionTimeOut", 0);
            if (timeOut > 0) ActionTimeOut = timeOut;
        }

        /// <summary>
        /// 默认的返回错误信息
        /// </summary>
        public const string DefaultErrorInfo = "访问失败";

        /// <summary>
        /// 接口访问处理情况
        /// </summary>
        public enum LogActionStat
        {
            /// <summary>
            /// 接口访问成功
            /// </summary>
            Sucess = 0,
            /// <summary>
            /// 访问失败
            /// </summary>
            Fail
        }
        /// <summary>
        /// 接口访问开始时间
        /// </summary>
        protected DateTime iVisitBeginTime;
        /// <summary>
        /// 接口访问结束时间
        /// </summary>
        protected DateTime iVisitEndTime;
        private string logActionResult = "";
        /// <summary>
        /// 写日志的对象
        /// </summary>
        protected BaseLog oBaseLog = null;
        /// <summary>
        /// 数据类
        /// </summary>
        protected DataStruct dataStruct = new DataStruct();

        /// <summary>
        /// ActionID，接口编号
        /// </summary>
        protected int actionId;
        /// <summary>
        /// 是否是错误的URL请求串
        /// </summary>
        private bool IsError = false;

        /// <summary>
        /// 是否是主动推送
        /// </summary>
        protected bool IsPush = false;

        /// <summary>
        /// 请求上来的消息编号，主动下发编号为0
        /// </summary>
        protected int MsgId = 0;

        /// <summary>
        /// 消息St
        /// </summary>
        protected string St = "st";

        /// <summary>
        /// 返回Action是否为ErrorAction
        /// </summary>
        /// <returns></returns>
        public bool GetError()
        {
            return IsError;
        }
        private string errorInfo = string.Empty;
        /// <summary>
        /// 获取或设置错误信息
        /// </summary>
        public String ErrorInfo
        {
            get
            {
                return errorInfo;
            }
            set
            {
                errorInfo = value;
            }
        }

        private int errorCode = 0;
        /// <summary>
        /// 获取或设置错误信息
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return errorCode;
            }
            set
            {
                if (errorCode != 0)
                {
                    IsError = true;
                }
                errorCode = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aActionId"></param>
        protected GameStruct(int aActionId)
        {
            this.actionId = aActionId;
        }

        /// <summary>
        /// int类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(int obj)
        {
            dataStruct.PushIntoStack(obj);
        }
        /// <summary>
        /// short类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(short obj)
        {
            dataStruct.PushIntoStack(obj);
        }
        /// <summary>
        /// byte类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(byte obj)
        {
            dataStruct.PushIntoStack(obj);
        }
        /// <summary>
        /// string类型
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(string obj)
        {
            dataStruct.PushIntoStack(obj);
        }
        /// <summary>
        /// 将数据加到栈尾
        /// </summary>
        /// <param name="obj"></param>
        public void PushIntoStack(DataStruct obj)
        {
            dataStruct.PushIntoStack(obj);
        }

        /// <summary>
        /// 输出当前协议返回
        /// </summary>
        public void WriteAction(IGameResponse response)
        {
            dataStruct.WriteAction(response, actionId, errorCode, errorInfo, MsgId, St);
            this.iVisitEndTime = DateTime.Now;
            WatchAction();
            this.SaveActionLogToDB(LogActionStat.Sucess, this.logActionResult);
        }

        /// <summary>
        /// 输出一个错误的Action 与 相应错误内容描述
        /// </summary>
        public void WriteErrorAction(IGameResponse response)
        {
            logActionResult += errorCode.ToString() + "-";
            if (string.IsNullOrEmpty(errorInfo))
            {
                logActionResult += DefaultErrorInfo + "-";
            }
            else
            {
                logActionResult += errorInfo + "-";
            }
            this.iVisitEndTime = DateTime.Now;
            WatchAction();
            this.SaveActionLogToDB(LogActionStat.Fail, logActionResult);
            dataStruct.WriteAction(response, actionId, errorCode, errorInfo, MsgId, St);
        }

        private void WatchAction()
        {
        }

        //protected void InitAction(System.Web.HttpResponse m_Response)
        //{
        //    IGameResponse response = new HttpGameResponse(m_Response);
        //    InitAction(response);
        //}

        /// <summary>
        /// 设为推送模式
        /// </summary>
        public void SetPush()
        {
            IsPush = true;
        }
        /// <summary>
        /// 初始化Action
        /// </summary>
        protected void InitAction()
        {
            this.iVisitBeginTime = DateTime.Now;
        }

        /// <summary>
        /// ReadUrlElement
        /// </summary>
        /// <returns></returns>
        public abstract bool ReadUrlElement();
        /// <summary>
        /// 接收用户请求的参数，并根据相应类进行检测
        /// </summary>
        /// <returns></returns>
        public abstract bool GetUrlElement();
        /// <summary>
        /// 响应动作类
        /// </summary>
        public abstract bool DoAction();
        /// <summary>
        /// 创建返回协议内容输出栈
        /// </summary>
        public abstract void BuildPacket();

        #region //日志记录

        /// <summary>
        /// 保存日志到文本文件
        /// 日志格式：
        /// 1.请求串
        /// 2.发生日期
        /// 3.错误描述
        /// 4.错误栈详细
        /// 5.分隔线
        /// </summary>
        /// <param name="aUseLog">日志内容</param>
        protected void SaveLog(String aUseLog)
        {
            if (oBaseLog == null)
            {
                oBaseLog = new BaseLog();
            }
            oBaseLog.SaveLog(aUseLog);
        }
		/// <summary>
		/// Saves the debu log.
		/// </summary>
		/// <param name="aUseLog">A use log.</param>
        protected void SaveDebuLog(String aUseLog)
        {
            if (oBaseLog == null)
            {
                oBaseLog = new BaseLog();
            }
            oBaseLog.SaveDebugLog(aUseLog);
        }
        /// <summary>
        /// 保存日志到文本文件
        /// </summary>
        /// <param name="aExObj">出错时的异常描述</param>
        protected void SaveLog(Exception aExObj)
        {
            SaveLog("", aExObj);
        }

        /// <summary>
        /// 保存日志到文本文件
        /// </summary>
        /// <param name="aUseLog"></param>
        /// <param name="aExObj"></param>
        protected void SaveLog(String aUseLog, Exception aExObj)
        {
            if (oBaseLog == null)
            {
                oBaseLog = new BaseLog("Action" + this.actionId.ToString());
            }
            oBaseLog.SaveLog(aUseLog, aExObj);
        }

        /// <summary>
        /// 保存日志方法
        /// </summary>
        /// <param name="actionStat"></param>
        /// <param name="cont"></param>
        protected abstract void SaveActionLogToDB(LogActionStat actionStat, string cont);

        /// <summary>
        /// 获得访问者的IP
        /// </summary>
        /// <returns></returns>
        protected string GetRealIP()
        {
            string ip = "";
            try
            {
                if (HttpContext.Current == null) return ip;

                HttpRequest request = HttpContext.Current.Request;

                if (request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                    }
                }
                else
                {
                    ip = request.UserHostAddress;
                }
            }
            catch (Exception e)
            {
                this.SaveLog(e);
            }

            return ip;
        }
        #endregion

    }
}