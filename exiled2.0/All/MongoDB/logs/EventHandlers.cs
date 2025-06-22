using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Scp914;
using System;
using System.Linq;
using UnityEngine;
using Respawning;
using MEC;
using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;

namespace MongoDB.logs
{
	public class EventHandlers
	{
		public DiscordLogs plugin;
		public EventHandlers(DiscordLogs plugin) => this.plugin = plugin;
		public send send;
		private bool firststart = true;
		internal static bool roundstart = false;
		internal bool abusetrue = false;
		internal bool abusestart = false;
		internal int abuseagree = 0;
		private static Dictionary<string, bool> agree = new Dictionary<string, bool>();
		public static void UpdateServerStatus()
		{
			try
			{
				int max = GameCore.ConfigFile.ServerConfig.GetInt("max_players", 35);
				int cur = Player.List.Count();
				int aliveCount = 0;
				int scpCount = 0;
				foreach (Player player in Player.List)
					if (player.ReferenceHub.characterClassManager.IsHuman())
						aliveCount++;
					else if (player.ReferenceHub.characterClassManager.IsAnyScp())
						scpCount++;
				string warhead = Warhead.IsDetonated ? "Альфа-Боеголовка взорвана." :
					Warhead.IsInProgress ? "Альфа-Боеголовка запущена." :
					"Альфа-Боеголовка не взорвана.";
				send.sendchanneltopic(
					$"Игроков онлайн: {cur}/{max}. Длительность раунда: {Round.ElapsedTime.Minutes} минут. Живых людей: {aliveCount}. Живых scp: {scpCount}. {warhead} IP: {ServerConsole.Ip}:{ServerConsole.Port}");
			}
			catch
			{
			}
		}
		internal void abuseactivate(string nick)
		{
			Map.Broadcast(15, $"<size=30%><color=red>{nick}</color> <color=#00ffff>запросил возможность абузить в этом раунде</color>\n<color=lime>Если вы согласны, то напишите в консоли на</color> <color=#ff0>[<color=#00ffff>ё</color>]</color>\n<color=red>.abuse yes</color>\n<color=#fdffbb>У вас есть <color=red>60</color> секунд на выбор</color></size>");
			abuseagree = 0; 
			abusestart = true;
			int players = Player.List.Count();
			bool end = false;
			Timing.CallDelayed(60f, () =>
			{
				if (!end)
				{
					end = true;
					if (abuseagree >= players)
					{
						send.sendmsg("<:youdead:616720748341624832> Разрешен абуз в этом раунде");
						abusetrue = true;
						abusestart = false;
						Map.Broadcast(10, "<i><color=lime>Абуз разрешен</color>\n<color=red>Но наказания без причины <b>запрещены</b></color></i>");
					}
					else
					{
						send.sendmsg("<:doge:616720488969928836> Абуз запрещен в этом раунде");
						abusetrue = false;
						abusestart = false;
						Map.Broadcast(10, $"<i><color=red>Абуз запрещен</color>\n<color=#00ffff><b>{players - abuseagree}</b> против</color></i>");
						abuseagree = 0;
					}
				}
			});
		}
		public void OnCommand(SendingRemoteAdminCommandEventArgs ev)
		{
			#region abuse
			if (ev.Name == "abuse")
			{
				ev.IsAllowed = false;
				if (!roundstart)
				{
					ev.ReplyMessage = $"abuse#Раунд не запущен";
				}
				else if (abusestart)
				{
					ev.ReplyMessage = $"abuse#Голосование уже начато";
				}
				else if (abusetrue)
				{
					ev.ReplyMessage = $"abuse#Абуз уже разрешен";
				}
				else
				{
					ev.ReplyMessage = $"abuse#Отправлено на голосование";
					abuseactivate(ev.Sender.Nickname);
				}
			}
			#endregion
			#region list
			if (ev.Name.ToLower() == "list")
			{
				ev.IsAllowed = false;
				string message = $"{PlayerManager.players.Count}/{plugin.MaxPlayers}\n";
				foreach (Player player in Player.List)
					if (!player.IsHost)
						message += $"{player.Nickname} - {player.UserId}\n";
				if (string.IsNullOrEmpty(message))
					message = $"Нет игроков онлайн.";
				ev.CommandSender.RaReply($"{message}", true, true, string.Empty);
			}
			else if (ev.Name.ToLower() == "stafflist")
			{
				ev.IsAllowed = false;
				bool isStaff = false;
				string names = "";
				foreach (Player player in Player.List)
				{
					if (player.ReferenceHub.serverRoles.RemoteAdmin)
					{
						isStaff = true;
						names += $"{player.Nickname} ";
					}
				}

				string response = isStaff ? names : $"Нет администрации онлайн.";
				ev.CommandSender.RaReply($"{PlayerManager.players.Count}/{plugin.MaxPlayers}\n{response}", true, true, string.Empty);
			}
			#endregion
			#region logs
			if (ev.Sender.UserId == "") return;
			if (ev.Sender.Nickname == "Dedicated Server") return;
			if (ev.Sender.UserId == "-@steam") return;
			string Args = string.Join(" ", ev.Arguments);
			string msg = "";
			try
			{
				if (ev.Name == "forceclass")
				{
					string targets = "";
					RoleType role = RoleType.None;
					var role_id = 0;
					if (ev.Arguments.Count > 1)
					{
						string[] spearator = { "." };
						string[] strlist = ev.Arguments[0].Split(spearator, 2,
							   System.StringSplitOptions.RemoveEmptyEntries);
						foreach (string s in strlist)
						{
							try { targets += $"{s}^{Player.Get(int.Parse(s)).Nickname}^ "; } catch { }
						}
						try { role_id = Convert.ToInt32(ev.Arguments[1]); } catch { }
						try { role = (RoleType)Convert.ToInt32(ev.Arguments[1]); } catch { }
					}
					else
					{
						try { role_id = Convert.ToInt32(ev.Arguments[0]); } catch { }
						try { role = (RoleType)Convert.ToInt32(ev.Arguments[0]); } catch { }
					}
					msg = $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) использовал команду: {ev.Name} {targets} {role_id}^{role}^ {Args.Replace(ev.Arguments[0], "").Replace($"{role_id}", "")}";
				}
				else if (ev.Name == "request_data")
				{
					string targets = "";
					string[] spearator = { "." };
					string[] strlist = ev.Arguments[1].Split(spearator, 2,
						   System.StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in strlist)
					{
						targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ ";
					}
					msg = $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) использовал команду: {ev.Name} {ev.Arguments[0]} {targets} {Args.Replace(ev.Arguments[0], "").Replace(ev.Arguments[1], "")}";
				}
				else if (ev.Name == "give")
				{
					string targets = "";
					ItemType item = ItemType.Coin;
					var item_id = 0;
					if (ev.Arguments.Count > 1)
					{
						string[] spearator = { "." };
						string[] strlist = ev.Arguments[0].Split(spearator, 2,
							   System.StringSplitOptions.RemoveEmptyEntries);
						foreach (string s in strlist)
						{
							try { targets += $"{s}^{Player.Get(int.Parse(s)).Nickname}^ "; } catch { }
						}
						try { item = (ItemType)Convert.ToInt32(ev.Arguments[1]); } catch { }
						try { item_id = Convert.ToInt32(ev.Arguments[1]); } catch { }
					}
					else
					{
						try { item = (ItemType)Convert.ToInt32(ev.Arguments[0]); } catch { }
						try { item_id = Convert.ToInt32(ev.Arguments[0]); } catch { }
					}
					msg = $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) использовал команду: {ev.Name} {targets} {item_id}^{item}^ {Args.Replace(ev.Arguments[0], "").Replace($"{item_id}", "")}";
				}
				else if (ev.Name == "overwatch" || ev.Name == "bypass" || ev.Name == "heal" || ev.Name == "god" || ev.Name == "noclip")
				{
					string targets = "";
					string[] spearator = { "." };
					string[] strlist = ev.Arguments[0].Split(spearator, 2,
						   System.StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in strlist)
					{
						targets += $"{s}^{Player.Get(int.Parse(s))?.Nickname}^ ";
					}
					msg = $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) использовал команду: {ev.Name} {targets} {Args.Replace(ev.Arguments[0], "")}";
				}
				else if (ev.Name == "bring" || ev.Name == "goto")
				{
					string target = $"{ev.Arguments[0]}^{Player.Get(int.Parse(ev.Arguments[0]))?.Nickname}^ ";
					msg = $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) использовал команду: {ev.Name} {target} {Args.Replace(ev.Arguments[0], "")}";
				}
				else
				{
					msg = $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) использовал команду: {ev.Name} {Args}";
				}
			}
			catch
			{
				msg = $":keyboard: {ev.Sender.Nickname}({ev.Sender.UserId}) использовал команду: {ev.Name} {Args}";
			}
			var main = DiscordLogs.statplug.donate.main[ev.Sender.UserId];
			send.sendmsg($"{msg}");
			if (main.sr || main.hr || main.ghr || main.ar || main.gar)
			{
				send.sendralog($"{msg.Replace($"{ev.Sender.Nickname}({ev.Sender.UserId})", main.name)}");
				if (!abusetrue) send.sendra($"{msg.Replace($"{ev.Sender.Nickname}({ev.Sender.UserId})", main.name)}");
			}
			else if (main.pr || main.vr || main.vpr)
			{
				send.senddonateralog($"{msg.Replace($"{ev.Sender.Nickname}({ev.Sender.UserId})", main.name)}");
			}
			else
			{
				if (!abusetrue) send.sendra($"{msg}");
			}
			#endregion
		}

        public void OnWaitingForPlayers()
		{
			send.sendmsg($":hourglass: Ожидание игроков...");
			send.senddonateralog($"⌛ Ожидание игроков...");
			if (firststart)
			{
				send.sendplayers();
				send.sendinfo();
				send.fatalsendmsgtime();
				firststart = false;
			}
		}

		public void OnRoundStart()
		{
			agree.Clear();
			abusetrue = false;
			abusestart = false;
			roundstart = true;
			send.sendmsg($":arrow_forward: Раунд запущен: {Player.List.Count()} игроков на сервере.");
			send.senddonateralog($"▶️ Раунд запущен: {Player.List.Count()} игроков на сервере.");
		}

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			roundstart = false;
			send.sendmsg($":stop_button: Раунд закончен: {Player.List.Count()} игроков онлайн.");
			send.senddonateralog($"⏹️ Раунд закончен: {Player.List.Count()} игроков онлайн.");
		}

		public void OnCheaterReport(ReportingCheaterEventArgs ev)
		{
			if (ev.IsAllowed)
			{
				send.sendmsg($"**Отправлен репорт на читера: {ev.Reporter.UserId} зарепорчен {ev.Reported.UserId} за {ev.Reason}.**");
			}
		}
		public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
		{
			if (ev.Name == "abuse")
            {
				ev.IsAllowed = false;
                if (abusestart)
				{
					string yes = "yes";
					if (ev.Arguments[0] == yes)
					{
						if (agree.ContainsKey(ev.Player.ReferenceHub.characterClassManager.UserId))
						{
							ev.ReturnMessage = "\nВы уже голосовали";
							ev.Color = "red";
						}
                        else
						{
							abuseagree++;
							agree.Add(ev.Player.ReferenceHub.characterClassManager.UserId, true);
							ev.ReturnMessage = "\nВы успешно проголосовали за.";
							ev.Color = "green";
						}
                    }
                    else
					{
						ev.ReturnMessage = $"\nИспользуйте:\n.abuse {yes}";
						ev.Color = "red";
					}
				}
                else
				{
					ev.ReturnMessage = "\nГолосование не начато";
					ev.Color = "red";
				}
            }
			if (ev.Player.UserId == "-@steam") return;
			string Argies = string.Join(" ", ev.Arguments);
			send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) использовал команду на [ё]: {ev.Name} {Argies}");
		}

		public void OnRespawn(RespawningTeamEventArgs ev)
		{
			string msg = ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency ? $":spy: Chaos Insurgency" : $":cop: Nine-Tailed Fox";
			send.sendmsg($"Приехал отряд {msg} в кол-ве {ev.Players.Count} человек.");
		}
		public void OnGenInsert(InsertingGeneratorTabletEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} вставил планшет в генератор.");
		}

		public void OnGenOpen(OpeningGeneratorEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} открыл генератор.");
		}

		public void OnGenUnlock(UnlockingGeneratorEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} разблокировал дверь генератора.");
		}

		public void On106Contain(ContainingEventArgs ev)
		{
			if (!ev.IsAllowed)
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} попытался восстановить условия содержания SCP 106.");
			}
			else
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} успешно восстановил условия содержания SCP 106.");
			}
		}

		public void On106CreatePortal(CreatingPortalEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} создал портал.");
		}

		public void OnItemChanged(ChangingItemEventArgs ev)
		{
			send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} поменял предмет в руке: {ev.OldItem.id} -> {ev.NewItem.id}.");
		}

		public void On079GainExp(GainingExperienceEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} получил {ev.Amount} опыта за {ev.GainType}.");
		}

		public void On079GainLvl(GainingLevelEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} получил {ev.NewLevel} уровень 079.");
		}

		public void OnPlayerLeave(LeftEventArgs ev)
		{
			send.sendmsg($":arrow_left: **{ev.Player.Nickname} - {ev.Player.UserId} ливнул с сервера.**");
			send.sendplayersinfo();
		}

		public void OnPlayerReload(ReloadingWeaponEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} перезарядил оружие: {ev.Player.CurrentItem.id}.");
		}

		public void OnWarheadAccess(ActivatingWarheadPanelEventArgs ev)
		{
			if (!ev.IsAllowed)
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} попытался получить доступ к крышке кнопки детонации альфа-боеголовки.");
			}
			else
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} получил доступ к крышке кнопки детонации альфа-боеголовки.");
			}
		}

		public void OnElevatorInteraction(InteractingElevatorEventArgs ev)
		{
			if (!ev.IsAllowed)
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} попытался вызвать лифт.");
			}
			else
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} вызвал лифт.");
			}
		}

		public void OnLockerInteraction(InteractingLockerEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} открыл шкафчик.");
		}

		public void OnTriggerTesla(TriggeringTeslaEventArgs ev)
		{
			if (ev.IsTriggerable)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} заагрил теслу.");
		}

		public void OnGenClosed(ClosingGeneratorEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} закрыл генератор.");
		}

		public void OnGenEject(EjectingGeneratorTabletEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} вставил планшет в генератор.");
		}

		public void OnDoorInteract(InteractingDoorEventArgs ev)
		{
			if (ev.Door.GetComponent<DoorNametagExtension>()?.GetName == "") return;
			if (ev.IsAllowed)
				send.sendmsg(ev.Door.NetworkTargetState
						? $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) закрыл дверь: {ev.Door.GetComponent<DoorNametagExtension>()?.GetName}."
						: $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) открыл дверь: {ev.Door.GetComponent<DoorNametagExtension>()?.GetName}."
						);
		}

		public void On914Activation(ActivatingEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) активировал SCP-914, настройки: {Scp914Machine.singleton.knobState}.");
		}

		public void On914KnobChange(ChangingKnobSettingEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) изменил настройки SCP-914 на {ev.KnobSetting}.");
		}

		public void OnPocketEnter(EnteringPocketDimensionEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.ReferenceHub.GetCustomRole()} ({ev.Player.ReferenceHub.GetCustomRole()}) попал в карманное измерение.");
		}

		public void OnPocketEscape(EscapingPocketDimensionEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.ReferenceHub.GetCustomRole()} ({ev.Player.ReferenceHub.GetCustomRole()}) сбежал из карманного измерения.");
		}

		public void On106Teleport(TeleportingEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} сбежал из карманного измерения.");
		}

		public void On079Tesla(InteractingTeslaEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($":zap: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) заагрил теслу.");
		}

		public void OnPlayerHurt(HurtingEventArgs ev)
		{
			if (ev.IsAllowed)
				try
				{
					if (ev.Attacker.UserId == ev.Target.UserId) return;
					if (ev.Attacker != null && ev.Target.ReferenceHub.GetCustomRole().GetTeam() == ev.Attacker.ReferenceHub.GetCustomRole().GetTeam() && ev.Target != ev.Attacker)
						send.sendmsg($":crossed_swords: **{ev.Attacker.Nickname} - {ev.Attacker.UserId} ({ev.Attacker.ReferenceHub.GetCustomRole()}) нанес {ev.Amount} урона {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.ReferenceHub.GetCustomRole()}) с {DamageTypes.FromIndex(ev.Tool).name}.**");
					else
					{
						send.sendmsg($"{ev.HitInformations.Attacker}  нанес {ev.Amount} урона {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.ReferenceHub.GetCustomRole()}) с {DamageTypes.FromIndex(ev.Tool).name}.");
					}
				}
				catch
				{
				}
		}

		public void OnPlayerDeath(DiedEventArgs ev)
		{
			try
			{
				if (ev.Killer.UserId == ev.Target.UserId) return;
				if (ev.Killer != null && ev.Target.ReferenceHub.GetCustomRole().GetTeam() == ev.Killer.ReferenceHub.GetCustomRole().GetTeam())
				{
					send.sendtk($":o: **{ev.Killer.Nickname} - {ev.Killer.UserId} ({ev.Killer.ReferenceHub.GetCustomRole()}) убил {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.ReferenceHub.GetCustomRole()}) с {DamageTypes.FromIndex(ev.HitInformations.Tool).name}.**");
					send.sendmsg($":o: **{ev.Killer.Nickname} - {ev.Killer.UserId} ({ev.Killer.ReferenceHub.GetCustomRole()}) убил {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.ReferenceHub.GetCustomRole()}) с {DamageTypes.FromIndex(ev.HitInformations.Tool).name}.**");
				}
				else
				{
					send.sendmsg($":skull_crossbones: **{ev.Killer.Nickname} - {ev.Killer.UserId} ({ev.Killer.ReferenceHub.GetCustomRole()}) убил {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.ReferenceHub.GetCustomRole()}) с {DamageTypes.FromIndex(ev.HitInformations.Tool).name}.**");
				}
			}
			catch
			{
			}
		}

		public void OnGrenadeThrown(ThrowingGrenadeEventArgs ev)
		{
			if (ev.Player == null)
				return;
			if (ev.IsAllowed)
				send.sendmsg($":bomb: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) бросил гранату.");
		}

		public void OnMedicalItem(UsingMedicalItemEventArgs ev)
		{
			if (ev.Player == null)
				return;
			if (ev.IsAllowed)
				send.sendmsg($":medical_symbol: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) использовал {ev.Item}");
		}

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (ev.Player == null)
				return;
			if (ev.IsEscaped)
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} сбежал, новая роль: {ev.NewRole}.");
			}
			else
			{
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} появился за {ev.NewRole}.");
			}
		}

		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (ev.Player.Nickname != "Dedicated Server")
				send.sendmsg($":arrow_right: **{ev.Player.Nickname} - {ev.Player.UserId} присоединился к игре.**");
		}

		public void OnPlayerFreed(RemovingHandcuffsEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($":unlock: {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.ReferenceHub.GetCustomRole()}) был освобожден {ev.Cuffer.Nickname} - {ev.Cuffer.UserId} ({ev.Cuffer.ReferenceHub.GetCustomRole()})");
		}

		public void OnPlayerHandcuffed(HandcuffingEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($":lock: {ev.Target.Nickname} - {ev.Target.UserId} ({ev.Target.ReferenceHub.GetCustomRole()}) был связан {ev.Cuffer.Nickname} - {ev.Cuffer.UserId} ({ev.Cuffer.ReferenceHub.GetCustomRole()})");
		}

		public void OnPlayerBanned(BannedEventArgs ev)
		{
			send.sendmsg($":no_entry: {ev.Details.OriginalName} - {ev.Details.Id} забанен {ev.Details.Issuer} за {ev.Details.Reason}. До {new DateTime(ev.Details.Expires).ToString("dd.MM.yyyy HH:mm")}");
		}

		public void OnIntercomSpeak(IntercomSpeakingEventArgs ev)
		{
            try
			{
				if (ev.IsAllowed)
					send.sendmsg($":loud_sound: {ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) начал использовать интерком.");
			}
            catch { }
		}

		public void OnPickupItem(PickingUpItemEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) подобрал {ev.Pickup.ItemId}.");
		}

		public void OnDropItem(ItemDroppedEventArgs ev)
		{
			send.sendmsg($"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.ReferenceHub.GetCustomRole()}) дропнул {ev.Pickup.ItemId}.");
		}

		public void OnSetGroup(ChangingGroupEventArgs ev)
		{
			if (ev.IsAllowed)
				try
				{
					send.sendmsg(
						$"{ev.Player.Nickname} - {ev.Player.UserId} получил роль: **{ev.NewGroup.BadgeText} ({ev.NewGroup.BadgeColor})**.");
				}
				catch
				{
				}
		}
		public void OnWarheadDetonation()
		{
			send.sendmsg($":radioactive: **Альфа-боеголовка успешно взорвана**");
		}


		public void OnGenFinish(GeneratorActivatedEventArgs ev)
		{
			send.sendmsg($"Активировался генератор");
		}

		public void OnDecon(DecontaminatingEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($":biohazard: **Началось обеззараживание легкой зоны**");
		}

		public void OnWarheadStart(StartingEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($":radioactive: **Альфа-Боеголовка взорвется через {Warhead.Controller.NetworktimeToDetonation} секунд.**");
		}

		public void OnWarheadCancelled(StoppingEventArgs ev)
		{
			if (ev.IsAllowed)
				send.sendmsg($"***{ev.Player.Nickname} - {ev.Player.UserId} выключил Альфа-Боеголовку.***");
		}

		public void OnScp194Upgrade(UpgradingItemsEventArgs ev)
		{
			string players = "";
			foreach (Player player in ev.Players)
				players += $"\n{player.Nickname} - {player.UserId} ({player.ReferenceHub.GetCustomRole()})";
			string items = "";
			foreach (Pickup item in ev.Items)
				items += $"\n{item.ItemId}";

			send.sendmsg($"В SCP-914 улучшены: {players}, предметы: {items}.");
		}
	}
}