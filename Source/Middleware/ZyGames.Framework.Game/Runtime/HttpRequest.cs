using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace ZyGames.Framework.Game.Runtime
{
    public class HttpRequest
    {
        public const string ContentTypeXml = "application/xml";

        public void Get(string contentType, string url, string paramData, Func<Stream, bool> successHandle)
        {
            Send("GET", contentType, url, paramData, Encoding.UTF8, successHandle);
        }

        public void Post(string contentType, string url, string paramData, Func<Stream, bool> successHandle)
        {
            Send("POST", contentType, url, paramData, Encoding.UTF8, successHandle);
        }

        public bool Send(string methodType, string contentType, string url, string paramData, Encoding encoding, Func<Stream, bool> successHandle)
        {
            byte[] bytesRequestData = encoding.GetBytes(paramData);
            WebRequest myReq = WebRequest.Create(url);
            myReq.Method = methodType;
            if (!string.IsNullOrEmpty(contentType))
            {
                myReq.ContentType = contentType;
            }

            //填充POST数据
            myReq.ContentLength = bytesRequestData.Length;
            Stream requestStream = myReq.GetRequestStream();
            requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
            requestStream.Close();

            //发送POST数据请求服务器
            HttpWebResponse httpResp = (HttpWebResponse)myReq.GetResponse();
            if (httpResp != null)
            {
                Stream myStream = httpResp.GetResponseStream();
                return successHandle(myStream);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string ToString(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public XmlDocument ToXml(Stream stream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlTextReader Reader = new XmlTextReader(stream);
            xmlDoc.Load(Reader);
            return xmlDoc;
        }
    }
}
