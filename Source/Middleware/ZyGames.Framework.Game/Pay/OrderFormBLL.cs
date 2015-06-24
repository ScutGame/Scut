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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ZyGames.Framework.Common;
using ZyGames.Framework.Data;
using ZyGames.Framework.Data.Sql;

namespace ZyGames.Framework.Game.Pay
{
    /// <summary>
    /// Order form BL.
    /// </summary>
    public class OrderFormBLL
    {
        /// <summary>
        /// Add the specified model.
        /// </summary>
        /// <param name="model">Model.</param>
        public bool Add(OrderInfo model)
        {
            model.CreateDate = MathUtils.Now;
            var command = ConfigManger.Provider.CreateCommandStruct("OrderInfo", CommandMode.Insert);
            command.AddParameter("OrderNO", model.OrderNO);
            command.AddParameter("MerchandiseName", model.MerchandiseName);
            command.AddParameter("PayType", model.PayType);
            command.AddParameter("Amount", model.Amount);
            command.AddParameter("Currency", model.Currency);
            command.AddParameter("Expand", model.Expand);
            command.AddParameter("SerialNumber", model.SerialNumber);
            command.AddParameter("PassportID", model.PassportID);
            command.AddParameter("ServerID", model.ServerID);
            command.AddParameter("GameID", model.GameID);
            command.AddParameter("gameName", model.GameName);
            command.AddParameter("ServerName", model.ServerName);
            command.AddParameter("PayStatus", model.PayStatus);
            command.AddParameter("Signature", model.Signature);
            command.AddParameter("Remarks", model.Remarks);
            command.AddParameter("GameCoins", model.GameCoins);
            command.AddParameter("SendState", model.SendState);
            command.AddParameter("RetailID", model.RetailID);
            command.AddParameter("DeviceID", model.DeviceID == null ? string.Empty : model.DeviceID);
            if (model.SendDate > DateTime.MinValue)
            {
                command.AddParameter("SendDate", model.SendDate);
            }
            command.AddParameter("CreateDate", model.CreateDate);
            command.Parser();

            return ConfigManger.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(OrderInfo model)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("OrderInfo", CommandMode.Modify);
            command.AddParameter("SerialNumber", model.SerialNumber);
            command.AddParameter("PayStatus", model.PayStatus);
            command.AddParameter("@Signature", model.Signature);
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("OrderNO");
            command.Filter.AddParam("OrderNO", model.OrderNO);
            command.Parser();

            return ConfigManger.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        /// <summary>
        /// 获取游戏币.
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="gameID">Game.</param>
        /// <param name="serverID">Server.</param>
        /// <param name="pID">Account.</param>
        public OrderInfo[] GetList(int gameID, int serverID, string pID)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("OrderInfo", CommandMode.Inquiry);
            command.OrderBy = "ID ASC";
            command.Columns = "OrderNO,MerchandiseName,PayType,Amount,Currency,Expand,SerialNumber,PassportID,ServerID,GameID,gameName,ServerName,PayStatus,Signature,Remarks,GameCoins,SendState,CreateDate,SendDate,RetailID,DeviceID";
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = string.Format("{0} AND {1} AND {2} AND {3} AND {4}",
                command.Filter.FormatExpression("GameID"),
                command.Filter.FormatExpression("ServerID"),
                command.Filter.FormatExpression("PassportID"),
                command.Filter.FormatExpression("SendState"),
                command.Filter.FormatExpression("PayStatus")
                );
            command.Filter.AddParam("GameID", gameID);
            command.Filter.AddParam("ServerID", serverID);
            command.Filter.AddParam("PassportID", pID);
            command.Filter.AddParam("SendState", 1);
            command.Filter.AddParam("PayStatus", 2);
            command.Parser();

            using (var reader = ConfigManger.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                List<OrderInfo> olist = new List<OrderInfo>();
                while (reader.Read())
                {
                    OrderInfo ordermode = SetOrderInfo(reader);
                    olist.Add(ordermode);
                }
                return olist.ToArray();
            }
        }

        /// <summary>
        /// Determines whether this instance is exists the specified OrderNo.
        /// </summary>
        /// <returns><c>true</c> if this instance is exists the specified OrderNo; otherwise, <c>false</c>.</returns>
        /// <param name="orderNo">Order no.</param>
        public bool IsExists(string orderNo)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("OrderInfo", CommandMode.Inquiry, "OrderNO");
            command.OrderBy = "ID ASC";
            command.Top = 1;
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("OrderNO");
            command.Filter.AddParam("OrderNO", orderNo);
            command.Parser();
            return ConfigManger.Provider.ExecuteScalar(CommandType.Text, command.Sql, command.Parameters) != null;
        }

