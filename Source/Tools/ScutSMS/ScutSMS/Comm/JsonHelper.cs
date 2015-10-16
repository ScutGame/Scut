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
using System.Linq;
using System.Text;

namespace Scut.SMS.Comm
{
    public static class JsonHelper
    {

        public static string FormatJsonToString(string json)
        {
            var tabLen = 4;
            var write = new StringBuilder();
            var charStack = new Stack<char>();
            int depth = 0;
            var buffer = json.ToCharArray();
            string preStr = "".PadLeft(depth * tabLen, ' ');
            char preByte = ' ';
            foreach (var ch in buffer)
            {
                if (!IsStringChar(charStack) && (ch == '{' || ch == '['))
                {
                    depth++;
                    charStack.Push(ch);
                    write.Append(ch);
                    write.AppendLine();
                    preStr = "".PadLeft(depth * tabLen, ' ');
                    write.Append(preStr);
                }
                else if (!IsStringChar(charStack) && (ch == ']' || ch == '}'))
                {
                    depth--;
                    charStack.Pop();
                    preStr = "".PadLeft(depth * tabLen, ' ');
                    write.AppendLine();
                    write.Append(preStr);
                    write.Append(ch);
                }
                else if (!IsStringChar(charStack) && ch == ',')
                {
                    write.Append(ch);
                    if (!IsStringChar(charStack))
                    {
                        write.AppendLine();
                        write.Append(preStr);
                    }
                }
                else
                {
                    if (preByte == ':' && !IsStringChar(charStack))
                    {
                        write.Append(' ');
                    }
                    write.Append(ch);
                    if (ch == '"')
                    {
                        if (charStack.Peek() == ch)
                        {
                            charStack.Pop();
                        }
                        else
                        {
                            charStack.Push(ch);
                        }
                    }
                }
                preByte = ch;
            }
            return write.ToString();
        }

        private static bool IsStringChar(Stack<char> charStack)
        {
            return charStack.Count > 0 && charStack.Peek() == '"';
        }
    }
}