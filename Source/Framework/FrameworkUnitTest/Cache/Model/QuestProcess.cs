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
using System.Threading.Tasks;
using ProtoBuf;
using ZyGames.Framework.Model;

namespace FrameworkUnitTest.Cache.Model
{
    [Serializable, ProtoContract]
    [EntityTable(CacheType.Entity, MyDataConfigger.DbKey)]
    public class QuestProcess : ShareEntity
    {
        public QuestProcess()
            : base(false)
        {
        }

        [ProtoMember(1)]
        [EntityField(true)]
        public int Suoyin
        {
            get; set; 
        }
        /// <summary>  /// 玩家的Id
        /// </summary>
        private int _character;
        [ProtoMember(2)]
        [EntityField]
        public int Character
        {
            get { return _character; }
            set { _character = value; }
        }
        /// <summary>  /// 任务的Id
        /// </summary>
        private int _questindex;
        [ProtoMember(3)]
        [EntityField]
        public int QuestIndex
        {
            get { return _questindex; }
            set { _questindex = value; }
        }
        /// <summary>  /// 接到任务的时间
        /// </summary>
        private DateTime _registerdate;
        [ProtoMember(4)]
        [EntityField]
        public DateTime RegisterDate
        {
            get { return _registerdate; }
            set { _registerdate = value; }
        }
        /// <summary>  /// 任务更新时间
        /// </summary>
        private DateTime _updatedate;
        [ProtoMember(5)]
        [EntityField]
        public DateTime UpdateDate
        {
            get { return _updatedate; }
            set { _updatedate = value; }
        }
        /// <summary>  /// 任务完成时间
        /// </summary>
        private DateTime _completedate;
        [ProtoMember(6)]
        [EntityField]
        public DateTime CompleteDate
        {
            get { return _completedate; }
            set { _completedate = value; }
        }
        /// <summary>  /// 待定
        /// </summary>
        private int _checkcount;
        [ProtoMember(7)]
        [EntityField]
        public int CheckCount
        {
            get { return _checkcount; }
            set { _checkcount = value; }
        }
        /// <summary>  /// 已完成次数
        /// </summary>
        private int _timecount;
        [ProtoMember(8)]
        [EntityField]
        public int TimeCount
        {
            get { return _timecount; }
            set { _timecount = value; }
        }
        /// <summary>  /// 任务状态
        /// </summary>
        private int _queststate;
        [ProtoMember(9)]
        [EntityField]
        public int QuestState
        {
            get { return _queststate; }
            set { _queststate = value; }
        }
        /// <summary>  /// 是否删除
        /// </summary>
        private int _apply;
        [ProtoMember(10)]
        [EntityField]
        public int Apply
        {
            get { return _apply; }
            set { _apply = value; }
        }
        /// <summary>  /// ConditionState1
        /// </summary>
        private int _conditionstate1;
        [ProtoMember(11)]
        [EntityField]
        public int ConditionState1
        {
            get { return _conditionstate1; }
            set { _conditionstate1 = value; }
        }
        /// <summary>  /// ConditionState2
        /// </summary>
        private int _conditionstate2;
        [ProtoMember(12)]
        [EntityField]
        public int ConditionState2
        {
            get { return _conditionstate2; }
            set { _conditionstate2 = value; }
        }
        /// <summary>  /// ConditionState3
        /// </summary>
        private int _conditionstate3;
        [ProtoMember(13)]
        [EntityField]
        public int ConditionState3
        {
            get { return _conditionstate3; }
            set { _conditionstate3 = value; }
        }
        /// <summary>  /// ConditionState4
        /// </summary>
        private int _conditionstate4;
        [ProtoMember(14)]
        [EntityField]
        public int ConditionState4
        {
            get { return _conditionstate4; }
            set { _conditionstate4 = value; }
        }
        /// <summary>  /// ConditionState5
        /// </summary>
        private int _conditionstate5;
        [ProtoMember(15)]
        [EntityField]
        public int ConditionState5
        {
            get { return _conditionstate5; }
            set { _conditionstate5 = value; }
        }
        /// <summary>  /// ConditionState6
        /// </summary>
        private int _conditionstate6;
        [ProtoMember(16)]
        [EntityField]
        public int ConditionState6
        {
            get { return _conditionstate6; }
            set { _conditionstate6 = value; }
        }
        /// <summary>  /// ConditionState7
        /// </summary>
        private int _conditionstate7;
        [ProtoMember(17)]
        [EntityField]
        public int ConditionState7
        {
            get { return _conditionstate7; }
            set { _conditionstate7 = value; }
        }
        /// <summary>  /// ConditionState8
        /// </summary>
        private int _conditionstate8;
        [ProtoMember(18)]
        [EntityField]
        public int ConditionState8
        {
            get { return _conditionstate8; }
            set { _conditionstate8 = value; }
        }
        /// <summary>  /// ConditionState9
        /// </summary>
        private int _conditionstate9;
        [ProtoMember(19)]
        [EntityField]
        public int ConditionState9
        {
            get { return _conditionstate9; }
            set { _conditionstate9 = value; }
        }
        /// <summary>  /// ConditionState10
        /// </summary>
        private int _conditionstate10;
        [ProtoMember(20)]
        [EntityField]
        public int ConditionState10
        {
            get { return _conditionstate10; }
            set { _conditionstate10 = value; }
        }

    }
}
