using System;
using System.Data;
//using System.Configuration;
//using System.Web;
using System.Web.Security;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;

namespace CitGame
{
    /// <summary>
    /// func 的摘要说明
    /// </summary>
    public class func
    {
        public const string cst_enter = "\r\n";
        public func()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static void savetofile(string strValue, string folder)
        {
            try
            {
                string savedate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                string folderpath = AppDomain.CurrentDomain.BaseDirectory + "\\ErrLog";
                DirectoryInfo dirinfo = new DirectoryInfo(folderpath);
                if (!dirinfo.Exists)
                {
                    dirinfo.Create();
                }

                DateTime CurDt = DateTime.Now;
                string datefolderName = CurDt.ToString("yyyy_MM_dd");
                folderpath += "\\" + datefolderName;
                dirinfo = new DirectoryInfo(folderpath);
                if (!dirinfo.Exists)
                {
                    dirinfo.Create();
                }

                //if (folder != "")
                //{
                //    folderpath = folderpath + "\\" + folder;
                //    dirinfo = new DirectoryInfo(folderpath);
                //    if (!dirinfo.Exists)
                //    {
                //        dirinfo.Create();
                //    }
                //}
                string fileName = "";
                if (folder != "")
                {
                    fileName = folder;
                }
                else
                {
                    fileName = CurDt.ToString("yyyyMMdd");
                }
                

                string filepath = folderpath + "\\" + fileName + ".txt";
                FileStream fst = new FileStream(filepath, FileMode.Append);
                StreamWriter swt = new StreamWriter(fst, System.Text.Encoding.GetEncoding("GB2312"));
                swt.WriteLine(strValue);
                swt.Close();
                fst.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 验证输入的性别值是否合法
        /// </summary>
        /// <param name="aSexType">待验证的性别值</param>
        /// <returns>如果输入的性别值，不合法，则默认为男</returns>
        /*public static byte CompareSexType(byte aSexType)
        {
            if (aSexType != Convert.ToByte(SexType.Man) && aSexType != Convert.ToByte(SexType.Woman))
            {
                return Convert.ToByte(SexType.Man);
            }
            return aSexType;
        }*/

        /// <summary>
        /// 生成输入字符串的MD5值
        /// </summary>
        /// <param name="aSource">字符串</param>
        /// <returns>MD5值</returns>
        public static string DoMd5(string aSource)
        {
            string ret = "";
            MD5 md5 = MD5.Create();
            byte[] bs = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(aSource));
            for (int i = 0; i < bs.Length; i++)
            {
                ret += bs[i].ToString("X2");
            }

            md5.Clear();
            return ret.ToLower();
        }
    }
}