using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Qurre.API.Objects;
namespace Loli.Spawns
{
	public class SerpentsHand
	{
		private readonly SpawnManager SpawnManager;
		public SerpentsHand(SpawnManager SpawnManager) => this.SpawnManager = SpawnManager;
		internal static string HandTag => " SerpentsHand";
		private MapBroadcast bc;
		public static List<Player> SerpentsHands => Player.List.Where(x => x.Tag.Contains(HandTag) && x.Role is RoleType.Tutorial).ToList();
		public void Refresh() => already = false;
		public Vector3 RandomSpawnPoint
		{
			get
			{
				try
				{
					var spawn = Map.GetRandomSpawnPoint(RoleType.NtfCaptain);
					return new Vector3(spawn.x - 360, spawn.y, spawn.z);
				}
				catch { return new Vector3(-176, 989, -58); }
			}
		}
		private bool already = false;
		public static bool ItsAliveHand(Player pl) => pl.Tag.Contains(HandTag) && pl.Role is RoleType.Tutorial && Modules.EventHandlers.IsAlive(pl.UserId);
		public void Spawn()
		{
			if (Addons.OmegaWarhead.InProgress) return;
			if (Alpha.Detonated) return;
			if (Plugin.ClansWars) return;
			if (already)
			{
				SpawnManager.Spawn();
				return;
			}
			already = true;
			if (Player.List.Where(x => x.Role is RoleType.Spectator && !x.Overwatch).Count() == 0) return;
			bc = Map.Broadcast("", 25, true);
			int toSpawn = 15;
			bool spawned = false;
			Timing.RunCoroutine(SpawnPost());
			IEnumerator<float> SpawnPost()
			{
				while (!spawned)
				{
					if (0 >= toSpawn)
					{
						var list = Player.List.Where(x => x.Role is RoleType.Spectator && !x.Overwatch).ToList();
						if (list.Count() != 0)
						{
							Cassie.Send("SERPENTS HAND HASENTERED");
							bc.Message = "<size=30%><color=red>Внимание всему персоналу!</color>\n" +
								"<color=#00ffff>Отряд <color=#15ff00>Длань Змеи</color></color> <color=#0089c7>замечен на территории комплекса</color></size>";
						}
						else bc.End();
						int cc = list.Count();
						if (list.Count() > 10) list = list.Where(x => !Blocked(x)).ToList();
						bool Blocked(Player pl)
						{
							bool bl = pl.BlockThis();
							if (bl)
							{
								if (cc > 10) cc--;
								else bl = false;
							}
							return bl;
						}
						list.Shuffle();
						list = list.Take(10).ToList();
						try { SCPDiscordLogs.Api.SendMessage($"<:serpents_hand:840582086544064543> Приехал отряд длань змеи в кол-ве {list.Count} человек."); } catch { }
						foreach (Player sh in list) SpawnOne(sh);
						spawned = true;
						yield break;
					}
					else if (Player.List.Where(x => x.Role is RoleType.Spectator && !x.Overwatch).Count() != 0)
					{
						bc.Message = $"<size=30%><color=red>Внимание всему персоналу!</color>\n<color=#00ffff>Замечен отряд <color=#15ff00>Длань Змеи</color></color>\n" +
							$"<color=#0089c7>Они будут на территории комплекса через {toSpawn} секунд</color></size>";
					}
					else bc.Message = "";
					toSpawn--;
					yield return Timing.WaitForSeconds(1f);
				}
			}
		}
		public void SpawnOne(Player sh)
		{
			SpawnManager.SpawnProtect(sh);
			sh.Tag += HandTag;
			sh.Role = RoleType.Tutorial;
			sh.ClearInventory();
			sh.GetAmmo();
			sh.AddItem(ItemType.KeycardChaosInsurgency);
			sh.AddItem(ItemType.GunE11SR);
			sh.AddItem(ItemType.Radio);
			sh.AddItem(ItemType.Medkit);
			sh.AddItem(ItemType.GrenadeFlash);
			sh.AddItem(ItemType.GrenadeHE);
			sh.AddItem(ItemType.Flashlight);
			sh.AddItem(ItemType.ArmorHeavy);
			sh.Hp = 125;
			sh.MaxHp = 125;
			string mission = "<color=#00ffdc>Ваша задача - убить всех, кроме <color=red>SCP</color></color>";
			if (Plugin.RolePlay) mission = "<color=#00ffdc>Ваша задача - вывести <color=red>SCP</color> из комплекса</color>";
			sh.Broadcast(10, $"<size=30%><color=red>Вы</color> — <color=#15ff00>Длань змея</color>\n{mission}</size>", true);
			foreach (Player pl in Player.List) try { pl.Scp173Controller.IgnoredPlayers.Add(sh); } catch { }
			sh.NicknameSync.Network_customPlayerInfoString = "Длань змея";
			sh.NicknameSync.Network_playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.PowerStatus;
		}









