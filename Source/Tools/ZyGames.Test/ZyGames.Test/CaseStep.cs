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
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Common.Security;
using ZyGames.Framework.RPC.IO;
using ZyGames.Framework.Script;
using ZyGames.Test.Net;

namespace ZyGames.Test
{
    public abstract class CaseStep
    {
        public static CaseStep Create(string formatType, string stepName)
        {
            string typeName = string.Format(formatType, stepName);
            string code = string.Format(ConfigUtils.GetSetting("CaseStep.Script.Format", "Case.Step{0}.cs"), stepName);
            var instance = ScriptEngines.Execute(code, typeName);
            //var instance = type.CreateInstance<CaseStep>();
            if (instance == null)
            {
                throw new NullReferenceException(string.Format("Get CaseStep object is null, type:{1}, script code:{0}", code, typeName));
            }
            instance.Action = stepName;
            return instance;
        }

        protected ThreadSession _session;
        protected StepTimer _stepTimer;
        private Dictionary<string, string> _params;

        protected CaseStep()
        {
            _stepTimer = new StepTimer();
            Action = "";
            _params = new Dictionary<string, string>();
            Runtimes = 1;
        }

        public string Action { get; private set; }

        public StepTimer Timer
        {
            get { return _stepTimer; }
        }

        /// <summary>
        /// Running times of each thread.
        /// </summary>
        public int Runtimes { get; set; }

        /// <summary>
        /// Child run step.
        /// </summary>
        public CaseStep ChildStep { get; private set; }

        protected void SetRequestParam(string key, object value)
        {
            _params[key] = value.ToString();
        }

        internal protected virtual void Init(ThreadSession session)
        {
            _session = session;
            int msgId = _stepTimer.Runtimes + 1;
            SetRequestParam("MsgId", msgId);
            SetRequestParam("Sid", _session.Context.SessionId);
            SetRequestParam("Uid", _session.Context.UserId);
            SetRequestParam("ActionId", Action);
        }


        private byte[] GetRequestData()
        {
            StringBuilder param = new StringBuilder();
            var pairs = _params.ToArray();
            foreach (var pair in pairs)
            {
                if (param.Length > 0)
                {
                    param.Append("&");
                }
                param.AppendFormat("{0}={1}", pair.Key, NetProxy.Encoding(pair.Value));
            }
            return Encoding.UTF8.GetBytes(string.Format("?d={0}", NetProxy.GetSign(param.ToString(), _session.Setting.SignKey)));
        }

        public virtual void StartRun()
        {
            try
            {
                //todo: connect
                InitConnect();
                SetUrlElement();
                var sendData = GetRequestData();
                for (int i = 0; i < Runtimes; i++)
                {
                    DoRunProcess(sendData);
                }
                DoResult();

            }
            catch (Exception e)
            {
                TraceLog.WriteError("StartRun error:{0}", e);
            }
        }

        private void DoRunProcess(byte[] sendData)
        {
            bool success = false;
            try
            {
                MessageStructure reader = null;
                MessageHead head = null;
                _stepTimer.StartTime();
                _session.Proxy.SendAsync(sendData, data =>
                {
                    try
                    {
                        if (data.Length == 0) return true;
                        reader = new MessageStructure(data);
                        head = reader.ReadHeadGzip();
                        return head.Action.ToString() == Action;
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteError("Step {0} error:{1}", Action, ex);
                        return false;
                    }
                });

                if (CheckCompleted(head) && DecodePacket(reader, head))
                {
                    success = true;
                    _stepTimer.Completed();
                }
            }
            catch (Exception ex)
            {
                _stepTimer.PushError(ex.Message);
            }
            finally
            {
                _stepTimer.StopTime();
            }

            if (success && ChildStep != null)
            {
                ChildStep.StartRun();
            }
        }

        protected virtual bool CheckCompleted(MessageHead head)
        {
            if (head == null || !Equals(head.Action.ToString(), Action))
            {
                _stepTimer.PushError(string.Format("Step {0} pid:{1} request timeout.", Action,
                    _session.Context.PassportId));
                return false;
            }

            if (head.ErrorCode >= 10000)
            {
                _stepTimer.PushError(string.Format("Step {0} pid:{1} request error:{2}-{3}",
                    Action,
                    _session.Context.PassportId,
                    head.ErrorCode,
                    head.ErrorInfo));
                return false;
            }
            return true;
        }

        private void InitConnect()
        {
            _session.InitConnect();
        }

        protected void SetChildStep(string stepName)
        {
            CaseStep caseStep = null;
            if (!string.IsNullOrEmpty(stepName))
            {
                caseStep = Create(_session.Setting.CaseStepTypeFormat, stepName);
                if (caseStep == null) throw new Exception(string.Format(_session.Setting.CaseStepTypeFormat, stepName) + " isn't found.");
                caseStep.Runtimes = 1;
                caseStep.Init(_session);
            }
            ChildStep = caseStep;
        }

        protected abstract void SetUrlElement();

        protected abstract bool DecodePacket(MessageStructure reader, MessageHead head);

        private void DoResult()
        {
            _stepTimer.DoResult();
        }

        protected string EncodePassword(string pwd)
        {
            return new DESAlgorithmNew().EncodePwd(pwd, _session.Setting.EncodePwdKey);
        }
    }
}