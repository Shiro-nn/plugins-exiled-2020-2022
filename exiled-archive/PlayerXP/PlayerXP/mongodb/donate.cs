using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlayerXP
{
	public class donate
	{
		private readonly Plugin plugin;
		public donate(Plugin plugin) => this.plugin = plugin;
		private readonly Regex regexSmartSiteReplacer = new Regex(@"#" + Configs.pn);
		internal Dictionary<string, int> money = new Dictionary<string, int>();
		private Dictionary<string, int> xp = new Dictionary<string, int>();
		private Dictionary<string, int> lvl = new Dictionary<string, int>();
		private Dictionary<string, int> to = new Dictionary<string, int>();
		private Dictionary<string, bool> pr = new Dictionary<string, bool>();
		private Dictionary<string, bool> vr = new Dictionary<string, bool>();
		private Dictionary<string, bool> vpr = new Dictionary<string, bool>();
		private Dictionary<string, bool> er = new Dictionary<string, bool>();
		private Dictionary<string, bool> rr = new Dictionary<string, bool>();
		private Dictionary<string, bool> sr = new Dictionary<string, bool>();
		private Dictionary<string, bool> hr = new Dictionary<string, bool>();
		private Dictionary<string, bool> ghr = new Dictionary<string, bool>();
		private Dictionary<string, bool> ar = new Dictionary<string, bool>();
		private Dictionary<string, bool> gar = new Dictionary<string, bool>();
		private Dictionary<string, bool> or = new Dictionary<string, bool>();
		private Dictionary<string, string> prefix = new Dictionary<string, string>();
		private Dictionary<string, bool> find = new Dictionary<string, bool>();
		private Dictionary<string, int> time = new Dictionary<string, int>();
		private Dictionary<string, string> warns = new Dictionary<string, string>();
		internal static Dictionary<string, int> giveway = new Dictionary<string, int>();
		internal static Dictionary<string, int> force = new Dictionary<string, int>();
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		//private static string mongodburl = "mongodb+srv://fydne:Qaz123fydne@cluster0.dseab.mongodb.net/login?retryWrites=true&w=majority";
		private string mongodburl = "mongodb://localhost/login";
		public void OnWaitingForPlayers()
		{
			var client = new MongoClient(mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			Coroutines.Add(Timing.RunCoroutine(SecondCounter()));
			force.Clear();
			giveway.Clear();
		}
		public void OnPlayerDeath(DiedEventArgs ev)
		{
			try
			{
				if (plugin.mtfvsci.GamemodeEnabled) return;
				ReferenceHub target = ev.Target.ReferenceHub;
				ReferenceHub killer = ev.Killer.ReferenceHub;
				string targetname = ev.Target?.Nickname;
				try { setprefix(target); } catch { }
				List<Team> pList = Player.List.Select(x => Extensions.GetTeam(x.ReferenceHub)).ToList();
				if (target == null || string.IsNullOrEmpty(target.characterClassManager.UserId))
					return;


				if (killer == null || string.IsNullOrEmpty(killer.characterClassManager.UserId))
					return;

				int cxp = xp[killer.characterClassManager.UserId];
				if (killer != target)
				{
					ev.Killer.ClearBroadcasts();
					if (killer == pList.Contains(Team.CHI))
					{
						money[killer.characterClassManager.UserId] += 5;
						if (target == pList.Contains(Team.MTF))
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.RSC))
						{
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							cxp += 75;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "75").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.TUT))
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "5").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.TUT))
					{
						money[killer.characterClassManager.UserId] += 3;
						if (target == pList.Contains(Team.CDP))
						{
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.RSC))
						{
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.MTF))
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.CHI))
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							cxp += 10;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "10").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.RSC))
					{
						money[killer.characterClassManager.UserId] += 7;
						if (target == pList.Contains(Team.CDP))
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.CHI))
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.TUT))
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							cxp += 200;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.CDP))
					{
						money[killer.characterClassManager.UserId] += 7;
						if (target == pList.Contains(Team.RSC))
						{
							cxp += 50;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.MTF))
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.SCP))
						{
							cxp += 200;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
						}
						else if (target == pList.Contains(Team.TUT))
						{
							cxp += 100;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "7").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
					else if (killer == pList.Contains(Team.SCP))
					{
						money[killer.characterClassManager.UserId] += 3;
						cxp += 25;
						killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%money%", "7").Replace("%xp%", "25").Replace("%player%", $"{targetname}"), 10, 0);
					}
				}
				string nick = ev.Killer?.Nickname;
				MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
				if (matches.Count > 0)
				{
					if (killer != target)
					{
						ev.Killer.ClearBroadcasts();
						if (killer == pList.Contains(Team.CHI))
						{
							money[killer.characterClassManager.UserId] += 5;
							if (target == pList.Contains(Team.MTF))
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.RSC))
							{
								cxp += 25;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								cxp += 75;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "150").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.TUT))
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "10").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.TUT))
						{
							money[killer.characterClassManager.UserId] += 3;
							if (target == pList.Contains(Team.CDP))
							{
								cxp += 25;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.RSC))
							{
								cxp += 25;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.MTF))
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.CHI))
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								cxp += 10;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "20").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.RSC))
						{
							money[killer.characterClassManager.UserId] += 7;
							if (target == pList.Contains(Team.CDP))
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.CHI))
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.TUT))
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								cxp += 200;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "400").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.CDP))
						{
							money[killer.characterClassManager.UserId] += 7;
							if (target == pList.Contains(Team.RSC))
							{
								cxp += 50;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "100").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.MTF))
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.SCP))
							{
								cxp += 200;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "400").Replace("%player%", $"{targetname}"), 10, 0);
							}
							else if (target == pList.Contains(Team.TUT))
							{
								cxp += 100;
								killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "14").Replace("%xp%", "200").Replace("%player%", $"{targetname}"), 10, 0);
							}
						}
						else if (killer == pList.Contains(Team.SCP))
						{
							money[killer.characterClassManager.UserId] += 3;
							cxp += 25;
							killer.GetComponent<Broadcast>().TargetAddElement(killer.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{targetname}"), 10, 0);
						}
					}
				}
				xp[killer.characterClassManager.UserId] = cxp;
				AddXP(killer);
			}
			catch { }
		}
		public void RoundEnd(RoundEndedEventArgs ev)
		{
			var client = new MongoClient(mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var stata = database.GetCollection<BsonDocument>("stats");
			List<ReferenceHub> players = Extensions.GetHubs().ToList();
			foreach (ReferenceHub player in players)
			{
				try
				{
					string nick = player.nicknameSync.DisplayName;
					MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
					if (matches.Count > 0)
					{
						xp[player.characterClassManager.UserId] += 200;
						player.Broadcast("<color=#fdffbb>Вы получили <color=red>200xp</color>, т.к в вашем нике есть <color=#0089c7>#fydne</color>!</color>", 10);
						AddXP(player);
					}
					var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
					if (find[player.characterClassManager.UserId])
					{
						int min = time[player.characterClassManager.UserId];
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
						collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", min));
					}
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", xp[player.characterClassManager.UserId]));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", lvl[player.characterClassManager.UserId]));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", money[player.characterClassManager.UserId]));
					stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", to[player.characterClassManager.UserId]));
				}
				catch { }
			}
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
			force.Clear();
			giveway.Clear();
		}
		public void ban(BanningEventArgs ev)
		{
			DateTime ExpireDate = DateTime.Now.AddMinutes(ev.Duration);
			try
			{
				var client = new MongoClient(mongodburl);
				var database = client.GetDatabase("login");
				var collection = database.GetCollection<BsonDocument>("accounts");
				var filterr = Builders<BsonDocument>.Filter.Eq("steam", ev.Issuer.UserId.Replace("@steam", ""));
				var statsa = collection.Find(new BsonDocument("steam", ev.Issuer.UserId.Replace("@steam", ""))).ToList();
				foreach (var document in statsa)
				{
					int bc = (int)document["bans"];
					int bcc = bc++;
					collection.UpdateOne(filterr, Builders<BsonDocument>.Update.Set("bans", bcc));
				}
				var filter = Builders<BsonDocument>.Filter.Eq("steam", ev.Target.UserId.Replace("@steam", ""));
				collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("ban", ExpireDate.ToString("dd.MM.yyyy HH:mm")));
			}
            catch { }
			string reason = "";
			if (ev.Reason != "")
			{
				reason = $"<color=#00ffff>Причина</color>: <color=red>{ev.Reason}</color>";
			}
			Map.Broadcast(5, $"<color=#00ffff>{ev.Target.Nickname}</color> <color=red>забанен</color> <color=#00ffff>{ev.Issuer.Nickname}</color> <color=red>до</color> <color=#00ffff> {ExpireDate.ToString("dd.MM.yyyy HH:mm")}</color> {reason}");

		}
		public void kick(KickingEventArgs ev)
		{
			var client = new MongoClient(mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var filter = Builders<BsonDocument>.Filter.Eq("steam", ev.Issuer.UserId.Replace("@steam", ""));
			var statsa = collection.Find(new BsonDocument("steam", ev.Issuer.UserId.Replace("@steam", ""))).ToList();
			foreach (var document in statsa)
			{
				int kc = (int)document["kicks"];
				int kcc = kc++;
				collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("kicks", kcc));
			}
			string reason = "";
			if (ev.Reason != "")
			{
				reason = $"<color=#00ffff>Причина</color>: <color=red>{ev.Reason}</color>";
			}
			Map.Broadcast(5, $"<color=#00ffff>{ev.Target.Nickname}</color> <color=red>кикнут</color> <color=#00ffff>{ev.Issuer.Nickname}</color>. {reason}");
		}
		public void left(LeftEventArgs ev)
		{
			try
			{
				ReferenceHub player = ev.Player.ReferenceHub;
				var client = new MongoClient(mongodburl);
				var database = client.GetDatabase("login");
				var collection = database.GetCollection<BsonDocument>("accounts");
				var stata = database.GetCollection<BsonDocument>("stats");
				var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", xp[player.characterClassManager.UserId]));
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", lvl[player.characterClassManager.UserId]));
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", money[player.characterClassManager.UserId]));
				stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", to[player.characterClassManager.UserId]));
				xp.Remove(player.characterClassManager.UserId);
				lvl.Remove(player.characterClassManager.UserId);
				money.Remove(player.characterClassManager.UserId);
				to.Remove(player.characterClassManager.UserId);
				force.Remove(player.characterClassManager.UserId);
				giveway.Remove(player.characterClassManager.UserId);
				if (find[player.characterClassManager.UserId])
				{
					int min = time[player.characterClassManager.UserId];
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", min));

					time.Remove(player.characterClassManager.UserId);
					prefix.Remove(player.characterClassManager.UserId);

					pr.Remove(player.characterClassManager.UserId);
					vr.Remove(player.characterClassManager.UserId);
					vpr.Remove(player.characterClassManager.UserId);
					er.Remove(player.characterClassManager.UserId);
					rr.Remove(player.characterClassManager.UserId);
					sr.Remove(player.characterClassManager.UserId);
					hr.Remove(player.characterClassManager.UserId);
					ghr.Remove(player.characterClassManager.UserId);
					ar.Remove(player.characterClassManager.UserId);
					gar.Remove(player.characterClassManager.UserId);
					or.Remove(player.characterClassManager.UserId);
					warns.Remove(player.characterClassManager.UserId);
				}
				find.Remove(player.characterClassManager.UserId);
			}
			catch { }
		}
		public void OnPlayerJoin(JoinedEventArgs ev)
		{
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
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;
			try
			{
				secs(ev.Player.ReferenceHub);
			}
			catch
			{
				try
				{
					errorsecs(ev.Player.ReferenceHub);
				}
				catch
				{
					errorsecsfatal(ev.Player.ReferenceHub);
				}
			}
		}
		public void OnPlayerSpawn(SpawningEventArgs ev)
		{
			if (string.IsNullOrEmpty(ev.Player.ReferenceHub.characterClassManager.UserId) || ev.Player.ReferenceHub.characterClassManager.IsHost || ev.Player.ReferenceHub.nicknameSync.MyNick == "Dedicated Server")
				return;

			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(ev.Player.ReferenceHub);
			});
		}
		public void OnCheckEscape(EscapingEventArgs ev)
		{
			if (ev.NewRole == RoleType.NtfCadet) ev.NewRole = RoleType.NtfLieutenant;
			try
			{
				int cxp = xp[ev.Player.ReferenceHub.characterClassManager.UserId];

				ev.Player.ClearBroadcasts();
				cxp += 100;
				money[ev.Player.ReferenceHub.characterClassManager.UserId] += 10;
				ev.Player.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(ev.Player.ReferenceHub.scp079PlayerScript.connectionToClient, Configs.eb.Replace("%xp%", "100").Replace("%money%", "10"), 10, 0);
				string nick = ev.Player?.Nickname;
				MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
				if (matches.Count > 0)
				{
					ev.Player.ClearBroadcasts();
					cxp += 100;
					money[ev.Player.ReferenceHub.characterClassManager.UserId] += 10;
					ev.Player.ReferenceHub.GetComponent<Broadcast>().TargetAddElement(ev.Player.ReferenceHub.scp079PlayerScript.connectionToClient, Configs.eb.Replace("%xp%", "200").Replace("%money%", "20"), 10, 0);
				}
				xp[ev.Player.ReferenceHub.characterClassManager.UserId] = cxp;
				AddXP(ev.Player.ReferenceHub);
			}
			catch { }
		}
		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
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
					int cxp = xp[scp106.characterClassManager.UserId];
					cxp += 25;
					money[scp106.characterClassManager.UserId] += 3;
					pscp106.ClearBroadcasts();
					scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "3").Replace("%xp%", "25").Replace("%player%", $"{ev.Player?.Nickname}"), 10, 0);
					string nick = pscp106?.Nickname;
					MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
					if (matches.Count > 0)
					{
						money[scp106.characterClassManager.UserId] += 3;
						pscp106.ClearBroadcasts();
						cxp += 25;
						scp106.GetComponent<Broadcast>().TargetAddElement(scp106.scp079PlayerScript.connectionToClient, Configs.kb.Replace("%money%", "6").Replace("%xp%", "50").Replace("%player%", $"{ev.Player?.Nickname}"), 10, 0);
					}
					xp[scp106.characterClassManager.UserId] = cxp;
					AddXP(scp106);
				}
			}
			catch { }
		}
		public void console(SendingConsoleCommandEventArgs ev)
		{
			try
			{
				string cmd = ev.Name;
				if (cmd == "money" || cmd == "мани" || cmd == "баланс" || cmd == "xp" || cmd == "lvl" || cmd == Configs.cc)
				{
					ev.IsAllowed = false;
					ev.ReturnMessage = $"\n----------------------------------------------------------- \nXP:\n{xp[ev.Player.ReferenceHub.characterClassManager.UserId]}/{to[ev.Player.ReferenceHub.characterClassManager.UserId]}\nlvl:\n{lvl[ev.Player.ReferenceHub.characterClassManager.UserId]}\nБаланс:\n{money[ev.Player.ReferenceHub.characterClassManager.UserId]}\n-----------------------------------------------------------";
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
					if (money[ev.Player.ReferenceHub.characterClassManager.UserId] >= result)
					{
						money[ev.Player.ReferenceHub.characterClassManager.UserId] -= result;
						money[rh.characterClassManager.UserId] += result;
						ev.ReturnMessage = $"Вы успешно передали {result} монет игроку {rh.GetNickname()}.";
						ev.Color = "green";
						rh.ClearBroadcasts();
						rh.Broadcast($"<size=20><color=lime>{rh.GetNickname()} передал вам {result} монет</color></size>", 5);
						return;
					}
					ev.ReturnMessage = $"Недостаточно средств({money[ev.Player.ReferenceHub.characterClassManager.UserId]}/{result}).";
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
		public void ra(SendingRemoteAdminCommandEventArgs ev)
		{
			string effort = ev.Name;
			foreach (string s in ev.Arguments)
				effort += $" {s}";
			string[] args = effort.Split(' ');
			string[] command = ev.Arguments.ToArray();
			CommandSender send = ev.CommandSender;
			ReferenceHub player = ev.Sender.ReferenceHub;
			ReferenceHub sender = ev.CommandSender.SenderId == "SERVER CONSOLE" || ev.CommandSender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Extensions.GetPlayer(ev.CommandSender.SenderId);
			if (ev.Name == "give")
			{
				try
				{
					if (vpr[player.characterClassManager.UserId] || vr[player.characterClassManager.UserId])
					{
						ev.IsAllowed = false;
						if (command[1] == "16")
						{
							ev.returnMessage = ("GIVE#Пылесос только 1");
							ev.IsAllowed = false;
							return;
						}
						else if (command[1] == "11")
						{
							ev.returnMessage = ("GIVE#Черная карта-слишком");
							ev.IsAllowed = false;
							return;
						}
						else if (sender.GetRole() == RoleType.ClassD)
						{
							if (command[1] == "13" || command[1] == "17" || command[1] == "20" || command[1] == "21" || command[1] == "23" || command[1] == "24" || command[1] == "25" || command[1] == "30" || command[1] == "31" || command[1] == "32" || command[1] == "6" || command[1] == "7" || command[1] == "8" || command[1] == "9" || command[1] == "10")
							{
								if (!RoundSummary.RoundInProgress() || (double)180 >= (double)(float)RoundSummary.roundTime)
								{
									ev.returnMessage = ("GIVE#3 минуты не прошло");
									ev.IsAllowed = false;
									return;
								}
							}
						}
						else if (sender.GetRole() == RoleType.Scientist)
						{
							if (command[1] == "13" || command[1] == "17" || command[1] == "20" || command[1] == "21" || command[1] == "23" || command[1] == "24" || command[1] == "25" || command[1] == "30" || command[1] == "31" || command[1] == "32" || command[1] == "6" || command[1] == "7" || command[1] == "8" || command[1] == "9" || command[1] == "10")
							{
								if (!RoundSummary.RoundInProgress() || (double)300 >= (double)(float)RoundSummary.roundTime)
								{
									ev.returnMessage = ("GIVE#5 минут не прошло");
									ev.IsAllowed = false;
									return;
								}
							}
						}
						else if (giveway[sender.characterClassManager.UserId] == 4)
						{
							ev.returnMessage = ("GIVE#Вы уже выдали 3 предмета");
							ev.IsAllowed = false;
							return;
						}
						Timing.CallDelayed(0.4f, () => GameCore.Console.singleton.TypeCommand($"/GIVE {command[0]}. {command[1]}"));
						ev.returnMessage = ("GIVE#Успешно");
						giveway[sender.characterClassManager.UserId]++;
					}
					else if (sr[player.characterClassManager.UserId])
					{
						ev.IsAllowed = false;
						if (command[1] == "16")
						{
							ev.returnMessage = ("GIVE#Пылесос только 1");
							ev.IsAllowed = false;
							return;
						}
						else if (command[1] == "11")
						{
							ev.returnMessage = ("GIVE#Черная карта-слишком");
							ev.IsAllowed = false;
							return;
						}
						GameCore.Console.singleton.TypeCommand($"/GIVE {command[0]}. {command[1]}");
						ev.returnMessage = ("GIVE#Успешно");
					}
				}
				catch
				{
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					ev.IsAllowed = false;
					ev.returnMessage = ("GIVE#Произошла ошибка, повторите попытку позже");
				}
			}
			if (ev.Name == "forceclass")
			{
				try
				{
					if (vpr[player.characterClassManager.UserId] || pr[player.characterClassManager.UserId])
					{
						ev.IsAllowed = false;
						if (force[sender.characterClassManager.UserId] != 4)
						{
							var role = Convert.ToInt32(command[1]);
							int Scp173 = Player.List.Where(x => x.Role == RoleType.Scp173).ToList().Count;
							int Scp106 = Player.List.Where(x => x.Role == RoleType.Scp106).ToList().Count;
							int Scp049 = Player.List.Where(x => x.Role == RoleType.Scp049).ToList().Count;
							int Scp079 = Player.List.Where(x => x.Role == RoleType.Scp079).ToList().Count;
							int Scp096 = Player.List.Where(x => x.Role == RoleType.Scp096).ToList().Count;
							int Scp93989 = Player.List.Where(x => x.Role == RoleType.Scp93989).ToList().Count;
							int Scp93953 = Player.List.Where(x => x.Role == RoleType.Scp93953).ToList().Count;
							if ((RoleType)role == RoleType.Scp93953)
							{
								if (Scp93953 == 0) forcev(role, player, ev);
								else ev.returnMessage = "FORCECLASS#Этот SCP уже есть";
							}
							else if ((RoleType)role == RoleType.Scp93989)
							{
								if (Scp93989 == 0) forcev(role, player, ev);
								else ev.returnMessage = "FORCECLASS#Этот SCP уже есть";
							}
							else if ((RoleType)role == RoleType.Scp096)
							{
								if (Scp096 == 0) forcev(role, player, ev);
								else ev.returnMessage = "FORCECLASS#Этот SCP уже есть";
							}
							else if ((RoleType)role == RoleType.Scp079)
							{
								if (Scp079 == 0) forcev(role, player, ev);
								else ev.returnMessage = "FORCECLASS#Этот SCP уже есть";
							}
							else if ((RoleType)role == RoleType.Scp049)
							{
								if (Scp049 == 0) forcev(role, player, ev);
								else ev.returnMessage = "FORCECLASS#Этот SCP уже есть";
							}
							else if ((RoleType)role == RoleType.Scp106)
							{
								if (Scp106 == 0) forcev(role, player, ev);
								else ev.returnMessage = "FORCECLASS#Этот SCP уже есть";
							}
							else if ((RoleType)role == RoleType.Scp173)
							{
								if (Scp173 == 0) forcev(role, player, ev);
								else ev.returnMessage = "FORCECLASS#Этот SCP уже есть";
							}
							else
							{
								ev.returnMessage = "FORCECLASS#Не забывайте, что у вас есть только 3 спавна";
								player.characterClassManager.SetClassID((RoleType)role);
								force[sender.characterClassManager.UserId]++;
							}
						}
						else
						{
							ev.returnMessage = "FORCECLASS#Спавниться более трех раз ЗАПРЕЩЕНО";
						}
					}
				}
				catch
				{
					Log.Info(ev.CommandSender.SenderId);
					if (ev.CommandSender.SenderId == "SERVER CONSOLE") return;
					if (ev.CommandSender.SenderId == "GAME CONSOLE") return;
					ev.IsAllowed = false;
					ev.returnMessage = ("FORCECLASS#Произошла ошибка, повторите попытку позже");
				}
			}
			if (ev.Name == "hidetag")
			{
				ev.IsAllowed = false;
				ev.returnMessage = "HIDETAG#Недоступно";
			}
		}





		internal void forcev(int role, ReferenceHub player, SendingRemoteAdminCommandEventArgs ev)
		{
			ev.returnMessage = "FORCECLASS#Не забывайте, что у вас есть только 3 спавна";
			player.characterClassManager.SetClassID((RoleType)role);
			//GameCore.Console.singleton.TypeCommand($"/FORCECLASS {sender.GetPlayerId()}. {command[1]}");
			force[player.characterClassManager.UserId]++;
			return;
		}
		public void AddXP(ReferenceHub player)
		{
			int cxp = xp[player.characterClassManager.UserId];
			int cto = to[player.characterClassManager.UserId];
			int clvl = lvl[player.characterClassManager.UserId];
			if (cxp >= cto)
			{
				cxp -= cto;
				clvl++;
				cto = clvl * 250 + 750;
				player.Broadcast(Configs.lvlup.Replace("%lvl%", $"{clvl}").Replace("%to.xp%", ((cto) - cxp).ToString()), 10);
				xp[player.characterClassManager.UserId] = cxp;
				to[player.characterClassManager.UserId] = cto;
				lvl[player.characterClassManager.UserId] = clvl;
				setprefix(player);
			}
		}
		private IEnumerator<float> SecondCounter()
		{
			for (; ; )
			{
				try
				{
					foreach (Player playerp in Player.List.ToList())
					{
						ReferenceHub player = playerp.ReferenceHub;
						try
						{
							time[player.characterClassManager.UserId]++;
						}
						catch { }
					}
				}
				catch { };
				yield return Timing.WaitForSeconds(1f);
			}
		}
		public void secs(ReferenceHub player)//oh, no. Я не это имел ввиду
		{
			var client = new MongoClient(mongodburl);
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
				xp.Add(player.characterClassManager.UserId, 0);
				lvl.Add(player.characterClassManager.UserId, 0);
				money.Add(player.characterClassManager.UserId, 0);
				to.Add(player.characterClassManager.UserId, 0);
			}
			else
			{
				foreach (var document in statsa)
				{
					int xpd = (int)document["xp"];
					int tod = (int)document["to"];
					int lvld = (int)document["lvl"];
					int moneyd = (int)document["money"];
					xp.Add(player.characterClassManager.UserId, xpd);
					lvl.Add(player.characterClassManager.UserId, lvld);
					money.Add(player.characterClassManager.UserId, moneyd);
					to.Add(player.characterClassManager.UserId, tod);
				}
			}
			foreach (var document in list)
			{
				int t = (int)document["min"];
				string prefd = (string)document["prefix"];
				time.Add(player.characterClassManager.UserId, t);
				prefix.Add(player.characterClassManager.UserId, prefd);
				find.Add(player.characterClassManager.UserId, true);

				pr.Add(player.characterClassManager.UserId, (bool)document["pe"]);
				vr.Add(player.characterClassManager.UserId, (bool)document["ve"]);
				vpr.Add(player.characterClassManager.UserId, (bool)document["vpe"]);
				er.Add(player.characterClassManager.UserId, (bool)document["ee"]);
				rr.Add(player.characterClassManager.UserId, (bool)document["re"]);
				sr.Add(player.characterClassManager.UserId, (bool)document["sr"]);
				hr.Add(player.characterClassManager.UserId, (bool)document["hr"]);
				ghr.Add(player.characterClassManager.UserId, (bool)document["ghr"]);
				ar.Add(player.characterClassManager.UserId, (bool)document["ar"]);
				gar.Add(player.characterClassManager.UserId, (bool)document["gar"]);
				or.Add(player.characterClassManager.UserId, (bool)document["or"]);
				giveway.Add(player.characterClassManager.UserId, 0);
				force.Add(player.characterClassManager.UserId, 0);
				try
				{
					if ((int)document["warnings"] == 0)
					{
						warns.Add(player.characterClassManager.UserId, "");
					}
					else
					{
						warns.Add(player.characterClassManager.UserId, "| " + (int)document["warnings"] + " пред(а) ");
					}
				}
				catch
				{
					warns.Add(player.characterClassManager.UserId, "");
				}
			}
			Timing.CallDelayed(1.5f, () =>
			{
				spawnpref(player, list, collection);
			});
			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(player);
			});
		}
		public void errorsecs(ReferenceHub player)//oh, no. Я не это имел ввиду
		{
			var client = new MongoClient(mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var stata = database.GetCollection<BsonDocument>("stats");
			var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", xp[player.characterClassManager.UserId]));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", lvl[player.characterClassManager.UserId]));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", money[player.characterClassManager.UserId]));
			stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", to[player.characterClassManager.UserId]));
			xp.Remove(player.characterClassManager.UserId);
			lvl.Remove(player.characterClassManager.UserId);
			to.Remove(player.characterClassManager.UserId);
			money.Remove(player.characterClassManager.UserId);
			force.Remove(player.characterClassManager.UserId);
			giveway.Remove(player.characterClassManager.UserId);
			if (find[player.characterClassManager.UserId])
			{
				int min = 0;
				if (time.ContainsKey(player.characterClassManager.UserId))
				{
					min = time[player.characterClassManager.UserId];
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", min));
				}
				time.Remove(player.characterClassManager.UserId);
				prefix.Remove(player.characterClassManager.UserId);

				pr.Remove(player.characterClassManager.UserId);
				vr.Remove(player.characterClassManager.UserId);
				vpr.Remove(player.characterClassManager.UserId);
				er.Remove(player.characterClassManager.UserId);
				rr.Remove(player.characterClassManager.UserId);
				sr.Remove(player.characterClassManager.UserId);
				hr.Remove(player.characterClassManager.UserId);
				ghr.Remove(player.characterClassManager.UserId);
				ar.Remove(player.characterClassManager.UserId);
				gar.Remove(player.characterClassManager.UserId);
				or.Remove(player.characterClassManager.UserId);
				warns.Remove(player.characterClassManager.UserId);
			}
			find.Remove(player.characterClassManager.UserId);
			var list = collection.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			var statsa = stata.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			if (statsa.Count == 0)
			{
				var sta = database.GetCollection<Person>("stats");
				sta.InsertOneAsync(new Person { steam = player.characterClassManager.UserId.Replace("@steam", "") });
				xp.Add(player.characterClassManager.UserId, 0);
				lvl.Add(player.characterClassManager.UserId, 0);
				money.Add(player.characterClassManager.UserId, 0);
				to.Add(player.characterClassManager.UserId, 0);
			}
			else
			{
				foreach (var document in statsa)
				{
					int xpd = (int)document["xp"];
					int tod = (int)document["to"];
					int lvld = (int)document["lvl"];
					int moneyd = (int)document["money"];
					xp.Add(player.characterClassManager.UserId, xpd);
					lvl.Add(player.characterClassManager.UserId, lvld);
					money.Add(player.characterClassManager.UserId, moneyd);
					to.Add(player.characterClassManager.UserId, tod);
				}
			}
			foreach (var document in list)
			{
				int t = (int)document["min"];
				string prefd = (string)document["prefix"];
				time.Add(player.characterClassManager.UserId, t);
				prefix.Add(player.characterClassManager.UserId, prefd);
				find.Add(player.characterClassManager.UserId, true);

				pr.Add(player.characterClassManager.UserId, (bool)document["pe"]);
				vr.Add(player.characterClassManager.UserId, (bool)document["ve"]);
				vpr.Add(player.characterClassManager.UserId, (bool)document["vpe"]);
				er.Add(player.characterClassManager.UserId, (bool)document["ee"]);
				rr.Add(player.characterClassManager.UserId, (bool)document["re"]);
				sr.Add(player.characterClassManager.UserId, (bool)document["sr"]);
				hr.Add(player.characterClassManager.UserId, (bool)document["hr"]);
				ghr.Add(player.characterClassManager.UserId, (bool)document["ghr"]);
				ar.Add(player.characterClassManager.UserId, (bool)document["ar"]);
				gar.Add(player.characterClassManager.UserId, (bool)document["gar"]);
				or.Add(player.characterClassManager.UserId, (bool)document["or"]);
				giveway.Add(player.characterClassManager.UserId, 0);
				force.Add(player.characterClassManager.UserId, 0);
				try
				{
					if ((int)document["warnings"] == 0)
					{
						warns.Add(player.characterClassManager.UserId, "");
					}
					else
					{
						warns.Add(player.characterClassManager.UserId, "| " + (int)document["warnings"] + " пред(а) ");
					}
				}
				catch
				{
					warns.Add(player.characterClassManager.UserId, "");
				}
			}
			Timing.CallDelayed(1.5f, () =>
			{
				spawnpref(player, list, collection);
			});
			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(player);
			});
		}
		public void errorsecsfatal(ReferenceHub player)//oh, no. Я не это имел ввиду
		{
			var client = new MongoClient(mongodburl);
			var database = client.GetDatabase("login");
			var collection = database.GetCollection<BsonDocument>("accounts");
			var stata = database.GetCollection<BsonDocument>("stats");
			var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
			#region update levels
			try
			{
				if (xp.ContainsKey(player.characterClassManager.UserId)) stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("xp", xp[player.characterClassManager.UserId]));
			}
			catch
			{
				Log.Info("error in update xp");
			}
			try
			{
				if (lvl.ContainsKey(player.characterClassManager.UserId)) stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("lvl", lvl[player.characterClassManager.UserId]));
			}
			catch
			{
				Log.Info("error in update lvl");
			}
			try
			{
				if (money.ContainsKey(player.characterClassManager.UserId)) stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("money", money[player.characterClassManager.UserId]));
			}
			catch
			{
				Log.Info("error in update money");
			}
			try
			{
				if (to.ContainsKey(player.characterClassManager.UserId)) stata.UpdateOne(filter, Builders<BsonDocument>.Update.Set("to", to[player.characterClassManager.UserId]));
			}
			catch
			{
				Log.Info("error in update xp to");
			}
			#endregion




			#region delete levels
			try
			{
				if (xp.ContainsKey(player.characterClassManager.UserId)) xp.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete xp");
			}
			try
			{
				if (lvl.ContainsKey(player.characterClassManager.UserId)) lvl.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete lvl");
			}
			try
			{
				if (money.ContainsKey(player.characterClassManager.UserId)) money.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete money");
			}
			try
			{
				if (to.ContainsKey(player.characterClassManager.UserId)) to.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete xp to");
			}
			try
			{
				if (force.ContainsKey(player.characterClassManager.UserId)) force.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete force");
			}
			try
			{
				if (giveway.ContainsKey(player.characterClassManager.UserId)) giveway.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete give");
			}
			#endregion
			if (find.ContainsKey(player.characterClassManager.UserId))
				if (find[player.characterClassManager.UserId])
				{
					#region update time
					try
					{
						int min = 0;
						if (time.ContainsKey(player.characterClassManager.UserId))
						{
							min = time[player.characterClassManager.UserId];
							collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("min", min));
							collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", Extensions.GetHRT(min)));
							collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("adminmin", min));
						}
					}
					catch
					{
						Log.Info("error in update time");
					}
					#endregion
					#region delete account data
					try
					{
						if (time.ContainsKey(player.characterClassManager.UserId)) time.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete time");
					}
					try
					{
						if (prefix.ContainsKey(player.characterClassManager.UserId)) prefix.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete prefix");
					}

					try
					{
						if (pr.ContainsKey(player.characterClassManager.UserId)) pr.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete premium data");
					}
					try
					{
						if (vr.ContainsKey(player.characterClassManager.UserId)) vr.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete vip data");
					}
					try
					{
						if (vpr.ContainsKey(player.characterClassManager.UserId)) vpr.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete vip+ data");
					}
					try
					{
						if (er.ContainsKey(player.characterClassManager.UserId)) er.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete elite data");
					}
					try
					{
						if (rr.ContainsKey(player.characterClassManager.UserId)) rr.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete rainbow data");
					}
					try
					{
						if (sr.ContainsKey(player.characterClassManager.UserId)) sr.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete trainee data");
					}
					try
					{
						if (hr.ContainsKey(player.characterClassManager.UserId)) hr.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete helper data");
					}
					try
					{
						if (ghr.ContainsKey(player.characterClassManager.UserId)) ghr.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete main helper data");
					}
					try
					{
						if (ar.ContainsKey(player.characterClassManager.UserId)) ar.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete admin data");
					}
					try
					{
						if (gar.ContainsKey(player.characterClassManager.UserId)) gar.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete main admin data");
					}
					try
					{
						if (or.ContainsKey(player.characterClassManager.UserId)) or.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete owner data");
					}
					try
					{
						if (warns.ContainsKey(player.characterClassManager.UserId)) warns.Remove(player.characterClassManager.UserId);
					}
					catch
					{
						Log.Info("error in delete warns");
					}
					#endregion
				}
			try
			{
				if (find.ContainsKey(player.characterClassManager.UserId)) find.Remove(player.characterClassManager.UserId);
			}
			catch
			{
				Log.Info("error in delete find");
			}
			var list = collection.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			var statsa = stata.Find(new BsonDocument("steam", player.characterClassManager.UserId.Replace("@steam", ""))).ToList();
			if (statsa.Count == 0)
			{
				var sta = database.GetCollection<Person>("stats");
				sta.InsertOneAsync(new Person { steam = player.characterClassManager.UserId.Replace("@steam", "") });
				if (!xp.ContainsKey(player.characterClassManager.UserId)) xp.Add(player.characterClassManager.UserId, 0);
				if (!lvl.ContainsKey(player.characterClassManager.UserId)) lvl.Add(player.characterClassManager.UserId, 0);
				if (!money.ContainsKey(player.characterClassManager.UserId)) money.Add(player.characterClassManager.UserId, 0);
				if (!to.ContainsKey(player.characterClassManager.UserId)) to.Add(player.characterClassManager.UserId, 0);
			}
			else
			{
				foreach (var document in statsa)
				{
					int xpd = (int)document["xp"];
					int tod = (int)document["to"];
					int lvld = (int)document["lvl"];
					int moneyd = (int)document["money"];
					if (!xp.ContainsKey(player.characterClassManager.UserId)) xp.Add(player.characterClassManager.UserId, xpd);
					if (!lvl.ContainsKey(player.characterClassManager.UserId)) lvl.Add(player.characterClassManager.UserId, lvld);
					if (!money.ContainsKey(player.characterClassManager.UserId)) money.Add(player.characterClassManager.UserId, moneyd);
					if (!to.ContainsKey(player.characterClassManager.UserId)) to.Add(player.characterClassManager.UserId, tod);
				}
			}
			foreach (var document in list)
			{
				int t = (int)document["min"];
				string prefd = (string)document["prefix"];
				if (!time.ContainsKey(player.characterClassManager.UserId)) time.Add(player.characterClassManager.UserId, t);
				if (!prefix.ContainsKey(player.characterClassManager.UserId)) prefix.Add(player.characterClassManager.UserId, prefd);
				if (!find.ContainsKey(player.characterClassManager.UserId)) find.Add(player.characterClassManager.UserId, true);

				if (!pr.ContainsKey(player.characterClassManager.UserId)) pr.Add(player.characterClassManager.UserId, (bool)document["pe"]);
				if (!vr.ContainsKey(player.characterClassManager.UserId)) vr.Add(player.characterClassManager.UserId, (bool)document["ve"]);
				if (!vpr.ContainsKey(player.characterClassManager.UserId)) vpr.Add(player.characterClassManager.UserId, (bool)document["vpe"]);
				if (!er.ContainsKey(player.characterClassManager.UserId)) er.Add(player.characterClassManager.UserId, (bool)document["ee"]);
				if (!rr.ContainsKey(player.characterClassManager.UserId)) rr.Add(player.characterClassManager.UserId, (bool)document["re"]);
				if (!sr.ContainsKey(player.characterClassManager.UserId)) sr.Add(player.characterClassManager.UserId, (bool)document["sr"]);
				if (!hr.ContainsKey(player.characterClassManager.UserId)) hr.Add(player.characterClassManager.UserId, (bool)document["hr"]);
				if (!ghr.ContainsKey(player.characterClassManager.UserId)) ghr.Add(player.characterClassManager.UserId, (bool)document["ghr"]);
				if (!ar.ContainsKey(player.characterClassManager.UserId)) ar.Add(player.characterClassManager.UserId, (bool)document["ar"]);
				if (!gar.ContainsKey(player.characterClassManager.UserId)) gar.Add(player.characterClassManager.UserId, (bool)document["gar"]);
				if (!or.ContainsKey(player.characterClassManager.UserId)) or.Add(player.characterClassManager.UserId, (bool)document["or"]);
				if (!giveway.ContainsKey(player.characterClassManager.UserId)) giveway.Add(player.characterClassManager.UserId, 0);
				if (!force.ContainsKey(player.characterClassManager.UserId)) force.Add(player.characterClassManager.UserId, 0);
				try
				{
					if ((int)document["warnings"] == 0)
					{
						if (!warns.ContainsKey(player.characterClassManager.UserId)) warns.Add(player.characterClassManager.UserId, "");
					}
					else
					{
						if (!warns.ContainsKey(player.characterClassManager.UserId)) warns.Add(player.characterClassManager.UserId, "| " + (int)document["warnings"] + " пред(а) ");
					}
				}
				catch
				{
					if (!warns.ContainsKey(player.characterClassManager.UserId)) warns.Add(player.characterClassManager.UserId, "");
				}
			}
			Timing.CallDelayed(1.5f, () =>
			{
				spawnpref(player, list, collection);
			});
			Timing.CallDelayed(1.5f, () =>
			{
				setprefix(player);
			});
		}
		public void spawnpref(ReferenceHub player, List<BsonDocument> list, IMongoCollection<BsonDocument> collection)
		{
			foreach (var document in list)
			{
				if ((bool)document["re"])
				{
					if (document["rt"] >= DateTime.Now)
					{
						var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
						var update = Builders<BsonDocument>.Update.Set("re", false);
						collection.UpdateOne(filter, update);
					}
					else if (DateTime.Now >= document["rt"])
					{
						var component = player.GetComponent<RainbowTagController>();

						if (component == null)
						{
							player.gameObject.AddComponent<RainbowTagController>();
						}
					}
				}

				if ((bool)document["pe"])
				{
					if (document["pt"] >= DateTime.Now)
					{
						var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
						var update = Builders<BsonDocument>.Update.Set("pe", false);
						collection.UpdateOne(filter, update);
					}
					else if (DateTime.Now >= document["pt"])
					{
						ServerStatic.GetPermissionsHandler().GetGroup("premium").BadgeColor = "lime";
						ServerStatic.GetPermissionsHandler().GetGroup("premium").BadgeText = "Premium";
						ServerStatic.GetPermissionsHandler().GetGroup("premium").HiddenByDefault = false;
						ServerStatic.GetPermissionsHandler().GetGroup("premium").Cover = true;
						player.serverRoles.RemoteAdmin = true;
						player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("premium"), false, true, false);
						setprefix(player);
					}
				}
				if ((bool)document["ve"])
				{
					if (document["vt"] >= DateTime.Now)
					{
						var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
						var update = Builders<BsonDocument>.Update.Set("ve", false);
						collection.UpdateOne(filter, update);
					}
					else if (DateTime.Now >= document["vt"])
					{
						ServerStatic.GetPermissionsHandler().GetGroup("vip").BadgeColor = "yellow";
						ServerStatic.GetPermissionsHandler().GetGroup("vip").BadgeText = "ViP";
						ServerStatic.GetPermissionsHandler().GetGroup("vip").HiddenByDefault = false;
						ServerStatic.GetPermissionsHandler().GetGroup("vip").Cover = true;
						player.serverRoles.RemoteAdmin = true;
						player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("vip"), false, true, false);
						setprefix(player);
					}
				}
				if ((bool)document["vpe"])
				{
					if (document["vpt"] >= DateTime.Now)
					{
						var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
						var update = Builders<BsonDocument>.Update.Set("vpe", false);
						collection.UpdateOne(filter, update);
					}
					else if (DateTime.Now >= document["vpt"])
					{
						ServerStatic.GetPermissionsHandler().GetGroup("viplus").BadgeColor = "pumpkin";
						ServerStatic.GetPermissionsHandler().GetGroup("viplus").BadgeText = "ViP+";
						ServerStatic.GetPermissionsHandler().GetGroup("viplus").HiddenByDefault = false;
						ServerStatic.GetPermissionsHandler().GetGroup("viplus").Cover = true;
						player.serverRoles.RemoteAdmin = true;
						player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("viplus"), false, true, false);
						setprefix(player);
					}
				}
				if ((bool)document["ee"])
				{
					if (document["et"] >= DateTime.Now)
					{
						var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
						var update = Builders<BsonDocument>.Update.Set("ee", false);
						collection.UpdateOne(filter, update);
					}
					else if (DateTime.Now >= document["et"])
					{
						var componentr = player.GetComponent<RainbowTagController>();

						if (componentr == null)
						{
							player.gameObject.AddComponent<RainbowTagController>();
						}
						ServerStatic.GetPermissionsHandler().GetGroup("elite").BadgeColor = "emerald";
						ServerStatic.GetPermissionsHandler().GetGroup("elite").BadgeText = "Elite";
						ServerStatic.GetPermissionsHandler().GetGroup("elite").HiddenByDefault = false;
						ServerStatic.GetPermissionsHandler().GetGroup("elite").Cover = true;
						player.serverRoles.RemoteAdmin = true;
						player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("elite"), false, true, false);
						setprefix(player);
					}
				}
				if ((bool)document["sr"])
				{
					ServerStatic.GetPermissionsHandler().GetGroup("sta").BadgeColor = "lime";
					ServerStatic.GetPermissionsHandler().GetGroup("sta").BadgeText = "Стажер";
					ServerStatic.GetPermissionsHandler().GetGroup("sta").HiddenByDefault = false;
					ServerStatic.GetPermissionsHandler().GetGroup("sta").Cover = true;
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["hr"])
				{
					ServerStatic.GetPermissionsHandler().GetGroup("helper").BadgeColor = "aqua";
					ServerStatic.GetPermissionsHandler().GetGroup("helper").BadgeText = "Хелпер";
					ServerStatic.GetPermissionsHandler().GetGroup("helper").HiddenByDefault = false;
					ServerStatic.GetPermissionsHandler().GetGroup("helper").Cover = true;
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["ghr"])
				{
					ServerStatic.GetPermissionsHandler().GetGroup("glhelper").BadgeColor = "cyan";
					ServerStatic.GetPermissionsHandler().GetGroup("glhelper").BadgeText = "Главный хелпер";
					ServerStatic.GetPermissionsHandler().GetGroup("glhelper").HiddenByDefault = false;
					ServerStatic.GetPermissionsHandler().GetGroup("glhelper").Cover = true;
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["ar"])
				{
					ServerStatic.GetPermissionsHandler().GetGroup("admin").BadgeColor = "yellow";
					ServerStatic.GetPermissionsHandler().GetGroup("admin").BadgeText = "Админ";
					ServerStatic.GetPermissionsHandler().GetGroup("admin").HiddenByDefault = false;
					ServerStatic.GetPermissionsHandler().GetGroup("admin").Cover = true;
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["gar"])
				{
					ServerStatic.GetPermissionsHandler().GetGroup("gladmin").BadgeColor = "red";
					ServerStatic.GetPermissionsHandler().GetGroup("gladmin").BadgeText = "Главный Админ";
					ServerStatic.GetPermissionsHandler().GetGroup("gladmin").HiddenByDefault = false;
					ServerStatic.GetPermissionsHandler().GetGroup("gladmin").Cover = true;
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true, false);
					setprefix(player);
				}
				if ((bool)document["or"])
				{
					ServerStatic.GetPermissionsHandler().GetGroup("owner").BadgeColor = "black";
					ServerStatic.GetPermissionsHandler().GetGroup("owner").BadgeText = "Owner";
					ServerStatic.GetPermissionsHandler().GetGroup("owner").HiddenByDefault = false;
					ServerStatic.GetPermissionsHandler().GetGroup("owner").Cover = true;
					player.serverRoles.RemoteAdmin = true;
					player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
					setprefix(player);
				}
				else if (!(bool)document["or"])
				{
					var filter = Builders<BsonDocument>.Filter.Eq("steam", player.characterClassManager.UserId.Replace("@steam", ""));
					var update = Builders<BsonDocument>.Update.Set("or", false);
					collection.UpdateOne(filter, update);
					setprefix(player);
				}
			}
		}
		public void setprefix(ReferenceHub player)
		{
			try
			{
				if (lvl[player.characterClassManager.UserId] == 1)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "green");
				}
				else if (lvl[player.characterClassManager.UserId] == 2)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "crimson");
				}
				else if (lvl[player.characterClassManager.UserId] == 3)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "cyan");
				}
				else if (lvl[player.characterClassManager.UserId] == 4)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "deep_pink");
				}
				else if (lvl[player.characterClassManager.UserId] == 5)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "yellow");
				}
				else if (lvl[player.characterClassManager.UserId] == 6)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "orange");
				}
				else if (lvl[player.characterClassManager.UserId] == 7)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "lime");
				}
				else if (lvl[player.characterClassManager.UserId] == 8)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "pumpkin");
				}
				else if (lvl[player.characterClassManager.UserId] == 9)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "red");
				}
				else if (lvl[player.characterClassManager.UserId] >= 10)
				{
					if (20 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "green");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 20)
				{
					if (30 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "crimson");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 30)
				{
					if (40 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "cyan");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 40)
				{
					if (50 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "deep_pink");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 50)
				{
					if (60 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "yellow");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 60)
				{
					if (70 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "orange");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 70)
				{
					if (80 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "lime");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 80)
				{
					if (90 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "pumpkin");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 90)
				{
					if (100 >= lvl[player.characterClassManager.UserId])
					{
						player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "red");
					}
				}
				else if (lvl[player.characterClassManager.UserId] >= 100)
				{
					player.SetRank(lvl[player.characterClassManager.UserId] + $" {Configs.lvl}", "red");
				}
				try
				{
					if (player.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
					{
						player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]}  {Configs.lvl}", "red");
					}
				}
				catch
				{
					Log.Info("hmm, error in scp228-ru-j");
				}
				try
				{
					if (player.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId)
					{
						player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]}  {Configs.lvl}", "red");
					}
				}
				catch
				{
					Log.Info("hmm, error in scp343");
				}
				if (find[player.characterClassManager.UserId])
				{
					if (rr[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | {prefix[player.characterClassManager.UserId]}", "red");
					}
					if (player.adminsearch())
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | {ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeText} | {prefix[player.characterClassManager.UserId]}", ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeColor);
						return;
					}
					if (er[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | Elite | {prefix[player.characterClassManager.UserId]}", "emerald");
						return;
					}
					if (vpr[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | ViP+", "pumpkin");
						return;
					}
					if (vr[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | ViP", "yellow");
						return;
					}
					if (pr[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | Premium", "lime");
						return;
					}
					if (gar[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | Главный Админ {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
						return;
					}
					if (ar[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | Админ {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "yellow");
						return;
					}
					if (ghr[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | Главный хелпер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "cyan");
						return;
					}
					if (hr[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | Хелпер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "aqua");
						return;
					}
					if (sr[player.characterClassManager.UserId])
					{
						player.SetRank($"{lvl[player.characterClassManager.UserId]} {Configs.lvl} | Стажер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "lime");
						return;
					}
					try
					{
						if (player.queryProcessor.PlayerId == Extensions.TryGet228()?.queryProcessor.PlayerId)
						{
							player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]}  {Configs.lvl}", "red");
							if (rr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | {prefix[player.characterClassManager.UserId]}", "red");
							}
							if (player.adminsearch())
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | {ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeText} | {prefix[player.characterClassManager.UserId]}", "red");
							}
							if (er[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Elite | {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (vpr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | ViP+", "red");
								return;
							}
							if (vr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | ViP", "red");
								return;
							}
							if (pr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Premium", "red");
								return;
							}
							if (gar[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Главный Админ {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (ar[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Админ {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (ghr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Главный хелпер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (hr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Хелпер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (sr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 228 RU J(гопник) | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Стажер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
						}
					}
					catch
					{
						Log.Info("hmm, error in scp228-ru-j");
					}
					try
					{
						if (player.queryProcessor.PlayerId == Extensions.TryGet343()?.queryProcessor.PlayerId)
						{
							player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]}  {Configs.lvl}", "red");
							if (rr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | {prefix[player.characterClassManager.UserId]}", "red");
							}
							if (player.adminsearch())
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | {ServerStatic.GetPermissionsHandler().GetUserGroup(player.characterClassManager.UserId).BadgeText} | {prefix[player.characterClassManager.UserId]}", "red");
							}
							if (er[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Elite | {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (vpr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | ViP+", "red");
								return;
							}
							if (vr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | ViP", "red");
								return;
							}
							if (pr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Premium", "red");
								return;
							}
							if (gar[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Главный Админ {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (ar[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Админ {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (ghr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Главный хелпер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (hr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Хелпер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
							if (sr[player.characterClassManager.UserId])
							{
								player.SetRank($"SCP 343 | {lvl[player.characterClassManager.UserId]} {Configs.lvl} | Стажер {warns[player.characterClassManager.UserId]}| {prefix[player.characterClassManager.UserId]}", "red");
								return;
							}
						}
					}
					catch
					{
						Log.Info("hmm, error in scp343");
					}
				}

			}
			catch { }
		}
	}
}