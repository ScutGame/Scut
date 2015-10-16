using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.IO;
namespace DirCenter.Model
{
    public class HttpHelp
    {
        private static HttpHelp inst;
        static public HttpHelp getInst()
        {
            if (inst == null)
            {
                inst = new HttpHelp();
            }

            return inst;
        }
        public string URL { get; set; }

		public string httpPost(String urlStr, String postData, Encoding encoding)
		{
			HttpWebResponse resp = null;
			try
			{
				Uri uri = new Uri(urlStr);
				string postdata = postData;

				byte[] bytes = encoding.GetBytes(postdata);
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
				req.ProtocolVersion = System.Net.HttpVersion.Version11;
				req.KeepAlive = false;
				req.ContentType = "application/x-www-form-urlencoded";
				req.Method = "POST";
				req.Timeout = 3000;
				req.ContentLength = bytes.Length;
				Stream os = req.GetRequestStream();
				os.Write(bytes, 0, bytes.Length); //Push it out there
				os.Close();
				resp = (HttpWebResponse)req.GetResponse();
				if (resp == null) return null;
				StreamReader sr = new StreamReader(resp.GetResponseStream(), encoding);

				string str = sr.ReadToEnd().Trim();

				sr.Close();

				return str;
			}
			catch (Exception ex)
			{
				//new BaseLog().SaveLog(ex);
			}
			finally
			{
				if (null != resp)
				{
					resp.Close();
				}
			}
			return "";
		}
    }
}
