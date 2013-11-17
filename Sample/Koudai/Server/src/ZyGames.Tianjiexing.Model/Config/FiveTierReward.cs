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
using ZyGames.Framework.Event;

namespace ZyGames.Tianjiexing.Model.Config
{
    [Serializable, ProtoContract]
    public class FiveTierReward : EntityChangeEvent
    {
        public FiveTierReward()
            : base(false)
        {
            
        }

        private Int32 _battleNum;

        /// <summary>
        /// 某一轮
        /// </summary>
        [ProtoMember(1)]
        public Int32 BattleNum
        {
            get { return _battleNum; }
            set
            {
                _battleNum = value;
                NotifyByModify();
            }
        }

        private Int32 _fiveTierNum;

        /// <summary>
        /// 每五层
        /// </summary>
        [ProtoMember(2)]
        public Int32 FiveTierNum
        {
            get { return _fiveTierNum; }
            set
            {
                _fiveTierNum = value;
                NotifyByModify();
            }
        }

        private Int32 _fiveTierStarNum;

        /// <summary>
        /// 五层星星数
        /// </summary>
        [ProtoMember(3)]
        public Int32 FiveTierStarNum
        {
            get { return _fiveTierStarNum; }
            set
            {
                _fiveTierStarNum = value;
                NotifyByModify();
            }
        }

    }
}