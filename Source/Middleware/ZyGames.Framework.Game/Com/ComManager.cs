using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Collection.Generic;

namespace ZyGames.Framework.Game.Com
{
    /// <summary>
    /// 中间层组件管理类（抽象工厂模式）
    /// </summary>
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
