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
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.BLL.Base;

namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 1903_魔法阵启用接口
    /// </summary>
    public class Action1903 : BaseAction
    {
        private int _magicID;


        public Action1903(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action1903, httpGet)
        {

        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            if (httpGet.GetInt("MagicID", ref _magicID))
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
            var userMagicArray = new GameDataCacheSet<UserMagic>().FindAll(ContextUser.UserID, m => m.MagicType == MagicType.MoFaZhen);
            foreach (UserMagic magic in userMagicArray)
            {
                if (magic.MagicID == _magicID)
                {
                    magic.IsEnabled = true;
                    ContextUser.UseMagicID = _magicID;
                    //ContextUser.Update();
                }
                else
                {
                    magic.IsEnabled = false;
                }
                //magic.Update();
                UserHelper.GetGameUserCombat(ContextUser.UserID);
            }
            return true;
        }
    }
}