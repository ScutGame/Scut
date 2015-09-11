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
using System.Runtime.Serialization;
#if !(_WINDOWS_CE)

#endif

namespace ZyGames.Framework.Common.Threading
{
    #region Exceptions

    /// <summary>
    /// Represents an exception in case IWorkItemResult.GetResult has been canceled
    /// </summary>
    public sealed partial class WorkItemCancelException : Exception
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemCancelException"/> class.
		/// </summary>
        public WorkItemCancelException()
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemCancelException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
        public WorkItemCancelException(string message)
            : base(message)
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemCancelException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="e">E.</param>
        public WorkItemCancelException(string message, Exception e)
            : base(message, e)
        {
        }
    }

    /// <summary>
    /// Represents an exception in case IWorkItemResult.GetResult has been timed out
    /// </summary>
    public sealed partial class WorkItemTimeoutException : Exception
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemTimeoutException"/> class.
		/// </summary>
        public WorkItemTimeoutException()
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemTimeoutException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
        public WorkItemTimeoutException(string message)
            : base(message)
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemTimeoutException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="e">E.</param>
        public WorkItemTimeoutException(string message, Exception e)
            : base(message, e)
        {
        }
    }

    /// <summary>
    /// Represents an exception in case IWorkItemResult.GetResult has been timed out
    /// </summary>
    public sealed partial class WorkItemResultException : Exception
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemResultException"/> class.
		/// </summary>
        public WorkItemResultException()
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemResultException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
        public WorkItemResultException(string message)
            : base(message)
        {
        }
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemResultException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="e">E.</param>
        public WorkItemResultException(string message, Exception e)
            : base(message, e)
        {
        }
    }


#if !(_WINDOWS_CE) && !(_SILVERLIGHT) && !(WINDOWS_PHONE)
    /// <summary>
    /// Represents an exception in case IWorkItemResult.GetResult has been canceled
    /// </summary>
    [Serializable]
    public sealed partial class WorkItemCancelException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemCancelException"/> class.
		/// </summary>
		/// <param name="si">Si.</param>
		/// <param name="sc">Sc.</param>
        public WorkItemCancelException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }
    }

    /// <summary>
    /// Represents an exception in case IWorkItemResult.GetResult has been timed out
    /// </summary>
    [Serializable]
    public sealed partial class WorkItemTimeoutException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemTimeoutException"/> class.
		/// </summary>
		/// <param name="si">Si.</param>
		/// <param name="sc">Sc.</param>
        public WorkItemTimeoutException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }
    }

    /// <summary>
    /// Represents an exception in case IWorkItemResult.GetResult has been timed out
    /// </summary>
    [Serializable]
    public sealed partial class WorkItemResultException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkItemResultException"/> class.
		/// </summary>
		/// <param name="si">Si.</param>
		/// <param name="sc">Sc.</param>
        public WorkItemResultException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }
    }

#endif

    #endregion
}