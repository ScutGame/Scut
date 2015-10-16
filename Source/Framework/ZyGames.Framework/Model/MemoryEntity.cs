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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Event;
using ProtoBuf;

namespace ZyGames.Framework.Model
{
    /// <summary>
    /// 内存中一定时间存在的实体，不存储数据库
    /// </summary>
    [ProtoContract, Serializable]
    public class MemoryEntity : EntityChangeEvent, IDataExpired, ISqlEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public MemoryEntity()
            : base(true)
        {
        }

        /// <summary>
        /// entity modify time.
        /// </summary>
        [ProtoMember(100025)]
        public DateTime TempTimeModify { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeChangeEvent()
        {
            //禁用Change事件绑定
        }

        /// <summary>k
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool RemoveExpired(string key)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetKeyCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int GetMessageQueueId()
        {
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PersonalId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ResetState()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Reset()
        {
            ResetState();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetCreateTime()
        {
            return DateTime.Now;
        }

    }
}