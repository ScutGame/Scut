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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace ZyGames.Framework.Game.Runtime
{
	/// <summary>
	/// Http request.
	/// </summary>
    public class HttpRequest
    {
		/// <summary>
		/// The content type xml.
		/// </summary>
        public const string ContentTypeXml = "application/xml";
		/// <summary>
		/// Get the specified contentType, url, paramData and successHandle.
		/// </summary>
		/// <param name="contentType">Content type.</param>
		/// <param name="url">URL.</param>
		/// <param name="paramData">Parameter data.</param>
		/// <param name="successHandle">Success handle.</param>
        public void Get(string contentType, string url, string paramData, Func<Stream, bool> successHandle)
        {
            Send("GET", contentType, url, paramData, Encoding.UTF8, successHandle);
        }
		/// <summary>
		/// Post the specified contentType, url, paramData and successHandle.
		/// </summary>
		/// <param name="contentType">Content type.</param>
		/// <param name="url">URL.</param>
		/// <param name="paramData">Parameter data.</param>
		/// <param name="successHandle">Success handle.</param>
        public void Post(string contentType, string url, string paramData, Func<Stream, bool> successHandle)
        {
            Send("POST", contentType, url, paramData, Encoding.UTF8, successHandle);
        }
		/// <summary>
		/// Send the specified methodType, contentType, url, paramData, encoding and successHandle.
		/// </summary>
		/// <param name="methodType">Method type.</param>
		/// <param name="contentType">Content type.</param>
		/// <param name="url">URL.</param>
		/// <param name="paramData">Parameter data.</param>
		/// <param name="encoding">Encoding.</param>
		/// <param name="successHandle">Success handle.</param>
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