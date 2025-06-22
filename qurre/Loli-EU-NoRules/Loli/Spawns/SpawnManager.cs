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
		}

		static void ServerRaEvents(RemoteAdminCommandEvent ev)
		{
			if (!ThisAccess(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			if (ev.Args[0].ToLower() == "respawn_mtf")
			{
				ev.Allowed = false;
				ev.Success = true;
				ev.Reply = "Successfully";
				MobileTaskForces.SpawnMtf();
			}
			else if (ev.Args[0].ToLower() == "respawn_ci")
			{
				ev.Allowed = false;
				ev.Success = true;
				ev.Reply = "Successfully";
				ChaosInsurgency.SpawnCI();
			}
		}
		static void RaCi(RemoteAdminCommandEvent ev)
		{
			if (!ThisOwner(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.Reply = "Successfully";
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
		}
		static void RaMtf(RemoteAdminCommandEvent ev)
		{
			if (!ThisOwner(ev.Player.UserInfomation.UserId)) return;
			ev.Prefix = "SERVER_EVENT";
			ev.Allowed = false;
			ev.Reply = "Successfully";
			RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
		}
		static bool ThisAccess(string userid)
		{
			try
			{
				if (Data.Users.TryGetValue(userid, out var data) &&
					(CustomDonates.ThisYt(data) || data.administration.owner || 
					data.administration.admin || data.administration.moderator))
					return true;
				else return false;
			}
			catch { return false; }
		}
		static bool ThisOwner(string userid)
		{
			try
			{
				if (Data.Users.TryGetValue(userid, out var main) && main.administration.owner)
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
				if (50 >= random) ChaosInsurgency.SpawnCI();
				else MobileTaskForces.SpawnMtf();
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

		[EventMethod(RoundEvents.Start)]
		static internal void SpawnCor() => Timing.RunCoroutine(SpawnTeam(), "SpawnManager_SpawnTeam");

		[EventMethod(RoundEvents.End)]
		static internal void SpawnCor2() => Timing.KillCoroutines("SpawnManager_SpawnTeam");
		static internal IEnumerator<float> SpawnTeam()
		{
			for (; ; )
			{
				int random = Random.Range(120, 180);
				yield return Timing.WaitForSeconds(random);
				Spawn();
			}
		}
	}
}