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
using System.Data;
using System.Data.SqlClient;

namespace ZyGames.Tianjiexing.BLL.WebService
{
    public abstract class BaseDataProcesser
    {
        public abstract void Process(JsonParameter[] paramList);


        /// <summary>
        /// 写库方法
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        protected void SendSqlCmd(int identityID, string connection, CommandType type, string sql, SqlParameter[] para)
        {
            //todo not imp
            //ctionMSMQ.Instance().SendSqlCmd(identityID, connection, type, sql, para);
        }
    }
}