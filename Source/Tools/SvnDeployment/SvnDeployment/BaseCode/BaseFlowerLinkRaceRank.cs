using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Data.SqlClient;

/// <summary>
/// BaseFlowerLinkRaceRank 的摘要说明
/// </summary>
public class BaseFlowerLinkRaceRank
{
    #region 构造函数
    public BaseFlowerLinkRaceRank() { }

    public BaseFlowerLinkRaceRank(SqlDataReader reader)
    {
        if(reader ==null)
            throw new Exception();

        this.InitData(reader);
    }

    private void InitData(SqlDataReader reader)
    {
        this.LinkRankId = Convert.ToInt32(reader["LinkRankId"]);
        this.RaceId = Convert.ToInt16(reader["RaceId"]);
        this.RankID = Convert.ToInt16(reader["RankID"]);
        this.MustGardenLv = Convert.ToInt16(reader["MustGardenLv"]);
        this.MustAchievement = Convert.ToInt32(reader["MustAchievement"]);
        this.GrowingMinute = Convert.ToInt32(reader["GrowingMinute"]);
        this.RipeMinute = Convert.ToInt32(reader["RipeMinute"]);
        this.YieldMinute = Convert.ToInt32(reader["YieldMinute"]);
        this.OutputCount = Convert.ToInt16(reader["OutputCount"]);
        this.OutputNum = Convert.ToInt16(reader["OutputNum"]);
        this.MaxSteal = Convert.ToInt16(reader["MaxSteal"]);
        this.UnitSteal = Convert.ToInt16(reader["UnitSteal"]);
        this.PollenUnitName = reader["PollenUnitName"].ToString();
        this.PollenToolLV = Convert.ToInt16(reader["PollenToolLV"]);
        this.ReachSilver = Convert.ToInt16(reader["ReachSilver"]);
        this.ReachGold = Convert.ToInt16(reader["ReachGold"]);
        this.AchForSilver = Convert.ToInt16(reader["AchForSilver"]);
        this.AchForGold = Convert.ToInt16(reader["AchForGold"]);
        this.AchForNormal = Convert.ToInt16(reader["AchForNormal"]);
        this.RaiseExp = Convert.ToInt16(reader["RaiseExp"]);
        this.SellExp = Convert.ToInt16(reader["SellExp"]);
        this.WaterExp = Convert.ToInt16(reader["WaterExp"]);
        this.FertGrowExp = Convert.ToInt16(reader["FertGrowExp"]);
        this.GoldenMaxScale = Convert.ToSingle(reader["GoldenMaxScale"]);
        this.SilverMaxScale = Convert.ToSingle(reader["SilverMaxScale"]);
    }
   
    #endregion

    #region 私有变量
    private Int32 _linkrankid = Int32.MinValue;
    private short _raceid = short.MaxValue;
    private short _rankid = short.MaxValue;
    private short _mustgardenlv = short.MaxValue;
    private Int32 _mustachievement = Int32.MinValue;
    private Int32 _growingminute = Int32.MinValue;
    private Int32 _ripeminute = Int32.MinValue;
    private Int32 _yieldminute = Int32.MinValue;
    private short _outputcount = short.MaxValue;
    private short _outputnum = short.MaxValue;
    private short _maxsteal = short.MaxValue;
    private short _unitsteal = short.MaxValue;
    private string _pollenunitname = null;
    private short _pollentoollv = short.MaxValue;
    private short _reachsilver = short.MaxValue;
    private short _reachgold = short.MaxValue;
    private short _achforsilver = short.MaxValue;
    private short _achforgold = short.MaxValue;
    private short _achfornormal = short.MaxValue;
    private short _raiseexp = short.MaxValue;
    private short _sellexp = short.MaxValue;
    private short _waterexp = short.MaxValue;
    private short _fertgrowexp = short.MaxValue;
    private float _goldenmaxscale = Int32.MinValue;
    private float _silvermaxscale = Int32.MinValue;
    #endregion

