using Loli.DataBase.Modules.Controllers;
using MEC;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Loli.DataBase.Modules
{
	static class Loader
	{
		static Loader()
		{
			InitLoader();
			InitDonates();
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
				Data.Users.Add(userid, json);
				Timing.CallDelayed(0.1f, () => LoadRoles(pl, json));
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
			Core.Socket.Emit("database.get.donate.ra", new object[] { data.id, Core.DonateID, player.UserInfomation.UserId });
			Core.Socket.Emit("database.get.donate.roles", new object[] { data.id, Core.DonateID, player.UserInfomation.UserId });
			Core.Socket.Emit("database.get.donate.customize", new object[] { data.id, player.UserInfomation.UserId });
			if (data.trainee)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "sta");
			}
			if (data.helper)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "helper");
			}
			if (data.mainhelper)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "glhelper");
			}
			if (data.admin)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "admin");
			}
			if (data.mainadmin)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "gladmin");
			}
			if (data.maincontrol || data.control || data.id == 1)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInfomation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInfomation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInfomation.UserId, "owner");
			}
		}
		internal static readonly Dictionary<string, RainbowTagController> RainbowRoles = new();
		static internal void UpdateRole(Player pl)
		{
			UpdateRa();
			UpdateStar();
			Timing.CallDelayed(5f, () => UpdatePriest());
			void UpdateStar()
			{
				if (!Data.Roles.TryGetValue(pl.UserInfomation.UserId, out var data)) return;
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
				if (!Data.Roles.TryGetValue(pl.UserInfomation.UserId, out var data)) return;
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
				if (!Data.Roles.TryGetValue(pl.UserInfomation.UserId, out var data)) return;
				if (pl.Administrative.RemoteAdmin && !(data.Priest || data.Mage || data.Sage || data.Star))
				{
					if (pl.UserInfomation.UserId.Contains("@northwood")) return;
					if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var data2) && (data2.donater || data2.id == 1 ||
						data2.trainee || data2.helper || data2.mainhelper || data2.admin || data2.mainadmin || data2.control || data2.maincontrol)) return;
					pl.Administrative.RaLogout();
					return;
				}
				if (!pl.Administrative.RemoteAdmin && (data.Priest || data.Mage || data.Sage || data.Star))
				{
					pl.Administrative.RaLogin();
					return;
				}
			}
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
						case 2: data._primes++; break;
						case 3: data._priests++; break;
						case 4: data._mages++; break;
						case 5: data._sages++; break;
						case 6: data._stars++; break;
					}
				}
				UpdateRole(pl);
			});
			Core.Socket.On("database.get.donate.ra", obj =>
			{
				string userid = obj[1].ToString();
				var pl = userid.GetPlayer();
				if (pl is null) return;
				BDDonateRA[] donates = JsonConvert.DeserializeObject<BDDonateRA[]>(obj[0].ToString());
				foreach (var donate in donates)
				{
					if (!Data.Donates.TryGetValue(pl.UserInfomation.UserId, out var data))
					{
						var _data = new DonateRA()
						{
							Force = donate.force,
							Give = donate.give,
							Effects = donate.effects,
							ViewRoles = donate.players_roles
						};
						Data.Donates.Add(pl.UserInfomation.UserId, _data);
					}
					else
					{
						if (!data.Force) data.Force = donate.force;
						if (!data.Give) data.Give = donate.give;
						if (!data.Effects) data.Effects = donate.effects;
						if (!data.ViewRoles) data.ViewRoles = donate.players_roles;
					}
					pl.Administrative.RaLogin();
					if (donate.force)
					{
						if (Data.forcer.ContainsKey(pl.UserInfomation.UserId)) Data.forcer[pl.UserInfomation.UserId] = true;
						else Data.forcer.Add(pl.UserInfomation.UserId, true);
					}
					if (donate.give)
					{
						if (Data.giver.ContainsKey(pl.UserInfomation.UserId)) Data.giver[pl.UserInfomation.UserId] = true;
						else Data.giver.Add(pl.UserInfomation.UserId, true);
					}
					if (donate.effects)
					{
						if (Data.effecter.ContainsKey(pl.UserInfomation.UserId)) Data.effecter[pl.UserInfomation.UserId] = true;
						else Data.effecter.Add(pl.UserInfomation.UserId, true);
					}
					if (!Module.Prefixs.TryGetValue(pl.UserInfomation.UserId, out var _pd))
					{
						Module.Prefixs.Add(pl.UserInfomation.UserId, new Module.RaPrefix()
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
					if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var _d)) _d.donater = true;
				}
			});
		}
	}
}