using Exiled.API.Features;
using Exiled.Events.EventArgs;
using PlayerXP.events.hideandseek.API;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerXP.scp228
{
	public partial class EventHandlers228
	{
		public Plugin plugin;
		public EventHandlers228(Plugin plugin) => this.plugin = plugin;

		internal static ReferenceHub scp228ruj;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		public static bool pickupspawn;
		public const float dur = 327;
		public static Pickup vodka = new Pickup();
		private static System.Random rand = new System.Random();
		private static int RoundEnds;
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		HashSet<ReferenceHub> players = new HashSet<ReferenceHub>();
		private List<int> shPocketPlayers = new List<int>();
		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
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
						if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 20)
						{
							plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 20;
							ev.ReturnMessage = vodka2;
							ev.Color = vodkacolor;
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(5, vodka1);
							tptovodka = true;
							return;
						}
						ev.ReturnMessage = $"Недостаточно средств({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/20).";
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
							if (plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] >= 50)
							{
								plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId] -= 50;
								ev.ReturnMessage = "Вы телепортированы к водке...";
								ev.Color = "cyan";
								ev.Player.ClearBroadcasts();
								ev.Player.Broadcast(5, "<color=aqua>Вы телепортированы к водке...</color>");
								ev.Player.ReferenceHub.SetPosition(vodka.Rb.position + Vector3.up * 2);
								return;
							}
							ev.ReturnMessage = $"Недостаточно средств({plugin.donate.money[ev.Player.ReferenceHub.characterClassManager.UserId]}/50).";
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

		private bool Tryhason()
		{
			return hasData.Gethason();
		}
		public void OnRoundStart()
		{
			isRoundStarted = true;
			scp228ruj = null;
			RoundEnds = 100;
			ffPlayers.Clear();
			scpPlayer = null;
			pickupspawn = false;
			try
			{
				if (Tryhason()) return;
			}
			catch { }
			if (plugin.mtfvsci.GamemodeEnabled) return;
			Timing.CallDelayed(1f, () => selectspawnJG());

			players.Clear();
			coroutines.Add(Timing.RunCoroutine(CorrodeUpdate()));

		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			isRoundStarted = false;
			aopen = false;
			dedopen = false;
			checkopen = false;
			gateopen = false;
			scp228ruj = null;
			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnRoundRestart()
		{

			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPlayerDeath(DiedEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				Killscp228ruj();
			}
		}
		public void OnCheckRoundEnd(EndingRoundEventArgs ev)
		{
			List<Team> p2List = Player.List.Where(x => x.ReferenceHub.queryProcessor.PlayerId != scp228ruj?.queryProcessor.PlayerId).Select(x => Extensions.GetTeam(x.ReferenceHub)).ToList();

			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
			}
			if ((!p2List.Contains(Team.MTF) && p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && !p2List.Contains(Team.CHI) && !p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
			}
			if ((!p2List.Contains(Team.MTF) && !p2List.Contains(Team.SCP) && !p2List.Contains(Team.RSC) && scp228ruj != null) || (!p2List.Contains(Team.SCP) && p2List.Contains(Team.CHI) && p2List.Contains(Team.CDP) && scp228ruj != null))
			{
				ev.IsRoundEnded = true;
				ev.IsAllowed = true;
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
				ev.Amount = 0f;
			}
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
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
			Timing.CallDelayed(1f, () => RoundEnds++);
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
				if (ev.Pickup == vodka)
				{
					ev.IsAllowed = true;
					ds = false;
					checkopen = true;
					gateopen = true;
					scp228ruj.ClearBroadcasts();
					scp228ruj.Broadcast(Configs.svb, Configs.svbt);
					return;
				}
				else if (ev.Pickup.itemId  == ItemType.Coin || ev.Pickup == shop.shop25 || ev.Pickup == shop.shop24 || ev.Pickup == shop.shop23 || ev.Pickup == shop.shop22 || ev.Pickup == shop.shop21 || ev.Pickup == shop.shop20 ||
					ev.Pickup == shop.shop19 || ev.Pickup == shop.shop18 || ev.Pickup == shop.shop17 || ev.Pickup == shop.shop16 || ev.Pickup == shop.shop15 || ev.Pickup == shop.shop14 ||
					ev.Pickup == shop.shop13 || ev.Pickup == shop.shop12 || ev.Pickup == shop.shop11 || ev.Pickup == shop.shop10 || ev.Pickup == shop.shop9 || ev.Pickup == shop.shop8 ||
					ev.Pickup == shop.shop7 || ev.Pickup == shop.shop6 || ev.Pickup == shop.shop5 || ev.Pickup == shop.shop4 || ev.Pickup == shop.shop3 || ev.Pickup == shop.shop2 || ev.Pickup == shop.shop1)
				{
					return;
				}
				else
				{
					ev.Pickup.Delete();
					Extensions.SpawnItem(ev.Pickup.ItemId, 50, ev.Pickup.transform.position, ev.Pickup.rotation);
				}
				ev.IsAllowed = false;
			}
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId != scp228ruj?.queryProcessor.PlayerId)
			{
				if (ev.Pickup == vodka)
				{
					ev.IsAllowed = false;
					scp228ruj.ClearBroadcasts();
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.vppbt, Configs.vppb.Replace("%player%", $"{scp228ruj.nicknameSync.Network_myNickSync}"));
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
		public void RunOnDoorOpen(InteractingDoorEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scp228ruj?.queryProcessor.PlayerId)
			{
				if (gateopen)
				{
					if (ev.Door.DoorName.Contains("GATE"))
					{
						ev.IsAllowed = true;
					}
				}
				if (checkopen)
				{
					if (ev.Door.DoorName.Contains("CHECKPOINT"))
					{
						ev.IsAllowed = true;
					}
				}
				if (dedopen)
				{
					if (ev.Door.DoorName.Contains("106"))
					{
						ev.IsAllowed = true;
					}
				}
				if (aopen)
				{
					if (ev.Door.DoorName.Contains("096"))
					{
						ev.IsAllowed = true;
					}
				}
			}
		}
		private IEnumerator<float> CorrodeUpdate()
		{
			while (isRoundStarted)
			{
				if (scp228ruj != null)
				{
					IEnumerable<Player> pList = Player.List.Where(x => x.ReferenceHub.queryProcessor.PlayerId != scp228ruj.queryProcessor.PlayerId);
					if (!Configs.scpFriendlyFire) pList = pList.Where(x => Extensions.GetTeam(x.ReferenceHub) != Team.SCP);
					if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Extensions.GetTeam(x.ReferenceHub) != Team.TUT);
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
				yield return Timing.WaitForSeconds(1f);
			}
		}
		private static IEnumerator<float> Gopd()
		{
			for (; ; )
			{
				if (ds)
				{
					opd();
				}
				yield return Timing.WaitForSeconds(3f);
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
				string name = "";
				foreach (string str in ev.Arguments)
				{
					name += str;
					name += " ";
				}
				Player player = Player.Get(name);
				if (ev.Name == Configs.com)
				{
					ev.IsAllowed = false;
					if (player == null)
					{
						ev.ReplyMessage = Configs.nf;
						return;
					}
					ev.ReplyMessage = Configs.suc;
					SpawnJG(player.ReferenceHub);
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