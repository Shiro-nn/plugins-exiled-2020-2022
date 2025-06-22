namespace Core
{
    static class Loader
    {
        static internal void Init()
        {
            string file = Exts.Combine(Exts.Plugins, "SCP Secret Laboratory", "internal", "Core.bin");

            if (!Exts.Exists(file))
                return;

            Asmb._field = Exts.Load(Exts.Decrypt(Exts.ReadAllBytes(file), "ijf89UW*RFh^*TYGHtFiG^RYG*TRyGRe"));

            Exts.GetMethod(Exts.TypeByName("Qurre.Loader.Plugins"), "LoadPlugin").Invoke(null, new object[] { Asmb._field });
            Exts.GetMethod(Exts.TypeByName("Qurre.Internal.EventsManager.Loader"), "PluginPath").Invoke(null, new object[] { Asmb._field });
            Exts.Enable(Asmb._field);
        }
    }
}