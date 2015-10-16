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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Com.Model
{
    /// <summary>
    /// 题库数据
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, "", "")]
    public class QuestionData : ShareEntity
    {
        ///<summary>
        ///</summary>
        public QuestionData()
            : base(AccessLevel.ReadOnly)
        {
        }

        #region auto-generated Property
        private Int32 _ID;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityFieldExtend]
        [EntityField(true)]
        public Int32 ID
        {
            get
            {
                return _ID;
            }
            private set
            {
                SetChange("ID", value);
            }
        }
        private String _Topic;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityFieldExtend]
        [EntityField]
        public String Topic
        {
            get
            {
                return _Topic;
            }
            private set
            {
                SetChange("Topic", value);
            }
        }
        private CacheList<QuestionOption> _Options;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityFieldExtend]
        [EntityField( true, ColumnDbType.Text)]
        public CacheList<QuestionOption> Options
        {
            get
            {
                return _Options;
            }
            private set
            {
                SetChange("Options", value);
            }
        }
        private string _Answer;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityFieldExtend]
        [EntityField]
        public string Answer
        {
            get
            {
                return _Answer;
            }
            private set
            {
                SetChange("Answer", value);
            }
        }
		/// <summary>
		/// 对象索引器属性
		/// </summary>
		/// <returns></returns>
		/// <param name="index">Index.</param>
        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "ID": return ID;
                    case "Topic": return Topic;
                    case "Options": return Options;
                    case "Answer": return Answer;
                    default: throw new ArgumentException(string.Format("QuestionData index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "ID":
                        _ID = value.ToInt();
                        break;
                    case "Topic":
                        _Topic = value.ToNotNullString();
                        break;
                    case "Options":
                        _Options = ConvertCustomField<CacheList<QuestionOption>>(value, index);
                        break;
                    case "Answer":
                        _Answer = value.ToNotNullString();
                        break;
                    default: throw new ArgumentException(string.Format("QuestionData index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion
    }
}