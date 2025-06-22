using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using System.Linq;
namespace scp966
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static Player scp966;
		public void WaitingForPlayers() => plugin.cfg1();
		public void RoundStart()
		{
			scp966 = null;
			Timing.CallDelayed(0.7f, () => selectspawn());
		}
		public void Door(InteractingDoorEventArgs ev)
		{
			if (ev.Player.Id == scp966?.Id)
			{
				Timing.CallDelayed(0.5f, () => GameCore.Console.singleton.TypeCommand($"/effect {ev.Player.Id}. scp268=1"));
			}
		}
		public void Hurt(HurtingEventArgs ev)
		{
			if (ev.Attacker.Id == scp966?.Id)
			{
				GameCore.Console.singleton.TypeCommand($"/effect {ev.Attacker.Id}. scp268=0");
				Timing.CallDelayed(2f, () => GameCore.Console.singleton.TypeCommand($"/effect {ev.Attacker.Id}. scp268=1"));
			}
		}
		public void Leave(LeftEventArgs ev)
		{
			if (ev.Player.Id == scp966?.Id)
				Kill();
		}
		public void SetClass(ChangingRoleEventArgs ev)
		{
			if (ev.Player.Id == scp966?.Id && ev.NewRole != RoleType.Scp0492)
				Kill();
		}
		public void Died(DiedEventArgs ev)
		{
			if (ev.Target.Id == scp966?.Id)
				Kill();
		}
		public void Scp914(UpgradingItemsEventArgs ev)
        {
			try
			{
				if (ev.Players.Contains(Player.Get(scp966.ReferenceHub)))
				{
					ev.Players.Remove(Player.Get(scp966.ReferenceHub));
				}
			}
			catch { }
		}
		internal void Kill()
		{
			scp966.Inventory.Clear();
			scp966.ReferenceHub.serverRoles.NetworkMyText = "";
			scp966 = null;
			Cassie.Message(plugin.config.Cassie_dead);
		}
		public void Spawn(Player sss)
		{
			scp966 = sss;
			sss.SetRole(RoleType.Scp0492);
			Timing.CallDelayed(0.5f, () => sss.MaxHealth = plugin.config.Hp);
			Timing.CallDelayed(0.5f, () => sss.Health = plugin.config.Hp);
			Timing.CallDelayed(2.5f, () => sss.MaxHealth = plugin.config.Hp);
			Timing.CallDelayed(2.5f, () => sss.Health = plugin.config.Hp);
			Timing.CallDelayed(0.5f, () => sss.ReferenceHub.serverRoles.NetworkMyText = "SCP 966");
			Timing.CallDelayed(0.5f, () => sss.Position = Map.GetRandomSpawnPoint(RoleType.Scp049));
			Timing.CallDelayed(0.5f, () => sss.AddItem(ItemType.SCP268));
			Timing.CallDelayed(1f, () => GameCore.Console.singleton.TypeCommand($"/effect {sss.Id}. scp268=1"));
			sss.ClearBroadcasts();
			sss.Broadcast(plugin.config.Bc_time, plugin.config.Bc);
			Timing.RunCoroutine(scp268Cor(sss.ReferenceHub));
		}
		public IEnumerator<float> scp268Cor(ReferenceHub player)
		{
			while(Player.Get(player) != null && scp966 != null && Player.Get(player).Id == scp966.Id)
			{
				yield return Timing.WaitForSeconds(15f);
				GameCore.Console.singleton.TypeCommand($"/effect {Player.Get(player).Id}. scp268=1");
			}
		}
		public void selectspawn()
		{
			List<Player> pList = Player.List.Where(x => x.Team == Team.SCP && x.UserId != null && x.UserId != string.Empty).ToList();
			if (Player.List.ToList().Count > plugin.config.min_players_for_spawn && scp966 == null)
				Spawn(pList[UnityEngine.Random.Range(0, pList.Count)]);
		}
		public void Ra(SendingRemoteAdminCommandEventArgs ev)
		{
			string name = string.Join(" ", ev.Arguments.Skip(0));
			Player player = Player.Get(name);
			if (ev.Name == "scp966")
			{
				ev.IsAllowed = false;
				if (player == null)
				{
					ev.ReplyMessage = "хмм, грок не найден";
					return;
				}
				ev.ReplyMessage = "Успешно";
				Spawn(player);
			}
		}
	}
}