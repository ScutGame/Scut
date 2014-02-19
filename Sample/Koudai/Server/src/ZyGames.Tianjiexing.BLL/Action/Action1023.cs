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

using System.Data;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Com;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Lang;
using System.Text;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1023_玩家改名
    /// </summary>
    public class Action1023 : BaseAction
    {
        private int MaxLength = 0;
        private string _nickName = string.Empty;
        private int itemID = 0;

        public Action1023(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1023, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("NickName", ref _nickName))
            {
                _nickName = _nickName.Trim();
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {
            try
            {
                MaxLength = ConfigEnvSet.GetInt("User.MaxLength");
                itemID = ConfigEnvSet.GetInt("UserName.ItemID");
                int length = System.Text.Encoding.Default.GetByteCount(_nickName);
                if (length <= 0 || length > MaxLength)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = string.Format(LanguageManager.GetLang().St1005_KingNameTooLong, MaxLength);
                    return false;
                }
                if (GameUser.IsNickName(_nickName))
                {
                    ErrorCode = 1;
                    ErrorInfo = LanguageManager.GetLang().St1005_Rename;
                    return false;
                }
                //UserItemLog userItem = UserItemLog.FindKey(itemID);
                var package = UserItemPackage.Get(Uid);
                UserItemInfo userItem = package.ItemPackage.Find(m => !m.IsRemove && m.ItemID == itemID);
                if (userItem == null || userItem.Num <= 0)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    return false;
                }
                ContextUser.NickName = _nickName;

                int rankID = 0;
                UserRank rankInfo;
                Ranking<UserRank> ranking = RankingFactory.Get<UserRank>(CombatRanking.RankingKey);
                if (ranking.TryGetRankNo(m => m.UserID == ContextUser.UserID, out rankID))
                {
                    rankInfo = ranking.Find(s => s.UserID == ContextUser.UserID);
                    if (rankInfo != null)
                    {
                        rankInfo.NickName = _nickName;
                    }
                }
                //ContextUser.Update();
                //UserGeneral usergen = UserGeneral.GetMainGeneral(ContextUser.UserID);
                //if (usergen != null)
                //{
                //    usergen.GeneralName = _nickName;
                //    //usergen.Update();
                //}
                UserItemHelper.UseUserItem(ContextUser.UserID, itemID, 1);

                return true;
            }
            catch (Exception ex)
            {
                SaveLog(ex);
                return false;
            }
        }
    }
}