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
using System.Collections;
using System.Runtime.CompilerServices;
using System;
using Microsoft.CSharp.RuntimeBinder;

namespace ZyGames.Framework.Common.Reflect
{
    internal static class CallSiteCache
    {
        private static readonly Hashtable getters = new Hashtable(), setters = new Hashtable();

        internal static object GetValue(string name, object target)
        {
            CallSite<Func<CallSite, object, object>> callSite = (CallSite<Func<CallSite, object, object>>)getters[name];
            if (callSite == null)
            {
                CallSite<Func<CallSite, object, object>> newSite = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, name, typeof(CallSiteCache), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
                lock (getters)
                {
                    callSite = (CallSite<Func<CallSite, object, object>>)getters[name];
                    if (callSite == null)
                    {
                        getters[name] = callSite = newSite;
                    }
                }
            }
            return callSite.Target(callSite, target);
        }
        internal static void SetValue(string name, object target, object value)
        {
            CallSite<Func<CallSite, object, object, object>> callSite = (CallSite<Func<CallSite, object, object, object>>)setters[name];
            if (callSite == null)
            {
                CallSite<Func<CallSite, object, object, object>> newSite = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, name, typeof(CallSiteCache), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) }));
                lock (setters)
                {
                    callSite = (CallSite<Func<CallSite, object, object, object>>)setters[name];
                    if (callSite == null)
                    {
                        setters[name] = callSite = newSite;
                    }
                }
            }
            callSite.Target(callSite, target, value);
        }
        
    }
}