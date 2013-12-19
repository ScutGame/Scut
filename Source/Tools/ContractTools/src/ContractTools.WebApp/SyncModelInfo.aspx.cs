using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Model;

namespace ContractTools.WebApp
{
    public partial class SyncModelInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsCallback)
            {
                LoadFile();
            }
        }

        private void LoadFile()
        {
            string fileName = GetSchemaFileName();
            if (File.Exists(fileName))
            {
                txtContent.Text = File.ReadAllText(fileName);
            }
        }

        private string GetSchemaFileName()
        {
            int slnId = Request["slnID"].ToInt();
            string path = Server.MapPath(string.Format("~/Schema/{0}", slnId));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, "ScutSchemaInfo.lua");
        }

        protected void BtnUp_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    string fileName = FileUpload1.FileName ?? "";
                    string ext = Path.GetExtension(fileName).ToLower();
                    if (ext == ".dll")
                    {
                        //string savePath = Server.MapPath("~/upload/");//指定上传文件在服务器上的保存路径
                        ////检查服务器上是否存在这个物理路径，如果不存在则创建
                        //if (!Directory.Exists(savePath))
                        //{
                        //    Directory.CreateDirectory(savePath);
                        //}
                        //savePath = Path.Combine(savePath, fileName);
                        //FileUpload1.SaveAs(savePath);
                        //int slnId = Request["slnID"].ToInt();
                        //GeneralSyncSchema(savePath, Server.MapPath("~/Schema/" + slnId));
                    }
                    else if (ext == ".lua")
                    {
                        fileName = GetSchemaFileName();
                        FileUpload1.SaveAs(fileName);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('上传文件失败，文件类型不被支持！')</script>");
                    }

                }
                catch (Exception ex)
                {
                    TraceLog.WriteError("GeneralSyncSchema:{0}", ex);
                    ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('上传文件出错！')</script>");
                }
            }
        }


        private void GeneralSyncSchema(string assemblyPath, string path)
        {
            string fileName = "ScutSchemaInfo.lua";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var assembly = Assembly.LoadFrom(assemblyPath);
            EntitySchemaSet.LoadAssembly(assembly);
            string content = EntitySchemaSet.ExportSync(fileName);
            using (var sw = File.CreateText(Path.Combine(path, fileName)))
            {
                sw.Write(content);
                sw.Flush();
            }
            txtContent.Text = content;
        }

        protected void btnDown_Click(object sender, EventArgs e)
        {
            string filename = GetSchemaFileName();
            Response.ContentType = "text/plain";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AppendHeader("Content-Encoding", "UTF-8");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filename));
            Response.WriteFile(filename);
            Response.End();
        }

    }
}