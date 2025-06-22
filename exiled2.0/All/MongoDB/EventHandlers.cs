using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Grenades;
using Hints;
using MEC;
using Mirror;
using MongoDB.auto_events;
using MongoDB.Bson;
using MongoDB.discord;
using MongoDB.Driver;
using Respawning;
using Respawning.NamingRules;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MongoDB
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		internal bool first = true;
		internal int randomspawn = 0;
		internal bool alphakill = false;
		private int colors = 9;
		private bool wg = false;
		public List<Vector3> Bloods = new List<Vector3>();
		public static Dictionary<string, VecPos> Pos = new Dictionary<string, VecPos>();
		public static bool RoundStarted = false;
		internal void AntiBan(SendingRemoteAdminCommandEventArgs ev)
		{
			if (ev.Name == "ban")
			{
				string[] str = ev.Arguments[0].Split('.');
				if (str.Count() > 2)
				{
					ev.IsAllowed = false;
					ev.Sender.Disconnect("ай, ай, ай");
					try
					{
						var client = new MongoClient(plugin.mongodburl);
						var database = client.GetDatabase("login");
						var collection = database.GetCollection<BsonDocument>("accounts");
						var filter = Builders<BsonDocument>.Filter.Eq("steam", ev.Sender.UserId.Replace("@steam", ""));
						collection.UpdateMany(filter, Builders<BsonDocument>.Update.Set("sr", false));
						collection.UpdateMany(filter, Builders<BsonDocument>.Update.Set("hr", false));
						collection.UpdateMany(filter, Builders<BsonDocument>.Update.Set("ghr", false));
						collection.UpdateMany(filter, Builders<BsonDocument>.Update.Set("ar", false));
						collection.UpdateMany(filter, Builders<BsonDocument>.Update.Set("gar", false));
					}
					catch { }
					string hook = "https://discord.com/api/webhooks/";
					Webhook webhk = new Webhook(hook);
					List<Embed> listEmbed = new List<Embed>();
					Embed embed = new Embed();
					embed.Title = "Попытка краша";
					embed.Color = 1;
					embed.Description = $"Попытался крашнуть:\n{ev.Sender.Nickname} - {ev.Sender.UserId}";
					embed.TimeStamp = DateTimeOffset.Now;
					listEmbed.Add(embed);
					webhk.Send("ну да, ну да, -админка у чела, снимите роль, а то мне лень", Plugin.ServerName, null, false, embeds: listEmbed);
				}
			}
		}
		public void AntiEscapeBag(EscapingEventArgs ev)
		{
			if (Round.ElapsedTime.TotalSeconds < 10) ev.IsAllowed = false;
		}
		internal void AntiBloodFlood(PlacingBloodEventArgs ev)
		{
			try { if (ev.Player.Id == Player.Get(scp343.API.scp343Data.GetScp343()).Id) ev.IsAllowed = false; } catch { }
			try { if (ev.Player.Id == Player.Get(scp228.API.Scp228Data.GetScp228()).Id) ev.IsAllowed = false; } catch { }
			if (ev.IsAllowed)
			{
				bool spawn = true;
				foreach (Vector3 bp in Bloods)
				{
					if (Vector3.Distance(bp, ev.Position) < 2f) spawn = false;
				}
				if (spawn) Bloods.Add(ev.Position);
				else ev.IsAllowed = false;
			}
		}
		internal void AntiScp106Bag(ContainingEventArgs ev)
		{
			if (ev.IsAllowed)
			{
				plugin.donate.contain = true;
				Timing.CallDelayed(15f, () =>
				{
					List<Player> list = Player.List.Where(x => x.Role == RoleType.Scp106).ToList();
					foreach (Player pl in list) pl.ReferenceHub.Damage(999999, DamageTypes.Poison);
				});
			}
		}
		public void AntiEscapeBag()
		{/*
			Vector3 escape1 = new Vector3(174, 989, 32);
			Vector3 escape2 = new Vector3(166, 980, 25);
			try
			{
				foreach (Player pl in Player.List.Where(x => x.Role == RoleType.ClassD &&
				x.Position.x <= escape1.x && x.Position.x >= escape2.x && x.Position.y <= escape1.y &&
				x.Position.y >= escape2.y && x.Position.z <= escape1.z && x.Position.z >= escape2.z))
                {
                    if (pl.IsCuffed) pl.SetRole(RoleType.NtfCadet, false, true);
                    else pl.SetRole(RoleType.ChaosInsurgency, false, true);
                }
			}
			catch { }
			try
			{
				foreach (Player pl in Player.List.Where(x => x.Role == RoleType.Scientist &&
				x.Position.x <= escape1.x && x.Position.x >= escape2.x && x.Position.y <= escape1.y &&
				x.Position.y >= escape2.y && x.Position.z <= escape1.z && x.Position.z >= escape2.z))
				{
					if (pl.IsCuffed) pl.SetRole(RoleType.ChaosInsurgency, false, true);
					else pl.SetRole(RoleType.NtfScientist, false, true);
				}
			}
			catch { }*/
		}
		internal void RoundEnd(RoundEndedEventArgs ev)
		{
			Timing.CallDelayed(ev.TimeToRestart - 1f, () =>
			{
				foreach (Player player in Player.List)
				{
					player.ReferenceHub.playerStats.CallRpcRoundrestart(20, true);
				}
			});
			RoundStarted = false;
		}
		public void OnWaitingForPlayers()
		{
			try
			{
				if (first)
				{
					first = false;
					ServerConsole.ReloadServerName();
					Plugin.Coroutines.Add(Timing.RunCoroutine(alle()));
					Plugin.Coroutines.Add(Timing.RunCoroutine(plugin.mec.randombc()));
					Plugin.Coroutines.Add(Timing.RunCoroutine(Clear()));
					Plugin.Coroutines.Add(Timing.RunCoroutine(ChopperThread()));
					Plugin.Coroutines.Add(Timing.RunCoroutine(RotatePickup()));
					Plugin.Coroutines.Add(Timing.RunCoroutine(CorrodeHost()));
					Plugin.Coroutines.Add(Timing.RunCoroutine(plugin.armor.Offstamina()));
					Plugin.Coroutines.Add(Timing.RunCoroutine(spawns()));
				}
			}
			catch { }
			try { GameObject.Find("StartRound").transform.localScale = Vector3.zero; } catch { }
			try { plugin.cfg1(); } catch { }
			try { GameCore.Console.singleton.TypeCommand($"/mapeditor load new"); } catch { }
			wg = false;
			RoundStarted = false;
		}
		public void OnRoundStart()
		{
			RoundStarted = true;
			alphakill = false;
			randomspawn = Random.Range(1, 100);
			foreach (GameObject work in UnityEngine.Object.FindObjectsOfType<GameObject>())
			{
				try
				{
					if (work == null) continue;
					if (work.gameObject == null) continue;
					if (work.GetComponent<WorkStation>() == null) continue;
					if (work?.gameObject?.name == "Work Station")
						work.GetComponent<WorkStation>().NetworkisTabletConnected = true;
				}
				catch { }
			}
			colors = 9;
			RespawnManager.Singleton.NamingManager.AllUnitNames.Clear();
			RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
			{
				SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
				UnitName = $"<color=#ff0000>#</color>" +
						$"<color=#ffff00>f</color>" +
						$"<color=#00ff3c>y</color>" +
						$"<color=#00eaff>d</color>" +
						$"<color=#0082ff>n</color>" +
						$"<color=#0019ff>e</color>"
			});
			RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
			{
				SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
				UnitName = $"<size=80%><color=#fdffbb>Длительность раунда</color>\n<color=#0089c7>{Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}</color></size>"
			});
			RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
			{
				SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
				UnitName = $"<color=#9b9b9b>Охрана</color>"
			});
			wg = false;
			//foreach(Player pl in Player.List) pl.SetPlayerScale(1f);
		}
		internal void Elevator(InteractingElevatorEventArgs ev) => ev.Lift.movingSpeed = 1;
		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			string str = $"\n<color=#00fffb>Добро пожаловать на сервер</color> <color=#ffa600>f</color><color=#ffff00>y<color=#1eff00>d</color><color=#0004ff>n</color><color=#9d00ff>e</color>\n<color=#09ff00>Приятной игры!</color>";
			ev.Player.ReferenceHub.hints.Show(new TextHint(str.Trim(), new HintParameter[]
			{
					new StringHintParameter("")
			}, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f), 10f));
			Timing.CallDelayed(1f, () =>
			{
				if (!Round.IsStarted && Round.ElapsedTime.Minutes == 0)
				{
					ev.Player.SetRole(RoleType.Tutorial);
					//ev.Player.SetPlayerScale(0.5f);
					Timing.CallDelayed(0.5f, () =>
					{
						if (randomspawn < 25) Timing.CallDelayed(0.3f, () => ev.Player.ReferenceHub.SetPosition(new Vector3(181, 991, 29)));
						else if (randomspawn < 50) Timing.CallDelayed(0.3f, () => ev.Player.ReferenceHub.SetPosition(new Vector3(152, 1019.5f, -17)));
						else if (randomspawn < 75) Timing.CallDelayed(0.3f, () => ev.Player.ReferenceHub.SetPosition(new Vector3(220, 1027, -18)));
						else Timing.CallDelayed(0.3f, () => ev.Player.ReferenceHub.SetPosition(new Vector3(187, 998, -3)));
					});
				}
			});
		}
		public void tesla(TriggeringTeslaEventArgs ev)
		{
			if (!Warhead.IsInProgress)
			{
				ev.IsTriggerable = false;
			}
		}
		internal void roundend(RoundEndedEventArgs ev)
		{
			if (!Server.FriendlyFire)
			{
				Cassie.Message("ATTENTION TO ALL PERSONNEL . f f ENABLE");
				Server.FriendlyFire = true;
				Timing.CallDelayed(ev.TimeToRestart - 3f, () => Server.FriendlyFire = false);
			}
			Timing.CallDelayed(0.5f, () =>
			{
				foreach (ReferenceHub player in Extensions.GetHubs().ToList())
					plugin.donate.setprefix(player);
			});
		}
		internal void OnTeamRespawn(RespawningTeamEventArgs ev)
		{
			if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
			{
				Map.TurnOffAllLights(10, false);
				Cassie.Message(".g1", false, false);
			}
		}
		internal void detonated()
		{
			Timing.CallDelayed(30f, () =>
			{
				if (Warhead.IsDetonated)
				{
					wg = false;
					Cassie.Message("ATTENTION TO ALL PERSONNEL . THE outside will be Corrupted Through 30 Seconds", false, false);
				}

			});
			Timing.CallDelayed(60f, () =>
			{
				if (Warhead.IsDetonated)
				{
					alphakill = true;
					Cassie.Message("ATTENTION TO ALL PERSONNEL . THE outside will be Corrupted", false, false);
					Map.Broadcast(10, "<size=25%><color=#6f6f6f>По приказу совета О5 территория комплекса <color=red>отравлена</color></color></size>");
				}

			});
		}
		internal void Dying(DyingEventArgs ev)
		{
			if (!Round.IsStarted && Round.ElapsedTime.Minutes == 0)
				ev.IsAllowed = false;
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == plugin.cat_hook.hook_owner?.ReferenceHub.queryProcessor.PlayerId)
				if (ev.HitInformation.GetDamageType() == DamageTypes.Flying)
					ev.IsAllowed = false;
		}
		internal void Hurt(HurtingEventArgs ev)
		{
			if (!Round.IsStarted && Round.ElapsedTime.Minutes == 0)
				ev.IsAllowed = false;
			if (ev.Target.ReferenceHub.queryProcessor.PlayerId == plugin.cat_hook.hook_owner?.ReferenceHub.queryProcessor.PlayerId)
				if (ev.DamageType == DamageTypes.Flying)
					ev.IsAllowed = false;
		}
		internal void scp914(UpgradingItemsEventArgs ev)
		{
			foreach (Player player in ev.Players.Where(x => x.Role == RoleType.Scp0492))
			{
				Vector3 pos = player.ReferenceHub.transform.position;
				player.ReferenceHub.SetRole(RoleType.ClassD);
				player.Health = 25;
				Timing.CallDelayed(0.5f, () => player.Position = pos);
			}
		}
		public void CheaterReport(ReportingCheaterEventArgs ev)
		{
			string hook = "https://discordapp.com/api/webhooks/";
			string[] ignoreKeywords = { "fly", "flying", "flyed", "летает", "читер", "пин", "летчик", "пилот", "cheat", "читы", "cheater", "clip", "клип" };
			bool keywordFound = ignoreKeywords.Any(s => ev.Reason.ToLower().IndexOf(s, System.StringComparison.OrdinalIgnoreCase) >= 0);
			if (ev.Reported.ReferenceHub.queryProcessor.PlayerId == plugin.cat_hook.hook_owner?.ReferenceHub.queryProcessor.PlayerId)
			{
				if (keywordFound)
				{
					ev.Reporter.ReferenceHub.Broadcast("<size=30%><color=#fdffbb>Вы кидаете репорт на владельца " +
						"<color=#0089c7>крюк<color=#9bff00>-</color>кошки</color> за полет</color>\n" +
						"<color=#f47fff>Прочитайте про " +
						"<color=#0089c7>крюк<color=#9bff00>-</color>кошку</color> " +
						"в консоли на <color=#ffffff>[<color=#e6ffa1>ё</color>]</color><color=lime>,</color> " +
						"написав <color=#ff5e70>.</color><color=#387aff>cat_hook info</color></color></size>", 5);
					ev.IsAllowed = false;
					return;
				}
			}
			if (ev.Reported.ReferenceHub.characterClassManager.UserId == ev.Reporter.ReferenceHub.characterClassManager.UserId)
			{
				ev.Reporter.ReferenceHub.Broadcast("Вы не можете кинуть репорт на себя", 5);
				ev.IsAllowed = false;
				return;
			}

			Webhook webhk = new Webhook(hook);

			List<Embed> listEmbed = new List<Embed>();


			EmbedField reporterName = new EmbedField();
			reporterName.Name = "Репорт отправил:";
			reporterName.Value = $"{ev.Reporter.ReferenceHub.nicknameSync.MyNick} - {ev.Reporter.ReferenceHub.characterClassManager.UserId}";
			reporterName.Inline = true;

			EmbedField reportedName = new EmbedField();
			reportedName.Name = "Зарепорчен:";
			reportedName.Value = $"{ev.Reported.ReferenceHub.nicknameSync.MyNick} - {ev.Reported.ReferenceHub.characterClassManager.UserId}";
			reportedName.Inline = true;

			EmbedField Reason = new EmbedField();
			Reason.Name = "Причина";
			Reason.Value = ev.Reason;
			Reason.Inline = true;

			Embed embed = new Embed();
			embed.Title = "Новый репорт";
			embed.Color = 1;
			embed.Fields.Add(reporterName);
			embed.Fields.Add(reportedName);
			embed.Fields.Add(Reason);


			listEmbed.Add(embed);

			webhk.Send("Новый репорт **на читера**", Plugin.ServerName, null, false, embeds: listEmbed);
		}
		public void LocalReport(LocalReportingEventArgs ev)
		{
			string hook = "https://discordapp.com/api/webhooks/";
			if (ev.Target.ReferenceHub.characterClassManager.UserId == ev.Issuer.ReferenceHub.characterClassManager.UserId)
			{
				ev.Issuer.ReferenceHub.Broadcast("Вы не можете кинуть репорт на себя", 5);
				ev.IsAllowed = false;
				return;
			}

			Webhook webhk = new Webhook(hook);

			List<Embed> listEmbed = new List<Embed>();


			EmbedField reporterName = new EmbedField();
			reporterName.Name = "Репорт отправил:";
			reporterName.Value = $"{ev.Issuer.ReferenceHub.nicknameSync.MyNick} - {ev.Issuer.ReferenceHub.characterClassManager.UserId}";
			reporterName.Inline = true;

			EmbedField reportedName = new EmbedField();
			reportedName.Name = "Зарепорчен:";
			reportedName.Value = $"{ev.Target.ReferenceHub.nicknameSync.MyNick} - {ev.Target.ReferenceHub.characterClassManager.UserId}";
			reportedName.Inline = true;

			EmbedField Reason = new EmbedField();
			Reason.Name = "Причина";
			Reason.Value = ev.Reason;
			Reason.Inline = true;

			Embed embed = new Embed();
			embed.Title = "Новый репорт";
			embed.Color = 1;
			embed.Fields.Add(reporterName);
			embed.Fields.Add(reportedName);
			embed.Fields.Add(Reason);


			listEmbed.Add(embed);

			webhk.Send("Новый репорт", Plugin.ServerName, null, false, embeds: listEmbed);
		}
		public IEnumerator<float> spawns()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(170f);
				plugin.shEventHandlers.spawn();
			}
		}
		public IEnumerator<float> alle()
		{
			for (; ; )
			{
				try { AntiZeroHP(); } catch { }
				try { ScientistNull(); } catch { }
				try { AntiEscapeBag(); } catch { }
				try { AutoAlpha(); } catch { }
				try { PosCheck(); } catch { }
				try { accordion(); } catch { }
				try { alphadead(); } catch { }
				try { alphagrenades(); } catch { }
				try { names(); } catch { }
				try { checkescape(); } catch { }
				try { plugin.EventHandlers343.alltime(); } catch { }
				try { plugin.EventHandlers228.Gopd(); } catch { }
				try { plugin.EventHandlers228.CorrodeUpdate(); } catch { }
				try { plugin.scpheal.scpheals(); } catch { }
				try { plugin.text.icomtext(); } catch { }
				try { logs.send.Handle(); } catch { }
				try { plugin.Main035.EventHandlers.CorrodeUpdate(); } catch { }
				try { plugin.shEventHandlers.shtimee(); } catch { }
				try { plugin.EventHandlersP.enpolt(); } catch { }
				try { plugin.Main.storm_base.Refresh(); } catch { }
				try { plugin.stalky.CooldownUpdate(); } catch { }

				yield return Timing.WaitForSeconds(1f);
			}
		}

		public void AntiZeroHP()
		{
			try
			{
				List<Player> list = Player.List.Where(x => x.Health < 1).ToList();
				foreach (Player player in list)
				{
					player.Health = 1;
					player.ReferenceHub.Damage(1000, DamageTypes.Poison);
				}
			}
			catch { }
		}
		public void PosCheck()
		{
			try
			{
				List<Player> nf = Player.List.Where(x => !Pos.ContainsKey(x.UserId)).ToList();
				foreach (Player p in nf) Pos.Add(p.UserId, new VecPos());
				List<Player> list = Player.List.Where(x => Pos.ContainsKey(x.UserId)).ToList();
				foreach (Player pl in list)
				{
					if (Vector3.Distance(Pos[pl.UserId].Pos, pl.Position) < 0.5)
					{
						if (Pos[pl.UserId].sec > 30) Pos[pl.UserId].Alive = false;
						Pos[pl.UserId].sec++;
						Pos[pl.UserId].Pos = pl.Position;
					}
				}
			}
			catch { }
		}
		public void accordion()
		{
			if (!Round.IsStarted)
			{
				string onemsg = $"<size=50><color=yellow><b>Раунд будет запущен через {GameCore.RoundStart.singleton.NetworkTimer} секунд</b></color></size>";
				if (GameCore.RoundStart.singleton.NetworkTimer < 0)
				{
					onemsg = "";
				}
				int playersc = Extensions.GetHubs().ToList().Count;
				List<ReferenceHub> players = Extensions.GetHubs().Where(x => x.GetRole() == RoleType.Tutorial).ToList();
				string msg = $"{onemsg}\n<size=40><i>{playersc} игроков</i></size>";
				foreach (ReferenceHub player in players)
				{
					player.Hint(msg, 1);
				}
			}
		}

		public void alphadead()
		{
			if (alphakill)
			{
				foreach (Player player in Player.List)
				{
					if (player.Team == Team.SCP) player.ReferenceHub.Damage(150, DamageTypes.Nuke);
					else player.ReferenceHub.Damage(5, DamageTypes.Nuke);
				}
			}
		}
		public void grenadespawn(Vector3 position)
		{
			GrenadeManager gm = Extensions.GetHost().FirstOrDefault().GetComponent<GrenadeManager>();
			GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFrag);
			FragGrenade granade = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FragGrenade>();
			granade.fuseDuration = 2f;
			granade.InitData(gm, Vector3.zero, Vector3.zero, 0f);
			granade.transform.position = position;
			NetworkServer.Spawn(granade.gameObject);
		}
		private int c { get; set; } = 26;
		public void alphagrenades()
		{
			if (wg)
			{
				c--;
				if (c == 25) grenadespawn(new Vector3(190, 1002, -59));
				else if (c == 25)
				{
					grenadespawn(new Vector3(185, 1002, -60));
					grenadespawn(new Vector3(180, 1002, -59));
					grenadespawn(new Vector3(185, 1002, -40));
				}
				else if (c == 24)
				{
					grenadespawn(new Vector3(185, 1002, -70));
					grenadespawn(new Vector3(170, 1002, -59));
					grenadespawn(new Vector3(185, 1002, -30));
				}
				else if (c == 23)
				{
					grenadespawn(new Vector3(185, 1002, -80));
					grenadespawn(new Vector3(160, 1002, -59));
					grenadespawn(new Vector3(185, 1002, -20));
				}
				else if (c == 22)
				{
					grenadespawn(new Vector3(185, 1002, -90));
					grenadespawn(new Vector3(150, 1002, -59));
					grenadespawn(new Vector3(185, 1002, -10));
				}
				else if (c == 21)
				{
					grenadespawn(new Vector3(140, 1002, -59));
					grenadespawn(new Vector3(185, 1002, 0));
				}
				else if (c == 20)
				{
					grenadespawn(new Vector3(130, 1002, -59));
					grenadespawn(new Vector3(175, 1002, 0));
				}
				else if (c == 19)
				{
					grenadespawn(new Vector3(120, 1002, -59));
					grenadespawn(new Vector3(175, 1002, 10));
				}
				else if (c == 18)
				{
					grenadespawn(new Vector3(110, 1002, -59));
					grenadespawn(new Vector3(170, 1002, 20));
				}
				else if (c == 17)
				{
					grenadespawn(new Vector3(100, 1002, -59));
					grenadespawn(new Vector3(170, 1002, 30));
				}
				else if (c == 16)
				{
					grenadespawn(new Vector3(90, 1002, -59));
					grenadespawn(new Vector3(170, 1002, 40));
				}
				else if (c == 15) grenadespawn(new Vector3(80, 1002, -59));
				else if (c == 14) grenadespawn(new Vector3(70, 1002, -59));
				else if (c == 13) grenadespawn(new Vector3(60, 1002, -59));
				else if (c == 12)
				{
					grenadespawn(new Vector3(50, 1012, -59));
					grenadespawn(new Vector3(50, 1012, -70));
					grenadespawn(new Vector3(50, 1002, -70));
					grenadespawn(new Vector3(50, 1012, -44));
				}
				else if (c == 11)
				{
					grenadespawn(new Vector3(40, 1012, -59));
					grenadespawn(new Vector3(40, 1012, -70));
					grenadespawn(new Vector3(40, 1002, -70));
					grenadespawn(new Vector3(40, 1012, -44));
				}
				else if (c == 10)
				{
					grenadespawn(new Vector3(30, 1012, -59));
					grenadespawn(new Vector3(30, 1012, -70));
					grenadespawn(new Vector3(30, 1002, -70));
					grenadespawn(new Vector3(30, 1012, -44));
				}
				else if (c == 9)
				{
					grenadespawn(new Vector3(20, 1012, -59));
					grenadespawn(new Vector3(20, 1012, -70));
					grenadespawn(new Vector3(20, 1002, -70));
					grenadespawn(new Vector3(20, 1012, -44));
				}
				else if (c == 8)
				{
					grenadespawn(new Vector3(10, 1012, -59));
					grenadespawn(new Vector3(10, 1012, -70));
					grenadespawn(new Vector3(10, 1002, -70));
					grenadespawn(new Vector3(10, 1012, -44));
				}
				else if (c == 7)
				{
					grenadespawn(new Vector3(0, 1012, -59));
					grenadespawn(new Vector3(0, 1012, -70));
					grenadespawn(new Vector3(0, 1002, -70));
					grenadespawn(new Vector3(0, 1012, -44));
					grenadespawn(new Vector3(0, 1002, -30));
				}
				else if (c == 6)
				{
					grenadespawn(new Vector3(-10, 1012, -59));
					grenadespawn(new Vector3(-10, 1012, -70));
					grenadespawn(new Vector3(-10, 1002, -70));
					grenadespawn(new Vector3(-10, 1012, -44));
					grenadespawn(new Vector3(0, 1002, -20));
				}
				else if (c == 5)
				{
					grenadespawn(new Vector3(-20, 1012, -59));
					grenadespawn(new Vector3(-20, 1012, -70));
					grenadespawn(new Vector3(-20, 1002, -70));
					grenadespawn(new Vector3(-20, 1012, -44));
					grenadespawn(new Vector3(-10, 1002, -10));
					grenadespawn(new Vector3(0, 1002, -10));
					grenadespawn(new Vector3(10, 1002, -10));
				}
				else if (c == 4)
				{
					grenadespawn(new Vector3(-30, 1002, -59));
					grenadespawn(new Vector3(0, 1002, 0));
				}
				else if (c == 3)
				{
					grenadespawn(new Vector3(-40, 1002, -59));
					grenadespawn(new Vector3(-10, 1002, 10));
					grenadespawn(new Vector3(0, 1002, 10));
					grenadespawn(new Vector3(10, 1002, 10));
				}
				else if (c == 2) grenadespawn(new Vector3(-50, 1002, -59));
				else if (c == 1) grenadespawn(new Vector3(-60, 1002, -59));
				else if (c == 0)
				{
					grenadespawn(new Vector3(-190, 1002, -59));
					c = 25;
				}
			}
		}

		public void names()
		{
			try
			{
				string lgbt = "";
				if (colors == 9)
				{
					colors--;
					lgbt = $"" +
						$"<color=#ff0000>#</color>" +
						$"<color=#ffff00>f</color>" +
						$"<color=#00ff3c>y</color>" +
						$"<color=#00eaff>d</color>" +
						$"<color=#0082ff>n</color>" +
						$"<color=#0019ff>e</color>";
				}
				else if (colors == 8)
				{
					colors--;
					lgbt = $"" +
						$"<color=#ffff00>#</color>" +
						$"<color=#00ff3c>f</color>" +
						$"<color=#00eaff>y</color>" +
						$"<color=#0082ff>d</color>" +
						$"<color=#0019ff>n</color>" +
						$"<color=#7f00ff>e</color>";
				}
				else if (colors == 7)
				{
					colors--;
					lgbt = $"" +
						$"<color=#00ff3c>#</color>" +
						$"<color=#00eaff>f</color>" +
						$"<color=#0082ff>y</color>" +
						$"<color=#0019ff>d</color>" +
						$"<color=#7f00ff>n</color>" +
						$"<color=#ef00ff>e</color>";
				}
				else if (colors == 6)
				{
					colors--;
					lgbt = $"" +
						$"<color=#00eaff>#</color>" +
						$"<color=#0082ff>f</color>" +
						$"<color=#0019ff>y</color>" +
						$"<color=#7f00ff>d</color>" +
						$"<color=#ef00ff>n</color>" +
						$"<color=#ff00ae>e</color>";
				}
				else if (colors == 5)
				{
					colors--;
					lgbt = $"" +
						$"<color=#0082ff>#</color>" +
						$"<color=#0019ff>f</color>" +
						$"<color=#7f00ff>y</color>" +
						$"<color=#ef00ff>d</color>" +
						$"<color=#ff00ae>n</color>" +
						$"<color=#ff0000>e</color>";
				}
				else if (colors == 4)
				{
					colors--;
					lgbt = $"" +
						$"<color=#0019ff>#</color>" +
						$"<color=#7f00ff>f</color>" +
						$"<color=#ef00ff>y</color>" +
						$"<color=#ff00ae>d</color>" +
						$"<color=#ff0000>n</color>" +
						$"<color=#ffff00>e</color>";
				}
				else if (colors == 3)
				{
					colors--;
					lgbt = $"" +
						$"<color=#7f00ff>#</color>" +
						$"<color=#ef00ff>f</color>" +
						$"<color=#ff00ae>y</color>" +
						$"<color=#ff0000>d</color>" +
						$"<color=#ffff00>n</color>" +
						$"<color=#00ff3c>e</color>";
				}
				else if (colors == 3)
				{
					colors--;
					lgbt = $"" +
						$"<color=#ef00ff>#</color>" +
						$"<color=#ff00ae>f</color>" +
						$"<color=#ff0000>y</color>" +
						$"<color=#ffff00>d</color>" +
						$"<color=#00ff3c>n</color>" +
						$"<color=#00eaff>e</color>";
				}
				else if (colors == 2)
				{
					colors = 9;
					lgbt = $"" +
						$"<color=#ff00ae>#</color>" +
						$"<color=#ff0000>f</color>" +
						$"<color=#ffff00>y</color>" +
						$"<color=#00ff3c>d</color>" +
						$"<color=#00eaff>n</color>" +
						$"<color=#0082ff>e</color>";
				}
				else
				{
					colors = 9;
				}
				foreach (Player player in Player.List.Where(x => x.ReferenceHub.characterClassManager.NetworkCurUnitName.Replace("</color>", "")
				.Replace("<color=#ff0000>", "")
				.Replace("<color=#ffff00>", "")
				.Replace("<color=#00ff3c>", "")
				.Replace("<color=#00eaff>", "")
				.Replace("<color=#0082ff>", "")
				.Replace("<color=#0019ff>", "")
				.Replace("<color=#7f00ff>", "")
				.Replace("<color=#ef00ff>", "")
				.Replace("<color=#ff00ae>", "")
				.Replace("<color=#ff0000>", "")
				.Replace("<color=#ffff00>", "")
				.Replace("<color=#00ff3c>", "")
				.Replace("<color=#00eaff>", "")
				.Replace("<color=#0082ff>", "")
				== "#fydne"))
				{
					player.ReferenceHub.GetComponent<UnitNamingManager>();
					player.ReferenceHub.characterClassManager.NetworkCurUnitName = lgbt;
				}
				foreach (Player player in Player.List.Where(x => x.Role == RoleType.FacilityGuard))
				{
					player.ReferenceHub.characterClassManager.NetworkCurUnitName = $"Охрана";
				}
				if (Round.IsStarted)
				{
					try
					{
						RespawnManager.Singleton.NamingManager.AllUnitNames[0] = new SyncUnit
						{
							SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
							UnitName = lgbt
						};
						RespawnManager.Singleton.NamingManager.AllUnitNames[1] = new SyncUnit
						{
							SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
							UnitName = $"<size=80%><color=#fdffbb>Длительность раунда</color>\n<color=#0089c7>{Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}</color></size>"
						};
					}
					catch { }
				}
			}
			catch { }
		}
		public void checkescape()
		{
			if (Round.IsStarted)
			{
				Vector3 escape1 = new Vector3(174, 989, 32);
				Vector3 escape2 = new Vector3(166, 980, 25);
				List<Player> playerssl = Player.List.ToList();
				foreach (Player player in playerssl)
				{
					if (player.ReferenceHub.transform.position.x <= escape1.x && player.ReferenceHub.transform.position.x >= escape2.x && player.ReferenceHub.transform.position.y <= escape1.y && player.ReferenceHub.transform.position.y >= escape2.y && player.ReferenceHub.transform.position.z <= escape1.z && player.ReferenceHub.transform.position.z >= escape2.z)
					{
						if (!player.IsCuffed)
						{
							if (player.ReferenceHub.GetRole() == RoleType.FacilityGuard)
							{
								plugin.armor.spawn(player);
								player.ReferenceHub.ChangeRole(RoleType.NtfLieutenant);
								Timing.CallDelayed(0.05f, () => player.ReferenceHub.DropItem());
							}
						}
						if (player.IsCuffed)
						{
							if (player.ReferenceHub.GetRole() == RoleType.ChaosInsurgency)
							{
								player.ReferenceHub.ChangeRole(RoleType.NtfLieutenant);
							}
							else if (player.Team == Team.MTF)
							{
								player.ReferenceHub.ChangeRole(RoleType.ChaosInsurgency);
							}
						}
					}
				}
			}
		}
		public void AnnouncingMTF(AnnouncingNtfEntranceEventArgs ev)
		{
			ev.IsAllowed = false;
			Cassie.Message($"XMAS_EPSILON11 {ev.UnitName} XMAS_HASENTERED {ev.UnitNumber} XMAS_SCPSUBJECTS");
		}
		private IEnumerator<float> CorrodeHost()
		{
			for (; ; )
			{
				if (scp035.EventHandlers.scpPlayer != null)
				{
					scp035.EventHandlers.scpPlayer.playerStats.Health -= 1;
					if (scp035.EventHandlers.scpPlayer.playerStats.Health <= 0)
					{
						scp035.EventHandlers.scpPlayer.ChangeRole(RoleType.Spectator);
						plugin.Main035.EventHandlers.KillScp035();
					}
				}
				yield return Timing.WaitForSeconds(5);
			}
		}

		private IEnumerator<float> RotatePickup()
		{
			while (plugin.Main035.EventHandlers.isRoundStarted)
			{
				if (scp035.EventHandlers.isRotating)
				{
					plugin.Main035.EventHandlers.RefreshItems();
				}
				yield return Timing.WaitForSeconds(120);
			}
		}
		public void AutoAlpha()
		{
			if (Round.ElapsedTime.Minutes == 16)
			{
				if (!plugin.EventHandlers343.autowarheadstart && !Warhead.IsDetonated)
				{
					Map.ClearBroadcasts();
					Map.Broadcast(scp343.Configs.autoabct, scp343.Configs.autoabc);
					Warhead.Start();
					plugin.EventHandlers343.autowarheadstart = true;
				}
			}
		}
		public void RoundEnding(EndingRoundEventArgs ev)
		{
			try
			{
				if (storm_base.Enabled) return;
				ReferenceHub scp343 = null;
				try { scp343 = Player.Get(MongoDB.scp343.API.scp343Data.GetScp343()).ReferenceHub; } catch { }
				ReferenceHub scp035 = null;
				try { scp035 = Player.Get(MongoDB.scp035.API.Scp035Data.GetScp035()).ReferenceHub; } catch { }
				bool end = false;
				int cw = 0;
				int mw = 0;
				int sw = 0;
				int d = ev.ClassList.class_ds - (scp343 != null && scp343.GetRole() == RoleType.ClassD ? 1 : 0) - (scp035 != null && scp035.GetRole() == RoleType.ClassD ? 1 : 0);
				int s = ev.ClassList.scientists - (scp035 != null && scp035.GetRole() == RoleType.Scientist ? 1 : 0);
				int ci = ev.ClassList.chaos_insurgents - (scp035 != null && scp035.GetRole() == RoleType.ChaosInsurgency ? 1 : 0);
				int mtf = ev.ClassList.mtf_and_guards - (scp035 != null && Player.Get(scp035).Team == Team.MTF ? 1 : 0);
				int scp = ev.ClassList.scps_except_zombies + (scp035 != null && Player.Get(scp035).Team == Team.MTF ? 1 : 0);
				bool MTFAlive = Extensions.CountRoles(Team.MTF) > 0;
				bool CiAlive = Extensions.CountRoles(Team.CHI) > 0;
				bool ScpAlive = Extensions.CountRoles(Team.SCP) + (scp035 != null && scp035.GetRole() != RoleType.Spectator ? 1 : 0) > 0;
				bool DClassAlive = Extensions.CountRoles(Team.CDP) > 0;
				bool ScientistsAlive = Extensions.CountRoles(Team.RSC) > 0;
				bool SHAlive = sh.shEventHandlers.shPlayers.Where(x => Player.Get(x)?.Role == RoleType.Tutorial).ToList().Count > 0;
				ev.IsAllowed = false;
				if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive/* && !CiAlive*/)
				{
					end = true;
					sw++;
				}
				else if (!SHAlive && !ScpAlive && (MTFAlive || ScientistsAlive) && !DClassAlive && !CiAlive)
				{
					end = true;
					mw++;
				}
				else if (!SHAlive && !ScpAlive && !MTFAlive && !ScientistsAlive && (DClassAlive || CiAlive))
				{
					end = true;
					cw++;
				}
				if (end)
				{
					ev.IsAllowed = true;
					ev.IsRoundEnded = true;
					if (d > s) cw++;
					else if (d < s) mw++;
					else if (scp > d + s) sw++;
					if (ci > mtf) cw++;
					else if (ci < mtf) mw++;
					else if (scp > ci + mtf) sw++;
					if (cw > mw)
					{
						if (cw > sw) ev.LeadingTeam = (Exiled.API.Enums.LeadingTeam)LeadingTeam.ChaosInsurgency;
						else if (mw < sw) ev.LeadingTeam = (Exiled.API.Enums.LeadingTeam)LeadingTeam.Anomalies;
						else ev.LeadingTeam = (Exiled.API.Enums.LeadingTeam)LeadingTeam.Draw;
					}
					else if (mw > cw)
					{
						if (mw > sw) ev.LeadingTeam = (Exiled.API.Enums.LeadingTeam)LeadingTeam.FacilityForces;
						else if (cw < sw) ev.LeadingTeam = (Exiled.API.Enums.LeadingTeam)LeadingTeam.Anomalies;
						else ev.LeadingTeam = (Exiled.API.Enums.LeadingTeam)LeadingTeam.Draw;
					}
					else ev.LeadingTeam = (Exiled.API.Enums.LeadingTeam)LeadingTeam.Draw;
				}
			}
			catch { }
		}
		public void ScientistNull()
		{
			try
			{
				bool ScientistsAlive = Extensions.CountRoles(Team.RSC) > 0;
				if (!ScientistsAlive)
				{
					if (Round.ElapsedTime.Minutes > 13)
					{
						if (plugin.shEventHandlers.alphadis && !plugin.shEventHandlers.alphaon)
						{
							plugin.shEventHandlers.alphaon = true;
							Map.ClearBroadcasts();
							Map.Broadcast(10, "<size=25%><color=#6f6f6f>Совет О5 согласился на <color=#0089c7>взрыв</color> <color=red>Альфа Боеголовки</color></color></size>");
							plugin.EventHandlers343.autowarheadstart = true;
							Warhead.Start();
						}
						else if (!plugin.shEventHandlers.alphaon)
						{
							plugin.shEventHandlers.alphaon = true;
							Map.ClearBroadcasts();
							Map.Broadcast(10, "<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер/сбежал</color>\n<color=#6f6f6f>Совет О5 дал приказ запуска <color=red>Альфа Боеголовки</color></color></size>");
							plugin.EventHandlers343.autowarheadstart = true;
							Warhead.Start();
						}
					}
					else
					{
						if (Round.ElapsedTime.Minutes > 1)
							if (!plugin.shEventHandlers.alphadis)
							{
								plugin.shEventHandlers.alphadis = true;
								Map.Broadcast(10, "<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер<color=red>/</color>сбежал</color>\n<color=#6f6f6f>Совет О5 <color=#0089c7>обсуждает</color> запуск <color=red>Альфа Боеголовки</color></color></size>");
							}
					}
				}
			}
			catch { }
		}
		public IEnumerator<float> Clear()
		{
			while (Plugin.ServerID == 3 || Plugin.ServerID == 4)
			{
				yield return Timing.WaitForSeconds(305);
				Map.Broadcast(10, "<size=25%><color=#6f6f6f>Cовет О5 приказал <color=red>уничтожить</color> <color=#0089c7>все незначительные объекты</color>, <color=#0089c7>а так же трупы</color>,\n<color=lime>ввиду сохранности секретности комплекса</color>.</color></size>", Broadcast.BroadcastFlags.Normal);
				foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>().Where(x => x.itemId != ItemType.SCP268 && x.itemId != ItemType.SCP207 && x.itemId != ItemType.SCP500 && x.itemId != ItemType.MicroHID && x.itemId != ItemType.Medkit && x.itemId != ItemType.KeycardO5 && x.itemId != ItemType.KeycardFacilityManager && x.itemId != ItemType.KeycardContainmentEngineer && x.itemId != ItemType.GunLogicer && x.itemId != ItemType.GunE11SR && x.durability != plugin.armor.jugarmor.durability && x.durability != 100000 && x.durability != 10000 && x.durability != plugin.armor.jugarmor?.durability && x.durability != 6574356 && x.durability != scp035.EventHandlers.dur && x.durability != scp228.EventHandlers228.dur)) item.Delete();
				foreach (Ragdoll doll in UnityEngine.Object.FindObjectsOfType<Ragdoll>()) NetworkServer.Destroy(doll.gameObject);
			}
		}
		public IEnumerator<float> ChopperThread()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(600);
				RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
				Map.Broadcast(5, "<size=25%><color=#6f6f6f>Cовет О5 отправил <color=lime>вертолет с припасами</color>.</color></size>", Broadcast.BroadcastFlags.Normal);
				yield return Timing.WaitForSeconds(15f);
				Vector3 randomSpawnPoint = Map.GetRandomSpawnPoint(RoleType.NtfCadet);
				foreach (KeyValuePair<ItemType, int> keyValuePair in allowedItems)
				{
					for (int i = 0; i < keyValuePair.Value; i++)
					{
						Extensions.SpawnItem(keyValuePair.Key, (float)Extensions.ItemDur(keyValuePair.Key), randomSpawnPoint);
					}
				}
				yield return Timing.WaitForSeconds(15f);
			}
		}
		public Dictionary<ItemType, int> allowedItems = new Dictionary<ItemType, int>
		{
			{
				ItemType.GunE11SR,
				1
			},
			{
				ItemType.Medkit,
				3
			},
			{
				ItemType.Adrenaline,
				2
			},
			{
				ItemType.Ammo762,
				2
			}
		};
	}
}