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
using System.Threading;
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
                bool hasBuild = false;
                FilePattern = (FilePattern ?? "*.dll");
                if (!FilePattern.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                {
                    FilePattern = FilePattern + ".dll";
                }
                var pathList = Directory.GetFiles(SolutionDir, FilePattern, SearchOption.AllDirectories);
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
                        hasBuild = true;
                        break;
                    }
                }
                if (!hasBuild)
                {
                    Log.LogWarning("The model:\"" + FilePattern + "\" has not be builded.");
                }
            }
            catch (Exception ex)
            {
                Log.LogError("The model:\"" + FilePattern + "\" build error:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private bool ProcessEntityType(TypeDefinition type, bool setSuccess)
        {
            TypeDefinition baseType = FindBaseTypeDefinition(type.BaseType, "AbstractEntity");
            if (baseType != null)
            {
                foreach (PropertyDefinition prop in type.Properties)
                {
                    foreach (CustomAttribute attribute in prop.CustomAttributes)
                    {
                        if (attribute.Constructor != null &&
                            attribute.Constructor.DeclaringType != null &&
                            attribute.Constructor.DeclaringType.Name == "EntityFieldAttribute")
                        {
                            setSuccess = SetChangePropertyMethod(type, baseType, prop, setSuccess);
                        }
                    }
                }
                return setSuccess;
            }

            baseType = FindBaseTypeDefinition(type.BaseType, "EntityChangeEvent");
            if (baseType != null)
            {
                //子类定义模式
                foreach (PropertyDefinition prop in type.Properties)
                {
                    setSuccess = SetChildNotifyMethod(type, baseType, prop, setSuccess);
                }
            }
            return setSuccess;
        }

        private bool SetChildNotifyMethod(TypeDefinition type, TypeDefinition baseType, PropertyDefinition prop, bool setSuccess)
        {
            MethodDefinition notifyMethod = baseType.Methods.First(m => m.Name == "BindAndNotify");
            if (notifyMethod == null)
            {
                return setSuccess;
            }
            if (prop.SetMethod == null)
            {
                return setSuccess;
            }
            WriteAopCode(type, prop, notifyMethod, prop.Name);
            return true;
        }

        private bool SetChangePropertyMethod(TypeDefinition type, TypeDefinition baseType, PropertyDefinition prop, bool setSuccess)
        {
            MethodDefinition notifyMethod = baseType.Methods.First(m => m.Name == "BindAndChangeProperty");
            if (notifyMethod == null || prop.SetMethod == null)
            {
                return setSuccess;
            }
            WriteAopCode(type, prop, notifyMethod, prop.Name);
            return true;
        }

        private void WriteAopCode(TypeDefinition type, PropertyDefinition prop, MethodDefinition method, string propName)
        {
            try
            {

                MethodDefinition setMethod = prop.SetMethod;
                if (setMethod.Body.Instructions.Count > 5)
                {
                    return;
                }
                var field = type.Fields.FirstOrDefault(p => p.Name.StartsWith("<" + propName + ">") ||
                    p.Name.Equals("_" + propName, StringComparison.CurrentCultureIgnoreCase));
                if (field == null)
                {
                    return;
                }
                while (setMethod.Body.Instructions.Count > 1)
                {
                    setMethod.Body.Instructions.RemoveAt(0);
                }
                MethodReference notifyMethod = type.Module.Import(method);
                var paramType = setMethod.Parameters[0].ParameterType;
                bool isDateTime = paramType.Name == "DateTime";
                var fieldRefType = Type.GetType("System.Object&");
                var fieldType = Type.GetType("System.Object");
                var exchangeMethod = type.Module.Import(typeof(Interlocked).GetMethod("Exchange", new Type[] { fieldRefType, fieldType }));
                ILProcessor worker = setMethod.Body.GetILProcessor();
                Instruction ins = setMethod.Body.Instructions[setMethod.Body.Instructions.Count - 1];
                var equalsMethod = type.Module.Import(typeof(Object).GetMethod("Equals", new Type[] { typeof(object), typeof(object) }));
                setMethod.Body.Variables.Add(new VariableDefinition(equalsMethod.ReturnType));
                worker.InsertBefore(setMethod.Body.Instructions[0], worker.Create(OpCodes.Nop));

                worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
                worker.InsertBefore(ins, worker.Create(OpCodes.Ldfld, field));
                worker.InsertBefore(ins, worker.Create(OpCodes.Box, paramType));
                worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_1));
                worker.InsertBefore(ins, worker.Create(OpCodes.Box, paramType));
                worker.InsertBefore(ins, worker.Create(OpCodes.Call, equalsMethod));

                worker.InsertBefore(ins, worker.Create(OpCodes.Stloc_0));
                worker.InsertBefore(ins, worker.Create(OpCodes.Ldloc_0));
                worker.InsertBefore(ins, worker.Create(OpCodes.Brtrue_S, ins));

                //exchange field value
                worker.InsertBefore(ins, worker.Create(OpCodes.Nop));
                worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
                if (isDateTime)
                {
                    worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_1));
                    worker.InsertBefore(ins, worker.Create(OpCodes.Stfld, field));
                }
                else
                {
                    worker.InsertBefore(ins, worker.Create(OpCodes.Ldflda, field));
                    worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_1));
                    worker.InsertBefore(ins, worker.Create(OpCodes.Call, exchangeMethod));
                    worker.InsertBefore(ins, worker.Create(OpCodes.Pop));
                }

                worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
                worker.InsertBefore(ins, worker.Create(OpCodes.Ldarg_0));
                worker.InsertBefore(ins, worker.Create(OpCodes.Ldfld, field));
                worker.InsertBefore(ins, worker.Create(OpCodes.Box, paramType));
                if (notifyMethod.Parameters.Count == 2)
                {
                    worker.InsertBefore(ins, worker.Create(OpCodes.Ldstr, propName));
                }
                worker.InsertBefore(ins, worker.Create(OpCodes.Call, notifyMethod));

                worker.InsertBefore(ins, worker.Create(OpCodes.Nop));
                worker.InsertBefore(ins, worker.Create(OpCodes.Nop));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("at type:{0},propName:{1}", type.FullName, propName), ex);
            }
        }


        private static TypeDefinition FindBaseTypeDefinition(TypeReference type, string name)
        {
            if (type == null ||
                type.Module == null ||
                type.Name == typeof(object).Name)
            {
                return null;
            }
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
        /// <summary>
        /// Gets or sets the solution dir.
        /// </summary>
        /// <value>The solution dir.</value>
        [Required]
        public string SolutionDir { get; set; }

        /// <summary>
        /// 过滤文件类型
        /// </summary>
        public string FilePattern { get; set; }
    }
}