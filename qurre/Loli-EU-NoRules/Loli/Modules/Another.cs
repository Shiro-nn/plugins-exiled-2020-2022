using Loli.BetterHints;
using Loli.DataBase;
using Loli.Discord;
using MEC;
using Newtonsoft.Json;
using NorthwoodLib;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Loli.Modules
{
    static class Another
	{
		[EventMethod(PlayerEvents.Leave)]
		static void Leave(LeaveEvent ev)
		{
			if (!Round.Ended)
				Core.Socket.Emit("server.leave", new object[] { Core.ServerID, ev.Player.UserInfomation.Ip });
		}

		[EventMethod(RoundEvents.End)]
		static void ClearIps() => Core.Socket.Emit("server.clearips", new object[] { Core.ServerID });

		[EventMethod(RoundEvents.Waiting)]
		static void WaitingPlayers()
		{
			//Log.Info(StringUtils.Base64Encode(ServerConsole.singleton.RefreshServerNameSafe()).Replace('+', '-'));
			//try { GameObject.Find("StartRound").transform.localScale = Vector3.zero; } catch { }
		}

		[EventMethod(MapEvents.TriggerTesla)]
		static void Tesla(TriggerTeslaEvent ev)
		{
			try { if (!Alpha.Active) ev.Allowed = false; }
			catch { ev.Allowed = false; }
		}

		[EventMethod(RoundEvents.End)]
		static void RoundEnd()
		{
			if (!Server.FriendlyFire)
			{
				Cassie.Send("ATTENTION TO ALL PERSONNEL . f f ENABLE");
			}
			Timing.CallDelayed(0.5f, () =>
			{
				foreach (Player player in Player.List)
					Levels.SetPrefix(player);
			});
		}

		[EventMethod(PlayerEvents.Attack)]
		static void EndFF(AttackEvent ev)
		{
			if (!Round.Ended)
				return;

			ev.Allowed = true;
			ev.FriendlyFire = false;
			if (ev.Damage == 0)
				ev.Damage = 10f;
		}

		[EventMethod(PlayerEvents.Dead)]
		static void DoSpawn(DeadEvent ev)
		{
			if (Round.ElapsedTime.Minutes == 0 && Round.Started && ev.Attacker.RoleInfomation.Role != RoleTypeId.Scp049
				&& ev.Attacker.RoleInfomation.Role != RoleTypeId.Scp0492)
			{
				Player plr = ev.Target;
				Timing.CallDelayed(0.5f, () =>
				{
					if (plr.RoleInfomation.Role == RoleTypeId.Spectator && !plr.Tag.Contains("NotForce"))
						plr.RoleInfomation.SetNew(RoleTypeId.ClassD, RoleChangeReason.Respawn);
				});
			}
		}

		[EventMethod(PlayerEvents.Attack, int.MinValue)]
		static void DamageHint(AttackEvent ev)
		{
			if (!ev.Allowed)
				return;
			if (ev.Target == ev.Attacker)
				return;
			if (ev.Target.GamePlay.GodMode)
				return;

			string color;
			if (20 > ev.Damage) color = "#2dd300";
			else if (50 > ev.Damage) color = "#ff9c00";
			else color = "#ff0000";
			if (ev.Damage == -1) color = "#ff0000";

			ev.Attacker.Hint(new(UnityEngine.Random.Range(-15, 15), UnityEngine.Random.Range(-6, 6),
				$"<b><color={color}><size=180%>{(ev.Damage == -1 ? "Убит" : Math.Round(ev.Damage))}</size></color></b>", 1, true));
		}

		[EventMethod(PlayerEvents.Preauth, int.MinValue)]
		static void GBan(PreauthEvent ev)
		{
			if (ev.UserId == "-@steam")
			{
				ev.RejectionReason = RejectionReason.GloballyBanned;
				ev.Allowed = false;
			}
		}
	}
}