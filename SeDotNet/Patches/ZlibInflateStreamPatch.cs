using System.IO.Compression;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace SeDotNet.Patches;

[HarmonyPatch("SixLabors.ImageSharp.Formats.Png.Zlib.ZlibInflateStream", "InitializeInflateStream")]
internal class ZlibInflateStreamPatch
{
    private static readonly ConstructorInfo OriginalDeflateStream =
        AccessTools.Constructor(typeof(DeflateStream), [typeof(Stream), typeof(CompressionMode), typeof(bool)]);

    private static readonly ConstructorInfo FixedDeflateStream = AccessTools.Constructor(typeof(FullReadDeflateStream),
        [typeof(Stream), typeof(CompressionMode), typeof(bool)]);

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (var ins in instructions)
            if (ins.opcode == OpCodes.Newobj && ins.operand is ConstructorInfo ctor && ctor == OriginalDeflateStream)
                yield return new CodeInstruction(OpCodes.Newobj, FixedDeflateStream);
            else
                yield return ins;
    }

    private sealed class FullReadDeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : DeflateStream(stream, mode, leaveOpen)
    {
        public override int Read(byte[] buffer, int offset, int count)
        {
            var total = 0;
            while (total < count)
            {
                var bytesRead = base.Read(buffer, offset + total, count - total);
                if (bytesRead == 0) break;
                total += bytesRead;
            }

            return total;
        }
    }
}