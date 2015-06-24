using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ContractTools.WebApp.Base;
using ContractTools.WebApp.Model;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ZyGames.Framework.RPC.IO;

namespace ContractTools.WebApp
{
    public class ZipFileInfo
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public byte[] Data { get; set; }

        public byte[] ReadBytes(bool isBom)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return Data;
            }
            UTF8Encoding WithBOM = new System.Text.UTF8Encoding(isBom);
            if (isBom)
            {
                return BufferUtils.MergeBytes(new byte[] { 0xEF, 0xBB, 0xBF }, WithBOM.GetBytes(Content));
            }
            return WithBOM.GetBytes(Content);
        }
    }

    public class BasePage : System.Web.UI.Page
    {
        public static KeyValuePair<int, string>[][] FieldTypeMaps = new[]
        {
            new []{
               new KeyValuePair<int, string>(-1,"Password"), 
               new KeyValuePair<int, string>(1,"Int"), 
               new KeyValuePair<int, string>(2,"String"), 
               new KeyValuePair<int, string>(3,"Short"), 
               new KeyValuePair<int, string>(4,"Byte"), 
               new KeyValuePair<int, string>(8,"Long"), 
               new KeyValuePair<int, string>(9,"Bool")
            },
             new []{
               new KeyValuePair<int, string>(1,"Int"), 
               new KeyValuePair<int, string>(2,"String"), 
               new KeyValuePair<int, string>(3,"Short"), 
               new KeyValuePair<int, string>(4,"Byte"), 
               new KeyValuePair<int, string>(5,"Record"), 
               new KeyValuePair<int, string>(6,"End"), 
               new KeyValuePair<int, string>(7,"Void"), 
               new KeyValuePair<int, string>(8,"Long"), 
               new KeyValuePair<int, string>(9,"Bool"), 
               new KeyValuePair<int, string>(10,"Float"), 
               new KeyValuePair<int, string>(11,"Double"), 
               new KeyValuePair<int, string>(12,"Date"), 
               new KeyValuePair<int, string>(13,"UInt"), 
               new KeyValuePair<int, string>(14,"UShort"), 
               new KeyValuePair<int, string>(15,"ULong"),
               new KeyValuePair<int, string>(16,"SigleRecord"), 
            }
        };
        protected void Alert(string msg, string url)
        {
            Response.Write(string.Format("<script language=javascript>alert('{0}');location.href ='{1}';</script>", msg, url));
        }

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

        public void SaveAsAttachment(string txtContent, string filename, bool isBom = false)
        {
            Response.AppendHeader("Pragma", "No-cache");
            Response.AppendHeader("Cache-Control", "No-cache");
            Response.Expires = 0;
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Encoding", "UTF8");
            Response.ContentEncoding = new System.Text.UTF8Encoding(isBom);
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);

            UTF8Encoding WithBOM = new System.Text.UTF8Encoding(isBom);
            byte[] buffer = isBom
                ? BufferUtils.MergeBytes(new byte[] { 0xEF, 0xBB, 0xBF }, WithBOM.GetBytes(txtContent))//UTF-8 Head
                : WithBOM.GetBytes(txtContent);

            Response.OutputStream.Write(buffer, 0, buffer.Length);
            Response.End();
        }

        public void SaveAsAttachment(string fileName, IEnumerable<ZipFileInfo> fileEnumerable, bool isBom = false)
        {
            byte[] buffer;
            if (TryZipFileMain(fileEnumerable, out buffer, isBom))
            {
                Response.AppendHeader("Pragma", "No-cache");
                Response.AppendHeader("Cache-Control", "No-cache");
                Response.Expires = 0;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Encoding", "UTF8");
                Response.ContentEncoding = new System.Text.UTF8Encoding(isBom);
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.AddHeader("Content-Length", buffer.Length.ToString());
                Response.OutputStream.Write(buffer, 0, buffer.Length);
                Response.End();
            }
        }

        public bool TryZipFileMain(IEnumerable<ZipFileInfo> fileEnumerable, out byte[] outStream, bool isBom, int zipLevel = 9)
        {
            bool result = true;
            outStream = null;
            using (MemoryStream ms = new MemoryStream())
            {
                ZipOutputStream s = new ZipOutputStream(ms);
                Crc32 crc = new Crc32();
                //压缩级别
                s.SetLevel(zipLevel); // 0 - store only to 9 - means best compression
                try
                {
                    foreach (var file in fileEnumerable)
                    {
                        byte[] buffer = file.ReadBytes(isBom);
                        //建立压缩实体
                        ZipEntry entry = new ZipEntry(file.Name); //原文件名
                        //时间
                        entry.DateTime = DateTime.Now;
                        //空间大小
                        entry.Size = buffer.Length;
                        entry.IsUnicodeText = true;
                        crc.Reset();
                        crc.Update(buffer);
                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);
                        s.Write(buffer, 0, buffer.Length);
                    }
                }
                catch
                {
                    result = false;
                }
                finally
                {
                    s.Finish();
                    if (result && ms.CanRead)
                    {
                        outStream = new byte[ms.Length];
                        var buffer = ms.GetBuffer();
                        if (buffer.Length >= outStream.Length)
                        {
                            System.Buffer.BlockCopy(buffer, 0, outStream, 0, outStream.Length);
                        }
                    }
                    s.Close();
                }
            }
            return result;
        }

        public static void GetParamInfo(int slnId, int contractId, int versionId, out List<ParamInfoModel> requestParams, out List<ParamInfoModel> responseParams)
        {
            var paramList = DbDataLoader.GetParamInfo(slnId, contractId, versionId);
            var pairs = paramList.GroupBy(t => t.ParamType);
            requestParams = new List<ParamInfoModel>();
            responseParams = new List<ParamInfoModel>();
            foreach (var pair in pairs)
            {
                switch (pair.Key)
                {
                    case 1:
                        requestParams = pair.ToList();
                        break;
                    case 2:
                        responseParams = pair.ToList();
                        break;
                    default:
                        break;
                }
            }
        }
    }

}