    #region 公共属性
    /// <summary>
    /// 主键 关联ID(NOT NULL)
    /// </summary>
    public Int32 LinkRankId
    {
        set { _linkrankid = value; }
        get { return _linkrankid; }
    }
    /// <summary>
    /// 种族编号
    /// </summary>
    public short RaceId
    {
        set { _raceid = value; }
        get { return _raceid; }
    }
    /// <summary>
    /// 花阶编号
    /// </summary>
    public short RankID
    {
        set { _rankid = value; }
        get { return _rankid; }
    }
    /// <summary>
    /// 种植所需用户等
    /// </summary>
    public short MustGardenLv
    {
        set { _mustgardenlv = value; }
        get { return _mustgardenlv; }
    }
    /// <summary>
    /// 种植所需成就值
    /// </summary>
    public Int32 MustAchievement
    {
        set { _mustachievement = value; }
        get { return _mustachievement; }
    }
    /// <summary>
    /// 种子到成熟所需时间
    /// </summary>
    public Int32 GrowingMinute
    {
        set { _growingminute = value; }
        get { return _growingminute; }
    }
    /// <summary>
    /// RipeMinute
    /// </summary>
    public Int32 RipeMinute
    {
        set { _ripeminute = value; }
        get { return _ripeminute; }
    }
    /// <summary>
    /// 产粉期待产时间
    /// </summary>
    public Int32 YieldMinute
    {
        set { _yieldminute = value; }
        get { return _yieldminute; }
    }
    /// <summary>
    /// 产粉多少季
    /// </summary>
    public short OutputCount
    {
        set { _outputcount = value; }
        get { return _outputcount; }
    }
    /// <summary>
    /// 每季产粉数量
    /// </summary>
    public short OutputNum
    {
        set { _outputnum = value; }
        get { return _outputnum; }
    }
    /// <summary>
    /// 每季最大可偷取数量
    /// </summary>
    public short MaxSteal
    {
        set { _maxsteal = value; }
        get { return _maxsteal; }
    }
    /// <summary>
    /// 单次可偷取数量
    /// </summary>
    public short UnitSteal
    {
        set { _unitsteal = value; }
        get { return _unitsteal; }
    }
    /// <summary>
    /// 花粉的单位名称
    /// </summary>
    public string PollenUnitName
    {
        set { _pollenunitname = value; }
        get { return _pollenunitname; }
    }
    /// <summary>
    /// PollenToolLV
    /// </summary>
    public short PollenToolLV
    {
        set { _pollentoollv = value; }
        get { return _pollentoollv; }
    }
    /// <summary>
    /// 点亮银品花所需化肥个数
    /// </summary>
    public short ReachSilver
    {
        set { _reachsilver = value; }
        get { return _reachsilver; }
    }
    /// <summary>
    /// 点亮金品花所需化肥个数
    /// </summary>
    public short ReachGold
    {
        set { _reachgold = value; }
        get { return _reachgold; }
    }
    /// <summary>
    /// 点亮银品花得到的成就值
    /// </summary>
    public short AchForSilver
    {
        set { _achforsilver = value; }
        get { return _achforsilver; }
    }
    /// <summary>
    /// 点亮金品花得到的成就值
    /// </summary>
    public short AchForGold
    {
        set { _achforgold = value; }
        get { return _achforgold; }
    }
    /// <summary>
    /// AchForNormal
    /// </summary>
    public short AchForNormal
    {
        set { _achfornormal = value; }
        get { return _achfornormal; }
    }
    /// <summary>
    /// RaiseExp
    /// </summary>
    public short RaiseExp
    {
        set { _raiseexp = value; }
        get { return _raiseexp; }
    }
    /// <summary>
    /// SellExp
    /// </summary>
    public short SellExp
    {
        set { _sellexp = value; }
        get { return _sellexp; }
    }
    /// <summary>
    /// WaterExp
    /// </summary>
    public short WaterExp
    {
        set { _waterexp = value; }
        get { return _waterexp; }
    }
    /// <summary>
    /// FertGrowExp
    /// </summary>
    public short FertGrowExp
    {
        set { _fertgrowexp = value; }
        get { return _fertgrowexp; }
    }
    /// <summary>
    /// GoldenMaxScale
    /// </summary>
    public float GoldenMaxScale
    {
        set { _goldenmaxscale = value; }
        get { return _goldenmaxscale; }
    }
    /// <summary>
    /// SilverMaxScale
    /// </summary>
    public float SilverMaxScale
    {
        set { _silvermaxscale = value; }
        get { return _silvermaxscale; }
    }
    #endregion
}

