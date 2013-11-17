using System;
using System.Data;
using System.Configuration;

namespace CitGame
{
    /// <summary>
    /// BaseLog 的摘要说明
    /// 保存日志到文本的接口类
    /// </summary>
    public class BaseLog:IDisposable
    {
        private string logUserIP = "";
        private string logReqUrl = "";
        protected string folderName = "";
        public BaseLog()
        {

        }
        public BaseLog(string _folderName)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            this.folderName = _folderName;
            this.logReqUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            this.logUserIP = this.GetRealIP();
        }

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
        public void SaveLog(String aUseLog)
        {
            SaveLog(aUseLog, (Exception)null);
        }
        /// <summary>
        /// 保存日志到文本文件
        /// </summary>
        /// <param name="aExObj">出错时的异常描述</param>
        public void SaveLog(Exception aExObj)
        {
            SaveLog("", aExObj);
        }

        /// <summary>
        /// 保存日志到文本文件
        /// </summary>
        /// <param name="aUseLog"></param>
        /// <param name="aExObj"></param>
        public void SaveLog(String aUseLog, Exception aExObj)
        {
            string strLogValue = "";
            strLogValue += "发生日期：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
            strLogValue += "请求地址：" + this.logReqUrl + "\r\n";
            strLogValue += "客户端IP：" + this.logUserIP + "\r\n";
            strLogValue += "错误描述：" + aUseLog;
            if (aExObj == null)
            {
                strLogValue += "\r\n";
                strLogValue += "堆栈详情：\r\n";
            }
            else
            {
                if (aUseLog != "")
                {
                    strLogValue += " >>> ";
                }
                strLogValue += aExObj.Message + "\r\n";
                strLogValue += "堆栈详情：" + aExObj.StackTrace + "\r\n";
            }
            strLogValue += "===============================================================================================================\r\n";
            strLogValue += "===============================================================================================================";

            func.savetofile(strLogValue, this.folderName);
        }


        public string GetRealIP()
        {
            string ip = "";
            try
            {
                System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;

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

        public string GetViaIP()
        {
            string viaIp = "";

            try
            {
                System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;

                if (request.ServerVariables["HTTP_VIA"] != null)
                {
                    viaIp = request.UserHostAddress;
                }

            }
            catch (Exception e)
            {

                this.SaveLog(e);
            }

            return viaIp;
        }


        public void Dispose()
        {

        }
    }
}