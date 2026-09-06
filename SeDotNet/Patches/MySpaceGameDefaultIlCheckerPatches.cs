using System.Reflection;
using HarmonyLib;
using SpaceEngineers.Game;

namespace SeDotNet.Patches;

[HarmonyPatch(typeof(MySpaceGameDefaultIlChecker))]
internal class MySpaceGameDefaultIlCheckerPatches
{
    private static readonly MethodInfo GetMethod =
        AccessTools.Method(typeof(Type), nameof(Type.GetMethod), [typeof(string)]);

    private static bool IsClosedGenericSignature(MemberInfo memberInfo)
    {
        var types = memberInfo switch
        {
            MethodInfo methodInfo => methodInfo.GetParameters().Select(p => p.ParameterType)
                .Append(methodInfo.ReturnType),
            FieldInfo fieldInfo => [fieldInfo.FieldType],
            PropertyInfo propertyInfo => [propertyInfo.PropertyType],
            _ => Enumerable.Empty<Type>()
        };

        return types.Any(t => t is
            { IsGenericType: true, IsGenericTypeDefinition: false, IsGenericTypeParameter: false });
    }

    [HarmonyPatch("AllDeclaredMembers")]
    private static void Prefix(ref IEnumerable<MemberInfo> __result)
    {
        __result = __result.Where(info => !IsClosedGenericSignature(info) && info is not TypeInfo);
    }

    private static MethodInfo? GetMethodFix(Type type, string name)
    {
        if (name == "op_Inequality")
            return type.GetMethod(name,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                null, [typeof(Type), typeof(Type)], null
            );

        return type.GetMethod(name);
    }

    [HarmonyPatch("AllowDefaultNamespaces")]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.Select(instruction =>
            instruction.Calls(GetMethod)
                ? CodeInstruction.Call(typeof(MySpaceGameDefaultIlCheckerPatches), nameof(GetMethodFix))
                : instruction);
    }
}