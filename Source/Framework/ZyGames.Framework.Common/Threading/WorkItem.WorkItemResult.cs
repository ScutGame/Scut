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
using System.Threading;

namespace ZyGames.Framework.Common.Threading
{
    public partial class WorkItem
    {
        #region WorkItemResult class

        private class WorkItemResult : IWorkItemResult, IInternalWorkItemResult, IInternalWaitableResult
        {
            /// <summary>
            /// A back reference to the work item
            /// </summary>
            private readonly WorkItem _workItem;

            public WorkItemResult(WorkItem workItem)
            {
                _workItem = workItem;
            }

            internal WorkItem GetWorkItem()
            {
                return _workItem;
            }

            #region IWorkItemResult Members

            public bool IsCompleted
            {
                get
                {
                    return _workItem.IsCompleted;
                }
            }

            public bool IsCanceled
            {
                get
                {
                    return _workItem.IsCanceled;
                }
            }

            public object GetResult()
            {
                return _workItem.GetResult(Timeout.Infinite, true, null);
            }

            public object GetResult(int millisecondsTimeout, bool exitContext)
            {
                return _workItem.GetResult(millisecondsTimeout, exitContext, null);
            }

            public object GetResult(TimeSpan timeout, bool exitContext)
            {
                return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, null);
            }

            public object GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle)
            {
                return _workItem.GetResult(millisecondsTimeout, exitContext, cancelWaitHandle);
            }

            public object GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle)
            {
                return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, cancelWaitHandle);
            }

            public object GetResult(out Exception e)
            {
                return _workItem.GetResult(Timeout.Infinite, true, null, out e);
            }

            public object GetResult(int millisecondsTimeout, bool exitContext, out Exception e)
            {
                return _workItem.GetResult(millisecondsTimeout, exitContext, null, out e);
            }

            public object GetResult(TimeSpan timeout, bool exitContext, out Exception e)
            {
                return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, null, out e);
            }

            public object GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception e)
            {
                return _workItem.GetResult(millisecondsTimeout, exitContext, cancelWaitHandle, out e);
            }

            public object GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception e)
            {
                return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, cancelWaitHandle, out e);
            }

            public bool Cancel()
            {
                return Cancel(false);
            }

            public bool Cancel(bool abortExecution)
            {
                return _workItem.Cancel(abortExecution);
            }

            public object State
            {
                get
                {
                    return _workItem._state;
                }
            }

            public WorkItemPriority WorkItemPriority
            {
                get
                {
                    return _workItem._workItemInfo.WorkItemPriority;
                }
            }

            /// <summary>
            /// Return the result, same as GetResult()
            /// </summary>
            public object Result
            {
                get { return GetResult(); }
            }

            /// <summary>
            /// Returns the exception if occured otherwise returns null.
            /// This value is valid only after the work item completed,
            /// before that it is always null.
            /// </summary>
            public object Exception
            {
                get { return _workItem._exception; }
            }

            #endregion

            #region IInternalWorkItemResult Members

            public event WorkItemStateCallback OnWorkItemStarted
            {
                add
                {
                    _workItem.OnWorkItemStarted += value;
                }
                remove
                {
                    _workItem.OnWorkItemStarted -= value;
                }
            }


            public event WorkItemStateCallback OnWorkItemCompleted
            {
                add
                {
                    _workItem.OnWorkItemCompleted += value;
                }
                remove
                {
                    _workItem.OnWorkItemCompleted -= value;
                }
            }

            #endregion

            #region IInternalWorkItemResult Members

            public IWorkItemResult GetWorkItemResult()
            {
                return this;
            }

            public IWorkItemResult<TResult> GetWorkItemResultT<TResult>()
            {
                return new WorkItemResultTWrapper<TResult>(this);
            }

            #endregion
        }

        #endregion

    }
}