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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Generic
{
    /// <summary>
    /// 新手引导
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class NoviceGuide<T, V>
        where T : UserGuide, new()
        where V : GuideData, new()
    {
		/// <summary>
		/// The user identifier.
		/// </summary>
        protected readonly int UserId;
        private ShareCacheStruct<V> _guideSet;
        private T _userGuide;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        protected NoviceGuide(int userId)
        {
            UserId = userId;
            _guideSet = new ShareCacheStruct<V>();
            var cacheSet = new PersonalCacheStruct<T>();
            _userGuide = cacheSet.FindKey(userId.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateUserGuide();

        /// <summary>
        /// 
        /// </summary>
        protected void CheckUserGuide()
        {
            if (_userGuide == null)
            {
                _userGuide = CreateUserGuide();
                new PersonalCacheStruct<T>().Add(_userGuide);
            }
        }
        /// <summary>
        /// 是否完成引导
        /// </summary>
        public bool HasClose
        {
            get
            {
                CheckUserGuide();
                return _userGuide == null ? false : _userGuide.IsClose;
            }
        }
        /// <summary>
        /// 检测是否有引导
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public bool Check(out GuideProgressItem progress)
        {
            V guideData;
            progress = null;
            CheckUserGuide();
            int currGuideId = _userGuide == null ? 0 : _userGuide.CurrGuideId;
            if (currGuideId == 0)
            {
                if (TryGetNextId(currGuideId, out guideData))
                {
                    currGuideId = guideData.ID;
                }
                else
                {
                    CloseGuide();
                    return false;
                }
            }
            GuideProgressItem guideProgress;
            if (TryGetOrAddProgress(currGuideId, out guideProgress))
            {
                if (guideProgress.Status != GuideStatus.Closed)
                {
                    progress = guideProgress;
                    return true;
                }
                if (TryGetNextId(currGuideId, out guideData))
                {
                    //下发下一个引导
                    if (TryGetOrAddProgress(guideData.ID, out guideProgress))
                    {
                        progress = guideProgress;
                        return true;
                    }
                }
                else
                {
                    CloseGuide();
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取当前引导
        /// </summary>
        public GuideProgressItem CurrentProgress
        {
            get
            {
                int currGuideId = _userGuide == null ? 0 : _userGuide.CurrGuideId;
                GuideProgressItem guideProgress;
                if (TryGetOrAddProgress(currGuideId, out guideProgress))
                {
                    return guideProgress;
                }
                return null;
            }
        }
        /// <summary>
        /// 处理引导完成
        /// </summary>
        public bool DoComplated(int guideId)
        {
            GuideProgressItem guideProgress;
            if (TryGetProgress(guideId, out guideProgress))
            {
                guideProgress.Status = GuideStatus.Complate;
            }
            return false;
        }

        /// <summary>
        /// 处理奖励
        /// </summary>
        /// <param name="guideId"></param>
        /// <returns></returns>
        public object DoPrize(int guideId)
        {
            GuideProgressItem guideProgress;
            if (TryGetProgress(guideId, out guideProgress))
            {
                guideProgress.Status = GuideStatus.Closed;
            }
            return ProcessPrize();
        }
        /// <summary>
        /// 子类实现奖励
        /// </summary>
        /// <returns></returns>
        protected virtual object ProcessPrize()
        {
            return null;
        }

        /// <summary>
        /// 完成引导
        /// </summary>
        protected void CloseGuide()
        {
            if (_userGuide != null)
            {
                _userGuide.IsClose = true;
                _userGuide.CloseDate = DateTime.Now;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guideId"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        protected bool TryGetProgress(int guideId, out GuideProgressItem progress)
        {
            progress = null;
            if (_guideSet.FindKey(guideId) == null)
            {
                return false;
            }
            progress = _userGuide.GuideProgress.Find(m => m.GuideId == guideId);
            return progress != null;
        }

        /// <summary>
        /// 尝试获取引导进度
        /// </summary>
        /// <param name="guideId"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        protected bool TryGetOrAddProgress(int guideId, out GuideProgressItem progress)
        {
            progress = null;
            if (_guideSet.FindKey(guideId) == null)
            {
                return false;
            }
            progress = _userGuide.GuideProgress.Find(m => m.GuideId == guideId);
            if (progress == null)
            {
                progress = new GuideProgressItem() { GuideId = guideId };
                _userGuide.CurrGuideId = guideId;
                _userGuide.GuideProgress.Add(progress);
            }
            return true;
        }

        /// <summary>
        /// 尝试获取下个引导，未找到引导完成
        /// </summary>
        /// <param name="guideId"></param>
        /// <param name="guideData"></param>
        /// <returns></returns>
        protected bool TryGetNextId(int guideId, out V guideData)
        {
            int nextGuidId = 0;
            guideData = _guideSet.FindKey(guideId);
            if (guideData == null)
            {
                _guideSet.Foreach((key, g) =>
                {
                    if (nextGuidId == 0 || g.ID < nextGuidId)
                    {
                        nextGuidId = g.ID;
                    }
                    return true;
                });
            }
            else
            {
                nextGuidId = guideData.NextID;
            }
            guideData = _guideSet.FindKey(nextGuidId);
            return guideData != null;
        }
    }
}