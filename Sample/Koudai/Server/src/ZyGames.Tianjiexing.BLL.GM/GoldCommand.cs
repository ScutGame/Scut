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
using ZyGames.Framework;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.GM
{
    public class GoldCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int goldNum = args.Length > 0 ? args[0].Trim().ToInt() : 1;
            int type = args.Length > 1 ? args[1].Trim().ToInt() : 0;
            Process(UserID, goldNum, type);
        }

        private void Process(string userID, int goldNum, int type)
        {
            GameUser currentUser = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (currentUser != null)
            {
                if (type == 0)
                {
                    currentUser.GiftGold = MathUtils.Addition(currentUser.GiftGold, goldNum, int.MaxValue);
                }
                else
                {
                    currentUser.ExtGold = MathUtils.Addition(currentUser.ExtGold, goldNum);
                }
            }
        }
    }
}