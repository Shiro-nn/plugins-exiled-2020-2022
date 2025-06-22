using Qurre.API;
using System.Linq;
using Loli.Scps.Api;
using Loli.Modules;
namespace Loli.Addons
{
	public class Icom
	{
		public void Update()
		{
			try
			{
				var scp035 = Scp035.Get035();
				int DClass = Extensions.CountRoles(Team.CDP);
				int Scientists = Extensions.CountRoles(Team.RSC);
				int MTF = Extensions.CountRoles(Team.MTF);
				int ci = Extensions.CountRoles(Team.CHI);
				int SH = Player.List.Where(x => Spawns.SerpentsHand.ItsAliveHand(x)).Count();
				int scp = Extensions.CountRoles(Team.SCP) + scp035.Count;
				Intercom icom = ReferenceHub.HostHub.GetComponent<Intercom>();
				string name = "<size=18%><color=#ff0000>f</color><color=#ff00aa>y</color><color=#ff00d4>d</color><color=#ff00f7>n</color><color=#ea00ff>e</color>" +
					"<color=black> ~~ Промокод на скидку в 5% => 733c-4d12-b261</color></size>\n";
				string stats = $"" +
					$"<size=12%>Живых:</size>\n" +
					$"<size=10%>" +
					$"<color=#ff7100>D: {DClass}</color>\n" +
					$"<color=#fdffbb>Ученых: {Scientists}</color>\n" +
					$"<color=#0089c7>МОГ: {MTF}</color>\n" +
					$"<color=#0d9100>ХАОС: {ci}</color>\n" +
					$"<color=#15ff00>Длани: {SH}</color>\n" +
					$"<color=#ff0000>SCP: {scp}</color>\n" +
					$"<color=#C0C0C0>Активных генераторов: {Round.ActiveGenerators}</color>\n" +
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
					icom.CustomContent = $"<size=20%>{name}Перезапуск...</size>\n{stats}\n{newValue}";
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
					icom.CustomContent = $"<size=20%>{name}Трансляция...</size>\n{stats}\n{newValue2}";
				}
				else
				{
					icom.CustomContent = $"<size=20%>{name}Готово к использованию</size>\n{stats}";
				}
			}
			catch { }
		}
	}
}