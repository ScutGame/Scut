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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Serialization;

namespace ZyGames.Framework.Redis
{
    /// <summary>
    /// Server redis info
    /// </summary>
    public class RedisInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public RedisInfo()
        {
            ClientVersion = RedisStorageVersion.Hash;
            SlaveSet = new Dictionary<string, RedisInfo>();
        }
        /// <summary>
        /// Server info hash
        /// </summary>
        public string HashCode { get; set; }

        /// <summary>
        /// Redis client version
        /// </summary>
        public RedisStorageVersion ClientVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerHost { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SerializerType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StarTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, RedisInfo> SlaveSet { get; set; }

    }
}
