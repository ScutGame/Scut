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
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1902_魔法阵设置接口
    /// </summary>
    public class Action1902 : BaseAction
    {
        private string generalID = string.Empty;
        private string location = string.Empty;
        private int magicID = 0;
        private int ops =0;
        private List<UserEmbattle> _userEmbattlePos = new List<UserEmbattle>();

        public Action1902(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1902, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MagicID", ref magicID)
                 && httpGet.GetString("GeneralID", ref generalID)
                 && httpGet.GetString("Location", ref location)
                 && httpGet.GetInt("Ops", ref ops))
            {
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {

            if (ContextUser.UserStatus == UserStatus.CountryCombat)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1902_CountryCombatNotUpEmbattle;
                return false;
            }

            _userEmbattlePos = new GameDataCacheSet<UserEmbattle>().FindAll(ContextUser.UserID, m => m.MagicID == magicID);
            Dictionary<int, short> generalPos = GetEmbattle(generalID.Split(','), location.Split(','));
            short rePostion = GeneralHelper.ReplacePostion(ContextUser.UserID, magicID);
            foreach (KeyValuePair<int, short> keyValuePair in generalPos)
            {
                if (keyValuePair.Value == rePostion && keyValuePair.Key == LanguageManager.GetLang().GameUserGeneralID)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St1902_PostionNotGeneral;
                    return false;
                }
            }

            bool isModify = false;

            foreach (UserEmbattle embattle in _userEmbattlePos)
            {
                int generalId;
                if (TryFindGeneralId(generalPos, embattle.Position, out generalId))
                {

                    // 如果是左右交换
                    if (ops == 0)
                    {
                        foreach (KeyValuePair<int, short> keyValuePair in generalPos)
                        {
                            if (keyValuePair.Value == embattle.Position)
                            {
                                embattle.GeneralID = keyValuePair.Key;
                            }
                        }
                    }
                    else if (ops == 1)
                    {
                        // 佣兵上阵或移除时，对 IsBattle 和 Position 进行更改
                        var userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, embattle.GeneralID);
                        if (userGeneral != null)
                        {
                            if (generalId != embattle.GeneralID)
                            {
                                userGeneral.IsBattle = false;
                                userGeneral.Position = 0;
                            }

                        }
                        userGeneral = new GameDataCacheSet<UserGeneral>().FindKey(ContextUser.UserID, generalId);
                        if (userGeneral != null)
                        {
                            userGeneral.IsBattle = true;
                            userGeneral.Position = embattle.Position;
                        }
                        embattle.GeneralID = generalId;
                    }
                }
                else
                {
                    embattle.GeneralID = 0;
                }
                //embattle.Update();
                isModify = true;
            }
            //var embattlesArray = new GameDataCacheSet<UserEmbattle>().FindAll(ContextUser.UserID, m => m.GeneralID == LanguageManager.GetLang().GameUserGeneralID && m.MagicID == magicID);
            //if (embattlesArray.Count == 0)
            //{
            //    ErrorCode = LanguageManager.GetLang().ErrorCode;
            //    ErrorInfo = LanguageManager.GetLang().St1902_UserGeneralUnable;
            //    return false;
            //}
            if (UserHelper.IsUserEmbattle(ContextUser.UserID, magicID))
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St4004_EmbattleEmpty;
                return false;
            }

            if (isModify)
            {
                UserHelper.GetGameUserCombat(Uid);
            }
            return true;
        }

        private bool TryFindGeneralId(Dictionary<int, short> generalPos, short pos, out int generalID)
        {
            generalID = 0;
            foreach (KeyValuePair<int, short> keyValuePair in generalPos)
            {
                if (keyValuePair.Value == pos)
                {
                    generalID = keyValuePair.Key > 0 ? keyValuePair.Key : 0;
                    return true;
                }
            }
            return false;
        }

        private Dictionary<int, short> GetEmbattle(string[] generals, string[] locations)
        {
            var generalPos = new Dictionary<int, short>();

            for (int i = 0; i < generals.Length; i++)
            {
                int generalId = generals[i].ToInt();
                short pos = locations.Length > i ? locations[i].ToShort() : (short)0;

                if (!generalPos.ContainsKey(generalId))
                {
                    generalPos.Add(generalId, pos);
                }
            }
            return generalPos;
        }
    }
}