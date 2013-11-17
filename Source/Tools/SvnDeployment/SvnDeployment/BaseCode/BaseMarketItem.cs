using System;
using System.Data;
using System.Configuration;

namespace CitGame
{
    /// <summary>
    /// 商城道具类的基础类
    /// </summary>
    public abstract class BaseMarketItem
    {
        protected const string sDataVersionSplit = ",";
        /// <summary>
        /// 商店道具类型定义枚举
        /// </summary>
        public enum EItemType
        {
            /// <summary>
            /// 食物 - 1
            /// </summary>
            Food = 1,
          
            /// <summary>
            /// 动物 - 2
            /// </summary>
            Animal, 
            /// <summary>
            /// 材料 - 3
            /// </summary>
            Stuffer,          
            /// <summary>
            /// 道具 - 4
            /// </summary>
            Item 
        }
        
        /// <summary>
        /// 物品/道具/食物详细分类
        /// </summary>
        public enum EItemSort
        {
            /// <summary>
            /// 聊天喇叭道具 - 1
            /// </summary>
            ChatItem = 1,
            /// <summary>
            /// 卡片道具 - 2
            /// </summary>
            CardItem,
            /// <summary>
            /// VIP资格包 - 3
            /// </summary>
            VIP_Pack,
            /// <summary>
            /// 特殊饲料 - 4 
            /// </summary>
            SpecFood,
            /// <summary>
            /// 普通食物 - 5
            /// </summary>
            NormalFood,
            /// <summary>
            /// 材料/副产品 - 6
            /// </summary>
            BasicStuffer,
            /// <summary>
            /// 出产物幼仔 - 7
            /// </summary>
            OutputBaby,
            /// <summary>
            /// 商店动物幼仔 - 8 
            /// </summary>
            ShopAnimalBaby,
            /// <summary>
            /// 星级动物 - 9
            /// </summary>
            GotAnimalStar, 
            /// <summary>
            /// 未定义
            /// </summary>
            UnDefine
        }

        /// <summary>
        /// 物品定义唯一ID
        /// </summary>
        public short ShopID;

        /// <summary>
        /// 道具的唯一ID
        /// </summary>
        public short itemid;
        /// <summary>
        /// 道具名称
        /// </summary>
        public string itemName;
        /// <summary>
        /// 道具类型
        /// </summary>
        public EItemType itemType;
        /// <summary>
        /// 获取物品的详细分类
        /// </summary>
        public EItemSort itemSort{get{return _itemSort;}}
        private EItemSort _itemSort;

        /// <summary>
        /// 道具所属栏目
        /// </summary>
        public short BelongSort;
        /// <summary>
        /// 购买时消费的币种类型
        /// </summary>
        public PriceType PriceType;

        /// <summary>
        /// 原价格（单位：分）
        /// </summary>
        public int originalPrice;
        /// <summary>
        /// 优惠后的价格（单位：分）
        /// </summary>
        public int ratePrice;
        /// <summary>
        /// 是否推荐类道具
        /// </summary>
        public byte IsCommend;
        /// <summary>
        /// 是否热门道具
        /// </summary>
        public byte IsHot;
        /// <summary>
        /// 是否新品
        /// </summary>
        public byte IsNew;
        /// <summary>
        /// 道具说明
        /// </summary>
        public string ItemDesc;
        /// <summary>
        /// 道具购买后在仓库中的有效期（单位：天）
        /// </summary>
        public short validDate;
        /// <summary>
        /// 道具的头像ID
        /// </summary>
        public short HeadID;
        /// <summary>
        /// 是否在商店里隐藏
        /// </summary>
        public bool IsHidden;
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName;
        /// <summary>
        /// 是否可出售给系统商店
        /// </summary>
        public bool IsCanSellToShop;
        /// <summary>
        /// 出售给商店的价格
        /// </summary>
        public int SellToShopPrice;
        /// <summary>
        /// 是否仅VIP可买
        /// </summary>
        public bool IsBuyMustVip;
        /// <summary>
        /// 道具是否绑定
        /// </summary>
        public bool IsBind;

        /// <summary>
        /// 数据可使用对应的客户端版本号
        /// </summary>
        protected string _DataForClientVersions;
        
        public BaseMarketItem()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 重载类，初始化数据类
        /// </summary>
        /// <param name="aReader"></param>
        public abstract void initData(System.Data.SqlClient.SqlDataReader aReader);


        protected void PutData(System.Data.SqlClient.SqlDataReader aReader)
        {
            if (aReader == null) throw new Exception();
            this.ShopID = Convert.ToInt16(aReader["ShopID"]);
            
            this.HeadID = Convert.ToInt16(aReader["HeadID"]);
            this.IsCommend = Convert.ToByte(aReader["IsCommend"]);
            this.IsHot = Convert.ToByte(aReader["IsHot"]);
            this.IsNew = Convert.ToByte(aReader["IsNew"]);
            this.IsCanSellToShop = Convert.ToBoolean(aReader["IsCanSellToShop"]);
            this.SellToShopPrice = Convert.ToInt32(aReader["SellToShopPrice"]);
            this.IsBuyMustVip = Convert.ToBoolean(aReader["IsBuyMustVip"]);
            this.IsBind = Convert.ToBoolean(aReader["IsBind"]);
            this.ItemDesc = Convert.ToString(aReader["ItemDesc"]);
            this.itemid = Convert.ToInt16(aReader["ItemId"]);
            this.itemName = Convert.ToString(aReader["itemName"]);
            this.originalPrice = Convert.ToInt32(aReader["BuyoriginalPrice"]);
            this.ratePrice = Convert.ToInt32(aReader["BuyRatePrice"]);
            this.validDate = Convert.ToInt16(aReader["ValidDate"]);
            this.PriceType = (PriceType)Enum.Parse(typeof(PriceType), Convert.ToInt16(aReader["BuyPriceType"]).ToString(), true);
            this.IsHidden = Convert.ToBoolean(aReader["IsHidden"]);
            this.BelongSort = Convert.ToInt16(aReader["BelongSort"]);
            this.UnitName = Convert.ToString(aReader["UnitName"]);
            this.itemType = (EItemType )Enum.Parse(typeof(EItemType), Convert.ToInt16(aReader["itemType"]).ToString(), true);
            this._itemSort = (EItemSort)Enum.Parse(typeof(EItemSort), Convert.ToInt16(aReader["ItemSort"]).ToString(), true);
            this._DataForClientVersions = sDataVersionSplit + Convert.ToString(aReader["ForClientVersions"]) + sDataVersionSplit;
        }
    }
}