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
namespace Loli.Modules
{
	public partial class EventHandlers
	{
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
			if (!Plugin.RolePlay) ev.Lift.MovingSpeed = 3;
			if (Plugin.Anarchy) ev.Lift.MovingSpeed = 1;
		}
		public void Tesla(TeslaTriggerEvent ev)
		{
			if (Plugin.RolePlay)
			{
				if (ev.Player.Role == RoleType.Scientist || ev.Player.Team == Team.MTF) ev.Triggerable = false;
				else ev.Triggerable = true;
			}
			else
			{
				try { if (!Alpha.Active) ev.Triggerable = false; }
				catch { ev.Triggerable = false; }
			}
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
			if (ev.Target.Team == Team.SCP || ev.Target.Team == Team.TUT) return;
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

		public void PosCheck()
		{
			try
			{
				List<Player> nf = Player.List.Where(x => !Pos.ContainsKey(x.UserId)).ToList();
				foreach (Player p in nf) Pos.Add(p.UserId, new VecPos());
				List<Player> list = Player.List.ToList();
				foreach (Player pl in list)
				{
					if (Vector3.Distance(Pos[pl.UserId].Pos, pl.Position) < 0.1 && pl.Role != RoleType.Spectator)
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
			if (Plugin.ClansWars) return;
			if (pl != null && pl.Escape != null && Vector3.Distance(pl.Position, pl.Escape.worldPosition) < global::Escape.radius)
			{
				if (!pl.Cuffed && pl.GetCustomRole() == Module.RoleType.FacilityGuard && !Plugin.RolePlay)
				{
					pl.Position = Map.GetRandomSpawnPoint(RoleType.NtfSpecialist);
					pl.BlockSpawnTeleport = true;
					pl.DropItems();
					pl.SetRole(RoleType.NtfSpecialist, false, CharacterClassManager.SpawnReason.Respawn);
				}
				else if (pl.Cuffed)
				{
					if (Plugin.RolePlay) pl.SetRole(RoleType.Spectator, false, CharacterClassManager.SpawnReason.Respawn);
					else if (pl.GetTeam() == Team.CHI) pl.SetRole(RoleType.NtfSergeant, false, CharacterClassManager.SpawnReason.Respawn);
					else if (pl.GetTeam() == Team.MTF) pl.SetRole(RoleType.ChaosRepressor, false, CharacterClassManager.SpawnReason.Respawn);
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
			if (Plugin.ClansWars) return;
			if (Round.Started && !Round.Ended && Player.List.Count() == 0) Round.Restart();
		}
		internal void DoSpawn(DeadEvent ev)
		{
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) return;
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
			ev.Attacker.ShowHint($"<voffset=6em><b><color={color}><size=180%>{(ev.Amount == -1 ? "Убит" : Math.Round(ev.Amount))}</size></color></b></voffset>", 1);
		}
		internal void SinkHole(SinkholeWalkingEvent ev)
		{
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) ev.Allowed = false;
			else if (ev.Player.GetTeam() == Team.SCP || ev.Player.Role == RoleType.Tutorial) ev.Allowed = false;
			else if (Vector3.Distance(ev.Player.Position, ev.Sinkhole.Transform.position) < ev.Sinkhole.DistanceToGiveEffect * 0.7f && !ev.Player.GodMode)
			{
				ev.Player.DisableEffect(EffectType.SinkHole);
				ev.Player.Position = Vector3.down * 1998.5f;
				ev.Player.EnableEffect(EffectType.Corroding);
				ev.Allowed = false;
			}
		}
		internal void Voice(PressAltChatEvent ev)
		{
			if (!(ev.Player.Role == RoleType.Scp049 || (ev.Player.Role != RoleType.Scp079 && ev.Player.Team == Team.SCP &&
				Manager.Static.Data.Roles.TryGetValue(ev.Player.UserId, out var _data) && _data.Prime)
				/*|| pl.GetCustomRole() == Module.RoleType.Scp035 || pl.Tag.Contains(SerpentsHand.HandTag)*/)) return;
			ev.Player.Dissonance.MimicAs939 = ev.Value;
		}
		internal void Tantrum(TantrumWalkingEvent ev)
		{
			if (AutoEvents.StormBase.Enabled || AutoEvents.GunGame.Enabled || AutoEvents.TeamDeathmatch.Enabled) ev.Allowed = false;
			else if (ev.Player.Role == RoleType.Tutorial || ev.Player.GetTeam() == Team.SCP) ev.Allowed = false;
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
			if (ev.Player.UserId != "-@steam") return;
			/*if (ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam" &&
				ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam") return;*/
			if (UnityEngine.Random.Range(0, 4) != 1) return;
			var rand = UnityEngine.Random.Range(UnityEngine.Random.Range(200, 300), 940);
			Timing.CallDelayed(rand, () => { try { ev.Player.Connection.Disconnect(); } catch { } });
		}
		internal int AMCRandom { get; private set; } = UnityEngine.Random.Range(0, 30);
		internal void AntiMaybeCheaters(SpawnEvent ev)
		{
			if (ev.Player.UserId.Contains("@northwood")) return;
			if (Plugin.Anarchy) return;
			if (!Round.Started) return;
			if (ev.Player.UserId != "-@steam") return;
			/*if (ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam" &&
				ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam") return;*/
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
			if (ev.Player.UserId != "-@steam") return;
			/*if (ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam" &&
				ev.Player.UserId != "-@steam" && ev.Player.UserId != "-@steam") return;*/
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
			new Thread(() =>
			{
				if (ev.Player.UserId.Contains("@northwood")) return;
				if (Plugin.Anarchy) return;
				if (ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam") return;


				if (ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" || ev.Player.UserId == "-@steam" ||
				(ev.Player.Nickname != "" && (ev.Player.Nickname.Trim() == ev.Player.UserId.Replace("@steam", "") ||
				(ev.Player.Nickname.ToLower().Contains("black") && ev.Player.Nickname.ToLower().Contains("suetolog"))))) DoRun();//-@steam -@steam
				else if (ev.Player.Ip == "188.227.20.17" || ev.Player.Ip == "212.232.79.128" || ev.Player.Ip == "109.196.70.234" ||
				ev.Player.Ip.Contains("185.147.213.") || ev.Player.Ip.Contains("216.131.114.")) DoRun();
				else if (LowLevel())
				{
					GeoIP geoip = GetGeoIP();
					if (ev.Player.Ip.Contains("95.25.") && Check(geoip, "Moscow", "AS8402 PJSC Vimpelcom")) return;
					if (Check(geoip, "Moscow", "AS204490 Kontel LLC")) return;
					if (Check(geoip, "Vladimir", "AS35645 Limited Liability Company VLADINFO")) return;
					if (Check(geoip, "Novokuznetsk", "AS40995 Sibirskie Seti Ltd.")) return;
					if (Check(geoip, "Ussuriysk", "AS42038 Krivets Sergey Sergeevich")) return;
					if (Check(geoip, "Miass", "AS47733 Digital Networks Ltd")) return;
					if (Check(geoip, "Homyel'", "AS6697 Republican Unitary Telecommunication Enterprise Beltelecom")) return;
					//if (Check(geoip, "Kazan", "AS41668 JSC ER-Telecom Holding")) return;
					if (Check(geoip, "Moscow", "AS13178 LLC Real-net")) return;
					if (Check(geoip, "Rostov-na-Donu", "AS52094 BitByteNet LLC")) return;
					if (Check(geoip, "Saint Petersburg", "AS35807 SkyNet Ltd.")) return;
					if (Check(geoip, "Moscow", "AS43936 PE Zinstein Hariton Vladimirovich")) return;
					if (Check(geoip, "Moscow", "AS13335 Cloudflare, Inc.")) return;
					if (Check(geoip, "Ulyanovsk", "AS8402 PJSC Vimpelcom")) return;
					if (Check(geoip, "Saratov", "AS16345 PJSC Vimpelcom")) return;
					if (Check(geoip, "Saratov", "AS12389 PJSC Rostelecom")) return;
					if (Check(geoip, "Izhevsk", "AS3226 OOO NI")) return;
					if (Check(geoip, "Krasnodar", "AS3216 PJSC Vimpelcom")) return;
					DoubtfulData doubtful = GetDoubtful();
					if (doubtful is null) return;
					var doubtful_dodate = doubtful.created_formatted.Split('.');
					int _year = int.Parse(doubtful_dodate[2]);
					var doubtful_date = new DateTime(_year, int.Parse(doubtful_dodate[1]), int.Parse(doubtful_dodate[0]));
					if (!doubtful.banned && _year != 1970 && (DateTime.Now - doubtful_date).TotalDays > 10) return;
					UserPrivateData userPrivate = GetGeoIPs();
					var geoips = userPrivate.geoips;
					if(geoips.Count() == 0) return;
					ulong traveler_success = 0;
					string last_loc = geoips.First().loc;
					List<string> cities = new();
					List<string> countries = new();
					for (int i = 0; i < geoips.Count(); i++)
					{
						var _geo = geoips[i];
						if (cities.Contains(_geo.city)) continue;
						if (_geo.loc == last_loc) continue;
						cities.Add(_geo.city);
                        try { if (!countries.Contains(_geo.country)) countries.Add(_geo.country); } catch { }
						traveler_success += CalculateDistance(last_loc, _geo.loc);
						last_loc = _geo.loc;
					}
					if((traveler_success > 3000 && countries.Count > 2) || countries.Count > 3)
					{
						DoRun();
						return;
					}
					string hook = "https://discord.com/api/webhooks";
					Webhook webhk = new(hook);
					List<Embed> listEmbed = new();
					Embed embed = new();
					embed.Title = "Мутная личность на сервере";
					embed.Color = 1;
					embed.Description = $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Ip})\nПровайдер: {geoip.org}\nГород: {geoip.city} ({geoip.country})";
					embed.TimeStamp = DateTimeOffset.Now;
					listEmbed.Add(embed);
					webhk.Send("На сервер подключилась мутная личность.", Plugin.ServerName, null, false, embeds: listEmbed);
				}
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
				bool LowLevel()
				{
					try
					{
						if (ev.Player.UserId.Contains("@discord")) return false;
						var url = "https://" + $"api.steampowered.com/IPlayerService/GetSteamLevel/v1/?key=C6080922CC0D5C8AE73B1C2499805ED5&steamid=" +
							$"{ev.Player.UserId.Replace("@steam", "").Replace("@discord", "")}";
						var request = WebRequest.Create(url);
						request.Method = "GET";
						using var webResponse = request.GetResponse();
						using var webStream = webResponse.GetResponseStream();
						using var reader = new StreamReader(webStream);
						var data = reader.ReadToEnd();
						if (!data.Contains("player_level")) return true;
						SteamData json = JsonConvert.DeserializeObject<SteamData>(data);
						if (2 >= json.response.player_level) return true;
						return false;
					}
					catch { return false; }
				}
				void DoRun(string text = "")
				{
					ev.Player.Disconnect("Вам был закрыт доступ на сервера\nОпределение автоматизировано,\nесли вас случайно занесло в черный список, откройте тикет на сервере в Discord");
					string hook = "https://discord.com/api/webhooks";
					Webhook webhk = new(hook);
					List<Embed> listEmbed = new();
					Embed embed = new();
					embed.Title = "Попытка подключения";
					embed.Color = 1;
					embed.Description = $"{ev.Player.Nickname} - {ev.Player.UserId} ({ev.Player.Ip}) {text}";
					embed.TimeStamp = DateTimeOffset.Now;
					listEmbed.Add(embed);
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
				GeoIP GetGeoIP()
				{
					var url = "https://" + $"scpsl.store/api/geoip?ip={(ev.Player.Ip == "127.0.0.1" ? "37.79.4.230" : ev.Player.Ip)}";
					var request = WebRequest.Create(url);
					request.Method = "POST";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					GeoIP json = JsonConvert.DeserializeObject<GeoIP>(data);
					return json;
				}
				UserPrivateData GetGeoIPs()
				{
					var url = "https://" + $"scpsl.store/api/userips?steam={ev.Player.UserId.Replace("@steam", "")}&token={Plugin.ApiToken}";
					var request = WebRequest.Create(url);
					request.Method = "POST";
					using var webResponse = request.GetResponse();
					using var webStream = webResponse.GetResponseStream();
					using var reader = new StreamReader(webStream);
					var data = reader.ReadToEnd();
					UserPrivateData json = JsonConvert.DeserializeObject<UserPrivateData>(data);
					return json;
				}
				DoubtfulData GetDoubtful()
				{
					if (!ev.Player.UserId.Contains("@steam")) return null;
					var url = "https://" + $"scpsl.store/api/doubtful?steam={ev.Player.UserId.Replace("@steam", "")}";
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
		}
	}
}