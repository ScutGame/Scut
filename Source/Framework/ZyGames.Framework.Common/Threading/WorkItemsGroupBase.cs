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
	/// <summary>
	/// Work items group base.
	/// </summary>
    public abstract class WorkItemsGroupBase : IWorkItemsGroup
    {
        #region Private Fields

        /// <summary>
        /// Contains the name of this instance of SmartThreadPool.
        /// Can be changed by the user.
        /// </summary>
        private string _name = "WorkItemsGroupBase";
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemsGroupBase"/> class.
		/// </summary>
        public WorkItemsGroupBase()
        {
            IsIdle = true;
        }

        #endregion

        #region IWorkItemsGroup Members

        #region Public Methods

        /// <summary>
        /// Get/Set the name of the SmartThreadPool/WorkItemsGroup instance
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Abstract Methods
		/// <summary>
		/// Get/Set the maximum number of workitem that execute cocurrency on the thread pool
		/// </summary>
		/// <value>The concurrency.</value>
        public abstract int Concurrency { get; set; }
       /// <summary>
       /// Get the number of work items waiting in the queue.
       /// </summary>
       /// <value>The waiting callbacks.</value>
		 public abstract int WaitingCallbacks { get; }
        /// <summary>
        /// Get an array with all the state objects of the currently running items.
        /// The array represents a snap shot and impact performance.
        /// </summary>
        /// <returns>The states.</returns>
		public abstract object[] GetStates();
        /// <summary>
        /// Get the WorkItemsGroup start information
        /// </summary>
        /// <value>The WIG start info.</value>
		public abstract WIGStartInfo WIGStartInfo { get; }
        /// <summary>
        /// Starts to execute work items
        /// </summary>
		public abstract void Start();
		/// <summary>
		/// Cancel all work items using thread abortion
		/// </summary>
		/// <param name="abortExecution">True to stop work items by raising ThreadAbortException</param>
		/// <returns><c>true</c> if this instance cancel abortExecution; otherwise, <c>false</c>.</returns>
        public abstract void Cancel(bool abortExecution);
		/// <summary>
		/// Wait for all work item to complete, until timeout expired
		/// </summary>
		/// <param name="millisecondsTimeout">How long to wait for the work items to complete</param>
		/// <returns>Returns true if work items completed within the timeout, otherwise false.</returns>
        public abstract bool WaitForIdle(int millisecondsTimeout);
		/// <summary>
		/// This event is fired when all work items are completed.
		/// (When IsIdle changes to true)
		/// This event only work on WorkItemsGroup. On SmartThreadPool
		/// it throws the NotImplementedException.
		/// </summary>
        public abstract event WorkItemsGroupIdleHandler OnIdle;

        internal abstract void Enqueue(WorkItem workItem);
        internal virtual void PreQueueWorkItem() { }

        #endregion

        #region Common Base Methods

        /// <summary>
        /// Cancel all the work items.
        /// Same as Cancel(false)
        /// </summary>
        public virtual void Cancel()
        {
            Cancel(false);
        }

        /// <summary>
        /// Wait for the SmartThreadPool/WorkItemsGroup to be idle
        /// </summary>
        public void WaitForIdle()
        {
            WaitForIdle(Timeout.Infinite);
        }

        /// <summary>
        /// Wait for the SmartThreadPool/WorkItemsGroup to be idle
        /// </summary>
        public bool WaitForIdle(TimeSpan timeout)
        {
            return WaitForIdle((int)timeout.TotalMilliseconds);
        }

        /// <summary>
        /// IsIdle is true when there are no work items running or queued.
        /// </summary>
        public bool IsIdle { get; protected set; }

        #endregion

        #region QueueWorkItem

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback)
        {
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="workItemPriority">The priority of the work item</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback, WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="workItemInfo">Work item info</param>
        /// <param name="callback">A callback to execute</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemInfo workItemInfo, WorkItemCallback callback)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, workItemInfo, callback);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback, object state)
        {
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="workItemPriority">The work item priority</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemCallback callback, object state, WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="workItemInfo">Work item information</param>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(WorkItemInfo workItemInfo, WorkItemCallback callback, object state)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, workItemInfo, callback, state);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <param name="workItemPriority">The work item priority</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback,
            WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <param name="callToPostExecute">Indicates on which cases to call to the post execute callback</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback,
            CallToPostExecute callToPostExecute)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback, callToPostExecute);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        /// <summary>
        /// Queue a work item
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        /// <param name="state">
        /// The context object of the work item. Used for passing arguments to the work item. 
        /// </param>
        /// <param name="postExecuteWorkItemCallback">
        /// A delegate to call after the callback completion
        /// </param>
        /// <param name="callToPostExecute">Indicates on which cases to call to the post execute callback</param>
        /// <param name="workItemPriority">The work item priority</param>
        /// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(
            WorkItemCallback callback,
            object state,
            PostExecuteWorkItemCallback postExecuteWorkItemCallback,
            CallToPostExecute callToPostExecute,
            WorkItemPriority workItemPriority)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(this, WIGStartInfo, callback, state, postExecuteWorkItemCallback, callToPostExecute, workItemPriority);
            Enqueue(workItem);
            return workItem.GetWorkItemResult();
        }

        #endregion

        #region QueueWorkItem(Action<...>)
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">A callback to execute</param>
		/// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem(Action action)
        {
            return QueueWorkItem (action, SmartThreadPool.DefaultWorkItemPriority);
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">A callback to execute</param>
		/// <param name="priority">The priority of the work item</param>
		/// <returns>Returns a work item result</returns>
        public IWorkItemResult QueueWorkItem (Action action, WorkItemPriority priority)
        {
            PreQueueWorkItem ();
            WorkItem workItem = WorkItemFactory.CreateWorkItem (
                this,
                WIGStartInfo,
                delegate
                {
                    action.Invoke ();
                    return null;
                }, priority);
            Enqueue (workItem);
            return workItem.GetWorkItemResult ();
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="arg">Argument.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T>(Action<T> action, T arg)
        {
            return QueueWorkItem<T> (action, arg, SmartThreadPool.DefaultWorkItemPriority);
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="arg">Argument.</param>
		/// <param name="priority">Priority.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T> (Action<T> action, T arg, WorkItemPriority priority)
        {
            PreQueueWorkItem ();
            WorkItem workItem = WorkItemFactory.CreateWorkItem (
                this,
                WIGStartInfo,
                state =>
                {
                    action.Invoke (arg);
                    return null;
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg } : null, priority);
            Enqueue (workItem);
            return workItem.GetWorkItemResult ();
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T1, T2>(Common.Threading.Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            return QueueWorkItem<T1, T2> (action, arg1, arg2, SmartThreadPool.DefaultWorkItemPriority);
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <param name="priority">Priority.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T1, T2> (Common.Threading.Action<T1, T2> action, T1 arg1, T2 arg2, WorkItemPriority priority)
        {
            PreQueueWorkItem ();
            WorkItem workItem = WorkItemFactory.CreateWorkItem (
                this,
                WIGStartInfo,
                state =>
                {
                    action.Invoke (arg1, arg2);
                    return null;
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2 } : null, priority);
            Enqueue (workItem);
            return workItem.GetWorkItemResult ();
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <param name="arg3">Arg3.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
		/// <typeparam name="T3">The 3rd type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T1, T2, T3>(Common.Threading.Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return QueueWorkItem<T1, T2, T3> (action, arg1, arg2, arg3, SmartThreadPool.DefaultWorkItemPriority);
            ;
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <param name="arg3">Arg3.</param>
		/// <param name="priority">Priority.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
		/// <typeparam name="T3">The 3rd type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T1, T2, T3> (Common.Threading.Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3, WorkItemPriority priority)
        {
            PreQueueWorkItem ();
            WorkItem workItem = WorkItemFactory.CreateWorkItem (
                this,
                WIGStartInfo,
                state =>
                {
                    action.Invoke (arg1, arg2, arg3);
                    return null;
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3 } : null, priority);
            Enqueue (workItem);
            return workItem.GetWorkItemResult ();
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <param name="arg3">Arg3.</param>
		/// <param name="arg4">Arg4.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
		/// <typeparam name="T3">The 3rd type parameter.</typeparam>
		/// <typeparam name="T4">The 4th type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T1, T2, T3, T4>(Common.Threading.Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return QueueWorkItem<T1, T2, T3, T4> (action, arg1, arg2, arg3, arg4,
                                                  SmartThreadPool.DefaultWorkItemPriority);
        }
		/// <summary>
		/// Queue a work item.
		/// </summary>
		/// <returns>Returns a IWorkItemResult object, but its GetResult() will always return null</returns>
		/// <param name="action">Action.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <param name="arg3">Arg3.</param>
		/// <param name="arg4">Arg4.</param>
		/// <param name="priority">Priority.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
		/// <typeparam name="T3">The 3rd type parameter.</typeparam>
		/// <typeparam name="T4">The 4th type parameter.</typeparam>
        public IWorkItemResult QueueWorkItem<T1, T2, T3, T4> (Common.Threading.Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, WorkItemPriority priority)
        {
            PreQueueWorkItem ();
            WorkItem workItem = WorkItemFactory.CreateWorkItem (
                           this,
                           WIGStartInfo,
                           state =>
                           {
                               action.Invoke (arg1, arg2, arg3, arg4);
                               return null;
                           },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3, arg4 } : null, priority);
            Enqueue (workItem);
            return workItem.GetWorkItemResult ();
        }

        #endregion

        #region QueueWorkItem(Func<...>)
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="func">Func.</param>
		/// <typeparam name="TResult">The 1st type parameter.</typeparam>
        public IWorkItemResult<TResult> QueueWorkItem<TResult>(Common.Threading.Func<TResult> func)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            state =>
                            {
                                return func.Invoke();
                            });
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="func">Func.</param>
		/// <param name="arg">Argument.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="TResult">The 2nd type parameter.</typeparam>
        public IWorkItemResult<TResult> QueueWorkItem<T, TResult>(Common.Threading.Func<T, TResult> func, T arg)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                this,
                WIGStartInfo,
                state =>
                {
                    return func.Invoke(arg);
                },
                WIGStartInfo.FillStateWithArgs ? new object[] { arg } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <param name="func">Func.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
		/// <typeparam name="TResult">The 3rd type parameter.</typeparam>
        public IWorkItemResult<TResult> QueueWorkItem<T1, T2, TResult>(Common.Threading.Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            state =>
                            {
                                return func.Invoke(arg1, arg2);
                            },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2 } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <returns>Returns a work item result</returns>
		/// <param name="func">Func.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <param name="arg3">Arg3.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
		/// <typeparam name="T3">The 3rd type parameter.</typeparam>
		/// <typeparam name="TResult">The 4th type parameter.</typeparam>
        public IWorkItemResult<TResult> QueueWorkItem<T1, T2, T3, TResult>(Common.Threading.Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            state =>
                            {
                                return func.Invoke(arg1, arg2, arg3);
                            },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3 } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }
		/// <summary>
		/// Queue a work item
		/// </summary>
		/// <returns>Returns a work item result</returns>
		/// <param name="func">Func.</param>
		/// <param name="arg1">Arg1.</param>
		/// <param name="arg2">Arg2.</param>
		/// <param name="arg3">Arg3.</param>
		/// <param name="arg4">Arg4.</param>
		/// <typeparam name="T1">The 1st type parameter.</typeparam>
		/// <typeparam name="T2">The 2nd type parameter.</typeparam>
		/// <typeparam name="T3">The 3rd type parameter.</typeparam>
		/// <typeparam name="T4">The 4th type parameter.</typeparam>
		/// <typeparam name="TResult">The 5th type parameter.</typeparam>
        public IWorkItemResult<TResult> QueueWorkItem<T1, T2, T3, T4, TResult>(Common.Threading.Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            PreQueueWorkItem();
            WorkItem workItem = WorkItemFactory.CreateWorkItem(
                            this,
                            WIGStartInfo,
                            state =>
                            {
                                return func.Invoke(arg1, arg2, arg3, arg4);
                            },
                           WIGStartInfo.FillStateWithArgs ? new object[] { arg1, arg2, arg3, arg4 } : null);
            Enqueue(workItem);
            return new WorkItemResultTWrapper<TResult>(workItem.GetWorkItemResult());
        }

        #endregion

        #endregion
    }
}