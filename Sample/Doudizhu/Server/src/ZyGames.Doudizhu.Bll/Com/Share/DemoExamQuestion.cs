using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Doudizhu.Model;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Exam;
using ZyGames.Framework.Game.Com.Model;

namespace ZyGames.Doudizhu.Bll.Com.Share
{

    internal class MyClass
    {
        private const int SystemMerId = 1000;
        static void InitMall(GameUser  user)
        {
            var examquest = new DemoExamQuestion(user.UserId);
            examquest.DoAnswer();
        }
    }

    class DemoExamQuestion : ExamQuestion<QuestionData>
    {
        private object _prizeObj;

        public DemoExamQuestion(int userId)
            : base(userId)
        {
        }

        protected override object ProcessResult(int questionId, bool answerResult)
        {
            if (answerResult)
            {
                _prizeObj = new ConfigCacheSet<QuestionInfo>().FindKey(questionId);
                //处理奖励
            }
            return _prizeObj;
        }

        public override object GetPrize()
        {
            return _prizeObj;
        }
    }
}
