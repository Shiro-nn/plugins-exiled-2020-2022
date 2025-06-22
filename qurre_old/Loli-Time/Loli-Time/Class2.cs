using HarmonyLib;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using RoundRestarting;
using System;
using System.Linq;
namespace Loli_Time
{
	[HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery), new Type[] { typeof(string), typeof(CommandSender) })]
	internal static class RemoteAdminCommand
	{
		private static bool Prefix(string q, CommandSender sender)
		{
			try
			{
				string[] allarguments = q.Split(' ');
				string name = allarguments[0].ToLower();
				string[] args = allarguments.Skip(1).ToArray();
				IdleMode.PreauthStopwatch.Restart();
				IdleMode.SetIdleMode(false);
				var ev = new SendingRAEvent(sender, q, name, args);
				Class1.Ra(ev);
				if (!string.IsNullOrEmpty(ev.ReplyMessage))
					sender.RaReply(ev.ReplyMessage, ev.Success, true, string.Empty);
				return ev.Allowed;
			}
			catch (Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
				return true;
			}
		}
	}
	[HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
	internal static class Waiting
	{
		private static void Prefix(string q)
		{
			try
			{
				if (q == "Waiting for players...") Events.Wait();
			}
			catch (Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.NetworkRoundStarted), MethodType.Setter)]
	internal static class RoundStart
	{
		private static void Prefix(bool value)
		{
			try
			{
				if (value) Events.Start();
			}
			catch (Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
	[HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.RpcShowRoundSummary))]
	internal static class RoundRestart1
	{
		static void Postfix()
		{
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
	[HarmonyPatch(typeof(RoundRestart), nameof(RoundRestart.InitiateRoundRestart))]
	internal static class RoundRestart2
	{
		static void Prefix()
		{
			Timing.CallDelayed(1f, () =>
			{
				Server.Restart();
			});
		}
	}
	[HarmonyPatch(typeof(AlphaWarheadController), nameof(AlphaWarheadController.Detonate))]
	internal static class Alpha
	{
		private static void Prefix()
		{
			try
			{
				Events.alpha();
			}
			catch (Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
	[HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.InitializeNewRole))]
	internal static class Spawn
	{
		private static void Postfix(PlayerRoleManager __instance)
		{
			try
			{
				Events.spawn(__instance.Hub, __instance.CurrentRole.RoleTypeId);
			}
			catch (Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
	[HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.OnDestroy))]
	internal static class Leave
	{
		private static void Prefix(ReferenceHub __instance)
		{
			try
			{
				Events.Leave(__instance);
			}
			catch (Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
	[HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserId), MethodType.Setter)]
	internal static class Join
	{
		private static void Postfix(CharacterClassManager __instance, string value)
		{
			try
			{
				Events.Join(__instance, value);
			}
			catch(Exception ex)
			{
				ServerConsole.AddLog($"{ex}", ConsoleColor.Red);
			}
		}
	}
}