using HarmonyLib;
using InventorySystem;
using MEC;
using Newtonsoft.Json;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Core.Interfaces;
using PluginAPI.Enums;
using RoundRestarting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RoundSummary;

namespace Loli_Time
{
	public static class Events
	{
		static public void Wait()
		{
			ServerConsole.AddLog("waiting", ConsoleColor.Blue);
			try { Class1.Donates.Clear(); } catch { }
			try { Class1.Users.Clear(); } catch { }
			try { Class1.giveway.Clear(); } catch { }
			try { Class1.force.Clear(); } catch { }
			try { Class1.effect.Clear(); } catch { }
			try { Class1.gives.Clear(); } catch { }
			try { Class1.giver.Clear(); } catch { }
			try { Class1.forces.Clear(); } catch { }
			try { Class1.forcer.Clear(); } catch { }
			try { Class1.effecter.Clear(); } catch { }
			try { Class1.scp_play.Clear(); } catch { }
		}

		static public void Start()
		{
			ServerConsole.AddLog("started", ConsoleColor.Blue);
			Class1.start = DateTime.Now;
			Class1.AlphaDet = false;
			Class1.StartedRound = true;
		}

		static public void alpha()
		{
			Timing.CallDelayed(10f, () =>
			{
				RoundRestart.InitiateRoundRestart();
				Timing.CallDelayed(1f, () =>
				{
					Server.Restart();
				});
			});
			ServerConsole.AddLog("detonated", ConsoleColor.Blue);
			Class1.AlphaDet = true;
			Round.End();
			RoundSummary.singleton.RpcShowRoundSummary(default, default, LeadingTeam.Draw, 666, 777, 555, 666, 12345);
		}

		static public void spawn(ReferenceHub player, RoleTypeId role)
		{
			ServerConsole.AddLog("spawned", ConsoleColor.Blue);
			if (role.GetTeam() == Team.SCPs)
			{
				if (!Class1.scp_play.ContainsKey(player.characterClassManager.UserId)) Class1.scp_play.Add(player.characterClassManager.UserId, true);
				else Class1.scp_play[player.characterClassManager.UserId] = true;
			}
		}

		static public void Leave(ReferenceHub player)
		{
			ServerConsole.AddLog("leave", ConsoleColor.Blue);
			Class1.Socket.Emit("server.leave", new object[] { Class1.ServerID, player.characterClassManager.connectionToClient.address });
			if (Class1.Users.ContainsKey(player.characterClassManager.UserId)) Class1.Users.Remove(player.characterClassManager.UserId);
		}

		static public void LoadData(string userid)
		{
			if (Class1.Roles.ContainsKey(userid)) Class1.Roles.Remove(userid);
			Class1.Roles.Add(userid, new DonateRoles());
			Class1.Socket.Emit("database.get.data", new object[] { userid.Replace("@steam", "").Replace("@discord", ""),
				userid.Contains("discord"), 1, userid });
			ServerConsole.AddLog("sended request", ConsoleColor.Green);
		}

		static public void Join(CharacterClassManager ccm, string userid)
		{
			Timing.CallDelayed(1f, () => Class1.Socket.Emit("server.join", new object[] { userid, ccm.connectionToClient.address }));
			try { Class1.Socket.Emit("server.addip", new object[] { Class1.ServerID, ccm.connectionToClient.address }); } catch { }
			ServerConsole.AddLog("JOINED", ConsoleColor.Red);
			try { LoadData(userid); } catch (Exception e) { ServerConsole.AddLog($"{e}", ConsoleColor.Red); }
			ServerConsole.AddLog(userid, ConsoleColor.Red);
			if (!Class1.force.ContainsKey(userid)) Class1.force.Add(userid, 0);
			if (!Class1.giveway.ContainsKey(userid)) Class1.giveway.Add(userid, 0);
			if (!Class1.effect.ContainsKey(userid)) Class1.effect.Add(userid, DateTime.Now);
			if (!Class1.gives.ContainsKey(userid)) Class1.gives.Add(userid, DateTime.Now);
			if (!Class1.forces.ContainsKey(userid)) Class1.forces.Add(userid, DateTime.Now);
			if (Class1.forcer.ContainsKey(userid)) Class1.forcer[userid] = false;
			else Class1.forcer.Add(userid, false);
			if (Class1.giver.ContainsKey(userid)) Class1.giver[userid] = false;
			else Class1.giver.Add(userid, false);
			if (Class1.effecter.ContainsKey(userid)) Class1.effecter[userid] = false;
			else Class1.effecter.Add(userid, false);
			if (!Class1.scp_play.ContainsKey(userid)) Class1.scp_play.Add(userid, false);
		}
	}
	public class Class1
	{
		public static Class1 Singleton;
		public static DateTime start = DateTime.Now;
		public static bool AlphaDet = false;
		public static bool StartedRound = false;
		public static bool Ended = false;
		internal static int DonateLimint => 5;
		internal static string ApiToken => "-";
		internal static string SocketIP => "45.142.122.184";
		internal static readonly QurreSocket.Client Socket = new(2467, SocketIP);

