using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace scp035
{
	public partial class EventHandlers
	{
		private readonly Plugin plugin;
		private static Plugin splugin;
		public EventHandlers(Plugin plugin)
        {
			this.plugin = plugin;
			splugin = plugin;
		}
		private static Dictionary<Pickup, float> scpPickups = new Dictionary<Pickup, float>();
		internal static ReferenceHub scpPlayer;
		internal static bool isRotating;
		private static int maxHP;
		private const float dur = 327;
		public void WFP() => plugin.cfg1();
		public void OnRoundStart()
		{
			RefreshItems();
			isRotating = true;
			scpPlayer = null;
		}
		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (ev.IsAllowed && scpPlayer == null && ev.Pickup.durability == dur && ev.Player.Role != RoleType.Tutorial)
			{
				ev.IsAllowed = false;
				InfectPlayer(ev.Player.ReferenceHub, ev.Pickup);
			}
		}
		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			bool _ = false;
			bool __ = false;
			try { _ = serpentshand.EventHandlers.shPlayers.Contains(ev.Attacker.Id); } catch { }
			try { __ = serpentshand.EventHandlers.shPlayers.Contains(ev.Target.Id); } catch { }
			RemoveFF(ev.Attacker);
			if (scpPlayer != null)
			{
				if ((ev.Attacker.Id == scpPlayer?.queryProcessor.PlayerId &&
					(ev.Target.Team == Team.SCP || __)) ||
					(ev.Target.Id == scpPlayer?.queryProcessor.PlayerId &&
					(ev.Attacker.Team == Team.SCP || _)))
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
			if (ev.Target.Id == scpPlayer?.queryProcessor.PlayerId)
				KillScp035();
		}
		public void scpzeroninesixe(EnragingEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void scpzeroninesixeadd(AddingTargetEventArgs ev)
		{
			if (ev.Target.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnPocketDimensionEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnFemurBreaker(EnteringFemurBreakerEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnCheckEscape(EscapingEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId && ev.NewRole == RoleType.Spectator) KillScp035();
		}
		public void OnPlayerLeave(LeftEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) KillScp035(false);
		}
		public void OnContain106(ContainingEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (ev.Target.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnInsertTablet(InsertingGeneratorTabletEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnEjectTablet(EjectingGeneratorTabletEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId) ev.IsAllowed = false;
		}
		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (ev.Player.Id == scpPlayer?.queryProcessor.PlayerId)
			{
				ev.IsAllowed = false;
				Extensions.TeleportTo106(ev.Player);
			}
		}
		public void RunOnRACommandSent(SendingRemoteAdminCommandEventArgs ev)
		{
			string name = string.Join(" ", ev.Arguments.Skip(0));
			Player player = Player.Get(name);
			if (ev.Name == "scp035")
			{
				ev.IsAllowed = false;
				if (player == null)
				{
					ev.ReplyMessage = "хмм, грок не найден";
					return;
				}
				ev.ReplyMessage = "Успешно";
				Spawn035(player.ReferenceHub);
			}
		}
		private void GrantFF(Player player) => player.IsFriendlyFireEnabled = true;
		private void RemoveFF(Player player) => player.IsFriendlyFireEnabled = false;
	}
}