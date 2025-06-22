using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using UnityEngine;
using Grenades;
using MEC;
using System;
namespace D_class_servers
{
	partial class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public static bool dl = false;
		public static bool sl = false;
		public static bool cl = false;
		public static bool tl = false;
		public static bool ssl = false;
		public static bool ml = false;
		public static int gena = 0;
		public static int sc = 0;
		public static int nt = 0;
		public static int ct = 0;
		public static string aw;
		public static float forceShoot = 100.0f;
		public static float rangeShoot = 7.0f;
		public static string handcuffer;
		public int Scp096KillCount;
		public void RoundStart()
		{
			dl = false;
			sl = false;
			cl = false;
			tl = false;
			ssl = false;
			ml = false;
			gena = 0;
			sc = 0;
			nt = 0;
			ct = 0;
			aw = Configs.wnd;
		}

		public void RoundEnd()
		{
			dl = false;
			sl = false;
			cl = false;
			tl = false;
			ssl = false;
			ml = false;
			gena = 0;
		}
		public void OnWarheadDetonation()
		{
			aw = Configs.wbd;
		}
		public void bc1(AnnounceNtfEntranceEvent ev)
		{
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.ms.Replace("%NtfLetter%", $"{ev.NtfLetter}").Replace("%NtfNumber%", $"{ev.NtfNumber}").Replace("%ScpsLeft%", $"{ev.ScpsLeft}"), Configs.newmtfsquadt);//.Replace("%NtfLetter%", $"{ev.NtfLetter}").Replace("%NtfNumber%", $"{ev.NtfNumber}").Replace("%ScpsLeft%", $"{ev.ScpsLeft}")
			DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.newmtfsquad.Replace("%NtfLetter%", $"{ev.NtfLetter}").Replace("%NtfNumber%", $"{ev.NtfNumber}").Replace("%ScpsLeft%", $"{ev.ScpsLeft}"), Configs.channel);
			nt++;
		}
		public void bc2(ref TeamRespawnEvent ev)
		{
			if (ev.IsChaos)
			{
				List<ReferenceHub> pList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ChaosInsurgency || x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
				foreach (ReferenceHub player in pList)
				{
					player.ClearBroadcasts();
					player.Broadcast(Configs.newchaossquad, Configs.newchaossquadt);
					DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dcs, Configs.channel);
				}
				ct++;
			}
		}
		public void bc3(ref CheckRoundEndEvent ev)
		{
			List<ReferenceHub> dList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ClassD && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (dList.Count != 1)
				dl = false;
			if (dList.Count == 1)
			{
				if (!dl)
				{
					foreach (ReferenceHub dboy in dList)
					{
						dboy.ClearBroadcasts();
						dboy.Broadcast(Configs.plastm.Replace("%user%", $"{dboy.GetNickname()}").Replace("%user.role%", $"<color=orange>{Configs.d}</color>"), Configs.plastpt);
						dl = true;
					}
				}
			}
			List<ReferenceHub> ScList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scientist && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (ScList.Count != 1)
				sl = false;
			if (ScList.Count == 1)
			{
				if (!sl)
				{
					foreach (ReferenceHub sp in ScList)
					{
						sp.ClearBroadcasts();
						sp.Broadcast(Configs.plastm.Replace("%user%", $"{sp.GetNickname()}").Replace("%user.role%", $"<color=yellow>{Configs.sc}</color>"), Configs.plastpt);
						sl = true;
					}
				}
			}
			List<ReferenceHub> CIList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ChaosInsurgency && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (CIList.Count != 1)
				cl = false;
			if (CIList.Count == 1)
			{
				if (!cl)
				{
					foreach (ReferenceHub CI in CIList)
					{
						CI.ClearBroadcasts();
						CI.Broadcast(Configs.plastm.Replace("%user%", $"{CI.GetNickname()}").Replace("%user.role%", $"<color=green>{Configs.ci}</color>"), Configs.plastpt);
						cl = true;
					}
				}
			}
			List<ReferenceHub> TutList = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.ChaosInsurgency && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (TutList.Count != 1)
				tl = false;
			if (TutList.Count == 1)
			{
				if (!tl)
				{
					foreach (ReferenceHub T in TutList)
					{
						T.ClearBroadcasts();
						T.Broadcast(Configs.plastm.Replace("%user%", $"{T.GetNickname()}").Replace("%user.role%", $"<color=lime>{Configs.t}</color>"), Configs.plastpt);
						tl = true;
					}
				}
			}
			List<ReferenceHub> SList = Player.GetHubs().Where(x => x.GetTeam() == Team.SCP && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (SList.Count != 1)
				ssl = false;
			if (SList.Count == 1)
			{
				if (!ssl)
				{
					foreach (ReferenceHub S in SList)
					{
						S.ClearBroadcasts();
						S.Broadcast(Configs.plastm.Replace("%user%", $"{S.GetNickname()}").Replace("%user.role%", $"<color=red>{S.GetRole()}</color>"), Configs.plastpt);
						ssl = true;
					}
				}
			}
			List<ReferenceHub> MList = Player.GetHubs().Where(x => x.GetTeam() == Team.MTF && x.characterClassManager.UserId != null && x.characterClassManager.UserId != string.Empty).ToList();
			if (MList.Count != 1)
				ml = false;
			if (MList.Count == 1)
			{
				if (!ml)
				{
					foreach (ReferenceHub M in MList)
					{
						M.ClearBroadcasts();
						M.Broadcast(Configs.plastm.Replace("%user%", $"{M.GetNickname()}").Replace("%user.role%", $"<color=blue>{M.GetRole()}</color>"), Configs.plastpt);
						ml = true;
					}
				}
			}
		}
		public void bc4(ref PlayerDeathEvent ev)
		{
			if (ev.Player.GetHandCuffer())
			{
				handcuffer = Configs.ht;
			}
			if (!ev.Player.GetHandCuffer())
			{
				handcuffer = Configs.hf;
			}
			if (ev.Player.queryProcessor.PlayerId == ev.Killer?.queryProcessor.PlayerId) return;
			if (ev.Player.GetTeam() != Team.SCP)
			{
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(Configs.killer.Replace("%killer.id%", $"{ev.Killer.characterClassManager.UserId}").Replace("%handcuffer%", $"{handcuffer}").Replace("%killer.name%", $"{ev.Killer.GetNickname()}").Replace("%killer.role%", $"{ev.Killer.GetRole()}").Replace("%player.name%", $"{ev.Player.GetNickname()}").Replace("%player.role%", $"{ev.Player.GetRole()}"), Configs.killerdur);
				//player.Broadcast($"<size=20><color=aqua>{ev.Killer.GetNickname()}</color><color=red>(</color><color=aqua>{ev.Killer.GetRole()}</color><color=red>) killed</color> <color=aqua>{ev.Player.GetNickname()}</color><color=red>(</color><color=aqua>{ev.Player.GetRole()}</color><color=red>)</color></size>", 5);
			}
			if (ev.Player.GetTeam() == Team.SCP)
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.cs.Replace("%killer.name%", $"{ev.Killer.GetNickname()}").Replace("%killer.role%", $"{ev.Killer.GetRole()}").Replace("%player.name%", $"{ev.Player.GetNickname()}").Replace("%player.role%", $"{ev.Player.GetRole()}"), Configs.csdur);
				sc++;
				//player.Broadcast($"<size=20><color=aqua>{ev.Killer.GetNickname()}</color><color=red>(</color><color=aqua>{ev.Killer.GetRole()}</color><color=red>) killed</color> <color=aqua>{ev.Player.GetNickname()}</color><color=red>(</color><color=aqua>{ev.Player.GetRole()}</color><color=red>)</color></size>", 5);
			}
		}
		public void bc5(PlayerLeaveEvent ev)
		{
			if (ev.Player.GetTeam() == Team.SCP)
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.pl.Replace("%user%", $"{ev.Player.GetNickname()}").Replace("%user.role%", $"{ev.Player.GetRole()}").Replace("%steam64%", $"{ev.Player.GetUserId()}"), Configs.plt);
			}
		}
		public void bc6(AnnounceDecontaminationEvent ev)
		{
			if (ev.AnnouncementId == 0)
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.flc.Replace("%time%", $"15").Replace("<color=rainbow>", $"<color=white>"), 1);
				DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dlc.Replace("%time%", $"15"), Configs.channel);
				Timing.CallDelayed(1f, () =>
				{
					Map.ClearBroadcasts();
					Map.Broadcast(Configs.flc.Replace("%time%", $"15").Replace("<color=rainbow>", $"<color=red>"), 1);
					Timing.CallDelayed(1f, () =>
					{
						Map.ClearBroadcasts();
						Map.Broadcast(Configs.flc.Replace("%time%", $"15").Replace("<color=rainbow>", $"<color=white>"), 1);
						Timing.CallDelayed(1f, () =>
						{
							Map.ClearBroadcasts();
							Map.Broadcast(Configs.flc.Replace("%time%", $"15").Replace("<color=rainbow>", $"<color=red>"), 1);
						});
						Timing.CallDelayed(1f, () =>
						{
							Map.ClearBroadcasts();
							Map.Broadcast(Configs.flc.Replace("%time%", $"15").Replace("<color=rainbow>", $"<color=white>"), Configs.flct - 5);
						});
					});
				});
			}
			if (ev.AnnouncementId == 1)
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.lc.Replace("%time%", $"10"), Configs.lct);
				DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dlc.Replace("%time%", $"10"), Configs.channel);
			}
			if (ev.AnnouncementId == 2)
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.lc.Replace("%time%", $"5"), Configs.lct);
				DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dlc.Replace("%time%", $"5"), Configs.channel);
			}
			if (ev.AnnouncementId == 3)
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.lc.Replace("%time%", $"1"), Configs.lct);
				DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dlc.Replace("%time%", $"1"), Configs.channel);
			}
			if (ev.AnnouncementId == 4)
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.llc.Replace("%time%", $"30"), Configs.llct);
				DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"30"), Configs.channel);
				Timing.CallDelayed(17f, () =>
				{
					Map.ClearBroadcasts();
					Map.Broadcast(Configs.llc.Replace("%time%", $"20"), 1);
					DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"20"), Configs.channel);
					Timing.CallDelayed(1f, () =>
					{
						Map.ClearBroadcasts();
						Map.Broadcast(Configs.llc.Replace("%time%", $"19"), 1);
						Timing.CallDelayed(1f, () =>
						{
							Map.ClearBroadcasts();
							Map.Broadcast(Configs.llc.Replace("%time%", $"18"), 1);
							Timing.CallDelayed(1f, () =>
							{
								Map.ClearBroadcasts();
								Map.Broadcast(Configs.llc.Replace("%time%", $"17"), 1);
								Timing.CallDelayed(1f, () =>
								{
									Map.ClearBroadcasts();
									Map.Broadcast(Configs.llc.Replace("%time%", $"16"), 1);
									Timing.CallDelayed(1f, () =>
									{
										Map.ClearBroadcasts();
										Map.Broadcast(Configs.llc.Replace("%time%", $"15"), 1);
										DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"15"), Configs.channel);
										Timing.CallDelayed(1f, () =>
										{
											Map.ClearBroadcasts();
											Map.Broadcast(Configs.llc.Replace("%time%", $"14"), 1);
											Timing.CallDelayed(1f, () =>
											{
												Map.ClearBroadcasts();
												Map.Broadcast(Configs.llc.Replace("%time%", $"13"), 1);
												Timing.CallDelayed(1f, () =>
												{
													Map.ClearBroadcasts();
													Map.Broadcast(Configs.llc.Replace("%time%", $"12"), 1);
													Timing.CallDelayed(1f, () =>
													{
														Map.ClearBroadcasts();
														Map.Broadcast(Configs.llc.Replace("%time%", $"11"), 1);
														Timing.CallDelayed(1f, () =>
														{
															Map.ClearBroadcasts();
															Map.Broadcast(Configs.llc.Replace("%time%", $"10"), 1);
															DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"10"), Configs.channel);
															Timing.CallDelayed(1f, () =>
															{
																Map.ClearBroadcasts();
																Map.Broadcast(Configs.llc.Replace("%time%", $"9"), 1);
																Timing.CallDelayed(1f, () =>
																{
																	Map.ClearBroadcasts();
																	Map.Broadcast(Configs.llc.Replace("%time%", $"8"), 1);
																	Timing.CallDelayed(1f, () =>
																	{
																		Map.ClearBroadcasts();
																		Map.Broadcast(Configs.llc.Replace("%time%", $"7"), 1);
																		Timing.CallDelayed(1f, () =>
																		{
																			Map.ClearBroadcasts();
																			Map.Broadcast(Configs.llc.Replace("%time%", $"6"), 1);
																			Timing.CallDelayed(1f, () =>
																			{
																				Map.ClearBroadcasts();
																				Map.Broadcast(Configs.llc.Replace("%time%", $"5"), 1);
																				DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"5"), Configs.channel);
																				Timing.CallDelayed(1f, () =>
																				{
																					Map.ClearBroadcasts();
																					Map.Broadcast(Configs.llc.Replace("%time%", $"4"), 1);
																					Timing.CallDelayed(1f, () =>
																					{
																						Map.ClearBroadcasts();
																						Map.Broadcast(Configs.llc.Replace("%time%", $"3"), 1);
																						DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"3"), Configs.channel);
																						Timing.CallDelayed(1f, () =>
																						{
																							Map.ClearBroadcasts();
																							Map.Broadcast(Configs.llc.Replace("%time%", $"2"), 1);
																							DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"2"), Configs.channel);
																							Timing.CallDelayed(1f, () =>
																							{
																								Map.ClearBroadcasts();
																								Map.Broadcast(Configs.llc.Replace("%time%", $"1"), 1);
																								DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dllc.Replace("%time%", $"1"), Configs.channel);
																							});
																						});
																					});
																				});
																			});
																		});
																	});
																});
															});
														});
													});
												});
											});
										});
									});
								});
							});
						});
					});
				});
			}
			if (ev.AnnouncementId == 5)
			{

				Map.ClearBroadcasts();
				Map.Broadcast(Configs.ld.Replace("<color=rainbow>", $"<color=white>"), 1);
				DiscordIntegration_Plugin.ProcessSTT.SendData($"```{Configs.dld}```", Configs.channel);
				Timing.CallDelayed(1f, () =>
				{
					Map.ClearBroadcasts();
					Map.Broadcast(Configs.ld.Replace("<color=rainbow>", $"<color=red>"), 1);
					Timing.CallDelayed(1f, () =>
					{
						Map.ClearBroadcasts();
						Map.Broadcast(Configs.ld.Replace("<color=rainbow>", $"<color=white>"), 1);
						Timing.CallDelayed(1f, () =>
						{
							Map.ClearBroadcasts();
							Map.Broadcast(Configs.ld.Replace("<color=rainbow>", $"<color=red>"), 1);
						});
						Timing.CallDelayed(1f, () =>
						{
							Map.ClearBroadcasts();
							Map.Broadcast(Configs.ld.Replace("<color=rainbow>", $"<color=white>"), Configs.ldt - 5);
						});
					});
				});
			}
		}
		public void bc7(PlayerJoinEvent ev)
		{
			List<ReferenceHub> players = Plugin.GetHubs();
			ev.Player.ClearBroadcasts();
			ev.Player.Broadcast(Configs.wm.Replace("%players.Count%", $"{players.Count}").Replace("%round.time%", $"{RoundSummary.roundTime / 60}"), Configs.wmt);
		}
		public void bc8()//round end
		{
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.re.Replace("%warhead.info%", $"{aw}").Replace("%chaos.teams%", $"{ct}").Replace("%ntf.teams%", $"{nt}").Replace("%kills.by.scp%", $"{RoundSummary.kills_by_scp}").Replace("%kills%", $"{RoundSummary.Kills}").Replace("%scp.contained%", $"{sc}").Replace("%round.time%", $"{RoundSummary.roundTime / 60}").Replace("%escaped.d%", $"{RoundSummary.escaped_ds}").Replace("%escaped.scientists%", $"{RoundSummary.escaped_scientists}"), Configs.ret);
			DiscordIntegration_Plugin.ProcessSTT.SendData(Configs.dre.Replace("%warhead.info%", $"{aw}").Replace("%chaos.teams%", $"{ct}").Replace("%ntf.teams%", $"{nt}").Replace("%kills.by.scp%", $"{RoundSummary.kills_by_scp}").Replace("%kills%", $"{RoundSummary.Kills}").Replace("%scp.contained%", $"{sc}").Replace("%round.time%", $"{RoundSummary.roundTime / 60}").Replace("%escaped.d%", $"{RoundSummary.escaped_ds}").Replace("%escaped.scientists%", $"{RoundSummary.escaped_scientists}"), Configs.channel);
		}//Round end! [Playing time %round.time%] [scp contained %scp.contained%] [%escaped.scientists% Scientists escape] [%escaped.d% D-class escape] [kills %kills%] [kills by scp %kills.by.scp%] [Ntf teams: %ntf.teams%] [Chaos teams: %chaos.teams%] [Alpha Warhead %warhead.info%]
		public void bc9(ref GeneratorFinishEvent ev)
		{
			gena++;
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.ga.Replace("%gen.activated%", $"{gena}").Replace("%Generator.Room%", $"{ev.Generator.curRoom}"), Configs.gat);
		}
		public void bc10(WarheadStartEvent ev)
		{
			try
			{
				Map.ClearBroadcasts();
				Map.Broadcast(Configs.wa.Replace("%player%", $"{ev.Player.GetNickname()}").Replace("%player.role%", $"{ev.Player.GetRole()}"), Configs.wat);
			}
			catch (Exception e)
			{
				Log.Info($"{e}");
			}
		}
		public void bc11(WarheadCancelEvent ev)
		{
			Map.ClearBroadcasts();
			Map.Broadcast(Configs.wd.Replace("%player%", $"{ev.Player.GetNickname()}").Replace("%player.role%", $"{ev.Player.GetRole()}"), Configs.wdt);
		}
		public void bc12()
		{
			IEnumerable<ReferenceHub> SCPs = Player.GetHubs().Where(rh => rh.GetTeam() == Team.SCP);
			string response = Configs.asd;
			foreach (ReferenceHub scp in SCPs)
			{
				response += $"\n{scp.GetNickname()} - {scp.characterClassManager.Classes.SafeGet(scp.GetRole()).fullName}";
				scp.ClearBroadcasts();
				scp.Broadcast(response, Configs.rst);
			}
		}
		public void bc13(Scp106ContainEvent ev)
		{
			if (!RoundSummary.RoundInProgress() || RoundSummary.roundTime < Configs.initialCooldown)
			{
				ev.Allow = false;
				int i = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)i), 1U, false);
				Timing.CallDelayed(1U, () =>
				{
					int ia = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)ia), 1U, false);
				});
				Timing.CallDelayed(2U, () =>
				{
					int iq = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iq), 1U, false);
				});
				Timing.CallDelayed(3U, () =>
				{
					int iz = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iz), 1U, false);
				});
				Timing.CallDelayed(4U, () =>
				{
					int iw = (int)Configs.initialCooldown - (int)RoundSummary.roundTime;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast(string.Format(Configs.dontaccess, (object)(float)(double)iw), 1U, false);
				});
			}
		}






		internal void RoundStart2()
		{
			Plugin.Coroutines.Add(Timing.RunCoroutine(this.DoScp049Heal()));
		}
		private IEnumerator<float> DoScp049Heal()
		{
			while (true)
			{
				foreach (ReferenceHub hub1 in Player.GetHubs())
				{
					ReferenceHub hub = hub1;
					if (hub.GetRole() == RoleType.Scp049)
					{
						int counter = 0;
						foreach (ReferenceHub hub2 in Player.GetHubs())
						{
							ReferenceHub rh = hub2;
							if (rh.GetRole() == RoleType.Scp0492)
							{
								if ((double)Vector3.Distance(rh.GetPosition(), hub.GetPosition()) < 10.0)
									++counter;
								if (Plugin.Instance.Scp0492Healing && (double)rh.GetHealth() + (double)Plugin.Instance.Scp0492HealAmount <= (double)rh.playerStats.maxHP)
									rh.SetHealth(rh.GetHealth() + (float)Plugin.Instance.Scp0492HealAmount);
								rh = (ReferenceHub)null;
							}
						}
						float healing = (float)Math.Pow((double)counter, (double)Plugin.Instance.Scp049HealPow) * (float)Plugin.Instance.Scp0492HealAmount;
						if (Plugin.Instance.Scp049Healing && (double)hub.GetHealth() + (double)healing <= (double)hub.playerStats.maxHP)
							hub.SetHealth(hub.GetHealth() + healing);
					}
					hub = (ReferenceHub)null;
				}
				yield return Timing.WaitForSeconds(5f);
			}
		}
		public void OnPlayerDeath(ref PlayerDeathEvent ev)
		{
			if (ev.Killer.GetRole() == RoleType.Scp096)
				++Scp096KillCount;
			if (ev.Killer.GetRole().Is939() && Plugin.Instance.Scp939Healing && Plugin.Instance.Gen.Next(1, 100) > 50)
				Plugin.Coroutines.Add(Timing.RunCoroutine(this.HealOverTime(ev.Killer, Plugin.Instance.Scp939Heal, 10f), ev.Killer.GetUserId()));
			if (ev.Killer.GetRole() != RoleType.Scp173 || !Plugin.Instance.Scp173Healing)
				return;
			Plugin.Coroutines.Add(Timing.RunCoroutine(this.HealOverTime(ev.Killer, Plugin.Instance.Scp173HealAmount, 5f), ev.Killer.GetUserId()));
		}

		public void OnPocketDeath(PocketDimDeathEvent ev)
		{
			if (!Plugin.Instance.Scp106Healing)
				return;
			ReferenceHub referenceHub = Player.GetHubs().FirstOrDefault<ReferenceHub>((Func<ReferenceHub, bool>)(r => r.GetRole() == RoleType.Scp106));
			Plugin.Coroutines.Add(Timing.RunCoroutine(this.HealOverTime(referenceHub, Plugin.Instance.Scp106HealAmount, 10f), referenceHub.GetUserId()));
		}

		public void OnEnrage(ref Scp096EnrageEvent ev)
		{
			Scp096KillCount = 0;
		}

		public void OnCalm(ref Scp096CalmEvent ev)
		{
			if (!Plugin.Instance.Scp096Healing)
				return;
			Plugin.Coroutines.Add(Timing.RunCoroutine(HealOverTime(ev.Player, Plugin.Instance.Scp096Heal * Scp096KillCount, 10f), ev.Player.GetUserId()));
		}
		public IEnumerator<float> HealOverTime(ReferenceHub hub, int amount, float duration = 10f)
		{
			float amountPerTick = (float)amount / duration;
			for (int i = 0; (double)i < (double)duration && (double)hub.GetHealth() + (double)amountPerTick <= (double)hub.playerStats.maxHP; ++i)
			{
				hub.SetHealth(hub.GetHealth() + amountPerTick);
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}
}
