using Loli.DataBase.Modules.Controllers;
using MEC;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Events;
using QurreSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
namespace Loli.DataBase.Modules
{
	internal class Loader
	{
		internal readonly Manager Manager;
		internal Loader(Manager manager)
		{
			Manager = manager;
			InitializeSocket();
		}
		internal void LoadClans()
		{
			new Thread(() =>
			{
				try
				{
					var collection = Manager.DataBase.GetCollection("clans");
					var list = collection.Find(_ => true).ToList();
					Data.Clans.Clear();
					foreach (var document in list)
					{
						List<int> Users = new();
						List<int> Boosts = new();
						{
							var url = "https://" + $"scpsl.store/api/clan?type=all_users&tag={(string)document["tag"]}&token={Plugin.ApiToken}";
							var request = WebRequest.Create(url);
							request.Method = "POST";
							using var webResponse = request.GetResponse();
							using var webStream = webResponse.GetResponseStream();
							using var reader = new StreamReader(webStream);
							var data = reader.ReadToEnd();
							ClanUsersJson json = JsonConvert.DeserializeObject<ClanUsersJson>(data);
							foreach (int user in json.lvl1) Users.Add(user);
							foreach (int user in json.lvl2) Users.Add(user);
							foreach (int user in json.lvl3) Users.Add(user);
							foreach (int user in json.lvl4) Users.Add(user);
							foreach (int user in json.lvl5) Users.Add(user);
						}
						{
							var url = "https://" + $"scpsl.store/api/clan?type=boosts&tag={(string)document["tag"]}&token={Plugin.ApiToken}";
							var request = WebRequest.Create(url);
							request.Method = "POST";
							using var webResponse = request.GetResponse();
							using var webStream = webResponse.GetResponseStream();
							using var reader = new StreamReader(webStream);
							var data = reader.ReadToEnd();
							ClanBoostsJson json = JsonConvert.DeserializeObject<ClanBoostsJson>(data);
							foreach (int boost in json.boosts) Boosts.Add(boost);
						}
						Data.Clans.Add((string)document["tag"], new Clan { Tag = (string)document["tag"], Users = Users, Boosts = Boosts });
					}
				}
				catch
				{
					Timing.CallDelayed(5f, () => LoadClans());
				}
			}).Start();
		}
		internal void Join(JoinEvent ev)
		{
			if (string.IsNullOrEmpty(ev.Player.UserId) || ev.Player.IsHost || ev.Player.Nickname == "Dedicated Server") return;
			if (!Manager.Data.force.ContainsKey(ev.Player.UserId)) Manager.Data.force.Add(ev.Player.UserId, 0);
			if (!Manager.Data.giveway.ContainsKey(ev.Player.UserId)) Manager.Data.giveway.Add(ev.Player.UserId, 0);
			if (!Manager.Data.effect.ContainsKey(ev.Player.UserId)) Manager.Data.effect.Add(ev.Player.UserId, DateTime.Now);
			if (!Manager.Data.gives.ContainsKey(ev.Player.UserId)) Manager.Data.gives.Add(ev.Player.UserId, DateTime.Now);
			if (!Manager.Data.forces.ContainsKey(ev.Player.UserId)) Manager.Data.forces.Add(ev.Player.UserId, DateTime.Now);
			if (Manager.Data.forcer.ContainsKey(ev.Player.UserId)) Manager.Data.forcer[ev.Player.UserId] = false;
			else Manager.Data.forcer.Add(ev.Player.UserId, false);
			if (Manager.Data.giver.ContainsKey(ev.Player.UserId)) Manager.Data.giver[ev.Player.UserId] = false;
			else Manager.Data.giver.Add(ev.Player.UserId, false);
			if (Manager.Data.effecter.ContainsKey(ev.Player.UserId)) Manager.Data.effecter[ev.Player.UserId] = false;
			else Manager.Data.effecter.Add(ev.Player.UserId, false);
			if (!Manager.Data.scp_play.ContainsKey(ev.Player.UserId)) Manager.Data.scp_play.Add(ev.Player.UserId, false);
			new Thread(() =>
			{
				try { LoadData(ev.Player); }
				catch (Exception ex)
				{
					Qurre.Log.Error(ex);
				}
			}).Start();
		}
		internal void LoadData(Player pl)
		{
			if (Manager.Data.Roles.ContainsKey(pl.UserId)) Manager.Data.Roles.Remove(pl.UserId);
			Manager.Data.Roles.Add(pl.UserId, new DonateRoles());
			#region create data
			if (Manager.Data.Users.ContainsKey(pl.UserId)) Manager.Data.Users.Remove(pl.UserId);
			string uu = "steam";
			if (pl.UserId.Contains("@discord")) uu = "discord";
			var collection = Manager.DataBase.GetCollection("accounts");
			var stata = Manager.DataBase.MongoDatabase.GetCollection<User_Stats>("stats");
			var filter = Builders<BsonDocument>.Filter.Eq(uu, pl.UserId.Replace($"@{uu}", ""));
			var list = collection.Find(new BsonDocument(uu, pl.UserId.Replace($"@{uu}", ""))).ToList();
			var statsa = stata.Find(new BsonDocument(uu, pl.UserId.Replace($"@{uu}", ""))).ToList();
			if (statsa.Count == 0)
			{
				var sta = Manager.DataBase.MongoDatabase.GetCollection<User_Stats>("stats");
				if (pl.UserId.Contains("@discord")) sta.InsertOneAsync(new User_Stats { discord = pl.UserId.Replace("@discord", "") });
				else sta.InsertOneAsync(new User_Stats { steam = pl.UserId.Replace("@steam", "") });
				Manager.Data.Users.Add(pl.UserId, CreateData(pl.UserId, money: 0, xp: 0, lvl: 1, to: 750));
			}
			else
			{
				var document = statsa.First();
				Manager.Data.Users.Add(pl.UserId, CreateData(pl.UserId, money: document.money, xp: document.xp, lvl: document.lvl, to: document.to));
			}
			if (list.Count > 1)
			{
				list.Reverse();
				foreach (var document in list)
				{
					var data = Manager.Data.Users[pl.UserId];
					if ((((bool)document["sr"] || (bool)document["hr"]
						|| (bool)document["ghr"] || (bool)document["ar"] || (bool)document["gar"]
						|| (bool)document["asr"] || (bool)document["dcr"] || (bool)document["or"])
						&& data.name != "[data deleted]") || data.name == "[data deleted]")
					{
						data.admintime = (int)document["adminmin"];
						data.now = DateTime.Now;
						data.prefix = ((string)document["prefix"]).Replace("<", "").Replace(">", "");
						data.clan = (string)document["clan"];
						data.find = true;
						data.sr = (bool)document["sr"];
						data.hr = (bool)document["hr"];
						data.ghr = (bool)document["ghr"];
						data.ar = (bool)document["ar"];
						data.gar = (bool)document["gar"];
						data.asr = (bool)document["asr"];
						data.dcr = (bool)document["dcr"];
						data.or = (bool)document["or"];
						data.name = (string)document["name"];
						if(data.name == "" || data.name == null) data.name = (string)document["user"];
						data.id = (int)document["id"];
						data.discord = $"{document["discord"]}";
						try
						{
							if ((int)document["warnings"] != 0)
							{
								data.warns = "| " + (int)document["warnings"] + " пред(а) ";
							}
						}
						catch
						{ }
					}
				}
			}
			else
			{
				var data = Manager.Data.Users[pl.UserId];
				foreach (var document in list)
				{
					data.admintime = (int)document["adminmin"];
					data.now = DateTime.Now;
					data.prefix = ((string)document["prefix"]).Replace("<", "").Replace(">", "");
					data.clan = (string)document["clan"];
					data.find = true;
					data.sr = (bool)document["sr"];
					data.hr = (bool)document["hr"];
					data.ghr = (bool)document["ghr"];
					data.ar = (bool)document["ar"];
					data.gar = (bool)document["gar"];
					data.asr = (bool)document["asr"];
					data.dcr = (bool)document["dcr"];
					data.or = (bool)document["or"];
					data.name = (string)document["name"];
					if (data.name == "" || data.name == null) data.name = (string)document["user"];
					data.id = (int)document["id"];
					data.discord = $"{document["discord"]}";
					try
					{
						if ((int)document["warnings"] != 0)
						{
							data.warns = "| " + (int)document["warnings"] + " пред(а) ";
						}
					}
					catch
					{ }
				}
			}
			#endregion
			#region set prefix
			Timing.CallDelayed(1f, () => LoadRoles(pl, list));
			Timing.CallDelayed(1.5f, () => Levels.SetPrefix(pl));
			#endregion
			static UserData CreateData(string userId, int money, int xp, int lvl, int to)
			{
				return new UserData()
				{
					UserId = userId,
					now = DateTime.Now,
					money = money,
					xp = xp,
					lvl = lvl,
					to = to
				};
			}
		}
		internal void LoadRoles(Player player, List<BsonDocument> list)
		{
			foreach (var document in list)
			{
				GetDonate(player, (string)document["user"]);
				GetRoles(player, (int)document["id"]);
				CustomDonates.CheckGetDonate(player, (int)document["id"]);
				if ((bool)document["sr"])
				{
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "sta");
					Levels.SetPrefix(player);
				}
				if ((bool)document["hr"])
				{
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "helper");
					Levels.SetPrefix(player);
				}
				if ((bool)document["ghr"])
				{
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "glhelper");
					Levels.SetPrefix(player);
				}
				if ((bool)document["ar"])
				{
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "admin");
					Levels.SetPrefix(player);
				}
				if ((bool)document["gar"])
				{
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "gladmin");
					Levels.SetPrefix(player);
				}
				if ((bool)document["asr"] || (bool)document["dcr"])
				{
					Levels.SetPrefix(player);
				}
				if ((bool)document["or"])
				{
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "owner");
					Levels.SetPrefix(player);
				}
				if ((int)document["id"] == 3053)
				{
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "owner");
					Levels.SetPrefix(player);
				}
				if (Plugin.YTAcess && (bool)document["ytr"])
				{
					if (Manager.Data.Users.ContainsKey(player.UserId)) Manager.Data.Users[player.UserId].ytr = (bool)document["ytr"];
					player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("yt"), false, true, false);
					if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
					ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "yt");
					Levels.SetPrefix(player);
				}
			}
		}
		internal static readonly Dictionary<string, RainbowTagController> RainbowRoles = new();
		internal static void InitializeSocket()
		{
			var client = new Client(999, Plugin.SocketIP);
			client.On("connect", data => client.Emit("SCPServerInit", new string[] { Plugin.ApiToken }));
			client.On("ChangeFreezeSCPServer", data =>
			{
				BDDonateRoles doc = JsonConvert.DeserializeObject<BDDonateRoles>($"{data[0]}");
				if (doc.server != -1 && doc.server != Plugin.ServerID) return;
				foreach (var user in Manager.Static.Data.Users.Where(x => x.Key is not null && x.Value is not null && x.Value.id == doc.owner))
				{
					var pl = Player.Get(user.Key);
					if (pl is null) continue;
					if (Manager.Static.Data.Roles.TryGetValue(user.Key, out var role))
					{
						if (doc.freezed)
						{
							if (doc.id == 1)
							{
								role._rainbows--;
								if (!role.Rainbow && RainbowRoles.TryGetValue(user.Key, out var _rainbow))
								{
									UnityEngine.Object.Destroy(_rainbow);
								}
							}
							else if (doc.id == 2) role._primes--;
							else if (doc.id == 3) role._priests--;
							else if (doc.id == 4) role._mages--;
							else if (doc.id == 5) role._sages--;
							else if (doc.id == 6) role._stars--;
						}
						else
						{
							if (doc.id == 1)
							{
								if (!role.Rainbow)
								{
									var component = pl.ReferenceHub.GetComponent<RainbowTagController>();
									if (component == null) component = pl.GameObject.AddComponent<RainbowTagController>();
									if (RainbowRoles.ContainsKey(user.Key)) RainbowRoles.Remove(user.Key);
									RainbowRoles.Add(pl.UserId, component);
								}
								role._rainbows++;
							}
							else if (doc.id == 2) role._primes++;
							else if (doc.id == 3) role._priests++;
							else if (doc.id == 4) role._mages++;
							else if (doc.id == 5) role._sages++;
							else if (doc.id == 6) role._stars++;
						}
						Manager.Static.Loader.UpdateRole(pl);
						try { Levels.SetPrefix(pl); } catch { }
					}
				}
			});
		}
		internal void GetRoles(Player pl, int web_id)
		{
			var collection = Manager.DataBase.GetCollection("roles");
			var list = collection.Find(new BsonDocument("owner", web_id)).ToList();
			foreach (var docu in list)
			{
				BDDonateRoles doc = new()
				{
					id = (int)docu["id"],
					server = (int)docu["server"],
					freezed = (bool)docu["freezed"],
				};
				if (!doc.freezed && (doc.server == -1 || doc.server == Plugin.ServerID))
				{
					if (!Manager.Data.Roles.ContainsKey(pl.UserId))
					{
						var _data = new DonateRoles();
						Manager.Data.Roles.Add(pl.UserId, _data);
					}
					var data = Manager.Data.Roles[pl.UserId];
					if (doc.id == 1)
					{
						if (!data.Rainbow)
						{
							var component = pl.ReferenceHub.GetComponent<RainbowTagController>();
							if (component == null) component = pl.GameObject.AddComponent<RainbowTagController>();
							if (RainbowRoles.ContainsKey(pl.UserId)) RainbowRoles.Remove(pl.UserId);
							RainbowRoles.Add(pl.UserId, component);
						}
						data._rainbows++;
					}
					else if (doc.id == 2) data._primes++;
					else if (doc.id == 3) data._priests++;
					else if (doc.id == 4) data._mages++;
					else if (doc.id == 5) data._sages++;
					else if (doc.id == 6) data._stars++;
				}
			}
			UpdateRole(pl);
			try { Levels.SetPrefix(pl); } catch { }
		}
		internal void UpdateRole(Player pl)
		{
			UpdateRa();
			UpdateStar();
			UpdatePriest();
			void UpdateStar()
			{
				if (!Manager.Data.Roles.TryGetValue(pl.UserId, out var data)) return;
				if (!data.Star)
				{
					var __ = Star.Get(pl);
					if (__ is null) return;
					__.Break();
					return;
				}
				if (Star.Get(pl) is not null) return;
				new Star(pl);
			}
			void UpdatePriest()
			{
				if (!Manager.Data.Roles.TryGetValue(pl.UserId, out var data)) return;
				if (!data.Priest)
				{
					var __ = Priest.Get(pl);
					if (__ is null) return;
					__.Break();
					return;
				}
				if (Priest.Get(pl) is not null) return;
				new Priest(pl);
			}
			void UpdateRa()
			{
				if (!Manager.Data.Roles.TryGetValue(pl.UserId, out var data)) return;
				if (pl.RemoteAdminAccess && !(data.Priest || data.Mage || data.Sage || data.Star))
				{
					if (Manager.Data.Users.TryGetValue(pl.UserId, out var data2) && (data2.don ||
						data2.sr || data2.hr || data2.ghr || data2.ar || data2.gar || data2.or)) return;
					pl.RaLogout();
					return;
				}
				if (!pl.RemoteAdminAccess && (data.Priest || data.Mage || data.Sage || data.Star))
				{
					pl.RaLogin();
					return;
				}
			}
		}
		internal void GetDonate(Player pl, string web_name)
		{
			try { Customize.Static.Get(pl, web_name); } catch { }
			var collection = Manager.DataBase.GetCollection("donates");
			var list = collection.Find(new BsonDocument("owner", web_name));
			foreach (var document in list)
			{
				if ((int)document["server"] == Plugin.ServerID)
				{
					if (!Manager.Data.Donates.TryGetValue(pl.UserId, out var data))
					{
						var _data = new Ra_Cfg()
						{
							force = (bool)document["force"],
							give = (bool)document["give"],
							effects = (bool)document["effects"],
							players_roles = (bool)document["players_roles"]
						};
						Manager.Data.Donates.Add(pl.UserId, _data);
					}
					else
					{
						if (!data.force) Manager.Data.Donates[pl.UserId].force = (bool)document["force"];
						if (!data.give) Manager.Data.Donates[pl.UserId].give = (bool)document["give"];
						if (!data.effects) Manager.Data.Donates[pl.UserId].effects = (bool)document["effects"];
						if (!data.players_roles) Manager.Data.Donates[pl.UserId].players_roles = (bool)document["players_roles"];
					}
					pl.RaLogin();
					if ((bool)document["force"])
					{
						if (Manager.Data.forcer.ContainsKey(pl.UserId)) Manager.Data.forcer[pl.UserId] = true;
						else Manager.Data.forcer.Add(pl.UserId, true);
					}
					if ((bool)document["give"])
					{
						if (Manager.Data.giver.ContainsKey(pl.UserId)) Manager.Data.giver[pl.UserId] = true;
						else Manager.Data.giver.Add(pl.UserId, true);
					}
					if ((bool)document["effects"])
					{
						if (Manager.Data.effecter.ContainsKey(pl.UserId)) Manager.Data.effecter[pl.UserId] = true;
						else Manager.Data.effecter.Add(pl.UserId, true);
					}
					if (!Module.Prefixs.ContainsKey(pl.UserId))
					{
						Module.Prefixs.Add(pl.UserId, new Module.ra_pref());
						Module.Prefixs[pl.UserId].prefix = (string)document["prefix"];
						Module.Prefixs[pl.UserId].color = (string)document["color"];
						Module.Prefixs[pl.UserId].gameplay_data = (bool)document["players_roles"];
					}
					else
					{
						Module.Prefixs[pl.UserId].prefix = (string)document["prefix"];
						Module.Prefixs[pl.UserId].color = (string)document["color"];
						if ((bool)document["players_roles"]) Module.Prefixs[pl.UserId].gameplay_data = (bool)document["players_roles"];
					}
					if (Manager.Data.Users.ContainsKey(pl.UserId)) Manager.Data.Users[pl.UserId].don = true;
				}
			}
		}
	}
}