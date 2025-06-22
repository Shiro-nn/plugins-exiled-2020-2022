using HarmonyLib;
using MEC;
using Newtonsoft.Json;
using PluginAPI.Core.Attributes;
using System;
using System.Linq;

namespace Loli_Time
{
	public static class Events
	{
		static public void Leave(ReferenceHub player)
		{
			ServerConsole.AddLog("leave", ConsoleColor.Blue);
			Class1.Socket.Emit("server.leave", new object[] { Class1.ServerID, player.characterClassManager.connectionToClient.address });
		}

		static public void LoadData(string userid)
		{
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
		}
	}
	public class Class1
	{
		internal static string ApiToken => "-";
		internal static string SocketIP => "45.142.122.184";
		internal static readonly QurreSocket.Client Socket = new(2467, SocketIP);

		internal const int ServerID = 8;

		static internal Harmony _harmony;


		[PluginEntryPoint("Loli", "0.0.0", "hello", "fydne")]
		public void Init()
		{
			try { if (_harmony is not null) _harmony.UnpatchAll(); } catch { }
			_harmony = new Harmony("loli.patches");
			_harmony.PatchAll();
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
			Socket.On("database.get.data", obj =>
			{
				string userid = obj[1].ToString();
				ServerConsole.AddLog(userid, ConsoleColor.Red);
				ReferenceHub pl = ReferenceHub.AllHubs.FirstOrDefault(x => x.characterClassManager.UserId == userid);
				ServerConsole.AddLog($"{pl is null}", ConsoleColor.Red);
				if (pl is null) return;
				UserData json = JsonConvert.DeserializeObject<UserData>(obj[0].ToString());
				json.entered = DateTime.Now;
				Timing.CallDelayed(0.1f, () => LoadRoles(pl, json));
			});
		}
		internal void LoadRoles(ReferenceHub player, UserData data)
		{
			if (data.trainee)
			{
				player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("sta"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.characterClassManager.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.characterClassManager.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.characterClassManager.UserId, "sta");
			}
			if (data.helper)
			{
				player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("helper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.characterClassManager.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.characterClassManager.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.characterClassManager.UserId, "helper");
			}
			if (data.mainhelper)
			{
				player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("glhelper"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.characterClassManager.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.characterClassManager.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.characterClassManager.UserId, "glhelper");
			}
			if (data.admin)
			{
				player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("admin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.characterClassManager.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.characterClassManager.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.characterClassManager.UserId, "admin");
			}
			if (data.mainadmin)
			{
				player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("gladmin"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.characterClassManager.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.characterClassManager.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.characterClassManager.UserId, "gladmin");
			}
			if (data.owner)
			{
				player.serverRoles.SetGroup(ServerStatic.GetPermissionsHandler().GetGroup("owner"), false, true, false);
				if (ServerStatic.GetPermissionsHandler()._members.ContainsKey(player.characterClassManager.UserId)) ServerStatic.GetPermissionsHandler()._members.Remove(player.characterClassManager.UserId);
				ServerStatic.GetPermissionsHandler()._members.Add(player.characterClassManager.UserId, "owner");
			}
		}
	}
}