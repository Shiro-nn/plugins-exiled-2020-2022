using Loli.DataBase.Modules.Controllers;
using MEC;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
namespace Loli.DataBase.Modules
{
	static class Loader
	{
		static Loader()
		{
			InitializeSocket();
			InitLoadClans();
			InitLoader();
			InitDonates();
		}

		static internal void LoadClans()
		{
			Core.Socket.Emit("server.database.clans", new object[] { });
		}

		static bool _initLoadClans = false;
		static void InitLoadClans()
		{
			if (_initLoadClans) return;
			Core.Socket.On("socket.database.clans", data =>
			{
				var tags = ((System.Collections.IEnumerable)data[0]).Cast<object>().Select(x => x.ToString());
				new Thread(() => { foreach (string tag in tags) try { AddClan(tag); } catch { } }).Start();
			});
			_initLoadClans = true;
		}
		static void AddClan(string tag)
		{
			List<int> Users = new();
			List<int> Boosts = new();
			{
				var url = $"{Core.APIUrl}/clan?tag={tag}&token={Core.ApiToken}&type=all_users";
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
				var url = $"{Core.APIUrl}/clan?tag={tag}&token={Core.ApiToken}&type=boosts";
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

		[EventMethod(PlayerEvents.Join)]
		static void Join(JoinEvent ev)
		{
			if (!Data.force.ContainsKey(ev.Player.UserInfomation.UserId)) Data.force.Add(ev.Player.UserInfomation.UserId, 0);
			if (!Data.giveway.ContainsKey(ev.Player.UserInfomation.UserId)) Data.giveway.Add(ev.Player.UserInfomation.UserId, 0);
			if (!Data.effect.ContainsKey(ev.Player.UserInfomation.UserId)) Data.effect.Add(ev.Player.UserInfomation.UserId, DateTime.Now);
			if (!Data.gives.ContainsKey(ev.Player.UserInfomation.UserId)) Data.gives.Add(ev.Player.UserInfomation.UserId, DateTime.Now);
			if (!Data.forces.ContainsKey(ev.Player.UserInfomation.UserId)) Data.forces.Add(ev.Player.UserInfomation.UserId, DateTime.Now);
			if (Data.forcer.ContainsKey(ev.Player.UserInfomation.UserId)) Data.forcer[ev.Player.UserInfomation.UserId] = false;
			else Data.forcer.Add(ev.Player.UserInfomation.UserId, false);
			if (Data.giver.ContainsKey(ev.Player.UserInfomation.UserId)) Data.giver[ev.Player.UserInfomation.UserId] = false;
			else Data.giver.Add(ev.Player.UserInfomation.UserId, false);
			if (Data.effecter.ContainsKey(ev.Player.UserInfomation.UserId)) Data.effecter[ev.Player.UserInfomation.UserId] = false;
			else Data.effecter.Add(ev.Player.UserInfomation.UserId, false);
			if (!Data.scp_play.ContainsKey(ev.Player.UserInfomation.UserId)) Data.scp_play.Add(ev.Player.UserInfomation.UserId, false);
			try { LoadData(ev.Player); } catch (Exception ex) { Log.Error(ex); }
		}
		static void InitLoader()
		{
			Core.Socket.On("database.get.data", obj =>
			{
				string userid = obj[1].ToString();
				var pl = userid.GetPlayer();
				if (pl is null) return;
				UserData json = JsonConvert.DeserializeObject<UserData>(obj[0].ToString());
				if (Data.Users.ContainsKey(userid)) Data.Users.Remove(userid);
				json.entered = DateTime.Now;
				Data.Users.Add(userid, json);
				Timing.CallDelayed(0.1f, () => LoadRoles(pl, json));
				Timing.CallDelayed(0.5f, () => Levels.SetPrefix(pl));
			});
		}
		static internal void LoadData(Player pl)
		{
			if (Data.Roles.ContainsKey(pl.UserInfomation.UserId)) Data.Roles.Remove(pl.UserInfomation.UserId);
			Data.Roles.Add(pl.UserInfomation.UserId, new DonateRoles());
			if (Data.Users.ContainsKey(pl.UserInfomation.UserId)) Data.Users.Remove(pl.UserInfomation.UserId);
			Core.Socket.Emit("database.get.data", new object[] { pl.UserInfomation.UserId.Replace("@steam", "").Replace("@discord", ""),
				pl.UserInfomation.UserId.Contains("discord"), 1, pl.UserInfomation.UserId });
		}
		static void LoadRoles(Player player, UserData data)
		{
			Core.Socket.Emit("database.get.donate.roles", new object[] { data.id, -1, player.UserInfomation.UserId });
			if (data.trainee)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "sta");
				Levels.SetPrefix(player);
			}
			if (data.helper)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "helper");
				Levels.SetPrefix(player);
			}
			if (data.mainhelper)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "glhelper");
				Levels.SetPrefix(player);
			}
			if (data.admin)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "admin");
				Levels.SetPrefix(player);
			}
			if (data.mainadmin)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "gladmin");
				Levels.SetPrefix(player);
			}
			if (data.owner)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "owner");
				Levels.SetPrefix(player);
			}
		}
		internal static readonly Dictionary<string, RainbowTagController> RainbowRoles = new();
		internal static void InitializeSocket()
		{
			Core.Socket.On("ChangeFreezeSCPServer", data =>
			{
				BDDonateRoles doc = JsonConvert.DeserializeObject<BDDonateRoles>($"{data[0]}");
				if (doc.server != -1) return;
				foreach (var user in Data.Users.Where(x => x.Key is not null && x.Value is not null && x.Value.id == doc.owner))
				{
					var pl = user.Key.GetPlayer();
					if (pl is null) continue;
					if (Data.Roles.TryGetValue(user.Key, out var role))
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
									RainbowRoles.Add(pl.UserInfomation.UserId, component);
								}
								role._rainbows++;
							}
						}
						try { Levels.SetPrefix(pl); } catch { }
					}
				}
			});
		}
		static internal void InitDonates()
		{
			Core.Socket.On("database.get.donate.roles", obj =>
			{
				string userid = obj[1].ToString();
				var pl = userid.GetPlayer();
				if (pl is null) return;
				int[] roles = JsonConvert.DeserializeObject<int[]>(obj[0].ToString());
				if (!Data.Roles.TryGetValue(pl.UserInfomation.UserId, out var data))
				{
					data = new DonateRoles();
					Data.Roles.Add(pl.UserInfomation.UserId, data);
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
									if (RainbowRoles.ContainsKey(pl.UserInfomation.UserId)) RainbowRoles.Remove(pl.UserInfomation.UserId);
									RainbowRoles.Add(pl.UserInfomation.UserId, component);
								}
								data._rainbows++;
								break;
							}
					}
				}
				try { Levels.SetPrefix(pl); } catch { }
			});
		}
	}
}