		internal const int ServerID = 4;

		internal static readonly Dictionary<string, UserData> Users = new();
		static internal readonly Dictionary<string, DonateRoles> Roles = new();
		static internal readonly Dictionary<string, DonateRA> Donates = new();
		static internal readonly Dictionary<string, int> force = new();
		static internal readonly Dictionary<string, int> giveway = new();
		static internal readonly Dictionary<string, DateTime> effect = new();
		static internal readonly Dictionary<string, DateTime> gives = new();
		static internal readonly Dictionary<string, DateTime> forces = new();
		static internal readonly Dictionary<string, bool> giver = new();
		static internal readonly Dictionary<string, bool> forcer = new();
		static internal readonly Dictionary<string, bool> effecter = new();
		static internal readonly Dictionary<string, bool> scp_play = new();

		internal static readonly Dictionary<string, RainbowTagController> RainbowRoles = new();

		static internal Harmony _harmony;

		static void CheckRound()
		{
			try
			{
				if ((DateTime.Now - start).TotalMinutes < 1) return;
				if ((DateTime.Now - start).TotalMinutes > 20)
				{
					if (singleton is null) singleton = new();
					Round.End();
					Ended = true;
					Timing.CallDelayed(10f, () =>
					{
						RoundRestart.InitiateRoundRestart();
						Timing.CallDelayed(1f, () =>
						{
							Server.Restart();
						});
					});
					return;
				}
				if (!StartedRound) return;
				if (Ended) return;
				bool end = false;
				int cw = 0;
				int mw = 0;
				int sw = 0;
				int nd = 0;
				int ns = 0;
				int ci = 0;
				int mtf = 0;
				int scp = 0;
				int sh = 0;
				int zombies = 0;
				foreach (var pl in ReferenceHub.AllHubs)
				{
					switch (pl.GetRoleId().GetTeam())
					{
						case Team.ClassD:
							nd++;
							break;
						case Team.Scientists:
							ns++;
							break;
						case Team.ChaosInsurgency:
							ci++;
							break;
						case Team.FoundationForces:
							mtf++;
							break;
						case Team.SCPs:
							{
								scp++;
								if (pl.GetRoleId() is RoleTypeId.Scp0492) zombies++;
								break;
							}
					}
				}
				int d = nd;
				int s = ns;
				bool MTFAlive = mtf > 0;
				bool CiAlive = ci > 0;
				bool ScpAlive = scp > 0;
				bool DClassAlive = nd > 0;
				bool ScientistsAlive = ns > 0;
				bool SHAlive = sh > 0;
				var cList = new RoundSummary.SumInfo_ClassList
				{
					class_ds = d,
					scientists = s,
					chaos_insurgents = ci,
					mtf_and_guards = mtf,
					scps_except_zombies = scp - zombies,
					zombies = zombies
				};
				if ((SHAlive || ScpAlive) && !MTFAlive && !DClassAlive && !ScientistsAlive)
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
				else if (!ScpAlive && !MTFAlive && !ScientistsAlive && !DClassAlive && !CiAlive)
				{
					end = true;
				}
				if (end)
				{
					ServerConsole.AddLog($"{RoundSummary.singleton is null}", ConsoleColor.Magenta);
					if (RoundSummary.singleton is null) RoundSummary.singleton = new RoundSummary();
					LeadingTeam leading;
					Round.End();
					if (d > s) cw++;
					else if (d < s) mw++;
					else if (scp > d + s) sw++;
					if (ci > mtf) cw++;
					else if (ci < mtf) mw++;
					else if (scp > ci + mtf) sw++;
					if (cw > mw)
					{
						if (cw > sw) leading = LeadingTeam.ChaosInsurgency;
						else if (mw < sw) leading = LeadingTeam.Anomalies;
						else leading = LeadingTeam.Draw;
					}
					else if (mw > cw)
					{
						if (mw > sw) leading = LeadingTeam.FacilityForces;
						else if (cw < sw) leading = LeadingTeam.Anomalies;
						else leading = LeadingTeam.Draw;
					}
					else leading = LeadingTeam.Draw;
					RoundSummary.singleton.RpcShowRoundSummary(cList, cList, leading, d, s, scp, 666, 12345);
					Ended = true;
					Timing.CallDelayed(10f, () =>
					{
						RoundRestart.InitiateRoundRestart();
						Timing.CallDelayed(1f, () =>
						{
							Server.Restart();
						});
					});

				}
			}
			catch { }
		}
		IEnumerator<float> Сycle()
		{
			for (; ; )
			{
				try { CheckRound(); } catch { }
				yield return Timing.WaitForSeconds(1f);
			}
		}

