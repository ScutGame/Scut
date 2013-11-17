using System;
using System.Diagnostics;

namespace ZyGames.OA.WatchService.BLL.Tools
{
    public static class CommandExeHelper
    {
        public static string CombineDir(string str1, string str2)
        {
            string val = @"\";
            str1 = str1 == null ? "" : str1.Replace("/", val).Replace(@"\\", val);
            str2 = str2 == null ? "" : str2.Replace("/", val).Replace(@"\\", val);
            str1 = str1.EndsWith(val) ? str1 : str1 + val;
            str2 = str2.Trim('\\');
            return str1 + str2;
        }

        public static string CombineUrl(string str1, string str2)
        {
            string val = @"\";
            str1 = str1 == null ? "" : str1.Replace(val, "/").Replace("//", val);
            str2 = str2 == null ? "" : str2.Replace(val, "/");
            str1 = str1.EndsWith("/") ? str1 : str1 + "/";
            str2 = str2.Trim('/');
            return str1 + str2;
        }

        public static string Run(string fileName, string arguments)
        {
            return Run(fileName, arguments, null);
        }

        public static string Run(string fileName, string arguments, string directory)
        {
            //实例化一个进程类
            Process procExe = new Process();

            //获得系统信息，使用的是 systeminfo.exe 这个控制台程序
            //procExe.StartInfo.FileName = CurrentContext.Server.MapPath("/") + strSvnExePath;
            if (!string.IsNullOrEmpty(directory))
            {
                procExe.StartInfo.WorkingDirectory = directory;
            }
            procExe.StartInfo.FileName = fileName;

            if (!string.IsNullOrEmpty(arguments))
            {
                procExe.StartInfo.Arguments = arguments;
            }

            //将procExe的标准输入和输出全部重定向到.NET的程序里

            procExe.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常
            procExe.StartInfo.RedirectStandardInput = true; //标准输入
            procExe.StartInfo.RedirectStandardOutput = true; //标准输出
            procExe.StartInfo.RedirectStandardError = true;

            //不显示命令行窗口界面
            procExe.StartInfo.CreateNoWindow = true;
            procExe.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            var loger = new ZyGames.GameService.BaseService.LogService.BaseLog();
            try
            {
                procExe.Start(); //启动进程
            }
            catch (Exception Ex)
            {
                loger.SaveLog(Ex);
                return Ex.Message;
            }

            //获取输出
            //需要说明的：此处是指明开始获取，要获取的内容，
            //只有等进程退出后才能真正拿到
            string strOutput = procExe.StandardOutput.ReadToEnd() + procExe.StandardError.ReadToEnd();
            loger.SaveLog(strOutput);

            procExe.WaitForExit();//等待控制台程序执行完成
            procExe.Close();//关闭该进程
            procExe.Dispose();
            return strOutput;
        }
    }
}
