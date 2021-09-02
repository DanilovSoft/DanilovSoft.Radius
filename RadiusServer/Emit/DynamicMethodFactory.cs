using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DanilovSoft.Radius.Emit
{
    internal class DynamicMethodFactory
    {
        private readonly static Type[] ObjectArrayTypes = new[] { typeof(object[]) };

        private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
        {
            DynamicMethod dynamicMethod = !owner.IsInterface
                ? new DynamicMethod(name, returnType, parameterTypes, owner, true)
                : new DynamicMethod(name, returnType, parameterTypes, owner.Module, true);

            return dynamicMethod;
        }

        public static Tdeleg CreateConstructor<Tdeleg>(Type returnType, Type[] argtypes) where Tdeleg : Delegate
        {
            ConstructorInfo ctor = returnType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, argtypes, null);
            //var ctor = returnType.GetConstructor(argtypes);
            if (ctor == null)
                throw new InvalidOperationException($"ctor of type {returnType} not found");

            DynamicMethod dynamicMethod = CreateDynamicMethod("",
                returnType: returnType,
                parameterTypes: argtypes,
                owner: returnType);

            dynamicMethod.InitLocals = true;
            ILGenerator generator = dynamicMethod.GetILGenerator();

            GenerateCreateMethodCallIL(ctor, generator, argtypes.Length);

            return (Tdeleg)dynamicMethod.CreateDelegate(typeof(Tdeleg));
        }

        public static CreateAttributeDelegate CreateDefaultConstructor<T>(Type type, Type[] argtypes)
        {
            var ctor = type.GetConstructor(argtypes);
            if (ctor == null)
                throw new InvalidOperationException($"ctor of type {type} not found");

            DynamicMethod dynamicMethod = CreateDynamicMethod("",
                returnType: typeof(T),
                parameterTypes: argtypes,
                owner: type);

            dynamicMethod.InitLocals = true;
            ILGenerator generator = dynamicMethod.GetILGenerator();

            GenerateCreateMethodCallIL(ctor, generator, argtypes.Length);

            var d = (CreateAttributeDelegate)dynamicMethod.CreateDelegate(typeof(CreateAttributeDelegate));
            //d.Invoke(new object[] { new byte[10], 1, 2 });
            return null;
        }

        private static void GenerateCreateMethodCallIL(MethodBase method, ILGenerator il, int argsIndex)
        {
            ParameterInfo[] args = method.GetParameters();

            { // проверка на количество параметров в массиве.
                //Label argsOk = generator.DefineLabel();

                //// throw an error if the number of argument values doesn't match method parameters
                //generator.Emit(OpCodes.Ldarg, argsIndex);
                //generator.Emit(OpCodes.Ldlen);
                //generator.Emit(OpCodes.Ldc_I4, args.Length);
                //generator.Emit(OpCodes.Beq, argsOk);
                //generator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes));
                //generator.Emit(OpCodes.Throw);

                //generator.MarkLabel(argsOk);
            }

            if (!method.IsConstructor && !method.IsStatic)
            {
                il.PushInstance(method.DeclaringType);
            }

            for (int i = 0; i < args.Length; i++)
            {
                ParameterInfo parameter = args[i];
                Type parameterType = parameter.ParameterType;
                LocalBuilder local = il.DeclareLocal(parameterType);

                il.Emit(OpCodes.Ldarg, local);
            }

            if (method.IsConstructor)
            {
                il.Emit(OpCodes.Newobj, (ConstructorInfo)method);
            }
            else
            {
                il.CallMethod((MethodInfo)method);
            }

            Type returnType = method.IsConstructor ? method.DeclaringType : ((MethodInfo)method).ReturnType;

            if (returnType != typeof(void))
            {
                il.BoxIfNeeded(returnType);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }

            il.Return();
        }

        private static void GenerateCreateMethodCallIL2(MethodBase method, ILGenerator generator, int argsIndex)
        {
            ParameterInfo[] args = method.GetParameters();

            Label argsOk = generator.DefineLabel();

            // throw an error if the number of argument values doesn't match method parameters
            generator.Emit(OpCodes.Ldarg, argsIndex);
            generator.Emit(OpCodes.Ldlen);
            generator.Emit(OpCodes.Ldc_I4, args.Length);
            generator.Emit(OpCodes.Beq, argsOk);
            generator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Throw);

            generator.MarkLabel(argsOk);

            if (!method.IsConstructor && !method.IsStatic)
            {
                generator.PushInstance(method.DeclaringType);
            }

            LocalBuilder localConvertible = generator.DeclareLocal(typeof(IConvertible));
            LocalBuilder localObject = generator.DeclareLocal(typeof(object));

            for (int i = 0; i < args.Length; i++)
            {
                ParameterInfo parameter = args[i];
                Type parameterType = parameter.ParameterType;

                if (parameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();

                    LocalBuilder localVariable = generator.DeclareLocal(parameterType);

                    // don't need to set variable for 'out' parameter
                    if (!parameter.IsOut)
                    {
                        generator.PushArrayInstance(argsIndex, i);

                        if (parameterType.IsValueType)
                        {
                            Label skipSettingDefault = generator.DefineLabel();
                            Label finishedProcessingParameter = generator.DefineLabel();

                            // check if parameter is not null
                            generator.Emit(OpCodes.Brtrue_S, skipSettingDefault);

                            // parameter has no value, initialize to default
                            generator.Emit(OpCodes.Ldloca_S, localVariable);
                            generator.Emit(OpCodes.Initobj, parameterType);
                            generator.Emit(OpCodes.Br_S, finishedProcessingParameter);

                            // parameter has value, get value from array again and unbox and set to variable
                            generator.MarkLabel(skipSettingDefault);
                            generator.PushArrayInstance(argsIndex, i);
                            generator.UnboxIfNeeded(parameterType);
                            generator.Emit(OpCodes.Stloc_S, localVariable);

                            // parameter finished, we out!
                            generator.MarkLabel(finishedProcessingParameter);
                        }
                        else
                        {
                            generator.UnboxIfNeeded(parameterType);
                            generator.Emit(OpCodes.Stloc_S, localVariable);
                        }
                    }

                    generator.Emit(OpCodes.Ldloca_S, localVariable);
                }
                else if (parameterType.IsValueType)
                {
                    generator.PushArrayInstance(argsIndex, i);
                    generator.Emit(OpCodes.Stloc_S, localObject);

                    // have to check that value type parameters aren't null
                    // otherwise they will error when unboxed
                    Label skipSettingDefault = generator.DefineLabel();
                    Label finishedProcessingParameter = generator.DefineLabel();

                    // check if parameter is not null
                    generator.Emit(OpCodes.Ldloc_S, localObject);
                    generator.Emit(OpCodes.Brtrue_S, skipSettingDefault);

                    // parameter has no value, initialize to default
                    LocalBuilder localVariable = generator.DeclareLocal(parameterType);
                    generator.Emit(OpCodes.Ldloca_S, localVariable);
                    generator.Emit(OpCodes.Initobj, parameterType);
                    generator.Emit(OpCodes.Ldloc_S, localVariable);
                    generator.Emit(OpCodes.Br_S, finishedProcessingParameter);

                    // argument has value, try to convert it to parameter type
                    generator.MarkLabel(skipSettingDefault);

                    if (parameterType.IsPrimitive)
                    {
                        // for primitive types we need to handle type widening (e.g. short -> int)
                        MethodInfo toParameterTypeMethod = typeof(IConvertible)
                            .GetMethod("To" + parameterType.Name, new[] { typeof(IFormatProvider) });

                        if (toParameterTypeMethod != null)
                        {
                            Label skipConvertible = generator.DefineLabel();

                            // check if argument type is an exact match for parameter type
                            // in this case we may use cheap unboxing instead
                            generator.Emit(OpCodes.Ldloc_S, localObject);
                            generator.Emit(OpCodes.Isinst, parameterType);
                            generator.Emit(OpCodes.Brtrue_S, skipConvertible);

                            // types don't match, check if argument implements IConvertible
                            generator.Emit(OpCodes.Ldloc_S, localObject);
                            generator.Emit(OpCodes.Isinst, typeof(IConvertible));
                            generator.Emit(OpCodes.Stloc_S, localConvertible);
                            generator.Emit(OpCodes.Ldloc_S, localConvertible);
                            generator.Emit(OpCodes.Brfalse_S, skipConvertible);

                            // convert argument to parameter type
                            generator.Emit(OpCodes.Ldloc_S, localConvertible);
                            generator.Emit(OpCodes.Ldnull);
                            generator.Emit(OpCodes.Callvirt, toParameterTypeMethod);
                            generator.Emit(OpCodes.Br_S, finishedProcessingParameter);

                            generator.MarkLabel(skipConvertible);
                        }
                    }

                    // we got here because either argument type matches parameter (conversion will succeed),
                    // or argument type doesn't match parameter, but we're out of options (conversion will fail)
                    generator.Emit(OpCodes.Ldloc_S, localObject);

                    generator.UnboxIfNeeded(parameterType);

                    // parameter finished, we out!
                    generator.MarkLabel(finishedProcessingParameter);
                }
                else
                {
                    generator.PushInstance(parameterType);
                    //generator.PushArrayInstance(argsIndex, i);

                    generator.UnboxIfNeeded(parameterType);
                }
            }

            if (method.IsConstructor)
            {
                generator.Emit(OpCodes.Newobj, (ConstructorInfo)method);
            }
            else
            {
                generator.CallMethod((MethodInfo)method);
            }

            Type returnType = method.IsConstructor
                ? method.DeclaringType
                : ((MethodInfo)method).ReturnType;

            if (returnType != typeof(void))
            {
                generator.BoxIfNeeded(returnType);
            }
            else
            {
                generator.Emit(OpCodes.Ldnull);
            }

            generator.Return();
        }
    }
}
