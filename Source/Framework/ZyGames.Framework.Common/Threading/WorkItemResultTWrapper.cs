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
    #region WorkItemResultTWrapper class

    internal class WorkItemResultTWrapper<TResult> : IWorkItemResult<TResult>, IInternalWaitableResult
    {
        private readonly IWorkItemResult _workItemResult;

        public WorkItemResultTWrapper(IWorkItemResult workItemResult)
        {
            _workItemResult = workItemResult;
        }

        #region IWorkItemResult<TResult> Members

        public TResult GetResult()
        {
            return (TResult)_workItemResult.GetResult();
        }

        public TResult GetResult(int millisecondsTimeout, bool exitContext)
        {
            return (TResult)_workItemResult.GetResult(millisecondsTimeout, exitContext);
        }

        public TResult GetResult(TimeSpan timeout, bool exitContext)
        {
            return (TResult)_workItemResult.GetResult(timeout, exitContext);
        }

        public TResult GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle)
        {
            return (TResult)_workItemResult.GetResult(millisecondsTimeout, exitContext, cancelWaitHandle);
        }

        public TResult GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle)
        {
            return (TResult)_workItemResult.GetResult(timeout, exitContext, cancelWaitHandle);
        }

        public TResult GetResult(out Exception e)
        {
            return (TResult)_workItemResult.GetResult(out e);
        }

        public TResult GetResult(int millisecondsTimeout, bool exitContext, out Exception e)
        {
            return (TResult)_workItemResult.GetResult(millisecondsTimeout, exitContext, out e);
        }

        public TResult GetResult(TimeSpan timeout, bool exitContext, out Exception e)
        {
            return (TResult)_workItemResult.GetResult(timeout, exitContext, out e);
        }

        public TResult GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception e)
        {
            return (TResult)_workItemResult.GetResult(millisecondsTimeout, exitContext, cancelWaitHandle, out e);
        }

        public TResult GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception e)
        {
            return (TResult)_workItemResult.GetResult(timeout, exitContext, cancelWaitHandle, out e);
        }

        public bool IsCompleted
        {
            get { return _workItemResult.IsCompleted; }
        }

        public bool IsCanceled
        {
            get { return _workItemResult.IsCanceled; }
        }

        public object State
        {
            get { return _workItemResult.State; }
        }

        public bool Cancel()
        {
            return _workItemResult.Cancel();
        }

        public bool Cancel(bool abortExecution)
        {
            return _workItemResult.Cancel(abortExecution);
        }

        public WorkItemPriority WorkItemPriority
        {
            get { return _workItemResult.WorkItemPriority; }
        }

        public TResult Result
        {
            get { return (TResult)_workItemResult.Result; }
        }

        public object Exception
        {
            get { return (TResult)_workItemResult.Exception; }
        }

        #region IInternalWorkItemResult Members

        public IWorkItemResult GetWorkItemResult()
        {
            return _workItemResult.GetWorkItemResult();
        }

        public IWorkItemResult<TRes> GetWorkItemResultT<TRes>()
        {
            return (IWorkItemResult<TRes>)this;
        }

        #endregion

        #endregion
    }

    #endregion

}