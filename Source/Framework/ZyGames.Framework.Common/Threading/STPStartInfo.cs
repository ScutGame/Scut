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
	/// Summary description for STPStartInfo.
	/// </summary>
    public class STPStartInfo : WIGStartInfo
    {
        private int _idleTimeout = SmartThreadPool.DefaultIdleTimeout;
        private int _minWorkerThreads = SmartThreadPool.DefaultMinWorkerThreads;
        private int _maxWorkerThreads = SmartThreadPool.DefaultMaxWorkerThreads;
#if !(WINDOWS_PHONE)
        private ThreadPriority _threadPriority = SmartThreadPool.DefaultThreadPriority;
#endif
        private string _performanceCounterInstanceName = SmartThreadPool.DefaultPerformanceCounterInstanceName;
        private bool _areThreadsBackground = SmartThreadPool.DefaultAreThreadsBackground;
        private bool _enableLocalPerformanceCounters;
        private string _threadPoolName = SmartThreadPool.DefaultThreadPoolName;
        private int? _maxStackSize = SmartThreadPool.DefaultMaxStackSize;
		/// <summary>
		/// Initializes a new instance of the <see cref="STPStartInfo"/> class.
		/// </summary>
        public STPStartInfo()
        {
            _performanceCounterInstanceName = SmartThreadPool.DefaultPerformanceCounterInstanceName;
#if !(WINDOWS_PHONE)
            _threadPriority = SmartThreadPool.DefaultThreadPriority;
#endif
            _maxWorkerThreads = SmartThreadPool.DefaultMaxWorkerThreads;
            _idleTimeout = SmartThreadPool.DefaultIdleTimeout;
            _minWorkerThreads = SmartThreadPool.DefaultMinWorkerThreads;
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="STPStartInfo"/> class.
		/// </summary>
		/// <param name="stpStartInfo">Stp start info.</param>
	    public STPStartInfo(STPStartInfo stpStartInfo)
            : base(stpStartInfo)
        {
            _idleTimeout = stpStartInfo.IdleTimeout;
            _minWorkerThreads = stpStartInfo.MinWorkerThreads;
            _maxWorkerThreads = stpStartInfo.MaxWorkerThreads;
#if !(WINDOWS_PHONE)
            _threadPriority = stpStartInfo.ThreadPriority;
#endif
            _performanceCounterInstanceName = stpStartInfo.PerformanceCounterInstanceName;
            _enableLocalPerformanceCounters = stpStartInfo._enableLocalPerformanceCounters;
            _threadPoolName = stpStartInfo._threadPoolName;
            _areThreadsBackground = stpStartInfo.AreThreadsBackground;
#if !(_SILVERLIGHT) && !(WINDOWS_PHONE)
            _apartmentState = stpStartInfo._apartmentState;
#endif
        }
	  
	    /// <summary>
	    /// Get/Set the idle timeout in milliseconds.
	    /// If a thread is idle (starved) longer than IdleTimeout then it may quit.
	    /// </summary>
	    public virtual int IdleTimeout
	    {
	        get { return _idleTimeout; }
	        set 
            {
                ThrowIfReadOnly();
                _idleTimeout = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set the lower limit of threads in the pool.
	    /// </summary>
	    public virtual int MinWorkerThreads
	    {
	        get { return _minWorkerThreads; }
	        set 
            {
                ThrowIfReadOnly();
                _minWorkerThreads = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set the upper limit of threads in the pool.
	    /// </summary>
	    public virtual int MaxWorkerThreads
	    {
	        get { return _maxWorkerThreads; }
	        set 
            {
                ThrowIfReadOnly();
                _maxWorkerThreads = value; 
            }
	    }

#if !(WINDOWS_PHONE)
	    /// <summary>
	    /// Get/Set the scheduling priority of the threads in the pool.
	    /// The Os handles the scheduling.
	    /// </summary>
	    public virtual ThreadPriority ThreadPriority
	    {
	        get { return _threadPriority; }
	        set 
            {
                ThrowIfReadOnly();
                _threadPriority = value; 
            }
	    }
#endif
        /// <summary>
        /// Get/Set the thread pool name. Threads will get names depending on this.
        /// </summary>
        public virtual string ThreadPoolName {
            get { return _threadPoolName; }
            set
            {
                ThrowIfReadOnly ();
                _threadPoolName = value;
            }
        }

	    /// <summary>
	    /// Get/Set the performance counter instance name of this SmartThreadPool
	    /// The default is null which indicate not to use performance counters at all.
	    /// </summary>
	    public virtual string PerformanceCounterInstanceName
	    {
	        get { return _performanceCounterInstanceName; }
	        set 
            {
                ThrowIfReadOnly();
                _performanceCounterInstanceName = value; 
            }
	    }

        /// <summary>
        /// Enable/Disable the local performance counter.
        /// This enables the user to get some performance information about the SmartThreadPool 
        /// without using Windows performance counters. (Useful on WindowsCE, Silverlight, etc.)
        /// The default is false.
        /// </summary>
        public virtual bool EnableLocalPerformanceCounters
	    {
	        get { return _enableLocalPerformanceCounters; }
	        set 
			{ 
				ThrowIfReadOnly(); 
				_enableLocalPerformanceCounters = value; 
			}
	    }

        /// <summary>
        /// Get/Set backgroundness of thread in thread pool.
        /// </summary>
	    public virtual bool AreThreadsBackground
 	    {
 	        get { return _areThreadsBackground; }
 	        set
 	        {
 	            ThrowIfReadOnly ();
 	            _areThreadsBackground = value;
 	        }
 	    }

	    /// <summary>
        /// Get a readonly version of this STPStartInfo.
        /// </summary>
        /// <returns>Returns a readonly reference to this STPStartInfo</returns>
        public new STPStartInfo AsReadOnly()
        {
            return new STPStartInfo(this) { _readOnly = true };
        }

#if !(_SILVERLIGHT) && !(WINDOWS_PHONE)

        private ApartmentState _apartmentState = SmartThreadPool.DefaultApartmentState;

        /// <summary>
        /// Get/Set the apartment state of threads in the thread pool
        /// </summary>
        public ApartmentState ApartmentState
        {
            get { return _apartmentState; }
            set
            {
                ThrowIfReadOnly();
                _apartmentState = value;
            }
        } 

#if !(_SILVERLIGHT) && !(WINDOWS_PHONE)
        
        /// <summary>
        /// Get/Set the max stack size of threads in the thread pool
        /// </summary>
        public int? MaxStackSize
        {
            get { return _maxStackSize; }
            set
            {
                ThrowIfReadOnly();
                if (value.HasValue && value.Value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Value must be greater than 0.");
                }
                _maxStackSize = value;
            }
        }
#endif

#endif
    }
}