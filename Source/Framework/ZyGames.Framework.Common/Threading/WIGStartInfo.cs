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
	/// <summary>
	/// Summary description for WIGStartInfo.
	/// </summary>
	public class WIGStartInfo
	{
        private bool _useCallerCallContext;
        private bool _useCallerHttpContext;
        private bool _disposeOfStateObjects;
        private CallToPostExecute _callToPostExecute;
        private PostExecuteWorkItemCallback _postExecuteWorkItemCallback;
        private bool _startSuspended;
        private WorkItemPriority _workItemPriority;
        private bool _fillStateWithArgs;
		/// <summary>
		/// The _read only.
		/// </summary>
        protected bool _readOnly;
		/// <summary>
		/// Initializes a new instance of the <see cref="WIGStartInfo"/> class.
		/// </summary>
	    public WIGStartInfo()
        {
	        _fillStateWithArgs = SmartThreadPool.DefaultFillStateWithArgs;
	        _workItemPriority = SmartThreadPool.DefaultWorkItemPriority;
            _startSuspended = SmartThreadPool.DefaultStartSuspended;
            _postExecuteWorkItemCallback = SmartThreadPool.DefaultPostExecuteWorkItemCallback;
            _callToPostExecute = SmartThreadPool.DefaultCallToPostExecute;
            _disposeOfStateObjects = SmartThreadPool.DefaultDisposeOfStateObjects;
            _useCallerHttpContext = SmartThreadPool.DefaultUseCallerHttpContext;
            _useCallerCallContext = SmartThreadPool.DefaultUseCallerCallContext;
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="WIGStartInfo"/> class.
		/// </summary>
		/// <param name="wigStartInfo">Wig start info.</param>
	    public WIGStartInfo(WIGStartInfo wigStartInfo)
        {
            _useCallerCallContext = wigStartInfo.UseCallerCallContext;
            _useCallerHttpContext = wigStartInfo.UseCallerHttpContext;
            _disposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
            _callToPostExecute = wigStartInfo.CallToPostExecute;
            _postExecuteWorkItemCallback = wigStartInfo.PostExecuteWorkItemCallback;
            _workItemPriority = wigStartInfo.WorkItemPriority;
            _startSuspended = wigStartInfo.StartSuspended;
            _fillStateWithArgs = wigStartInfo.FillStateWithArgs;
        }
		/// <summary>
		/// Throws if read only.
		/// </summary>
        protected void ThrowIfReadOnly()
        {
            if (_readOnly)
            {
                throw new NotSupportedException("This is a readonly instance and set is not supported");
            }
        }

	    /// <summary>
	    /// Get/Set if to use the caller's security context
	    /// </summary>
	    public virtual bool UseCallerCallContext
	    {
	        get { return _useCallerCallContext; }
            set 
            { 
                ThrowIfReadOnly();  
                _useCallerCallContext = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set if to use the caller's HTTP context
	    /// </summary>
	    public virtual bool UseCallerHttpContext
	    {
	        get { return _useCallerHttpContext; }
            set 
            { 
                ThrowIfReadOnly();  
                _useCallerHttpContext = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set if to dispose of the state object of a work item
	    /// </summary>
	    public virtual bool DisposeOfStateObjects
	    {
	        get { return _disposeOfStateObjects; }
            set 
            { 
                ThrowIfReadOnly();  
                _disposeOfStateObjects = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set the run the post execute options
	    /// </summary>
	    public virtual CallToPostExecute CallToPostExecute
	    {
	        get { return _callToPostExecute; }
            set 
            { 
                ThrowIfReadOnly();  
                _callToPostExecute = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set the default post execute callback
	    /// </summary>
	    public virtual PostExecuteWorkItemCallback PostExecuteWorkItemCallback
	    {
	        get { return _postExecuteWorkItemCallback; }
            set 
            { 
                ThrowIfReadOnly();  
                _postExecuteWorkItemCallback = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set if the work items execution should be suspended until the Start()
	    /// method is called.
	    /// </summary>
	    public virtual bool StartSuspended
	    {
	        get { return _startSuspended; }
            set 
            { 
                ThrowIfReadOnly();  
                _startSuspended = value; 
            }
	    }


	    /// <summary>
	    /// Get/Set the default priority that a work item gets when it is enqueued
	    /// </summary>
	    public virtual WorkItemPriority WorkItemPriority
	    {
	        get { return _workItemPriority; }
	        set { _workItemPriority = value; }
	    }

	    /// <summary>
        /// Get/Set the if QueueWorkItem of Action&lt;...&gt;/Func&lt;...&gt; fill the
	    /// arguments as an object array into the state of the work item.
	    /// The arguments can be access later by IWorkItemResult.State.
	    /// </summary>
	    public virtual bool FillStateWithArgs
	    {
	        get { return _fillStateWithArgs; }
            set 
            { 
                ThrowIfReadOnly();  
                _fillStateWithArgs = value; 
            }
	    }

	    /// <summary>
        /// Get a readonly version of this WIGStartInfo
        /// </summary>
        /// <returns>Returns a readonly reference to this WIGStartInfoRO</returns>
        public WIGStartInfo AsReadOnly()
	    {
            return new WIGStartInfo(this) { _readOnly = true };
	    }
    }
}