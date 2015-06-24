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
using System.Web;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Service;


namespace ZyGames.DirCenter
{
    public partial class Service : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Charset = "unicode";// "unicode";
            HttpGet httpGet = new HttpGet(HttpContext.Current.Request);
            IGameResponse gameResponse = new HttpGameResponse(response);
            String ActionID = string.Empty;
            if (httpGet.GetString("ActionID", ref ActionID))
            {
                try
                {
                    string actionName = string.Concat("Action", ActionID);
                    string sname = string.Concat("ZyGames.DirCenter.Action." + actionName);
                    object[] args = new object[1];
                    args[0] = response;
                   
                    BaseStruct obj = (BaseStruct) Activator.CreateInstance(Type.GetType(sname), new object[] { httpGet });
                    obj.DoInit();
                    if (obj.ReadUrlElement() && obj.DoAction() && !obj.GetError())
                    {
                        obj.BuildPacket();
                        obj.WriteAction(gameResponse);
                    }
                    else
                    {
                        obj.WriteErrorAction(gameResponse);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    BaseLog oBaseLog = new BaseLog("DirCenterErrMain");
                    oBaseLog.SaveLog(ex);
                }
            }
        }
    }
}