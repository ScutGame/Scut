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
using System.Xml;
using ZyGames.Core.Web;
using System.Text;
using System.Net;
using System.IO;
using ZyGames.GameService.BaseService.LogService;

namespace CitGame
{
    /// <summary>  
    /// 提供web处理方法的类  
    /// </summary>  
    public class WebProcess
    {
        private const string ConstFolderName = "WebProcess";
        /// <summary>  
        /// 通过GET方式获取页面的方法  
        /// </summary>  
        /// <param name="urlString">请求的URL</param>  
        public static string GetHtmlFromGet(string urlString)
        {
            return GetHtmlFromGet(urlString, Encoding.UTF8);
        }

        /// <summary>  
        /// 通过GET方式获取页面的方法  
        /// </summary>  
        /// <param name="urlString">请求的URL</param>  
        /// <param name="encoding">页面编码</param>  
        public static string GetHtmlFromGet(string urlString, Encoding encoding)
        {
            return GetHtmlFromGet(urlString, encoding, "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 1.1.4322; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)");
        }

        /// <summary>  
        /// 通过GET方式获取页面的方法  
        /// </summary>  
        /// <param name="urlString">请求的URL</param>  
        /// <param name="encoding">页面编码</param>  
        /// <param name="sAgent">User-agentHTTP 标头的值</param>
        public static string GetHtmlFromGet(string urlString, Encoding encoding, string sAgent)
        {
            string htmlString = string.Empty;
            try
            {
                HttpWebRequest request = WebRequest.Create(urlString) as HttpWebRequest;
                request.Referer = urlString;
                request.UserAgent = sAgent;
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, encoding))
                        {
                            htmlString = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BaseLog olog = new BaseLog(ConstFolderName);
                olog.SaveLog(ex); 
            }
            return htmlString;
        }

        /// <summary>  
        /// 提供通过POST方法获取页面的方法  
        /// </summary>  
        /// <param name="urlString">请求的URL</param>          
        /// <param name="postDataString">POST数据</param>  
        public static string GetHtmlFromPost(string urlString, string postDataString)
        {
            return GetHtmlFromPost(urlString, postDataString, Encoding.UTF8);
        }

        /// <summary>  
        /// 提供通过POST方法获取页面的方法  
        /// </summary>  
        /// <param name="urlString">请求的URL</param>          
        /// <param name="postDataString">POST数据</param>  
        /// <param name="encoding">页面使用的编码</param>  
        public static string GetHtmlFromPost(string urlString, string postDataString, Encoding encoding)
        {
            string htmlString = string.Empty;
            try
            {
                byte[] postDataByte = encoding.GetBytes(postDataString);

                HttpWebRequest request = WebRequest.Create(urlString) as HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postDataByte.Length;
                using (Stream inputStream = request.GetRequestStream())
                {
                    inputStream.Write(postDataByte, 0, postDataByte.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream outputStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(outputStream, encoding))
                        {
                            htmlString = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                BaseLog olog = new BaseLog(ConstFolderName);
                olog.SaveLog(ex);
            }
            return htmlString;
        }

        /// <summary>
        /// 提供通过POST方法获取页面的方法  
        /// </summary>
        /// <param name="urlString">请求的URL</param>
        /// <param name="postXMLDoc">POST数据  XmlDocument</param>
        public static XmlDocument PostXMLTransaction(string urlString, XmlDocument postXMLDoc)
        {
            return PostXMLTransaction(urlString, postXMLDoc, Encoding.UTF8);
        }

        /// <summary>
        /// 提供通过POST方法获取页面的方法  
        /// </summary>
        /// <param name="urlString">请求的URL</param>
        /// <param name="postXMLDoc">POST数据  XmlDocument</param>
        /// <param name="encoding">页面使用的编码</param>
        public static XmlDocument PostXMLTransaction(string urlString, XmlDocument postXMLDoc, Encoding encoding)
        {
            string htmString = GetHtmlFromPost(urlString, postXMLDoc.InnerXml, encoding);
            if (string.IsNullOrEmpty(htmString))
                return null;
            XmlDocument XMLResponse = new XmlDocument();
            XMLResponse.LoadXml(htmString);
            return XMLResponse;
        }
    }
}
