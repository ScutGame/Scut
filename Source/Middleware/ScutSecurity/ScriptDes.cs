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
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ScutSecurity
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptDes
    {

        /// <summary>
        /// 8位的key
        /// </summary>
        private const string DefaultKey = "!7@8#S~D";

        /// <summary>
        /// 8位的IV
        /// </summary>
        private const string DefaultIV = "5=8S!K~L";

        private const string DesPreChar = "??DES??";

        public string Encode(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(source);
            dESCryptoServiceProvider.Key = Encoding.Default.GetBytes(DefaultKey);
            dESCryptoServiceProvider.IV = Encoding.Default.GetBytes(DefaultIV);
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
            return DesPreChar + stringBuilder.ToString();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public string DecodeCallback(string source, string ext)
        {
            if (string.IsNullOrEmpty(source) || !source.StartsWith(DesPreChar))
            {
                return source;
            }
            source = source.Substring(DesPreChar.Length);
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] array = new byte[source.Length / 2];
            for (int i = 0; i < source.Length / 2; i++)
            {
                int num = Convert.ToInt32(source.Substring(i * 2, 2), 16);
                array[i] = (byte)num;
            }
            dESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(DefaultKey);
            dESCryptoServiceProvider.IV = Encoding.UTF8.GetBytes(DefaultIV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }
    }
}
