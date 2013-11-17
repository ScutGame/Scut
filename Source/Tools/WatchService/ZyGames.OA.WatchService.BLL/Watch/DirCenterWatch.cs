using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ZyGames.OA.WatchService.BLL.Watch
{
    public class DirCenterWatch : BaseWatch
    {
        private static string DirCenterUrl = ConfigurationManager.AppSettings["DirCenter_Url"];
        private static string Timming = ConfigurationManager.AppSettings["DirCenter_Timing"];

        public DirCenterWatch()
            : base(Timming)
        {
        }

        protected override bool DoProcess(object obj)
        {
            try
            {
                Logger.SaveLog("The game served sort refresh start...");
                Encoding encode = Encoding.GetEncoding("utf-8");
                string postData = string.Empty;
                byte[] bufferData = encode.GetBytes(postData);

                HttpWebRequest serverRequest = (HttpWebRequest)WebRequest.Create(DirCenterUrl);
                serverRequest.Method = "POST";
                serverRequest.ContentType = "application/x-www-form-urlencoded";
                serverRequest.ContentLength = bufferData.Length;
                Stream requestStream = serverRequest.GetRequestStream();
                requestStream.Write(bufferData, 0, bufferData.Length);
                requestStream.Close();

                //·µ»ØÁ÷
                StringBuilder respContent = new StringBuilder();
                WebResponse serverResponse = serverRequest.GetResponse();
                Stream responseStream = serverResponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string msg = reader.ReadToEnd();
                    Logger.SaveLog(msg);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.SaveLog(ex);
                return false;
            }
        }
    }
}
