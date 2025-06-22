using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.logs;
using NorthwoodLib.Pools;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MongoDB
{
	[Serializable]
	public class dictdonate
	{
		public string UserId;
		public int money;
		public int xp;
		public int lvl;
		public int to;
		public bool ytr;
		public bool don;
		public bool pr;
		public bool vr;
		public bool vpr;
		public bool rr;
		public bool aa;
		public bool sr;
		public bool hr;
		public bool ghr;
		public bool ar;
		public bool gar;
		public bool asr;
		public bool dcr;
		public bool or;

		public string prefix;
		public bool find;
		public bool anonym;
		public int time;
		public int admintime;
		public DateTime now;
		public string warns;
		public string name;
	}
	[Serializable]
	public class Ra_Cfg
	{
		public string UserId;
		public bool force = false;
		public bool give = false;
		public bool effects = false;
		public bool players_roles = false;
		public DateTime now = DateTime.Now;
	}
	public class donate
	{
		private readonly Plugin plugin;
		public donate(Plugin plugin) => this.plugin = plugin;
		private readonly Regex regexSmartSiteReplacer = new Regex(@"#" + Configs.pn);
		public Dictionary<string, dictdonate> main = new Dictionary<string, dictdonate>();
		public Dictionary<string, Ra_Cfg> Donate = new Dictionary<string, Ra_Cfg>();
		public Dictionary<string, int> force = new Dictionary<string, int>();
		public Dictionary<string, int> giveway = new Dictionary<string, int>();
		private Dictionary<string, DateTime> effect = new Dictionary<string, DateTime>();
		public Dictionary<string, bool> giver = new Dictionary<string, bool>();
		public Dictionary<string, bool> forcer = new Dictionary<string, bool>();
		public Dictionary<string, bool> effecter = new Dictionary<string, bool>();
		public Dictionary<string, bool> scp_play = new Dictionary<string, bool>();
		private bool update = false;
		internal bool contain = false;
		private bool Waititng = false;
		public void WaitingForPlayers()
		{
			contain = false;
			update = true;
			Waititng = true;
			try { main.Clear(); } catch { }
			try { Donate.Clear(); } catch { }
			try { giveway.Clear(); } catch { }
			try { force.Clear(); } catch { }
			try { effect.Clear(); } catch { }
			try { giver.Clear(); } catch { }
			try { forcer.Clear(); } catch { }
			try { effecter.Clear(); } catch { }
			try { scp_play.Clear(); } catch { }
			AddReserve();
		}
		public void RoundStarted() => Waititng = false;
		public void PlayerDeath(DyingEventArgs ev)
		{
			try
			{
				ReferenceHub target = ev.Target.ReferenceHub;
				ReferenceHub killer = ev.Killer.ReferenceHub;
				string targetname = ev.Target?.Nickname;
				try { setprefix(target); } catch { }
				if (target == null || string.IsNullOrEmpty(target.characterClassManager.UserId))
					return;

				if (killer == null || string.IsNullOrEmpty(killer.characterClassManager.UserId))
					return;

				int cxp = main[killer.characterClassManager.UserId].xp;
				if (killer != target)
				{
					ev.Killer.ClearBroadcasts();
					if (killer.GetTeam() == Team.CHI)
					{
						main[killer.characterClassManager.UserId].money += 5;
						if (target.GetTeam() == Team.MTF)
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.RSC)
						{
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.SCP)
						{
							cxp += 75;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "75").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.TUT)
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer.GetTeam() == Team.TUT)
					{
						main[killer.characterClassManager.UserId].money += 3;
						if (target.GetTeam() == Team.CDP)
						{
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.RSC)
						{
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.MTF)
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.CHI)
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.SCP)
						{
							cxp += 10;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "10").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer.GetTeam() == Team.RSC)
					{
						main[killer.characterClassManager.UserId].money += 7;
						if (target.GetTeam() == Team.CDP)
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.CHI)
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.TUT)
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.SCP)
						{
							cxp += 200;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer.GetTeam() == Team.CDP)
					{
						main[killer.characterClassManager.UserId].money += 7;
						if (target.GetTeam() == Team.RSC)
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.MTF)
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.SCP)
						{
							cxp += 200;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target.GetTeam() == Team.TUT)
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer.GetTeam() == Team.SCP)
					{
						main[killer.characterClassManager.UserId].money += 3;
						cxp += 25;
						killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%money%", "7").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
					}
				}
				string nick = ev.Killer?.Nickname.ToLower();
				MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
				if (matches.Count > 0)
				{
					if (killer != target)
					{
						ev.Killer.ClearBroadcasts();
						if (killer.GetTeam() == Team.CHI)
						{
							main[killer.characterClassManager.UserId].money += 5;
							if (target.GetTeam() == Team.MTF)
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.RSC)
							{
								cxp += 25;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.SCP)
							{
								cxp += 75;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "150").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.TUT)
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer.GetTeam() == Team.TUT)
						{
							main[killer.characterClassManager.UserId].money += 3;
							if (target.GetTeam() == Team.CDP)
							{
								cxp += 25;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.RSC)
							{
								cxp += 25;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.MTF)
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.CHI)
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.SCP)
							{
								cxp += 10;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "20").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer.GetTeam() == Team.RSC)
						{
							main[killer.characterClassManager.UserId].money += 7;
							if (target.GetTeam() == Team.CDP)
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.CHI)
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.TUT)
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.SCP)
							{
								cxp += 200;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "400").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer.GetTeam() == Team.CDP)
						{
							main[killer.characterClassManager.UserId].money += 7;
							if (target.GetTeam() == Team.RSC)
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.MTF)
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.SCP)
							{
								cxp += 200;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "400").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target.GetTeam() == Team.TUT)
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer.GetTeam() == Team.SCP)
						{
							main[killer.characterClassManager.UserId].money += 3;
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
				}
				main[killer.characterClassManager.UserId].xp = cxp;
				AddXP(killer);
			}
			catch { }
		}
		public void RoundEnd(RoundEndedEventArgs ev)
		{
			contain = false;
			update = false;
			List<ReferenceHub> players = Extensions.GetHubs().ToList();
			foreach (ReferenceHub player in players)
			{
				try
				{
					string nick = player.nicknameSync.DisplayName.ToLower();
					MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
					if (matches.Count > 0)
					{
						main[player.characterClassManager.UserId].xp += 200;
						player.Broadcast("<color=#fdffbb>Вы получили <color=red>200xp</color>, т.к в вашем нике есть <color=#0089c7>#fydne</color>!</color>", 10);
						AddXP(player);
					}
				}
				catch { }
			}
			Plugin.Coroutines.Add(Timing.RunCoroutine(endupdate()));
			giveway.Clear();
			force.Clear();
			effect.Clear();
		}
		public IEnumerator<float> endupdate()
		{
			var client = new MongoClient(plugin.mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var stata = database.GetCollection<BsonDocument>("stats");
			List<ReferenceHub> players = Extensions.GetHubs().ToList();
			foreach (ReferenceHub player in players)
			{
				try
				{
					var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
					if (main[player.characterClassManager.UserId].find)
					{
						var diff = DateTime.Now - main[player.characterClassManager.UserId].now;
						var game = diff.TotalSeconds;
						int min = (int)(main[player.characterClassManager.UserId].time + (int)game);
						int adminmin = (int)(main[player.characterClassManager.UserId].admintime + (int)game);
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", adminmin));
					}
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", main[player.characterClassManager.UserId].xp));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", main[player.characterClassManager.UserId].lvl));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", main[player.characterClassManager.UserId].money));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", main[player.characterClassManager.UserId].to));
				}
				catch { }
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public void Ban(BanningEventArgs ev)
		{
			DateTime ExpireDate = DateTime.Now.AddSeconds(ev.Duration);
			if (ev.Issuer.Nickname != "Dedicated Server")
			{
				try
				{
					var client = new MongoClient(plugin.mongodburl);
					var database = client.GetDatabase("login");
					var collection = database.GetCollection<BsonDocument>("accounts");
					var statsa = collection.Find(new BsonDocument("steam", ev.Issuer.UserId.Replace("@steam", ""))).ToList();
					foreach (var document in statsa)
					{
						int bc = (int)document["bans"];
						bc++;
						var filterr = Builders<BsonDocument>.Filter.Eq("user", document["user"]);
						collection.UpdateOne(filterr, Builders<BsonDocument>.Update.Set("bans", bc));
					}
					var filter = Builders<BsonDocument>.Filter.Eq("steam", ev.Target.UserId.Replace("@steam", ""));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("ban", ExpireDate.ToString("dd.MM.yyyy HH:mm")));
				}
				catch (Exception e) { Log.Info(e); }
				string reason = "";
				if (ev.Reason != "")
				{
					reason = $"<color=#00ffff>Причина</color>: <color=red>{ev.Reason}</color>";
				}
				Map.Broadcast(5, $"<color=#00ffff>{ev.Target.Nickname}</color> <color=red>забанен</color> <color=#00ffff>{ev.Issuer.Nickname}</color> <color=red>до</color> <color=#00ffff> {ExpireDate.ToString("dd.MM.yyyy HH:mm")}</color> {reason}");
				string banner = ev.Issuer.Nickname;
				try
				{
					send.sendban(ev.Reason, $"{ev.Target.Nickname}({ev.Target.UserId})", banner, ExpireDate.ToString("dd.MM.yyyy HH:mm"));
				}
				catch { }
			}

		}
		public void Kick(KickingEventArgs ev)
		{
			var client = new MongoClient(plugin.mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var statsa = collection.Find(new BsonDocument("steam", ev.Issuer.UserId.Replace("@steam", ""))).ToList();
			foreach (var document in statsa)
			{
				int kc = (int)document["kicks"];
				kc++;
				var filter = Builders<BsonDocument>.Filter.Eq("user", document["user"]);
				collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("kicks", kc));
			}
			string reason = "";
			if (ev.Reason != "")
			{
				reason = $"<color=#00ffff>Причина</color>: <color=red>{ev.Reason}</color>";
			}
			Map.Broadcast(5, $"<color=#00ffff>{ev.Target.Nickname}</color> <color=red>кикнут</color> <color=#00ffff>{ev.Issuer.Nickname}</color>. {reason}");
			string banner = "Discord";
			if (ev.Issuer.Nickname != "Dedicated Server")
			{
				banner = ev.Issuer.Nickname;
			}
			try
			{
				send.sendban(ev.Reason, ev.Target.Nickname, banner, "kick");
			}
			catch { }
		}
		public void Left(LeftEventArgs ev)
		{
			try
			{
				if (update)
				{
					ReferenceHub player = ev.Player.ReferenceHub;
					var client = new MongoClient(plugin.mongodburl);
					var database = client.GetDatabase("login");
					var collection = database.GetCollection<BsonDocument>("accounts");
					var stata = database.GetCollection<BsonDocument>("stats");
					var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", main[player.characterClassManager.UserId].xp));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", main[player.characterClassManager.UserId].lvl));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", main[player.characterClassManager.UserId].money));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", main[player.characterClassManager.UserId].to));
					main.Remove(player.characterClassManager.UserId);
					if (main[player.characterClassManager.UserId].find)
					{
						var diff = DateTime.Now - main[player.characterClassManager.UserId].now;
						var game = diff.TotalSeconds;
						int min = (int)(main[player.characterClassManager.UserId].time + (int)game);
						int adminmin = (int)(main[player.characterClassManager.UserId].admintime + (int)game);
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", adminmin));
					}
					main.Remove(player.characterClassManager.UserId);
				}
			}
			catch { }
		}
		public void PlayerJoin(JoinedEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			ev.Player.ReferenceHub.SetRank("1 уровень", "green");
			bool bcs = false;
			try
			{
				Timing.CallDelayed(100f, () =>
				{
					if (!bcs)
					{
						bcs = true;
						ev.Player.ReferenceHub.Broadcast(Configs.jm, 15);
					}
				});
			}
			catch { }
			if (!force.ContainsKey(ev.Player.UserId)) force.Add(ev.Player.UserId, 0);
			if (!giveway.ContainsKey(ev.Player.UserId)) giveway.Add(ev.Player.UserId, 0);
			if (!effect.ContainsKey(ev.Player.UserId)) effect.Add(ev.Player.UserId, DateTime.Now);
			if (forcer.ContainsKey(ev.Player.UserId)) forcer[ev.Player.UserId] = false;
			else forcer.Add(ev.Player.UserId, false);
			if (giver.ContainsKey(ev.Player.UserId)) giver[ev.Player.UserId] = false;
			else giver.Add(ev.Player.UserId, false);
			if (effecter.ContainsKey(ev.Player.UserId)) effecter[ev.Player.UserId] = false;
			else effecter.Add(ev.Player.UserId, false);
			if (!scp_play.ContainsKey(ev.Player.UserId)) scp_play.Add(ev.Player.UserId, false);
			try
			{
				mongoadd(ev.Player.ReferenceHub);
			}
			catch
			{
				try
				{
					errormongoadd(ev.Player.ReferenceHub);
				}
				catch
				{
					errormongoaddfatal(ev.Player.ReferenceHub);
				}
			}
		}
		public void PlayerSpawn(SpawningEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;

			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(ev.Player.ReferenceHub);
			});
		}
		public void CheckEscape(EscapingEventArgs ev)
		{
			if (ev.IsAllowed)
			{
				if (ev.NewRole == RoleType.NtfCadet) ev.NewRole = RoleType.NtfLieutenant;
				try
				{
					int cxp = main[ev.Player.ReferenceHub.characterClassManager.UserId].xp;

					ev.Player.ClearBroadcasts();
					cxp += 100;
					main[ev.Player.ReferenceHub.characterClassManager.UserId].money += 10;
					ev.Player.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(ev.Player.ReferenceHub.scp079PlayerScript.connectionToClient, Configs.eb.Replace("%xp%", "100").Replace("%money%", "10"), 10, 0);
					string nick = ev.Player?.Nickname.ToLower();
					MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
					if (matches.Count > 0)
					{
						ev.Player.ClearBroadcasts();
						cxp += 100;
						main[ev.Player.ReferenceHub.characterClassManager.UserId].money += 10;
						ev.Player.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(ev.Player.ReferenceHub.scp079PlayerScript.connectionToClient, Configs.eb.Replace("%xp%", "200").Replace("%money%", "20"), 10, 0);
					}
					main[ev.Player.ReferenceHub.characterClassManager.UserId].xp = cxp;
					AddXP(ev.Player.ReferenceHub);
				}
				catch { }
			}
		}
		public void PocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			try
			{
				if (ev.IsAllowed &&
					ev.Player.ReferenceHub.queryProcessor.PlayerId != Extensions.TryGet343()?.queryProcessor.PlayerId &&
					ev.Player.ReferenceHub.queryProcessor.PlayerId != Extensions.TryGet228()?.queryProcessor.PlayerId &&
					ev.Player.Role != RoleType.Tutorial)
				{
					Player pscp106 = Player.List.Where(x => x.ReferenceHub.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
					ReferenceHub scp106 = pscp106.ReferenceHub;
					int cxp = main[scp106.characterClassManager.UserId].xp;
					cxp += 25;
					main[scp106.characterClassManager.UserId].money += 3;
					pscp106.ClearBroadcasts();
					scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "25").Replace("%player%", $"{ev.Player?.Nickname}"), 10, 0);
					string nick = pscp106?.Nickname.ToLower();
					MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
					if (matches.Count > 0)
					{
						main[scp106.characterClassManager.UserId].money += 3;
						pscp106.ClearBroadcasts();
						cxp += 25;
						scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{ev.Player?.Nickname}"), 10, 0);
					}
					main[scp106.characterClassManager.UserId].xp = cxp;
					AddXP(scp106);
				}
			}
			catch { }
		}
		internal void Dead(DiedEventArgs ev)
		{
			if (ev.Target.Team == Team.SCP)
			{
				if (scp_play.ContainsKey(ev.Target.UserId)) scp_play[ev.Target.UserId] = true;
				else scp_play.Add(ev.Target.UserId, true);
			}
		}
		public void Console(SendingConsoleCommandEventArgs ev)
		{
			try
			{
				string cmd = ev.Name;
				if (cmd == "money" || cmd == "мани" || cmd == "баланс" || cmd == "xp" || cmd == "lvl" || cmd == Configs.cc)
				{
					ev.IsAllowed = false;
					ev.ReturnMessage = $"\n----------------------------------------------------------- \nXP:\n{main[ev.Player.ReferenceHub.characterClassManager.UserId].xp}/{main[ev.Player.ReferenceHub.characterClassManager.UserId].to}\nlvl:\n{main[ev.Player.ReferenceHub.characterClassManager.UserId].lvl}\nБаланс:\n{main[ev.Player.ReferenceHub.characterClassManager.UserId].money}\n-----------------------------------------------------------";
					ev.Color = "red";
				}
				if (cmd == "pay" || cmd == "пей" || cmd == "пэй")
				{
					ev.IsAllowed = false;
					if (ev.Arguments.Count < 2)
					{
						ev.ReturnMessage = $"Ошибка! Пример: {ev.Name[0]} 10 hmm";
						ev.Color = "red";
						return;
					}
					if (!int.TryParse(ev.Arguments[0], out int result))
					{
						ev.ReturnMessage = "Введите корректное кол-во монет";
						ev.Color = "red";
						return;
					}
					ReferenceHub rh = Extensions.GetPlayer(string.Join(" ", ev.Arguments.Skip(1)));
					if (rh == null)
					{
						ev.ReturnMessage = "Игрок не найден.";
						ev.Color = "red";
						return;
					}
					if (main[ev.Player.ReferenceHub.characterClassManager.UserId].money >= result)
					{
						main[ev.Player.ReferenceHub.characterClassManager.UserId].money -= result;
						main[rh.characterClassManager.UserId].money += result;
						ev.ReturnMessage = $"Вы успешно передали {result} монет игроку {rh.GetNickname()}.";
						ev.Color = "green";
						rh.ClearBroadcasts();
						rh.Broadcast($"<size=20><color=lime>{rh.GetNickname()} передал вам {result} монет</color></size>", 5);
						return;
					}
					ev.ReturnMessage = $"Недостаточно средств({main[ev.Player.ReferenceHub.characterClassManager.UserId].money}/{result}).";
					ev.Color = "red";
					return;
				}
			}
			catch
			{
				ev.ReturnMessage = "\nПроизошла ошибка, повторите попытку позже!";
				ev.Color = "red";
			}
		}
		public void Ra(SendingRemoteAdminCommandEventArgs ev)
		{
			string[] command = ev.Arguments.ToArray();
			CommandSender send = ev.CommandSender;
			try { if (ev.Name == "reserve_update" && (ev.CommandSender.SenderId == "SERVER CONSOLE" || ev.CommandSender.SenderId == "GAME CONSOLE")) AddReserve(); } catch { }
			if (ev.Name == "give")
			{
				try
				{
					if (giver[ev.CommandSender.SenderId])
					{
						ev.IsAllowed = false;
						if (Waititng)
						{
							ev.ReplyMessage = "RA#увы, но сейчас - ожидание игроков";
							return;
						}
						try
						{
							if (ev.Sender.Id == scp228.EventHandlers228.scp228ruj?.queryProcessor.PlayerId || ev.Sender.Id == scp343.EventHandlers343.scp343?.queryProcessor.PlayerId)
							{
								ev.ReplyMessage = "GIVE#Увы, но не сейчас, вы - кастомный scp";
								return;
							}
							if (giveway[ev.Sender.UserId] < 3)
							{
								var item = -1;
								if (command.Length > 1)
								{
									try { item = Convert.ToInt32(command[1]); } catch { }
								}
								else
								{
									try { item = Convert.ToInt32(command[0]); } catch { }
								}
								if (item == -1 || item > 35)
								{
									ev.ReplyMessage = "GIVE#Либо ты криво написал команду, либо ты попытался выдать кривой предмет";
									return;
								}
								if (item == 16)
								{
									ev.ReplyMessage = "GIVE#Пылесос только 1";
									ev.IsAllowed = false;
									return;
								}
								else if (item == 11)
								{
									ev.ReplyMessage = "GIVE#Черная карта-слишком";
									ev.IsAllowed = false;
									return;
								}
								else if (ev.Sender.Role == RoleType.ClassD)
								{
									if (item == 13 || item == 17 || item == 20 || item == 21 || item == 23 || item == 24 || item == 25 || item == 30 || item == 31 || item == 32 || item == 6 || item == 7 || item == 8 || item == 9 || item == 10)
									{
										if (!RoundSummary.RoundInProgress() || 180 >= RoundSummary.roundTime)
										{
											ev.ReplyMessage = "GIVE#3 минуты не прошло";
											ev.IsAllowed = false;
											return;
										}
									}
								}
								else if (ev.Sender.Role == RoleType.Scientist)
								{
									if (item == 13 || item == 17 || item == 20 || item == 21 || item == 23 || item == 24 || item == 25 || item == 30 || item == 31 || item == 32 || item == 6 || item == 7 || item == 8 || item == 9 || item == 10)
									{
										if (!RoundSummary.RoundInProgress() || 300 >= RoundSummary.roundTime)
										{
											ev.ReplyMessage = "GIVE#5 минут не прошло";
											ev.IsAllowed = false;
											return;
										}
									}
								}
								ev.Sender.AddItem((ItemType)item);
								ev.ReplyMessage = "GIVE#Успешно";
								giveway[ev.Sender.UserId]++;
							}
							else
							{
								ev.ReplyMessage = "GIVE#Вы уже выдали 3 предмета";
								ev.IsAllowed = false;
								return;
							}
						}
						catch (Exception e)
						{
							ev.ReplyMessage = $"GIVE#Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
						}
					}
					else if (main[ev.Sender.UserId].sr)
					{
						ev.IsAllowed = false;
						if (command[1] == "16")
						{
							ev.ReplyMessage = ("GIVE#Пылесос только 1");
							ev.IsAllowed = false;
							return;
						}
						else if (command[1] == "11")
						{
							ev.ReplyMessage = ("GIVE#Черная карта-слишком");
							ev.IsAllowed = false;
							return;
						}
						GameCore.Console.singleton.TypeCommand($"/GIVE {command[0]} {command[1]}");
						ev.ReplyMessage = ("GIVE#Успешно");
					}
				}
				catch
				{
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					try
					{
						bool admin = main[ev.Sender.UserId].sr || main[ev.Sender.UserId].hr
							|| main[ev.Sender.UserId].ghr || main[ev.Sender.UserId].ar
							|| main[ev.Sender.UserId].gar || main[ev.Sender.UserId].or;
						if (admin) return;
					}
					catch { }
					ev.IsAllowed = false;
					ev.ReplyMessage = "GIVE#Произошла ошибка, повторите попытку позже";
				}
			}
			if (ev.Name == "forceclass")
			{
				try
				{
					if (forcer[ev.CommandSender.SenderId])
					{
						ev.IsAllowed = false;
						try
						{
							if (ev.Sender.Id == scp228.EventHandlers228.scp228ruj?.queryProcessor.PlayerId) plugin.EventHandlers228.Killscp228ruj();
							if (ev.Sender.Id == scp343.EventHandlers343.scp343?.queryProcessor.PlayerId) plugin.EventHandlers343.Killscp343();
							if (force[ev.Sender.UserId] < 3)
							{
								var role = -1;
								if (command.Length > 1)
								{
									try { role = Convert.ToInt32(command[1]); } catch { }
								}
								else
								{
									try { role = Convert.ToInt32(command[0]); } catch { }
								}
								if (role == -1 || role > 17)
								{
									ev.ReplyMessage = "FORCECLASS#Либо ты криво написал команду, либо ты попытался заспавнить себя за кривую команду";
									return;
								}
								int Scp173 = Player.List.Where(x => x.Role == RoleType.Scp173).ToList().Count;
								int Scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).ToList().Count;
								int Scp049 = Player.List.Where(x => x.Role == RoleType.Scp049).ToList().Count;
								int Scp079 = Player.List.Where(x => x.Role == RoleType.Scp079).ToList().Count;
								int Scp096 = Player.List.Where(x => x.Role == RoleType.Scp096).ToList().Count;
								int Scp93989 = Player.List.Where(x => x.Role == RoleType.Scp93989).ToList().Count;
								int Scp93953 = Player.List.Where(x => x.Role == RoleType.Scp93953).ToList().Count;
								try
								{
									if (Extensions.GetTeam((Modules.RoleType)role) == Team.SCP)
									{
										if (!scp_play.ContainsKey(ev.Sender.UserId)) scp_play.Add(ev.Sender.UserId, false);
										if (scp_play[ev.Sender.UserId])
										{
											ev.ReplyMessage = "FORCECLASS#Вы уже играли за SCP";
											return;
										}
									}
								}
								catch { }
								if ((RoleType)role == RoleType.Scp93953)
								{
									if (Scp93953 > 0)
									{
										ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
										return;
									}
								}
								else if ((RoleType)role == RoleType.Scp93989)
								{
									if (Scp93989 > 0)
									{
										ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
										return;
									}
								}
								else if ((RoleType)role == RoleType.Scp096)
								{
									if (Scp096 > 0)
									{
										ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
										return;
									}
								}
								else if ((RoleType)role == RoleType.Scp079)
								{
									if (Scp079 > 0)
									{
										ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
										return;
									}
								}
								else if ((RoleType)role == RoleType.Scp049)
								{
									if (Scp049 > 0)
									{
										ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
										return;
									}
								}
								else if ((RoleType)role == RoleType.Scp106)
								{
									if (Scp106 > 0)
									{
										ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
										return;
									}
									else if (contain)
									{
										ev.ReplyMessage = "FORCECLASS#Условия содержания SCP106 уже восстановлены";
										return;
									}
								}
								else if ((RoleType)role == RoleType.Scp173)
								{
									if (Scp173 > 0)
									{
										ev.ReplyMessage = "FORCECLASS#Этот SCP уже есть";
										return;
									}
								}
								else if ((RoleType)role == RoleType.Tutorial)
								{
									ev.ReplyMessage = "FORCECLASS#Не забывайте, что у вас есть только 3 спавна";
									plugin.shEventHandlers.spawnonesh(ev.Sender);
									force[ev.Sender.UserId]++;
									return;
								}
								else if ((RoleType)role == RoleType.NtfScientist)
								{
									ev.ReplyMessage = "FORCECLASS#Увы, но нет";
									return;
								}
								ev.ReplyMessage = "FORCECLASS#Не забывайте, что у вас есть только 3 спавна";
								ev.Sender.SetRole((RoleType)role);
								force[ev.Sender.UserId]++;
							}
							else
							{
								ev.ReplyMessage = "FORCECLASS#Спавниться более трех раз ЗАПРЕЩЕНО";
							}
						}
						catch (Exception e)
						{
							ev.ReplyMessage = $"FORCECLASS#Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
						}
					}
				}
				catch
				{
					Log.Info(ev.CommandSender.SenderId);
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					try
					{
						bool admin = main[ev.Sender.UserId].sr || main[ev.Sender.UserId].hr
							|| main[ev.Sender.UserId].ghr || main[ev.Sender.UserId].ar
							|| main[ev.Sender.UserId].gar || main[ev.Sender.UserId].or;
						if (admin) return;
					}
					catch { }
					ev.IsAllowed = false;
					ev.ReplyMessage = "FORCECLASS#Произошла ошибка, повторите попытку позже";
				}
			}
			if (ev.Name == "effect")
			{
				try
				{
					if (effecter[ev.CommandSender.SenderId])
					{
						try
						{
							double CoolDown = 3;
							if ((DateTime.Now - effect[ev.CommandSender.SenderId]).TotalSeconds > 0)
							{
								effect[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
							}
							else
							{
								ev.IsAllowed = false;
								ev.ReplyMessage = $"EFFECT#Эффекты можно использовать раз в {CoolDown} минут";
							}
						}
						catch (Exception e)
						{
							ev.IsAllowed = false;
							ev.ReplyMessage = $"EFFECT#Произошла какая-то ошибка, открой тикет в дискорде, скинув следующий код ошибки:\n{e.Message}";
						}
					}
				}
				catch
				{
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					try
					{
						bool admin = main[ev.Sender.UserId].sr || main[ev.Sender.UserId].hr
							|| main[ev.Sender.UserId].ghr || main[ev.Sender.UserId].ar
							|| main[ev.Sender.UserId].gar || main[ev.Sender.UserId].or;
						if (admin) return;
					}
					catch { }
					ev.IsAllowed = false;
					ev.ReplyMessage = "EFFECT#Произошла ошибка, повторите попытку позже";
				}
			}
			if (ev.Name == "hidetag")
			{
				ev.IsAllowed = false;
				ev.ReplyMessage = "HIDETAG#Недоступно";
			}
		}





		public void AddXP(ReferenceHub player)
		{
			int cxp = main[player.characterClassManager.UserId].xp;
			int cto = main[player.characterClassManager.UserId].to;
			int clvl = main[player.characterClassManager.UserId].lvl;
			if (cxp >= cto)
			{
				cxp -= cto;
				clvl++;
				cto = clvl * 250 + 750;
				player.Broadcast(Configs.lvlup.Replace("%lvl%", $"{clvl}").Replace("%to.xp%", ((cto) - cxp).ToString()), 10);
				main[player.characterClassManager.UserId].xp = cxp;
				main[player.characterClassManager.UserId].to = cto;
				main[player.characterClassManager.UserId].lvl = clvl;
				setprefix(player);
			}
		}
		public void mongoadd(ReferenceHub player)
		{
			#region create data
			var client = new MongoClient(plugin.mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var stata = database.GetCollection<BsonDocument>("stats");
			var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
			var list = collection.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			var statsa = stata.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			if (statsa.Count == 0)
			{
				var sta = database.GetCollection<Person>("stats");
				sta.InsertOneAsync(new Person { steam = player.characterClassManager.UserId.Replace("@steam", "") });
				main.Add(player.characterClassManager.UserId, LoadSDonate(player.characterClassManager.UserId));
			}
			else
			{
				foreach (var document in statsa)
				{
					int xpd = (int)document["xp"];
					int tod = (int)document["to"];
					int lvld = (int)document["lvl"];
					int moneyd = (int)document["money"];
					main.Add(player.characterClassManager.UserId, LoadSDonate(player.characterClassManager.UserId));
					main[player.characterClassManager.UserId].xp = xpd;
					main[player.characterClassManager.UserId].lvl = lvld;
					main[player.characterClassManager.UserId].money = moneyd;
					main[player.characterClassManager.UserId].to = tod;
				}
			}
			foreach (var document in list)
			{
				int t = (int)document["min"];
				int at = (int)document["adminmin"];
				string prefd = (string)document["prefix"];
				main[player.characterClassManager.UserId].time = t;
				main[player.characterClassManager.UserId].admintime = at;
				main[player.characterClassManager.UserId].now = DateTime.Now;
				main[player.characterClassManager.UserId].prefix = prefd;
				main[player.characterClassManager.UserId].find = true;
				main[player.characterClassManager.UserId].anonym = (bool)document["anonym"];
				main[player.characterClassManager.UserId].pr = (bool)document["pe"];
				main[player.characterClassManager.UserId].vr = (bool)document["ve"];
				main[player.characterClassManager.UserId].vpr = (bool)document["vpe"];
				main[player.characterClassManager.UserId].rr = (bool)document["re"];
				main[player.characterClassManager.UserId].aa = (bool)document["aae"];
				main[player.characterClassManager.UserId].sr = (bool)document["sr"];
				main[player.characterClassManager.UserId].hr = (bool)document["hr"];
				main[player.characterClassManager.UserId].ghr = (bool)document["ghr"];
				main[player.characterClassManager.UserId].ar = (bool)document["ar"];
				main[player.characterClassManager.UserId].gar = (bool)document["gar"];
				main[player.characterClassManager.UserId].asr = (bool)document["asr"];
				main[player.characterClassManager.UserId].dcr = (bool)document["dcr"];
				main[player.characterClassManager.UserId].or = (bool)document["or"];
				main[player.characterClassManager.UserId].name = (string)document["user"];
				try
				{
					if ((int)document["warnings"] != 0)
					{
						main[player.characterClassManager.UserId].warns = "| " + (int)document["warnings"] + " пред(а) ";
					}
				}
				catch
				{ }
			}
			#endregion
			#region set prefix
			Timing.CallDelayed(1f, () =>
			{
				spawnpref(player, list, collection);
			});
			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(player);
			});
			#endregion
		}
		public void errormongoadd(ReferenceHub player)
		{
			#region delete data
			var client = new MongoClient(plugin.mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var stata = database.GetCollection<BsonDocument>("stats");
			var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", main[player.characterClassManager.UserId].xp));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", main[player.characterClassManager.UserId].lvl));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", main[player.characterClassManager.UserId].money));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", main[player.characterClassManager.UserId].to));
			if (main[player.characterClassManager.UserId].find)
			{
				int min = 0;
				try
				{
					var diff = DateTime.Now - main[player.characterClassManager.UserId].now;
					var game = diff.TotalSeconds;
					min = (int)(main[player.characterClassManager.UserId].time + (int)game);
					int adminmin = (int)(main[player.characterClassManager.UserId].admintime + (int)game);
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", adminmin));
				}
				catch { }
			}
			main.Remove(player.characterClassManager.UserId);
			#endregion
			#region create data
			var list = collection.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			var statsa = stata.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			main.Add(player.characterClassManager.UserId, LoadSDonate(player.characterClassManager.UserId));
			if (statsa.Count == 0)
			{
				var sta = database.GetCollection<Person>("stats");
				sta.InsertOneAsync(new Person { steam = player.characterClassManager.UserId.Replace("@steam", "") });
			}
			else
			{
				foreach (var document in statsa)
				{
					int xpd = (int)document["xp"];
					int tod = (int)document["to"];
					int lvld = (int)document["lvl"];
					int moneyd = (int)document["money"];
					main[player.characterClassManager.UserId].xp = xpd;
					main[player.characterClassManager.UserId].lvl = lvld;
					main[player.characterClassManager.UserId].money = moneyd;
					main[player.characterClassManager.UserId].to = tod;
				}
			}
			foreach (var document in list)
			{
				main[player.characterClassManager.UserId].time = (int)document["min"];
				main[player.characterClassManager.UserId].admintime = (int)document["adminmin"];
				main[player.characterClassManager.UserId].now = DateTime.Now;
				main[player.characterClassManager.UserId].prefix = (string)document["prefix"];
				main[player.characterClassManager.UserId].find = true;
				main[player.characterClassManager.UserId].anonym = (bool)document["anonym"];
				main[player.characterClassManager.UserId].pr = (bool)document["pe"];
				main[player.characterClassManager.UserId].vr = (bool)document["ve"];
				main[player.characterClassManager.UserId].vpr = (bool)document["vpe"];
				main[player.characterClassManager.UserId].rr = (bool)document["re"];
				main[player.characterClassManager.UserId].sr = (bool)document["sr"];
				main[player.characterClassManager.UserId].hr = (bool)document["hr"];
				main[player.characterClassManager.UserId].ghr = (bool)document["ghr"];
				main[player.characterClassManager.UserId].ar = (bool)document["ar"];
				main[player.characterClassManager.UserId].gar = (bool)document["gar"];
				main[player.characterClassManager.UserId].asr = (bool)document["asr"];
				main[player.characterClassManager.UserId].dcr = (bool)document["dcr"];
				main[player.characterClassManager.UserId].or = (bool)document["or"];
				main[player.characterClassManager.UserId].name = (string)document["user"];
				try
				{
					if ((int)document["warnings"] != 0)
					{
						main[player.characterClassManager.UserId].warns = "| " + (int)document["warnings"] + " пред(а) ";
					}
				}
				catch
				{ }
			}
			#endregion
			#region set prefix
			Timing.CallDelayed(1f, () =>
			{
				spawnpref(player, list, collection);
			});
			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(player);
			});
			#endregion
		}
		public void errormongoaddfatal(ReferenceHub player)
		{
			var client = new MongoClient(plugin.mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var stata = database.GetCollection<BsonDocument>("stats");
			var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
			#region update levels
			try
			{
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", main[player.characterClassManager.UserId].xp));
			}
			catch
			{
				Log.Info("error in update xp");
			}
			try
			{
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", main[player.characterClassManager.UserId].lvl));
			}
			catch
			{
				Log.Info("error in update lvl");
			}
			try
			{
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", main[player.characterClassManager.UserId].money));
			}
			catch
			{
				Log.Info("error in update money");
			}
			try
			{
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", main[player.characterClassManager.UserId].to));
			}
			catch
			{
				Log.Info("error in update xp to");
			}
			#endregion
			#region update time
			if (main[player.characterClassManager.UserId].find)
			{
				try
				{
					int min = 0;
					var diff = DateTime.Now - main[player.characterClassManager.UserId].now;
					var game = diff.TotalSeconds;
					min = (int)(main[player.characterClassManager.UserId].time + (int)game);
					int adminmin = (int)(main[player.characterClassManager.UserId].admintime + (int)game);
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", adminmin));
				}
				catch
				{
					Log.Info("error in update time");
				}
			}
			#endregion
			#region delete data
			try
			{
				if (main.ContainsKey(player.characterClassManager.UserId)) main.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete data");
			}
			#endregion
			#region create data
			var list = collection.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			var statsa = stata.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			if (statsa.Count == 0)
			{
				var sta = database.GetCollection<Person>("stats");
				sta.InsertOneAsync(new Person { steam = player.characterClassManager.UserId.Replace("@steam", "") });
				if (!main.ContainsKey(player.characterClassManager.UserId)) main.Add(player.characterClassManager.UserId, LoadSDonate(player.characterClassManager.UserId));
			}
			else
			{
				foreach (var document in statsa)
				{
					int xpd = (int)document["xp"];
					int tod = (int)document["to"];
					int lvld = (int)document["lvl"];
					int moneyd = (int)document["money"];
					main[player.characterClassManager.UserId].xp = xpd;
					main[player.characterClassManager.UserId].lvl = lvld;
					main[player.characterClassManager.UserId].money = moneyd;
					main[player.characterClassManager.UserId].to = tod;
				}
			}
			foreach (var document in list)
			{
				int t = (int)document["min"];
				int at = (int)document["adminmin"];
				string prefd = (string)document["prefix"];
				main[player.characterClassManager.UserId].time = t;
				main[player.characterClassManager.UserId].admintime = at;
				main[player.characterClassManager.UserId].now = DateTime.Now;
				main[player.characterClassManager.UserId].prefix = prefd;
				main[player.characterClassManager.UserId].find = true;
				main[player.characterClassManager.UserId].anonym = (bool)document["anonym"];
				main[player.characterClassManager.UserId].pr = (bool)document["pe"];
				main[player.characterClassManager.UserId].vr = (bool)document["ve"];
				main[player.characterClassManager.UserId].vpr = (bool)document["vpe"];
				main[player.characterClassManager.UserId].rr = (bool)document["re"];
				main[player.characterClassManager.UserId].sr = (bool)document["sr"];
				main[player.characterClassManager.UserId].hr = (bool)document["hr"];
				main[player.characterClassManager.UserId].ghr = (bool)document["ghr"];
				main[player.characterClassManager.UserId].ar = (bool)document["ar"];
				main[player.characterClassManager.UserId].gar = (bool)document["gar"];
				main[player.characterClassManager.UserId].asr = (bool)document["asr"];
				main[player.characterClassManager.UserId].dcr = (bool)document["dcr"];
				main[player.characterClassManager.UserId].or = (bool)document["or"];
				main[player.characterClassManager.UserId].name = (string)document["user"];
				try
				{
					if ((int)document["warnings"] != 0)
					{
						main[player.characterClassManager.UserId].warns = "| " + (int)document["warnings"] + " пред(а) ";
					}
				}
				catch
				{ }
			}
			#endregion
			#region set prefix
			Timing.CallDelayed(1f, () =>
			{
				spawnpref(player, list, collection);
			});
			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(player);
			});
			#endregion
		}
		public void GetDonate(Player pl, string web_name)
		{
			var client = new MongoClient(plugin.mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("donates");
			var list = collection.Find(new BsonDocument("owner", web_name)).ToList();
			foreach (var document in list)
			{
				if((int)document["server"] == Plugin.ServerID)
				{
					Log.Info(document);
					if (!Donate.ContainsKey(pl.UserId))
					{
						Donate.Add(pl.UserId, new Ra_Cfg());
						Donate[pl.UserId].force = (bool)document["force"];
						Donate[pl.UserId].give = (bool)document["give"];
						Donate[pl.UserId].effects = (bool)document["effects"];
						Donate[pl.UserId].players_roles = (bool)document["players_roles"];
					}
					else
					{
						if (!Donate[pl.UserId].force) Donate[pl.UserId].force = (bool)document["force"];
						if (!Donate[pl.UserId].give) Donate[pl.UserId].give = (bool)document["give"];
						if (!Donate[pl.UserId].effects) Donate[pl.UserId].effects = (bool)document["effects"];
						if (!Donate[pl.UserId].players_roles) Donate[pl.UserId].players_roles = (bool)document["players_roles"];
					}
					pl.ReferenceHub.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("index"), false, true, true);
					pl.ReferenceHub.serverRoles.RemoteAdmin = true;
					if ((bool)document["force"])
					{
						if (forcer.ContainsKey(pl.UserId)) forcer[pl.UserId] = true;
						else forcer.Add(pl.UserId, true);
					}
					if ((bool)document["give"])
					{
						if (giver.ContainsKey(pl.UserId)) giver[pl.UserId] = true;
						else giver.Add(pl.UserId, true);
					}
					if ((bool)document["effects"])
					{
						if (effecter.ContainsKey(pl.UserId)) effecter[pl.UserId] = true;
						else effecter.Add(pl.UserId, true);
					}
					if (!CommandProcessor.Prefixs.ContainsKey(pl.UserId))
					{
						CommandProcessor.Prefixs.Add(pl.UserId, new CommandProcessor.ra_pref());
						CommandProcessor.Prefixs[pl.UserId].prefix = (string)document["prefix"];
						CommandProcessor.Prefixs[pl.UserId].color = (string)document["color"];
						CommandProcessor.Prefixs[pl.UserId].gameplay_data = (bool)document["players_roles"];
					}
					else
					{
						CommandProcessor.Prefixs[pl.UserId].prefix = (string)document["prefix"];
						CommandProcessor.Prefixs[pl.UserId].color = (string)document["color"];
						CommandProcessor.Prefixs[pl.UserId].gameplay_data = (bool)document["players_roles"];
					}
					try { setprefix(pl.ReferenceHub); } catch { }
					if (main.ContainsKey(pl.UserId)) main[pl.UserId].don = true;
				}
			}
		}
		public void spawnpref(ReferenceHub player, List<BsonDocument> list, IMongoCollection<BsonDocument> collection)
		{
			foreach (var document in list)
			{
				GetDonate(Player.Get(player), (string)document["user"]);
				if ((bool)document["re"])
				{
					var component = player.GetComponent<RainbowTagController>();
					if (component == null) player.gameObject.AddComponent<RainbowTagController>();
				}
				if ((bool)document["pe"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("premium"), false, true, false);
					setprefix(player);
					if (forcer.ContainsKey(Player.Get(player).UserId)) forcer[Player.Get(player).UserId] = true;
					else forcer.Add(Player.Get(player).UserId, true);
				}
				if ((bool)document["ve"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("vip"), false, true, false);
					setprefix(player);
					if (giver.ContainsKey(Player.Get(player).UserId)) giver[Player.Get(player).UserId] = true;
					else giver.Add(Player.Get(player).UserId, true);
				}
				if ((bool)document["vpe"] || ((bool)document["pe"] && (bool)document["ve"]))
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("viplus"), false, true, false);
					setprefix(player);
					if (forcer.ContainsKey(Player.Get(player).UserId)) forcer[Player.Get(player).UserId] = true;
					else forcer.Add(Player.Get(player).UserId, true);
					if (giver.ContainsKey(Player.Get(player).UserId)) giver[Player.Get(player).UserId] = true;
					else giver.Add(Player.Get(player).UserId, true);
				}
				if ((bool)document["sr"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["hr"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["ghr"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["ar"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["gar"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["asr"] || (bool)document["dcr"])
				{
					setprefix(player);
				}
				if ((bool)document["or"])
				{
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
					setprefix(player);
				}
				if (Plugin.YTAcess && (bool)document["ytr"])
				{
					main[player.characterClassManager.UserId].ytr = (bool)document["ytr"];
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("elite"), false, true, false);
					setprefix(player);
				}
			}
		}
		public void setprefix(ReferenceHub player)
		{
			string hook = "";
			try
			{
				if (player.queryProcessor.PlayerId == plugin.cat_hook.hook_owner.ReferenceHub.queryProcessor.PlayerId)
				{
					hook = " | Нашел Крюк-Кошку";
				}
			}
			catch
			{ }
			string prefix = "";
			string role = "";
			string mrole = "";
			string srole = "";
			string antiadmin = "";
			string color = "red";
			var imain = main[player.characterClassManager.UserId];
			int lvl = imain.lvl;
			try
			{
				if (lvl == 1) color = "green";
				else if (lvl == 2) color = "crimson";
				else if (lvl == 3) color = "cyan";
				else if (lvl == 4) color = "deep_pink";
				else if (lvl == 5) color = "yellow";
				else if (lvl == 6) color = "orange";
				else if (lvl == 7) color = "lime";
				else if (lvl == 8) color = "pumpkin";
				else if (lvl == 9) color = "red";
				else if (lvl >= 10 && 20 >= lvl) color = "green";
				else if (lvl >= 20 && 30 >= lvl) color = "crimson";
				else if (lvl >= 30 && 40 >= lvl) color = "cyan";
				else if (lvl >= 40 && 50 >= lvl) color = "deep_pink";
				else if (lvl >= 50 && 60 >= lvl) color = "yellow";
				else if (lvl >= 60 && 70 >= lvl) color = "orange";
				else if (lvl >= 70 && 80 >= lvl) color = "lime";
				else if (lvl >= 80 && 90 >= lvl) color = "pumpkin";
				else if (lvl >= 90 && 100 >= lvl) color = "red";
				else if (lvl >= 100) color = "red";
			}
			catch { }
			try
			{
				if (imain.rr)
				{
					color = "red";
					if (imain.prefix != "")
					{
						prefix = $" | {imain.prefix}";
					}
				}
				if (player.adminsearch())
				{
					color = ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeColor;
					role = $" | {ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeText} | {imain.prefix}";
				}
				if (imain.sr)
				{
					color = "lime";
					role = $" | Стажер {imain.warns}";
				}
				if (imain.hr)
				{
					color = "aqua";
					role = $" | Хелпер {imain.warns}";
				}
				if (imain.ghr)
				{
					color = "cyan";
					role = $" | Главный хелпер {imain.warns}";
				}
				if (imain.ar)
				{
					color = "yellow";
					role = $" | Админ {imain.warns}";
				}
				if (imain.gar)
				{
					color = "red";
					role = $" | Главный Админ {imain.warns}";
				}
				if (imain.pr)
				{
					color = "lime";
					role += $" | Premium";
				}
				if (imain.vr)
				{
					color = "yellow";
					role += $" | ViP";
				}
				if (imain.vpr)
				{
					color = "pumpkin";
					role += $" | ViP+";
				}
				if (Donate.ContainsKey(player.characterClassManager.UserId))
				{
					color = "lime";
					role += $" | Донатер";
				}
				if (imain.ytr)
				{
					color = "red";
					role += $" | YouTube";
				}
				if (imain.aa)
				{
					antiadmin = $"Защита от админов | ";
				}
				if (imain.asr)
				{
					color = "pumpkin";
					mrole += $"Набор Администрации | ";
				}
				if (imain.dcr)
				{
					color = "pumpkin";
					mrole += $"Контроль Донатеров | ";
				}
				if (player.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
				{
					color = "red";
					srole = $"SCP 228 RU J(гопник) | {srole}";
				}
				if (player.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId)
				{
					color = "red";
					srole = $"SCP 343 | {srole}";
				}
				if (player.queryProcessor.PlayerId == scp035.EventHandlers.scpPlayer?.queryProcessor.PlayerId)
				{
					color = "red";
					srole = $"SCP 035 | {srole}";
				}
			}
			catch
			{ }
			try
			{
				player.SetRank($"{srole}{antiadmin}{lvl} {Configs.lvl}{hook}{prefix}", color);
				if (imain.find && !imain.anonym) player.SetRank($"{srole}{antiadmin}{mrole}{lvl} {Configs.lvl}{hook}{role}{prefix}", color);
				if (player.characterClassManager.UserId == "-@steam") player.SetRank($"{srole}{hook}{role.Replace("|", "")}", "red");
			}
			catch { }
		}
		public void AddReserve()
		{
			try { ReservedSlot.Res.Clear(); } catch { }
			var client = new MongoClient(plugin.mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			foreach (var document in collection.Find(new BsonDocument("ree", true)).ToList()) if ((string)document["steam"] != "" && !ReservedSlot.Res.Contains($"{(string)document["steam"]}@steam")) ReservedSlot.Res.Add($"{(string)document["steam"]}@steam");
			foreach (var document in collection.Find(new BsonDocument("sr", true)).ToList()) if ((string)document["steam"] != "" && !ReservedSlot.Res.Contains($"{(string)document["steam"]}@steam")) ReservedSlot.Res.Add($"{(string)document["steam"]}@steam");
			foreach (var document in collection.Find(new BsonDocument("hr", true)).ToList()) if ((string)document["steam"] != "" && !ReservedSlot.Res.Contains($"{(string)document["steam"]}@steam")) ReservedSlot.Res.Add($"{(string)document["steam"]}@steam");
			foreach (var document in collection.Find(new BsonDocument("ghr", true)).ToList()) if ((string)document["steam"] != "" && !ReservedSlot.Res.Contains($"{(string)document["steam"]}@steam")) ReservedSlot.Res.Add($"{(string)document["steam"]}@steam");
			foreach (var document in collection.Find(new BsonDocument("ar", true)).ToList()) if ((string)document["steam"] != "" && !ReservedSlot.Res.Contains($"{(string)document["steam"]}@steam")) ReservedSlot.Res.Add($"{(string)document["steam"]}@steam");
			foreach (var document in collection.Find(new BsonDocument("gar", true)).ToList()) if ((string)document["steam"] != "" && !ReservedSlot.Res.Contains($"{(string)document["steam"]}@steam")) ReservedSlot.Res.Add($"{(string)document["steam"]}@steam");
			foreach (var document in collection.Find(new BsonDocument("or", true)).ToList()) if ((string)document["steam"] != "" && !ReservedSlot.Res.Contains($"{(string)document["steam"]}@steam")) ReservedSlot.Res.Add($"{(string)document["steam"]}@steam");
			Timing.CallDelayed(1f, () => ReservedSlot.Reload());
		}
		internal static dictdonate LoadSDonate(string userId)
		{
			return new dictdonate()
			{
				UserId = userId,
				money = 0,
				xp = 0,
				lvl = 1,
				to = 750,
				ytr = false,
				pr = false,
				vr = false,
				vpr = false,
				rr = false,
				aa = false,
				sr = false,
				hr = false,
				ghr = false,
				ar = false,
				gar = false,
				asr = false,
				dcr = false,
				or = false,
				prefix = "",
				find = false,
				anonym = false,
				time = 0,
				admintime = 0,
				now = DateTime.Now,
				warns = "",
				name = "[data deleted]",
			};
		}
		internal string Prefix(GameObject gm)
		{
			Player pl = Player.Get(gm);
			var imain = main[pl.UserId];
			if (imain.or) return "";
			else if (imain.anonym) return "";
			else if (imain.gar) return "<color=#ff0000>[MainAdmin]</color>";
			else if (imain.ar) return "<color=#fdffbb>[Admin]</color>";
			else if (imain.ghr) return "<color=#0089c7>[MainHelper]</color>";
			else if (imain.hr) return "<color=#00ffff>[Helper]</color>";
			else if (imain.sr) return "<color=#9bff00>[Trainee]</color>";
			else if (imain.vpr || (imain.vr && imain.pr)) return "<color=#ffb500>[ViP+]</color>";
			else if (imain.vr) return "<color=#ffff00>[ViP]</color>";
			else if (imain.pr) return "<color=#00ff88>[Premium]</color>";
			else return "[RA]";
		}
		internal void CustomRA(SendingRemoteAdminCommandEventArgs ev)
		{
			if (ev.Name == "request_data")
			{
				CommandSender sender = ev.CommandSender;
				string logName = sender.LogName;
				if (ev.Arguments.Count >= 1)
				{
					string text2 = ev.Arguments[0].ToUpper();
					if (!(text2 == "PLAYER_LIST"))
					{
						if (!(text2 == "PLAYER") && !(text2 == "SHORT-PLAYER"))
						{
							sender.RaReply(ev.Name.ToUpper() + ":PLAYER#Please specify the PlayerId!", false, true, "");
							return;
						}
					}
					else
					{
						ev.IsAllowed = false;
						try
						{
							string text3 = "\n";
							if (ev.Sender.ReferenceHub != null)
							{
								bool gameplayData = PermissionsHandler.IsPermitted(sender.Permissions, PlayerPermissions.GameplayData);
								ev.Sender.ReferenceHub.queryProcessor.GameplayData = gameplayData;
							}
							bool flag = ev.Arguments.Contains("STAFF", StringComparison.OrdinalIgnoreCase);
							foreach (GameObject gameObject4 in PlayerManager.players)
							{
								QueryProcessor component = gameObject4.GetComponent<QueryProcessor>();
								if (!flag)
								{
									string text4 = string.Empty;
									bool flag4 = false;
									global::ServerRoles component2 = component.GetComponent<global::ServerRoles>();
									try
									{
										if (string.IsNullOrEmpty(component2.HiddenBadge))
										{
											text4 = (component2.RaEverywhere ? "[~] " : (component2.Staff ? "[@] " : (component2.RemoteAdmin ? $"{Prefix(gameObject4)} " : string.Empty)));
										}
										flag4 = ev.Sender.IsOverwatchEnabled;
									}
									catch
									{
									}
									text3 = string.Concat(new object[]
									{
																text3,
																text4,
																"(",
																component.PlayerId,
																") ",
																component.GetComponent<global::NicknameSync>().CombinedName.Replace("\n", string.Empty),
																flag4 ? "<OVRM>" : string.Empty
									});
								}
								else
								{
									text3 = string.Concat(new object[]
									{
																text3,
																component.PlayerId,
																";",
																component.GetComponent<global::NicknameSync>().CombinedName
									});
								}
								text3 += "\n";
							}
							if (!ev.Arguments.Contains("STAFF", StringComparison.OrdinalIgnoreCase))
							{
								sender.RaReply(ev.Name.ToUpper() + ":PLAYER_LIST#" + text3, true, ev.Arguments.Count < 2 || ev.Arguments[1].ToUpper() != "SILENT", "");
							}
							else
							{
								sender.RaReply("StaffPlayerListReply#" + text3, true, ev.Arguments.Count < 2 || ev.Arguments[1].ToUpper() != "SILENT", "");
							}
							return;
						}
						catch (Exception ex2)
						{
							sender.RaReply(string.Concat(new string[]
							{
														ev.Name.ToUpper(),
														":PLAYER_LIST#An unexpected problem has occurred!\nMessage: ",
														ex2.Message,
														"\nStackTrace: ",
														ex2.StackTrace,
														"\nAt: ",
														ex2.Source
							}), false, true, "");
							throw;
						}
					}
					if (ev.Arguments.Count >= 1)
					{
						if (string.Equals(ev.Arguments[0], "PLAYER", StringComparison.OrdinalIgnoreCase) && (ev.Sender.ReferenceHub == null || !ev.Sender.ReferenceHub.serverRoles.Staff) && !PermissionsHandler.IsPermitted(sender.Permissions, global::PlayerPermissions.PlayerSensitiveDataAccess)) return;
						try
						{
							GameObject gameObject5 = null;
							NetworkConnection networkConnection = null;
							foreach (NetworkConnection networkConnection2 in NetworkServer.connections.Values)
							{
								GameObject gameObject6 = GameCore.Console.FindConnectedRoot(networkConnection2);
								if (ev.Arguments[1].Contains("."))
								{
									ev.Arguments[1] = ev.Arguments[1].Split(new char[]
									{
																'.'
									})[0];
								}
								if (!(gameObject6 == null) && !(gameObject6.GetComponent<QueryProcessor>().PlayerId.ToString() != ev.Arguments[1]))
								{
									gameObject5 = gameObject6;
									networkConnection = networkConnection2;
								}
							}
							if (gameObject5 == null)
							{
								sender.RaReply(ev.Name.ToUpper() + ":PLAYER#Player with id " + (string.IsNullOrEmpty(ev.Arguments[1]) ? "[null]" : ev.Arguments[1]) + " not found!", false, true, "");
							}
							else
							{
								bool flag5 = global::PermissionsHandler.IsPermitted(sender.Permissions, global::PlayerPermissions.GameplayData);
								bool flag6 = global::PermissionsHandler.IsPermitted(sender.Permissions, 18007046UL);
								if (ev.Sender.ReferenceHub != null)
								{
									ev.Sender.ReferenceHub.queryProcessor.GameplayData = flag5;
								}
								if (ev.Sender.ReferenceHub != null && (ev.Sender.ReferenceHub.serverRoles.Staff || ev.Sender.ReferenceHub.serverRoles.RaEverywhere))
								{
									flag6 = true;
								}
								global::ReferenceHub hub = global::ReferenceHub.GetHub(gameObject5.gameObject);
								global::CharacterClassManager characterClassManager = hub.characterClassManager;
								global::ServerRoles serverRoles = hub.serverRoles;
								if (ev.Arguments[0].ToUpper() == "PLAYER")
								{
									global::ServerLogs.AddLog(global::ServerLogs.Modules.DataAccess, string.Concat(new object[]
									{
																logName,
																" accessed IP address of player ",
																gameObject5.GetComponent<QueryProcessor>().PlayerId,
																" (",
																gameObject5.GetComponent<global::NicknameSync>().MyNick,
																")."
									}), global::ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging, false);
								}
								StringBuilder stringBuilder = StringBuilderPool.Shared.Rent("<color=white>");
								stringBuilder.Append("Ник: " + hub.nicknameSync.CombinedName);
								stringBuilder.Append("\nPlayer ID: " + hub.queryProcessor.PlayerId);
								stringBuilder.Append("\nIP: " + ((networkConnection != null) ? ((ev.Arguments[0].ToUpper() == "PLAYER") ? networkConnection.address : "[УДАЛЕНО]") : "null"));
								stringBuilder.Append("\nUser ID: " + (flag6 ? (string.IsNullOrEmpty(characterClassManager.UserId) ? "(none)" : characterClassManager.UserId) : "<color=#D4AF37>НЕДОСТАТОЧНО ПРАВ</color>"));
								if (flag6)
								{
									if (characterClassManager.SaltedUserId != null && characterClassManager.SaltedUserId.Contains("$"))
									{
										stringBuilder.Append("\nSalted User ID: " + characterClassManager.SaltedUserId);
									}
									if (!string.IsNullOrEmpty(characterClassManager.UserId2))
									{
										stringBuilder.Append("\nUser ID 2: " + characterClassManager.UserId2);
									}
								}
								stringBuilder.Append("\nРоль: " + serverRoles.GetColoredRoleString(false));
								bool flag7 = PermissionsHandler.IsPermitted(sender.Permissions, global::PlayerPermissions.ViewHiddenBadges);
								bool flag8 = PermissionsHandler.IsPermitted(sender.Permissions, global::PlayerPermissions.ViewHiddenGlobalBadges);
								if (ev.Sender.ReferenceHub != null && ev.Sender.ReferenceHub.serverRoles.Staff)
								{
									flag7 = true;
									flag8 = true;
								}
								bool flag9 = !string.IsNullOrEmpty(serverRoles.HiddenBadge);
								bool flag10 = !flag9 || (serverRoles.GlobalHidden && flag8) || (!serverRoles.GlobalHidden && flag7);
								if (flag10)
								{
									if (flag9)
									{
										stringBuilder.Append("\n<color=#DC143C>Hidden role: </color>" + serverRoles.HiddenBadge);
										stringBuilder.Append("\n<color=#DC143C>Hidden role type: </color>" + (serverRoles.GlobalHidden ? "GLOBAL" : "LOCAL"));
									}
									if (serverRoles.RaEverywhere)
									{
										stringBuilder.Append("\nActive flag: <color=#BCC6CC>Studio GLOBAL Staff (management or global moderation)</color>");
									}
									else if (serverRoles.Staff)
									{
										stringBuilder.Append("\nActive flag: Studio Staff");
									}
								}
								if (characterClassManager.Muted)
								{
									stringBuilder.Append("\nActive flag: <color=#F70D1A>SERVER MUTED</color>");
								}
								else if (characterClassManager.IntercomMuted)
								{
									stringBuilder.Append("\nActive flag: <color=#F70D1A>INTERCOM MUTED</color>");
								}
								if (characterClassManager.GodMode)
								{
									stringBuilder.Append("\nActive flag: <color=#659EC7>GOD MODE</color>");
								}
								if (characterClassManager.NoclipEnabled)
								{
									stringBuilder.Append("\nActive flag: <color=#DC143C>NOCLIP ENABLED</color>");
								}
								else if (ev.Sender.NoClipEnabled)//serverRoles.NoclipReady
								{
									stringBuilder.Append("\nActive flag: <color=#E52B50>NOCLIP UNLOCKED</color>");
								}
								if (serverRoles.DoNotTrack)
								{
									stringBuilder.Append("\nActive flag: <color=#BFFF00>DO NOT TRACK</color>");
								}
								if (serverRoles.BypassMode)
								{
									stringBuilder.Append("\nActive flag: <color=#BFFF00>BYPASS MODE</color>");
								}
								if (flag10 && serverRoles.RemoteAdmin)
								{
									stringBuilder.Append("\nActive flag: <color=#43C6DB>REMOTE ADMIN AUTHENTICATED</color>");
								}
								if (Player.Get(hub).IsOverwatchEnabled)
								{
									stringBuilder.Append("\nActive flag: <color=#008080>OVERWATCH MODE</color>");
								}
								else
								{
									stringBuilder.Append("\nClass: " + (flag5 ? (characterClassManager.Classes.CheckBounds(characterClassManager.CurClass) ? characterClassManager.CurRole.fullName : "None") : "<color=#D4AF37>НЕДОСТАТОЧНО ПРАВ</color>"));
									stringBuilder.Append("\nHP: " + (flag5 ? hub.playerStats.HealthToString() : "<color=#D4AF37>НЕДОСТАТОЧНО ПРАВ</color>"));
									stringBuilder.Append("\nPosition: " + (flag5 ? string.Format("[{0}; {1}; {2}]", hub.playerMovementSync.RealModelPosition.x, hub.playerMovementSync.RealModelPosition.y, hub.playerMovementSync.RealModelPosition.z) : "<color=#D4AF37>НЕДОСТАТОЧНО ПРАВ</color>"));
									if (!flag5)
									{
										stringBuilder.Append("\n<color=#D4AF37>*Требуется разрешение GameplayData</color>");
									}
								}
								stringBuilder.Append("</color>");
								sender.RaReply(ev.Name.ToUpper() + ":PLAYER#" + stringBuilder, true, true, "PlayerInfo");
								StringBuilderPool.Shared.Return(stringBuilder);
								sender.RaReply("PlayerInfoQR#" + (string.IsNullOrEmpty(characterClassManager.UserId) ? "(no User ID)" : characterClassManager.UserId), true, false, "PlayerInfo");
							}
							return;
						}
						catch (Exception ex3)
						{
							sender.RaReply(string.Concat(new string[]
							{
														ev.Name.ToUpper(),
														"#An unexpected problem has occurred!\nMessage: ",
														ex3.Message,
														"\nStackTrace: ",
														ex3.StackTrace,
														"\nAt: ",
														ex3.Source
							}), false, true, "PlayerInfo");
							throw;
						}
					}
				}
			}
		}
	}
}