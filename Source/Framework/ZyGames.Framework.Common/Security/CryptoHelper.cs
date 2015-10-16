/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZyGames.Framework.Common.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class CryptoHelper
    {
        private const string DefaultKey = "!@#ASD12";
        /// <summary>
        /// 
        /// </summary>
        public static string Key
        {
            get
            {
                return DefaultKey;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UrlParam"></param>
        /// <returns></returns>
        public static string UrlParamUrlEncodeRun(string UrlParam)
        {
            UrlParam = UrlParam.Replace("+", "＋");
            UrlParam = UrlParam.Replace("=", "＝");
            UrlParam = UrlParam.Replace("&", "＆");
            UrlParam = UrlParam.Replace("?", "？");
            return UrlParam;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UrlParam"></param>
        /// <returns></returns>
        public static string UrlParamUrlDecodeRun(string UrlParam)
        {
            UrlParam = UrlParam.Replace("＋", "+");
            UrlParam = UrlParam.Replace("＝", "=");
            UrlParam = UrlParam.Replace("＆", "&");
            UrlParam = UrlParam.Replace("？", "?");
            return UrlParam;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, string encoding)
        {
            return MD5_Encrypt(source, string.Empty, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source)
        {
            return MD5_Encrypt(source, string.Empty, Encoding.Default);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, Encoding encoding)
        {
            return MD5_Encrypt(source, string.Empty, encoding);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="addKey"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, string addKey, Encoding encoding)
        {
            if (addKey.Length > 0)
            {
                source += addKey;
            }
            byte[] bytes = encoding.GetBytes(source);
            return MD5_Encrypt(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string MD5_Encrypt(byte[] bytes)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(bytes);
            string text = null;
            for (int i = 0; i < array.Length; i++)
            {
                string text2 = array[i].ToString("x");
                if (text2.Length == 1)
                {
                    text2 = "0" + text2;
                }
                text += text2;
            }
            return text;
        }

        /// <summary>
        /// 获取文件的MD5 Hash值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ToFileMd5Hash(string fileName)
        {
            String hashMD5 = String.Empty;
            if (File.Exists(fileName))
            {
                FileInfo fi = new FileInfo(fileName);
                string str = "";
                using (var sr = fi.OpenText())
                {
                    str = sr.ReadToEnd();
                }
                hashMD5 = ToMd5Hash(str);
            }
            return hashMD5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5Hash(string str)
        {
            return ToMd5Hash(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToMd5Hash(byte[] bytes)
        {
            String hashMD5;

            //计算文件的MD5值
            MD5 calculator = MD5.Create();
            Byte[] buffer = calculator.ComputeHash(bytes);
            calculator.Clear();
            //将字节数组转换成十六进制的字符串形式
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                stringBuilder.Append(buffer[i].ToString("x2"));
            }
            hashMD5 = stringBuilder.ToString();
            return hashMD5;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strpwd"></param>
        /// <returns></returns>
        public static string RegUser_MD5_Pwd(string strpwd)
        {
            string s = "fdjf,jkgfkl";
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(s);
            byte[] bytes2 = Encoding.Default.GetBytes(strpwd);
            byte[] array = new byte[bytes.Length + 4 + bytes2.Length];
            int i;
            for (i = 0; i < bytes2.Length; i++)
            {
                array[i] = bytes2[i];
            }
            array[i++] = 163;
            array[i++] = 172;
            array[i++] = 161;
            array[i++] = 163;
            for (int j = 0; j < bytes.Length; j++)
            {
                array[i] = bytes[j];
                i++;
            }
            byte[] array2 = mD.ComputeHash(array);
            string text = null;
            for (i = 0; i < array2.Length; i++)
            {
                string text2 = array2[i].ToString("x");
                if (text2.Length == 1)
                {
                    text2 = "0" + text2;
                }
                text += text2;
            }
            return text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DES_Encrypt(string source, string key)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(source);
            dESCryptoServiceProvider.Key = Encoding.Default.GetBytes(key);
            dESCryptoServiceProvider.IV = Encoding.Default.GetBytes(key);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = memoryStream.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                stringBuilder.AppendFormat("{0:X2}", b);
            }
            stringBuilder.ToString();
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string DES_Encrypt(string source)
        {
            return CryptoHelper.DES_Encrypt(source, DefaultKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string DES_Decrypt(string source)
        {
            return CryptoHelper.DES_Decrypt(source, DefaultKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key">8 length</param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DES_Decrypt(string source, string key, string iv = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = DefaultKey;
            }
            if (string.IsNullOrWhiteSpace(iv))
            {
                iv = key;
            }
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] array = new byte[source.Length / 2];
            for (int i = 0; i < source.Length / 2; i++)
            {
                int num = Convert.ToInt32(source.Substring(i * 2, 2), 16);
                array[i] = (byte)num;
            }
            dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(key);
            dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(iv);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            new StringBuilder();
            return Encoding.Default.GetString(memoryStream.ToArray());
        }

        /// <summary>
        /// Des encrypt
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="privateKey">16 length</param>
        /// <param name="privateIv"></param>
        /// <returns></returns>
        public static string DES16_Encrypt(string toEncrypt, string privateKey, string privateIv)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(toEncrypt);//得到需要加密的字节数组
            des.Key = Encoding.UTF8.GetBytes(privateKey);//设置密钥及密钥向量
            des.IV = Encoding.UTF8.GetBytes(privateIv);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组
            cs.Close();
            ms.Close();
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// Des decrypt
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <param name="privateKey"></param>
        /// <param name="privateIv"></param>
        /// <returns></returns>
        public static string DES16_Decrypt(string toDecrypt, string privateKey, string privateIv)
        {
            byte[] bKey = Encoding.UTF8.GetBytes(privateKey);
            byte[] bIV = Encoding.UTF8.GetBytes(privateIv);
            byte[] byteArray = Convert.FromBase64String(toDecrypt);

            string decrypt = null;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch { }
            aes.Clear();

            return decrypt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SHA1_Encrypt(string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            SHA1CryptoServiceProvider sHa1CryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] value = sHa1CryptoServiceProvider.ComputeHash(bytes);
            sHa1CryptoServiceProvider.Clear();
            string text = BitConverter.ToString(value);
            text = text.Replace("-", "");
            return text.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string HttpBase64Encode(string source)
        {
            if (source == null || source.Length == 0)
            {
                return "";
            }
            string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
            text = text.Replace("+", "~");
            text = text.Replace("/", "@");
            return text.Replace("=", "$");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string HttpBase64Decode(string source)
        {
            if (source == null || source.Length == 0)
            {
                return "";
            }
            string text = source.Replace("~", "+");
            text = text.Replace("@", "/");
            text = text.Replace("$", "=");
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
    }
}