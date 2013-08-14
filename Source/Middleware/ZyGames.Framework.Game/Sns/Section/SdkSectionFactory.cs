using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Sns.Section
{
    public static class SdkSectionFactory
    {
        private static SdkChannelSection GetSection()
        {
            return GetSection("sdkChannel");
        }

        private static SdkChannelSection GetSection(string name)
        {
            return ConfigurationManager.GetSection(name) as SdkChannelSection;
        }

        public static Section91Element Section91
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.Section91;
            }
        }

        public static SectionDanleElement SectionDanle
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.SectionDanle;
            }
        }

        public static SectionUCElement SectionUC
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.SectionUC;
            }
        }

        public static Section10086Element Section10086
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.Section10086;
            }
        }

        public static Section360Element Section360
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.Section360;
            }
        }
    }
}
