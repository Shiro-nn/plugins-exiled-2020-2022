using MongoDB.Bson;
using MongoDB.Driver;
using Qurre;
using Qurre.API;
using Qurre.API.Events;
using System;
using System.Linq;
using System.Threading;
namespace Loli.DataBase.Modules
{
	internal class Admins
	{
		internal readonly Manager Manager;
		public Admins(Manager manager) => Manager = manager;
		internal void Prefixs(RaRequestPlayerListEvent ev)
		{
			bool gameplayData = Module.GD(ev.CommandSender) || CheckPerms(ev.CommandSender, PlayerPermissions.GameplayData);
			string text = "\n";
			foreach (Player pl in Player.List.Where(x => x != null).OrderBy(x => x.Id))
			{
				string nick = pl.Nickname.Replace("\n", string.Empty);
				string dn = pl.DisplayNickname?.Trim();
				if (dn != null && dn != nick && dn != "") nick = $"{dn}<color=#855439>*</color> ({nick})";
				string nickname = $"({pl.Id}) {nick}";
				if (gameplayData)
				{
					string color = "white";
					if (pl.Overwatch) color = "#00d7ff";
					else if (pl.GetTeam() == Team.CDP) color = "#ff9900";
					else if (pl.GetTeam() == Team.SCP) color = "#ff0000";
					else if (pl.GetTeam() == Team.RSC) color = "#e2e26d";
					else if (pl.GetTeam() == Team.TUT) color = "#00ff00";
					else if (pl.Role == RoleType.ChaosConscript) color = "#58be58";
					else if (pl.Role == RoleType.ChaosMarauder) color = "#23be23";
					else if (pl.Role == RoleType.ChaosRepressor) color = "#38ac38";
					else if (pl.Role == RoleType.ChaosRifleman) color = "#1cac1c";
					else if (pl.Role == RoleType.FacilityGuard) color = "#afafa1";
					else if (pl.Role == RoleType.NtfPrivate) color = "#00a5ff";
					else if (pl.Role == RoleType.NtfCaptain) color = "#0200ff";
					else if (pl.Role == RoleType.NtfSergeant) color = "#0074ff";
					else if (pl.Role == RoleType.NtfSpecialist) color = "#1f7fff";
					nickname = $"<color={color}>{nickname}</color>";
				}
				try
				{
					string prefix = Module.Prefix(pl);
					text += $"{prefix}{nickname}\n";
				}
				catch
				{
					text += $"{nickname}\n";
				}
			}
			ev.CommandSender.RaReply($"$0 {text}".Replace("RA_", string.Empty), true, false, string.Empty);
			ev.Allowed = false;
			static bool CheckPerms(CommandSender commandSender, PlayerPermissions perms)
			{
				return (ServerStatic.IsDedicated && commandSender.FullPermissions) ||
						PermissionsHandler.IsPermitted(commandSender.Permissions, perms);
			}
		}
		internal void Ban(BanEvent ev)
		{
			DateTime ExpireDate = DateTime.Now.AddSeconds(ev.Duration);
			if (ev.Issuer.Nickname != "Dedicated Server")
			{
				new Thread(() =>
				{
					try
					{
						string uu = "steam";
						if (ev.Issuer.UserId.Contains("@discord")) uu = "discord";
						var collection = Manager.DataBase.GetCollection("accounts");
						var statsa = collection.Find(new BsonDocument(uu, ev.Issuer.UserId.Replace($"@{uu}", ""))).ToList();
						foreach (var document in statsa)
						{
							int bc = (int)document["bans"];
							bc++;
							var filterr = Builders<BsonDocument>.Filter.Eq("user", document["user"]);
							collection.UpdateOne(filterr, Builders<BsonDocument>.Update.Set("bans", bc));
						}
					}
					catch (Exception e) { Log.Info($"{e}"); }
				}).Start();
				string reason = "";
				if (ev.Reason != "") reason = $"<color=#ff0000>Причина</color>: {ev.Reason}";
				Map.Broadcast($"<size=30%><color=#6f6f6f><color=#ff0000>{ev.Issuer.Nickname}</color> забанил <color=#ff0000>{ev.Target.Nickname}</color> " +
					$"до <color=#ff0000>{ExpireDate:dd.MM.yyyy HH:mm}</color>. {reason}</color></size>", 15);
			}
		}
		internal void Kick(KickEvent ev)
		{
			new Thread(() =>
			{
				string uu = "steam";
				if (ev.Issuer.UserId.Contains("@discord")) uu = "discord";
				var collection = Manager.DataBase.GetCollection("accounts");
				var statsa = collection.Find(new BsonDocument(uu, ev.Issuer.UserId.Replace($"@{uu}", ""))).ToList();
				foreach (var document in statsa)
				{
					int kc = (int)document["kicks"];
					kc++;
					var filter = Builders<BsonDocument>.Filter.Eq("user", document["user"]);
					collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("kicks", kc));
				}
			}).Start();
			string reason = "";
			if (ev.Reason != "") reason = $"<color=#ff0000>Причина</color>: {ev.Reason}";
			Map.Broadcast($"<size=30%><color=#6f6f6f><color=#ff0000>{ev.Issuer.Nickname}</color> кикнул <color=#ff0000>{ev.Target.Nickname}</color>. {reason}</color></size>", 15);
		}
	}
}