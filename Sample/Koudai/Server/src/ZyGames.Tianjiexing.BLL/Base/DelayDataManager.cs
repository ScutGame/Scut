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
using ServiceStack.Text;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Common;
using ZyGames.Tianjiexing.Model;

namespace ZyGames.Tianjiexing.BLL.Base
{
    public class DelayDataManager
    {
        public static void DoProcessAll()
        {
            try
            {
                WriteItemPackage(true);
                WriteSparePackage(true);
                WriteCrystal(true);
                WriteEnchant(true);
                WriteTrump(true);
                WriteGeneralMedicine(true);
                WriteUserGeneral(true);
                WriteUserMagic(true);
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        private static void WriteSparePackage(bool isAll)
        {
            try
            {
                new GameDataCacheSet<UserSparePackage>().Foreach((personalId, key, package) =>
                {
                    if (isAll || package.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("SpareInfo:{0}", package.ToJson());
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        private static void WriteItemPackage(bool isAll)
        {
            try
            {
                new GameDataCacheSet<UserItemPackage>().Foreach((personalId, key, data) =>
                {
                    if (isAll || data.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("ItemInfo:{0}", data.ToJson());
                        //new BaseLog("ItemInfo").SaveDebugLog();
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        private static void WriteCrystal(bool isAll)
        {
            try
            {
                new GameDataCacheSet<UserCrystalPackage>().Foreach((personalId, key, data) =>
                {
                    if (isAll || data.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("Crystal:{0}", data.ToJson());
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        private static void WriteEnchant(bool isAll)
        {
            try
            {
                new GameDataCacheSet<UserEnchant>().Foreach((personalId, key, data) =>
                {
                    if (isAll || data.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("Enchant:{0}", data.ToJson());
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        private static void WriteTrump(bool isAll)
        {
            try
            {
                new GameDataCacheSet<UserTrump>().Foreach((personalId, key, data) =>
                {
                    if (isAll || data.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("Trump:{0}", data.ToJson());
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        public static void WriteGeneralMedicine(bool isAll)
        {
            try
            {
                new GameDataCacheSet<GeneralMedicine>().Foreach((personalId, key, data) =>
                {
                    if (isAll || data.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("Medicine:{0}", data.ToJson());
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        public static void WriteUserGeneral(bool isAll)
        {
            try
            {
                new GameDataCacheSet<UserGeneral>().Foreach((personalId, key, data) =>
                {
                    if (isAll || data.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("General:{0}", data.ToJson());
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }

        public static void WriteUserMagic(bool isAll)
        {
            try
            {
                new GameDataCacheSet<UserMagic>().Foreach((personalId, key, data) =>
                {
                    if (isAll || data.HasChanged)
                    {
                        TraceLog.ReleaseWriteDebug("Magic:{0}", data.ToJson());
                    }
                    return true;
                });
            }
            catch (Exception ex)
            {
                new BaseLog().SaveLog(ex);
            }
        }
    }
}