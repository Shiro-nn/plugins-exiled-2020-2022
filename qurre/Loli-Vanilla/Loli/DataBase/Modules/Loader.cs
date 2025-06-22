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
		[EventMethod(RoundEvents.Waiting)]
		static void NullCall() { }

		static Loader()
		{
			InitializeSocket();
			InitLoader();
			InitDonates();
		}

		[EventMethod(PlayerEvents.Join)]
		static void Join(JoinEvent ev)
		{
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
			});
		}
		static internal void LoadData(Player pl)
		{
			if (Data.Roles.ContainsKey(pl.UserInformation.UserId)) Data.Roles.Remove(pl.UserInformation.UserId);
			Data.Roles.Add(pl.UserInformation.UserId, new DonateRoles());
			if (Data.Users.ContainsKey(pl.UserInformation.UserId)) Data.Users.Remove(pl.UserInformation.UserId);

			string userid = pl.UserInformation.UserId.Replace("@steam", "").Replace("@discord", "");

			Core.Socket.Emit("database.get.data", [ userid,
				pl.UserInformation.UserId.Contains("discord"), 1, pl.UserInformation.UserId ]);
		}
		static void LoadRoles(Player player, UserData data)
		{
			Core.Socket.Emit("database.get.donate.roles", [data.id, -1, player.UserInformation.UserId]);
			Core.Socket.Emit("database.get.donate.customize", [data.login, player.UserInformation.UserId]);
			Core.Socket.Emit("database.get.donate.visual", [data.id, player.UserInformation.UserId]);
			if (data.trainee)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInformation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInformation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInformation.UserId, "sta");
			}
			if (data.helper)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInformation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInformation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInformation.UserId, "helper");
			}
			if (data.mainhelper)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInformation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInformation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInformation.UserId, "glhelper");
			}
			if (data.admin)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInformation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInformation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInformation.UserId, "admin");
			}
			if (data.mainadmin || data.id == 6040)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInformation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInformation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInformation.UserId, "gladmin");
			}
			if (data.maincontrol || data.control || data.id == 1)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInformation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInformation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInformation.UserId, "owner");
			}
			if (data.it)
			{
				player.Administrative.ServerRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("tech"), false, true);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.UserInformation.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.UserInformation.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.UserInformation.UserId, "tech");
			}
		}
		internal static readonly Dictionary<string, RainbowTagController> RainbowRoles = [];
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
							switch (doc.id)
							{
								case 1:
									{
										role._rainbows--;
										if (!role.Rainbow && RainbowRoles.TryGetValue(user.Key, out var _rainbow))
										{
											UnityEngine.Object.Destroy(_rainbow);
										}
										break;
									}
							}
						}
						else
						{
							switch (doc.id)
							{
								case 1:
									{
										if (!role.Rainbow)
										{
											var component = pl.ReferenceHub.GetComponent<RainbowTagController>()
											?? pl.GameObject.AddComponent<RainbowTagController>();

											if (RainbowRoles.ContainsKey(user.Key))
											{
												UnityEngine.Object.Destroy(RainbowRoles[pl.UserInformation.UserId]);
												RainbowRoles.Remove(user.Key);
											}

											RainbowRoles.Add(pl.UserInformation.UserId, component);
										}
										role._rainbows++;
										break;
									}
							}
						}
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
                if (!Data.Roles.TryGetValue(pl.UserInformation.UserId, out var data))
                {
                    data = new DonateRoles();
                    Data.Roles.Add(pl.UserInformation.UserId, data);
                }

                foreach (int role in roles)
                {
                    switch (role)
                    {
                        case 1:
                        {
                            if (!data.Rainbow)
                            {
                                var component = pl.ReferenceHub.GetComponent<RainbowTagController>() ??
                                                pl.GameObject.AddComponent<RainbowTagController>();

                                if (RainbowRoles.ContainsKey(pl.UserInformation.UserId))
                                {
                                    UnityEngine.Object.Destroy(RainbowRoles[pl.UserInformation.UserId]);
                                    RainbowRoles.Remove(pl.UserInformation.UserId);
                                }

                                RainbowRoles.Add(pl.UserInformation.UserId, component);
                            }

                            data._rainbows++;
                            break;
                        }
                    }
                }
            });
        }
    }
}