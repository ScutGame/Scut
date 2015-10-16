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
namespace ZyGames.Framework.Game.Sns.Service
{

    /// <summary>
    /// 
    /// </summary>
    public class ResponseBody<T> where T : ResponseData, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public ResponseBody()
        {
            StateCode = StateCode.OK;
            StateDescription = "";
            Vesion = "1.0";
            Handler = "";
            Data = new T();
        }
        /// <summary>
        /// 
        /// </summary>
        public StateCode StateCode { get; set; }
        /// <summary>
        /// /
        /// </summary>
        public string StateDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Vesion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ResponseBody : ResponseBody<ResponseData>
    {

    }
}