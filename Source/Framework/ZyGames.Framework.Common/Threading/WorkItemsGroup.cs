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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ZyGames.Framework.Common.Threading
{

	#region WorkItemsGroup class 

	/// <summary>
	/// Summary description for WorkItemsGroup.
	/// </summary>
	public class WorkItemsGroup : WorkItemsGroupBase
	{
		#region Private members

		private readonly object _lock = new object();

		/// <summary>
		/// A reference to the SmartThreadPool instance that created this 
		/// WorkItemsGroup.
		/// </summary>
		private readonly SmartThreadPool _stp;

		/// <summary>
		/// The OnIdle event
		/// </summary>
		private event WorkItemsGroupIdleHandler _onIdle;

        /// <summary>
        /// A flag to indicate if the Work Items Group is now suspended.
        /// </summary>
        private bool _isSuspended;

		/// <summary>
		/// Defines how many work items of this WorkItemsGroup can run at once.
		/// </summary>
		private int _concurrency;

		/// <summary>
		/// Priority queue to hold work items before they are passed 
		/// to the SmartThreadPool.
		/// </summary>
		private readonly PriorityQueue _workItemsQueue;

		/// <summary>
		/// Indicate how many work items are waiting in the SmartThreadPool
		/// queue.
		/// This value is used to apply the concurrency.
		/// </summary>
		private int _workItemsInStpQueue;

		/// <summary>
		/// Indicate how many work items are currently running in the SmartThreadPool.
		/// This value is used with the Cancel, to calculate if we can send new 
		/// work items to the STP.
		/// </summary>
		private int _workItemsExecutingInStp = 0;

		/// <summary>
		/// WorkItemsGroup start information
		/// </summary>
		private readonly WIGStartInfo _workItemsGroupStartInfo;

		/// <summary>
		/// Signaled when all of the WorkItemsGroup's work item completed.
		/// </summary>
        //private readonly ManualResetEvent _isIdleWaitHandle = new ManualResetEvent(true);
        private readonly ManualResetEvent _isIdleWaitHandle = EventWaitHandleFactory.CreateManualResetEvent(true);

		/// <summary>
		/// A common object for all the work items that this work items group
		/// generate so we can mark them to cancel in O(1)
		/// </summary>
		private CanceledWorkItemsGroup _canceledWorkItemsGroup = new CanceledWorkItemsGroup();

		#endregion 

		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemsGroup"/> class.
		/// </summary>
		/// <param name="stp">Stp.</param>
		/// <param name="concurrency">Concurrency.</param>
		/// <param name="wigStartInfo">Wig start info.</param>
	    public WorkItemsGroup(
			SmartThreadPool stp, 
			int concurrency, 
			WIGStartInfo wigStartInfo)
		{
			if (concurrency <= 0)
			{
				throw new ArgumentOutOfRangeException(
                    "concurrency",
#if !(_WINDOWS_CE) && !(_SILVERLIGHT) && !(WINDOWS_PHONE)
                    concurrency,
#endif
 "concurrency must be greater than zero");
			}
			_stp = stp;
			_concurrency = concurrency;
			_workItemsGroupStartInfo = new WIGStartInfo(wigStartInfo).AsReadOnly();
			_workItemsQueue = new PriorityQueue();
	        Name = "WorkItemsGroup";

			// The _workItemsInStpQueue gets the number of currently executing work items,
			// because once a work item is executing, it cannot be cancelled.
			_workItemsInStpQueue = _workItemsExecutingInStp;

            _isSuspended = _workItemsGroupStartInfo.StartSuspended;
		}

		#endregion 

        #region WorkItemsGroupBase Overrides
		/// <summary>
		/// Get/Set the maximum number of workitem that execute cocurrency on the thread pool
		/// </summary>
		/// <value>The concurrency.</value>
        public override int Concurrency
        {
            get { return _concurrency; }
            set
            {
                Debug.Assert(value > 0);

                int diff = value - _concurrency;
                _concurrency = value;
                if (diff > 0)
                {
                    EnqueueToSTPNextNWorkItem(diff);
                }
            }
        }
		/// <summary>
		/// Get the number of work items waiting in the queue.
		/// </summary>
		/// <value>The waiting callbacks.</value>
        public override int WaitingCallbacks
        {
            get { return _workItemsQueue.Count; }
        }
		/// <summary>
		/// Get an array with all the state objects of the currently running items.
		/// The array represents a snap shot and impact performance.
		/// </summary>
		/// <returns>The states.</returns>
        public override object[] GetStates()
        {
            lock (_lock)
            {
                object[] states = new object[_workItemsQueue.Count];
                int i = 0;
                foreach (WorkItem workItem in _workItemsQueue)
                {
                    states[i] = workItem.GetWorkItemResult().State;
                    ++i;
                }
                return states;
            }
        }

	    /// <summary>
        /// WorkItemsGroup start information
        /// </summary>
        public override WIGStartInfo WIGStartInfo
        {
            get { return _workItemsGroupStartInfo; }
        }

	    /// <summary>
	    /// Start the Work Items Group if it was started suspended
	    /// </summary>
	    public override void Start()
	    {
	        // If the Work Items Group already started then quit
	        if (!_isSuspended)
	        {
	            return;
	        }
	        _isSuspended = false;
            
	        EnqueueToSTPNextNWorkItem(Math.Min(_workItemsQueue.Count, _concurrency));
	    }
		/// <summary>
		/// Cancel all work items using thread abortion
		/// </summary>
		/// <param name="abortExecution">True to stop work items by raising ThreadAbortException</param>
		/// <returns><c>true</c> if this instance cancel abortExecution; otherwise, <c>false</c>.</returns>
	    public override void Cancel(bool abortExecution)
	    {
	        lock (_lock)
	        {
	            _canceledWorkItemsGroup.IsCanceled = true;
	            _workItemsQueue.Clear();
	            _workItemsInStpQueue = 0;
	            _canceledWorkItemsGroup = new CanceledWorkItemsGroup();
	        }

	        if (abortExecution)
	        {
	            _stp.CancelAbortWorkItemsGroup(this);
	        }
	    }

	    /// <summary>
        /// Wait for the thread pool to be idle
        /// </summary>
        public override bool WaitForIdle(int millisecondsTimeout)
        {
            SmartThreadPool.ValidateWorkItemsGroupWaitForIdle(this);
            return STPEventWaitHandle.WaitOne(_isIdleWaitHandle, millisecondsTimeout, false);
        }
		/// <summary>
		/// This event is fired when all work items are completed.
		/// (When IsIdle changes to true)
		/// This event only work on WorkItemsGroup. On SmartThreadPool
		/// it throws the NotImplementedException.
		/// </summary>
	    public override event WorkItemsGroupIdleHandler OnIdle
		{
			add { _onIdle += value; }
			remove { _onIdle -= value; }
		}

	    #endregion 

		#region Private methods

	    private void RegisterToWorkItemCompletion(IWorkItemResult wir)
		{
			IInternalWorkItemResult iwir = (IInternalWorkItemResult)wir;
			iwir.OnWorkItemStarted += OnWorkItemStartedCallback;
			iwir.OnWorkItemCompleted += OnWorkItemCompletedCallback;
		}
		/// <summary>
		/// Raises the STP is starting event.
		/// </summary>
	    public void OnSTPIsStarting()
		{
            if (_isSuspended)
            {
                return;
            }
			
            EnqueueToSTPNextNWorkItem(_concurrency);
		}
		/// <summary>
		/// Enqueues to STP next N work item.
		/// </summary>
		/// <param name="count">Count.</param>
	    public void EnqueueToSTPNextNWorkItem(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                EnqueueToSTPNextWorkItem(null, false);
            }
        }

		private object FireOnIdle(object state)
		{
			FireOnIdleImpl(_onIdle);
			return null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void FireOnIdleImpl(WorkItemsGroupIdleHandler onIdle)
		{
			if(null == onIdle)
			{
				return;
			}

			Delegate[] delegates = onIdle.GetInvocationList();
			foreach(WorkItemsGroupIdleHandler eh in delegates)
			{
				try
				{
					eh(this);
				}
                catch { }  // Suppress exceptions
			}
		}

		private void OnWorkItemStartedCallback(WorkItem workItem)
		{
			lock(_lock)
			{
				++_workItemsExecutingInStp;
			}
		}

		private void OnWorkItemCompletedCallback(WorkItem workItem)
		{
			EnqueueToSTPNextWorkItem(null, true);
		}

        internal override void Enqueue(WorkItem workItem)
        {
            EnqueueToSTPNextWorkItem(workItem);
        }

	    private void EnqueueToSTPNextWorkItem(WorkItem workItem)
		{
			EnqueueToSTPNextWorkItem(workItem, false);
		}

		private void EnqueueToSTPNextWorkItem(WorkItem workItem, bool decrementWorkItemsInStpQueue)
		{
			lock(_lock)
			{
				// Got here from OnWorkItemCompletedCallback()
				if (decrementWorkItemsInStpQueue)
				{
					--_workItemsInStpQueue;

					if(_workItemsInStpQueue < 0)
					{
						_workItemsInStpQueue = 0;
					}

					--_workItemsExecutingInStp;

					if(_workItemsExecutingInStp < 0)
					{
						_workItemsExecutingInStp = 0;
					}
				}

				// If the work item is not null then enqueue it
				if (null != workItem)
				{
					workItem.CanceledWorkItemsGroup = _canceledWorkItemsGroup;

					RegisterToWorkItemCompletion(workItem.GetWorkItemResult());
					_workItemsQueue.Enqueue(workItem);
					//_stp.IncrementWorkItemsCount();

					if ((1 == _workItemsQueue.Count) && 
						(0 == _workItemsInStpQueue))
					{
						_stp.RegisterWorkItemsGroup(this);
                        IsIdle = false;
                        _isIdleWaitHandle.Reset();
					}
				}

				// If the work items queue of the group is empty than quit
				if (0 == _workItemsQueue.Count)
				{
					if (0 == _workItemsInStpQueue)
					{
						_stp.UnregisterWorkItemsGroup(this);
                        IsIdle = true;
                        _isIdleWaitHandle.Set();
                        if (decrementWorkItemsInStpQueue && _onIdle != null && _onIdle.GetInvocationList().Length > 0)
                        {
                            _stp.QueueWorkItem(new WorkItemCallback(FireOnIdle));
                        }
					}
					return;
				}

                if (!_isSuspended)
				{
					if (_workItemsInStpQueue < _concurrency)
					{
						WorkItem nextWorkItem = _workItemsQueue.Dequeue() as WorkItem;
                        try
                        {
                            _stp.Enqueue(nextWorkItem);
                        }
                        catch (ObjectDisposedException e)
                        {
                            e.GetHashCode();
                            // The STP has been shutdown
                        }

						++_workItemsInStpQueue;
					}
				}
			}
		}

		#endregion
    }

	#endregion
}