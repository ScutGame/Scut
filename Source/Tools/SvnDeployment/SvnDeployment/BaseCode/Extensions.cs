using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

public static class Extensions
{
    /// <summary>
    /// 获取枚举变量值的 Description 属性
    /// </summary>
    /// <param name="obj">枚举变量</param>
    /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
    public static string GetDescription(this object obj)
    {
        return GetDescription(obj, false);
    }

    /// <summary>
    /// 获取枚举变量值的 Description 属性
    /// </summary>
    /// <param name="obj">枚举变量</param>
    /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
    /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
    public static string GetDescription(this object obj, bool isTop)
    {
        if (obj == null)
        {
            return string.Empty;
        }
        try
        {
            Type _enumType = obj.GetType();
            DescriptionAttribute dna = null;
            if (isTop)
            {
                dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
            }
            else
            {
                FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
                dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            }
            if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                return dna.Description;
        }
        catch
        {
        }
        return obj.ToString();
    }

    public static List<ListItem> GetEnumList(Type enumType, bool allAllOption)
    {
        if (enumType.IsEnum == false)
        {
            return null;
        }
        List<ListItem> list = new List<ListItem>();
        if (allAllOption == true)
        {
            list.Add(new ListItem("全部", string.Empty));
        }

        Type typeDescription = typeof(DescriptionAttribute);
        System.Reflection.FieldInfo[] fields = enumType.GetFields();
        string strText = string.Empty;
        string strValue = string.Empty;
        foreach (FieldInfo field in fields)
        {
            if (field.IsSpecialName) continue;
            strValue = field.GetRawConstantValue().ToString();
            object[] arr = field.GetCustomAttributes(typeDescription, true);
            if (arr.Length > 0)
            {
                strText = (arr[0] as DescriptionAttribute).Description;
            }
            else
            {
                strText = field.Name;
            }

            list.Add(new ListItem(strText, strValue));
        }

        return list;
    }

    public static List<ListItem> GetNameEnumList(Type enumType, bool allAllOption)
    {
        if (enumType.IsEnum == false)
        {
            return null;
        }
        List<ListItem> list = new List<ListItem>();
        if (allAllOption == true)
        {
            list.Add(new ListItem("全部", string.Empty));
        }

        Type typeDescription = typeof(DescriptionAttribute);
        System.Reflection.FieldInfo[] fields = enumType.GetFields();
        string strText = string.Empty;
        string strValue = string.Empty;
        foreach (FieldInfo field in fields)
        {
            if (field.IsSpecialName) continue;
            strValue = field.Name;
            object[] arr = field.GetCustomAttributes(typeDescription, true);
            if (arr.Length > 0)
            {
                strText = (arr[0] as DescriptionAttribute).Description;
            }
            else
            {
                strText = field.Name;
            }

            list.Add(new ListItem(strText, strValue));
        }
        return list;
    }      
}