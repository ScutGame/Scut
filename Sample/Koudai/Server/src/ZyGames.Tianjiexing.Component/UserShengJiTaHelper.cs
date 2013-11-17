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
using System.Linq;
using System.Web;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Com.Rank;
using ZyGames.Tianjiexing.Component;
using ZyGames.Tianjiexing.Component.Chat;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class UserShengJiTaHelper
    {
        public static void UserShengJiTaRank()
        {
            Ranking<UserRank> rankList = RankingFactory.Get<UserRank>(ShengJiTaRanking.RankingKey);
            if (rankList != null)
            {

                if (DateTime.Now.Hour == 0)
                {
                    foreach (UserRank userRank in rankList)
                    {

                        GameUser usergame = new GameDataCacheSet<GameUser>().FindKey(userRank.UserID);
                        if (usergame != null && userRank.ScoreStar>0)
                        {
                            // 信件通知玩家 “您在XX-XX的“勇闯圣吉塔”活动中名列XX榜第N，排名奖励M金币已经发送到您的账号中，请及时查收！”
                            TjxMailService mailService = new TjxMailService(usergame);
                            var mail = new UserMail(Guid.NewGuid());
                            mail.UserId = userRank.UserID.ToInt();
                            mail.MailType = (int)MailType.System;
                            mail.ToUserID = userRank.UserID.ToInt();
                            mail.FromUserName = LanguageManager.GetLang().St_SystemMailTitle;
                            mail.Title = LanguageManager.GetLang().St_SystemMailTitle;
                            mail.SendDate = DateTime.Now;

                            IGameLanguage gameLanguage = LanguageManager.GetLang();
                            SJTRankRewarInfo SJTRankRewar = new ShareCacheStruct<SJTRankRewarInfo>().FindKey(userRank.SJTRankId, userRank.SJTRankType.ToInt());
                            if (SJTRankRewar != null )
                            {
                                if (usergame.UserLv >= 10 && usergame.UserLv < 30)
                                {
                                    usergame.GameCoin = usergame.GameCoin + SJTRankRewar.GiftGold;
                                    mail.Content = String.Format(gameLanguage.St_ShengJiTaTip, DateTime.Now.ToString("MM-dd"), gameLanguage.St_ShengJiTaQintTong, userRank.SJTRankId, SJTRankRewar.GiftGold);

                                }
                                if (usergame.UserLv >= 30 && usergame.UserLv < 54)
                                {
                                    usergame.GameCoin = usergame.GameCoin + SJTRankRewar.GiftGold;
                                    mail.Content = String.Format(gameLanguage.St_ShengJiTaTip, DateTime.Now.ToString("MM-dd"), gameLanguage.St_ShengJiTaBaiYin, userRank.SJTRankId, SJTRankRewar.GiftGold);

                                }
                                if (usergame.UserLv >= 55)
                                {
                                    usergame.GameCoin = usergame.GameCoin + SJTRankRewar.GiftGold;
                                    mail.Content = String.Format(gameLanguage.St_ShengJiTaTip, DateTime.Now.ToString("MM-dd"), gameLanguage.St_ShengJiTaHuangJin, userRank.SJTRankId, SJTRankRewar.GiftGold);

                                }

                                mailService.Send(mail);
                            }

                        }
                    }
                }
            }
        }
    }
}