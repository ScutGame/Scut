using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace BLL
{
   public class JsonProperty
    {
        public JsonProperty(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }
    }


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
