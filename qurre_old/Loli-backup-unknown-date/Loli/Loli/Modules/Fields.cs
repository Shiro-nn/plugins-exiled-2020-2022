using System.Collections.Generic;
using UnityEngine;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		internal bool first = true;
		public readonly List<Vector3> Bloods = new();
		public readonly static Dictionary<string, VecPos> Pos = new();
		public readonly static Dictionary<string, bool> Upgrade914 = new();
		public readonly static Dictionary<int, RoleType> DRole = new();
		public static bool RoundStarted = false;
	}
}