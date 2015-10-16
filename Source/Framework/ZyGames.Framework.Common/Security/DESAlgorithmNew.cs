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
using System.Text;

namespace ZyGames.Framework.Common.Security
{
    /// <summary>
    /// DESAlgorithmNew 的摘要说明
    /// </summary>
    public class DESAlgorithmNew
    {
        private DesCopy desCopy = null;
        /// <summary>
        /// 初始化加解密数据结构
        /// </summary>
        public DESAlgorithmNew()
        {
            //subKey[0] = new byte[48];
            for (int i = 0; i < subKey.Length; i++)
            {
                subKey[i] = new byte[48];
            }
            this.desCopy = new DesCopy();
        }

        //private static DESAlgorithmNew instance = null;
        private static byte[] PC1_Table = { 57, 49, 41, 33, 25, 17, 9, 1, 58,
			50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52,
			44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14,
			6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };

        private static char[] HEXCODE = { '0', '1', '2', '3', '4', '5', '6',
			'7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        private byte[] K = new byte[64];
        private byte[] ROTATEL_TMP = new byte[256];
        private byte[] TRANSFORM_TMP = new byte[256];
        private byte[] F_FUNC_MR = new byte[48];
        private byte[] DES_RUN_TMP = new byte[32];
        private byte[] DES_RUN_M = new byte[64];
        private static byte ENCRYPT = 0;
        private static byte DECRYPT = 1;
        private byte[][] subKey = new byte[16][];//

        private static byte[] LOOP_TABLE = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2,
			2, 2, 2, 2, 1 };

        private static byte[] PC2_TABLE = { 14, 17, 11, 24, 1, 5, 3, 28, 15,
			6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31,
			37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42,
			50, 36, 29, 32 };

        private static byte[] IP_TABLE = { 58, 50, 42, 34, 26, 18, 10, 2, 60,
			52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56,
			48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43,
			35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39,
			31, 23, 15, 7 };

        private static byte[] E_TABLE = { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8,
			9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19,
			20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30,
			31, 32, 1 };

        // 32-bit permutation function P used on the output of the S-boxes
        private static byte[] P_TABLE = { 16, 7, 20, 21, 29, 12, 28, 17, 1,
			15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30,
			6, 22, 11, 4, 25 };

        private static byte[] S_BOXSINGLE = {
			// S1
			14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, 0, 15, 7, 4,
			14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8, 4, 1, 14, 8, 13, 6, 2, 11,
			15, 12, 9,
			7,
			3,
			10,
			5,
			0,
			15,
			12,
			8,
			2,
			4,
			9,
			1,
			7,
			5,
			11,
			3,
			14,
			10,
			0,
			6,
			13,
			// S2
			15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, 3, 13, 4, 7,
			15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5, 0, 14, 7, 11, 10, 4, 13,
			1, 5, 8, 12, 6, 9, 3,
			2,
			15,
			13,
			8,
			10,
			1,
			3,
			15,
			4,
			2,
			11,
			6,
			7,
			12,
			0,
			5,
			14,
			9,
			// S3
			10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, 13, 7, 0, 9,
			3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1, 13, 6, 4, 9, 8, 15, 3, 0,
			11, 1, 2, 12, 5, 10, 14, 7, 1,
			10,
			13,
			0,
			6,
			9,
			8,
			7,
			4,
			15,
			14,
			3,
			11,
			5,
			2,
			12,
			// S4
			7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, 13, 8, 11, 5,
			6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, 10, 6, 9, 0, 12, 11, 7, 13,
			15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6,
			10,
			1,
			13,
			8,
			9,
			4,
			5,
			11,
			12,
			7,
			2,
			14,
			// S5
			2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, 14, 11, 2,
			12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, 4, 2, 1, 11, 10, 13, 7,
			8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1, 14, 2,
			13,
			6,
			15,
			0,
			9,
			10,
			4,
			5,
			3,
			// S6
			12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, 10, 15, 4, 2,
			7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, 9, 14, 15, 5, 2, 8, 12, 3,
			7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9, 5, 15, 10, 11, 14,
			1,
			7,
			6,
			0,
			8,
			13,
			// S7
			4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, 13, 0, 11, 7,
			4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6, 1, 4, 11, 13, 12, 3, 7, 14,
			10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15,
			14, 2,
			3,
			12,
			// S8
			13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, 1, 15, 13, 8,
			10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2, 7, 11, 4, 1, 9, 12, 14, 2,
			0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0,
			3, 5, 6, 11 };

        private static byte[] IPR_TABLE = { 40, 8, 48, 16, 56, 24, 64, 32,
			39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37,
			5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3,
			43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41,
			9, 49, 17, 57, 25 };


   

        private byte[] string2Byte(String input)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(input);
            byte[] result;
            int len = byteArray.Length;
            if (len % 8 != 0)
            {
                int k = 8 - len % 8;
                result = new byte[len + k];
                byteArray.CopyTo(result, 0);
            }
            else
            {
                result = byteArray;
            }
            return result;
        }

      