		[PluginEntryPoint("Loli", "0.0.0", "hello", "fydne")]
        public void Init()
		{
			try { if (_harmony is not null) _harmony.UnpatchAll(); } catch { }
			try
			{
				Timing.RunCoroutine(Сycle());
			}
			catch { }
			_harmony = new Harmony("loli.patches");
			_harmony.PatchAll();
			Singleton = this;
			ServerConsole.AddLog("Loli <3", ConsoleColor.Red);
			Socket.On("connect", data => Socket.Emit("SCPServerInit", new string[] { ApiToken }));
			Socket.On("connect", data =>
			{
				ServerConsole.AddLog("Connected", ConsoleColor.Magenta);
				Timing.CallDelayed(1f, () => Socket.Emit("server.clearips", new object[] { ServerID }));
				Timing.CallDelayed(2f, () =>
				{
					try { foreach (var pl in ReferenceHub.AllHubs) Socket.Emit("server.addip", new object[] { ServerID, pl.characterClassManager.connectionToClient.address }); } catch { }
				});
			});
			Socket.On("ChangeFreezeSCPServer", data =>
			{
				BDDonateRoles doc = JsonConvert.DeserializeObject<BDDonateRoles>($"{data[0]}");
				if (doc.server != -1 && doc.server != 1) return;
				foreach (var user in Users.Where(x => x.Key is not null && x.Value is not null && x.Value.id == doc.owner))
				{
					ReferenceHub pl = ReferenceHub.AllHubs.FirstOrDefault(x => x.characterClassManager.UserId == user.Key);
					if (pl is null) continue;
					if (Roles.TryGetValue(user.Key, out var role))
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
									var component = pl.GetComponent<RainbowTagController>();
									if (component == null) component = pl.gameObject.AddComponent<RainbowTagController>();
									if (RainbowRoles.ContainsKey(user.Key)) RainbowRoles.Remove(user.Key);
									RainbowRoles.Add(pl.characterClassManager.UserId, component);
								}
								role._rainbows++;
							}
							else if (doc.id == 2) role._primes++;
							else if (doc.id == 3) role._priests++;
							else if (doc.id == 4) role._mages++;
							else if (doc.id == 5) role._sages++;
							else if (doc.id == 6) role._stars++;
						}
						UpdateRole(pl);
					}
				}
			});
			Socket.On("database.get.data", obj =>
			{
				string userid = obj[1].ToString();
				ServerConsole.AddLog(userid, ConsoleColor.Red);
				ReferenceHub pl = ReferenceHub.AllHubs.FirstOrDefault(x => x.characterClassManager.UserId == userid);
				ServerConsole.AddLog($"{pl is null}", ConsoleColor.Red);
				if (pl is null) return;
				UserData json = JsonConvert.DeserializeObject<UserData>(obj[0].ToString());
				if (Users.ContainsKey(userid)) Users.Remove(userid);
				json.entered = DateTime.Now;
				Users.Add(userid, json);
				Timing.CallDelayed(0.1f, () => LoadRoles(pl, json));
			});
			Socket.On("database.get.donate.roles", obj =>
			{
				string userid = obj[1].ToString();
				ReferenceHub pl = ReferenceHub.AllHubs.FirstOrDefault(x => x.characterClassManager.UserId == userid);
				if (pl is null) return;
				int[] roles = JsonConvert.DeserializeObject<int[]>(obj[0].ToString());
				if (!Roles.TryGetValue(pl.characterClassManager.UserId, out var data))
				{
					data = new DonateRoles();
					Roles.Add(pl.characterClassManager.UserId, data);
				}
				foreach (int role in roles)
				{
					switch (role)
					{
						case 1:
							{
								if (!data.Rainbow)
								{
									var component = pl.GetComponent<RainbowTagController>();
									if (component == null) component = pl.gameObject.AddComponent<RainbowTagController>();
									if (RainbowRoles.ContainsKey(pl.characterClassManager.UserId)) RainbowRoles.Remove(pl.characterClassManager.UserId);
									RainbowRoles.Add(pl.characterClassManager.UserId, component);
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
			Socket.On("database.get.donate.ra", obj =>
			{
				string userid = obj[1].ToString();
				var pl = Player.Get<Player>(userid);
				if (pl is null) return;
				BDDonateRA[] donates = JsonConvert.DeserializeObject<BDDonateRA[]>(obj[0].ToString());
				foreach (var donate in donates)
				{
					if (!Donates.TryGetValue(pl.UserId, out var data))
					{
						var _data = new DonateRA()
						{
							Force = donate.force,
							Give = donate.give,
							Effects = donate.effects,
							ViewRoles = donate.players_roles
						};
						Donates.Add(pl.UserId, _data);
					}
					else
					{
						if (!data.Force) data.Force = donate.force;
						if (!data.Give) data.Give = donate.give;
						if (!data.Effects) data.Effects = donate.effects;
						if (!data.ViewRoles) data.ViewRoles = donate.players_roles;
					}
					Login(pl.ReferenceHub);
					if (donate.force)
					{
						if (forcer.ContainsKey(pl.UserId)) forcer[pl.UserId] = true;
						else forcer.Add(pl.UserId, true);
					}
					if (donate.give)
					{
						if (giver.ContainsKey(pl.UserId)) giver[pl.UserId] = true;
						else giver.Add(pl.UserId, true);
					}
					if (donate.effects)
					{
						if (effecter.ContainsKey(pl.UserId)) effecter[pl.UserId] = true;
						else effecter.Add(pl.UserId, true);
					}
					if (Users.TryGetValue(pl.UserId, out var _d)) _d.donater = true;
				}
			});
		}
		internal static void UpdateRole(ReferenceHub pl)
		{
			UpdateRa();
			void UpdateRa()
			{
				if (!Roles.TryGetValue(pl.characterClassManager.UserId, out var data)) return;
				if (!pl.serverRoles.RemoteAdmin && (data.Priest || data.Mage || data.Sage || data.Star))
				{
					Login(pl);
					return;
				}
			}
		}
		internal static void Login(ReferenceHub rh)
		{
			rh.serverRoles.RemoteAdmin = true;
			rh.serverRoles.Permissions = rh.serverRoles._globalPerms;
			rh.serverRoles.RemoteAdminMode = rh.serverRoles.RemoteAdminMode == ServerRoles.AccessMode.GlobalAccess ? ServerRoles.AccessMode.GlobalAccess : ServerRoles.AccessMode.PasswordOverride;
			rh.serverRoles.TargetOpenRemoteAdmin(false);
		}
		internal void LoadRoles(ReferenceHub player, UserData data)
		{
			Socket.Emit("database.get.donate.ra", new object[] { data.login, 1, player.characterClassManager.UserId });
			Socket.Emit("database.get.donate.roles", new object[] { data.id, 1, player.characterClassManager.UserId });
			if (data.id == -5)
			{
				player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.characterClassManager.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.characterClassManager.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.characterClassManager.UserId, "owner");
			}
		}
		static internal DateTime LastCall = DateTime.Now;
		static internal Faction LastFaction = Faction.Unclassified;
		static internal void Ra(SendingRAEvent ev)
		{
			switch (ev.CommandSender.SenderId)
			{
				case "SERVER CONSOLE": return;
				case "GAME CONSOLE": return;
				case "Effects Controller": return;
			}
			ReferenceHub pl = ReferenceHub.AllHubs.FirstOrDefault(x => x.characterClassManager.UserId == ev.CommandSender.SenderId);
			switch (ev.Name)
			{
				case "give":
					{
						ev.Prefix = "GIVE";
						Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!(roles.Mage || roles.Sage || roles.Star || (Class1.giver.TryGetValue(ev.CommandSender.SenderId, out var ___) && ___))) return;
						ev.Allowed = false;
						if (!StartedRound)
						{
							ev.ReplyMessage = "Раунд еще не начался";
							ev.Success = false;
							return;
						}
						if (pl.GetRoleId().GetTeam() == Team.SCPs)
						{
							ev.ReplyMessage = "Вы играете за SCP. За SCP нельзя выдавать предметы.";
							ev.Success = false;
							return;
						}
						if (!giveway.ContainsKey(pl.characterClassManager.UserId)) giveway.Add(pl.characterClassManager.UserId, 0);
						giveway.TryGetValue(pl.characterClassManager.UserId, out var giver);
						if (giver >= DonateLimint)
						{
							ev.ReplyMessage = $"Вы уже выдали {DonateLimint} предметов";
							ev.Success = false;
							return;
						}
						var itemN = -1;
						if (ev.Args.Length > 1)
						{
							try { itemN = Convert.ToInt32(ev.Args[1].Split('.')[0]); } catch { }
						}
						else
						{
							try { itemN = Convert.ToInt32(ev.Args[0].Split('.')[0]); } catch { }
						}
						if (itemN < 0 || itemN > 43)
						{
							ev.ReplyMessage = "ID предмета не найден";
							ev.Success = false;
							return;
						}
						ItemType item = (ItemType)itemN;
						if (item == ItemType.Ammo12gauge || item == ItemType.Ammo44cal || item == ItemType.Ammo556x45 ||
							item == ItemType.Ammo762x39 || item == ItemType.Ammo9x19)
						{
							pl.inventory.GetCurAmmo(item);
							giveway[pl.characterClassManager.UserId]++;
							ev.ReplyMessage = "Успешно";
							return;
						}
						if (item == ItemType.MicroHID)
						{
							ev.ReplyMessage = "MicroHid только 1";
							ev.Success = false;
							return;
						}
						else if ((pl.GetRoleId() == RoleTypeId.ClassD || pl.GetRoleId() == RoleTypeId.Scientist) &&
								(item == ItemType.KeycardGuard || item == ItemType.KeycardNTFOfficer || item == ItemType.GunCOM15 || item == ItemType.GunCOM18
								|| item == ItemType.KeycardChaosInsurgency || item == ItemType.KeycardContainmentEngineer || item == ItemType.KeycardFacilityManager
								|| item == ItemType.KeycardNTFCommander || item == ItemType.KeycardNTFLieutenant || item == ItemType.KeycardO5 ||
								item == ItemType.SCP018 || item == ItemType.GrenadeHE || item == ItemType.GrenadeFlash) &&
								(3 >= (DateTime.Now - start).TotalMinutes))
						{
							ev.ReplyMessage = "3 минуты не прошло";
							ev.Success = false;
							return;
						}
						else if ((pl.GetRoleId() == RoleTypeId.ClassD || pl.GetRoleId() == RoleTypeId.Scientist) &&
								(item == ItemType.GunCrossvec || item == ItemType.GunFSP9 || item == ItemType.GunRevolver) &&
								(4 >= (DateTime.Now - start).TotalMinutes))
						{
							ev.ReplyMessage = "4 минуты не прошло";
							ev.Success = false;
							return;
						}
						else if ((pl.GetRoleId() == RoleTypeId.ClassD || pl.GetRoleId() == RoleTypeId.Scientist) && (item == ItemType.GunAK ||
							item == ItemType.GunLogicer || item == ItemType.GunShotgun || item == ItemType.GunE11SR) &&
								(5 >= (DateTime.Now - start).TotalMinutes))
						{
							ev.ReplyMessage = "5 минут не прошло";
							ev.Success = false;
							return;
						}
						double CoolDown = 2;
						if (!gives.ContainsKey(pl.characterClassManager.UserId)) gives.Add(pl.characterClassManager.UserId, DateTime.Now);
						else if ((DateTime.Now - gives[ev.CommandSender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((gives[ev.CommandSender.SenderId] - DateTime.Now).TotalSeconds);
							ev.ReplyMessage = $"Предметы можно выдавать раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						GameCore.Console.singleton.TypeCommand($"/{ev.Name} {pl.PlayerId} {itemN}.", new EffectsSender(ev.CommandSender.Nickname, $"{ev.Name} {pl.PlayerId} {itemN}."));
						ev.ReplyMessage = "Успешно";
						gives[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						giveway[pl.characterClassManager.UserId]++;
						return;
					}
				case "forceclass":
					{
						ev.Prefix = "FORCECLASS";
						Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!(roles.Priest || roles.Mage || roles.Sage || roles.Star ||
							(Class1.forcer.TryGetValue(ev.CommandSender.SenderId, out var ___) && ___))) return;
						ev.Allowed = false;
						if (!StartedRound)
						{
							ev.ReplyMessage = "Раунд еще не начался";
							ev.Success = false;
							return;
						}
						if (!force.ContainsKey(pl.characterClassManager.UserId)) force.Add(pl.characterClassManager.UserId, 0);
						force.TryGetValue(pl.characterClassManager.UserId, out var forcer);
						if (forcer >= DonateLimint)
						{
							ev.ReplyMessage = $"Вы уже меняли роль {DonateLimint} раз";
							ev.Success = false;
							return;
						}
						RoleTypeId role = RoleTypeId.None;
						try { role = (RoleTypeId)Enum.Parse(typeof(RoleTypeId), ev.Args[1]); } catch { }
						if(role == RoleTypeId.None)
						{
							ev.ReplyMessage = $"Произошла ошибка при получении роли. Проверьте правильность команды";
							ev.Success = false;
							return;
						}
						if ((role == RoleTypeId.ChaosConscript || role == RoleTypeId.ChaosMarauder || role == RoleTypeId.ChaosRepressor ||
										   role == RoleTypeId.ChaosRifleman || role == RoleTypeId.NtfSpecialist ||
										   role == RoleTypeId.NtfSergeant || role == RoleTypeId.NtfPrivate || role == RoleTypeId.NtfCaptain)
										   && (DateTime.Now - start).Minutes == 0)
						{
							ev.ReplyMessage = $"Подождите {Math.Round(60 - (DateTime.Now - start).TotalSeconds)} секунд.";
							ev.Success = false;
							return;
						}
						if (AlphaDet)
						{
							ev.ReplyMessage = "Нельзя заспавниться после взрыва боеголовки";
							ev.Success = false;
							return;
						}
						var team = role.GetTeam();
						if (role == RoleTypeId.Tutorial)
						{
							ev.Success = false;
							ev.ReplyMessage = "Отключен, ввиду баланса";
							return;
						}
						int scps = ReferenceHub.AllHubs.Where(x => x.GetRoleId().GetTeam() == Team.SCPs).Count();
						if (team == Team.SCPs && scps > 2 && ReferenceHub.AllHubs.Count() / scps < 6)
						{
							ev.Success = false;
							ev.ReplyMessage = "SCP слишком много";
							return;
						}
						if (team == Team.SCPs)
						{
							if (!scp_play.ContainsKey(pl.characterClassManager.UserId)) scp_play.Add(pl.characterClassManager.UserId, false);
							if (scp_play[pl.characterClassManager.UserId])
							{
								ev.Success = false;
								ev.ReplyMessage = "Вы уже играли за SCP";
								return;
							}
						}
						int Scp173 = 0;
						int Scp106 = 0;
						int Scp049 = 0;
						int Scp079 = 0;
						int Scp096 = 0;
						int Scp939 = 0;
						foreach (var rh in ReferenceHub.AllHubs)
						{
							switch (rh.GetRoleId())
							{
								case RoleTypeId.Scp173: Scp173++; break;
								case RoleTypeId.Scp106: Scp106++; break;
								case RoleTypeId.Scp049: Scp049++; break;
								case RoleTypeId.Scp079: Scp079++; break;
								case RoleTypeId.Scp096: Scp096++; break;
								case RoleTypeId.Scp939: Scp939++; break;
							}
						}
						if ((role == RoleTypeId.Scp939 && Scp939 > 0) || (role == RoleTypeId.Scp096 && Scp096 > 0) ||
							(role == RoleTypeId.Scp079 && Scp079 > 0) || (role == RoleTypeId.Scp049 && Scp049 > 0) || (role == RoleTypeId.Scp106 && Scp106 > 0) ||
							(role == RoleTypeId.Scp173 && Scp173 > 0))
						{
							ev.Success = false;
							ev.ReplyMessage = "Этот SCP уже есть";
							return;
						}
						double CoolDown = 2;
						if (!forces.ContainsKey(pl.characterClassManager.UserId)) forces.Add(pl.characterClassManager.UserId, DateTime.Now);
						else if ((DateTime.Now - forces[ev.CommandSender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((forces[ev.CommandSender.SenderId] - DateTime.Now).TotalSeconds);
							ev.ReplyMessage = $"Спавниться можно раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						ev.ReplyMessage = $"Успешно\nЛимит: {forcer + 1}/{DonateLimint}";
						pl.roleManager.ServerSetRole(role, RoleChangeReason.Respawn);
						forces[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						force[pl.characterClassManager.UserId]++;
						return;
					}
				case "pfx":
					{
						ev.Prefix = "pfx";
						Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!(roles.Sage || roles.Star || (effecter.TryGetValue(ev.CommandSender.SenderId, out var ___) && ___))) return;
						ev.Allowed = false;
						double CoolDown = 3;
						if (!effect.ContainsKey(pl.characterClassManager.UserId)) effect.Add(pl.characterClassManager.UserId, DateTime.Now);
						else if ((DateTime.Now - effect[ev.CommandSender.SenderId]).TotalSeconds < 0)
						{
							var wait = Math.Round((effect[ev.CommandSender.SenderId] - DateTime.Now).TotalSeconds);
							ev.ReplyMessage = $"Эффекты можно использовать раз в {CoolDown} минуты\nОсталось подождать {wait} секунд(ы)";
							ev.Success = false;
							return;
						}
						if(ev.Args[0].ToLower() == "movementboost" || ev.Args[0].ToLower() == "bodyshotreduction" || ev.Args[0].ToLower() == "damagereduction"
							|| ev.Args[0].ToLower() == "spawnprotected" || ev.Args[0].ToLower() == "scp1853")
						{
							ev.Allowed = false;
							ev.ReplyMessage = "Данный эффект слишком сильно влияет на баланс";
							ev.Success = false;
							return;
						}
						if (ev.Args[0].ToLower() == "invisible" && pl.GetRoleId().GetTeam() == Team.SCPs)
						{
							ev.Allowed = false;
							ev.ReplyMessage = "За SCP нельзя выдавать эффект невидимости";
							ev.Success = false;
							return;
						}
						if (ev.Args[0].ToLower() == "scp207" && ev.Args[1] != "0" && pl.GetRoleId().GetTeam() == Team.SCPs) ev.Args[1] = "1";
						else if (ev.Args[0].ToLower() == "invisible" && ev.Args[1] != "0") ev.Args[2] = "30";
						else
						{
							var com = $"{ev.Name} {ev.Args[0]} {ev.Args[1]} {ev.Args[2]} {pl.PlayerId}.";
							GameCore.Console.singleton.TypeCommand($"/{com}", new EffectsSender(ev.CommandSender.Nickname, com));
						}
						effect[ev.CommandSender.SenderId] = DateTime.Now.AddMinutes(CoolDown);
						ev.ReplyMessage = "Успешно";
						return;
					}
				case "server_event":
					{
						ev.Prefix = "SERVER_EVENT";
						Roles.TryGetValue(ev.CommandSender.SenderId, out var roles);
						if (!roles.Star) return;
						ev.Allowed = false;
						ev.Success = false;
						ev.ReplyMessage = "Отключен до обновления основного ядра плагинов";
						return;
					}
			}
		}
	}
}