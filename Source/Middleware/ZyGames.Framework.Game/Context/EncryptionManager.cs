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
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;

namespace ZyGames.Framework.Game.Context
{
	/// <summary>
	/// Encryption manager.
	/// </summary>
    public class EncryptionManager
    {
        private static SymmetricAlgorithm mobjCryptoService;
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Context.EncryptionManager"/> class.
		/// </summary>
        public EncryptionManager()
        {
            mobjCryptoService = new RijndaelManaged();
        }
       
       /// <summary>
       /// HMACSHs the a1 encrypt.
       /// </summary>
       /// <returns>The a1 encrypt.</returns>
       /// <param name="EncryptText">Encrypt text.</param>
       /// <param name="EncryptKey">Encrypt key.</param>
        public static string HMACSHA1Encrypt(string EncryptText, string EncryptKey)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(EncryptKey));
            byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(EncryptText));
            StringBuilder EnText = new StringBuilder();
            foreach (byte Byte in RstRes)
            {
                EnText.AppendFormat("{0:x2}", Byte);
            }

            return EnText.ToString();
            
        } 

		/// <summary>
		/// Gets the M d5_32.
		/// </summary>
		/// <returns>The M d5_32.</returns>
		/// <param name="s">S.</param>
		/// <param name="_input_charset">_input_charset.</param>
        public static string GetMD5_32(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        


        /// <summary>    
        /// 获得密钥    
        /// </summary>    
        /// <returns>密钥</returns>    
        private static byte[] GetLegalKey()
        {
            string sTemp = "";
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>    
        /// 获得初始向量IV    
        /// </summary>    
        /// <returns>初试向量IV</returns>    
        private static byte[] GetLegalIV(string key)
        {
            
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (key.Length > IVLength)
                key = key.Substring(0, IVLength);
            else if (key.Length < IVLength)
                key = key.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(key);
        }
        /// <summary>    
        /// 加密方法    
        /// </summary>    
		/// <param name="Source">待加密的串</param>    
		/// <param name="key">待加密的Key</param>    
        /// <returns>经过加密的串</returns>    
        public static string Encrypto(string Source, string key)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV(key);
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        /// <summary>    
        /// 解密方法    
        /// </summary>    
		/// <param name="Source">待解密的串</param>    
		/// <param name="key">待加密的Key</param>    
        /// <returns>经过解密的串</returns>    
        public static string Decrypto(string Source, string key)
        {
            byte[] bytIn = Convert.FromBase64String(Source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV(key);
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }






        //默认密钥向量 
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为8位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }





        //加密文件 
        private static void EncryptData(String inName, String outName, byte[] desKey, byte[] desIV)
        {
            //Create the file streams to handle the input and output files. 
            FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            //Create variables to help with read and write. 
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption. 
            long rdlen = 0;              //This is the total number of bytes written. 
            long totlen = fin.Length;    //This is the total length of the input file. 
            int len;                     //This is the number of bytes to be written at a time. 

            DES des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);

            //Read from the input file, then encrypt and write to the output file. 
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }

            encStream.Close();
            fout.Close();
            fin.Close();
        }

        //解密文件 
        private static void DecryptData(String inName, String outName, byte[] desKey, byte[] desIV)
        {
            //Create the file streams to handle the input and output files. 
            FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            //Create variables to help with read and write. 
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption. 
            long rdlen = 0; //This is the total number of bytes written. 
            long totlen = fin.Length; //This is the total length of the input file. 
            int len; //This is the number of bytes to be written at a time. 

            DES des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write);

            //Read from the input file, then encrypt and write to the output file. 
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }

            encStream.Close();
            fout.Close();
            fin.Close();
        }

        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="strPlain">明文</param>
        /// <param name="strDESKey">密钥</param>
        /// <param name="strDESIV">向量</param>
        /// <returns>密文</returns>
        public string DESEncrypt(string strPlain, string strDESKey, string strDESIV)
        {
            //把密钥转换成字节数组
            byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
            //把向量转换成字节数组
            byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
            //声明1个新的DES对象
            DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
            //开辟一块内存流
            MemoryStream msEncrypt = new MemoryStream();
            //把内存流对象包装成加密流对象
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt.CreateEncryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Write);
            //把加密流对象包装成写入流对象
            StreamWriter swEncrypt = new StreamWriter(csEncrypt);
            //写入流对象写入明文
            swEncrypt.WriteLine(strPlain);
            //写入流关闭
            swEncrypt.Close();
            //加密流关闭
            csEncrypt.Close();
            //把内存流转换成字节数组，内存流现在已经是密文了
            byte[] bytesCipher = msEncrypt.ToArray();
            //内存流关闭
            msEncrypt.Close();
            //把密文字节数组转换为字符串，并返回
            return UnicodeEncoding.Unicode.GetString(bytesCipher);
        }




        /// <summary>
        /// DES解密方法
        /// </summary>
        /// <param name="strCipher">密文</param>
        /// <param name="strDESKey">密钥</param>
        /// <param name="strDESIV">向量</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string strCipher, string strDESKey, string strDESIV)
        {
            //把密钥转换成字节数组
            byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
            //把向量转换成字节数组
            byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
            //把密文转换成字节数组
            byte[] bytesCipher = UnicodeEncoding.Unicode.GetBytes(strCipher);
            //声明1个新的DES对象
            DESCryptoServiceProvider desDecrypt = new DESCryptoServiceProvider();
            //开辟一块内存流，并存放密文字节数组
            MemoryStream msDecrypt = new MemoryStream(bytesCipher);
            //把内存流对象包装成解密流对象
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, desDecrypt.CreateDecryptor(bytesDESKey, bytesDESIV), CryptoStreamMode.Read);
            //把解密流对象包装成读出流对象
            StreamReader srDecrypt = new StreamReader(csDecrypt);
            //明文=读出流的读出内容
            string strPlainText = srDecrypt.ReadLine();
            //读出流关闭
            srDecrypt.Close();
            //解密流关闭
            csDecrypt.Close();
            //内存流关闭
            msDecrypt.Close();
            //返回明文
            return strPlainText;
        }
    }


}