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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;



namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1501_魔术列表接口
    /// </summary>
    public class Action1501 : BaseAction
    {
        private int pageIndex = 0;
        private int pageSize = 0;
        private MagicType magicType;
        private int pageCount = 0;
        private short isUp = 0;
        private int coldTime = 0;
        private string queueID = string.Empty;
        private List<UserMagic> userMagicArray = new List<UserMagic>();
        private MagicInfo magicInfo = null;

        public Action1501(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1501, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(pageCount);
            PushIntoStack(ContextUser.ExpNum);
            PushIntoStack(queueID);
            PushIntoStack(coldTime);
            PushIntoStack(userMagicArray.Count);
            foreach (UserMagic magic in userMagicArray)
            {
                magicInfo = new ConfigCacheSet<MagicInfo>().FindKey(magic.MagicID);
                if (IsUp(magic.MagicID, magic.MagicLv, ContextUser))
                {
                    isUp = 1;
                }
                else
                {
                    isUp = 0;
                }

                DataStruct ds = new DataStruct();
                ds.PushIntoStack(magic.MagicID);
                ds.PushIntoStack((int)magic.MagicType);
                ds.PushIntoStack(magicInfo == null ? string.Empty : magicInfo.MagicName.ToNotNullString());
                ds.PushIntoStack(magicInfo == null ? string.Empty : magicInfo.HeadID.ToNotNullString());
                ds.PushIntoStack((short)magic.MagicLv);
                ds.PushIntoStack(isUp);
                ds.PushIntoStack(magic.IsOpen ? 1 : 0);
                ds.PushIntoStack(magic.IsLv);
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("PageIndex", ref pageIndex)
                 && httpGet.GetInt("PageSize", ref pageSize)
                 && httpGet.GetEnum<MagicType>("MagicType", ref magicType))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            if (magicType == 0)
            {
                userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(ContextUser.UserID).GetPaging(pageIndex, pageSize, out pageCount);
            }
            else
            {
                userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(ContextUser.UserID, m => m.MagicType == magicType).GetPaging(pageIndex, pageSize, out pageCount);
                //List<UserMagic> tempList = new List<UserMagic>(userMagicArray);
                //userMagicArray = tempList.FindAll(m => m.MagicID != new GameUser().UserMagicID);
            }
            userMagicArray.ForEach(obj =>
            {
                obj.IsOpen = true;
            });
            if (magicType == MagicType.MoFaZhen)
            {
                var cacheSetMagic = new ConfigCacheSet<MagicInfo>();
                var magicList = cacheSetMagic.FindAll(s => s.MagicType == MagicType.MoFaZhen && s.DemandLv > ContextUser.UserLv);

                magicList.ForEach(mgic =>
                {
                    if (userMagicArray.Find(s => s.MagicID == mgic.MagicID) == null)
                    {
                        UserMagic userMagic = new UserMagic();
                        userMagic.IsOpen = mgic.DemandLv <= ContextUser.UserLv ? true : false;
                        userMagic.MagicID = mgic.MagicID;
                        userMagic.UserID = string.Empty;
                        userMagic.MagicLv = 0;
                        userMagic.MagicType = MagicType.MoFaZhen;
                        userMagic.IsLv = mgic.DemandLv;
                        userMagicArray.Add(userMagic);
                    }
                });
            }
            pageCount = MathUtils.Subtraction(pageCount, 1, 0);

            //List<UserQueue> userQueueArray = new GameDataCacheSet<UserQueue>().FindAll(ContextUser.UserID, m => m.QueueType == QueueType.MagicStrong);
            //if (userQueueArray.Count > 0 && userQueueArray[0].StrengNum >= 2)
            //{
            //    queueID = userQueueArray[0].QueueID;
            //    coldTime = userQueueArray[0].DoRefresh();
            //}
            userMagicArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.MagicID.CompareTo(y.MagicID);
            });
            ////记录操作日志
            //UserOperationLog userOperationLog = new UserOperationLog();
            //userOperationLog.UserID = ContextUser.UserID;
            //userOperationLog.ActionID = ActionIDDefine.Cst_Action1501;
            //userOperationLog.FunctionID = "魔术列表";
            //userOperationLog.CreateDate = DateTime.Now;
            //userOperationLog.Num = 1;
            //userOperationLog.Append();
            return true;
        }

        public static bool IsUp(int magicID, int magicLv, GameUser user)
        {
            bool result = true;
            int upLv = MathUtils.Addition(magicLv, 1, int.MaxValue);
            MagicLvInfo magicLvInfo = new ConfigCacheSet<MagicLvInfo>().FindKey(magicID, upLv);
            if (magicLvInfo == null || magicLvInfo.EscalateMinLv > user.UserLv || magicLvInfo.ExpNum > user.ExpNum)
            {
                result = false;
            }
            return result;
        }
    }
}