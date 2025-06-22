using System.Collections.Generic;
using System.Linq;

namespace force
{
    public static class Extensions
    {
        public static bool IsHost(this ReferenceHub player) => player.characterClassManager.IsHost;
        public static IEnumerable<ReferenceHub> GetHubs() => ReferenceHub.GetAllHubs().Values.Where(h => !h.IsHost());
    }
}
