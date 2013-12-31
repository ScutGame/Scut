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
using ZyGames.Tianjiexing.Model.Enum;

namespace ZyGames.Tianjiexing.BLL.GM
{
    public class GeneralCommand : TjBaseCommand
    {
        protected override void ProcessCmd(string[] args)
        {
            int generalID = args.Length > 0 ? args[0].Trim().ToInt() : 1;
            Process(UserID, generalID);
        }

        private void Process(string userID, int generalID)
        {
            GeneralInfo generalInfo = new ConfigCacheSet<GeneralInfo>().FindKey(generalID);
            if (generalInfo != null)
            {
                var cacheSet = new GameDataCacheSet<UserGeneral>();
                var usergeneral = cacheSet.FindKey(userID, generalID);
                if (usergeneral == null)
                {
                    usergeneral = new UserGeneral()
                    {
                        UserID = userID,
                        GeneralID = generalID,
                        GeneralName = generalInfo.GeneralName,
                        HeadID = generalInfo.HeadID,
                        PicturesID = generalInfo.PicturesID,
                        GeneralLv = generalInfo.GeneralLv,
                        LifeNum = generalInfo.LifeNum,
                        GeneralType = GeneralType.YongBing,
                        CareerID = generalInfo.CareerID,
                        PowerNum = generalInfo.PowerNum,
                        SoulNum = generalInfo.SoulNum,
                        IntellectNum = generalInfo.IntellectNum,
                        TrainingPower = 0,
                        TrainingSoul = 0,
                        TrainingIntellect = 0,
                        AbilityID = generalInfo.AbilityID,
                        Momentum = 25,
                        HitProbability = 85,
                        GeneralStatus = GeneralStatus.DuiWuZhong,
                        Experience1 = 0,
                        Experience2 = 0,
                        CurrExperience = 0,
                        Description = string.Empty,
                    };
                    cacheSet.Add(usergeneral);
                }
            }
        }
    }
}