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
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.GM
{
    /// <summary>
    /// 清缓存
    /// </summary>
    public class CacheCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            string childType = args.Length > 0 ? args[0].Trim() : string.Empty;

            if (childType.Equals("all"))
            {
                ConfigCacheGlobal.Load();
                ClearUser(string.Empty);
            }
            else if (string.IsNullOrEmpty(childType) || childType.Equals("self"))
            {
                //清自己
                ClearUser(UserID);
            }
            else if (childType.ToUpper().StartsWith("Z"))
            {
                string userId = "";
                string pid = childType;
                new GameDataCacheSet<GameUser>().Foreach((personalId, key, value) =>
                {
                    if (value != null && value.Pid.Equals(pid))
                    {
                        userId = value.UserID;
                        return false;
                    }
                    return true;
                });
                if (!string.IsNullOrEmpty(userId))
                {
                    ClearUser(userId);
                }
            }
            else
            {
                throw new Exception(string.Format(CmdError, childType));
            }
        }

        private void ClearUser(string userId)
        {
            UserCacheGlobal.ReLoad(userId);
        }
    }
}