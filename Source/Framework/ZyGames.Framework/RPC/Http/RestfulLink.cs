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
using Newtonsoft.Json;

namespace ZyGames.Framework.RPC.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class RestfulLink
    {
        /// <summary>
        /// 
        /// </summary>
        readonly protected string title;
        /// <summary>
        /// 
        /// </summary>
        readonly protected string rel;
        /// <summary>
        /// 
        /// </summary>
        readonly protected string href;

        internal RestfulLink(string title, string href, string rel)
        {
            this.title = title;
            this.rel = rel;
            this.href = href;
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get { return title; } }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("rel")]
        public string Rel { get { return rel; } }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("href")]
        public string Href { get { return href; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="href"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public static RestfulLink Create(string title, string href, string rel = "related")
        {
            return new RestfulLink(title, href, rel);
        }
    }
}
