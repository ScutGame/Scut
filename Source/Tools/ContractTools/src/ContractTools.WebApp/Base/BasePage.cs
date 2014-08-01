using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContractTools.WebApp.Model;

namespace ContractTools.WebApp
{
    public class BasePage : System.Web.UI.Page
    {
        private string GetCookesKey(string key)
        {
            return string.Format("__Contract{0}", key);
        }
        protected void SetCookies(string key, string value)
        {
            Response.Cookies[GetCookesKey(key)].Value = value;
            Response.Cookies[GetCookesKey(key)].Expires = DateTime.Now.AddMonths(1);
        }
        protected string GetCookies(string key)
        {
            if (Request.Cookies[GetCookesKey(key)] != null)
            {
                return Request.Cookies[GetCookesKey(key)].Value;
            }
            else
            {
                return string.Empty;
            }
        }
        protected string ConvertParamTypeName(int paramType)
        {
            string stu = string.Empty;
            if (paramType == 1)
            {
                stu = ParamType.Request;
            }
            else
            {
                stu = ParamType.Response;
            }
            return stu;
        }

        public string ToHtml(object str)
        {
            return (str ?? "").ToString().Replace("\r\n", "<br>").Replace("\n", "<br>").Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
        }
    }

}