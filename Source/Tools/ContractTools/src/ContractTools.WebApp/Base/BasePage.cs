using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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

        public byte[] ReadBytes()
        {
            if (string.IsNullOrEmpty(Content))
            {
                return Data;
            }
            return Encoding.UTF8.GetBytes(Content);
        }
    }

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

        public void SaveAsAttachment(string txtContent, string filename)
        {
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AppendHeader("Content-Encoding", "UTF8");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            byte[] buffer = Encoding.UTF8.GetBytes(txtContent);
            Response.OutputStream.Write(buffer, 0, buffer.Length);
            Response.End();
        }

        public void SaveAsAttachment(string fileName, IEnumerable<ZipFileInfo> fileEnumerable)
        {
            byte[] buffer;
            if (TryZipFileMain(fileEnumerable, out buffer))
            {
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = Encoding.UTF8;
                Response.AppendHeader("Content-Encoding", "UTF8");
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.AddHeader("Content-Length", buffer.Length.ToString());
                Response.OutputStream.Write(buffer, 0, buffer.Length);
                Response.End();
            }
        }

        public bool TryZipFileMain(IEnumerable<ZipFileInfo> fileEnumerable, out byte[] outStream, int zipLevel = 9)
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
                        byte[] buffer = file.ReadBytes();
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
    }

}