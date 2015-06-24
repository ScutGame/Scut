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

using System.IO;
using System.IO.Compression;

namespace ZyGames.Framework.RPC.IO
{
    /// <summary>
    /// 数据压缩类,数据长度大于50000，压缩才有意义
    /// </summary>
    public static class GzipUtils
    {
        #region 压缩
        /// <summary>
        /// 压缩流数据
        /// </summary>
        /// <param name="aSourceStream"></param>
        /// <returns></returns>
        public static byte[] EnCompress(Stream aSourceStream)
        {
            MemoryStream vMemory = new MemoryStream();

            aSourceStream.Seek(0, SeekOrigin.Begin);
            vMemory.Seek(0, SeekOrigin.Begin);
            try
            {
                using (GZipStream vZipStream = new GZipStream(vMemory, CompressionMode.Compress))
                {
                    byte[] vFileByte = new byte[1024];
                    int vRedLen = 0;
                    do
                    {
                        vRedLen = aSourceStream.Read(vFileByte, 0, vFileByte.Length);
                        vZipStream.Write(vFileByte, 0, vRedLen);
                    }
                    while (vRedLen > 0);
                }
            }
            finally
            {
                vMemory.Dispose();
            }
            return vMemory.ToArray();
        }

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="aSourceStream"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] EnCompress(byte[] aSourceStream, int index, int count)
        {
            using (MemoryStream vMemory = new MemoryStream(aSourceStream, index, count))
            {
                return EnCompress(vMemory);
            }
        }
        #endregion

        #region 解压
        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="aSourceStream"></param>
        /// <returns></returns>
        public static byte[] DeCompress(Stream aSourceStream)
        {
            byte[] vUnZipByte = null;
            GZipStream vUnZipStream;

            using (MemoryStream vMemory = new MemoryStream())
            {
                vUnZipStream = new GZipStream(aSourceStream, CompressionMode.Decompress);
                try
                {
                    byte[] vTempByte = new byte[1024];
                    int vRedLen = 0;
                    do
                    {
                        vRedLen = vUnZipStream.Read(vTempByte, 0, vTempByte.Length);
                        vMemory.Write(vTempByte, 0, vRedLen);
                    }
                    while (vRedLen > 0);
                    vUnZipStream.Close();
                }
                finally
                {
                    vUnZipStream.Dispose();
                }
                vUnZipByte = vMemory.ToArray();
            }
            return vUnZipByte;
        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="aSourceByte"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] DeCompress(byte[] aSourceByte, int index, int count)
        {
            using (MemoryStream vMemory = new MemoryStream(aSourceByte, index, count))
            {
                return DeCompress(vMemory);
            }
        }
        #endregion
    }
}