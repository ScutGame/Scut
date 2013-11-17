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
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.GM
{
    /// <summary>
    /// 玩家升级
    /// </summary>
    public class GeneralLvCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int generalID = 0;
            short lv = 1;
            if (args.Length > 1)
            {
                generalID = args[0].Trim().ToInt();
                lv = args[1].Trim().ToShort();
            }
            else if (args.Length > 0 && args[0].ToInt() < 200)
            {
                lv = args.Length > 0 ? args[0].Trim().ToShort() : (short)1;
            }
            Process(UserID, generalID, lv);
        }

        private void Process(string userID, int generalID, short lv)
        {
            var cacheSet = new GameDataCacheSet<UserGeneral>();
            var userGeneral = cacheSet.FindKey(userID, generalID);
            if (generalID > 0 && userGeneral != null)
            {
                userGeneral.GeneralLv = lv;
            }
            else
            {
                GameUser currentUser = new GameDataCacheSet<GameUser>().FindKey(UserID);
                currentUser.UserLv = lv;
                currentUser.IsLv = true;
            }
            //var userGeneralList = cacheSet.FindAll(userID, m => m.IsUserGeneral);

            //foreach (UserGeneral general in userGeneralList)
            //{
            //    general.GeneralLv = lv;
            //    //general.Update();
            //}
        }
    }
}