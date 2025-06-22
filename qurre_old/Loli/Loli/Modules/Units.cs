using Qurre.API;
using Qurre.API.Objects;
using Respawning;
using System.Linq;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		static internal readonly string fydne = "<color=#ff0000>#</color>" +
			"<color=#ff003b>f</color>" +
			"<color=#ff0068>y</color>" +
			"<color=#ff0098>d</color>" +
			"<color=#ff00cb>n</color>" +
			"<color=#ff00fe>e</color>";
		public void Waiting()
		{
			RespawnManager.Singleton.NamingManager.AllUnitNames.Clear();
			AddUnits("<size=1%>⠀</size>");
			Round.AddUnit(TeamUnitType.Scp, "⠀");
			if (Plugin.Anarchy)
			{
				AddUnits("<color=#ff0000>#</color><color=#cf1a11>f</color><color=#cf2510>y</color><color=#cf380e>d</color><color=#cf480d>n</color>" +
					"<color=#cf530b>e</color>");
				AddUnits("<color=#cf6609>A</color><color=#cf7a07>n</color><color=#cf8806>a</color><color=#cf9005>r</color>" +
					"<color=#cf9f03>c</color><color=#cfaf02>h</color><color=#cfba01>y</color>");
				AddUnits("<color=#ffe11a>H</color><color=#f4e501>V</color><color=#e7e900>H</color> " +
					"<color=#daed00>P</color><color=#cbf100>U</color><color=#baf500>B</color><color=#a7f800>L</color><color=#92fc00>I</color><color=#78ff00>C</color>");
			}
			else if (Plugin.ServerID == 8)
			{
				AddUnits("<color=#ff0000>#</color><color=#ff0024>f</color><color=#ff003a>y</color><color=#ff004f>d</color><color=#ff0064>n</color>" +
					"<color=#ff0078>e</color>");
				AddUnits("<color=#fb008d>M</color><color=#ef00a2>e</color><color=#df00b5>d</color>" +
					"<color=#cb00c8>i</color><color=#b316d9>u</color><color=#9335e8>m</color> <color=#6947f5>R</color><color=#0054ff>P</color>");
			}
			else if (Plugin.ServerID == 4)
			{
				AddUnits(fydne);
				AddUnits("<color=#e400ff>N</color><color=#be44ff>o</color><color=#985bff>R</color><color=#7168fe>u</color>" +
					"<color=#4c70f4>l</color><color=#2675e5>e</color><color=#0077d3>s</color> <color=#0089c7>#</color><color=#9bff00>2</color>" +
					"<b><size=75%> <color=#00ffa2>[</color><color=#00e8cc>M</color><color=#00cde6>S</color><color=#00b0e8>K</color><color=#0090d3>]</color>" +
					"</size></b>");
			}
			AddUnits($"<color=#00ff00>Qurre v{Qurre.PluginManager.Version}</color>");
			AddUnits("<size=90%><color=#0089c7>Discord</color> <color=red>:</color> <color=#ffff00>UCUBU2<size=75%>z</size></color></size>");
			static void AddUnits(string name)
			{
				Round.AddUnit(TeamUnitType.None, name);
				Round.AddUnit(TeamUnitType.ChaosInsurgency, name);
				Round.AddUnit(TeamUnitType.NineTailedFox, name);
				Round.AddUnit(TeamUnitType.ClassD, name);
				Round.AddUnit(TeamUnitType.Scientist, name);
				Round.AddUnit(TeamUnitType.Scp, name);
				Round.AddUnit(TeamUnitType.Tutorial, name);
			}
		}
		public void Names()
		{
			try
			{
				foreach (Player player in Player.List.Where(x => x.Role == RoleType.FacilityGuard)) player.UnitName = "Охрана";
				foreach (Player player in Player.List.Where(x => x.UnitName == "⠀" || x.UnitName == "<color=#00000000><size=1%>⠀</size></color>")) player.UnitName = fydne;
			}
			catch { }
		}
	}
}