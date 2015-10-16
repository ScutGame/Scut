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
using ProtoBuf;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Exam
{
    /// <summary>
    /// 答题中间件
    /// </summary>
    [Serializable, ProtoContract]
    public abstract class ExamQuestion<T> : AbstractExam<T> where T : QuestionData, new()
    {
		/// <summary>
		/// The cache set.
		/// </summary>
        protected ConfigCacheSet<T> CacheSet;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        protected ExamQuestion(int userId)
            : base(userId)
        {
            CacheSet = new ConfigCacheSet<T>();
        }

        /// <summary>
        /// 选取题目
        /// </summary>
        /// <param name="count">取的数量</param>
        /// <param name="ignore">忽略的</param>
        /// <returns></returns>
        public override List<T> Extract(int count, Predicate<T> ignore)
        {
            var list = CacheSet.FindAll(m => !ignore(m));
            var tempList = RandomUtils.GetRandomArray(list.ToArray(), count);
            return new List<T>(tempList);
        }

        /// <summary>
        /// 是否答题正确
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        protected override bool CheckAnswer(int questionId, string answer)
        {
            var question = CacheSet.FindKey(questionId);
            return question != null && question.Answer == answer;
        }
        
    }
}