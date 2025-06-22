using Qurre.API;
using System.Collections.Generic;
using System.Linq;
namespace ClassicCore.DataBase.Modules.Controllers
{
    internal class Beginner
    {
        internal static readonly List<Beginner> List = new();
        internal const string Tag = "PriestBeginnet";
        internal Player pl;
        internal Beginner(Player pl)
        {
            List.Add(this);
            this.pl = pl;
            pl.Tag += Tag;
        }
        internal void Break()
        {
            if (List.Contains(this)) List.Remove(this);
            try { pl.Tag = pl.Tag.Replace(Tag, ""); } catch { }
        }
        internal static Beginner Get(Player pl)
        {
            var _list = List.Where(x => x.pl == pl);
            if (_list.Count() > 0) return _list.First();
            return null;
        }
    }
}