using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loli.ClansWars.Capturing
{
    internal class CapturingManager
    {
        internal CapturingManager(Manager mngr)
        {
            Manager = mngr;
            Map2 = new Map2(this);
        }
        internal Manager Manager;
        internal Map2 Map2;
    }
}