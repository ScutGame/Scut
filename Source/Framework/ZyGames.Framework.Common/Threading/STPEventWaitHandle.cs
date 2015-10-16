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
#if !(_WINDOWS_CE)

using System.Threading;

namespace ZyGames.Framework.Common.Threading
{
#if _WINDOWS ||  WINDOWS_PHONE
    internal static class STPEventWaitHandle
    {
        public const int WaitTimeout = Timeout.Infinite;

        internal static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            return WaitHandle.WaitAll(waitHandles, millisecondsTimeout);
        }

        internal static int WaitAny(WaitHandle[] waitHandles)
        {
            return WaitHandle.WaitAny(waitHandles);
        }

        internal static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            return WaitHandle.WaitAny(waitHandles, millisecondsTimeout);
        }

        internal static bool WaitOne(WaitHandle waitHandle, int millisecondsTimeout, bool exitContext)
        {
            return waitHandle.WaitOne(millisecondsTimeout);
        }
    }
#else
    internal static class STPEventWaitHandle
    {
        public const int WaitTimeout = Timeout.Infinite;

        internal static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            return WaitHandle.WaitAll(waitHandles, millisecondsTimeout, exitContext);
        }

        internal static int WaitAny(WaitHandle[] waitHandles)
        {
            return WaitHandle.WaitAny(waitHandles);
        }

        internal static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
        {
            return WaitHandle.WaitAny(waitHandles, millisecondsTimeout, exitContext);
        }

        internal static bool WaitOne(WaitHandle waitHandle, int millisecondsTimeout, bool exitContext)
        {
            return waitHandle.WaitOne(millisecondsTimeout, exitContext);
        }
    }
#endif

}

#endif