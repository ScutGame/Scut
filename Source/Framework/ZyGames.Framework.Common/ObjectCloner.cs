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
using System.Reflection;

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
        /// 
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

            object clone = null;

            try
            {
                clone = Activator.CreateInstance(model.GetType());
            }
            catch
            {
            }

            cloneDictionary[model] = clone;

            if (clone == null)
                return null;

            foreach (PropertyInfo prop in model.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead)
                    continue;

                if (ExcludeProperty(model, flavor, prop))
                    continue;

                if (prop.PropertyType.GetInterface("IList") != null)
                {
                    IList origCollection = prop.GetValue(model, null) as IList;
                    if (origCollection == null)
                    {
                        if (prop.CanWrite)
                            prop.SetValue(clone, null, null);

                        continue;
                    }

                    IList cloneCollection = prop.GetValue(clone, null) as IList;

                    Type t = origCollection.GetType();

                    if (t.IsArray)
                    {
                        if (prop.CanWrite)
                        {
                            cloneCollection = (origCollection as Array).Clone() as IList;
                            prop.SetValue(clone, cloneCollection, null);
                        }
                    }
                    else
                    {
                        if (cloneCollection == null)
                        {
                            cloneCollection = Activator.CreateInstance(t) as IList;
                            prop.SetValue(clone, cloneCollection, null);
                        }

                        foreach (object elem in origCollection)
                        {
                            cloneCollection.Add(Clone(elem, flavor, cloneDictionary));
                        }
                    }

                    continue;
                }

                if (!prop.CanWrite)
                    continue;

                if (prop.PropertyType.IsValueType)
                {
                    prop.SetValue(clone, prop.GetValue(model, null), null);
                }
                else
                {
                    prop.SetValue(clone, Clone(prop.GetValue(model, null), flavor, cloneDictionary), null);
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
