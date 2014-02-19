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
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Lang;
using ZyGames.Tianjiexing.Model;
using ZyGames.Tianjiexing.Model.Config;
using ZyGames.Tianjiexing.Model.Enum;
using ZyGames.Tianjiexing.BLL.Base;
using System;


namespace ZyGames.Tianjiexing.BLL.Action
{

    /// <summary>
    /// 退出答题接口
    /// </summary>
    public class Action6304 : BaseAction
    {
        public Action6304(ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(ActionIDDefine.Cst_Action6304, httpGet)
        {
        }

        public override void BuildPacket()
        {

        }

        public override bool GetUrlElement()
        {
            return true;
        }

        public override bool TakeAction()
        {
            ContextUser.UserLocation = Location.City;
            UserGuild guild = new ShareCacheStruct<UserGuild>().FindKey(ContextUser.MercenariesID);
            if (guild == null)
            {
                return true;
            }
            if (guild.GuildExercise == null)
            {
                return true;
            }
            guild.GuildExercise.UpdateNotify(obj =>
                {
                    guild.GuildExercise.UserList.RemoveAll(u => u.UserID == Uid);
                    return true;
                }
                );
            return true;
        }
    }
}