		public void Leave(LeaveEvent ev)
		{
			if (!ev.Player.Tag.Contains(HandTag)) return;
			foreach (Player pl in Player.List) try { pl.Scp173Controller.IgnoredPlayers.Remove(ev.Player); } catch { }
		}
		public void Pocket(PocketFailEscapeEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
			ev.Player.TeleportTo106();
		}
		public void Pocket(PocketEscapeEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
			ev.Player.TeleportTo106();
		}
		public void Pocket(PocketEnterEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
		}
		public void Femur(FemurBreakerEnterEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
		}
		internal void AntiScpAttack(ScpAttackEvent ev)
		{
			if (ev.Target.Tag.Contains(HandTag)) ev.Allowed = false;
		}
		internal void Damage(DamageProcessEvent ev)
		{
			if (ev.Attacker.Id == 0) return;
			if ((ev.Target.Tag.Contains(HandTag) && (ev.Attacker.GetTeam() is Team.SCP || ev.DamageType is DamageTypes.Pocket)) ||
				(ev.Attacker.Tag.Contains(HandTag) && ev.Target.GetTeam() is Team.SCP) ||
				(ev.Target.Tag.Contains(HandTag) && ev.Attacker.Tag.Contains(HandTag) &&
				ev.Target.Id != ev.Attacker.Id))
			{
				ev.Allowed = false;
				ev.Amount = 0f;
			}
			else if (ev.Target.Tag.Contains(HandTag) && ev.Attacker.GetTeam() is Team.SCP)
			{
				ev.Allowed = false;
				ev.Amount = 0f;
			}
			else if (ev.Attacker.Tag.Contains(HandTag) && ev.Target.GetTeam() is Team.SCP)
			{
				ev.Allowed = false;
				ev.Amount = 0f;
			}
		}
		internal void Dead(DeadEvent ev)
		{
			if (ev.Target is null) return;
			if (!ev.Target.Tag.Contains(HandTag)) return;
			ev.Target.Tag = ev.Target.Tag.Replace(HandTag, "");
			foreach (Player pl in Player.List) try { pl.Scp173Controller.IgnoredPlayers.Remove(ev.Target); } catch { }
		}
		public void Spawn(RoleChangeEvent ev)
		{
			if (ev.Player is null) return;
			if (ev.NewRole is RoleType.Tutorial) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Player.Tag = ev.Player.Tag.Replace(HandTag, "");
			foreach (Player pl in Player.List) try { pl.Scp173Controller.IgnoredPlayers.Remove(ev.Player); } catch { }
		}
		public void Spawn(SpawnEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			if (ev.RoleType is RoleType.Tutorial)
			{
				//ev.Position = RandomSpawnPoint;
				int rand = Random.Range(0, 100);
				if (rand > 50) ev.Position = new Vector3(0, 1002, 8);
				else ev.Position = new Vector3(86, 989, -69);
				return;
			}
			ev.Player.Tag = ev.Player.Tag.Replace(HandTag, "");
			foreach (Player pl in Player.List) try { pl.Scp173Controller.IgnoredPlayers.Remove(ev.Player); } catch { }
		}
		public void Generator(InteractGeneratorEvent ev)
		{
			if (ev.Player is null) return;
			if (!ev.Player.Tag.Contains(HandTag)) return;
			ev.Allowed = false;
		}
	}
}