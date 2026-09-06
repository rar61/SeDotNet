using System.Reflection;
using System.Runtime.Loader;
using HarmonyLib;
using VRage.Scripting;

namespace SeDotNet.Patches;

[HarmonyPatch(typeof(MyScriptCompiler), MethodType.Constructor)]
internal class MyScriptCompilerPatch
{
    private static void Postfix(MyScriptCompiler __instance)
    {
        var objectModel = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("System.ObjectModel"));
        var regularExpressions = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("System.Text.RegularExpressions"));
        var linq = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("System.Linq"));
        var concurrent = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("System.Collections.Concurrent"));
        var typeConverter = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("System.ComponentModel.TypeConverter"));
        var traceSource = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("System.Diagnostics.TraceSource"));
        var immutable = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("System.Collections.Immutable"));
        __instance.AddReferencedAssemblies(objectModel.Location, regularExpressions.Location,
            linq.Location, concurrent.Location, typeConverter.Location, traceSource.Location, immutable.Location);
    }
}