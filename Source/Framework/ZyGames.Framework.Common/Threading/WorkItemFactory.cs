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
	#region WorkItemFactory class 
	/// <summary>
	/// Work item factory.
	/// </summary>
	public class WorkItemFactory
	{
		/// <summary>
		/// Create a new work item
		/// </summary>
		/// <param name="workItemsGroup">The WorkItemsGroup of this workitem</param>
		/// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback)
		{
			return CreateWorkItem(workItemsGroup, wigStartInfo, callback, null);
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The WorkItemsGroup of this workitem</param>
        /// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="workItemPriority">The priority of the work item</param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback, 
			WorkItemPriority workItemPriority)
		{
			return CreateWorkItem(workItemsGroup, wigStartInfo, callback, null, workItemPriority);
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The WorkItemsGroup of this workitem</param>
        /// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="workItemInfo">Work item info</param>
		/// <param name="callback">A callback to execute</param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemInfo workItemInfo, 
			WorkItemCallback callback)
		{
			return CreateWorkItem(
				workItemsGroup,
				wigStartInfo,
				workItemInfo, 
				callback, 
				null);
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The WorkItemsGroup of this workitem</param>
        /// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="state">
		/// The context object of the work item. Used for passing arguments to the work item. 
		/// </param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback, 
			object state)
		{
			ValidateCallback(callback);
            
			WorkItemInfo workItemInfo = new WorkItemInfo();
			workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
			workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
			workItemInfo.PostExecuteWorkItemCallback = wigStartInfo.PostExecuteWorkItemCallback;
			workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
			workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
            workItemInfo.WorkItemPriority = wigStartInfo.WorkItemPriority;

			WorkItem workItem = new WorkItem(
				workItemsGroup,
				workItemInfo,
				callback, 
				state);
			return workItem;
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
		/// <param name="workItemsGroup">The work items group</param>
		/// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="state">
		/// The context object of the work item. Used for passing arguments to the work item. 
		/// </param>
		/// <param name="workItemPriority">The work item priority</param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback, 
			object state, 
			WorkItemPriority workItemPriority)
		{
			ValidateCallback(callback);

			WorkItemInfo workItemInfo = new WorkItemInfo();
			workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
			workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
			workItemInfo.PostExecuteWorkItemCallback = wigStartInfo.PostExecuteWorkItemCallback;
			workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
			workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
			workItemInfo.WorkItemPriority = workItemPriority;

			WorkItem workItem = new WorkItem(
				workItemsGroup,
				workItemInfo,
				callback, 
				state);

			return workItem;
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The work items group</param>
		/// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="workItemInfo">Work item information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="state">
		/// The context object of the work item. Used for passing arguments to the work item. 
		/// </param>
		/// <returns>Returns a work item</returns>
        public static WorkItem CreateWorkItem(
            IWorkItemsGroup workItemsGroup,
            WIGStartInfo wigStartInfo,
            WorkItemInfo workItemInfo,
            WorkItemCallback callback,
            object state)
        {
            ValidateCallback(callback);
            ValidateCallback(workItemInfo.PostExecuteWorkItemCallback);

            WorkItem workItem = new WorkItem(
                workItemsGroup,
                new WorkItemInfo(workItemInfo),
                callback,
                state);

            return workItem;
        }

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The work items group</param>
		/// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="state">
		/// The context object of the work item. Used for passing arguments to the work item. 
		/// </param>
		/// <param name="postExecuteWorkItemCallback">
		/// A delegate to call after the callback completion
		/// </param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback, 
			object state,
			PostExecuteWorkItemCallback postExecuteWorkItemCallback)
		{
			ValidateCallback(callback);
			ValidateCallback(postExecuteWorkItemCallback);

			WorkItemInfo workItemInfo = new WorkItemInfo();
			workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
			workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
			workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
			workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
			workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
            workItemInfo.WorkItemPriority = wigStartInfo.WorkItemPriority;

			WorkItem workItem = new WorkItem(
				workItemsGroup,
				workItemInfo,
				callback, 
				state);

			return workItem;
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The work items group</param>
		/// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="state">
		/// The context object of the work item. Used for passing arguments to the work item. 
		/// </param>
		/// <param name="postExecuteWorkItemCallback">
		/// A delegate to call after the callback completion
		/// </param>
		/// <param name="workItemPriority">The work item priority</param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback, 
			object state,
			PostExecuteWorkItemCallback postExecuteWorkItemCallback,
			WorkItemPriority workItemPriority)
		{
			ValidateCallback(callback);
			ValidateCallback(postExecuteWorkItemCallback);

			WorkItemInfo workItemInfo = new WorkItemInfo();
			workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
			workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
			workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
			workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
			workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
			workItemInfo.WorkItemPriority = workItemPriority;

			WorkItem workItem = new WorkItem(
				workItemsGroup,
				workItemInfo,
				callback, 
				state);

			return workItem;
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The work items group</param>
		/// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="state">
		/// The context object of the work item. Used for passing arguments to the work item. 
		/// </param>
		/// <param name="postExecuteWorkItemCallback">
		/// A delegate to call after the callback completion
		/// </param>
		/// <param name="callToPostExecute">Indicates on which cases to call to the post execute callback</param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback, 
			object state,
			PostExecuteWorkItemCallback postExecuteWorkItemCallback,
			CallToPostExecute callToPostExecute)
		{
			ValidateCallback(callback);
			ValidateCallback(postExecuteWorkItemCallback);

			WorkItemInfo workItemInfo = new WorkItemInfo();
			workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
			workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
			workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
			workItemInfo.CallToPostExecute = callToPostExecute;
			workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
            workItemInfo.WorkItemPriority = wigStartInfo.WorkItemPriority;

			WorkItem workItem = new WorkItem(
				workItemsGroup,
				workItemInfo,
				callback, 
				state);

			return workItem;
		}

		/// <summary>
		/// Create a new work item
		/// </summary>
        /// <param name="workItemsGroup">The work items group</param>
		/// <param name="wigStartInfo">Work item group start information</param>
		/// <param name="callback">A callback to execute</param>
		/// <param name="state">
		/// The context object of the work item. Used for passing arguments to the work item. 
		/// </param>
		/// <param name="postExecuteWorkItemCallback">
		/// A delegate to call after the callback completion
		/// </param>
		/// <param name="callToPostExecute">Indicates on which cases to call to the post execute callback</param>
		/// <param name="workItemPriority">The work item priority</param>
		/// <returns>Returns a work item</returns>
		public static WorkItem CreateWorkItem(
			IWorkItemsGroup workItemsGroup,
			WIGStartInfo wigStartInfo,
			WorkItemCallback callback, 
			object state,
			PostExecuteWorkItemCallback postExecuteWorkItemCallback,
			CallToPostExecute callToPostExecute,
			WorkItemPriority workItemPriority)
		{

			ValidateCallback(callback);
			ValidateCallback(postExecuteWorkItemCallback);

			WorkItemInfo workItemInfo = new WorkItemInfo();
			workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
			workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
			workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
			workItemInfo.CallToPostExecute = callToPostExecute;
			workItemInfo.WorkItemPriority = workItemPriority;
			workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;

			WorkItem workItem = new WorkItem(
				workItemsGroup,
				workItemInfo,
				callback, 
				state);
			
			return workItem;
		}

		private static void ValidateCallback(Delegate callback)
		{
            if (callback != null && callback.GetInvocationList().Length > 1)
			{
				throw new NotSupportedException("SmartThreadPool doesn't support delegates chains");
			}
		}
	}

	#endregion
}