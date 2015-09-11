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
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Dynamic;

namespace ZyGames.Framework.Common.Reflect
{
    /// <summary>
    /// Provides by-name member-access to objects of a given type
    /// </summary>
    public abstract class TypeAccessor
    {
        // hash-table has better read-without-locking semantics than dictionary
        private static Hashtable publicAccessorsOnly = new Hashtable(), nonPublicAccessors = new Hashtable();

        /// <summary>
        /// Does this type support new instances via a parameterless constructor?
        /// </summary>
        public virtual bool CreateNewSupported { get { return false; } }
        /// <summary>
        /// Create a new instance of this type
        /// </summary>
        public virtual object CreateNew() { throw new NotSupportedException(); }

        /// <summary>
        /// Can this type be queried for member availability?
        /// </summary>
        public virtual bool GetMembersSupported { get { return false; } }
        /// <summary>
        /// Query the members available for this type
        /// </summary>
        public virtual MemberSet GetMembers() { throw new NotSupportedException(); }

        /// <summary>
        /// init
        /// </summary>
        public static void Init()
        {
            publicAccessorsOnly = new Hashtable();
            nonPublicAccessors = new Hashtable();
        }

        /// <summary>
        /// Provides a type-specific accessor, allowing by-name access for all objects of that type
        /// </summary>
        /// <remarks>The accessor is cached internally; a pre-existing accessor may be returned</remarks>
        public static TypeAccessor Create(Type type)
        {
            return Create(type, false);
        }

        /// <summary>
        /// Provides a type-specific accessor, allowing by-name access for all objects of that type
        /// </summary>
        /// <remarks>The accessor is cached internally; a pre-existing accessor may be returned</remarks>
        public static TypeAccessor Create(Type type, bool allowNonPublicAccessors)
        {
            if (type == null) throw new ArgumentNullException("type");
            var lookup = allowNonPublicAccessors ? nonPublicAccessors : publicAccessorsOnly;
            TypeAccessor obj = (TypeAccessor)lookup[type];
            if (obj != null) return obj;

            lock (lookup)
            {
                // double-check
                obj = (TypeAccessor)lookup[type];
                if (obj != null) return obj;

                obj = CreateNew(type, allowNonPublicAccessors);

                lookup[type] = obj;
                return obj;
            }
        }

        sealed class DynamicAccessor : TypeAccessor
        {
            public static readonly DynamicAccessor Singleton = new DynamicAccessor();
            private DynamicAccessor() { }
            public override object this[object target, string name]
            {
                get { return CallSiteCache.GetValue(name, target); }
                set { CallSiteCache.SetValue(name, target, value); }
            }
        }

        private static AssemblyBuilder assembly;
        private static ModuleBuilder module;
        private static int counter;

