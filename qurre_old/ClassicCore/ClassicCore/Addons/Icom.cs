using Qurre.API;
namespace ClassicCore.Addons
{
	static internal class Icom
	{
		static internal void Update()
		{
			try
			{
				int cdp = 0;
				int rsc = 0;
				int mtf = 0;
				int ci = 0;
				int scp = 0;
				foreach (var pl in Player.List)
				{
					if (pl.Role is RoleType.Spectator) continue;
					else switch (pl.Team)
						{
							case Team.CDP: cdp++; break;
							case Team.RSC: rsc++; break;
							case Team.MTF: mtf++; break;
							case Team.CHI: ci++; break;
							case Team.SCP: scp++; break;
						}
				}
				Intercom icom = ReferenceHub.HostHub.GetComponent<Intercom>();
				string name = "<size=18%><color=#ff0000>f</color><color=#ff00aa>y</color><color=#ff00d4>d</color><color=#ff00f7>n</color><color=#ea00ff>e</color>" +
					"<color=black> ~~ Промокод на скидку в 5% => 733c-4d12-b261</color></size>\n";
				string stats = $"" +
					$"<size=12%>Живых:</size>\n" +
					$"<size=10%>" +
					$"<color=#ff7100>D: {cdp}</color>\n" +
					$"<color=#fdffbb>Ученых: {rsc}</color>\n" +
					$"<color=#0089c7>МОГ: {mtf}</color>\n" +
					$"<color=#0d9100>ХАОС: {ci}</color>\n" +
					$"<color=#ff0000>SCP: {scp}</color>\n" +
					$"<color=#C0C0C0>Активных генераторов: {Round.ActiveGenerators}</color>\n" +
					$"<color=#00ffff>Длительность раунда: {Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}</color>" +
					$"</size>";
				if (icom.remainingCooldown > 0f)
				{
					string newValue = icom.remainingCooldown switch
					{
						< 10 => "<color=#ff0>█████████<color=#9c9c00>█</color></color>",
						< 15 => "<color=#ff0>████████<color=#9c9c00>██</color></color>",
						< 20 => "<color=#ff0>███████<color=#9c9c00>███</color></color>",
						< 25 => "<color=#ff0>██████<color=#9c9c00>████</color></color>",
						< 35 => "<color=#ff0>█████<color=#9c9c00>█████</color></color>",
						< 40 => "<color=#ff0>████<color=#9c9c00>██████</color></color>",
						< 50 => "<color=#ff0>███<color=#9c9c00>███████</color></color>",
						< 55 => "<color=#ff0>██<color=#9c9c00>████████</color></color>",
						< 60 => "<color=#ff0>█<color=#9c9c00>█████████</color></color>",
						_ => "<color=#9c9c00>██████████</color>",
					};
					icom.CustomContent = $"<size=20%>{name}Перезапуск...</size>\n{stats}\n{newValue}";
				}
				else if (icom.Networkspeaker is not null)
				{
					string newValue = icom.speechRemainingTime switch
					{
						< 10 => "<color=#ff0>█<color=#9c9c00>█████████</color></color>",
						< 20 => "<color=#ff0>██<color=#9c9c00>████████</color></color>",
						< 30 => "<color=#ff0>███<color=#9c9c00>███████</color></color>",
						< 40 => "<color=#ff0>████<color=#9c9c00>██████</color></color>",
						< 50 => "<color=#ff0>█████<color=#9c9c00>█████</color></color>",
						< 60 => "<color=#ff0>██████<color=#9c9c00>████</color></color>",
						< 70 => "<color=#ff0>███████<color=#9c9c00>███</color></color>",
						< 80 => "<color=#ff0>████████<color=#9c9c00>██</color></color>",
						< 90 => "<color=#ff0>█████████<color=#9c9c00>█</color></color>",
						_ => "<color=#ff0>██████████</color>",
					};
					icom.CustomContent = $"<size=20%>{name}Трансляция...</size>\n{stats}\n{newValue}";
				}
				else icom.CustomContent = $"<size=20%>{name}Готово к использованию</size>\n{stats}";
			}
			catch { }
		}
	}
}