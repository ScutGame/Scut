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
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using ZyGames.Framework.Plugin.PythonScript;

namespace ZyGames.Tianjiexing.BLL.WebService
{
    public class PythonScriptGetter : BaseDataGetter
    {

        public override object GetData(JsonParameter[] paramList)
        {
            string pythonCode = "";
            string pythonFunc = "Main";
            string pythonFuncArg = "";
            List<PythonParam> pyParams = new List<PythonParam>();
            foreach (var param in paramList)
            {
                switch (param.Key)
                {
                    case "_py_code":
                        pythonCode = param.Value;
                        break;
                    case "_py_func":
                        pythonFunc = param.Value;
                        break;
                    case "_py_func_arg":
                        pythonFuncArg = param.Value;
                        break;
                    default:
                        if (!string.IsNullOrEmpty(param.Key))
                        {
                            pyParams.Add(new PythonParam(param.Key, param.Value));
                        }
                        break;
                }
            }
            return PythonUtils.CallFunc(pythonFunc, "", pyParams, pythonCode);
        }
    }
}