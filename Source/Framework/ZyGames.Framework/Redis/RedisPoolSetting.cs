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
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Config;

namespace ZyGames.Framework.Redis
{
    /// <summary>
    /// Redis Pool Setting
    /// </summary>
    public class RedisPoolSetting
    {
        private RedisSection _redisSection;

        /// <summary>
        /// 
        /// </summary>
        public RedisPoolSetting(bool useConfig = true)
        {
            _redisSection = useConfig ? ConfigManager.Configger.GetFirstOrAddConfig<RedisSection>() : new RedisSection(false);
        }

        /// <summary>
        /// Host, format:password@ip:port
        /// </summary>
        public string Host
        {
            get { return _redisSection.Host; }
            set { _redisSection.Host = value; }
        }
        /// <summary>
        /// ReadOnlyHost
        /// </summary>
        public string ReadOnlyHost
        {
            get { return _redisSection.ReadOnlyHost; }
            set { _redisSection.Host = value; }
        }
        /// <summary>
        /// MaxWritePoolSize
        /// </summary>
        public int MaxWritePoolSize
        {
            get { return _redisSection.MaxWritePoolSize; }
            set { _redisSection.MaxWritePoolSize = value; }
        }
        /// <summary>
        /// MaxReadPoolSize
        /// </summary>
        public int MaxReadPoolSize
        {
            get { return _redisSection.MaxReadPoolSize; }
            set { _redisSection.MaxReadPoolSize = value; }
        }
        /// <summary>
        /// ConnectTimeout(ms)
        /// </summary>
        public int ConnectTimeout
        {
            get { return _redisSection.ConnectTimeout; }
            set { _redisSection.ConnectTimeout = value; }
        }
        /// <summary>
        /// PoolTimeOut(ms), default 2000ms
        /// </summary>
        public int PoolTimeOut
        {
            get { return _redisSection.PoolTimeOut; }
            set { _redisSection.PoolTimeOut = value; }
        }
        /// <summary>
        /// DbIndex
        /// </summary>
        public int DbIndex
        {
            get { return _redisSection.DbIndex; }
            set { _redisSection.DbIndex = value; }
        }

        /// <summary>
        /// ClientVersion
        /// </summary>
        public RedisStorageVersion ClientVersion
        {
            get { return _redisSection.ClientVersion; }
            set { _redisSection.ClientVersion = value; }
        }
    }
}
