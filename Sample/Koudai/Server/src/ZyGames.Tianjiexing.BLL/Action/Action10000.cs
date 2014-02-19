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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;


namespace ZyGames.Tianjiexing.BLL.Action
{


    /// <summary>
    /// 用户登录
    /// </summary>
    public class Action10000 : BaseAction
    {
        public Action10000(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action10000, httpGet)
        {
        }



        public override bool GetUrlElement()
        {
            return true;
        }

        public override void BuildPacket()
        {

        }

        public override bool TakeAction()
        {
            var cacheSetUserPack = new GameDataCacheSet<UserPack>();
            var cacheSetBackPack = new ConfigCacheSet<BackpackConfigInfo>();
            var backpackConfigInfo = cacheSetBackPack.FindKey(1);
         
            
            var userPack = cacheSetUserPack.FindKey(UserId.ToString());
            var packType = userPack != null && userPack.PackTypeList != null ? userPack.PackTypeList.Find(s => s.BackpackType.ToInt() == 1) : null;
            
            if (backpackConfigInfo == null)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().LoadDataError;
                return false;
            }
            if (ContextUser.GoldNum < backpackConfigInfo.RequiredGoldNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St_GoldNotEnough;
                return false;
            }
            if (packType != null && packType.OpenNum >= backpackConfigInfo.MaxOpenNum)
            {
                ErrorCode = LanguageManager.GetLang().ErrorCode;
                ErrorInfo = LanguageManager.GetLang().St1110_OverExpansion;
                return false;
            }
            if (userPack == null)
            {
                userPack = new UserPack(UserId);
                packType = new PackType();
                userPack.CreateDate = DateTime.Now;
                packType.OpenNum = 1;
                packType.BackpackType = BackpackType.HunJi;
                packType.OpenNum = MathUtils.Addition(backpackConfigInfo.DefaultNum, backpackConfigInfo.EveryAddNum);
                userPack.PackTypeList.AddChildrenListener(packType);
                cacheSetUserPack.Add(userPack, UserId);
                
                
            }
            else
            {
                if (packType == null)
                {
                    packType = new PackType();
                    userPack.CreateDate = DateTime.Now;
                    packType.OpenNum = 1;
                    packType.BackpackType = BackpackType.HunJi;
                    packType.OpenNum = MathUtils.Addition(backpackConfigInfo.DefaultNum, backpackConfigInfo.EveryAddNum); cacheSetUserPack.Add(userPack, UserId);
                }
                else
                {
                    userPack.UpdateNotify(obj =>
                    {
                        packType.OpenNum = MathUtils.Subtraction(packType.OpenNum, 1);
                        packType.OpenNum = MathUtils.Addition(packType.OpenNum, backpackConfigInfo.EveryAddNum);
                        return true;
                    });
                }
            }
            MathUtils.DiffDate(DateTime.Now.Date).TotalDays.ToInt();
            return true;
        }
    }
}