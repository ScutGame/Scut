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

namespace ZyGames.Tianjiexing.Model
{
    [Serializable, ProtoContract]
    public class TeamUser
    {
        [ProtoMember(1)]
        public string UserId { get; set; }

        [ProtoMember(2)]
        public string NickName { get; set; }

        //public short UserLv { get; set; }

        //public int UseMagicID { get; set; }
    }
    /// <summary>
    /// 玩家队伍表
    /// </summary>
    [Serializable, ProtoContract]
    public class MorePlotTeam
    {
        private static readonly object lockThis = new object();
        public const int MaxTeamNum = 3;

        public MorePlotTeam()
        {
            UserList = new List<TeamUser>();
            MorePlot = new MorePlot();
            CombatResult = false;
        }

        /// <summary>
        /// 队伍ID
        /// </summary>
        [ProtoMember(1)]
        public int TeamID
        {
            get;
            set;
        }

        /// <summary>
        /// 副本ID
        /// </summary>
        [ProtoMember(2)]
        public MorePlot MorePlot
        {
            get;
            set;
        }

        /// <summary>
        /// 战斗结果
        /// </summary>
        [ProtoMember(3)]
        public bool CombatResult { get; set; }

        /// <summary>
        /// 队长 
        /// </summary>
        [ProtoMember(4)]
        public TeamUser TeamUser
        {
            get;
            set;
        }

        /// <summary>
        /// 是否开始，开始后不可退出
        /// 1：等待中 2：开始战斗 3：解散
        /// </summary>
        [ProtoMember(5)]
        public int Status
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public List<TeamUser> UserList
        {
            get;
            protected set;
        }

        /// <summary>
        /// 是否可加入
        /// </summary>
        public bool IsAllow
        {
            get { return Status == 1 && UserList.Count < MaxTeamNum; }
        }

        public void Append(GameUser user)
        {
            lock (lockThis)
            {
                if (UserList.Count < MaxTeamNum)
                {
                    if (!UserList.Exists(m => m.UserId.Equals(user.UserID)))
                    {
                        UserList.Add(new TeamUser
                        {
                            UserId = user.UserID,
                            NickName = user.NickName,
                            //UserLv = user.UserLv,
                            //UseMagicID = user.UseMagicID
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 删除队员
        /// </summary>
        /// <param name="user"></param>
        public void Remove(GameUser user)
        {
            lock (lockThis)
            {
                UserList.RemoveAll(m => m.UserId.Equals(user.UserID));
            }
        }
    }
}