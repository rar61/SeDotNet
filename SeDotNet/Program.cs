using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using HarmonyLib;
using VRage.FileSystem;

namespace SeDotNet;

internal class Program
{
    private static void Main(string[] args)
    {
        var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
        Debug.Assert(basePath != null, nameof(basePath) + " != null");
        basePath = Path.GetFullPath(Path.Combine(basePath, "..", "SpaceEngineersDedicatedServer"));

        Environment.CurrentDirectory = basePath;

        var exePath = Path.Combine(basePath, "DedicatedServer64");

        MyFileSystem.ExePath = exePath;
        MyFileSystem.RootPath = basePath;

        AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("SixLabors.ImageSharp"));

        var harmony = new Harmony("SpaceEngineersDedicatedServer");

        harmony.PatchAll(Assembly.GetExecutingAssembly());

        AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(exePath, "SpaceEngineersDedicated.exe"));
        var main = AccessTools.Method("SpaceEngineersDedicated.MyProgram:Main");
        main.Invoke(null, [args]);
    }
}