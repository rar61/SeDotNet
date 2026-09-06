using System.Reflection;
using System.Xml;
using HarmonyLib;
using VRage;

namespace SeDotNet.Patches;

[HarmonyPatch(typeof(CustomRootWriter), "Init")]
internal class CustomRootWriterPatch
{
    private static readonly MethodInfo WriteAttrString = AccessTools.Method(typeof(XmlWriter),
        nameof(XmlWriter.WriteAttributeString), [typeof(string), typeof(string)]);

    private static void WriteAttrStringFix(XmlWriter xmlWriter, string localName, string value)
    {
        xmlWriter.WriteAttributeString("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance", value);
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.Select(instruction => instruction.Calls(WriteAttrString)
            ? CodeInstruction.Call(typeof(CustomRootWriterPatch), nameof(WriteAttrStringFix))
            : instruction);
    }
}