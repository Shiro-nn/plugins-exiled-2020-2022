using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClassicCore.Modules
{
    static internal class Cache
    {
        public readonly static Dictionary<string, VecPos> Pos = new();
        static internal readonly Dictionary<int, RoleType> Roles = new();
	}
}