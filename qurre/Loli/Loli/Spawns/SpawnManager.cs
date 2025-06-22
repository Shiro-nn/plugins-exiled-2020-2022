using Loli.Addons;
using Loli.DataBase.Modules;
using MEC;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using Respawning;
using System.Collections.Generic;
using UnityEngine;

namespace Loli.Spawns
{
	static class SpawnManager
	{
		internal static string SpawnProtectTag => " SpawnProtect";
		static SpawnManager()
		{
			CommandsSystem.RegisterRemoteAdmin("server_event", ServerRaEvents);
			CommandsSystem.RegisterRemoteAdmin("ci", RaCi);
			CommandsSystem.RegisterRemoteAdmin("mtf", RaMtf);
			CommandsSystem.RegisterRemoteAdmin("sh", RaSh);
			CommandsSystem.RegisterRemoteAdmin("shone", RaShOne);
		}

		static void ServerRaEvents(RemoteAdminCommandEvent ev)
		{
			if (!ThisAccess(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			if (ev.Args[0].ToLower() == "force_mtf_respawn")
			{
				ev.Allowed = false;
				ev.Success = true;
				ev.Reply = "Успешно";
				MobileTaskForces.SpawnMtf();
			}
			else if (ev.Args[0].ToLower() == "force_ci_respawn")
			{
				ev.Allowed = false;
				ev.Success = true;
				ev.Reply = "Успешно";
				ChaosInsurgency.SpawnCI();
			}
		}
		static void RaCi(RemoteAdminCommandEvent ev)
		{
			if (!ThisOwner(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.Reply = "Успешно";
			Addons.RealSpawn.Chaos.OpenGate(null);
		}
		static void RaMtf(RemoteAdminCommandEvent ev)
		{
			if (!ThisOwner(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.Reply = "Успешно";
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
		}
		static void RaSh(RemoteAdminCommandEvent ev)
		{
			if (!ThisOwner(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.Reply = "Успешно";
			SerpentsHand.Spawn();
		}
		static void RaShOne(RemoteAdminCommandEvent ev)
		{
			if (!ThisOwner(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			try
			{
				string name = string.Join(" ", ev.Args);
				Player player = name.GetPlayer();
				if (ev.Sender.SenderId == "-@steam"
					|| ev.Sender.SenderId == "SERVER CONSOLE")
				{
					if (player == null) ev.Reply = "Игрок не найден";
					else
					{
						ev.Reply = "Успешно";
						SerpentsHand.SpawnOne(player);
					}
				}
				else ev.Reply = "Отказано в доступе";
			}
			catch
			{
				ev.Reply = "Произошла ошибка";
			}
		}
		static bool ThisAccess(string userid)
		{
			try
			{
				if (Data.Users.TryGetValue(userid, out var data) &&
					(CustomDonates.ThisYt(data) ||
					data.trainee || data.helper || data.mainhelper || data.admin || data.mainadmin || data.owner))
					return true;
				else return false;
			}
			catch { return false; }
		}
		static bool ThisOwner(string userid)
		{
			try
			{
				if (Data.Users.TryGetValue(userid, out var main) && main.id == 1)
					return true;
				else return false;
			}
			catch { return false; }
		}
		static internal void Spawn()
		{
			if (Round.Started)
			{
				int random = Random.Range(0, 100);
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

        [EventMethod(PlayerEvents.Damage)]
		static void SpawnProtect(DamageEvent ev)
		{
			if (ev.Target.Tag.Contains(SpawnProtectTag) && ev.DamageType is not DamageTypes.Warhead)
			{
				ev.Allowed = false;
				ev.Damage = 0f;
			}
		}
		static void SpawnProtect(AddTargetEvent ev)
		{
			if (ev.Target.Tag.Contains(SpawnProtectTag))
			{
				ev.Allowed = false;
			}
		}
		static void SpawnProtect(PocketEnterEvent ev)
		{
			if (ev.Player.Tag.Contains(SpawnProtectTag))
			{
				ev.Allowed = false;
			}
		}

		static internal void SpawnCor() => Timing.RunCoroutine(SpawnTeam(), "SpawnManager_SpawnTeam");
		static internal void SpawnCor(RoundEndEvent _) => Timing.KillCoroutines("SpawnManager_SpawnTeam");
		static internal IEnumerator<float> SpawnTeam()
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