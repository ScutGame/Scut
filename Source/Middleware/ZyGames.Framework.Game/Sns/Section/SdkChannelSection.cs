using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    /// <summary>
    /// 渠道SDK配置
    /// </summary>
    public class SdkChannelSection : ConfigurationSection
    {

        [ConfigurationProperty("channel91")]
        public Section91Element Section91
        {
            get { return this["channel91"] as Section91Element; }
            set { this["channel91"] = value; }
        }

        [ConfigurationProperty("channelUC")]
        public SectionUCElement SectionUC
        {
            get { return this["channelUC"] as SectionUCElement; }
            set { this["channelUC"] = value; }
        }

        [ConfigurationProperty("channelDanle")]
        public SectionDanleElement SectionDanle
        {
            get { return this["channelDanle"] as SectionDanleElement; }
            set { this["channelDanle"] = value; }
        }

        [ConfigurationProperty("channel10086")]
        public Section10086Element Section10086
        {
            get { return this["channel10086"] as Section10086Element; }
            set { this["channel10086"] = value; }
        }

        [ConfigurationProperty("channel360")]
        public Section360Element Section360
        {
            get { return this["channel360"] as Section360Element; }
            set { this["channel360"] = value; }
        }
    }
}
