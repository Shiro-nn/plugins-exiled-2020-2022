using Loli.Discord;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Mirror;
using Loli.DataBase;
using System.Threading;
using Loli.BetterHints;
using Manager = Loli.DataBase.Manager;
namespace Loli.Modules
{
	public partial class EventHandlers
	{
		internal void Leave(LeaveEvent ev)
		{
			if (!Round.Ended) Plugin.Socket.Emit("server.leave", new object[] { Plugin.ServerID, ev.Player.Ip });
		}
		internal void ClearIps(RoundEndEvent _) => Plugin.Socket.Emit("server.clearips", new object[] { Plugin.ServerID });
		public void WaitingPlayers()
		{
			Rsc = 0;
			Cdp = 0;
			try { GameObject.Find("StartRound").transform.localScale = Vector3.zero; } catch { }
			RoundStarted = false;
			Upgrade914.Clear();
			AlphaKill = false;
			try { Extensions.CachedItems.Clear(); } catch { }
			try { Bloods.Clear(); } catch { }
		}
		internal void Elevator(InteractLiftEvent ev)
		{
			if (ev.Lift == null) return;
			ev.Lift.MovingSpeed = 3;
			if (Plugin.Anarchy) ev.Lift.MovingSpeed = 1;
		}
		public void Tesla(TeslaTriggerEvent ev)
		{
			try { if (!Alpha.Active) ev.Allowed = false; }
			catch { ev.Allowed = false; }
		}
		internal void RoundEnd(RoundEndEvent _)
		{
			RoundStarted = false;
			if (!Server.FriendlyFire)
			{
				Cassie.Send("ATTENTION TO ALL PERSONNEL . f f ENABLE");
			}
			Timing.CallDelayed(0.5f, () =>
			{
				foreach (Player player in Player.List) Levels.SetPrefix(player);
			});
		}
		public void UpdateDeaths(DiesEvent ev)
		{
			if (!ev.Allowed) return;
			if (DRole.ContainsKey(ev.Target.Id)) DRole[ev.Target.Id] = ev.Target.Role;
			else DRole.Add(ev.Target.Id, ev.Target.Role);
		}
		internal void EndFF(DamageProcessEvent ev)
		{
			if (!Round.Ended) return;
			ev.Allowed = true;
			ev.FriendlyFire = false;
			if (ev.Amount == 0) ev.Amount = 10f;
		}

		public static bool IsAlive(string userid)
		{
			if (!Pos.TryGetValue(userid, out var _data)) return true;
			return _data.Alive;
		}
		public void PosCheck()
		{
			try
			{
				foreach (Player pl in Player.List)
				{
					if (!Pos.ContainsKey(pl.UserId)) Pos.Add(pl.UserId, new VecPos());
					if (pl.Role is not RoleType.Spectator && Vector3.Distance(Pos[pl.UserId].Pos, pl.Position) < 0.1)
					{
						if (Pos[pl.UserId].sec > 30)
						{
							Pos[pl.UserId].Alive = false;
						}
						else
						{
							Pos[pl.UserId].sec += 5;
							Pos[pl.UserId].Pos = pl.Position;
						}
					}
					else
					{
						Pos[pl.UserId].Alive = true;
						Pos[pl.UserId].sec = 0;
						Pos[pl.UserId].Pos = pl.Position;
					}
				}
			}
			catch { }
		}
		internal void CheckEscape(Player pl)
		{
			if (!Round.Started) return;
			if (pl is not null && pl.Escape is not null && Vector3.Distance(pl.Position, pl.Escape.worldPosition) < global::Escape.radius)
			{
				bool _cuffed = pl.Cuffed;
				if (!_cuffed && pl.GetCustomRole() is Module.RoleType.FacilityGuard)
				{
					pl.Position = Map.GetRandomSpawnPoint(RoleType.NtfSpecialist);
					pl.BlockSpawnTeleport = true;
					pl.DropItems();
					pl.SetRole(RoleType.NtfSpecialist, false, CharacterClassManager.SpawnReason.Respawn);
				}
				else if (_cuffed)
				{
					var team = pl.GetTeam();
					if (team is Team.CHI) pl.SetRole(RoleType.NtfSergeant, false, CharacterClassManager.SpawnReason.Respawn);
					else if (team is Team.MTF) pl.SetRole(RoleType.ChaosRepressor, false, CharacterClassManager.SpawnReason.Respawn);
				}
			}
		}
		public void AnnouncingMTF(MTFAnnouncementEvent ev)
		{
			ev.Allowed = false;
			Cassie.Send($"XMAS_EPSILON11 {ev.UnitName} XMAS_HASENTERED {ev.UnitNumber} XMAS_SCPSUBJECTS");
		}

