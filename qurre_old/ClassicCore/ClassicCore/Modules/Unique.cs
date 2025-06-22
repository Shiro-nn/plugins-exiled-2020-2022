using ClassicCore.BetterHints;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClassicCore.Modules
{
	static internal class Unique
	{
		static internal void Elevator(InteractLiftEvent ev)
		{
			if (ev.Lift == null) return;
			ev.Lift.MovingSpeed = 3;
		}
		static internal void Tesla(TeslaTriggerEvent ev)
		{
			try { if (!Alpha.Active) ev.Allowed = false; }
			catch { ev.Allowed = false; }
		}
		static internal void DoSpawn(DeadEvent ev)
		{
			if (Round.ElapsedTime.Minutes == 0 && Round.Started && ev.Killer.Role != RoleType.Scp049 && ev.Killer.Role != RoleType.Scp0492)
			{
				Player plr = ev.Target;
				Timing.CallDelayed(0.5f, () =>
				{
					if (plr.Role == RoleType.Spectator && !plr.Tag.Contains("NotForce")) plr.SetRole(RoleType.ClassD);
				});
			}
		}
		static internal void DamageHint(DamageProcessEvent ev)
		{
			if (!ev.Allowed) return;
			if (ev.Target == ev.Attacker) return;
			if (ev.Target.GodMode) return;
			string color;
			if (20 > ev.Amount) color = "#2dd300";
			else if (50 > ev.Amount) color = "#ff9c00";
			else color = "#ff0000";
			if (ev.Amount == -1) color = "#ff0000";
			ev.Attacker.Hint(new(UnityEngine.Random.Range(-15, 15), UnityEngine.Random.Range(-6, 6),
				$"<b><color={color}><size=180%>{(ev.Amount == -1 ? "Убит" : Math.Round(ev.Amount))}</size></color></b>", 1, true));
		}
		static internal void SinkHole(SinkholeWalkingEvent ev)
		{
			if (ev.Event == HazardEventsType.Exit && ev.Player.Position.y.Difference(-2000) > 50)
			{
				ev.Player.DisableEffect(EffectType.SinkHole);
				ev.Player.DisableEffect(EffectType.Corroding);
			}
			else if (ev.Player.Team == Team.SCP || ev.Player.Role == RoleType.Tutorial) ev.Allowed = false;
			else if (ev.Event == HazardEventsType.Stay && !ev.Player.GodMode &&
				Vector3.Distance(ev.Player.Position, ev.Sinkhole.Transform.position) < ev.Sinkhole.DistanceToGiveEffect * 0.7f)
			{
				ev.Player.DisableEffect(EffectType.SinkHole);
				ev.Player.Position = Vector3.down * 1998.5f;
				ev.Player.EnableEffect(EffectType.Corroding);
				ev.Allowed = false;
			}
		}

		static internal void Voice(PressAltChatEvent ev)
		{
			if (ev.Player.Role is RoleType.Scp049)
			{
				ev.Player.Dissonance.MimicAs939 = ev.Value;
				return;
			}
			if (ev.Player.Role != RoleType.Scp079 && ev.Player.Team == Team.SCP &&
				DataBase.Manager.Static.Data.Roles.TryGetValue(ev.Player.UserId, out var _data) && _data.Prime)
				ev.Player.Dissonance.MimicAs939 = ev.Value;
		}
	}
}