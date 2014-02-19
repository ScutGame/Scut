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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Collection;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.BLL.Combat;
using ZyGames.Tianjiexing.Model;
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1901_魔法阵列表接口
    /// </summary>
    public class Action1901 : BaseAction
    {
        private List<UserGeneral> _userGeneralArray = new List<UserGeneral>();
        private List<UserMagic> _userMagicArray = new List<UserMagic>();//当前玩家所开启的魔法阵

        public Action1901(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1901, httpGet)
        {

        }

        public override void BuildPacket()
        {
            PushIntoStack(_userMagicArray.Count);
            foreach (UserMagic userMagic in _userMagicArray)
            {
                MagicInfo magicInfo = new ConfigCacheSet<MagicInfo>().FindKey(userMagic.MagicID);
                short repostion = GeneralHelper.ReplacePostion(ContextUser.UserID, userMagic.MagicID);
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(userMagic.MagicID);
                ds.PushIntoStack(magicInfo == null ? string.Empty : magicInfo.MagicName.ToNotNullString());
                ds.PushIntoStack((short)userMagic.MagicLv);
                ds.PushIntoStack(userMagic.IsEnabled == false ? 0 : 1);
                ds.PushIntoStack(magicInfo == null ? string.Empty : magicInfo.MagicDesc.ToNotNullString());
                string gridPostion = string.Empty;
                string[] gridRanges = new string[0];
                MagicLvInfo magicLv = new ConfigCacheSet<MagicLvInfo>().FindKey(userMagic.MagicID, userMagic.MagicLv);
                if (magicLv != null)
                {
                    var userFunction = new GameDataCacheSet<UserFunction>().FindKey(ContextUser.UserID, FunctionEnum.ReplaceGeneral);
                    gridPostion = magicLv.GridRange;
                    if (magicLv.ReplacePostion > 0 && userFunction != null)
                    {
                        gridPostion = gridPostion.TrimEnd(',') + "," + magicLv.ReplacePostion;
                    }
                }
                gridRanges = gridPostion.Split(',');
                ds.PushIntoStack(gridRanges.Length);

                foreach (string gridRang in gridRanges)
                {
                    int isDisplace = repostion == gridRang.ToInt() ? 1 : 0;
                    UserEmbattle userEmbattle = new GameDataCacheSet<UserEmbattle>().FindKey(ContextUser.UserID, userMagic.MagicID, gridRang.ToShort());
                    UserGeneral uGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, userEmbattle == null ? 0 : userEmbattle.GeneralID);

                    DataStruct ds1 = new DataStruct();
                    ds1.PushIntoStack(uGeneral == null ? (short)0 : (short)1);
                    ds1.PushIntoStack(gridRang.ToShort());
                    ds1.PushIntoStack(uGeneral == null ? 0 : uGeneral.GeneralID);
                    ds1.PushIntoStack(uGeneral == null ? string.Empty : uGeneral.HeadID.ToNotNullString());
                    ds1.PushIntoStack((short)isDisplace);
                    ds1.PushShortIntoStack(uGeneral == null ? (short)0 : MathUtils.ToShort(uGeneral.GeneralQuality));
                    ds.PushIntoStack(ds1);
                }
                PushIntoStack(ds);
            }
            // 获取佣兵品质
            var generalInfoCacheSet = new ShareCacheStruct<GeneralInfo>();
            PushIntoStack(_userGeneralArray.Count);
            foreach (UserGeneral general in _userGeneralArray)
            {
                GeneralInfo generalInfo = generalInfoCacheSet.FindKey(general.GeneralID);
                DataStruct ds = new DataStruct();
                ds.PushIntoStack(general.GeneralID);
                ds.PushIntoStack(general.HeadID.ToNotNullString());
                ds.PushShortIntoStack(generalInfo == null ? (short)0 : MathUtils.ToShort(generalInfo.GeneralQuality));
                PushIntoStack(ds);
            }
        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {

            GeneralHelper.StotyTaskFunction(ContextUser); //已完成替换佣兵功能开启
            _userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(ContextUser.UserID, u => u.MagicID != 1 && u.MagicType == MagicType.MoFaZhen);
            _userMagicArray.QuickSort((x, y) =>
            {
                if (x == null && y == null) return 0;
                if (x != null && y == null) return 1;
                if (x == null) return -1;
                return x.MagicID.CompareTo(y.MagicID);
            });

            var userFunction = new GameDataCacheSet<UserFunction>().FindKey(ContextUser.UserID, FunctionEnum.ReplaceGeneral);
            foreach (UserMagic magic in _userMagicArray)
            {
                if (magic == null) continue;
                MagicLvInfo magicLv = new ConfigCacheSet<MagicLvInfo>().FindKey(magic.MagicID, magic.MagicLv);
                if (magicLv != null)
                {
                    string gridPostion = magicLv.GridRange;
                    if (magicLv.ReplacePostion > 0 && userFunction != null)
                    {
                        gridPostion = gridPostion.TrimEnd(',') + "," + magicLv.ReplacePostion;
                    }
                    string[] gridRangeArray = gridPostion.Split(',');
                    foreach (string gridRange in gridRangeArray)
                    {
                        UserEmbattle userEmbattle = new GameDataCacheSet<UserEmbattle>().FindKey(ContextUser.UserID, magic.MagicID, gridRange.ToShort());
                        if (userEmbattle == null)
                        {
                            UserEmbattle embattle = new UserEmbattle()
                            {
                                UserID = ContextUser.UserID,
                                MagicID = magic.MagicID,
                                Position = gridRange.ToShort(),
                                GeneralID = 0
                            };
                            var cacheSet = new GameDataCacheSet<UserEmbattle>();
                            cacheSet.Add(embattle);
                        }
                    }
                }
            }
            _userGeneralArray = new GameDataCacheSet<UserGeneral>().FindAll(ContextUser.UserID, u => u.GeneralStatus == GeneralStatus.DuiWuZhong && u.GeneralType != GeneralType.Soul);
            
            // 佣兵排序
            GeneralSortHelper.GeneralSort(ContextUser.UserID, _userGeneralArray);

            return true;
        }
    }
}