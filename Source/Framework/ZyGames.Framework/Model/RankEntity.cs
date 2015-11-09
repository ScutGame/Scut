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
using Newtonsoft.Json;
using ProtoBuf;
using ZyGames.Framework.Event;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// Rank entity
    /// </summary>
    [ProtoContract, Serializable]
    public abstract class RankEntity : AbstractEntity, IComparable<RankEntity>
    {
        /// <summary>
        /// init
        /// </summary>
        protected RankEntity()
            : base(false)
        {

        }

        /// <summary>
        /// Gets key of rank
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// Get score of rank no, from hight to low
        /// </summary>
        [JsonIgnore]
        public double Score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal protected override int GetIdentityId()
        {
            return DefIdentityId;
        }
        /// <summary>
        /// from hight to low
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(RankEntity other)
        {
            return other.Score.CompareTo(Score);
        }
        /// <summary>
        /// 当前对象(包括继承)的属性触发通知事件
        /// </summary>
        /// <param name="sender">触发事件源</param>
        /// <param name="eventArgs"></param>
        protected override void Notify(object sender, CacheItemEventArgs eventArgs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">由IContainer对象触发</param>
        /// <param name="eventArgs"></param>
        protected override void NotifyByChildren(object sender, CacheItemEventArgs eventArgs)
        {
        }

        internal override void UnChangeNotify(object sender, CacheItemEventArgs eventArgs)
        {
        }
    }
}
