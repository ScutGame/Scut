using System;
using System.Collections.Generic;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Framework.Game.Com.Exam
{
    /// <summary>
    /// 答题中间件基类
    /// </summary>
    public abstract class AbstractExam<T> where T : QuestionData, new()
    {
        protected readonly int UserId;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        protected AbstractExam(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// 选取题目
        /// </summary>
        /// <param name="count">取的数量</param>
        /// <param name="ignore">忽略的</param>
        /// <returns></returns>
        public abstract List<T> Extract(int count, Predicate<T> ignore);


        ///<summary>
        /// 答题
        ///</summary>
        ///<param name="questionId">题目编号</param>
        ///<param name="answer">答题</param>
        public void DoAnswer(int questionId, string answer)
        {
            bool answerResult = CheckAnswer(questionId, answer);
            ProcessResult(questionId, answerResult);
        }

        /// <summary>
        /// 是否答题正确
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        protected abstract bool CheckAnswer(int questionId, string answer);

        /// <summary>
        /// 处理结果
        /// </summary>
        /// <param name="questionId">题目编号</param>
        /// <param name="answerResult">答题结果</param>
        /// <returns>返回处理结果(奖励)</returns>
        protected abstract object ProcessResult(int questionId, bool answerResult);

        /// <summary>
        /// 获取奖励
        /// </summary>
        /// <returns></returns>
        public abstract object GetPrize();
    }
}
