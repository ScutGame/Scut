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
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Combat
{
    /// <summary>
    /// 国家领土战过程
    /// </summary>
    public class CountryCombatProcess
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

        public static int CurrVersion{ get { return _version; } }

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
        public CountryCombatProcess()
        {
            Version = NextVersion;
            ProcessContainer= new CombatProcessContainer();
        }

        public string WinUserId
        {
            get;
            set;
        }

        public string WinUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 胜方连杀
        /// </summary>
        public int KillNum
        {
            get;
            set;
        }
        /// <summary>
        /// 败方连杀
        /// </summary>
        public int FaildKillNum
        {
            get;
            set;
        }
        public int WinGameCoin
        {
            get;
            set;
        }
        public int WinObtainNum
        {
            get;
            set;
        }

        public string FailUserId
        {
            get;
            set;
        }

        public string FailUserName
        {
            get;
            set;
        }

        public int FailGameCoin
        {
            get;
            set;
        }
        public int FailObtainNum
        {
            get;
            set;
        }

        public CombatProcessContainer ProcessContainer
        {
            get;
            set;
        }

        public int Version
        {
            get;
            set;
        }
    }
}