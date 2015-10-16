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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Model;

namespace ZyGames.Framework.Game.Task
{
    /// <summary>
    /// 任务配置信息
    /// </summary>
    public abstract class TaskBaseConfig : ShareEntity
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Task.TaskBaseConfig"/> class.
		/// </summary>
        protected TaskBaseConfig()
            : base(AccessLevel.ReadOnly)
        {

        }
		/// <summary>
		/// Initializes a new instance of the <see cref="ZyGames.Framework.Game.Task.TaskBaseConfig"/> class.
		/// </summary>
		/// <param name="taskID">Task I.</param>
        protected TaskBaseConfig(int taskID)
            : base(AccessLevel.ReadOnly)
        {
            TaskID = taskID;
        }

        /// <summary>
        /// 任务配置
        /// </summary>
        public virtual int TaskID
        {
            get;
            set;
        }
    }
}