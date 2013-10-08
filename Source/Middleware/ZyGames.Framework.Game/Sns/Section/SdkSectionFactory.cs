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
        public static SectionMIUIElement SectionMIUI
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.SectionMIUI;
            }
        }
        public static SectionDanleV2Element SectionDanleV2
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.SectionDanleV2;
            }
        }
        public static SectionGFanElement SectionGFan
        {
            get
            {
                SdkChannelSection section = GetSection();
                return section.SectionGFan;
            }
        }
    }
}