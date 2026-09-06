using System.Reflection;
using HarmonyLib;
using Microsoft.CodeAnalysis;
using VRage.Scripting;

namespace SeDotNet.Patches;

[HarmonyPatch(typeof(MyScriptWhitelist))]
[HarmonyPatch("Register", typeof(MyWhitelistTarget), typeof(ITypeSymbol), typeof(Type))]
[HarmonyPatch("Register", typeof(MyWhitelistTarget), typeof(INamespaceSymbol), typeof(Type))]
[HarmonyPatch("RegisterMember", typeof(MyWhitelistTarget), typeof(ISymbol), typeof(MemberInfo))]
internal class MyScriptWhitelistPatch
{
    private static Exception? Finalizer(Exception? __exception)
    {
        if (__exception is not MyWhitelistException e) return __exception;
        return e.Message.StartsWith("Duplicate") ? null : __exception;
    }
}