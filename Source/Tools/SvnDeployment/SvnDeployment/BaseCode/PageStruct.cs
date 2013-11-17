using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
namespace CitGame
{
    /// <summary>
    /// PageStruct 的摘要说明
    /// 分页大小计算类
    /// </summary>
    public class PageStruct
    {
        private const int _FirstPage = 1;

        private int _iPageIndex;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int iPageIndex
        {
            get { return _iPageIndex; }
            set
            {
                if (value <= 0)
                {
                    _iPageIndex = 1;
                }
                else
                {
                    _iPageIndex = value;
                }
            }
        }

        /// <summary>
        /// 传输到存储过程中进行查询的页码，为当前页码数减去1
        /// </summary>
        public int ProcPageIndex
        {
            get
            {
                return _iPageIndex - 1;
            }
        }
        /// <summary>
        /// 页大小
        /// </summary>
        public int iPageSize;
        /// <summary>
        /// 记录总条数
        /// </summary>
        public int sRecNum;      
        /// <summary>
        /// 页总数
        /// </summary>
        public int sPageNum; 
        /// <summary>
        /// 当页记录数
        /// </summary>
        public int sCurRecNum; //

        public PageStruct()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 数据库执行分页时的请求页面
        /// </summary>
        public int DBPageIndex
        {
            get { return Convert.ToInt32(this.iPageIndex + 1); }
        }
        public void Page()
        {
            this.Page(this.sRecNum, this.iPageIndex, this.iPageSize);
        }
        public void Page(int totalnum, int pageindex, int pagesize)
        {
            try
            {
               
                sRecNum = totalnum;
                iPageIndex = pageindex;
                iPageSize = pagesize;

                int index = Convert.ToInt32(iPageIndex);
                index = index + 1;

                sPageNum = Convert.ToInt32(sRecNum / iPageSize);
                if (sRecNum % iPageSize != 0)
                {
                    sPageNum++;
                }

                if (this.iPageIndex > sPageNum)
                {
                    this.iPageIndex = _FirstPage ;
                }

                //当前页
                if (index == sPageNum && sRecNum % iPageSize != 0)
                {
                    sCurRecNum = Convert.ToInt32(sRecNum % iPageSize);
                }
                else
                {
                    sCurRecNum = iPageSize;
                }

            }
            catch (Exception ee)
            {
                func.savetofile(ee.ToString(), "PageStruct");
            }
        }

        public void AddPageLink(ref Panel div, string aPageUrl, Dictionary<object, object> dicUrlElement )
        {

            dicUrlElement.Add("page", 1);
            if (this._iPageIndex != _FirstPage)
            {
                dicUrlElement.Remove("page");
                dicUrlElement.Add("page", 1);
                div.Controls.Add(Add("首页", aPageUrl, dicUrlElement));
                dicUrlElement.Remove("page");
                dicUrlElement.Add("page", this.iPageIndex - 1);
                div.Controls.Add(Add("上一页", aPageUrl, dicUrlElement));
            }
            if (this._iPageIndex != this.sPageNum)
            {
                dicUrlElement.Remove("page");
                dicUrlElement.Add("page", this.iPageIndex + 1);
                div.Controls.Add(Add("下一页", aPageUrl, dicUrlElement));
                dicUrlElement.Remove("page");
                dicUrlElement.Add("page", this.sPageNum);
                div.Controls.Add(Add("末页", aPageUrl, dicUrlElement));
            }

            div.Controls.Add(BaseControls.GetLabelValue(string.Concat("总记录：", this.sRecNum)));
            div.Controls.Add(BaseControls.GetLabelValue(string.Concat("共", this.sPageNum, "页")));
        }

        private Label Add(string aPageName, string aPageUrl, Dictionary<object, object> dicUrlElement)
        {
            Label oLabel = BaseControls.GetLabelValue("", 50, 20);
            HyperLink oTmp = BaseControls.GetHyperLink(aPageName, aPageUrl, dicUrlElement);            
            oLabel.Controls.Add(oTmp);
            return oLabel;
        }
    }
}