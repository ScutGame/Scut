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
using System.Collections;
using ZyGames.Framework.Game.SocketServer;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class AppStoreHelper : IComparable<AppStoreHelper>
    {
       
        public static AppStoreHelper GetSilverPiece(string product_id, int GameID)
        {
            if (GameID == 1)
            {
                AppStoreHelper appStoreHelper = (AppStoreHelper)AppstoreClientManager.Current.ListSelf[product_id];
                return appStoreHelper;
            }
            else
            {
                if (AppstoreClientManager.Current.ListAppstore.ContainsKey(product_id))
                {
                    AppStoreHelper appStoreHelper = (AppStoreHelper)AppstoreClientManager.Current.ListAppstore[product_id];
                    return appStoreHelper;
                }
            }


            return null;
        }

        /// <summary>
        /// 商品的标识
        /// </summary>
        public string product_id
        {
            get;
            set;
        }

        /// <summary>
        /// 对应元宝
        /// </summary>
        public int SilverPiece
        {
            get;
            set;
        }
        /// <summary>
        /// 对应美元
        /// </summary>
        public decimal Dollar
        {
            get;
            set;
        }

        public int RMB
        {
            get;
            set;
        }

        #region IComparable<AppStoreHelper> 成员

        public int CompareTo(AppStoreHelper other)
        {
            return this.SilverPiece.CompareTo(other.SilverPiece);
        }

        #endregion
    }
}