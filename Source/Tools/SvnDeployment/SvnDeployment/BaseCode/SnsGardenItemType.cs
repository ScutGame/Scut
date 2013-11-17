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

/// <summary>
/// SnsGardenItemType 的摘要说明
/// </summary>
public class SnsGardenItemType
{
    private string value;
    public string Value
    {
        get { return value; }
    }

    private string text;
    public string Text 
    {
        get { return text; }
    }

    private List<SnsGardenItemType> itemSort;
    public List<SnsGardenItemType> ItemSort
    {
        get
        {
            if (itemSort == null)
                itemSort = new List<SnsGardenItemType>();
            return itemSort;
        }
    } 

    public SnsGardenItemType()
	{
	}

    public void InitData(string value, string text)
    {
        this.value = value;
        this.text = text;
    }
}