        /// <summary>
        /// 对dataIn使用key进行加密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataIn"></param>
        /// <returns></returns>
        public char[] encode(String key, String dataIn)
        {
            generateKey(Encoding.Default.GetBytes(key));
            int pos = 0;
            byte[] dataTmp2 = string2Byte(dataIn);
            int len = dataTmp2.Length;
            while (pos < len)
            {
                desRun(dataTmp2, pos, dataTmp2, pos, ENCRYPT);
                pos += 8;
            }
            return transCode(dataTmp2);
        }

        /// <summary>
        /// 对data数据使用key进行解密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] decode(String key, char[] data)
        {
            generateKey(Encoding.Default.GetBytes(key));
            int pos = 0;
            byte[] temp = deTransCode(data);
            int len = temp.Length;
            while (pos < len)
            {
                desRun(temp, pos, temp, pos, DECRYPT);
                pos += 8;
            }
            return temp;
        }

        private String replaceAll(String src, String regex, String replacement)
        {
            int i = 0;
            int j = 0;

            StringBuilder buffer = new StringBuilder();
            while ((j = src.IndexOf(regex, i)) >= 0)
            {
                buffer.Append(src.Substring(i, j));
                buffer.Append(replacement);
                i = j + regex.Length;
            }
            buffer.Append(src.Substring(i));
            return buffer.ToString();
        }

        private void generateKey(byte[] Key)
        {
            byte[] newKey = new byte[8];
            this.desCopy.Copy(Key, 0, ref newKey, 0, Math.Min(Key.Length, newKey.Length));
            byte2Bit(K, 0, newKey, 0, 64);

            transform(K, 0, K, 0, PC1_Table, 56);
            for (int i = 0; i < 16; i++)
            {
                rotateL(K, 0, 28, LOOP_TABLE[i]);
                rotateL(K, 28, 28, LOOP_TABLE[i]);
                byte[] secSubKey = (byte[])subKey[i];
                transform(secSubKey, 0, K, 0, PC2_TABLE, 48);
            }
        }

        private void xor(byte[] inA, int inAIndex, byte[] inB, int inBIndex, int len)
        {
            for (int i = 0; i < len; i++)
                inA[inAIndex + i] ^= inB[inBIndex + i];
        }

        private void sBox(byte[] outby, int outIndex, byte[] inbte)
        {
            int OutIndex = 0;
            int InIndex = 0;
            for (int i = 0, j, k; i < 8; i++)
            {
                j = (inbte[InIndex + 0] << 1) + inbte[InIndex + 5];
                k = (inbte[InIndex + 1] << 3) + (inbte[InIndex + 2] << 2)
                        + (inbte[InIndex + 3] << 1) + inbte[InIndex + 4];
                byte2Bit(outby, outIndex + OutIndex, S_BOXSINGLE, i * 4 * 16 + j * 16
                        + k, 4);
                OutIndex += 4;
                InIndex += 6;
            }
        }

        private void function(byte[] inbte, int inIndex, byte[] ki)
        {
            transform(F_FUNC_MR, 0, inbte, inIndex, E_TABLE, 48);
            xor(F_FUNC_MR, 0, ki, 0, 48);
            sBox(inbte, inIndex, F_FUNC_MR);
            transform(inbte, inIndex, inbte, inIndex, P_TABLE, 32);
        }

        private void byte2Bit(byte[] outb, int outIndex, byte[] inbte, int inIndex,
                int bits)
        {
            for (int i = 0; i < bits; i++)
            {
                outb[outIndex + i] = (byte)((inbte[inIndex + i / 8] >> (i % 8)) & 1);
            }
        }

        private void bit2Byte(byte[] outb, int outIndex, byte[] inbte, int inIndex,
                int bits)
        {
            for (int i = 0; i < (bits + 7) / 8; i++)
            {
                outb[outIndex + i] = 0;
            }
            for (int i = 0; i < bits; i++)
                outb[outIndex + i / 8] |= Convert.ToByte(inbte[inIndex + i] << (i % 8));
        }

        private void transform(byte[] outb, int outIndex, byte[] inb, int inIndex, byte[] Table, int len)
        {
            for (int i = 0; i < len; i++)
                TRANSFORM_TMP[i] = inb[inIndex + Table[i] - 1];

            this.desCopy.Copy(TRANSFORM_TMP, 0, ref outb, outIndex, len);
        }

