using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// BaseControls 的摘要说明
/// </summary>
public class BaseControls
{
    private const string CstCssTableHeaderStyle = "TableHeaderStyle";
    private const string CstCssTableRowStyle = "TableRowStyle";
	public BaseControls()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public static Panel GetPanel()
    {
        return new Panel();
    }



    public static Label GetLabelKey(string aKeyValue)
    {
        return GetLabelKey(aKeyValue, 100, 20);
    }

    public static Label GetLabelKey(string aKeyValue, int iWidth)
    {
        return GetLabelKey(aKeyValue, iWidth, 20);
    }

    public static Label GetLabelKey(string aKeyValue, int iWidth, int iHeight)
    {
        Label oLabel = new Label();
        oLabel.Text = aKeyValue;
        oLabel.CssClass = CstCssTableHeaderStyle;
        oLabel.Width = iWidth;
        oLabel.Height = iHeight;        
        return oLabel;
    }

    public static Label GetLabelValue(string aKeyValue)
    {
        return GetLabelValue(aKeyValue, 100, 20);
    }
    public static Label GetLabelValue(string aKeyValue, int iWidth)
    {
        return GetLabelValue(aKeyValue, iWidth, 20);
    }
    public static Label GetLabelValue(string aKeyValue, int iWidth, int iHeight)
    {
        Label oLabel = new Label();
        oLabel.Text = aKeyValue;
        oLabel.CssClass = CstCssTableRowStyle;
        oLabel.Width = iWidth;
        oLabel.Height = iHeight;
        return oLabel;
    }
    /// <summary>
    /// 返回Url链接对象
    /// </summary>
    /// <param name="aTxtValue">文本内容</param>
    /// <param name="aLinkUrl">链接地址</param>
    /// <returns></returns>
    public static HyperLink GetHyperLink(string aTxtValue, string aLinkUrl)
    {
        return GetHyperLink(aTxtValue, aLinkUrl, null);
    }
    /// <summary>
    /// 重载返回Url链接对象
    /// </summary>
    /// <param name="aTxtValue">文本内容</param>
    /// <param name="aLinkUrl">链接地址</param>
    /// <param name="iDica">链接参数</param>
    /// <returns></returns>
    public static HyperLink GetHyperLink(string aTxtValue, string aLinkUrl, Dictionary<object, object> iDica)
    {

        HyperLink oTmpHyperLink = new HyperLink();
        oTmpHyperLink.Text = aTxtValue;
        oTmpHyperLink.NavigateUrl = aLinkUrl;
        
        if (iDica != null)
        {
            if ((aLinkUrl.LastIndexOf("?") + 1) != aLinkUrl.Length)
            {
                oTmpHyperLink.NavigateUrl = aLinkUrl + "?";
            }
            object[] linkKeys = new object[iDica.Keys.Count];
            iDica.Keys.CopyTo(linkKeys, 0);
            string sTmpUrlParameter = "";
            for (int i = 0; i < linkKeys.Length; i++)
            {
                object tmpParamValue = null;
                if (iDica.TryGetValue(linkKeys[i], out tmpParamValue))
                {
                    sTmpUrlParameter += string.Concat(linkKeys[i].ToString(), "=", tmpParamValue);
                }
                else
                {
                    throw new Exception();
                }
                if (i < linkKeys.Length - 1)
                {
                    sTmpUrlParameter += "&";
                }
            }
            oTmpHyperLink.NavigateUrl += sTmpUrlParameter;
        }
        return oTmpHyperLink;
    }

   

    public static HtmlAnchor GetHtmlAnchor(string aTxtValue, string aLinkUrl, Dictionary<object, object> iDica)
    {
        
        HtmlAnchor oTmpLink = new HtmlAnchor();
        oTmpLink.InnerText = aTxtValue;
        oTmpLink.HRef = aLinkUrl;
        if (iDica != null)
        {
            if ((aLinkUrl.LastIndexOf("?") + 1) != aLinkUrl.Length)
            {
                oTmpLink.HRef = aLinkUrl + "?";
            }
            object[] linkKeys = new object[iDica.Keys.Count];
            iDica.Keys.CopyTo(linkKeys, 0);
            string sTmpUrlParameter = "";
            for (int i = 0; i < linkKeys.Length; i++)
            {
                object tmpParamValue = null;
                if (iDica.TryGetValue(linkKeys[i], out tmpParamValue))
                {
                    sTmpUrlParameter += string.Concat(linkKeys[i].ToString(), "=", tmpParamValue);
                }
                else
                {
                    throw new Exception();
                }
                if (i < linkKeys.Length - 1)
                {
                    sTmpUrlParameter += "&";
                }
            }
            oTmpLink.HRef += sTmpUrlParameter;
        }
        return oTmpLink;
        //Ht
        //oTmpLink.Controls.Add(
    }

    public static TableRow GetTableRow(string tabid)
    {
        TableRow tmpTr = new TableRow();
        tmpTr.CssClass = CstCssTableHeaderStyle;
        if (tabid != "")
        {
            tmpTr.ID = tabid;
        }
        return tmpTr;
    }

    public static TableCell GetTableCell(string aTxtValue)
    {
        TableCell tmpTd = new TableCell();
        tmpTd.CssClass = CstCssTableRowStyle;
        tmpTd.Text = aTxtValue;
        return tmpTd;
    }

    /// <summary>
    /// 返回Html类型的Select控件
    /// </summary>
    /// <param name="sltName">控件名称（ID与Name）</param>
    /// <param name="aSltOptions">控件的下拉列表元素</param>
    /// <returns></returns>
    public static HtmlSelect GetHtmlSelect(string sltName, string[] aSltOptions)
    {
        Dictionary<object, object> aTmpDic = new Dictionary<object, object>();
        for (int i = 0; i < aSltOptions.Length; i++)
        {
            aTmpDic.Add(aSltOptions[i], aSltOptions[i]);
        }
        return GetHtmlSelect(sltName, aTmpDic);
    }

    /// <summary>
    /// 返回Html类型的Select控件
    /// </summary>
    /// <param name="sltName">控件名称（ID与Name）</param>
    /// <param name="aSltElement">控件的下拉列表元素</param>
    /// <returns></returns>
    public static HtmlSelect GetHtmlSelect(string sltName, Dictionary<object, object> aSltElement)
    {
        return GetHtmlSelect(sltName, aSltElement, false);
    }

    public static HtmlSelect GetHtmlSelect(string sltName, Dictionary<object, object> aSltElement, bool aRunServer)
    {
        HtmlSelect slt = new HtmlSelect();
        slt.ID = sltName;
        slt.Name = sltName;
        if (aRunServer)
        {
            slt.Attributes.Add("runat", "server");
        }
        object[] items = new object[aSltElement.Keys.Count];
        aSltElement.Keys.CopyTo(items, 0);
        for (int i = 0; i < items.Length; i++)
        {
            ListItem oListItem = new ListItem();
            oListItem.Text = items[i].ToString();
            object tmpParamValue = null;
            if (aSltElement.TryGetValue(items[i], out tmpParamValue))
            {
                oListItem.Value = Convert.ToString(tmpParamValue);
            }
            else
            {
                throw new Exception();
            }
            slt.Items.Add(oListItem);
        }
        return slt;
    }
}