		public void AutoEndZeroPlayers()
		{
			if (Round.Started && !Round.Ended && Player.List.Count() == 0) Round.Restart();
		}
		internal void DoSpawn(DeadEvent ev)
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
		internal void DamageHint(DamageProcessEvent ev)
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
		internal void SinkHole(SinkholeWalkingEvent ev)
		{
			if (ev.Event == HazardEventsType.Exit && ev.Player.Position.y.Difference(-2000) > 50)
			{
				ev.Player.DisableEffect(EffectType.SinkHole);
				ev.Player.DisableEffect(EffectType.Corroding);
			}
			else if (ev.Player.GetTeam() == Team.SCP || ev.Player.Role == RoleType.Tutorial) ev.Allowed = false;
			else if (ev.Event == HazardEventsType.Stay && !ev.Player.GodMode &&
				Vector3.Distance(ev.Player.Position, ev.Sinkhole.Transform.position) < ev.Sinkhole.DistanceToGiveEffect * 0.7f)
			{
				ev.Player.DisableEffect(EffectType.SinkHole);
				ev.Player.Position = Vector3.down * 1998.5f;
				ev.Player.EnableEffect(EffectType.Corroding);
				ev.Allowed = false;
			}
		}
		internal void Tantrum(TantrumWalkingEvent ev)
		{
			if (ev.Player.Role == RoleType.Tutorial || ev.Player.GetTeam() == Team.SCP) ev.Allowed = false;
		}
		internal void Voice(PressAltChatEvent ev)
		{
			if (ev.Player.Role is RoleType.Scp049)
			{
				ev.Player.Dissonance.MimicAs939 = ev.Value;
				return;
			}
			if (ev.Player.Role != RoleType.Scp079 && ev.Player.Team == Team.SCP &&
				Manager.Static.Data.Roles.TryGetValue(ev.Player.UserId, out var _data) && _data.Prime)
				ev.Player.Dissonance.MimicAs939 = ev.Value;
		}
		internal void BalanceGun914(UpgradePickupEvent ev)
		{
			var type = ev.Pickup.Info.ItemId;
			if (type == ItemType.GunAK || type == ItemType.GunCOM15 || type == ItemType.GunCOM18 || type == ItemType.GunCrossvec || type == ItemType.GunE11SR ||
				type == ItemType.GunFSP9 || type == ItemType.GunLogicer || type == ItemType.GunRevolver || type == ItemType.GunShotgun)
				ev.Allowed = false;
		}
		internal void BalanceGun914(UpgradedItemInventoryEvent ev)
		{
			var type = ev.Item.Type;
			if (type == ItemType.GunAK || type == ItemType.GunCOM15 || type == ItemType.GunCOM18 || type == ItemType.GunCrossvec || type == ItemType.GunE11SR ||
				type == ItemType.GunFSP9 || type == ItemType.GunLogicer || type == ItemType.GunRevolver || type == ItemType.GunShotgun)
				ev.Allowed = false;
		}
		internal void BalanceGun914(UpgradedItemPickupEvent ev)
		{
			var type = ev.Pickup.Type;
			if (type == ItemType.GunAK || type == ItemType.GunCOM15 || type == ItemType.GunCOM18 || type == ItemType.GunCrossvec || type == ItemType.GunE11SR ||
				type == ItemType.GunFSP9 || type == ItemType.GunLogicer || type == ItemType.GunRevolver || type == ItemType.GunShotgun)
				ev.Allowed = false;
		}
		internal void ForProSkillPlayersYes(DamageProcessEvent ev)
		{
			if (Plugin.Anarchy) return;
			if (ev.Attacker is null || ev.Target is null) return;
			if (ev.Attacker.Team != Team.SCP && (ev.Target.UserId == "-@steam" || ev.Target.UserId == "-@steam" ||
				ev.Target.UserId == "-@steam" || ev.Target.UserId == "-@steam"))
				ev.Amount *= 1.2f;
			if (ev.Attacker.Team != Team.SCP)
			{
				if (ev.Attacker.UserId == "-@steam") ev.Amount *= 0.7f;
				else if (ev.Attacker.UserId == "-@steam" || ev.Attacker.UserId == "-@steam" ||
					ev.Attacker.UserId == "-@steam") ev.Amount *= 0.8f;
			}
		}
		internal static List<Player> BlockedPlayers = new();
		internal void AntiMaybeCheaters(JoinEvent ev)
		{
			if (ev.Player.UserId.Contains("@northwood")) return;
			if (Plugin.Anarchy) return;
			if (ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam" &&
				ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam") return;
			if (UnityEngine.Random.Range(0, 4) != 1) return;
			var rand = UnityEngine.Random.Range(UnityEngine.Random.Range(200, 300), 940);
			Timing.CallDelayed(rand, () => { try { ev.Player.Connection.Disconnect(); } catch { } });
		}
		internal int AMCRandom { get; private set; } = UnityEngine.Random.Range(0, 5);
		internal void AntiMaybeCheaters(SpawnEvent ev)
		{
			if (ev.Player.UserId.Contains("@northwood")) return;
			if (Plugin.Anarchy) return;
			if (!Round.Started) return;
			if (ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam" &&
				ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam") return;
			if (ev.RoleType == RoleType.Spectator || ev.RoleType == RoleType.None || ev.RoleType == RoleType.Tutorial) return;
			if (AMCRandom != 2) return;
			NetworkIdentity identity = ev.Player.NetworkIdentity;
			ObjectDestroyMessage destroyMessage = new() { netId = identity.netId };
			foreach (Player pl in Player.List)
				pl.Connection.Send(destroyMessage, 0);
		}
		internal void AntiMaybeCheaters(RoleChangeEvent ev)
		{
			if (ev.Player.UserId.Contains("@northwood")) return;
			if (Plugin.Anarchy) return;
			if (!Round.Started) return;
			if (ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam" &&
				ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam") return;
			if (ev.NewRole == RoleType.Spectator || ev.NewRole == RoleType.None || ev.NewRole == RoleType.Tutorial) return;
			if (AMCRandom != 2) return;
			NetworkIdentity identity = ev.Player.NetworkIdentity;
			ObjectDestroyMessage destroyMessage = new() { netId = identity.netId };
			foreach (Player pl in Player.List)
				pl.Connection.Send(destroyMessage, 0);
		}
#pragma warning disable IDE1006
		internal class SteamData
		{
			public SteamLvl response { get; set; }
		}
		internal class SteamLvl
		{
			public int player_level { get; set; }
		}
		internal class SteamRespGames
		{
			public SteamPreGames response { get; set; }
		}
		internal class SteamPreGames
		{
			public int game_count { get; set; }
			public SteamGames[] games { get; set; }
		}
		internal class SteamGames
		{
			public int appid { get; set; }
			public int playtime_forever { get; set; }
		}
		internal class GeoIP
		{
			public string ip { get; set; }
			public string city { get; set; }
			public string region { get; set; }
			public string country { get; set; }
			public string loc { get; set; }
			public string org { get; set; }
			public string postal { get; set; }
			public string timezone { get; set; }
		}
		internal class DoubtfulData
		{
			public string id { get; set; }
			public bool banned { get; set; }
			public long created { get; set; }
			public string created_formatted { get; set; }
		}
		internal class UserPrivateData
		{
			public string steam { get; set; }
			public string[] ips { get; set; }
			public GeoIP[] geoips { get; set; }
		}
#pragma warning restore IDE1006
		internal void AntiCheaters(JoinEvent ev)
		{
#pragma warning disable CS0162
			return;
			new Thread(() =>
			{
				if (ev.Player.UserId.Contains("@northwood")) return;
				if (Plugin.Anarchy) return;
				string LowNick = ev.Player.Nickname.ToLower();
				if (ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam") return;


				if (ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				(ev.Player.Nickname != "" && (ev.Player.Nickname.Trim() == ev.Player.UserId.Replace("@steam", "") || LowNick.Contains("suetologa") ||
				(LowNick.Contains("black") && LowNick.Contains("suetolog")) || (LowNick.Contains("язагит") && LowNick.Contains("лера"))))) DoRun();
				else if (LowLevel(out int level))
				{
					GeoIP geoip = GetGeoIP();
					if (geoip.org.Contains("NVIDIA Ltd")) return;
					if (geoip.org.Contains("Cloudflare, Inc"))
					{
						DoRun("\nFrom: CloudFlare");
						return;
					}
					if (Check(geoip, "Moscow", "AS204490 Kontel LLC")) return;
					//if (Check(geoip, "Vladimir", "AS35645 Limited Liability Company VLADINFO")) return;
					if (Check(geoip, "Ussuriysk", "AS42038 Krivets Sergey Sergeevich")) return;
					if (Check(geoip, "Miass", "AS47733 Digital Networks Ltd")) return;
					//if (Check(geoip, "Kazan", "AS41668 JSC ER-Telecom Holding")) return;
					if (Check(geoip, "Moscow", "AS13178 LLC Real-net")) return;
					if (Check(geoip, "Rostov-na-Donu", "AS52094 BitByteNet LLC")) return;
					if (Check(geoip, "Moscow", "AS43936 PE Zinstein Hariton Vladimirovich")) return;
					if (Check(geoip, "Ulyanovsk", "AS8402 PJSC Vimpelcom")) return;
					//if (Check(geoip, "Saratov", "AS16345 PJSC Vimpelcom")) return;
					//if (Check(geoip, "Saratov", "AS12389 PJSC Rostelecom")) return;
					//if (Check(geoip, "Chelyabinsk", "AS8369 Intersvyaz-2 JSC")) return;
					if (Check(geoip, "Kyiv", "AS210177 LLC KHART LIAGUE OF TREYD")) return;
					if (Check(geoip, "Yalta", "AS57093 Yalta-TV KOM Ltd.")) return;
					if (Check(geoip, "Syktyvkar", "AS21191 Joint Stock Company TransTeleCom")) return;
					//if (Check(geoip, "Samara", "AS34533 JSC ER-Telecom Holding")) return;
					if (Check(geoip, "Mikhaylovsk", "AS42526 Sll Computer & Communication System")) return;
					if (Check(geoip, "Moscow", "AS47954 Alpha Net Telecom Ltd")) return;
					if (Check(geoip, "Kizilyurt", "AS60936 LLC Skynet")) return;
					if (Check(geoip, "Nevinnomyssk", "AS51158 Mobile Trend Ltd")) return;
					if (Check(geoip, "Ansan-si", "AS4766 Korea Telecom")) return;
					if (Check(geoip, "Moscow", "AS57712 NPF SOFTVIDEO Ltd.")) return;
					if (Check(geoip, "Aktobe", "AS9198 JSC Kazakhtelecom")) return;
					if (Check(geoip, "Taman’", "AS42239 Ltd. Cypher")) return;
					if (Check(geoip, "Yaroslavl", "AS8402 PJSC Vimpelcom")) return;
					if (Check(geoip, "Novosibirsk", "AS21127 Joint Stock Company TransTeleCom")) return;
					if (Check(geoip, "Moscow", "AS35816 Lancom Ltd.")) return;
					if (Check(geoip, "Sal’sk", "AS47626 Timer, LLC")) return;
					if (Check(geoip, "Kyiv", "AS41820 New Information Systems PP")) return;
					if (Check(geoip, "Moscow", "AS51579 Korporatvniy partner Ltd")) return;
					if (Check(geoip, "Vienna", "AS8447 A1 Telekom Austria AG")) return;
					if (Check(geoip, "Kimry", "AS47193 LAN-Optic, Ltd")) return;
					if (Check(geoip, "Saint Petersburg", "AS42893 Nord-West Link NP")) return;
					if (Check(geoip, "Moscow", "AS50340 OOO Network of data-centers Selectel")) return;
					if (Check(geoip, "Jerusalem", "AS12400 Partner Communications Ltd.")) return;
					if (Check(geoip, "Vladikavkaz", "AS201556 Videoline LLC")) return;
					if (ev.Player.Nickname.ToLower() == "heners" && Check(geoip, "Saint Petersburg", "AS12389 PJSC Rostelecom")) return;
					DoubtfulData doubtful = GetDoubtful();
					if (doubtful is null) return;
					var doubtful_dodate = doubtful.created_formatted.Split('.');
					int _year = int.Parse(doubtful_dodate[2]);
					var doubtful_date = new DateTime(_year, int.Parse(doubtful_dodate[1]), int.Parse(doubtful_dodate[0]));
					double td = (DateTime.Now - doubtful_date).TotalDays;
					if (BetterCheck(geoip, level, doubtful.created, td, doubtful.banned, out bool hardcheck)) return;
					if (HardCheck(geoip, level, doubtful.created, td, doubtful.banned, hardcheck)) return;
					if (!doubtful.banned && _year != 1970 && td > 30) return;
					if (td < 3)
					{
						DoRun();
						return;
					}
					/*
					UserPrivateData userPrivate = GetGeoIPs();
					var geoips = userPrivate.geoips;
					if (geoips.Count() == 0) return;
					ulong traveler_success = 0;
					string last_loc = geoips.First().loc;
					List<string> regions = new();
					List<string> countries = new();
					int longtravells = 0;
					for (int i = geoips.Count() > 20 ? geoips.Count() - 10 : 0; i < geoips.Count(); i++)
					{
						var _geo = geoips[i];
						if (regions.Contains(_geo.region)) continue;
						if (_geo.loc == last_loc) continue;
						regions.Add(_geo.region);
						try { if (!countries.Contains(_geo.country)) countries.Add(_geo.country); } catch { }
						var trv = CalculateDistance(last_loc, _geo.loc);
						if (trv > 200) longtravells++;
						traveler_success += trv;
						last_loc = _geo.loc;
					}
					if (longtravells > 3 && ((traveler_success > 3000 && regions.Count > 2) || countries.Count > 3))
					{
						DoRun();
						return;
					}
					*/
					string hook = "https://discord.com/api/webhooks";
					Webhook webhk = new(hook);
					List<Embed> listEmbed = new();
					Embed embed = new()
					{
						Title = "Мутная личность на сервере",
						Color = 1,
						Description = $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Ip})\nПровайдер: {geoip.org}\nГород: {geoip.city} ({geoip.country})",
						TimeStamp = DateTimeOffset.Now
					};
					listEmbed.Add(embed);
					webhk.Send("На сервер подключилась мутная личность.", Plugin.ServerName, null, false, embeds: listEmbed);
				}
				/*
				ulong CalculateDistance(string source, string destination)
				{
					var tempArray = source?.Split(',');
					if (tempArray == null) return 0;

					if (!float.TryParse(tempArray[0]?.Replace('.', ','), out float tempFloat)) return 0; var sourceLatitude = tempFloat * Math.PI / 180;

					if (!float.TryParse(tempArray[1]?.Replace('.', ','), out tempFloat)) return 0; var sourceLongitude = tempFloat * Math.PI / 180;

					tempArray = destination?.Split(',');
					if (tempArray == null) return 0;

					if (!float.TryParse(tempArray[0]?.Replace('.', ','), out tempFloat)) return 0; var destinationLatitude = tempFloat * Math.PI / 180;

					if (!float.TryParse(tempArray[1]?.Replace('.', ','), out tempFloat)) return 0; var destinationLongitude = tempFloat * Math.PI / 180;

					var sourceLatitude_Cos = Math.Cos(sourceLatitude);
					var destinationLatitude_Cos = Math.Cos(destinationLatitude);
					var sourceLatitude_Sin = Math.Sin(sourceLatitude);
					var destinationLatitude_Sin = Math.Sin(destinationLatitude);

					var delta = destinationLongitude - sourceLongitude;
					var delta_Cos = Math.Cos(delta);
					var delta_Sin = Math.Sin(delta);

					return (ulong)Math.Atan2(Math.Sqrt(Math.Pow(destinationLatitude_Cos * delta_Sin, 2) +
						Math.Pow(sourceLatitude_Cos * destinationLatitude_Sin - sourceLatitude_Sin * destinationLatitude_Cos * delta_Cos, 2)),
						sourceLatitude_Sin * destinationLatitude_Sin + sourceLatitude_Cos * destinationLatitude_Cos * delta_Cos) * 6371;
				}
				*/
				bool LowLevel(out int lvl)
				{
					lvl = 0;
					try
					{
						if (ev.Player.UserId.Contains("@discord")) return false;
						var url = "https://" + $"api.steampowered.com/IPlayerService/GetSteamLevel/v1/?key={"C31CBA5EA1018FABEA411F37E8845EA9"}&steamid=" +
							ev.Player.UserId.Replace("@steam", "").Replace("@discord", "");
						var request = WebRequest.Create(url);
						request.Method = "GET";
						using var webResponse = request.GetResponse();
						using var webStream = webResponse.GetResponseStream();
						using var reader = new StreamReader(webStream);
						var data = reader.ReadToEnd();
						if (!data.Contains("player_level")) return true;
						SteamData json = JsonConvert.DeserializeObject<SteamData>(data);
						lvl = json.response.player_level;
						if (2 >= json.response.player_level) return true;
						return false;
					}
					catch { return false; }
				}
				bool LowHourse(out bool hardcheck)
				{
					hardcheck = false;
					try
					{
						if (ev.Player.UserId.Contains("@discord")) return false;
						var url = "https://" + $"api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={"C31CBA5EA1018FABEA411F37E8845EA9"}&steamid=" +
							ev.Player.UserId.Replace("@steam", "").Replace("@discord", "");
						var request = WebRequest.Create(url);
						request.Method = "GET";
						using var webResponse = request.GetResponse();
						using var webStream = webResponse.GetResponseStream();
						using var reader = new StreamReader(webStream);
						var data = reader.ReadToEnd();
						hardcheck = true;
						if (!data.Contains("games")) return false;
						SteamRespGames json = JsonConvert.DeserializeObject<SteamRespGames>(data);
						if (!json.response.games.TryFind(out var game, x => x.appid == 700330)) return true;
						if (3600 >= game.playtime_forever) return true;
						hardcheck = false;
						return false;
					}
					catch { return false; }
				}
				void DoRun(string text = "")
				{
					ev.Player.Disconnect("<b><color=red>Вам был закрыт доступ на сервера</color>\n<color=#00ff19>Определение автоматизировано,\nЕсли вас случайно занесло в черный список,\nОткройте тикет на сервере в Discord</color></b>");
					string hook = "https://discord.com/api/webhooks";
					Webhook webhk = new(hook);
					Embed embed = new()
					{
						Title = "Попытка подключения",
						Color = 1,
						Description = $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Ip}) {text}",
						TimeStamp = DateTimeOffset.Now
					};
					List<Embed> listEmbed = new() { embed };
					webhk.Send("Замечена попытка подключения на сервер мутной личности.", Plugin.ServerName, null, false, embeds: listEmbed);
				}
				bool Check(GeoIP json, string city, string org)
				{
					if (json.city == city && json.org == org)
					{
						DoRun($"\nПровайдер: {json.org}\nГород: {json.city} ({json.country})");
						return true;
					}
					return false;
				}
				bool HardCheck(GeoIP json, int lvl, long created, double exp, bool banned, bool hardcheck)
				{
					if (lvl != 0) return false;
					if (!hardcheck && !banned) return false;
					if ((LowNick.Contains("капитан") && json.region == "Sakha" && json.org == "AS12389 PJSC Rostelecom")
						|| (json.region == "Moscow" || json.region == "Moscow Oblast") && (json.org == "AS49368 DomoLAN Ltd." ||
							json.org == "AS8905 Digit One LLC" || json.org == "AS8402 PJSC Vimpelcom" ||
							json.org == "AS25513 PJSC Moscow city telephone network")
						|| (json.region == "Samara Oblast" && json.org == "AS56407 Zhigulevsk Cable Network LLC")
						|| (json.region == "Magadan Oblast" && json.org == "AS42136 DVTK LLC")
						|| (json.region == "Astrakhan" && json.org == "AS31133 PJSC MegaFon")
						|| (json.city == "Starokucherganovka" && json.org == "AS49718 Nizhnevolzhskie Telecommunication Networks Real LLC")
						|| (json.city == "Sterlitamak" && json.org == "AS24955 JSC Ufanet")
						|| (json.region == "Kurgan Oblast" && json.org == "AS29363 OOO 'RUS'")
						|| (json.region == "Dagestan" && json.org == "AS44391 JSC Elektrosvyaz")
						|| (json.region == "Dagestan" && json.org == "AS44391 JSC Elektrosvyaz")
						|| (json.region == "Irkutsk Oblast" && json.org == "AS57737 Joint Stock Company Angarsk Electrolysis Chemical Complex")
						|| json.region == "Ljubljana"
					)
					{
						DoRun($"\nПровайдер: {json.org}\nГород: {json.city} ({json.country})");
						return true;
					}
					return false;
				}
				bool BetterCheck(GeoIP json, int lvl, long created, double exp, bool banned, out bool hardcheck)
				{
					hardcheck = false;
					if (json.org == "AS9009 M247 Ltd")
					{
						DoRun($"\nПровайдер: {json.org}\nГород: {json.city} ({json.country})");
						return true;
					}
					if (lvl != 0) return false;
					if (!LowHourse(out hardcheck) && !banned) return false;
					if (((json.region == "Moscow" || json.region == "Moscow Oblast") && (json.org == "AS49368 DomoLAN Ltd." || json.org == "AS50473 Altagen JSC" ||
					json.org == "AS34602 MEGASVYAZ LLC" || json.org == "AS8901 GKU Mosgortelecom" || json.org == "AS58238 MKS Telecom LLC" ||
					json.org == "AS8402 PJSC Vimpelcom" || json.org == "AS8905 Digit One LLC" || json.org == "AS25513 PJSC Moscow city telephone network"))
						|| (json.city == "Tyumen" && json.org == "AS12389 PJSC Rostelecom")
						|| (json.region == "Moscow Oblast" && json.org == "AS12389 PJSC Rostelecom")
						|| (json.city == "Kursk" && json.org == "AS43038 MTS PJSC")
						|| (json.city == "Kazan" && json.org == "AS29194 MTS PJSC")
						|| (json.city == "Samara" && json.org == "AS34533 JSC ER-Telecom Holding")
						|| (json.city == "Saratov" && json.org == "AS16345 PJSC Vimpelcom")
						|| (json.city == "Vyshneve" && json.org == "AS43139 Maximum-Net LLC")
						|| (json.city == "Saint Petersburg" && (json.org == "AS35807 SkyNet Ltd." || json.org == "AS42065 ZAO ElectronTelecom"))
						|| (json.city == "Rostov-na-Donu" && json.org == "AS51200 LLC Digital Dialogue-Nets")
						|| (json.city == "Saratov" && json.org == "AS16345 PJSC Vimpelcom")
						|| (json.city == "Slavgorod" && json.org == "AS20485 Joint Stock Company TransTeleCom")
						|| (json.city == "Rostov-na-Donu" && json.org == "AS3216 PJSC Vimpelcom")
						|| (json.city == "Kolomna" && json.org == "AS12389 PJSC Rostelecom")
						|| (json.city == "Nur-Sultan" && json.org == "AS9198 JSC Kazakhtelecom")
						|| (json.city == "Odintsovo" && json.org == "AS47119 VL-Telecom + Ltd")
						|| (json.city == "Penza" && json.org == "AS41754 JSC ER-Telecom Holding")
						|| (json.city == "Yekaterinburg" && json.org == "AS31224 PJSC MegaFon")
						|| (json.city == "YugorskS" && json.org == "AS12389 PJSC Rostelecom")
						|| (json.city == "Syktyvkar" && json.org == "AS12389 PJSC Rostelecom")
						|| (json.city == "Zheleznogorsk" && json.org == "AS209372 SIA Singularity Telecom")
						|| (json.city == "Samara" && json.org == "AS34533 JSC ER-Telecom Holding")
						|| (json.city == "Jūrmala" && json.org == "AS20910 Baltcom SIA")
						|| (json.city == "Vilnius" && json.org == "AS8764 Telia Lietuva, AB")
						|| (json.city == "Surgut" && (json.org == "AS50923 Metroset Ltd." || json.org == "AS12389 PJSC Rostelecom"))
						|| (json.org == "AS6697 Republican Unitary Telecommunication Enterprise Beltelecom" && (json.city == "Homyel'" || json.city == "Barysaw")))
					{
						DoRun($"\nПровайдер: {json.org}\nГород: {json.city} ({json.country})");
						return true;
					}
					return false;
				}
				GeoIP GetGeoIP()
				{
					var url = $"{Plugin.APIUrl}/geoip?ip={(ev.Player.Ip == "127.0.0.1" ? "37.79.4.230" : ev.Player.Ip)}";
					var request = WebRequest.Create(url);
					request.Method = "POST";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					GeoIP json = JsonConvert.DeserializeObject<GeoIP>(data);
					return json;
				}
				/*
				UserPrivateData GetGeoIPs()
				{
					var url = $"{Plugin.APIUrl}/userips?steam={ev.Player.UserId.Replace("@steam", "")}&token={Plugin.ApiToken}";
					var request = WebRequest.Create(url);
					request.Method = "POST";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					UserPrivateData json = JsonConvert.DeserializeObject<UserPrivateData>(data);
					return json;
				}
				*/
				DoubtfulData GetDoubtful()
				{
					if (!ev.Player.UserId.Contains("@steam")) return null;
					var url = $"{Plugin.APIUrl}/doubtful?steam={ev.Player.UserId.Replace("@steam", "")}";
					var request = WebRequest.Create(url);
					request.Method = "POST";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					DoubtfulData json = JsonConvert.DeserializeObject<DoubtfulData>(data);
					return json;
				}
			}).Start();
#pragma warning restore CS0162
		}
	}
}