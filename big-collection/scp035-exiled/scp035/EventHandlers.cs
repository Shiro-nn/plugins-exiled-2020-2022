using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;
using System;
using scp228ruj.API;
namespace scp035
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		private static Dictionary<Pickup, float> scpPickups = new Dictionary<Pickup, float>();
		private List<int> ffPlayers = new List<int>();
		internal static ReferenceHub scpPlayer;
		private static bool isHidden;
		private static bool hasTag;
		private bool isRoundStarted;
		private static bool isRotating;
		private static int maxHP;
		private static int HP;
		private static float HPF;
		private const float dur = 327;
		private static System.Random rand = new System.Random();

		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}

		public void OnRoundStart()
		{
			isRoundStarted = true;
			isRotating = true;
			scpPickups.Clear();
			ffPlayers.Clear();
			scpPlayer = null;

			coroutines.Add(Timing.CallDelayed(1f, () => Timing.RunCoroutine(RotatePickup())));
			coroutines.Add(Timing.RunCoroutine(CorrodeUpdate()));
		}

		public void OnRoundEnd()
		{
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnRoundRestart()
		{
			// In case the round is force restarted
			isRoundStarted = false;

			Timing.KillCoroutines(coroutines);
			coroutines.Clear();
		}

		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Item.info.durability == dur)
			{
				if (ev.Item != TryGetvodka())
				{
					ev.Allow = false;
					InfectPlayer(ev.Player, ev.Item);
				}
			}
		}

		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ffPlayers.Contains(ev.Attacker.queryProcessor.PlayerId))
			{
				GrantFF(ev.Attacker);
			}

			if (scpPlayer != null)
			{
				if (!Configs.scpFriendlyFire &&
					((ev.Attacker.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Player) == Team.SCP) ||
					(ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Attacker) == Team.SCP)))
				{
					ev.Amount = 0f;
				}

				if (!Configs.tutorialFriendlyFire &&
					ev.Attacker.queryProcessor.PlayerId != ev.Player.queryProcessor.PlayerId &&
					((ev.Attacker.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Player) == Team.TUT) ||
					(ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					Player.GetTeam(ev.Attacker) == Team.TUT)))
				{
					ev.Amount = 0f;
				}
			}
		}

		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.Target == null || scpPlayer == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;

			if ((ev.Shooter.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
				Player.GetTeam(target) == Player.GetTeam(scpPlayer))
				|| (target.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
				Player.GetTeam(ev.Shooter) == Player.GetTeam(scpPlayer)))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}

			if ((ev.Shooter.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId || target.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId) &&
				(((Player.GetTeam(ev.Shooter) == Team.CDP && Player.GetTeam(target) == Team.CHI)
				|| (Player.GetTeam(ev.Shooter) == Team.CHI && Player.GetTeam(target) == Team.CDP)) || 
				((Player.GetTeam(ev.Shooter) == Team.RSC && Player.GetTeam(target) == Team.MTF)
				|| (Player.GetTeam(ev.Shooter) == Team.MTF && Player.GetTeam(target) == Team.RSC))))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}
		}

		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				KillScp035();
			}

			if (ev.Killer.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				if (ev.Player.GetTeam() == Team.SCP) return;
				if (ev.Player.GetRole() == RoleType.Spectator) return;
				ReferenceHub spy = scpPlayer;
				{
					Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
					foreach (var item in spy.inventory.items) items.Add(item);
					Vector3 pos1 = ev.Player.transform.position;
					Quaternion rot = spy.transform.rotation;
					int health = (int)spy.playerStats.health;
					string ammo = spy.ammoBox.Networkamount;

					spy.SetRole(ev.Player.characterClassManager.CurClass);

					Timing.CallDelayed(0.3f, () =>
					{
						spy.plyMovementSync.OverridePosition(pos1, 0f);
						spy.SetRotation(rot.x, rot.y);
						spy.inventory.items.Clear();
						foreach (var item in items) spy.inventory.AddNewItem(item.id);
						spy.playerStats.health = health;
						spy.ammoBox.Networkamount = ammo;
						spy.SetRank("SCP 035", "red");
					});
				}
			}
		}
		public void scpzeroninesixe(ref Scp096EnrageEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
		{
			List<Team> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scpPlayer?.queryProcessor.PlayerId).Select(x => Player.GetTeam(x)).ToList();

			if ((!pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && ((pList.Contains(Team.SCP) && scpPlayer != null) || !pList.Contains(Team.SCP) && scpPlayer != null)) ||
				(Configs.winWithTutorial && !pList.Contains(Team.CHI) && !pList.Contains(Team.CDP) && !pList.Contains(Team.MTF) && !pList.Contains(Team.RSC) && pList.Contains(Team.TUT) && scpPlayer != null))
			{
				ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
				ev.ForceEnd = true;
			}

			// If 035 is the only scp alive keep the round going
			else if (scpPlayer != null && !pList.Contains(Team.SCP) && (pList.Contains(Team.CDP) || pList.Contains(Team.CHI) || pList.Contains(Team.MTF) || pList.Contains(Team.RSC)))
			{
				ev.Allow = false;
			}
		}

		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId) ev.Allow = false;
		}

		public void OnSetClass(SetClassEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				if(ev.Role == RoleType.Spectator)
				{
					KillScp035();
				}
			}
		}

		public void OnPlayerLeave(PlayerLeaveEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				KillScp035(false);
			}
		}

		public void OnContain106(Scp106ContainEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnInsertTablet(ref GeneratorInsertTabletEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && !Configs.scpFriendlyFire)
			{
				ev.Allow = false;
			}
		}

		public void OnPocketDimensionDie(PocketDimDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.Allow = false;
				ev.Player.plyMovementSync.OverridePosition(GameObject.FindObjectOfType<SpawnpointManager>().GetRandomPosition(RoleType.Scp096).transform.position, 0);
			}
		}

		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (!Configs.canUseMedicalItems && ev.Player.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId && (ev.Item == ItemType.Adrenaline || ev.Item == ItemType.Painkillers || ev.Item == ItemType.Medkit || ev.Item == ItemType.SCP500))
			{
				ev.Allow = false;
			}
		}

		private static void RemovePossessedItems()
		{
			for (int i = 0; i < scpPickups.Count; i++)
			{
				Pickup p = scpPickups.ElementAt(i).Key;
				if (p != null) p.Delete();
			}
			scpPickups.Clear();
		}

		private static Pickup GetRandomItem()
		{
			List<Pickup> pickups = GameObject.FindObjectsOfType<Pickup>().Where(x => !scpPickups.ContainsKey(x)).ToList();
			return pickups[rand.Next(pickups.Count)];
		}
		private static Pickup TryGetvodka()
		{
			return Scp228Data.Getvodka();
		}
		private static void RefreshItems()
		{
			if (Player.GetHubs().Where(x => Player.GetTeam(x) == Team.RIP && !x.serverRoles.OverwatchEnabled).ToList().Count > 0)
			{
				RemovePossessedItems();
				for (int i = 0; i < Configs.infectedItemCount; i++)
				{
					Pickup p = GetRandomItem();
					Pickup a = PlayerManager.localPlayer
						.GetComponent<Inventory>().SetPickup((ItemType)Configs.possibleItems[rand.Next(Configs.possibleItems.Count)],
						-4.656647E+11f,
						p.transform.position,
						p.transform.rotation,
						0, 0, 0).GetComponent<Pickup>();
					scpPickups.Add(a, a.info.durability);
					a.info.durability = dur;
				}
			}
		}

		private static void KillScp035(bool setRank = true)
		{
			if (setRank)
			{
				scpPlayer.SetRank("", "default");
				if (hasTag) scpPlayer.RefreshTag();
				if (isHidden) scpPlayer.HideTag();
			}
			scpPlayer.playerStats.maxHP = maxHP;
			scpPlayer = null;
			isRotating = true;
			RefreshItems();
		}

		public static void Spawn035(ReferenceHub p035, ReferenceHub player = null, bool full = true)
		{
			if (full)
			{
				if (p035 != null)
				{
					Vector3 pos = player.transform.position;
					Timing.CallDelayed(0.2f, () => p035.plyMovementSync.OverridePosition(pos, 0));
				}
				maxHP = player.playerStats.maxHP;
				p035.playerStats.maxHP = Configs.health;
				p035.playerStats.health = Configs.health;
				p035.ammoBox.Networkamount = "999:999:999";
			}

			hasTag = !string.IsNullOrEmpty(p035.serverRoles.NetworkMyText);
			isHidden = !string.IsNullOrEmpty(p035.serverRoles.HiddenBadge);
			if (isHidden) p035.RefreshTag();
			p035.SetRank("SCP 035", "red");
			p035.Broadcast($"<size=60>Вы-<color=\"red\"><b>SCP-035</b></color></size>{(full ? "\nВы заразили тело и получили контроль над ним, используйте его, чтобы помочь другим SCP!" : string.Empty)}", 10);
			Cassie.CassieMessage("ATTENTION TO ALL PERSONNEL . SCP 0 3 5 ESCAPE . ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B . REPEAT ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO GATE B", false, false);
			Cassie.DelayedCassieMessage("SCP 0 3 5 ESCAPE", false, false, 3);
			scpPlayer = p035;
		}
		public static void InfectPlayer(ReferenceHub player, Pickup pItem)
		{

			pItem.Delete();


			isRotating = false;

			Timing.CallDelayed(3f, () => Spawn035(player, player));

			RemovePossessedItems();

			if (Configs.corrodeHost)
			{
				coroutines.Add(Timing.RunCoroutine(CorrodeHost()));
			}
		}

		private static IEnumerator<float> CorrodeHost()
		{
			while (scpPlayer != null)
			{
				scpPlayer.playerStats.health -= Configs.corrodeHostAmount;
				if (scpPlayer.playerStats.health <= 0)
				{
					scpPlayer.ChangeRole(RoleType.Spectator);
					KillScp035();
				}
				yield return Timing.WaitForSeconds(Configs.corrodeHostInterval);
			}
		}

		private IEnumerator<float> RotatePickup()
		{
			while (isRoundStarted)
			{
				if (isRotating)
				{
					RefreshItems();
				}
				yield return Timing.WaitForSeconds(Configs.rotateInterval);
			}
		}

		private IEnumerator<float> CorrodeUpdate()
		{
			if (Configs.corrodePlayers)
			{
				while (isRoundStarted)
				{
					if (scpPlayer != null)
					{
						IEnumerable<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != scpPlayer.queryProcessor.PlayerId);
						if (!Configs.scpFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.SCP);
						if (!Configs.tutorialFriendlyFire) pList = pList.Where(x => Player.GetTeam(x) != Team.TUT);
						foreach (ReferenceHub player in pList)
						{
							if (player != null && Vector3.Distance(scpPlayer.transform.position, player.transform.position) <= Configs.corrodeDistance)
							{
								CorrodePlayer(player);
							}
						}
					}
					yield return Timing.WaitForSeconds(Configs.corrodeInterval);
				}
			}
		}

		public static IEnumerator<float> DelayAction(float delay, Action x)
		{
			yield return Timing.WaitForSeconds(delay);
			x();
		}

		private void CorrodePlayer(ReferenceHub player)
		{
			if (Configs.corrodeLifeSteal && scpPlayer != null)
			{
				int currHP = (int)scpPlayer.playerStats.health;
				scpPlayer.playerStats.health = currHP + Configs.corrodeDamage > Configs.health ? Configs.health : currHP + Configs.corrodeDamage;
			}
			player.Damage(Configs.corrodeDamage, DamageTypes.Nuke);
		}

		private void GrantFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = false;
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}
		public void RunOnRACommandSent(ref RACommandEvent RACom)
		{
			string[] command = RACom.Command.Split(' ');
			ReferenceHub sender = RACom.Sender.SenderId == "SERVER CONSOLE" || RACom.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(RACom.Sender.SenderId);
			ReferenceHub player = Plugin.GetPlayer(command[1]);
			if (command[0] == "scp035")
			{
				RACom.Allow = false;
				if (player == null)
				{
					RACom.Sender.RAMessage("Игрок не найден!");
					return;
				}
				RACom.Sender.RAMessage("Успешно!");
				Spawn035(player, player);
			}
		}
	}
}
