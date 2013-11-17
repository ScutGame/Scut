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

namespace ZyGames.Tianjiexing.Model.Config
{
    /// <summary>
    /// 公会争夺战过程
    /// </summary>
    [Serializable, ProtoContract]
    public class FightCombatProcess
    {
        private static readonly object thisLock = new object();
        private static int _version = 1;

        public static void RestVersion()
        {
            lock (thisLock)
            {
                _version = 1;
            }
        }

        [ProtoMember(1)]
        public static int CurrVersion { get { return _version; } }

        public static int NextVersion
        {
            get
            {
                lock (thisLock)
                {
                    _version++;
                }
                return _version;
            }
        }
        public FightCombatProcess()
        {
            Version = NextVersion;
            ProcessContainer = new CombatProcessContainer();
        }

        [ProtoMember(2)]
        public string WinUserId
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string WinUserName
        {
            get;
            set;
        }


        [ProtoMember(4)]
        public string FailUserId
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public string FailUserName
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public CombatProcessContainer ProcessContainer
        {
            get;
            set;
        }

        [ProtoMember(7)]
        public int Version
        {
            get;
            set;
        }

        /// <summary>
        /// 胜利获得声望
        /// </summary>
        [ProtoMember(8)]
        public int WinObtainNum { get; set; }

        /// <summary>
        /// 失败获得声望
        /// </summary>
        [ProtoMember(9)]
        public int FailObtainNum { get; set; }

        /// <summary>
        /// 第几届
        /// </summary>
        [ProtoMember(10)]
        public int FastID { get; set; }

        /// <summary>
        /// 战斗阶段
        /// </summary>
        [ProtoMember(11)]
        public FightStage stage { get; set; }
    }
}