using Hints;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using Respawning;
using Respawning.NamingRules;
using System;
using System.Collections.Generic;
using System.Linq;
namespace roundtime
{
	public class Events
	{
		private readonly Plugin plugin;
		public Events(Plugin plugin) => this.plugin = plugin;
		public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		private int colors = 9;
		private bool first = true;
		public void wait()
		{
			RespawnManager.Singleton.NamingManager.AllUnitNames.Clear();
			string str1 = $"" +
					$"<color=#ff00ae>#</color>" +
					$"<color=#ffff00>I</color>" +
					$"<color=#00ff3c>n</color>" +
					$"<color=#00eaff>f</color>" +
					$"<color=#0082ff>l</color>" +
					$"<color=#0019ff>u</color>" +
					$"<color=#7f00ff>e</color>" +
					$"<color=#ef00ff>n</color>" +
					$"<color=#ff00ae>c</color>" +
					$"<color=#ff0000>e</color>" +
					$" " +
					$"<color=#ffff00>M</color>" +
					$"<color=#00ff3c>o</color>" +
					$"<color=#00eaff>o</color>" +
					$"<color=#0082ff>n</color>";
			Round.AddUnit(TeamUnitType.ChaosInsurgency, str1);
			Round.AddUnit(TeamUnitType.ClassD, str1);
			Round.AddUnit(TeamUnitType.NineTailedFox, str1);
			Round.AddUnit(TeamUnitType.None, str1);
			Round.AddUnit(TeamUnitType.Scientist, str1);
			Round.AddUnit(TeamUnitType.Scp, str1);
			Round.AddUnit(TeamUnitType.Tutorial, str1);
			Round.AddUnit(TeamUnitType.NineTailedFox, "Охрана");
			if (first) ehhh();
		}
		public void ehhh()
		{
			first = false;
			string lgbt = "";
			if (colors == 9)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ff0000>#</color>" +
					$"<color=#ffff00>I</color>" +
					$"<color=#00ff3c>n</color>" +
					$"<color=#00eaff>f</color>" +
					$"<color=#0082ff>l</color>" +
					$"<color=#0019ff>u</color>" +
					$"<color=#7f00ff>e</color>" +
					$"<color=#ef00ff>n</color>" +
					$"<color=#ff00ae>c</color>" +
					$"<color=#ff0000>e</color>" +
					$"<color=#ffff00>M</color>" +
					$"<color=#00ff3c>o</color>" +
					$"<color=#00eaff>o</color>" +
					$"<color=#0082ff>n</color>";
			}
			else if (colors == 8)
			{
				colors--;
				lgbt = $"" +
					$"<color=#0082ff>#</color>" +
					$"<color=#ff0000>I</color>" +
					$"<color=#ffff00>n</color>" +
					$"<color=#00ff3c>f</color>" +
					$"<color=#00eaff>l</color>" +
					$"<color=#0082ff>u</color>" +
					$"<color=#0019ff>e</color>" +
					$"<color=#7f00ff>n</color>" +
					$"<color=#ef00ff>c</color>" +
					$"<color=#ff00ae>e</color>" +
					$"<color=#ff0000>M</color>" +
					$"<color=#ffff00>o</color>" +
					$"<color=#00ff3c>o</color>" +
					$"<color=#00eaff>n</color>";
			}
			else if (colors == 7)
			{
				colors--;
				lgbt = $"" +
					$"<color=#00eaff>#</color>" +
					$"<color=#0082ff>I</color>" +
					$"<color=#ff0000>n</color>" +
					$"<color=#ffff00>f</color>" +
					$"<color=#00ff3c>l</color>" +
					$"<color=#00eaff>u</color>" +
					$"<color=#0082ff>e</color>" +
					$"<color=#0019ff>n</color>" +
					$"<color=#7f00ff>c</color>" +
					$"<color=#ef00ff>e</color>" +
					$"<color=#ff00ae>M</color>" +
					$"<color=#ff0000>o</color>" +
					$"<color=#ffff00>o</color>" +
					$"<color=#00ff3c>n</color>";
			}
			else if (colors == 6)
			{
				colors--;
				lgbt = $"" +
					$"<color=#00ff3c>#</color>" +
					$"<color=#00eaff>I</color>" +
					$"<color=#0082ff>n</color>" +
					$"<color=#ff0000>f</color>" +
					$"<color=#ffff00>l</color>" +
					$"<color=#00ff3c>u</color>" +
					$"<color=#00eaff>e</color>" +
					$"<color=#0082ff>n</color>" +
					$"<color=#0019ff>c</color>" +
					$"<color=#7f00ff>e</color>" +
					$"<color=#ef00ff>M</color>" +
					$"<color=#ff00ae>o</color>" +
					$"<color=#ff0000>o</color>" +
					$"<color=#ffff00>n</color>";
			}
			else if (colors == 5)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ffff00>#</color>" +
					$"<color=#00ff3c>I</color>" +
					$"<color=#00eaff>n</color>" +
					$"<color=#0082ff>f</color>" +
					$"<color=#ff0000>l</color>" +
					$"<color=#ffff00>u</color>" +
					$"<color=#00ff3c>e</color>" +
					$"<color=#00eaff>n</color>" +
					$"<color=#0082ff>c</color>" +
					$"<color=#0019ff>e</color>" +
					$"<color=#7f00ff>M</color>" +
					$"<color=#ef00ff>o</color>" +
					$"<color=#ff00ae>o</color>" +
					$"<color=#ff0000>n</color>";
			}
			else if (colors == 4)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ff0000>#</color>" +
					$"<color=#ffff00>I</color>" +
					$"<color=#00ff3c>n</color>" +
					$"<color=#00eaff>f</color>" +
					$"<color=#0082ff>l</color>" +
					$"<color=#ff0000>u</color>" +
					$"<color=#ffff00>e</color>" +
					$"<color=#00ff3c>n</color>" +
					$"<color=#00eaff>c</color>" +
					$"<color=#0082ff>e</color>" +
					$"<color=#0019ff>M</color>" +
					$"<color=#7f00ff>o</color>" +
					$"<color=#ef00ff>o</color>" +
					$"<color=#ff00ae>n</color>";
			}
			else if (colors == 3)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ff00ae>#</color>" +
					$"<color=#ff0000>I</color>" +
					$"<color=#ffff00>n</color>" +
					$"<color=#00ff3c>f</color>" +
					$"<color=#00eaff>l</color>" +
					$"<color=#0082ff>u</color>" +
					$"<color=#ff0000>e</color>" +
					$"<color=#ffff00>n</color>" +
					$"<color=#00ff3c>c</color>" +
					$"<color=#00eaff>e</color>" +
					$"<color=#0082ff>M</color>" +
					$"<color=#0019ff>o</color>" +
					$"<color=#7f00ff>o</color>" +
					$"<color=#ef00ff>n</color>";
			}
			else if (colors == 3)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ef00ff>#</color>" +
					$"<color=#ff00ae>I</color>" +
					$"<color=#ff0000>n</color>" +
					$"<color=#ffff00>f</color>" +
					$"<color=#00ff3c>l</color>" +
					$"<color=#00eaff>u</color>" +
					$"<color=#0082ff>e</color>" +
					$"<color=#ff0000>n</color>" +
					$"<color=#ffff00>c</color>" +
					$"<color=#00ff3c>e</color>" +
					$"<color=#00eaff>M</color>" +
					$"<color=#0082ff>o</color>" +
					$"<color=#0019ff>o</color>" +
					$"<color=#7f00ff>n</color>";
			}
			else if (colors == 2)
			{
				colors--;
				lgbt = $"" +
					$"<color=#7f00ff>#</color>" +
					$"<color=#ef00ff>I</color>" +
					$"<color=#ff00ae>n</color>" +
					$"<color=#ff0000>f</color>" +
					$"<color=#ffff00>l</color>" +
					$"<color=#00ff3c>u</color>" +
					$"<color=#00eaff>e</color>" +
					$"<color=#0082ff>n</color>" +
					$"<color=#ff0000>c</color>" +
					$"<color=#ffff00>e</color>" +
					$"<color=#00ff3c>M</color>" +
					$"<color=#00eaff>o</color>" +
					$"<color=#0082ff>o</color>" +
					$"<color=#0019ff>n</color>";
			}
			else if (colors == 1)
			{
				colors--;
				lgbt = $"" +
					$"<color=#0019ff>#</color>" +
					$"<color=#7f00ff>I</color>" +
					$"<color=#ef00ff>n</color>" +
					$"<color=#ff00ae>f</color>" +
					$"<color=#ff0000>l</color>" +
					$"<color=#ffff00>u</color>" +
					$"<color=#00ff3c>e</color>" +
					$"<color=#00eaff>n</color>" +
					$"<color=#0082ff>c</color>" +
					$"<color=#ff0000>e</color>" +
					$"<color=#ffff00>M</color>" +
					$"<color=#00ff3c>o</color>" +
					$"<color=#00eaff>o</color>" +
					$"<color=#0082ff>n</color>";
			}
			else if (colors == 0)
			{
				colors--;
				lgbt = $"" +
					$"<color=#0082ff>#</color>" +
					$"<color=#0019ff>I</color>" +
					$"<color=#7f00ff>n</color>" +
					$"<color=#ef00ff>f</color>" +
					$"<color=#ff00ae>l</color>" +
					$"<color=#ff0000>u</color>" +
					$"<color=#ffff00>e</color>" +
					$"<color=#00ff3c>n</color>" +
					$"<color=#00eaff>c</color>" +
					$"<color=#0082ff>e</color>" +
					$"<color=#ff0000>M</color>" +
					$"<color=#ffff00>o</color>" +
					$"<color=#00ff3c>o</color>" +
					$"<color=#00eaff>n</color>";
			}
			else if (colors == -1)
			{
				colors--;
				lgbt = $"" +
					$"<color=#00eaff>#</color>" +
					$"<color=#0082ff>I</color>" +
					$"<color=#0019ff>n</color>" +
					$"<color=#7f00ff>f</color>" +
					$"<color=#ef00ff>l</color>" +
					$"<color=#ff00ae>u</color>" +
					$"<color=#ff0000>e</color>" +
					$"<color=#ffff00>n</color>" +
					$"<color=#00ff3c>c</color>" +
					$"<color=#00eaff>e</color>" +
					$"<color=#0082ff>M</color>" +
					$"<color=#ff0000>o</color>" +
					$"<color=#ffff00>o</color>" +
					$"<color=#00ff3c>n</color>";
			}
			else if (colors == -2)
			{
				colors--;
				lgbt = $"" +
					$"<color=#00ff3c>#</color>" +
					$"<color=#00eaff>I</color>" +
					$"<color=#0082ff>n</color>" +
					$"<color=#0019ff>f</color>" +
					$"<color=#7f00ff>l</color>" +
					$"<color=#ef00ff>u</color>" +
					$"<color=#ff00ae>e</color>" +
					$"<color=#ff0000>n</color>" +
					$"<color=#ffff00>c</color>" +
					$"<color=#00ff3c>e</color>" +
					$"<color=#00eaff>M</color>" +
					$"<color=#0082ff>o</color>" +
					$"<color=#ff0000>o</color>" +
					$"<color=#ffff00>n</color>";
			}
			else if (colors == -3)
			{
				colors = 9;
				lgbt = $"" +
					$"<color=#ffff00>#</color>" +
					$"<color=#00ff3c>I</color>" +
					$"<color=#00eaff>n</color>" +
					$"<color=#0082ff>f</color>" +
					$"<color=#0019ff>l</color>" +
					$"<color=#7f00ff>u</color>" +
					$"<color=#ef00ff>e</color>" +
					$"<color=#ff00ae>n</color>" +
					$"<color=#ff0000>c</color>" +
					$"<color=#ffff00>e</color>" +
					$"<color=#00ff3c>M</color>" +
					$"<color=#00eaff>o</color>" +
					$"<color=#0082ff>o</color>" +
					$"<color=#ff0000>n</color>";
			}
			else
			{
				colors = 9;
			}
			foreach (Player player in Player.List.Where(x => x.UnitName.Replace("</color>", "")
			.Replace("<color=#ff0000>", "")
			.Replace("<color=#ffff00>", "")
			.Replace("<color=#00ff3c>", "")
			.Replace("<color=#00eaff>", "")
			.Replace("<color=#0082ff>", "")
			.Replace("<color=#0019ff>", "")
			.Replace("<color=#7f00ff>", "")
			.Replace("<color=#ef00ff>", "")
			.Replace("<color=#ff00ae>", "")
			.Replace("<color=#ff0000>", "")
			.Replace("<color=#ffff00>", "")
			.Replace("<color=#00ff3c>", "")
			.Replace("<color=#00eaff>", "")
			.Replace("<color=#0082ff>", "").ToLower()
			== "#influencemoon"))
			{
				player.ReferenceHub.GetComponent<UnitNamingManager>();
				player.ReferenceHub.characterClassManager.NetworkCurUnitName = lgbt;
			}
			foreach (Player player in Player.List.Where(x => x.Role == RoleType.FacilityGuard))
			{
				player.UnitName = $"<color=#9b9b9b>Охрана</color>";
			}
			if (Round.Started)
			{
				try
				{
					Round.RenameUnit(TeamUnitType.None, 0, lgbt);
					Round.RenameUnit(TeamUnitType.ChaosInsurgency, 1, lgbt);
					Round.RenameUnit(TeamUnitType.NineTailedFox, 2, lgbt);
					Round.RenameUnit(TeamUnitType.ClassD, 3, lgbt);
					Round.RenameUnit(TeamUnitType.Scientist, 4, lgbt);
					Round.RenameUnit(TeamUnitType.Scp, 5, lgbt);
					Round.RenameUnit(TeamUnitType.Tutorial, 6, lgbt);
				}
				catch { }
			}
			
			Timing.CallDelayed(1f, () => ehhh());
		}
		internal void hurt(DamageEvent ev)
		{
			if (ev.Target != ev.Attacker)
			{
				string str;
				if (ev.Target.GodMode)
				{
					str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>GodMode</color>.</b>";
				}
				else if (ev.Target.Hp + ev.Target.Ahp - ev.Amount > 0)
				{
					if (ev.Target.Ahp - ev.Amount > 0)
					{
						str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>{Math.Round(ev.Target.Hp)}</color><color=red>HP</color>.</b>\n<color=#0089c7>{Math.Round(ev.Target.Ahp - ev.Amount)}</color><color=#00ff88>AHP</color>";
					}
					else if (ev.Target.Ahp > 0)
					{
						str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>{Math.Round(ev.Target.Hp + ev.Target.Ahp - ev.Amount)}</color><color=red>HP</color>.</b>";
					}
					else
					{
						str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>{Math.Round(ev.Target.Hp - ev.Amount)}</color><color=red>HP</color>.</b>";
					}
				}
				else
				{
					str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>Убит</color><color=red>!</color></b>";
				}
				Extensions.Hint(ev.Attacker.ReferenceHub, str, 1f);
			}
		}
	}
	public static class Extensions
	{
		public static void Hint(ReferenceHub player, string message, float time)
		{
			player.hints.Show(new TextHint(message.Trim(), new HintParameter[] { new StringHintParameter("") }, HintEffectPresets.FadeInAndOut(0.25f, time, 0f), 10f));
		}
	}
}