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
    /// 战斗中的阵形
    /// </summary>
    public class CombatEmbattle
    {
        public string UserID
        {
            get;
            set;
        }
        public int GeneralID
        {
            get;
            set;
        }

        public short GeneralLv
        {
            get;
            set;
        }

        public string GeneralName
        {
            get;
            set;
        }
        public string HeadID
        {
            get;
            set;
        }

        public int AbilityID
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        public int LiveNum
        {
            get;
            set;
        }

        public int LiveMaxNum
        {
            get;
            set;
        }

        public short MomentumNum
        {
            get;
            set;
        }

        public short MaxMomentumNum
        {
            get;
            set;
        }
    }
}