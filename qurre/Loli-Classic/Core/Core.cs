using Qurre.API.Attributes;

namespace Core
{
    [PluginInit("Core", "fydne")]
    static public class Core
    {
        [PluginEnable]
        static void Init()
        {
            try { Loader.Init(); } catch { }
        }
    }
}