        private void rotateL(byte[] inb, int startIndex, int len, int loop)
        {
            //System.arraycopy(inb, startIndex, ROTATEL_TMP, 0, loop);

            this.desCopy.Copy(inb, startIndex, ref ROTATEL_TMP, 0, loop);

            for (int i = 0; i < len - loop; i++)
            {
                inb[startIndex + i] = inb[startIndex + i + loop];
            }
            //System.arraycopy(ROTATEL_TMP, 0, inb, startIndex + len - loop, loop);
            this.desCopy.Copy(ROTATEL_TMP, 0, ref inb, startIndex + len - loop, loop);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outb"></param>
        /// <param name="outIndex"></param>
        /// <param name="inb"></param>
        /// <param name="inIndex"></param>
        /// <param name="type"></param>
        private void desRun(byte[] outb, int outIndex, byte[] inb, int inIndex, byte type)
        {
            byte2Bit(DES_RUN_M, 0, inb, inIndex, 64);
            transform(DES_RUN_M, 0, DES_RUN_M, 0, IP_TABLE, 64);

            if (type == ENCRYPT)
            {
                for (int i = 0; i < 16; i++)
                {
                    this.desCopy.Copy(DES_RUN_M, 32, ref DES_RUN_TMP, 0, 32);
                    Byte[] tmpSubKey = (Byte[])subKey[i];
                    function(DES_RUN_M, 32, tmpSubKey);
                    xor(DES_RUN_M, 32, DES_RUN_M, 0, 32);
                    this.desCopy.Copy(DES_RUN_TMP, 0, ref DES_RUN_M, 0, 32);
                }
            }
            else
            {
                for (int i = 15; i >= 0; i--)
                {
                    this.desCopy.Copy(DES_RUN_M, 0, ref DES_RUN_TMP, 0, 32);
                    Byte[] tmpSubKey = (Byte[])subKey[i];
                    function(DES_RUN_M, 0, tmpSubKey);
                    xor(DES_RUN_M, 0, DES_RUN_M, 32, 32);
                    this.desCopy.Copy(DES_RUN_TMP, 0, ref DES_RUN_M, 32, 32);
                }
            }

            transform(DES_RUN_M, 0, DES_RUN_M, 0, IPR_TABLE, 64);
            bit2Byte(outb, outIndex, DES_RUN_M, 0, 64);
        }

        private char[] transCode(byte[] data)
        {
            int len = data.Length;
            char[] buffer = new char[len * 3];
            int index = 0;
            for (int i = 0, k = 0; i < len; i++)
            {
                int tempChar = (data[i] & 0xFF);
                if ((tempChar >= 'a' && tempChar <= 'z')
                        || (tempChar >= 'A' && tempChar <= 'Z')
                        || (tempChar >= '0' && tempChar <= '9'))
                {// 
                    buffer[k++] = (char)tempChar; //
                    index++;
                }
                else
                {
                    buffer[k++] = '%';
                    index++;
                    buffer[k++] = (char)HEXCODE[this.desCopy.MoveByte(tempChar, 4) & 15]; // >>>
                    index++;
                    buffer[k++] = (char)HEXCODE[tempChar & 15];
                    index++;
                }
            }
            char[] result = new char[index];
            this.desCopy.Copy(buffer, 0, ref result, 0, index);
            return result;
        }

        private byte[] deTransCode(char[] data)
        {
            int len = data.Length;
            byte[] buffer = new byte[len];
            int k = 0;
            for (int m = 0; m < len; m++)
            {

                char tempChar = (char)data[m];
                if ((tempChar >= 'a' && tempChar <= 'z')
                        || (tempChar >= 'A' && tempChar <= 'Z')
                        || (tempChar >= '0' && tempChar <= '9'))
                {
                    buffer[k++] = (byte)tempChar;
                }
                else
                {
                    int num = 0;
                    num += 16 * convert((char)data[++m]);
                    num += convert((char)data[++m]);
                    buffer[k++] = (byte)num;
                }
            }
            byte[] result = new byte[k];
            this.desCopy.Copy(buffer, 0, ref result, 0, k);
            return result;
        }

        private int convert(char c)
        {
            int result = 0;
            if ((c >= 'a') && (c <= 'z'))
            {
                result = c - 'a' + 10;
            }
            else if (c >= 'A' && c <= 'Z')
            {
                result = c - 'A' + 10;
            }
            else if (c >= '0' && c <= '9')
            {
                result = c - '0';
            }
            else
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 使用securityKey 对 SourcePwd进行加密
        /// </summary>
        /// <param name="sourcePwd"></param>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public string EncodePwd(string sourcePwd, string securityKey)
        {
            char[] charfrompwd = encode(securityKey, sourcePwd);
            string sPwd = new string(charfrompwd);
            return sPwd;
        }

        /// <summary>
        /// 对字符串解密
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="securityKey"></param>
        /// <returns></returns>
        public string DecodePwd(string pwd, string securityKey)
        {
            char[] charfromPwd = pwd.ToCharArray();
            byte[] bpwd = decode(securityKey, charfromPwd);
            string sPwd = System.Text.Encoding.Unicode.GetString(bpwd).TrimEnd(new char[] { '\0' });
            return sPwd;
        }
    }
}