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

        [ConfigurationProperty("channelMIUI")]
        public SectionMIUIElement SectionMIUI
        {
            get { return this["channelMIUI"] as SectionMIUIElement; }
            set { this["channelMIUI"] = value; }
        }

        [ConfigurationProperty("channelDanle_v2")]
        public SectionDanleV2Element SectionDanleV2
        {
            get { return this["channelDanle_v2"] as SectionDanleV2Element; }
            set { this["channelDanle_v2"] = value; }
        }

        [ConfigurationProperty("channelGFan")]
        public SectionGFanElement SectionGFan
        {
            get { return this["channelGFan"] as SectionGFanElement; }
            set { this["channelGFan"] = value; }
        }
    }
}