using Exiled.API.Features;
using MongoDB.scp035.API;
using System.Linq;

namespace MongoDB.icom
{
	public class text
	{
		private readonly Plugin plugin;
		public text(Plugin plugin) => this.plugin = plugin;
		public void icomtext()
		{
			try
			{
				ReferenceHub scp035 = Scp035Data.GetScp035();
				int DClass = Extensions.CountRoles(Team.CDP);
				int Scientists = Extensions.CountRoles(Team.RSC);
				int MTF = Extensions.CountRoles(Team.MTF);
				int ci = Extensions.CountRoles(Team.CHI);
				int SH = sh.shEventHandlers.shPlayers.Where(x => Player.Get(x).Role == RoleType.Tutorial).ToList().Count;
				int scp = Extensions.CountRoles(Team.SCP) + (scp035 != null && scp035.GetRole() != RoleType.Spectator ? 1 : 0);
				Intercom icom = ReferenceHub.HostHub.GetComponent<Intercom>();
				string stats = $"" +
					$"<size=12%>Живых:</size>\n" +
					$"<size=10%>" +
					$"<color=#ff7100>D: {DClass}</color>\n" +
					$"<color=#fdffbb>Ученых: {Scientists}</color>\n" +
					$"<color=#0089c7>МОГ: {MTF}</color>\n" +
					$"<color=#0d9100>ХАОС: {ci}</color>\n" +
					$"<color=#15ff00>Длани: {SH}</color>\n" +
					$"<color=#ff0000>SCP: {scp}</color>\n" +
					$"<color=#00ffff>Длительность раунда: {Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}</color>" +
					$"</size>";
				if (icom.remainingCooldown > 0f)
				{
					string newValue = "";
					float num = icom.remainingCooldown;
					if (10 >= num) newValue = "<color=#ff0>█████████<color=#9c9c00>█</color></color>";
					else if (15 >= num) newValue = "<color=#ff0>████████<color=#9c9c00>██</color></color>";
					else if (20 >= num) newValue = "<color=#ff0>███████<color=#9c9c00>███</color></color>";
					else if (25 >= num) newValue = "<color=#ff0>██████<color=#9c9c00>████</color></color>";
					else if (35 >= num) newValue = "<color=#ff0>█████<color=#9c9c00>█████</color></color>";
					else if (40 >= num) newValue = "<color=#ff0>████<color=#9c9c00>██████</color></color>";
					else if (50 >= num) newValue = "<color=#ff0>███<color=#9c9c00>███████</color></color>";
					else if (55 >= num) newValue = "<color=#ff0>██<color=#9c9c00>████████</color></color>";
					else if (60 >= num) newValue = "<color=#ff0>█<color=#9c9c00>█████████</color></color>";
					else newValue = "<color=#ff0><color=#9c9c00>██████████</color></color>";
					icom.CustomContent = $"<size=20%>Перезапуск...</size>\n{stats}\n{newValue}";
				}
				else if (icom.Networkspeaker != null)
				{
					string newValue2 = "";
					float num2 = icom.speechRemainingTime;
					if (10 >= num2) newValue2 = "<color=#ff0>█<color=#9c9c00>█████████</color></color>";
					else if (20 >= num2) newValue2 = "<color=#ff0>██<color=#9c9c00>████████</color></color>";
					else if (30 >= num2) newValue2 = "<color=#ff0>███<color=#9c9c00>███████</color></color>";
					else if (40 >= num2) newValue2 = "<color=#ff0>████<color=#9c9c00>██████</color></color>";
					else if (50 >= num2) newValue2 = "<color=#ff0>█████<color=#9c9c00>█████</color></color>";
					else if (60 >= num2) newValue2 = "<color=#ff0>██████<color=#9c9c00>████</color></color>";
					else if (70 >= num2) newValue2 = "<color=#ff0>███████<color=#9c9c00>███</color></color>";
					else if (80 >= num2) newValue2 = "<color=#ff0>████████<color=#9c9c00>██</color></color>";
					else if (90 >= num2) newValue2 = "<color=#ff0>█████████<color=#9c9c00>█</color></color>";
					else newValue2 = "<color=#ff0>██████████<color=#9c9c00></color></color>";
					icom.CustomContent = $"<size=20%>Трансляция...</size>\n{stats}\n{newValue2}";
				}
				else
				{
					icom.CustomContent = $"<size=20%>Готово к использованию</size>\n{stats}";
				}
			}
			catch { }
		}
	}
}