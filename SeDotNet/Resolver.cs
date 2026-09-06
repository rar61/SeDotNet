using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace SeDotNet;

internal static class Resolver
{
    [ModuleInitializer]
    internal static void RegisterResolvers()
    {
        var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
        Debug.Assert(basePath != null, nameof(basePath) + " != null");
        var dllPath = Path.GetFullPath(Path.Combine(basePath, "..", "SpaceEngineersDedicatedServer", "DedicatedServer64"));

        AssemblyLoadContext.Default.Resolving += (ctx, name) =>
        {
            if (name.Name == "System.Windows.Forms.DataVisualization")
            {
                var path = Path.Combine(AppContext.BaseDirectory, "WinForms.DataVisualization.dll");
                return File.Exists(path) ? ctx.LoadFromAssemblyPath(path) : null;
            }

            var assemblyName = Path.Combine(dllPath, name.Name + ".dll");
            return File.Exists(assemblyName) ? ctx.LoadFromAssemblyPath(assemblyName) : null;
        };

        AssemblyLoadContext.Default.ResolvingUnmanagedDll += (_, name) =>
        {
            var assemblyName = Path.Combine(dllPath, name + ".dll");
            if (File.Exists(assemblyName) && NativeLibrary.TryLoad(assemblyName, out var handle)) return handle;
            return IntPtr.Zero;
        };
    }
}