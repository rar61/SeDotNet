using System.Reflection;
using HarmonyLib;
using VRage.Dedicated;

namespace SeDotNet.Patches;

[HarmonyPatch(typeof(ConfigForm), "StartServerAsConsole")]
internal class ConfigFormPatch
{
    private static readonly MethodInfo GetCommandLineArgs = AccessTools.Method(typeof(Environment), nameof(Environment.GetCommandLineArgs));

    private static string[] GetCommandLineArgsFix()
    {
        var args = Environment.GetCommandLineArgs();
        if (Environment.ProcessPath != null) args[0] = Environment.ProcessPath;
        return args;
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.Select(instruction =>
            instruction.Calls(GetCommandLineArgs)
                ? CodeInstruction.Call(typeof(ConfigFormPatch), nameof(GetCommandLineArgsFix))
                : instruction);
    }
}