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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Command;
using ZyGames.Framework.Game.Runtime;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Cache;
using ZyGames.Tianjiexing.BLL.Base;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{
    /// <summary>
    /// 1000_GM命令接口
    /// </summary>
    public class Action1000 : BaseAction
    {
        private string Cmd;
        private string Pid;
        private int _gold = 0;
        public Action1000(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1000, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetString("Cmd", ref Cmd))
            {
                httpGet.GetString("pid", ref Pid);
                return true;
            }
            return false;
        }

        public override bool TakeAction()
        {

            try
            {
                if (Cmd.Trim() != "")
                {
                    if ("cache".StartsWith(Cmd.ToLower()))
                    {
                        CacheFactory.UpdateNotify(true);
                    }
                    else if ("wxsign".StartsWith(Cmd.ToLower()))
                    {
                        return DoWeixinSign();
                    }
                    else
                    {
                        BaseCommand.Run(ContextUser.UserID, Cmd);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.SaveLog(ex);
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = ex.Message;
                return false;
            }
            
        }

        private bool DoWeixinSign()
        {
            GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(Uid);

            if (gameUser != null)
            {
                var registrationRewardArray = ConfigEnvSet.GetString("User.RegistrationReward").Split(',');
                int registrationNum = ConfigEnvSet.GetInt("User.RegistrationNum");
                var cacheSetRegistration = new GameDataCacheSet<UserRegistration>();
                var userRegistration = cacheSetRegistration.FindKey(gameUser.UserID);
                if (userRegistration == null)
                {
                    userRegistration = new UserRegistration(gameUser.UserID.ToInt());
                    userRegistration.RegistrationNum = 1;
                    cacheSetRegistration.Add(userRegistration);
                }
                if (registrationRewardArray.Length > 0)
                {
                    _gold = registrationRewardArray[0].ToInt();
                }

                if (userRegistration.RegistrationDate.Year == DateTime.Now.Year && userRegistration.RegistrationDate.Month == DateTime.Now.Month && userRegistration.RegistrationNum >= registrationNum)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = string.Format(LanguageManager.GetLang().St1000_RegistrationNum, registrationNum);
                    return false;
                }
                if (userRegistration.RegistrationDate.Date == DateTime.Now.Date)
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St1000_IsRegistration;
                    return false;
                }

                if (userRegistration.RegistrationDate.Month != DateTime.Now.Month)
                {
                    userRegistration.RegistrationNum = 1;
                    userRegistration.RegistrationDate = DateTime.Now;
                }
                else
                {
                    userRegistration.RegistrationNum = MathUtils.Addition(userRegistration.RegistrationNum, 1);
                    userRegistration.RegistrationDate = DateTime.Now;
                }

                int index = userRegistration.RegistrationNum - 1;
                if (registrationRewardArray.Length > index)
                {
                    _gold = registrationRewardArray[index].ToInt();
                }
                gameUser.GiftGold = MathUtils.Addition(gameUser.GiftGold, _gold);

                this.ErrorInfo = string.Format(LanguageManager.GetLang().St1000_GetRegistrationGold, _gold);
                return true;

            }
            this.ErrorCode = LanguageManager.GetLang().ErrorCode;
            this.ErrorInfo = LanguageManager.GetLang().St1000_UserExistent;
            return false;
        }
    }
}