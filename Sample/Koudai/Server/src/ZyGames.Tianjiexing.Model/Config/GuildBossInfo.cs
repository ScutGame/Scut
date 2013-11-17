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
using System.Runtime.Serialization;
using System.Text;
using ProtoBuf;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Event;

namespace ZyGames.Tianjiexing.Model.Config
{
    [Serializable, ProtoContract]
    public class GuildBossInfo : EntityChangeEvent
    {
        public GuildBossInfo():base(false)
        {

        }
        /// <summary>
        /// BOSS挑战时间段
        /// </summary>
        [ProtoMember(1)]
        public string EnablePeriod { get; set; }

        /// <summary>
        /// BOSS挑战周期
        /// </summary>
        [ProtoMember(2)]
        public int EnableWeek { get; set; }

        /// <summary>
        /// BOSS挑战设定时间
        /// </summary>
        [ProtoMember(3)]
        public DateTime RefreshDate { get; set; }

        /// <summary>
        /// Boss当前等级
        /// </summary>
        [ProtoMember(4)]
        public short BossLv { get; set; }

        /// <summary>
        /// 是否已杀
        /// </summary>
        [ProtoMember(5)]
        public bool IsKill { get; set; }
    }
}