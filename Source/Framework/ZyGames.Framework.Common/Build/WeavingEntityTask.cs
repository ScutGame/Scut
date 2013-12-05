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
using System.IO;
using System.Linq;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace ZyGames.Framework.Common.Build
{
    /// <summary>
    /// 实体属性重构生成
    /// </summary>
    public class WeavingEntityTask : Task
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            SearchForPropertiesAndAddMSIL();
            return true;
        }

        private void SearchForPropertiesAndAddMSIL()
        {
            try
            {
                var pathList = Directory.GetFiles(SolutionDir, (FilePattern ?? "*.dll"), SearchOption.AllDirectories);
                foreach (string assemblyPath in pathList)
                {
                    bool setSuccess = false;
                    var ass = AssemblyDefinition.ReadAssembly(assemblyPath);
                    var types = ass.MainModule.Types.Where(p => !p.IsEnum).ToList();
                    foreach (TypeDefinition type in types)
                    {
                        setSuccess = ProcessEntityType(type, setSuccess);
                    }

                    if (setSuccess)
                    {
                        ass.Write(assemblyPath);
                        Log.LogMessage("重建实体" + assemblyPath + "成功！");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError("重建实体dll异常:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private bool ProcessEntityType(TypeDefinition type, bool setSuccess)
        {
            if (type.BaseType != null && type.BaseType.Name == "EntityChangeEvent")
            {
                //子类定义模式
                foreach (PropertyDefinition prop in type.Properties)
                {
                    setSuccess = SetChildNotifyMethod(type, prop, setSuccess);
                }
            }
            else
            {
                foreach (PropertyDefinition prop in type.Properties)
                {
                    foreach (CustomAttribute attribute in prop.CustomAttributes)
                    {
                        if (attribute.Constructor != null &&
                            attribute.Constructor.DeclaringType != null &&
                            attribute.Constructor.DeclaringType.Name == "EntityFieldAttribute")
                        {
                            setSuccess = SetChangePropertyMethod(type, prop, setSuccess);
                        }
                    }
                }
            }
            return setSuccess;
        }

        private bool SetChildNotifyMethod(TypeDefinition type, PropertyDefinition prop, bool setSuccess)
        {
            TypeDefinition typeDefinition = FindBaseTypeDefinition(type.BaseType, "EntityChangeEvent");
            if (typeDefinition == null)
            {
                return setSuccess;
            }
            MethodDefinition baseMethod = typeDefinition.Methods.First(m => m.Name == "NotifyByModify");
            if (baseMethod == null)
            {
                return setSuccess;
            }
            MethodReference method = type.Module.Import(baseMethod);
            Instruction ins = null;
            if (prop.SetMethod == null)
            {
                return setSuccess;
            }
            if (prop.SetMethod.Body.Instructions.Count <= 4)
            {
                ILProcessor worker = prop.SetMethod.Body.GetILProcessor();
                ins = prop.SetMethod.Body.Instructions[prop.SetMethod.Body.Instructions.Count - 1];

                //定义这个字符
                worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
                //写入通知
                worker.InsertBefore(ins, worker.Create(OpCodes.Call, method));
                setSuccess = true;
            }
            return setSuccess;
        }

        private bool SetChangePropertyMethod(TypeDefinition type, PropertyDefinition prop, bool setSuccess)
        {
            string propName = prop.Name;
            ILProcessor worker = null;
            TypeDefinition typeDefinition = FindBaseTypeDefinition(type.BaseType, "AbstractEntity");
            if (typeDefinition == null)
            {
                return setSuccess;
            }
            MethodDefinition baseMethod = typeDefinition.Methods.First(m => m.Name == "SetChangeProperty");
            if (baseMethod == null)
            {
                return setSuccess;
            }
            MethodReference method = type.Module.Import(baseMethod);
            Instruction ins = null;

            if (prop.SetMethod == null)
            {
                return setSuccess;
                //MethodDefinition setMethod = new MethodDefinition("set_" + propName,
                //                                                  MethodAttributes.Private,
                //                                                  prop.GetMethod.ReturnType);
                //MethodBody setBody = new MethodBody(setMethod);
                //prop.SetMethod = setMethod;
                //worker = setBody.GetILProcessor();
                //prop.SetMethod.Body.Instructions.Add(worker.Create(OpCodes.Ret));
                //ins = prop.SetMethod.Body.Instructions[0];
                //FieldDefinition field = type.Fields.FirstOrDefault(p => p.FullName.EndsWith(propName, StringComparison.CurrentCultureIgnoreCase));
                //if (field != null)
                //{
                //    worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
                //    worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_1));
                //    worker.InsertBefore(ins, worker.Create(OpCodes.Stfld, field));
                //    ins = prop.SetMethod.Body.Instructions[prop.SetMethod.Body.Instructions.Count - 1];
                //}
            }
            else
            {
                worker = prop.SetMethod.Body.GetILProcessor();
                ins = prop.SetMethod.Body.Instructions[prop.SetMethod.Body.Instructions.Count - 1];
                if (prop.SetMethod.Body.Instructions.Count > 4)
                {
                    FieldDefinition field = type.Fields.FirstOrDefault(p => p.Name.Equals("_" + propName, StringComparison.CurrentCultureIgnoreCase));
                    if (field != null)
                    {
                        while (prop.SetMethod.Body.Instructions.Count > 1)
                        {
                            prop.SetMethod.Body.Instructions.RemoveAt(0);
                        }
                        ins = prop.SetMethod.Body.Instructions[0];

                        worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
                        worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_1));
                        worker.InsertBefore(ins, worker.Create(OpCodes.Stfld, field));

                        ins = prop.SetMethod.Body.Instructions[prop.SetMethod.Body.Instructions.Count - 1];
                    }
                    else
                    {
                        return setSuccess;
                    }
                }
            }

            //定义这个字符
            worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
            //定义这个字符
            worker.InsertBefore(ins, worker.Create(OpCodes.Ldstr, prop.Name));
            //写入通知
            worker.InsertBefore(ins, worker.Create(OpCodes.Call, method));
            setSuccess = true;
            return setSuccess;
        }

        private static TypeDefinition FindBaseTypeDefinition(TypeReference type, string name)
        {
            if (type == null || type.Module == null) return null;
            TypeDefinition typeDefinition = null;

            string fileName = new FileInfo(type.Module.FullyQualifiedName).DirectoryName + "\\" + type.Scope.Name;
            if (!fileName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
            {
                fileName += ".dll";
            }
            var md = ModuleDefinition.ReadModule(fileName);
            if (md != null && md.Assembly != null)
            {
                AssemblyDefinition ad = md.Assembly;
                typeDefinition = ad.MainModule.GetType(type.Namespace, type.Name);
            }
            if (typeDefinition != null)
            {
                typeDefinition = typeDefinition.Resolve();
                if (string.Equals(typeDefinition.Name, name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return typeDefinition;
                }
                if (typeDefinition.BaseType != null)
                {
                    return FindBaseTypeDefinition(typeDefinition.BaseType, name);
                }
            }
            return typeDefinition;
        }
        [Required]
        public string SolutionDir { get; set; }

        /// <summary>
        /// 过滤文件类型
        /// </summary>
        public string FilePattern { get; set; }
    }
}