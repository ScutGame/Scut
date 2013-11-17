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
    public class GoinCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int goinNum = args.Length > 0 ? args[0].Trim().ToInt() : 1;
            Process(UserID, goinNum);
        }

        private void Process(string userID, int goinNum)
        {
            GameUser currentUser = new GameDataCacheSet<GameUser>().FindKey(userID);
            if (currentUser != null)
            {
                currentUser.GameCoin = MathUtils.Addition(currentUser.GameCoin, goinNum, int.MaxValue);
            }
            //currentUser.Update();

        }
    }
}