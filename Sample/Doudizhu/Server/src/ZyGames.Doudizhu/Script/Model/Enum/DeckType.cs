using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Doudizhu.Model
{
    /// <summary>
    /// 牌型
    /// </summary>
    public enum DeckType
    {
        /// <summary>
        /// 未识别
        /// </summary>
        Error = -1,
        /// <summary>
        /// 过牌为空
        /// </summary>
        None = 0,
        /// <summary>
        /// 单牌1
        /// </summary>
        Single,
        /// <summary>
        /// 对牌2
        /// </summary>
        Double,
        /// <summary>
        /// 王炸3
        /// </summary>
        WangBomb,
        /// <summary>
        /// 三张4
        /// </summary>
        Three,
        /// <summary>
        /// 三带一5
        /// </summary>
        ThreeAndOne,
        /// <summary>
        /// 三带二6
        /// </summary>
        ThreeAndTwo,
        /// <summary>
        /// 炸弹7
        /// </summary>
        Bomb,
        /// <summary>
        /// 顺子8
        /// </summary>
        Shunzi,
        /// <summary>
        /// 四带二9
        /// </summary>
        FourAndTwo,
        /// <summary>
        /// 连对10
        /// </summary>
        Liandui,
        /// <summary>
        /// 三顺11
        /// </summary>
        Fly,
        /// <summary>
        /// 飞机带二12
        /// </summary>
        FlyAndTwo,
        /// <summary>
        /// 二连飞机带二对13
        /// </summary>
        FlyAndTwoDouble

    }
}
