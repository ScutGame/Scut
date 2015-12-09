using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Framework.Config
{
    /// <summary>
    /// Profile node of section
    /// </summary>
    public class ProfileSection : ConfigSection
    {
        /// <summary>
        /// 
        /// </summary>
        public ProfileSection()
        {

            ProfileEnableCollect = ConfigUtils.GetSetting("Profile.EnableCollect", false);
            ProfileCollectInterval = ConfigUtils.GetSetting("Profile.CollectInterval", 10);
            ProfileLogPath = ConfigUtils.GetSetting("Profile.LogPath", "");
            IsOpenWriteLog = ConfigUtils.GetSetting("Profile.OpenWriteLog", false);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ProfileEnableCollect { get; set; }
        /// <summary>
        /// Profile collect interval sec
        /// </summary>
        public int ProfileCollectInterval { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProfileLogPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsOpenWriteLog { get; set; }


    }
}
