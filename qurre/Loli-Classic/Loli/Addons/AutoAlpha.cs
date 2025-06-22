using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;

namespace Loli.Addons
{
	static class AutoAlpha
	{
		internal static bool InProgress { get; set; } = false;
		internal static bool Kill { get; set; } = false;

		[EventMethod(RoundEvents.Waiting)]
		[EventMethod(RoundEvents.Start)]
		static void Refresh()
		{
			Kill = false;
			InProgress = false;
		}

		static internal void Check()
		{
			if (20 >= Round.ElapsedTime.Minutes)
				return;
			if (InProgress || Alpha.Detonated)
				return;

			Map.Broadcast("<size=65%><color=#6f6f6f>По приказу совета О5 запущена <color=red>Альфа Боеголовка</color></color></size>", 10, true);

			if (!Alpha.Active)
				Alpha.Start();

			InProgress = true;
		}

		[EventMethod(AlphaEvents.Stop)]
		static void AntiDisable(AlphaStopEvent ev)
		{
			if (InProgress)
				ev.Allowed = false;
		}

		[EventMethod(AlphaEvents.Detonate)]
		static void Detonated()
		{
			int round = Round.CurrentRound;
			Timing.CallDelayed(30f, () =>
			{
				if (round != Round.CurrentRound) return;
				if (Alpha.Detonated)
				{
					Cassie.Send("ATTENTION TO ALL PERSONNEL . THE outside will be Corrupted Through 30 Seconds");
				}
			});

			Timing.CallDelayed(60f, () =>
			{
				if (round != Round.CurrentRound) return;
				if (Alpha.Detonated)
				{
					Kill = true;
					Cassie.Send("ATTENTION TO ALL PERSONNEL . THE outside will be Corrupted");
					Map.Broadcast("<size=65%><color=#6f6f6f>По приказу совета О5, территория комплекса <color=red>отравлена</color></color></size>", 10);
				}
			});
		}

		static internal void AlphaDead()
		{
			if (!Kill)
				return;

			foreach (Player player in Player.List)
			{
				player.Effects.Enable(EffectType.Poisoned, 0);

				if (player.RoleInfomation.Team is Team.SCPs)
					player.HealthInfomation.Damage(150, DeathTranslations.Warhead);
				else player.HealthInfomation.Damage(5, DeathTranslations.Warhead);
			}
		}
	}
}