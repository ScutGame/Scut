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
using System.Collections.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;
using System.Collections;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Game.SocketServer;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// AppStore充值详情
    /// </summary>
    public class Action1069 : BaseStruct
    {
        private List<AppStoreHelper> appList = new List<AppStoreHelper>();
        private MobileType _mobileType;
        private int gameID = 0;

        public Action1069(ZyGames.Framework.Game.Contract.HttpGet httpGet)
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