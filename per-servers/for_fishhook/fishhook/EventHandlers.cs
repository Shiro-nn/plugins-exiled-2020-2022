using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using UnityEngine;
using Grenades;
using MEC;
namespace fishhook
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static bool sb = false;
		public static bool sm = false;
		public static bool sd = false;
		public static bool fb = false;
		public static float forceShoot = 100.0f;
		public static float rangeShoot = 7.0f;
		private readonly int grenade_pickup_mask = 1049088;
		private static System.Random rand = new System.Random();
		public static ReferenceHub bomber;
		public static ReferenceHub med;
		private bool isRoundStarted;
		private bool o;
		private bool o2;
		public static ReferenceHub dr;
		public static ReferenceHub op;
		private static List<CoroutineHandle> coroutines = new List<CoroutineHandle>();
		private List<int> ffPlayers = new List<int>();
		public void OnPlayerDie(ref PlayerDeathEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == med?.queryProcessor.PlayerId)
			{
				med = null;
			}
			if (ev.Player.queryProcessor.PlayerId == bomber?.queryProcessor.PlayerId)
			{
				bomber = null;
			}
			if (ev.Player.queryProcessor.PlayerId == dr?.queryProcessor.PlayerId)
			{
				dr = null;
			}
		}
		public void OnTeamRespawn(ref TeamRespawnEvent ev)
		{
			sb = false; sm = false; sd = false;
			if (!ev.IsChaos)
			{
				foreach (ReferenceHub player in ev.ToRespawn)
				{
					if (!sb)
					{
						spawnbomber(player);
						sb = true;
						return;
					}
					if (!sm)
					{
						spawnmed(player);
						sm = true;
						return;
					}
					if (!sd)
					{
						sd = true;
						if (50 >= rand.Next(1, 101))
						{
							spawnd(player);
							return;
						}
					}
				}
			}
		}
		public void spawnd(ReferenceHub player)
		{
			if (dr == null)
			{
				player.ClearInventory();
				player.AddItem(ItemType.KeycardO5);
				player.AddItem(ItemType.GunLogicer);
				player.AddItem(ItemType.MicroHID);
				player.AddItem(ItemType.Radio);
				player.ClearBroadcasts();
				player.Broadcast(Configs.db, Configs.dbt);
				dr = player;
			}
		}
		public void spawnmed(ReferenceHub player)
		{
			if (med == null)
			{
				player.ClearInventory();
				player.AddItem(ItemType.KeycardNTFLieutenant);
				player.AddItem(ItemType.Radio);
				player.AddItem(ItemType.Medkit);
				player.ClearBroadcasts();
				player.Broadcast(Configs.mb, Configs.mbt);
				med = player;
			}
		}
		public void spawnbomber(ReferenceHub player)
		{
			if (bomber == null)
			{
				player.ClearInventory();
				player.AddItem(ItemType.KeycardNTFLieutenant);
				player.AddItem(ItemType.Radio);
				player.AddItem(ItemType.GrenadeFrag);
				player.ClearBroadcasts();
				player.Broadcast(Configs.bb, Configs.bbt);
				bomber = player;
			}
		}
		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == med?.queryProcessor.PlayerId)
			{
				if (!Configs.mp)
				{
					ev.Allow = false;
				}
			}
			if (ev.Player.queryProcessor.PlayerId == bomber?.queryProcessor.PlayerId)
			{
				if (!Configs.bp)
				{
					ev.Allow = false;
				}
			}
		}
		public void OnPlayerHurt(ref PlayerHurtEvent ev)
		{
			if (ffPlayers.Contains(ev.Attacker.queryProcessor.PlayerId))
			{
				GrantFF(ev.Attacker);
			}
			if (Player.GetTeam(ev.Attacker) == Team.SCP)
			{
				if (ev.Player.queryProcessor.PlayerId == dr?.queryProcessor.PlayerId)
				{
					ev.Amount = 20f;
				}
			}
			if (ev.Attacker.queryProcessor.PlayerId == op?.queryProcessor.PlayerId)
			{
				ev.Amount = Configs.dd;
			}
		}
		private void GrantFF(ReferenceHub player)
		{
			player.weaponManager.NetworkfriendlyFire = false;
			ffPlayers.Remove(player.queryProcessor.PlayerId);
		}
		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == med?.queryProcessor.PlayerId)
			{
				ev.Allow = true;
				Timing.CallDelayed(Configs.gmm, () => ev.Player.AddItem(ItemType.Medkit));
			}
		}
		private IEnumerator<float> healUpdate()
		{
			while (isRoundStarted)
			{
				if (med != null)
				{
					IEnumerable<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != med.queryProcessor.PlayerId);
					pList = pList.Where(x => Player.GetTeam(x) != Team.SCP);
					pList = pList.Where(x => Player.GetTeam(x) != Team.TUT);
					pList = pList.Where(x => Player.GetTeam(x) != Team.CHI);
					foreach (ReferenceHub player in pList)
					{
						if (player != null && Vector3.Distance(med.transform.position, player.transform.position) <= Configs.healDistance)
						{
							healPlayer(player);
						}
					}
				}
				yield return Timing.WaitForSeconds(Configs.healInterval);
			}
		}
		private void healPlayer(ReferenceHub player)
		{
			if (med != null)
			{
				int currHP = (int)player.playerStats.health;
				player.playerStats.health = currHP + Configs.healwm > Configs.health ? Configs.health : currHP + Configs.healwm;
			}
		}
		public void f(ref GrenadeThrownEvent ev)//press f
		{
			if (ev.Player.queryProcessor.PlayerId == bomber?.queryProcessor.PlayerId)
			{
				ev.Slow = true;
				Timing.CallDelayed(Configs.gbg, () => bomber.AddItem(ItemType.GrenadeFrag));
				if (5 >= rand.Next(1, 101))
					ev.Fuse = ev.Fuse * 50;
			}
		}
		public void Shoot(ref ShootEvent ev)
		{
			if (Physics.Linecast(ev.Shooter.GetPosition(), ev.TargetPos, out RaycastHit raycastHit, grenade_pickup_mask))
			{
				var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
				if (pickup != null && pickup.Rb != null)
				{
					pickup.Rb.AddExplosionForce(Vector3.Distance(ev.TargetPos, ev.Shooter.GetPosition()), ev.Shooter.GetPosition(), 500f, 3f, ForceMode.Impulse);
				}

				var grenade = raycastHit.transform.GetComponentInParent<FragGrenade>();
				if (grenade != null)
				{
					grenade.NetworkfuseTime = 0.1f;
				}
			}
			if (ev.Target == null || op == null) return;
			ReferenceHub target = Player.GetPlayer(ev.Target);
			if (target == null) return;

			if ((ev.Shooter.queryProcessor.PlayerId == op?.queryProcessor.PlayerId &&
				Player.GetTeam(target) == Player.GetTeam(op))
				|| (target.queryProcessor.PlayerId == op?.queryProcessor.PlayerId &&
				Player.GetTeam(ev.Shooter) == Player.GetTeam(op)))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}

			if ((ev.Shooter.queryProcessor.PlayerId == op?.queryProcessor.PlayerId || target.queryProcessor.PlayerId == op?.queryProcessor.PlayerId) &&
				(((Player.GetTeam(ev.Shooter) == Team.CDP && Player.GetTeam(target) == Team.CHI)
				|| (Player.GetTeam(ev.Shooter) == Team.CHI && Player.GetTeam(target) == Team.CDP)) ||
				((Player.GetTeam(ev.Shooter) == Team.RSC && Player.GetTeam(target) == Team.MTF)
				|| (Player.GetTeam(ev.Shooter) == Team.MTF && Player.GetTeam(target) == Team.RSC))))
			{
				ev.Shooter.weaponManager.NetworkfriendlyFire = true;
				ffPlayers.Add(ev.Shooter.queryProcessor.PlayerId);
			}
		}
		public void Shot(ref ShootEvent ev)
		{
			RaycastHit info;
			if (Physics.Linecast(ev.Shooter.GetComponent<Scp049PlayerScript>().plyCam.transform.position, ev.TargetPos, out info))
			{
				Collider[] arr = Physics.OverlapSphere(info.point, rangeShoot);
				foreach (Collider collider in arr)
				{
					if (collider.GetComponent<Pickup>() != null)
					{
						collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, info.point, rangeShoot);
					}
				}
			}
			else
			{
				Collider[] arr = Physics.OverlapSphere(ev.TargetPos, rangeShoot);
				foreach (Collider collider in arr)
				{
					if (collider.GetComponent<Pickup>() != null)
					{
						collider.GetComponent<Pickup>().Rb.AddExplosionForce(forceShoot, ev.TargetPos, rangeShoot);
					}
				}
			}
		}
		public void OnShoot(ref ShootEvent ev)
		{
			if (ev.TargetPos != Vector3.zero
				&& Physics.Linecast(ev.Shooter.GetPosition(), ev.TargetPos, out RaycastHit raycastHit, grenade_pickup_mask))
			{
				var pickup = raycastHit.transform.GetComponentInParent<Pickup>();
				if (pickup != null && pickup.Rb != null)
				{
					pickup.Rb.AddExplosionForce(Vector3.Distance(ev.TargetPos, ev.Shooter.GetPosition()), ev.Shooter.GetPosition(), 500f, 3f, ForceMode.Impulse);
				}

				var grenade = raycastHit.transform.GetComponentInParent<FragGrenade>();
				if (grenade != null)
				{
					grenade.NetworkfuseTime = 0.1f;
				}
			}
		}
		public void RoundStart()
		{
			fb = false;
			isRoundStarted = true;
			o = true;
			o2 = true;
			coroutines.Add(Timing.RunCoroutine(healUpdate()));
			bomber = null;
			med = null;
			dr = null;
			ffPlayers.Clear();
			Timing.CallDelayed(rand.Next(Configs.mins, Configs.maxs), () =>
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass != RoleType.Spectator && x.GetTeam() != Team.SCP && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				if (pList.Count >= Configs.mps)
					spawn(pList[rand.Next(pList.Count)]);
			});
		}
		public void OnDropItem(ref DropItemEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == op?.queryProcessor.PlayerId)
			{
				if (ev.Item.id == ItemType.Flashlight)
				{
					o2 = false;
					IEnumerable<ReferenceHub> pList = Player.GetHubs().Where(x => x.queryProcessor.PlayerId != op.queryProcessor.PlayerId);
					foreach (ReferenceHub playe in pList) 
					{
						if (playe.queryProcessor.PlayerId != op.queryProcessor.PlayerId)
						{
							coroutines.Add(Timing.RunCoroutine(stopgo(playe.transform.position, playe)));
						}
					}
					Timing.CallDelayed(Configs.gp, () =>
					{
						fb = true;
					});
				}
			}
		}
		public void spawn(ReferenceHub player)
		{
			if (o)
			{
				player.ClearInventory();
				player.AddItem(ItemType.Flashlight);
				player.AddItem(ItemType.MicroHID);
				player.AddItem(ItemType.GunLogicer);
				player.AddItem(ItemType.KeycardO5);
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.ob, Configs.obt);
				player.playerStats.maxHP = 500;
				player.playerStats.health = 500;
				op = player;
				o = false;
			}
		}
		private IEnumerator<float> stopgo(Vector3 p, ReferenceHub pl)
		{
			if (Configs.tpplayer)
			{
				while (isRoundStarted)
				{
					if (!o2)
					{
						if (!fb)
						{
							if (pl != null)
							{
								tpp(p, pl);
							}
						}
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public void tpp(Vector3 p, ReferenceHub pl)
		{
			pl.plyMovementSync.OverridePosition(p, 0f, false);
		}
		public void RoundEnd()
		{
			isRoundStarted = false;
			o = true;
		}
		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}
	}
}

