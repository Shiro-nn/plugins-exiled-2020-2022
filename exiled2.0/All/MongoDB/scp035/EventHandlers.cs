using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MongoDB.scp035
{
    public partial class EventHandlers
	{
		public static Main035 Main035;
		public EventHandlers(Main035 plugin) => Main035 = plugin;

		internal static ReferenceHub scpPlayer;
		internal bool isRoundStarted;
		internal static bool isRotating;
		private static int maxHP;
		internal const float dur = 327;
		public void OnRoundStart()
		{
			RefreshItems();
			isRoundStarted = true;
			isRotating = true;
			scpPlayer = null;
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			isRoundStarted = false;
		}

		public void OnRoundRestart()
		{
			isRoundStarted = false;
		}

		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (ev.IsAllowed && scpPlayer == null && ev.Pickup.durability == dur && ev.Pickup != TryGetvodka() && ev.Pickup != Main035.plugin.armor.jugarmor && ev.Player.ReferenceHub.GetTeam() != Team.SCP)
			{
				ev.IsAllowed = false;
				InfectPlayer(ev.Player.ReferenceHub, ev.Pickup);
			}
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			RemoveFF(ev.Attacker);
			if (scpPlayer != null)
			{
				if ((ev.Attacker.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					ev.Target.Team == Team.SCP) ||
					(ev.Target.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					ev.Attacker.Team == Team.SCP))
				{
					ev.IsAllowed = false;
					ev.Amount = 0f;
				}

				if (ev.Attacker.ReferenceHub.queryProcessor.PlayerId != ev.Target.ReferenceHub.queryProcessor.PlayerId &&
					((ev.Attacker.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					ev.Target.Team == Team.TUT) ||
					(ev.Target.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId &&
					ev.Attacker.Team == Team.TUT)))
				{
					ev.IsAllowed = false;
					ev.Amount = 0f;
				}
			}
		}

		public void OnShoot(ShootingEventArgs ev)
		{
			if (ev.Target == null || scpPlayer == null) return;
			Player target = Player.Get(ev.Target);
			if (target == null) return;
			if (target.Id == scpPlayer?.queryProcessor.PlayerId || ev.Shooter.Id == scpPlayer?.queryProcessor.PlayerId)
				GrantFF(ev.Shooter);
		}

		public void OnPlayerDie(DiedEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				KillScp035();
			}

			if (ev.Killer.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				if (ev.Target.Team == Team.SCP) return;
				if (ev.Target.Role == RoleType.Spectator) return;
				ReferenceHub spy = scpPlayer;
				{
					Inventory.SyncListItemInfo items = new Inventory.SyncListItemInfo();
					foreach (var item in spy.inventory.items) items.Add(item);
					Vector3 pos1 = ev.Target.ReferenceHub.transform.position;
					Quaternion rot = spy.transform.rotation;
					int health = (int)spy.playerStats.Health;

					spy.SetRole(ev.Target.Role);

					Timing.CallDelayed(0.5f, () =>
					{
						spy.playerMovementSync.OverridePosition(pos1, 0f);
						spy.SetRotation(rot.x, rot.y);
						spy.inventory.items.Clear();
						foreach (var item in items) spy.inventory.AddNewItem(item.id);
						spy.playerStats.maxHP = maxHP;
						spy.playerStats.Health = health;
						spy.ammoBox.ResetAmmo();
					});
				}
			}
		}

		public void OnPlayerDied(DiedEventArgs ev)
		{
			if (ev.Killer.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
				{
					if (doll.owner.PlayerId == ev.Target.ReferenceHub.queryProcessor.PlayerId)
					{
						NetworkServer.Destroy(doll.gameObject);
					}
				}
			}
		}

		public void scpzeroninesixe(EnragingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void scpzeroninesixeadd(AddingTargetEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnFemurBreaker(EnteringFemurBreakerEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnCheckEscape(EscapingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				if (ev.NewRole == RoleType.Spectator)
				{
					KillScp035();
                }
                else
				{
					scpPlayer.playerStats.maxHP = maxHP;
				}
			}
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				KillScp035(false);
			}
		}

		public void OnContain106(ContainingEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnInsertTablet(InsertingGeneratorTabletEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
			}
		}

		public void OnEjectTablet(EjectingGeneratorTabletEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (ev.Player.ReferenceHub.queryProcessor.PlayerId == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				Extensions.TeleportTo106(ev.Player.ReferenceHub);
			}
		}

		public void RunOnRACommandSent(SendingRemoteAdminCommandEventArgs ev)
		{
			string name = string.Join(" ", ev.Arguments.Skip(0));
			ReferenceHub player = Extensions.GetPlayer(name);
			if (ev.Name == "scp035")
			{
				ev.IsAllowed = false;
				if (ev.CommandSender.SenderId == "-@steam"
					|| ev.CommandSender.SenderId == "SERVER CONSOLE")
				{
					if (player == null)
					{
						ev.ReplyMessage = "Игрок не найден!";
						return;
					}
					ev.ReplyMessage = "Успешно!";
					Spawn035(player);
				}
				else
				{
					ev.ReplyMessage = "Отказано в доступе";
				}
			}
		}
		private void GrantFF(Player player)
		{
			player.IsFriendlyFireEnabled = true;
		}

		private void RemoveFF(Player player)
		{
			player.IsFriendlyFireEnabled = false;
		}
	}
}