        private static void WriteGetter(ILGenerator il, Type type, PropertyInfo[] props, FieldInfo[] fields, bool isStatic, bool allowNonPublicAccessors)
        {
            LocalBuilder loc = type.IsValueType ? il.DeclareLocal(type) : null;
            OpCode propName = isStatic ? OpCodes.Ldarg_1 : OpCodes.Ldarg_2, target = isStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1;
            foreach (PropertyInfo prop in props)
            {
                MethodInfo getter;
                if (prop.GetIndexParameters().Length != 0 || !prop.CanRead || (getter = prop.GetGetMethod(allowNonPublicAccessors)) == null) continue;

                Label next = il.DefineLabel();
                il.Emit(propName);
                il.Emit(OpCodes.Ldstr, prop.Name);
                il.EmitCall(OpCodes.Call, strinqEquals, null);
                il.Emit(OpCodes.Brfalse_S, next);
                // match:
                il.Emit(target);
                Cast(il, type, loc);
                il.EmitCall(type.IsValueType ? OpCodes.Call : OpCodes.Callvirt, getter, null);
                if (prop.PropertyType.IsValueType)
                {
                    il.Emit(OpCodes.Box, prop.PropertyType);
                }
                il.Emit(OpCodes.Ret);
                // not match:
                il.MarkLabel(next);
            }
            foreach (FieldInfo field in fields)
            {
                Label next = il.DefineLabel();
                il.Emit(propName);
                il.Emit(OpCodes.Ldstr, field.Name);
                il.EmitCall(OpCodes.Call, strinqEquals, null);
                il.Emit(OpCodes.Brfalse_S, next);
                // match:
                il.Emit(target);
                Cast(il, type, loc);
                il.Emit(OpCodes.Ldfld, field);
                if (field.FieldType.IsValueType)
                {
                    il.Emit(OpCodes.Box, field.FieldType);
                }
                il.Emit(OpCodes.Ret);
                // not match:
                il.MarkLabel(next);
            }
            il.Emit(OpCodes.Ldstr, "name");
            il.Emit(OpCodes.Newobj, typeof(ArgumentOutOfRangeException).GetConstructor(new Type[] { typeof(string) }));
            il.Emit(OpCodes.Throw);
        }
        private static void WriteSetter(ILGenerator il, Type type, PropertyInfo[] props, FieldInfo[] fields, bool isStatic, bool allowNonPublicAccessors)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Ldstr, "Write is not supported for structs");
                il.Emit(OpCodes.Newobj, typeof(NotSupportedException).GetConstructor(new Type[] { typeof(string) }));
                il.Emit(OpCodes.Throw);
            }
            else
            {
                OpCode propName = isStatic ? OpCodes.Ldarg_1 : OpCodes.Ldarg_2,
                       target = isStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1,
                       value = isStatic ? OpCodes.Ldarg_2 : OpCodes.Ldarg_3;
                LocalBuilder loc = type.IsValueType ? il.DeclareLocal(type) : null;
                foreach (PropertyInfo prop in props)
                {
                    MethodInfo setter;
                    if (prop.GetIndexParameters().Length != 0 || !prop.CanWrite || (setter = prop.GetSetMethod(allowNonPublicAccessors)) == null) continue;

                    Label next = il.DefineLabel();
                    il.Emit(propName);
                    il.Emit(OpCodes.Ldstr, prop.Name);
                    il.EmitCall(OpCodes.Call, strinqEquals, null);
                    il.Emit(OpCodes.Brfalse_S, next);
                    // match:
                    il.Emit(target);
                    Cast(il, type, loc);
                    il.Emit(value);
                    Cast(il, prop.PropertyType, null);
                    il.EmitCall(type.IsValueType ? OpCodes.Call : OpCodes.Callvirt, setter, null);
                    il.Emit(OpCodes.Ret);
                    // not match:
                    il.MarkLabel(next);
                }
                foreach (FieldInfo field in fields)
                {
                    Label next = il.DefineLabel();
                    il.Emit(propName);
                    il.Emit(OpCodes.Ldstr, field.Name);
                    il.EmitCall(OpCodes.Call, strinqEquals, null);
                    il.Emit(OpCodes.Brfalse_S, next);
                    // match:
                    il.Emit(target);
                    Cast(il, type, loc);
                    il.Emit(value);
                    Cast(il, field.FieldType, null);
                    il.Emit(OpCodes.Stfld, field);
                    il.Emit(OpCodes.Ret);
                    // not match:
                    il.MarkLabel(next);
                }
                il.Emit(OpCodes.Ldstr, "name");
                il.Emit(OpCodes.Newobj, typeof(ArgumentOutOfRangeException).GetConstructor(new Type[] { typeof(string) }));
                il.Emit(OpCodes.Throw);
            }
        }
        private static readonly MethodInfo strinqEquals = typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) });

        /// <summary>
        /// A TypeAccessor based on a Type implementation, with available member metadata
        /// </summary>
        protected abstract class RuntimeTypeAccessor : TypeAccessor
        {
            /// <summary>
            /// Returns the Type represented by this accessor
            /// </summary>
            protected abstract Type Type { get; }

            /// <summary>
            /// Can this type be queried for member availability?
            /// </summary>
            public override bool GetMembersSupported { get { return true; } }
            private MemberSet members;
            /// <summary>
            /// Query the members available for this type
            /// </summary>
            public override MemberSet GetMembers()
            {
                return members ?? (members = new MemberSet(Type));
            }
        }
        sealed class DelegateAccessor : RuntimeTypeAccessor
        {
            private readonly Func<object, string, object> getter;
            private readonly Action<object, string, object> setter;
            private readonly Func<object> ctor;
            private readonly Type type;
            protected override Type Type
            {
                get { return type; }
            }
            public DelegateAccessor(Func<object, string, object> getter, Action<object, string, object> setter, Func<object> ctor, Type type)
            {
                this.getter = getter;
                this.setter = setter;
                this.ctor = ctor;
                this.type = type;
            }
            public override bool CreateNewSupported { get { return ctor != null; } }
            public override object CreateNew()
            {
                return ctor != null ? ctor() : base.CreateNew();
            }
            public override object this[object target, string name]
            {
                get { return getter(target, name); }
                set { setter(target, name, value); }
            }
        }
        private static bool IsFullyPublic(Type type, PropertyInfo[] props, bool allowNonPublicAccessors)
        {
            while (type.IsNestedPublic) type = type.DeclaringType;
            if (!type.IsPublic) return false;

            if (allowNonPublicAccessors)
            {
                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].GetGetMethod(true) != null && props[i].GetGetMethod(false) == null) return false; // non-public getter
                    if (props[i].GetSetMethod(true) != null && props[i].GetSetMethod(false) == null) return false; // non-public setter
                }
            }

            return true;
        }
        static TypeAccessor CreateNew(Type type, bool allowNonPublicAccessors)
        {
            if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
            {
                return DynamicAccessor.Singleton;
            }

            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            ConstructorInfo ctor = null;
            if (type.IsClass && !type.IsAbstract)
            {
                ctor = type.GetConstructor(Type.EmptyTypes);
            }
            ILGenerator il;
            if (!IsFullyPublic(type, props, allowNonPublicAccessors))
            {
                DynamicMethod dynGetter = new DynamicMethod(type.FullName + "_get", typeof(object), new Type[] { typeof(object), typeof(string) }, type, true),
                              dynSetter = new DynamicMethod(type.FullName + "_set", null, new Type[] { typeof(object), typeof(string), typeof(object) }, type, true);
                WriteGetter(dynGetter.GetILGenerator(), type, props, fields, true, allowNonPublicAccessors);
                WriteSetter(dynSetter.GetILGenerator(), type, props, fields, true, allowNonPublicAccessors);
                DynamicMethod dynCtor = null;
                if (ctor != null)
                {
                    dynCtor = new DynamicMethod(type.FullName + "_ctor", typeof(object), Type.EmptyTypes, type, true);
                    il = dynCtor.GetILGenerator();
                    il.Emit(OpCodes.Newobj, ctor);
                    il.Emit(OpCodes.Ret);
                }
                return new DelegateAccessor(
                    (Func<object, string, object>)dynGetter.CreateDelegate(typeof(Func<object, string, object>)),
                    (Action<object, string, object>)dynSetter.CreateDelegate(typeof(Action<object, string, object>)),
                    dynCtor == null ? null : (Func<object>)dynCtor.CreateDelegate(typeof(Func<object>)), type);
            }

            // note this region is synchronized; only one is being created at a time so we don't need to stress about the builders
            if (assembly == null)
            {
                AssemblyName name = new AssemblyName("FastMember_dynamic");
                assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
                module = assembly.DefineDynamicModule(name.Name);
            }
            TypeBuilder tb = module.DefineType("FastMember_dynamic." + type.Name + "_" + Interlocked.Increment(ref counter),
                (typeof(TypeAccessor).Attributes | TypeAttributes.Sealed | TypeAttributes.Public) & ~(TypeAttributes.Abstract | TypeAttributes.NotPublic), typeof(RuntimeTypeAccessor));

            tb.DefineDefaultConstructor(MethodAttributes.Public);
            PropertyInfo indexer = typeof(TypeAccessor).GetProperty("Item");
            MethodInfo baseGetter = indexer.GetGetMethod(), baseSetter = indexer.GetSetMethod();
            MethodBuilder body = tb.DefineMethod(baseGetter.Name, baseGetter.Attributes & ~MethodAttributes.Abstract, typeof(object), new Type[] { typeof(object), typeof(string) });
            il = body.GetILGenerator();
            WriteGetter(il, type, props, fields, false, allowNonPublicAccessors);
            tb.DefineMethodOverride(body, baseGetter);

            body = tb.DefineMethod(baseSetter.Name, baseSetter.Attributes & ~MethodAttributes.Abstract, null, new Type[] { typeof(object), typeof(string), typeof(object) });
            il = body.GetILGenerator();
            WriteSetter(il, type, props, fields, false, allowNonPublicAccessors);
            tb.DefineMethodOverride(body, baseSetter);

            MethodInfo baseMethod;
            if (ctor != null)
            {
                baseMethod = typeof(TypeAccessor).GetProperty("CreateNewSupported").GetGetMethod();
                body = tb.DefineMethod(baseMethod.Name, baseMethod.Attributes, baseMethod.ReturnType, Type.EmptyTypes);
                il = body.GetILGenerator();
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Ret);
                tb.DefineMethodOverride(body, baseMethod);

                baseMethod = typeof(TypeAccessor).GetMethod("CreateNew");
                body = tb.DefineMethod(baseMethod.Name, baseMethod.Attributes, baseMethod.ReturnType, Type.EmptyTypes);
                il = body.GetILGenerator();
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Ret);
                tb.DefineMethodOverride(body, baseMethod);
            }

            baseMethod = typeof(RuntimeTypeAccessor).GetProperty("Type", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true);
            body = tb.DefineMethod(baseMethod.Name, baseMethod.Attributes & ~MethodAttributes.Abstract, baseMethod.ReturnType, Type.EmptyTypes);
            il = body.GetILGenerator();
            il.Emit(OpCodes.Ldtoken, type);
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
            il.Emit(OpCodes.Ret);
            tb.DefineMethodOverride(body, baseMethod);

            return (TypeAccessor)Activator.CreateInstance(tb.CreateType());
        }

        private static void Cast(ILGenerator il, Type type, LocalBuilder addr)
        {
            if (type == typeof(object)) { }
            else if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
                if (addr != null)
                {
                    il.Emit(OpCodes.Stloc, addr);
                    il.Emit(OpCodes.Ldloca_S, addr);
                }
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        /// <summary>
        /// Get or set the value of a named member on the target instance
        /// </summary>
        public abstract object this[object target, string name]
        {
            get;
            set;
        }
    }
}