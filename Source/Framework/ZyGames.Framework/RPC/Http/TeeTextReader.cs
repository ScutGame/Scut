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
using System.Text;

namespace ZyGames.Framework.RPC.Http
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TeeTextReader : TextReader
    {
        readonly TextReader input;
        readonly Action<string>[] outputs;
        readonly StringBuilder buffer;
        readonly int bufferSize;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputs"></param>
        /// <param name="bufferSize"></param>
        public TeeTextReader(TextReader input, Action<string>[] outputs, int bufferSize = 8192)
            : base()
        {
            this.input = input;
            this.outputs = outputs;

            this.bufferSize = bufferSize;
            this.buffer = new StringBuilder(bufferSize);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="bufferSize"></param>
        public TeeTextReader(TextReader input, Action<string> output, int bufferSize = 8192)
            : this(input, new Action<string>[1] { output }, bufferSize)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Peek()
        {
            return input.Peek();
        }

        void writeBuffer()
        {
            foreach (var output in outputs)
                output(buffer.ToString());
            buffer.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Read()
        {
            int c = input.Read();
            if (c == -1 || c == '\n')
            {
                // Both EOF and '\n' cause a newline append:
                if (c == '\n') buffer.Append('\n');
                writeBuffer();
            }
            else
            {
                buffer.Append((char)c);
                if (buffer.Length >= bufferSize)
                {
                    writeBuffer();
                }
            }
            return c;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(char[] buffer, int index, int count)
        {
            int nr = base.Read(buffer, index, count);
            if (nr > 0)
            {
                for (int i = index; i < index + nr; ++i)
                {
                    this.buffer.Append(buffer[i]);
                    if ((buffer.Length >= bufferSize) || (buffer[i] == '\n'))
                        writeBuffer();
                }
            }
            else writeBuffer();
            return nr;
        }
    }
}
