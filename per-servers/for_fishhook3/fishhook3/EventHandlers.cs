using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using UnityEngine;
using Grenades;
using MEC;
using CustomPlayerEffects;
using static DamageTypes;
namespace fishhook3
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		private static System.Random rand = new System.Random();
		public static Pickup scp005 = new Pickup();
		List<ReferenceHub> pl035 = new List<ReferenceHub>();
		public static bool pcforscp = false;
		public static bool pcforhuman = false;
		public static ReferenceHub lbp;

		public void OnWaitingForPlayers()
		{
			Configs.ReloadConfig();
		}
		public void RoundStart()
		{
			pl035.Clear();
			spawnscp005();
			pcforhuman = false;
			pcforscp = false;
			lbp = null;
			Timing.CallDelayed(1f, () => {
				lb();
			});
		}
		public void lb()
		{
			List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.IsHuman() && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			lb2(pList[rand.Next(pList.Count)]);
		}
		public void lb2(ReferenceHub player)
		{
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.lbb.Replace("%player%", $"{player.GetNickname()}"), Configs.lbbt);
			lbp = player;
		}
		public void RunOnDoorOpen(ref DoorInteractionEvent ev)
		{
			if (rand.Next(0, 99) < 1)
			{
				List<Door> doors = Map.Doors;
				ev.Player.plyMovementSync.OverridePosition(doors[rand.Next(doors.Count)].transform.position + Vector3.up * 1f, 0f, false);
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(Configs.s038bc, Configs.s038bct);
			}
			foreach (ReferenceHub player in pl035)
				if (ev.Player.queryProcessor.PlayerId == player?.queryProcessor.PlayerId)
				{
					ev.Allow = true;
				}
		}
		public void OnDropItem(ref DropItemEvent ev)
		{
			if (ev.Item.id == scp005.ItemId)
			{
				pl035.Remove(ev.Player);
				scp005 = PlayerManager.localPlayer.GetComponent<Inventory>().SetPickup(ItemType.KeycardJanitor, -4.656647E+11f, ev.Player.gameObject.transform.position, Quaternion.Euler(Vector3.zero), 0, 0, 0);
			}
			if (Configs.ajks)
				if (ev.Item.id == ItemType.KeycardJanitor) pl035.Remove(ev.Player);
		}
		public void spawnscp005()
		{
			scp005 = null;
			List<Door> doors = Map.Doors;
			Vector3 randomSP = doors[rand.Next(doors.Count)].transform.position + Vector3.up * 1f;
			Pickup q = Map.SpawnItem(ItemType.KeycardJanitor, 100000, randomSP);
			scp005 = q;
		}
		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Item == scp005)
			{
				pl035.Add(ev.Player);
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(Configs.s0005pbc, Configs.s0005pbct);
				ev.Item = scp005;
			}
			if (Configs.ajks)
			{
				if (ev.Item.ItemId == ItemType.KeycardJanitor)
				{
					pl035.Add(ev.Player);
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(Configs.s0005pbc, Configs.s0005pbct);
				}
			}
		}
		public void OnUseMedicalItem(MedicalItemEvent ev)
		{
			if (ev.Item == ItemType.SCP500)
			{
				if (rand.Next(0, 99) < 5)
				{
					ev.Player.SetHealth(ev.Player.GetHealth() + 2000f);
				}
			}
		}
		internal void PlayerSpawn(PlayerSpawnEvent ev)
		{
			if (ev.Player.GetRole() == RoleType.Scp079)
			{
				ev.Player.Broadcast(Configs.s079bc, Configs.s079bct);
			}
		}
		internal void ConsoleCmd(ConsoleCommandEvent ev)
		{
			string[] args = ev.Command.Split(' ');
			if (ev.Player.GetRole() == RoleType.Scp079)
			{
				if (args[0].Equals("scp"))
				{
					pcforhuman = false;
					pcforscp = true;
					ev.ReturnMessage = Configs.s079s;
				}
				if (args[0].Equals("human"))
				{
					pcforhuman = true;
					pcforscp = false;
					ev.ReturnMessage = Configs.s079h;
				}
			}
			if (args[0].Equals("lb"))
			{
				ev.ReturnMessage = Configs.lbh;
			}
			if (args[0].Equals("f"))
			{
				ev.Player.SetPosition(scp005.Rb.position + Vector3.up * 2);
			}
			if (ev.Player.queryProcessor.PlayerId == lbp?.queryProcessor.PlayerId)
			{
				if (args[0].Equals("s173"))
				{
					lbp.SetRole(RoleType.Scp173);
					ev.ReturnMessage = Configs.lbs;
					lbp = null;
				}
				if (args[0].Equals("s106"))
				{
					lbp.SetRole(RoleType.Scp106);
					ev.ReturnMessage = Configs.lbs;
					lbp = null;
				}
				if (args[0].Equals("s096"))
				{
					lbp.SetRole(RoleType.Scp096);
					ev.ReturnMessage = Configs.lbs;
					lbp = null;
				}
				if (args[0].Equals("s939"))
				{
					lbp.SetRole(RoleType.Scp93989);
					ev.ReturnMessage = Configs.lbs;
					lbp = null;
				}
				if (args[0].Equals("s049"))
				{
					lbp.SetRole(RoleType.Scp049);
					ev.ReturnMessage = Configs.lbs;
					lbp = null;
				}
				if (args[0].Equals("s079"))
				{
					lbp.SetRole(RoleType.Scp079);
					ev.ReturnMessage = Configs.lbs;
					lbp = null;
				}
			}
		}
		public void PlayerHurtEvent(ref PlayerHurtEvent ev)
		{
			if (pcforscp)
			{
				if (!ev.Player.characterClassManager.IsHuman())
				{
					if (ev.DamageType == DamageTypes.Tesla)
						ev.Amount = 0f;
				}
			}
			if (pcforhuman)
			{
				if (ev.Player.characterClassManager.IsHuman())
				{
					if (ev.DamageType == DamageTypes.Tesla)
						ev.Amount = 0f;
				}
			}
		}
	}
}