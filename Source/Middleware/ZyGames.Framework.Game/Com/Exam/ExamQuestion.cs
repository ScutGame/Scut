using System;
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Exam
{
    /// <summary>
    /// 答题中间件
    /// </summary>
    public abstract class ExamQuestion<T> : AbstractExam<T> where T : QuestionData, new()
    {
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
