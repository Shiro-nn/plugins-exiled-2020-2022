using Loli.DataBase.Modules.Controllers;
using MEC;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Events;
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
			InitLoadClans();
			InitLoader();
			InitDonates();
		}
		internal void LoadClans()
		{
			Plugin.Socket.Emit("server.database.clans", new object[] { });
		}
		private static bool _initLoadClans = false;
		private static void InitLoadClans()
		{
			if (_initLoadClans) return;
			Plugin.Socket.On("socket.database.clans", data =>
			{
				var tags = ((System.Collections.IEnumerable)data[0]).Cast<object>().Select(x => x.ToString());
				new Thread(() => { foreach (string tag in tags) try { AddClan(tag); } catch { } }).Start();
			});
			_initLoadClans = true;
		}
		private static void AddClan(string tag)
		{
			List<int> Users = new();
			List<int> Boosts = new();
			{
				var url = $"{Plugin.APIUrl}/clan?tag={tag}&token={Plugin.ApiToken}&type=all_users";
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
				var url = $"{Plugin.APIUrl}/clan?tag={tag}&token={Plugin.ApiToken}&type=boosts";
				var request = WebRequest.Create(url);
				request.Method = "POST";
				using var webResponse = request.GetResponse();
				using var webStream = webResponse.GetResponseStream();
				using var reader = new StreamReader(webStream);
				var data = reader.ReadToEnd();
				ClanBoostsJson json = JsonConvert.DeserializeObject<ClanBoostsJson>(data);
				foreach (int boost in json.boosts) Boosts.Add(boost);
			}
			Data.Clans.Add(tag, new Clan { Tag = tag, Users = Users, Boosts = Boosts });
		}
		internal void Join(JoinEvent ev)
		{
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
			try { LoadData(ev.Player); } catch (Exception ex) { Qurre.Log.Error(ex); }
		}
		private void InitLoader()
		{
			Plugin.Socket.On("database.get.data", obj =>
			{
				string userid = obj[1].ToString();
				var pl = Player.Get(userid);
				if (pl is null) return;
				UserData json = JsonConvert.DeserializeObject<UserData>(obj[0].ToString());
				if (Manager.Data.Users.ContainsKey(userid)) Manager.Data.Users.Remove(userid);
				json.entered = DateTime.Now;
				Manager.Data.Users.Add(userid, json);
				Timing.CallDelayed(0.1f, () => LoadRoles(pl, json));
				Timing.CallDelayed(0.5f, () => Levels.SetPrefix(pl));
			});
		}
		internal void LoadData(Player pl)
		{
			if (Manager.Data.Roles.ContainsKey(pl.UserId)) Manager.Data.Roles.Remove(pl.UserId);
			Manager.Data.Roles.Add(pl.UserId, new DonateRoles());
			if (Manager.Data.Users.ContainsKey(pl.UserId)) Manager.Data.Users.Remove(pl.UserId);
			Plugin.Socket.Emit("database.get.data", new object[] { pl.UserId.Replace("@steam", "").Replace("@discord", ""),
				pl.UserId.Contains("discord"), 1, pl.UserId });
		}
		internal void LoadRoles(Player player, UserData data)
		{
			CustomDonates.CheckGetDonate(player, data.id);
			Plugin.Socket.Emit("database.get.donate.ra", new object[] { data.login, Plugin.DonateID, player.UserId });
			Plugin.Socket.Emit("database.get.donate.roles", new object[] { data.id, Plugin.DonateID, player.UserId });
			Plugin.Socket.Emit("database.get.donate.customize", new object[] { data.login, player.UserId });
			if (data.trainee)
			{
				player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "sta");
				Levels.SetPrefix(player);
			}
			if (data.helper)
			{
				player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "helper");
				Levels.SetPrefix(player);
			}
			if (data.mainhelper)
			{
				player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "glhelper");
				Levels.SetPrefix(player);
			}
			if (data.admin)
			{
				player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "admin");
				Levels.SetPrefix(player);
			}
			if (data.mainadmin)
			{
				player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "gladmin");
				Levels.SetPrefix(player);
			}
			if (data.owner)
			{
				player.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserId, "owner");
				Levels.SetPrefix(player);
			}
		}
		internal static readonly Dictionary<string, RainbowTagController> RainbowRoles = new();
		internal static void InitializeSocket()
		{
			Plugin.Socket.On("ChangeFreezeSCPServer", data =>
			{
				BDDonateRoles doc = JsonConvert.DeserializeObject<BDDonateRoles>($"{data[0]}");
				if (doc.server != -1 && doc.server != Plugin.DonateID) return;
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
		internal void UpdateRole(Player pl)
		{
			UpdateRa();
			UpdateStar();
			Timing.CallDelayed(5f, () => UpdatePriest());
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
					if (pl.UserId.Contains("@northwood")) return;
					if (Manager.Data.Users.TryGetValue(pl.UserId, out var data2) && (data2.donater || data2.id == 1 ||
						data2.trainee || data2.helper || data2.mainhelper || data2.admin || data2.mainadmin || data2.owner)) return;
					if (CustomDonates.ThisDonater(data2)) return;
					if (CustomDonates.ThisYt(data2)) return;
					if (Patrol.List.Contains(pl.UserId)) return;
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
		internal void InitDonates()
		{
			Plugin.Socket.On("database.get.donate.roles", obj =>
			{
				string userid = obj[1].ToString();
				var pl = Player.Get(userid);
				if (pl is null) return;
				int[] roles = JsonConvert.DeserializeObject<int[]>(obj[0].ToString());
				if (!Manager.Data.Roles.TryGetValue(pl.UserId, out var data))
				{
					data = new DonateRoles();
					Manager.Data.Roles.Add(pl.UserId, data);
				}
				foreach (int role in roles)
				{
					switch (role)
					{
						case 1:
							{
								if (!data.Rainbow)
								{
									var component = pl.ReferenceHub.GetComponent<RainbowTagController>();
									if (component == null) component = pl.GameObject.AddComponent<RainbowTagController>();
									if (RainbowRoles.ContainsKey(pl.UserId)) RainbowRoles.Remove(pl.UserId);
									RainbowRoles.Add(pl.UserId, component);
								}
								data._rainbows++;
								break;
							}
						case 2: data._primes++; break;
						case 3: data._priests++; break;
						case 4: data._mages++; break;
						case 5: data._sages++; break;
						case 6: data._stars++; break;
					}
				}
				UpdateRole(pl);
				try { Levels.SetPrefix(pl); } catch { }
			});
			Plugin.Socket.On("database.get.donate.ra", obj =>
			{
				string userid = obj[1].ToString();
				var pl = Player.Get(userid);
				if (pl is null) return;
				BDDonateRA[] donates = JsonConvert.DeserializeObject<BDDonateRA[]>(obj[0].ToString());
				foreach (var donate in donates)
				{
					if (!Manager.Data.Donates.TryGetValue(pl.UserId, out var data))
					{
						var _data = new DonateRA()
						{
							Force = donate.force,
							Give = donate.give,
							Effects = donate.effects,
							ViewRoles = donate.players_roles
						};
						Manager.Data.Donates.Add(pl.UserId, _data);
					}
					else
					{
						if (!data.Force) data.Force = donate.force;
						if (!data.Give) data.Give = donate.give;
						if (!data.Effects) data.Effects = donate.effects;
						if (!data.ViewRoles) data.ViewRoles = donate.players_roles;
					}
					pl.RaLogin();
					if (donate.force)
					{
						if (Manager.Data.forcer.ContainsKey(pl.UserId)) Manager.Data.forcer[pl.UserId] = true;
						else Manager.Data.forcer.Add(pl.UserId, true);
					}
					if (donate.give)
					{
						if (Manager.Data.giver.ContainsKey(pl.UserId)) Manager.Data.giver[pl.UserId] = true;
						else Manager.Data.giver.Add(pl.UserId, true);
					}
					if (donate.effects)
					{
						if (Manager.Data.effecter.ContainsKey(pl.UserId)) Manager.Data.effecter[pl.UserId] = true;
						else Manager.Data.effecter.Add(pl.UserId, true);
					}
					if (!Module.Prefixs.TryGetValue(pl.UserId, out var _pd))
					{
						Module.Prefixs.Add(pl.UserId, new Module.RaPrefix()
						{
							prefix = donate.prefix,
							color = donate.color,
							gameplay_data = donate.players_roles
						});
					}
					else
					{
						_pd.prefix = donate.prefix;
						_pd.color = donate.color;
						if (donate.players_roles) _pd.gameplay_data = donate.players_roles;
					}
					if (Manager.Data.Users.TryGetValue(pl.UserId, out var _d)) _d.donater = true;
				}
			});
		}
	}
}