using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZyGames.Framework.RPC.IO
{
    public static class BufferUtils
    {
        /// <summary>
        /// 将1个2维数据包整合成以个一维数据包
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static public Byte[] MergeBytes(params Byte[][] args)
        {
            Int32 length = 0;
            foreach (byte[] tempbyte in args)
            {
                length += tempbyte.Length;  //计算数据包总长度
            }

            Byte[] bytes = new Byte[length]; //建立新的数据包

            Int32 tempLength = 0;

            foreach (byte[] tempByte in args)
            {
                tempByte.CopyTo(bytes, tempLength);
                tempLength += tempByte.Length;  //复制数据包到新数据包
            }

            return bytes;

        }

        /// <summary>
        /// 将一个32位整形转换成一个BYTE[]4字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Int32 data)
        {
            return BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 将一个64位整型转换成以个BYTE[] 8字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(UInt64 data)
        {
            return BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 将一个 1位CHAR转换成1位的BYTE
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Char data)
        {
            Byte[] bytes = new Byte[] { (Byte)data };
            return bytes;
        }

        /// <summary>
        /// 将一个BYTE[]数据包添加首位长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Byte[] data)
        {
            return MergeBytes(
                GetSocketBytes(data.Length),
                data
                );
        }

        /// <summary>
        /// 将一个字符串转换成BYTE[]，BYTE[]的首位是字符串的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(String data)
        {
            Byte[] bytes = Encoding.Unicode.GetBytes(data);

            return MergeBytes(
                GetSocketBytes(bytes.Length),
                bytes
                );
        }

        /// <summary>
        /// 将一个DATATIME转换成为BYTE[]数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(DateTime data)
        {
            return GetSocketBytes(data.ToString());
        }
    }
}
