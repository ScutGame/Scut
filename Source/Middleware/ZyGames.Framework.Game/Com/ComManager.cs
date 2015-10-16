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
using ProtoBuf;
using ZyGames.Framework.Collection.Generic;

namespace ZyGames.Framework.Game.Com
{
    /// <summary>
    /// 中间层组件管理类（抽象工厂模式）
    /// </summary>
    [Serializable, ProtoContract]
    public static class ComManager
    {
        private static DictionaryExtend<string, ComProxy> _pools = new DictionaryExtend<string, ComProxy>();

        /// <summary>
        /// 
        /// </summary>
        public static void Start(List<ComConfig> configs)
        {

        }

        /// <summary>
        /// 注册中间件
        /// </summary>
        public static void Register(ComConfig config)
        {

        }

        /// <summary>
        /// 组件启动之后处理
        /// </summary>
        public static void StartAfter(Func<bool> loadFactory)
        {
            loadFactory();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
        }

        private static T LoadProxy<T>(string comKey) where T : ComProxy
        {
            T proxy;
            if (TryLoadProxy(comKey, out proxy))
            {
                return proxy;
            }
            throw new NullReferenceException(string.Format("LoadProxy:{0} is error", comKey));
        }

        private static bool TryLoadProxy<T>(string comKey, out T proxy) where T : ComProxy
        {
            proxy = null;
            ComProxy comProxy;
            if (_pools.TryGetValue(comKey, out comProxy))
            {
                proxy = (T)comProxy;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建排行榜中间件代理对象
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns></returns>
        public static RankingProxy CreateRanking()
        {
            return LoadProxy<RankingProxy>("Ranking");
        }

        /// <summary>
        /// 新手引导中间件
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        /// <returns></returns>
        public static NoviceGuideProxy CreateNoviceGuide()
        {
            return LoadProxy<NoviceGuideProxy>("NoviceGuide");
        }
    }
}