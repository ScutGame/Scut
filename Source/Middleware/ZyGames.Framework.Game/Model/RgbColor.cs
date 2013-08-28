using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.Game.Model
{
    /// <summary>
    /// RGB颜色结构
    /// </summary>
    public struct RgbColor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public RgbColor(int r, int g, int b)
        {
            if (!CheckRange(r))
            {
                throw new ArgumentOutOfRangeException("r", r, "Rgb'r out range");
            }
            if (!CheckRange(g))
            {
                throw new ArgumentOutOfRangeException("g", r, "Rgb'g out range");
            }
            if (!CheckRange(b))
            {
                throw new ArgumentOutOfRangeException("b", r, "Rgb'b out range");
            }
            R = r;
            G = g;
            B = b;
        }

        private static bool CheckRange(int value)
        {
            return value > -1 && value < 256;
        }

        ///<summary>
        /// R
        ///</summary>
        public int R;

        ///<summary>
        /// G
        ///</summary>
        public int G;

        ///<summary>
        /// B
        ///</summary>
        public int B;
    }
}
