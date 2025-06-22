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
			if (Plugin.ClansWars)
			{
				AddUnits("<color=#ff004c>#</color><color=#ff0000>f</color><color=#ff2a00>y</color><color=#ff5500>d</color><color=#ff6a00>n</color>" +
					"<color=#ff7b00>e</color>");
				AddUnits("<color=#ff9500>К</color><color=#ffb300>л</color><color=#ffc400>а</color><color=#ffd500>н</color>" +
					"<color=#ffe600>о</color><color=#e1ff00>в</color><color=#d0ff00>ы</color><color=#c8ff00>е</color> " +
					"<color=#bbff00>В</color><color=#aeff00>о</color><color=#9dff00>й</color><color=#84ff00>н</color><color=#62ff00>ы</color>");
			}
			else if (Plugin.Anarchy)
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
			else if (Plugin.HardRP)
			{
				AddUnits("<color=#ff3030>#</color><color=#ff223f>f</color><color=#ff0e4e>y</color><color=#ff005d>d</color><color=#ff006d>n</color>" +
					"<color=#ff007d>e</color>");
				AddUnits("<color=#ff008d>H</color><color=#ff009e>a</color><color=#ff00af>r</color><color=#ff16c0>d</color> " +
					"<color=#ff2bd0>R</color><color=#ff3be1>P</color>");
			}
			else if (Plugin.ServerID == 1)
			{
				AddUnits(fydne);
				AddUnits("<color=#3d6796>ff</color><color=#516892>:</color><color=#3ee900>off</color>");
			}
			else if (Plugin.ServerID == 2)
			{
				AddUnits(fydne);
				AddUnits("<color=#3d6796>ff</color><color=#516892>:</color><color=#ff8f00>on</color>");
			}
			else if (Plugin.ServerID == 3)
			{
				AddUnits(fydne);
				AddUnits("<color=#e400ff>N</color><color=#be44ff>o</color><color=#985bff>R</color><color=#7168fe>u</color>" +
					"<color=#4c70f4>l</color><color=#2675e5>e</color><color=#0077d3>s</color> <color=#0089c7>#</color><color=#9bff00>1</color>" +
					"<b><size=75%> <color=#00ffa2>[</color><color=#00e8cc>E</color><color=#00cde6>K</color><color=#00b0e8>B</color><color=#0090d3>]</color>" +
					"</size></b>");
			}
			else if (Plugin.ServerID == 4)
			{
				AddUnits(fydne);
				AddUnits("<color=#e400ff>N</color><color=#be44ff>o</color><color=#985bff>R</color><color=#7168fe>u</color>" +
					"<color=#4c70f4>l</color><color=#2675e5>e</color><color=#0077d3>s</color> <color=#0089c7>#</color><color=#9bff00>2</color>" +
					"<b><size=75%> <color=#00ffa2>[</color><color=#00e8cc>M</color><color=#00cde6>S</color><color=#00b0e8>K</color><color=#0090d3>]</color>" +
					"</size></b>");
			}
			else if (Plugin.ServerID == 5)
			{
				AddUnits(fydne);
				AddUnits("<color=#e400ff>N</color><color=#be44ff>o</color><color=#985bff>R</color><color=#7168fe>u</color>" +
					"<color=#4c70f4>l</color><color=#2675e5>e</color><color=#0077d3>s</color> <color=#0089c7>#</color><color=#9bff00>3</color>" +
					"<b><size=75%> <color=#00ffa2>[</color><color=#00e8cc>B</color><color=#00cde6>R</color><color=#00b0e8>Y</color><color=#0090d3>]</color>" +
					"</size></b>");
			}
			else if (Plugin.YouTubersServer)
			{
				AddUnits("<color=#ff0000>#</color><color=#f93239>f</color><color=#f24751>y</color><color=#ec5763>d</color><color=#e56472>n</color>" +
					"<color=#de7080>e</color>");
				AddUnits("<b><size=85%>" +
					"<color=#d77b8c>Y</color><color=#d08597>o</color><color=#c88ea1>u</color><color=#c096ab>T</color><color=#b89eb4>u</color>" +
					"<color=#afa6bd>b</color><color=#a6aec6>e</color><color=#9cb5ce>r</color><color=#92bbd5>s</color> <color=#87c2dd>S</color>" +
					"<color=#7bc8e4>e</color><color=#6dcfeb>r</color><color=#5ed5f2>v</color><color=#4cdaf9>e</color><color=#33e0ff>r</color>" +
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