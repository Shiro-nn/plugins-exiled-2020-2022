using Loli.Addons;
using MEC;
using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using Respawning;
using System.Collections.Generic;
using UnityEngine;
namespace Loli.Spawns
{
	public class SpawnManager
	{
		internal static SpawnManager Instance;
		internal readonly ChaosInsurgency ChaosInsurgency;
		internal readonly MobileTaskForces MobileTaskForces;
		internal readonly SerpentsHand SerpentsHand;
		internal static string SpawnProtectTag => " SpawnProtect";
		public SpawnManager()
		{
			ChaosInsurgency = new ChaosInsurgency(this);
			MobileTaskForces = new MobileTaskForces(this);
			SerpentsHand = new SerpentsHand(this);
			Instance = this;
			CommandsSystem.RegisterRemoteAdmin("server_event", ServerRaEvents);
			CommandsSystem.RegisterRemoteAdmin("ci", RaCi);
			CommandsSystem.RegisterRemoteAdmin("mtf", RaMtf);
			CommandsSystem.RegisterRemoteAdmin("sh", RaSh);
			CommandsSystem.RegisterRemoteAdmin("shone", RaShOne);
		}
		private void ServerRaEvents(SendingRAEvent ev)
		{
			if (!ThisAccess(ev.Player.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			if (ev.Args[0].ToLower() == "force_mtf_respawn")
			{
				ev.Allowed = false;
				ev.Success = true;
				ev.ReplyMessage = "Успешно";
				MobileTaskForces.SpawnMtf();
			}
			else if (ev.Args[0].ToLower() == "force_ci_respawn")
			{
				ev.Allowed = false;
				ev.Success = true;
				ev.ReplyMessage = "Успешно";
				ChaosInsurgency.SpawnCI();
			}
		}
		private void RaCi(SendingRAEvent ev)
		{
			if (!ThisOwner(ev.Player.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.ReplyMessage = "Успешно";
			Addons.RealSpawn.Chaos.OpenGate(null);
		}
		private void RaMtf(SendingRAEvent ev)
		{
			if (!ThisOwner(ev.Player.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.ReplyMessage = "Успешно";
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
		}
		private void RaSh(SendingRAEvent ev)
		{
			if (!ThisOwner(ev.Player.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.ReplyMessage = "Успешно";
			SerpentsHand.Spawn();
		}
		private void RaShOne(SendingRAEvent ev)
		{
			if (!ThisOwner(ev.Player.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			try
			{
				string name = string.Join(" ", ev.Args);
				Player player = Player.Get(name);
				if (ev.CommandSender.SenderId == "-@steam"
					|| ev.CommandSender.SenderId == "SERVER CONSOLE")
				{
					if (player == null) ev.ReplyMessage = "Игрок не найден";
					else
					{
						ev.ReplyMessage = "Успешно";
						SerpentsHand.SpawnOne(player);
					}
				}
				else ev.ReplyMessage = "Отказано в доступе";
			}
			catch
			{
				ev.ReplyMessage = "Произошла ошибка";
			}
		}
		private bool ThisAccess(string userid)
		{
			try
			{
				if (DataBase.Manager.Static.Data.Users.TryGetValue(userid, out var data) &&
					(DataBase.Modules.CustomDonates.ThisYt(data) ||
					data.trainee || data.helper || data.mainhelper || data.admin || data.mainadmin || data.owner)) return true;
				else return false;
			}
			catch { return false; }
		}
		private bool ThisOwner(string userid)
		{
			try
			{
				if (DataBase.Manager.Static.Data.Users.TryGetValue(userid, out var main) && main.id == 1) { return true; }
				else return false;
			}
			catch { return false; }
		}
		public void Spawn()
		{
			if (Round.Started)
			{
				int random = Extensions.Random.Next(0, 100);
				if (40 >= random) ChaosInsurgency.SpawnCI();
				else if (80 >= random) MobileTaskForces.SpawnMtf();
				else SerpentsHand.Spawn();
			}
		}
		internal static void SpawnProtect(Player pl)
		{
			pl.Tag = pl.Tag.Replace(SpawnProtectTag, "") + SpawnProtectTag;
			Timing.CallDelayed(5f, () => pl.Tag = pl.Tag.Replace(SpawnProtectTag, ""));
		}
		internal void SpawnProtect(DamageEvent ev)
		{
			if (ev.Target.Tag.Contains(SpawnProtectTag) && ev.DamageType is not DamageTypes.Nuke)
			{
				ev.Allowed = false;
				ev.Amount = 0f;
			}
		}
		internal void SpawnProtect(AddTargetEvent ev)
		{
			if (ev.Target.Tag.Contains(SpawnProtectTag))
			{
				ev.Allowed = false;
			}
		}
		internal void SpawnProtect(PocketEnterEvent ev)
		{
			if (ev.Player.Tag.Contains(SpawnProtectTag))
			{
				ev.Allowed = false;
			}
		}
		internal void SpawnCor() => Timing.RunCoroutine(SpawnTeam(), "SpawnManager_SpawnTeam");
		internal void SpawnCor(RoundEndEvent _) => Timing.KillCoroutines("SpawnManager_SpawnTeam");
		internal IEnumerator<float> SpawnTeam()
		{
			for (; ; )
			{
				int random = Random.Range(120, 180);
				if (Plugin.Anarchy) random = Random.Range(45, 75);
				yield return Timing.WaitForSeconds(random);
				Spawn();
			}
		}
	}
}