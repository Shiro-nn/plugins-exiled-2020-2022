using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MongoDB.scp228
{
	public partial class EventHandlers228
	{
		internal static Plugin plugin;
		public EventHandlers228(Plugin plugi) => plugin = plugi;

		internal static ReferenceHub scp228ruj;
		private bool isRoundStarted;
		public const float dur = 1327;
		public static Pickup vodka = new Pickup();
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		public static bool dedopen = false;
		public static bool aopen = false;
		public static bool checkopen = false;
		public static bool gateopen = false;
		public static bool ds = false;
		public static string vodka1 = Configs.error1;
		public static string vodka2 = Configs.error2;
		public static string vodkacolor = Configs.error3;
		public static bool tptovodka = false;
		public void console(SendingConsoleCommandEventArgs ev)
		{
			if (ev.Name == "vodka" || ev.Name == "водка")
			{
				if (ev.Player.ReferenceHub.queryProcessor.PlayerId != Extensions.TryGet228()?.queryProcessor.PlayerId)
				{
					ev.ReturnMessage = "Вы не SCP 228 RU J";
					ev.Color = "red";
					return;
				}
				if (ev.Arguments.Count < 1)
				{
					if (ev.Player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
					{
						if (plugin.donate.main[ev.Player.ReferenceHub.characterClassManager.UserId].money >= 20)
						{
							plugin.donate.main[ev.Player.ReferenceHub.characterClassManager.UserId].money -= 20;
							ev.ReturnMessage = vodka2;
							ev.Color = vodkacolor;
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(5, vodka1);
							tptovodka = true;
							return;
						}
						ev.ReturnMessage = $"Недостаточно средств({plugin.donate.main[ev.Player.ReferenceHub.characterClassManager.UserId].money}/20).";
						ev.Color = "red";
						return;
					}
				}
				if (ev.Arguments[0] == "tp" || ev.Arguments[0] == "тп")
				{
					if (ev.Player.ReferenceHub.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
					{
						if (tptovodka)
						{
							if (plugin.donate.main[ev.Player.ReferenceHub.characterClassManager.UserId].money >= 50)
							{
								plugin.donate.main[ev.Player.ReferenceHub.characterClassManager.UserId].money -= 50;
								ev.ReturnMessage = "Вы телепортированы к водке...";
								ev.Color = "cyan";
								ev.Player.ClearBroadcasts();
								ev.Player.Broadcast(5, "<color=aqua>Вы телепортированы к водке...</color>");
								ev.Player.ReferenceHub.SetPosition(vodka.Rb.position + Vector3.up * 2);
								return;
							}
							ev.ReturnMessage = $"Недостаточно средств({plugin.donate.main[ev.Player.ReferenceHub.characterClassManager.UserId].money}/50).";
							ev.Color = "red";
						}
						return;
					}
				}
			}
		}

		public void OnWaitingForPlayers()
		{
			try
			{
				Configs.ReloadConfig();
			}
			catch { }
		}
		public void OnRoundStart()
		{
			isRoundStarted = true;
			scp228ruj = null;
			ffPlayers.Clear();
			scpPlayer = null;
			Timing.CallDelayed(1f, () => selectspawnJG());

			players.Clear();
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			isRoundStarted = false;
			aopen = false;
			dedopen = false;
			checkopen = false;
			gateopen = false;
			scp228ruj = null;
		}

		public void OnRoundRestart()
		{

			isRoundStarted = false;
		}

		public void OnPlayerDeath(DiedEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				Killscp228ruj();
			}
		}
		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.DamageType == DamageTypes.Decont || ev.DamageType == DamageTypes.Nuke)
				{
					return;
				}
			}
			if (ffPlayers.Contains(ev.Attacker.ReferenceHub.queryProcessor.PlayerId))
			{
				GrantFF(ev.Attacker.ReferenceHub);
			}
			if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				ev.Amount = 0f;
			}
		}

		public void OnCheckEscape(EscapingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (ds)
				{
					ev.IsAllowed = false;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.eebt, Configs.eeb);
				}
				if (!ds)
				{
					ev.NewRole = RoleType.Spectator;
					ev.IsAllowed = true;
					escapescp228ruj();
				}
			}
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if ((scp228ruj.GetRole() == RoleType.Spectator))
				{
					Killscp228ruj();
				}
			}
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				Killscp228ruj();
			}
		}

		public void OnContain106(EnteringFemurBreakerEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				shPocketPlayers.Add(ev.Player.ReferenceHub.queryProcessor.PlayerId);
				ev.IsAllowed = false;
			}
		}

		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				Extensions.TeleportTo106(ev.Player.ReferenceHub);
			}
		}

		public void OnPocketDimensionExit(EscapingPocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				Extensions.TeleportTo106(ev.Player.ReferenceHub);
			}
		}
		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = true;
				if (ev.Pickup.durability == dur)
				{
					ev.IsAllowed = true;
					ds = false;
					checkopen = true;
					gateopen = true;
					scp228ruj.ClearBroadcasts();
					scp228ruj.Broadcast(Configs.svb, Configs.svbt);
					return;
				}
				else if (ev.Pickup.itemId == ItemType.Coin)
				{
					ev.IsAllowed = false;
					return;
				}
				else if (ev.Pickup.durability == 100000)
				{
					ev.IsAllowed = false;
					return;
				}
				else if (ev.Pickup.durability == 10000)
				{
					ev.IsAllowed = false;
					return;
				}
				else if (ev.Pickup.durability == scp035.EventHandlers.dur)
				{
					ev.IsAllowed = false;
					return;
				}
				else if (ev.Pickup == plugin.armor.jugarmor)
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					plugin.armor.jugarmor = MongoDB.Extensions.SpawnItem(ev.Pickup.itemId, ev.Pickup.durability, ev.Pickup.transform.position);
				}
				else
				{
					ev.IsAllowed = false;
					ev.Pickup.Delete();
					Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position, ev.Pickup.rotation,
							ev.Pickup.weaponMods.Sight,
							ev.Pickup.weaponMods.Barrel,
							ev.Pickup.weaponMods.Other);
				}
				ev.IsAllowed = false;
			}
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId != scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.Pickup.durability == dur)
				{
					ev.IsAllowed = false;
					scp228ruj.ClearBroadcasts();
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.vppbt, Configs.vppb.Replace("%player%", $"{scp228ruj?.nicknameSync.Network_myNickSync}"));
					scp228ruj.Broadcast(Configs.vpb.Replace("%player%", $"{ev.Player?.Nickname}"), Configs.vpbt);
					ev.Pickup.Delete();
					vodka = Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position);
					return;
				}
			}
		}
		public void scpzeroninesixe(EnragingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public void scpzeroninesixeadd(AddingTargetEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				try
				{
					if (gateopen)
					{
						if (ev.Door.GetComponent<DoorNametagExtension>().GetName.Contains("GATE"))
						{
							ev.IsAllowed = true;
						}
					}
					if (checkopen)
					{
						if (ev.Door.GetComponent<DoorNametagExtension>().GetName.Contains("CHECKPOINT"))
						{
							ev.IsAllowed = true;
						}
					}
					if (dedopen)
					{
						if (ev.Door.GetComponent<DoorNametagExtension>().GetName.Contains("106"))
						{
							ev.IsAllowed = true;
						}
					}
					if (aopen)
					{
						if (ev.Door.GetComponent<DoorNametagExtension>().GetName.Contains("096"))
						{
							ev.IsAllowed = true;
						}
					}
				}
				catch { }
			}
		}
		internal void CorrodeUpdate()
		{
			if (isRoundStarted && scp228ruj != null)
			{
				IEnumerable<Player> pList = Player.List.Where(x => x.ReferenceHub.queryProcessor.PlayerId != scp228ruj.queryProcessor.PlayerId);
				if (!Configs.scpFriendlyFire) pList = pList.Where(x => Extensions.GetTeam(x.ReferenceHub) != Team.SCP);
				if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Extensions.GetTeam(x.ReferenceHub) != Team.TUT);
				if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Extensions.GetTeam(x.ReferenceHub) != Team.RIP);
				foreach (Player pplayer in pList)
				{
					ReferenceHub player = pplayer.ReferenceHub;
					if (player != null && Vector3.Distance(scp228ruj.transform.position, player.transform.position) <= 2f)
					{
						CorrodePlayer(player);
						pplayer.ClearBroadcasts();
						pplayer.Broadcast(1, Configs.a, 0);
					}
				}
			}
		}
		private int t = 0;
		internal void Gopd()
		{
			t++;
			if(t == 3)
			{
				t = 0;
				if (ds)
				{
					opd();
				}
			}
		}
		private static void opd()
		{
			scp228ruj.Damage(1, DamageTypes.Nuke);
		}
		private void CorrodePlayer(ReferenceHub player)
		{
			player.Damage(5, DamageTypes.Nuke);
		}
		public void RunOnRACommandSent(SendingRemoteAdminCommandEventArgs ev)
		{
			try
			{
				string name = string.Join(" ", ev.Arguments.Skip(0));
				Player player = Player.Get(name);
				if (ev.Name == Configs.com)
				{
					ev.IsAllowed = false;
					if (ev.CommandSender.SenderId == "-@steam"
						|| ev.CommandSender.SenderId == "SERVER CONSOLE")
					{
						if (player == null)
						{
							ev.ReplyMessage = Configs.nf;
							return;
						}
						ev.ReplyMessage = Configs.suc;
						SpawnJG(player.ReferenceHub);
					}
					else
					{
						ev.ReplyMessage = "Отказано в доступе";
					}
				}
			}
			catch (Exception e)
			{
				if (ev.Name == Configs.com)
				{
					ev.IsAllowed = false;
					ev.ReplyMessage = $"\nПроизошла ошибка:\n{e}";
				}
			}
		}
		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}
	}
}