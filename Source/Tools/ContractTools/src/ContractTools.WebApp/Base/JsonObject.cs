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
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace ContractTools.WebApp.Base
{
    public class JsonObject
    {
        private static string FormatExpress = "\"{0}\":\"{1}\"";
        private static string SplitChar = ",";
        private List<JsonProperty> PropertyList = new List<JsonProperty>();
        private object entiry;

        public JsonObject()
        {
        }

        public JsonObject(object obj)
        {
            entiry = obj;
        }

        public JsonObject Add(string name, object value)
        {
            Add(new JsonProperty(name, value));
            return this;
        }

        public JsonObject AddObject<T>(T t)
        {
            PropertyInfo[] fieldList = t.GetType().GetProperties();
            foreach (PropertyInfo field in fieldList)
            {
                Add(field.Name, field.GetValue(t, null));
            }
            return this;
        }

        public JsonObject Add(JsonProperty property)
        {
            PropertyList.Add(property);
            return this;
        }

        public JsonObject Add(string name, JsonObject json)
        {
            PropertyList.Add(new JsonProperty(name, json));
            return this;
        }

        public JsonObject Add(string name, JsonObject[] jsonList)
        {
            PropertyList.Add(new JsonProperty(name, jsonList));
            return this;
        }

        public string ToJson()
        {
            if (entiry != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(entiry);
            }
            else
            {
                StringBuilder json = new StringBuilder();
                DoJson(json, PropertyList);
                return json.ToString();
            }
        }

        private static void DoJson(StringBuilder json, List<JsonProperty> propertyList)
        {
            if (propertyList.Count == 0) return;

            json.Append("{");
            int index = 0;
            foreach (JsonProperty property in propertyList)
            {
                if (index > 0) json.Append(SplitChar);
                if (property.Value is bool)
                {
                    json.AppendFormat(FormatExpress, property.Name, (bool)property.Value ? 1 : 0);
                }
                else if (property.Value is JsonObject[])
                {
                    JsonObject[] jsonObjectList = (JsonObject[])property.Value;
                    if (string.IsNullOrEmpty(property.Name))
                    {
                        json.Append("[");
                    }
                    else
                    {
                        json.AppendFormat("\"{0}\":[", property.Name);
                    }

                    for (int i = 0; i < jsonObjectList.Length; i++)
                    {
                        if (i > 0) json.Append(SplitChar);

                        DoJson(json, jsonObjectList[i].PropertyList);
                    }
                    json.Append("]");
                }
                else if (property.Value is JsonObject)
                {
                    json.AppendFormat("\"{0}\":", property.Name);
                    DoJson(json, ((JsonObject)property.Value).PropertyList);
                }
                else
                {
                    json.AppendFormat(FormatExpress, property.Name, property.Value == null ? string.Empty : FilterChar(property.Value.ToString()));
                }

                index++;
            }

            json.Append("}");
        }

        private static string FilterChar(string str)
        {
            return str.Replace(@"'", @"\u0027")
                .Replace("\r\n", @"\r\n")
                .Replace("\n", @"\n")
                .Replace("\t", @"\t")
                .Replace(@"\", @"\\")
                .Replace("\"", "\\\"");
        }

        public static string ToJsonValue(string value)
        {
            value = value ?? string.Empty;
            return value;//.Replace("<br>", "\r\n").Replace("\'", "\"");
        }
    }
}