        /// <summary>
        /// Updates the by91.
        /// </summary>
        /// <returns><c>true</c>, if by91 was updated, <c>false</c> otherwise.</returns>
        /// <param name="model">Model.</param>
        /// <param name="callback">If set to <c>true</c> callback.</param>
        public bool UpdateBy91(OrderInfo model, bool callback)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("OrderInfo", CommandMode.Modify);
            if (callback)
            {
                command.AddParameter("MerchandiseName", model.MerchandiseName);
                command.AddParameter("PayType", model.PayType);
                command.AddParameter("Amount", model.Amount);
                command.AddParameter("SendState", model.SendState);
                command.AddParameter("PayStatus", model.PayStatus);
                command.AddParameter("GameCoins", model.GameCoins);
                command.AddParameter("Signature", model.Signature);
            }
            else
            {
                command.AddParameter("ServerID", model.ServerID);
                command.AddParameter("PassportID", model.PassportID);
                if (!string.IsNullOrEmpty(model.Expand))
                    command.AddParameter("Expand", model.Expand);
                command.AddParameter("GameID", model.GameID);
                command.AddParameter("RetailID", model.RetailID);//20

                if (!string.IsNullOrEmpty(model.ServerName))
                {
                    command.AddParameter("ServerName", model.ServerName);
                }
                if (!string.IsNullOrEmpty(model.GameName))
                {
                    command.AddParameter("gameName", model.GameName);
                }
            }
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("OrderNO");
            command.Filter.AddParam("OrderNO", model.OrderNO);
            command.Parser();
            return ConfigManger.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;

        }

        /// <summary>
        /// Add91s the pay.
        /// </summary>
        /// <returns><c>true</c>, if pay was add91ed, <c>false</c> otherwise.</returns>
        /// <param name="order">Order.</param>
        /// <param name="callback">If set to <c>true</c> callback.</param>
        public bool Add91Pay(OrderInfo order, bool callback)
        {
            if (!IsExists(order.OrderNO))
            {
                return Add(order);
            }
            else
            {
                return UpdateBy91(order, callback);
            }
        }


        private OrderInfo SetOrderInfo(IDataReader reader)
        {
            OrderInfo ordermode = new OrderInfo();
            ordermode.OrderNO = reader["OrderNO"].ToNotNullString();
            ordermode.MerchandiseName = reader["MerchandiseName"].ToNotNullString();
            ordermode.PayType = reader["PayType"].ToNotNullString();
            ordermode.Amount = reader["Amount"].ToDecimal();
            ordermode.Currency = reader["Currency"].ToNotNullString();
            ordermode.Expand = reader["Expand"].ToNotNullString();
            ordermode.SerialNumber = reader["SerialNumber"].ToNotNullString();
            ordermode.PassportID = reader["PassportID"].ToNotNullString();
            ordermode.ServerID = reader["ServerID"].ToInt();
            ordermode.GameID = reader["GameID"].ToInt();
            ordermode.GameName = reader["gameName"].ToNotNullString();
            ordermode.ServerName = reader["ServerName"].ToNotNullString();
            ordermode.PayStatus = reader["PayStatus"].ToInt();
            ordermode.Signature = reader["Signature"].ToNotNullString();
            ordermode.Remarks = reader["Remarks"].ToNotNullString();
            ordermode.GameCoins = reader["GameCoins"].ToInt();
            ordermode.SendState = reader["SendState"].ToInt();
            ordermode.CreateDate = reader["CreateDate"].ToDateTime();
            ordermode.SendDate = reader["SendDate"].ToDateTime();
            ordermode.RetailID = reader["RetailID"].ToNotNullString();
            ordermode.DeviceID = reader["DeviceID"].ToNotNullString();
            return ordermode;
        }


        /// <summary>
        /// Updatestr the specified OrderNo.
        /// </summary>
        /// <param name="OrderNo">Order no.</param>
        public bool Updatestr(string OrderNo)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("OrderInfo", CommandMode.Modify);
            command.AddParameter("SendState", 2);
            command.AddParameter("SendDate", DateTime.Now);
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("OrderNO");
            command.Filter.AddParam("OrderNO", OrderNo);
            command.Parser();

            return ConfigManger.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }

        /// <summary>
        /// 支付取游戏名改为直接查库-wuzf
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public ServerInfo GetServerData(int gameID, int serverID)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("ServerInfo", CommandMode.Inquiry);
            command.Columns = "ID,GameID,(SELECT GameName FROM GameInfo WHERE GameInfo.GameID=ServerInfo.GameID) GameName,ServerName";
            command.OrderBy = "GameID ASC,ID ASC";
            command.Top = 1;
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = string.Format("{0} AND {1}",
                    command.Filter.FormatExpression("GameID"),
                    command.Filter.FormatExpression("ID")
                );
            command.Filter.AddParam("GameID", gameID);
            command.Filter.AddParam("ID", serverID);
            command.Parser();

            using (var reader = ConfigManger.Provider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                var serverInfo = new ServerInfo
                {
                    GameID = gameID,
                    ID = serverID,
                    Name = string.Empty,
                    GameName = string.Empty
                };
                if (reader.Read())
                {
                    serverInfo.GameName = reader["GameName"].ToNotNullString();
                    serverInfo.Name = reader["ServerName"].ToNotNullString();
                }
                return serverInfo;
            }
        }

        internal bool PaySuccess(string orderNo, OrderInfo orderInfo)
        {
            var command = ConfigManger.Provider.CreateCommandStruct("OrderInfo", CommandMode.Modify);
            orderInfo.PayStatus = 2;
            command.AddParameter("PayStatus", orderInfo.PayStatus);
            if (!string.IsNullOrEmpty(orderInfo.PayType))
            {
                command.AddParameter("PayType", orderInfo.PayType);
            }
            if (orderInfo.Amount > 0 && orderInfo.GameCoins > 0)
            {
                command.AddParameter("Amount", orderInfo.Amount);
                command.AddParameter("GameCoins", orderInfo.GameCoins);
            }
            command.Filter = ConfigManger.Provider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("OrderNO");
            command.Filter.AddParam("OrderNO", orderNo);
            command.Parser();

            return ConfigManger.Provider.ExecuteQuery(CommandType.Text, command.Sql, command.Parameters) > 0;
        }
    }
}