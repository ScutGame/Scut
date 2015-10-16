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
using System.Security.Cryptography;
using System.Text;

namespace ZyGames.Framework.RPC.Http
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SHA1TextReader : TextReader
    {
        readonly SHA1 sha1;
        readonly TextReader input;
        readonly Encoder encoder;

        readonly static byte[] dum = new byte[0];
        bool isFinal = false;

        readonly char[] ibuf;
        readonly byte[] obuf;
        int ibufIndex;
        const int bufferSize = 8192;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        public SHA1TextReader(TextReader input, Encoding encoding)
            : base()
        {
            this.input = input;
            this.encoder = encoding.GetEncoder();

            this.sha1 = SHA1.Create();

            this.ibuf = new char[bufferSize];
            this.obuf = new byte[bufferSize];
            this.ibufIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Peek()
        {
            return input.Peek();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Read()
        {
            int c = input.Read();
            if (c == -1)
            {
                transformBuffer();
                return c;
            }

            ibuf[ibufIndex++] = (char)c;
            if (ibufIndex >= bufferSize)
            {
                transformBuffer();
            }
            return c;
        }

        private void transformBuffer()
        {
            if (ibufIndex == 0) return;

            // Encode the characters using the encoder and SHA1 those bytes:
            int nb = encoder.GetBytes(ibuf, 0, ibufIndex, obuf, 0, false);
            sha1.TransformBlock(obuf, 0, nb, null, 0);

            ibufIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetHash()
        {
            if (!isFinal)
            {
                transformBuffer();

                sha1.TransformFinalBlock(dum, 0, 0);
                isFinal = true;
            }

            return sha1.Hash;
        }
    }
}
