using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loli.Addons
{
	static class AutoAlpha
	{
		internal static bool InProgress { get; set; } = false;
		internal static bool Kill { get; set; } = false;
		internal static bool Enabled { get; set; } = false;
		internal static bool Discusion { get; set; } = false;

		[EventMethod(RoundEvents.Waiting)]
		[EventMethod(RoundEvents.Start)]
		static void Refresh()
		{
			Kill = false;
			Enabled = false;
			Discusion = false;
			InProgress = false;
		}

		static internal void Check()
		{
			if (20 >= Round.ElapsedTime.Minutes)
				return;
			if (InProgress || Alpha.Detonated || OmegaWarhead.InProgress)
				return;

			Map.Broadcast("<size=25%><color=#6f6f6f>По приказу совета О5 запущена <color=red>Альфа Боеголовка</color></color></size>", 10, true);

			if (!Alpha.Active)
				Alpha.Start();

			InProgress = true;
		}

		static internal void ScientistNull()
		{
			try
			{
				if (OmegaWarhead.InProgress)
					return;
				if (Enabled)
					return;

				bool ScientistsAlive = Team.Scientists.CountRoles() > 0;
				if (ScientistsAlive)
					return;

				if (Round.ElapsedTime.Minutes > 17)
				{
					if (Discusion)
					{
						Enabled = true;
						Map.Broadcast("<size=25%><color=#6f6f6f>Совет О5 согласился на <color=#0089c7>взрыв</color> <color=red>Альфа Боеголовки</color></color></size>", 10, true);
						InProgress = true;
						if (!Alpha.Active) Alpha.Start();
					}
					else
					{
						Enabled = true;
						Map.Broadcast("<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер<color=red>/</color>сбежал</color>\n" +
							"<color=#6f6f6f>Совет О5 дал приказ запуска <color=red>Альфа Боеголовки</color></color></size>", 10, true);
						InProgress = true;
						if (!Alpha.Active) Alpha.Start();
					}
				}
				else if (!Discusion && Round.ElapsedTime.Minutes > 1)
				{
					Discusion = true;
					Map.Broadcast("<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер<color=red>/</color>сбежал</color>\n" +
						"<color=#6f6f6f>Совет О5 <color=#0089c7>обсуждает</color> запуск <color=red>Альфа Боеголовки</color></color></size>", 10, true);
				}
			}
			catch { }
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
					Map.Broadcast("<size=25%><color=#6f6f6f>По приказу совета О5, территория комплекса <color=red>отравлена</color></color></size>", 10);
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