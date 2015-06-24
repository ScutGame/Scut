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
using System.Net;
using System.Web;

namespace ZyGames.Framework.Game.Service
{
    /// <summary>
    /// Http流输出类
    /// </summary>
    public class HttpGameResponse : BaseGameResponse
    {
        /// <summary>
        /// 输出类
        /// </summary>
        private System.Web.HttpResponse _response;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public HttpGameResponse(HttpResponse response)
        {
            _response = response;
            _response.Charset = "unicode";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public override void BinaryWrite(byte[] buffer)
        {
            _response.BinaryWrite(buffer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public override void Write(byte[] buffer)
        {
            _response.OutputStream.Write(buffer, 0, buffer.Length);
        }

    }

    /// <summary>
    /// HttpListent流输出类
    /// </summary>
    public class HttpListentGameResponse : BaseGameResponse
    {
        private readonly HttpListenerResponse _response;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public HttpListentGameResponse(HttpListenerResponse response)
        {
            _response = response;
            response.ContentType = "application/octet-stream";
            response.AddHeader("Access-Control-Allow-Origin", "*");
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
        public override void BinaryWrite(byte[] buffer)
        {
            _response.OutputStream.Write(buffer, 0, buffer.Length);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
        public override void Write(byte[] buffer)
        {
            _response.OutputStream.Write(buffer, 0, buffer.Length);
        }

    }
}