using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.Sql;

namespace ZyGames.Framework.Game.Pay
{
    /// <summary>
    /// 
    /// </summary>
    public class PayOperator
    {
        internal PayOperator()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<FeedbackInfo> GetFeedBackList(string condition)
        {
            string sql = string.Format("SELECT * FROM (SELECT row_number()over(order by [GMId] desc) as RowNumber, [GMID],[UId],[GameID],[ServerID],[GMType],[content],[SubmittedTime],[RContent],[ReplyTime],[ReplyID],Pid,NickName FROM GMFeedBack {0} order by SubmittedTime desc", condition);
            return GetFeedBackList(sql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uID"></param>
        /// <returns></returns>
        public List<FeedbackInfo> GetFeedBackList(int uID)
        {
            string sql = string.Format("SELECT [GMID],[UId],[GameID],[ServerID],[GMType],[content],[SubmittedTime],[RContent],[ReplyTime],[ReplyID],Pid,NickName FROM GMFeedBack where UId=@UId order by SubmittedTime desc");

            SqlParameter[] parameters = new[]{
					SqlParamHelper.MakeInParam("@UId", SqlDbType.Int, 0, uID),
            };
            return GetFeedBackList(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private List<FeedbackInfo> GetFeedBackList(string sql, params SqlParameter[] parameters)
        {
            using (SqlDataReader reader = SqlHelper.ExecuteReader(ConfigManger.connectionString, CommandType.Text, sql, parameters))
            {
                List<FeedbackInfo> olist = new List<FeedbackInfo>();
                while (reader.Read())
                {
                    FeedbackInfo gMFeedBackInfo = new FeedbackInfo();
                    gMFeedBackInfo.Uid = MathUtils.ToInt(reader["UId"]);
                    gMFeedBackInfo.GameID = MathUtils.ToInt(reader["GameID"]);
                    gMFeedBackInfo.ServerID = MathUtils.ToInt(reader["ServerID"]);
                    gMFeedBackInfo.Pid = MathUtils.ToNotNullString(reader["Pid"]);
                    gMFeedBackInfo.NickName = MathUtils.ToNotNullString(reader["NickName"]);
                    gMFeedBackInfo.Content = MathUtils.ToNotNullString(reader["content"]);
                    gMFeedBackInfo.ID = MathUtils.ToInt(reader["GMID"]);
                    gMFeedBackInfo.Type = MathUtils.ToInt(reader["GMType"]);
                    gMFeedBackInfo.ReplyContent = MathUtils.ToNotNullString(reader["RContent"]);
                    gMFeedBackInfo.ReplyID = MathUtils.ToInt(reader["ReplyID"]);

                    gMFeedBackInfo.ReplyDate = MathUtils.ToDateTime(reader["ReplyTime"], DateTime.MinValue);
                    gMFeedBackInfo.CreateDate = MathUtils.ToDateTime(reader["SubmittedTime"], DateTime.MinValue);


                    olist.Add(gMFeedBackInfo);
                }
                return olist;
            }


            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public bool PostFeedBack(FeedbackInfo info)
        {
            CommandStruct command = new CommandStruct("GMFeedBack", CommandMode.Insert);
            command.AddParameter("UId", SqlDbType.Int, info.Uid);
            command.AddParameter("GameID", SqlDbType.Int, info.GameID);
            command.AddParameter("ServerID", SqlDbType.Int, info.ServerID);
            command.AddParameter("GMType", SqlDbType.Int, info.Type);
            command.AddParameter("content", SqlDbType.VarChar, info.Content);
            command.AddParameter("Pid", SqlDbType.VarChar, info.Pid);
            command.AddParameter("NickName", SqlDbType.VarChar, info.NickName);
            command.AddParameter("SubmittedTime", SqlDbType.DateTime, info.CreateDate);
            command.Parser();

            return SqlHelper.ExecuteNonQuery(ConfigManger.connectionString, CommandType.Text, command.Sql, command.SqlParameters) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <param name="replyContent"></param>
        /// <param name="replyId">回复者</param>
        /// <returns></returns>
        public bool ReplyToFeedBack(int feedbackId, string replyContent, int replyId)
        {
            CommandStruct command = new CommandStruct("GMFeedBack", CommandMode.Modify);
            command.AddParameter("RContent", SqlDbType.VarChar, replyContent);
            command.AddParameter("ReplyID", SqlDbType.VarChar, replyId);
            command.AddParameter("ReplyTime", SqlDbType.DateTime, DateTime.Now);
            command.Filter = new CommandFilter();
            command.Filter.Condition = "GMId=@GMId";
            command.Filter.AddParam("@GMId", SqlDbType.Int, 0, feedbackId);
            command.Parser();

            return SqlHelper.ExecuteNonQuery(ConfigManger.connectionString, CommandType.Text, command.Sql, command.SqlParameters) > 0;
        }
    }
}
