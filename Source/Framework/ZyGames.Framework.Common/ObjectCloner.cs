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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZyGames.Framework.Common.Reflect;

namespace ZyGames.Framework.Common
{
    /// <summary>
    /// 
    /// </summary>
    public enum CloneableTag
    {
        /// <summary>
        /// 
        /// </summary>
        Include,
        /// <summary>
        /// 
        /// </summary>
        Exclude
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CloneableAttribute : Attribute
    {
        private CloneableTag _tag;
        private string _flavor;


        /// <summary>
        /// 
        /// </summary>
        public CloneableAttribute()
            : this(CloneableTag.Include)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public CloneableAttribute(CloneableTag tag)
            : this(tag, string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="flavor"></param>
        public CloneableAttribute(CloneableTag tag, string flavor)
        {
            _tag = tag;
            _flavor = flavor;
        }


        /// <summary>
        /// 
        /// </summary>
        public CloneableTag Tag
        {
            get { return _tag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Flavor
        {
            get { return _flavor; }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class ObjectCloner
    {
        /// <summary>
        /// Depth copy property and field of object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object Clone(object model)
        {
            return Clone(model, string.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="flavor"></param>
        /// <returns></returns>
        public static object Clone(object model, string flavor)
        {
            Dictionary<object, object> cloneDictionary = new Dictionary<object, object>();
            return Clone(model, flavor, cloneDictionary);
        }


        private static object Clone(object model, string flavor, Dictionary<object, object> cloneDictionary)
        {
            if (model == null)
                return null;

            if (model.GetType().IsValueType)
                return model;

            if (model is string)
                return model;

            if (cloneDictionary.ContainsKey(model))
                return cloneDictionary[model];

            object clone = FastActivator.Create(model.GetType());
            cloneDictionary[model] = clone;

            if (clone == null)
                return null;

            object value = null;

            foreach (PropertyInfo prop in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.GetField | BindingFlags.SetField))
            {
                if (!prop.CanRead ||
                    !prop.CanWrite ||
                    ExcludeProperty(model, flavor, prop) ||
                    prop.GetIndexParameters().Length > 0
                    )
                {
                    continue;
                }

                if (prop.PropertyType.GetInterface(typeof(IDictionary<,>).Name) != null)
                {
                    var origCollection = prop.GetValue(model, null) as IEnumerable;
                    dynamic cloneCollection = null;
                    if (origCollection != null)
                    {
                        cloneCollection = FastActivator.Create(prop.PropertyType);
                        foreach (dynamic elem in origCollection)
                        {
                            var key = Clone(elem.Key, flavor, cloneDictionary);
                            var obj = Clone(elem.Value, flavor, cloneDictionary);
                            cloneCollection.Add(key, obj);
                        }
                    }
                    prop.SetValue(clone, cloneCollection, null);
                    continue;
                }
                if (prop.PropertyType.GetInterface(typeof(ICollection<>).Name) != null)
                {
                    var origCollection = prop.GetValue(model, null) as IEnumerable;
                    if (origCollection == null)
                    {
                        prop.SetValue(clone, null, null);
                        continue;
                    }
                    dynamic cloneCollection = null;
                    if (prop.PropertyType.IsArray)
                    {
                        //cloneCollection = (origCollection as Array).Clone();
                        Type elemType = prop.PropertyType.GetElementType();
                        dynamic list = FastActivator.Create(Type.GetType(string.Format("{0}[[{1},{2}]]", typeof(List<>).FullName, elemType.FullName, elemType.Assembly.GetName())));
                        foreach (var elem in origCollection)
                        {
                            dynamic obj = Clone(elem, flavor, cloneDictionary);
                            list.Add(obj);
                        }
                        cloneCollection = list.ToArray();
                    }
                    else
                    {
                        cloneCollection = FastActivator.Create(prop.PropertyType);
                        foreach (var elem in origCollection)
                        {
                            dynamic obj = Clone(elem, flavor, cloneDictionary);
                            cloneCollection.Add(obj);
                        }
                    }
                    prop.SetValue(clone, cloneCollection, null);
                    continue;
                }
                try
                {
                    value = prop.GetValue(model, null);
                }
                catch (Exception)
                {
                }

                if (prop.PropertyType.IsValueType)
                {
                    prop.SetValue(clone, value, null);
                }
                else
                {
                    var obj = Clone(value, flavor, cloneDictionary);
                    try
                    {
                        prop.SetValue(clone, obj, null);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return clone;
        }


        private static bool ExcludeProperty(object model, string flavor, PropertyInfo prop)
        {
            bool excluded = false;
            object[] attributes = prop.GetCustomAttributes(typeof(CloneableAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                foreach (CloneableAttribute attrib in attributes)
                {
                    if (string.IsNullOrEmpty(attrib.Flavor))
                    {
                        if (attrib.Tag == CloneableTag.Exclude)
                            excluded = true;
                    }
                    else
                    {
                        if (attrib.Flavor == flavor)
                        {
                            if (attrib.Tag == CloneableTag.Include)
                            {
                                excluded = false;
                                break;
                            }
                        }
                    }
                }
            }

            return excluded;
        }


    }
}
