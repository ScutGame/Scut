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
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;

namespace ZyGames.Tianjiexing.BLL.WebService
{
    public class JsonParameter
    {
        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public static JsonParameterList Convert(JsonParameter[] parameters)
        {
            JsonParameterList ht = new JsonParameterList();
            foreach (JsonParameter param in parameters)
            {
                ht.Add(param);
            }
            return ht;
        }
    }
    public class JsonParameterList
    {
        private Hashtable ht = new Hashtable();

        public void Add(JsonParameter param)
        {
            if (!ht.ContainsKey(param.Key))
            {
                ht.Add(param.Key, param.Value);
            }
            else
            {
                ht[param.Key] = param.Value;
            }
        }

        public string this[string key]
        {
            get
            {
                if (ht.ContainsKey(key))
                {
                    return ht[key].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}