using Qurre.API;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli.DataBase.Modules.Controllers
{
    internal class Priest
    {
        internal static readonly List<Priest> List = new();
        internal const string Tag = "PriestBetterTag";
        private readonly Glow Glow;
        private readonly Nimb Nimb;
        internal readonly Player pl;
        internal Priest(Player pl)
        {
            List.Add(this);
            this.pl = pl;
            pl.Tag += Tag;
            Glow = new Glow(pl, new Color32(255, 242, 122, 255));
            Nimb = new Nimb(pl);
        }
        internal void Break()
        {
            if (List.Contains(this)) List.Remove(this);
            try { pl.Tag = pl.Tag.Replace(Tag, ""); } catch { }
            try { Glow.Destroy(); } catch { }
            try { Nimb.Destroy(); } catch { }
        }
        internal static Priest Get(Player pl)
        {
            var _list = List.Where(x => x.pl == pl);
            if (_list.Count() > 0) return _list.First();
            return null;
        }
    }
}