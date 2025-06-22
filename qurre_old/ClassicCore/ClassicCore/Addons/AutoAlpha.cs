using MEC;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ClassicCore.Addons
{
	static internal class AutoAlpha
	{
		static internal bool AutoAlphaInProgress { get; set; } = false;
		static internal bool AlphaKill { get; set; } = false;
		static internal bool AlphaEnabled { get; set; } = false;
		static internal bool AlphaDiscusion { get; set; } = false;
		static internal void Init()
		{
			Qurre.Events.Round.Waiting += Waiting;
			Qurre.Events.Alpha.Detonated += Detonated;
			Qurre.Events.Alpha.Stopping += AntiDisable;

			Timing.RunCoroutine(ScientistsCheck(), "AutoAlpha");
			Timing.RunCoroutine(UpdateDamage(), "AutoAlpha");

			static IEnumerator<float> ScientistsCheck()
			{
				for (; ; )
				{
					try { ScientistNull(); } catch { }
					yield return Timing.WaitForSeconds(5f);
				}
			}
			static IEnumerator<float> UpdateDamage()
			{
				for (; ; )
				{
					try { AlphaDead(); } catch { }
					yield return Timing.WaitForSeconds(1f);
				}
			}
		}
		static internal void Waiting()
		{
			AlphaKill = false;
			AlphaEnabled = false;
			AlphaDiscusion = false;
			AutoAlphaInProgress = false;
		}
		static internal void Enable()
		{
			if (20 >= Round.ElapsedTime.Minutes) return;
			if (AutoAlphaInProgress || Alpha.Detonated) return;
			Map.Broadcast("<size=25%><color=#6f6f6f>По приказу совета О5 запущена <color=red>Альфа Боеголовка</color></color></size>", 10, true);
			if (!Alpha.Active) Alpha.Start();
			AutoAlphaInProgress = true;
		}
		static internal void ScientistNull()
		{
			try
			{
				if (AlphaEnabled) return;
				if (Player.List.Any(x => x.Team == Team.RSC)) return;
				if (Round.ElapsedTime.Minutes > 17)
				{
					if (AlphaDiscusion)
					{
						AlphaEnabled = true;
						Map.Broadcast("<size=25%><color=#6f6f6f>Совет О5 согласился на <color=#0089c7>взрыв</color> <color=red>Альфа Боеголовки</color></color></size>", 10, true);
						AutoAlphaInProgress = true;
						if (!Alpha.Active) Alpha.Start();
					}
					else
					{
						AlphaEnabled = true;
						Map.Broadcast("<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер<color=red>/</color>сбежал</color>\n" +
							"<color=#6f6f6f>Совет О5 дал приказ запуска <color=red>Альфа Боеголовки</color></color></size>", 10, true);
						AutoAlphaInProgress = true;
						if (!Alpha.Active) Alpha.Start();
					}
				}
				else if (!AlphaDiscusion && Round.ElapsedTime.Minutes > 1)
				{
					AlphaDiscusion = true;
					Map.Broadcast("<size=25%><color=#6f6f6f>Весь <color=#fdffbb>Научный персонал</color> помер<color=red>/</color>сбежал</color>\n" +
						"<color=#6f6f6f>Совет О5 <color=#0089c7>обсуждает</color> запуск <color=red>Альфа Боеголовки</color></color></size>", 10, true);
				}
			}
			catch { }
		}
		static internal void AlphaDead()
		{
			if (!AlphaKill) return;
			foreach (Player player in Player.List)
			{
				player.EnableEffect(EffectType.Poisoned, 0);
				if (player.Team is Team.SCP) player.Damage(150, DeathTranslations.Warhead);
				else player.Damage(5, DeathTranslations.Warhead);
			}
		}
		static internal void AntiDisable(AlphaStopEvent ev)
		{
			if (AutoAlphaInProgress) ev.Allowed = false;
		}
		static internal void Detonated()
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
					AlphaKill = true;
					Cassie.Send("ATTENTION TO ALL PERSONNEL . THE outside will be Corrupted");
					Map.Broadcast("<size=25%><color=#6f6f6f>По приказу совета О5, территория комплекса <color=red>отравлена</color></color></size>", 10);
				}
			});
		}
	}
}