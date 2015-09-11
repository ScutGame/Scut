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

namespace ZyGames.Framework.Common.Threading
{
	#region WorkItemInfo class

	/// <summary>
	/// Summary description for WorkItemInfo.
	/// </summary>
	public class WorkItemInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemInfo"/> class.
		/// </summary>
	    public WorkItemInfo()
		{
			UseCallerCallContext = SmartThreadPool.DefaultUseCallerCallContext;
			UseCallerHttpContext = SmartThreadPool.DefaultUseCallerHttpContext;
			DisposeOfStateObjects = SmartThreadPool.DefaultDisposeOfStateObjects;
			CallToPostExecute = SmartThreadPool.DefaultCallToPostExecute;
			PostExecuteWorkItemCallback = SmartThreadPool.DefaultPostExecuteWorkItemCallback;
			WorkItemPriority = SmartThreadPool.DefaultWorkItemPriority;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemInfo"/> class.
		/// </summary>
		/// <param name="workItemInfo">Work item info.</param>
		public WorkItemInfo(WorkItemInfo workItemInfo)
		{
			UseCallerCallContext = workItemInfo.UseCallerCallContext;
			UseCallerHttpContext = workItemInfo.UseCallerHttpContext;
			DisposeOfStateObjects = workItemInfo.DisposeOfStateObjects;
			CallToPostExecute = workItemInfo.CallToPostExecute;
			PostExecuteWorkItemCallback = workItemInfo.PostExecuteWorkItemCallback;
			WorkItemPriority = workItemInfo.WorkItemPriority;
            Timeout = workItemInfo.Timeout;
		}

	    /// <summary>
	    /// Get/Set if to use the caller's security context
	    /// </summary>
	    public bool UseCallerCallContext { get; set; }

	    /// <summary>
	    /// Get/Set if to use the caller's HTTP context
	    /// </summary>
	    public bool UseCallerHttpContext { get; set; }

	    /// <summary>
	    /// Get/Set if to dispose of the state object of a work item
	    /// </summary>
	    public bool DisposeOfStateObjects { get; set; }

	    /// <summary>
	    /// Get/Set the run the post execute options
	    /// </summary>
        public CallToPostExecute CallToPostExecute { get; set; }

	    /// <summary>
	    /// Get/Set the post execute callback
	    /// </summary>
        public PostExecuteWorkItemCallback PostExecuteWorkItemCallback { get; set; }

	    /// <summary>
	    /// Get/Set the work item's priority
	    /// </summary>
	    public WorkItemPriority WorkItemPriority { get; set; }

	    /// <summary>
	    /// Get/Set the work item's timout in milliseconds.
        /// This is a passive timout. When the timout expires the work item won't be actively aborted!
	    /// </summary>
	    public long Timeout { get; set; }
	}

	#endregion
}