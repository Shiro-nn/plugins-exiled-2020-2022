using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using MEC;
using Respawning;
using Respawning.NamingRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roundtime
{
	public class Events
	{
		private readonly Plugin plugin;
		public Events(Plugin plugin) => this.plugin = plugin;
		public List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		private int colors = 9;
		public void wait()
		{
			ehhh();
			RespawnManager.Singleton.NamingManager.AllUnitNames.Clear();
			RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
			{
				SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
				UnitName = $"<color=red>fydne</color>"
			});
			RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
			{
				SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
				UnitName = $"Длительность раунда\n{Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}"
			});
			RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
			{
				SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
				UnitName = $"<color=#9b9b9b>Охрана</color>"
			});
		}
		public void ehhh()
		{
			string lgbt = "";
			if (colors == 9)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ff0000>#</color>" +
					$"<color=#ffff00>в</color>" +
					$"<color=#00ff3c>а</color>" +
					$"<color=#00eaff>ф</color>" +
					$"<color=#0082ff>л</color>" +
					$"<color=#0019ff>я</color>" +
					$" " +
					$"<color=#7f00ff>д</color>" +
					$"<color=#ef00ff>а</color>" +
					$"<color=#ff00ae>ф</color>" +
					$"<color=#ff0000>л</color>" +
					$"<color=#ffff00>я</color>";
			}
			else if (colors == 8)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ffff00>#</color>" +
					$"<color=#00ff3c>в</color>" +
					$"<color=#00eaff>а</color>" +
					$"<color=#0082ff>ф</color>" +
					$"<color=#0019ff>л</color>" +
					$"<color=#7f00ff>я</color>" +
					$" " +
					$"<color=#ef00ff>д</color>" +
					$"<color=#ff00ae>а</color>" +
					$"<color=#ff0000>ф</color>" +
					$"<color=#ffff00>л</color>" +
					$"<color=#00ff3c>я</color>";
			}
			else if (colors == 7)
			{
				colors--;
				lgbt = $"" +
					$"<color=#00ff3c>#</color>" +
					$"<color=#00eaff>в</color>" +
					$"<color=#0082ff>а</color>" +
					$"<color=#0019ff>ф</color>" +
					$"<color=#7f00ff>л</color>" +
					$"<color=#ef00ff>я</color>" +
					$" " +
					$"<color=#ff00ae>д</color>" +
					$"<color=#ff0000>а</color>" +
					$"<color=#ffff00>ф</color>" +
					$"<color=#00ff3c>л</color>" +
					$"<color=#00eaff>я</color>";
			}
			else if (colors == 6)
			{
				colors--;
				lgbt = $"" +
					$"<color=#00eaff>#</color>" +
					$"<color=#0082ff>в</color>" +
					$"<color=#0019ff>а</color>" +
					$"<color=#7f00ff>ф</color>" +
					$"<color=#ef00ff>л</color>" +
					$"<color=#ff00ae>я</color>" +
					$" " +
					$"<color=#ff0000>д</color>" +
					$"<color=#ffff00>а</color>" +
					$"<color=#00ff3c>ф</color>" +
					$"<color=#00eaff>л</color>" +
					$"<color=#0082ff>я</color>";
			}
			else if (colors == 5)
			{
				colors--;
				lgbt = $"" +
					$"<color=#0082ff>#</color>" +
					$"<color=#0019ff>в</color>" +
					$"<color=#7f00ff>а</color>" +
					$"<color=#ef00ff>ф</color>" +
					$"<color=#ff00ae>л</color>" +
					$"<color=#ff0000>я</color>" +
					$" " +
					$"<color=#ffff00>д</color>" +
					$"<color=#00ff3c>а</color>" +
					$"<color=#00eaff>ф</color>" +
					$"<color=#0082ff>л</color>" +
					$"<color=#ff0000>я</color>";
			}
			else if (colors == 4)
			{
				colors--;
				lgbt = $"" +
					$"<color=#0019ff>#</color>" +
					$"<color=#7f00ff>в</color>" +
					$"<color=#ef00ff>а</color>" +
					$"<color=#ff00ae>ф</color>" +
					$"<color=#ff0000>л</color>" +
					$"<color=#ffff00>я</color>" +
					$" " +
					$"<color=#00ff3c>д</color>" +
					$"<color=#00eaff>а</color>" +
					$"<color=#0082ff>ф</color>" +
					$"<color=#ff0000>л</color>" +
					$"<color=#ffff00>я</color>";
			}
			else if (colors == 3)
			{
				colors--;
				lgbt = $"" +
					$"<color=#7f00ff>#</color>" +
					$"<color=#ef00ff>в</color>" +
					$"<color=#ff00ae>а</color>" +
					$"<color=#ff0000>ф</color>" +
					$"<color=#ffff00>л</color>" +
					$"<color=#00ff3c>я</color>" +
					$" " +
					$"<color=#00eaff>д</color>" +
					$"<color=#0082ff>а</color>" +
					$"<color=#ff0000>ф</color>" +
					$"<color=#ffff00>л</color>" +
					$"<color=#00ff3c>я</color>";
			}
			else if (colors == 3)
			{
				colors--;
				lgbt = $"" +
					$"<color=#ef00ff>#</color>" +
					$"<color=#ff00ae>в</color>" +
					$"<color=#ff0000>а</color>" +
					$"<color=#ffff00>ф</color>" +
					$"<color=#00ff3c>л</color>" +
					$"<color=#00eaff>я</color>" +
					$" " +
					$"<color=#0082ff>д</color>" +
					$"<color=#ff0000>а</color>" +
					$"<color=#ffff00>ф</color>" +
					$"<color=#00ff3c>л</color>" +
					$"<color=#00eaff>я</color>";
			}
			else if (colors == 2)
			{
				colors = 9;
				lgbt = $"" +
					$"<color=#ff00ae>#</color>" +
					$"<color=#ff0000>в</color>" +
					$"<color=#ffff00>а</color>" +
					$"<color=#00ff3c>ф</color>" +
					$"<color=#00eaff>л</color>" +
					$"<color=#0082ff>я</color>" +
					$" " +
					$"<color=#ff0000>д</color>" +
					$"<color=#ffff00>а</color>" +
					$"<color=#00ff3c>ф</color>" +
					$"<color=#00eaff>л</color>" +
					$"<color=#0082ff>я</color>";
			}
			else
			{
				colors = 9;
			}
			foreach (Player player in Player.List.Where(x => x.ReferenceHub.characterClassManager.NetworkCurUnitName.Replace("</color>", "")
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
			.Replace("<color=#0082ff>", "")
			== "#вафля дафля"))
			{
				player.ReferenceHub.GetComponent<UnitNamingManager>();
				player.ReferenceHub.characterClassManager.NetworkCurUnitName = lgbt;
			}
			foreach (Player player in Player.List.Where(x => x.Role == RoleType.FacilityGuard))
			{
				player.ReferenceHub.characterClassManager.NetworkCurUnitName = $"<color=#9b9b9b>Охрана</color>";
			}
			if (Round.IsStarted)
			{
				try
				{
					RespawnManager.Singleton.NamingManager.AllUnitNames.Insert(0, new SyncUnit
					{
						SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
						UnitName = lgbt
					});
					RespawnManager.Singleton.NamingManager.AllUnitNames.Insert(2, new SyncUnit
					{
						SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
						UnitName = $"<size=80%><color=#fdffbb>Длительность раунда</color>\n<color=#0089c7>{Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}</color></size>"
					});
					RespawnManager.Singleton.NamingManager.AllUnitNames.RemoveAt(1);
					RespawnManager.Singleton.NamingManager.AllUnitNames.RemoveAt(2);
				}
				catch { }
			}

			Timing.CallDelayed(1f, () => ehhh());
		}
		internal void hurt(HurtingEventArgs ev)
		{
			if (ev.Target != ev.Attacker)
			{
				string str;
				if (ev.Target.IsGodModeEnabled)
				{
					str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>GodMode</color>.</b>";
				}
				else if (ev.Target.ReferenceHub.playerStats.Health + ev.Target.ArtificialHealth - ev.Amount > 0)
				{
					if (ev.Target.ArtificialHealth - ev.Amount > 0)
					{
						str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>{Math.Round(ev.Target.ReferenceHub.playerStats.Health)}</color><color=red>HP</color>.</b>\n<color=#0089c7>{Math.Round(ev.Target.ArtificialHealth - ev.Amount)}</color><color=#00ff88>AHP</color>";
					}
					else if (ev.Target.ArtificialHealth > 0)
					{
						str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>{Math.Round(ev.Target.ReferenceHub.playerStats.Health + ev.Target.ArtificialHealth - ev.Amount)}</color><color=red>HP</color>.</b>";
					}
					else
					{
						str = $"\n<b><color=#00ffff>{ev.Target.Nickname}</color>: <color=#0089c7>{Math.Round(ev.Target.ReferenceHub.playerStats.Health - ev.Amount)}</color><color=red>HP</color>.</b>";
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