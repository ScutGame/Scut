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
using System.Text;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Net;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;

namespace ZyGames.Tianjiexing.BLL.GM
{
    public class EnchantCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int enchantID = args.Length > 0 ? args[0].Trim().ToInt() : 0;
            AddEnchant(UserID, enchantID);
        }

        private void AddEnchant(string userID, int enchantID)
        {
            if (enchantID > 0)
            {
                var package = UserEnchant.Get(userID);
                if (package != null)
                {
                    package.SaveEnchant(GetUserEnchantInfo(enchantID));
                }
            }
            else
            {
                throw new Exception("无此附魔符");
            }
        }

        /// <summary>
        /// 获取附魔符
        /// </summary>
        /// <param name="enchantID"></param>
        /// <returns></returns>
        public static UserEnchantInfo GetUserEnchantInfo(int enchantID)
        {
            UserEnchantInfo enchant = new UserEnchantInfo();
            EnchantInfo enchantInfo = new ConfigCacheSet<EnchantInfo>().FindKey(enchantID);
            if (enchantInfo != null)
            {
                enchant.UserEnchantID = Guid.NewGuid().ToString();
                enchant.EnchantID = enchantID;
                enchant.EnchantLv = 1;
                enchant.CurrExprience = 0;
                enchant.Position = 0;
                enchant.UserItemID = string.Empty;
                enchant.MaxMature = (short)RandomUtils.GetRandom(enchantInfo.BeseNum, enchantInfo.MaxNum);
            }
            return enchant;
        }
    }
}