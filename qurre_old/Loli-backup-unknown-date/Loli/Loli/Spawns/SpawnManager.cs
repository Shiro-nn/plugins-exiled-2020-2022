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
		private readonly Plugin plugin;
		internal readonly ChaosInsurgency ChaosInsurgency;
		internal readonly MobileTaskForces MobileTaskForces;
		internal readonly SerpentsHand SerpentsHand;
		internal static string SpawnProtectTag => " SpawnProtect";
		public SpawnManager(Plugin plugin)
		{
			this.plugin = plugin;
			ChaosInsurgency = new ChaosInsurgency(this);
			MobileTaskForces = new MobileTaskForces(this);
			SerpentsHand = new SerpentsHand(this);
			Instance = this;
		}
		internal void Ra(SendingRAEvent ev)
		{
			try
			{
				var main = DataBase.Manager.Static.Data.Users[ev.Player.UserId];
				if (main.hr || main.ghr || main.ar || main.gar || main.or) { }
				else return;
			}
			catch { }
			if (ev.Name == "server_event")
			{
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
			else if (ev.Name == "ci")
			{
				ev.Prefix = "SERVER_EVENT";
				ev.Allowed = false;
				if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
				ev.ReplyMessage = "Успешно";
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.ChaosInsurgency);
			}
			else if (ev.Name == "mtf")
			{
				ev.Prefix = "SERVER_EVENT";
				ev.Allowed = false;
				if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
				ev.ReplyMessage = "Успешно";
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
			}
			else if (ev.Name == "sh")
			{
				ev.Prefix = "SERVER_EVENT";
				ev.Allowed = false;
				if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
				ev.ReplyMessage = "Успешно";
				SerpentsHand.Spawn();
			}
			else if (ev.Name == "shone")
			{
				ev.Prefix = "SERVER_EVENT";
				ev.Allowed = false;
				try
				{
					string name = string.Join(" ", ev.Args);
					Player player = Extensions.GetPlayer(name);
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
			if (ev.Target.Tag.Contains(SpawnProtectTag) && ev.DamageType != DamageTypes.Nuke)
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
				if (Plugin.RolePlay) random = Random.Range(200, 320);
				else if (Plugin.Anarchy) random = Random.Range(45, 75);
				yield return Timing.WaitForSeconds(random);
				Spawn();
			}
		}
	}
}