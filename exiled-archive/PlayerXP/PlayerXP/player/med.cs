using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerXP.player
{
    class med
    {
        public static void medical(Exiled.Events.EventArgs.UsingMedicalItemEventArgs ev)
        {
            Extensions.CurePlayer(ev.Player.ReferenceHub);
        }
    }
}