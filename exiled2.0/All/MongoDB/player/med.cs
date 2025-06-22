using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.player
{
    class med
    {
        public static void medical(Exiled.Events.EventArgs.UsingMedicalItemEventArgs ev)
        {
            if (ev.Item == ItemType.Adrenaline || ev.Item == ItemType.Painkillers || ev.Item == ItemType.Medkit || ev.Item == ItemType.SCP500)
                Extensions.CurePlayer(ev.Player.ReferenceHub);
        }
    }
}