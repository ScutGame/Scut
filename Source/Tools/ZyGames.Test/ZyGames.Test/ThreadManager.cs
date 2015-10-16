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
using System.Threading.Tasks;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Script;

namespace ZyGames.Test
{
    public static class ThreadManager
    {
        public static string RunTest(TaskSetting setting)
        {
            var sessionList = new List<ThreadSession>();
            for (int i = 0; i < setting.ThreadNum; i++)
            {
                sessionList.Add(new ThreadSession(i) { Setting = setting });
            }

            StringBuilder mainBuilder = new StringBuilder();
            StringBuilder stepBuilder = new StringBuilder();
            int errorCount = 0;
            if (setting.CaseStepList.Count == 0)
            {
                mainBuilder.AppendFormat("CaseStep is not setting.");
                mainBuilder.AppendLine();
            }
            else
            {
                foreach (var step in setting.CaseStepList)
                {
                    var arr = step.Split('=');
                    string stepName = arr[0];
                    int runtimes = setting.Runtimes;
                    if (arr.Length > 1)
                    {
                        runtimes = arr[1].ToInt();
                    }
                    errorCount += RunStep(stepBuilder, sessionList, setting, stepName, runtimes);
                }
            }

            DoTotalResult(mainBuilder, setting, errorCount);
            mainBuilder.Append(stepBuilder);
            mainBuilder.AppendLine("End testing.");
            return mainBuilder.ToString();
        }

        internal static int RunStep(StringBuilder stepBuilder, List<ThreadSession> sessionList, TaskSetting setting, string stepName, int runtimes)
        {
            CaseStep[] caseList = new CaseStep[sessionList.Count];
            Task[] taskList = new Task[sessionList.Count];
            for (int i = 0; i < taskList.Length; i++)
            {
                var session = sessionList[i];
                var caseStep = CaseStep.Create(setting.CaseStepTypeFormat, stepName);
                if (caseStep != null)
                {
                    caseStep.Runtimes = runtimes;
                    caseStep.Init(session);
                    caseList[i] = caseStep;
                    taskList[i] = Task.Factory.StartNew(caseStep.StartRun);
                }
            }
            if (setting.WaitTimeout == TimeSpan.Zero)
            {
                Task.WaitAll(taskList);
            }
            else
            {
                Task.WaitAll(taskList, setting.WaitTimeout);
            }

            int errorCount = caseList.Sum(t => t.Timer.FailNum);
            DoStepResult(stepBuilder, stepName, caseList);
            return errorCount;
        }

        private static void DoTotalResult(StringBuilder writer, TaskSetting setting, int errorCount)
        {
            writer.AppendLine("---------------------------------------------------");
            writer.AppendFormat("ThreadNum:\t{0}", setting.ThreadNum);
            writer.AppendLine();
            writer.AppendFormat("Runtimes:\t{0}", setting.Runtimes);
            writer.AppendLine();
            writer.AppendFormat("Run steps:\t{0}", string.Join(",", setting.CaseStepList));
            writer.AppendLine();
            writer.AppendFormat("Faild count:\t{0}", errorCount);
            writer.AppendLine();
        }

        private static void DoStepResult(StringBuilder writer, string stepName, ICollection<CaseStep> steps, int depth = 0)
        {
            string preChar = "".PadLeft(depth * 4);
            double minTime = steps.Min(t => t.Timer.MinTime);
            double aveTime = steps.Average(t => t.Timer.AveTime);
            double maxTime = steps.Max(t => t.Timer.MaxTime);
            int successNum = steps.Sum(t => t.Timer.SuccessNum);
            int failNum = steps.Sum(t => t.Timer.FailNum);
            if (failNum > 0)
            {
                string error = string.Join("", steps.Select(t => t.Timer.Error).ToList());
                TraceLog.WriteError("{0}", error);
            }
            writer.AppendFormat("{0}>>Step {1}: success:{2}, fail:{3}", preChar, stepName, successNum, failNum);
            writer.AppendLine();
            writer.AppendFormat("{0}    AVE:\t{1}ms", preChar, aveTime.ToString("F6"));
            writer.AppendLine();
            writer.AppendFormat("{0}    Min:\t{1}ms", preChar, minTime.ToString("F6"));
            writer.AppendLine();
            writer.AppendFormat("{0}    Max:\t{1}ms", preChar, maxTime.ToString("F6"));
            writer.AppendLine();
            var childs = steps.Where(t => t.ChildStep != null).Select(t => t.ChildStep).ToArray();
            if (childs.Length > 0)
            {
                DoStepResult(writer, childs[0].Action, childs, depth + 1);
            }
        }
    }
}

