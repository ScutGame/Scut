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

namespace ZyGames.Framework.Common.Threading
{
    public partial class SmartThreadPool
    {
        #region ThreadEntry class

        internal class ThreadEntry
        {
            /// <summary>
            /// The thread creation time
            /// The value is stored as UTC value.
            /// </summary>
            private readonly DateTime _creationTime;

            /// <summary>
            /// The last time this thread has been running
            /// It is updated by IAmAlive() method
            /// The value is stored as UTC value.
            /// </summary>
            private DateTime _lastAliveTime;

            /// <summary>
            /// A reference from each thread in the thread pool to its SmartThreadPool
            /// object container.
            /// With this variable a thread can know whatever it belongs to a 
            /// SmartThreadPool.
            /// </summary>
            private readonly SmartThreadPool _associatedSmartThreadPool;

            /// <summary>
            /// A reference to the current work item a thread from the thread pool 
            /// is executing.
            /// </summary>            
            public WorkItem CurrentWorkItem { get; set; }

            public ThreadEntry(SmartThreadPool stp)
            {
                _associatedSmartThreadPool = stp;
                _creationTime = DateTime.UtcNow;
                _lastAliveTime = DateTime.MinValue;
            }

            public SmartThreadPool AssociatedSmartThreadPool
            {
                get { return _associatedSmartThreadPool; }
            }

            public void IAmAlive()
            {
                _lastAliveTime = DateTime.UtcNow;
            }
        }

        #endregion
    }
}