using System;
using System.Collections;

namespace ZyGames.Doudizhu.Bll.Base
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
