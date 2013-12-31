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
using ZyGames.Framework.Game.Runtime;

namespace ZyGames.Tianjiexing.BLL.GM
{
    /// <summary>
    /// 魔术
    /// </summary>
    public class MagicCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int magicID = args.Length > 0 ? args[0].Trim().ToInt() : 0;
            short magicLv = args.Length > 1 ? args[1].Trim().ToShort() : (short)1;

            var cacheSet = new GameDataCacheSet<UserMagic>();
            var userMagic = cacheSet.FindKey(UserID, magicID);
            MagicInfo magic = new ConfigCacheSet<MagicInfo>().FindKey(magicID);
            if (magic == null)
            {
                return;
            }
            if (userMagic == null)
            {
                userMagic = new UserMagic()
                {
                    UserID = UserID,
                    MagicID = magicID,
                    MagicLv = magicLv,
                    IsEnabled = false,
                    MagicType = magic.MagicType,
                };
                cacheSet.Add(userMagic);
            }
            else
            {
                userMagic.MagicLv = magicLv;
            }
        }
    }
}