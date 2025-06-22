using Loli.Addons;
using MEC;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		public static bool AutoAlphaInProgress = false;
		internal bool AlphaKill = false;
		internal bool AlphaEnabled = false;
		internal bool AlphaDiscusion = false;
		public void AutoAlpha()
		{
			if (Plugin.Anarchy) return;//yes
			if (Plugin.RolePlay) return;
			if (Plugin.ClansWars) return;
			if (Plugin.YouTubersServer) return;
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
			if (20 >= Round.ElapsedTime.Minutes) return;
			if (AutoAlphaInProgress || Alpha.Detonated || OmegaWarhead.InProgress) return;
			Map.Broadcast("<size=25%><color=#6f6f6f>По приказу совета О5 запущена <color=red>Альфа Боеголовка</color></color></size>", 10, true);
			if (!Alpha.Active) Alpha.Start();
			AutoAlphaInProgress = true;
		}
		public void ScientistNull()
		{
			try
			{
				if (Plugin.Anarchy) return;//yes
				if (Plugin.RolePlay) return;
				if (Plugin.ClansWars) return;
				if (Plugin.YouTubersServer) return;
				if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
				if (OmegaWarhead.InProgress) return;
				if (AlphaEnabled) return;
				bool ScientistsAlive = Extensions.CountRoles(Team.RSC) > 0;
				if (ScientistsAlive) return;
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
		public void AntiDisable(AlphaStopEvent ev)
		{
			if (AutoAlphaInProgress) ev.Allowed = false;
		}
		public void AlphaRefresh()
		{
			AlphaKill = false;
			AutoAlphaInProgress = false;
		}
		internal void Detonated()
		{
			if (Plugin.YouTubersServer) return;
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
		public void AlphaDead()
		{
			if (!AlphaKill) return;
			foreach (Player player in Player.List)
			{
				player.EnableEffect(EffectType.Poisoned, 0);
				if (player.Team is Team.SCP) player.Damage(150, DeathTranslations.Warhead);
				else player.Damage(5, DeathTranslations.Warhead);
			}
		}
	}
}