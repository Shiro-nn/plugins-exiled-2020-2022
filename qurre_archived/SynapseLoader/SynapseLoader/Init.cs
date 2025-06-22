using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
namespace SynapseInjector
{
    internal class Init : Qurre.Plugin
    {
        public override string Developer => "Synapse";
        public override string Name => "SynapseInjector";
        public override Version Version => new Version(1, 0, 1);
        public override int Priority => int.MaxValue;
        public override void Enable()
        {
            try
            {
                var localpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Synapse");
                var synapsepath = Directory.Exists(localpath) ? localpath : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synapse");

                if (!Directory.Exists(synapsepath)) Directory.CreateDirectory(synapsepath);

                var dependencyAssemblies = new List<Assembly>();
                foreach (var depend in Directory.GetFiles(Path.Combine(synapsepath, "dependencies"), "*.dll"))
                {
                    var assembly = Assembly.Load(File.ReadAllBytes(depend));
                    dependencyAssemblies.Add(assembly);
                };

                var synapseAssembly = Assembly.Load(File.ReadAllBytes(Path.Combine(synapsepath, "Synapse.dll")));

                PrintBanner();
                InvokeAssembly();
                void InvokeAssembly()
                {
                    try
                    {
                        synapseAssembly.GetTypes()
                            .First((Type t) => t.Name == "SynapseController").GetMethods()
                            .First((MethodInfo m) => m.Name == "Init")
                            .Invoke(null, null);
                    }
                    catch (Exception e)
                    {
                        ServerConsole.AddLog($"SynapseLoader: Error while Loading Synapse! Please check your synapse and game version. If you can't fix it join our Discord and show us this Error:\n{e}", ConsoleColor.Red);
                    }
                }
                void PrintBanner()
                {
                    ServerConsole.AddLog(
                        "\nLoading Synapse...\n" +
                        "-------------------===Loader===-------------------\n" +
                        "  __                             \n" +
                        " (_       ._    _.  ._    _   _  \n" +
                        " __)  \\/  | |  (_|  |_)  _>  (/_ \n" +
                        "      /             |            \n\n" +
                        $"SynapseVersion {synapseAssembly.GetName().Version}\n" +
                        $"LoaderVersion: {Version}\n" +
                        $"RuntimeVersion: {Assembly.GetExecutingAssembly().ImageRuntimeVersion}\n\n" +
                        string.Join("\n", dependencyAssemblies.Select(assembly => $"{assembly.GetName().Name}: {assembly.GetName().Version}").ToList()) + "\n" +
                        "-------------------===Loader===-------------------", ConsoleColor.Yellow);

                }
            }
            catch (Exception e)
            {
                ServerConsole.AddLog($"SynapseLoader: Error occured while loading the assemblies. Please check if all required dll are installed. If you can't fix it join our Discord and show us this Error:\n{e}", ConsoleColor.Red);
            }

        }
        public override void Disable() { }
    }
}