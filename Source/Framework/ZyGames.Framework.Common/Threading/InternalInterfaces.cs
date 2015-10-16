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
    /// <summary>
    /// An internal delegate to call when the WorkItem starts or completes
    /// </summary>
    internal delegate void WorkItemStateCallback(WorkItem workItem);

    internal interface IInternalWorkItemResult
    {
        event WorkItemStateCallback OnWorkItemStarted;
        event WorkItemStateCallback OnWorkItemCompleted;
    }

    internal interface IInternalWaitableResult
    {
        /// <summary>
        /// This method is intent for internal use.
        /// </summary>   
        IWorkItemResult GetWorkItemResult();
    }
	/// <summary>
	/// I has work item priority.
	/// </summary>
    public interface IHasWorkItemPriority
    {
		/// <summary>
		/// Gets the work item priority.
		/// </summary>
		/// <value>The work item priority.</value>
        WorkItemPriority WorkItemPriority { get; }
    }
}