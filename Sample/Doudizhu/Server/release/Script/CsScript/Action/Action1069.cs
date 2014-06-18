using System.Collections;
using System.Collections.Generic;
using ZyGames.Doudizhu.Bll;
using ZyGames.Doudizhu.Bll.Base;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Service;

namespace ZyGames.Doudizhu.Script.CsScript.Action
{
    /// <summary>
    /// AppStore充值详情
    /// </summary>
    public class Action1069 : BaseStruct
    {
        private List<AppStoreHelper> appList = new List<AppStoreHelper>();
        private MobileType _mobileType;
        private int gameID = 0;

        public Action1069(HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1069, httpGet)
        {
        }

        public override bool TakeAction()
        {
            Hashtable ht = new Hashtable();
            //if (gameID > 0)
            //{
            //    ht = AppstoreClientManager.Current.ListSelf;
            //}
            //else
            //{

            switch (_mobileType)
            {
                case 0:
                    ht = AppstoreClientManager.Current.ListIphone;
                    break;
                case MobileType.iPod:
                case MobileType.iPad:
                    ht = AppstoreClientManager.Current.ListIpad;
                    break;
                case MobileType.iPhone:
                case MobileType.Phone_AppStore:
                    ht = AppstoreClientManager.Current.ListIphone;
                    break;
                default:
                    ht = AppstoreClientManager.Current.ListIphone;
                    break;
            }

            //}
            foreach (DictionaryEntry entry in ht)
            {
                appList.Add((AppStoreHelper)entry.Value);
            }
            appList.QuickSort((a, b) => a.CompareTo(b));
            return true;
        }

        public override void BuildPacket()
        {
            PushIntoStack(appList.Count);
            foreach (AppStoreHelper item in appList)
            {
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(item.Dollar.ToString());
                ds.PushIntoStack(item.product_id);
                ds.PushIntoStack(item.SilverPiece);
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            httpGet.GetEnum<MobileType>("MobileType", ref _mobileType);
            httpGet.GetInt("GameID", ref gameID);
            return true;
        }

    }

}
