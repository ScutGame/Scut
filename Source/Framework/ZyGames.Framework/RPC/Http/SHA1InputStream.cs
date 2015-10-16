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

namespace ZyGames.Framework.RPC.Http
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SHA1InputStream : Stream
    {
        private long position;
        private SHA1 sha1;
        private Stream input;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public SHA1InputStream(Stream input)
        {
            this.input = input;

            this.sha1 = SHA1.Create();
            this.position = 0L;
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead { get { return true; } }
        /// <summary>
        /// 
        /// </summary>
        public override bool CanSeek { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite { get { return false; } }
        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public override long Length
        {
            get { return input.Length; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override long Position
        {
            get
            {
                return this.position;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int nr = input.Read(buffer, offset, count);
            if (nr == 0) return 0;

            sha1.TransformBlock(buffer, offset, nr, null, 0);
            position += nr;

            return nr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        private readonly static byte[] dum = new byte[0];
        private bool isFinal = false;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetHash()
        {
            if (!isFinal)
            {
                sha1.TransformFinalBlock(dum, 0, 0);
                isFinal = true;
            }

            return sha1.Hash;
        }
    }
}
