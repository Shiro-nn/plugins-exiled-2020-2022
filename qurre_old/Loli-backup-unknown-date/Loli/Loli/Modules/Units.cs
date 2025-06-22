using Qurre.API;
using Qurre.API.Objects;
using Respawning;
using System.Linq;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		private string fydne = "<color=#ff0000>#</color>" +
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
			if (Plugin.ClansWars) fydne = "<color=#ff004c>#</color><color=#ff0000>f</color><color=#ff2a00>y</color><color=#ff5500>d</color><color=#ff6a00>n</color><color=#ff7b00>e</color>";
			else if (Plugin.RolePlay) fydne = "<color=#ff0000>#</color><color=#ff004c>f</color><color=#ff007b>y</color><color=#ff00a2>d</color><color=#e600ff>n</color><color=#c300ff>e</color>";
			AddUnits(fydne);
			if (Plugin.ClansWars)
			{
				AddUnits("<color=#ff9500>К</color><color=#ffb300>л</color><color=#ffc400>а</color><color=#ffd500>н</color>" +
					"<color=#ffe600>о</color><color=#e1ff00>в</color><color=#d0ff00>ы</color><color=#c8ff00>е</color> " +
					"<color=#bbff00>В</color><color=#aeff00>о</color><color=#9dff00>й</color><color=#84ff00>н</color><color=#62ff00>ы</color>");
			}
			else if (Plugin.ServerID == 8)
			{
				AddUnits("<color=#c300ff>M</color><color=#b300ff>e</color><color=#9900ff>d</color>" +
					"<color=#8400ff>i</color><color=#6200ff>u</color><color=#4000ff>m</color> <color=#1900ff>R</color><color=#0033ff>P</color>");
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
				foreach (Player player in Player.List.Where(x => x.UnitName == "⠀" || x.UnitName == "<size=1%>⠀</size>")) player.UnitName = fydne;
			}
			catch { }
		}
	}
}