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
using ZyGames.Framework.Cache.Generic;

namespace ZyGames.Tianjiexing.Model.Config
{

    /// <summary>
    /// 工会晨练
    /// </summary>
    [Serializable, ProtoContract]
    public class GuildExercise : EntityChangeEvent
    {
        /// <summary>
        /// 参加人员
        /// </summary>
        public CacheList<ExerciseUser> UserList
        {
            get;
            set;
        }
        /// <summary>
        /// 当前
        /// </summary>
        [ProtoMember(1)]
        public int Level
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public int QuestionID
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public int QuestionNo
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public DateTime QuesTime
        {
            get;
            set;
        }

        /// <summary>
        /// 所有玩家答题状态0未检查，1全对，2有错
        /// </summary>
        [ProtoMember(5)]
        public int CheckAllAnswer
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public CacheList<int> QuestionIDList
        {
            get;
            set;
        }
        /// <summary>
        /// 活动状态0未开始，1准备开始，2开始中，3已结束
        /// </summary>
        [ProtoMember(7)]
        public int Status
        {
            get;
            set;
        }

        public GuildExercise():base(false)
        {
            UserList = new CacheList<ExerciseUser>();
            QuestionIDList = new CacheList<int>();
            Level = 1;
        }



    }

    /// <summary>
    /// 晨练玩家
    /// </summary>
    [Serializable, ProtoContract]
    public class ExerciseUser
    {
        public string UserID
        {
            get;
            set;
        }
        /// <summary>
        /// 答题状态
        /// </summary>
        public GuildQuestionStatus QuestionStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 晨练状态
        /// </summary>
        public GuildExerciseStatus Status
        {
            get;
            set;
        }

        public bool AnswerStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 题目编号
        /// </summary>
        public int QuestionNo
        {
            get;
            set;
        }

        /// <summary>
        /// 晶石回答次数
        /// </summary>
        public int GameConisCount
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 历练状态
    /// </summary>
    public enum GuildExerciseStatus
    {
        Default,
        All,
    }

    /// <summary>
    /// 答题状态
    /// </summary>
    public enum GuildQuestionStatus
    {
        /// <summary>
        /// 待开始
        /// </summary>
        NotOpen = 0,
        /// <summary>
        /// 待回答
        /// </summary>
        ToAnswer = 1,
        /// <summary>
        /// 待结果
        /// </summary>
        WaitForResults = 2,
        /// <summary>
        /// 待下一题
        /// </summary>
        ToNext = 3,
    }
}