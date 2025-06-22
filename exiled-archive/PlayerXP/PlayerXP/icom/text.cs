using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerXP.icom
{
    public class text
	{
		private readonly Plugin plugin;
		public text(Plugin plugin) => this.plugin = plugin;
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		public void WaitingForPlayers()
		{
			Coroutines.Add(Timing.RunCoroutine(icomtext()));
		}
		public void RoundEnd(Exiled.Events.EventArgs.RoundEndedEventArgs ev)
		{
			Timing.KillCoroutines(Coroutines);
			Coroutines.Clear();
		}
		public IEnumerator<float> icomtext()
		{
			for (; ; )
			{
				string stats = $"<size=12%>Живых:</size>\n" +
					$"<size=10%>" +
					$"<color=#ff7100>D: {Player.List.Where(x => x.Role == RoleType.ClassD).ToList().Count}</color>\n" +
					$"<color=#fdffbb>Ученых: {Player.List.Where(x => x.Role == RoleType.Scientist).ToList().Count}</color>\n" +
					$"<color=#0089c7>МОГ: {Player.List.Where(x => x.Team == Team.MTF).ToList().Count}</color>\n" +
					$"<color=#0d9100>ХАОС: {Player.List.Where(x => x.Team == Team.CHI).ToList().Count}</color>\n" +
					$"<color=#15ff00>Длани: {Player.List.Where(x => x.Team == Team.TUT).ToList().Count}</color>\n" +
					$"<color=#ff0000>SCP: {Player.List.Where(x => x.Team == Team.SCP).ToList().Count}</color>\n" +
					$"<color=#00ffff>Длительность раунда: {Round.ElapsedTime.Minutes}:{Round.ElapsedTime.Seconds}</color>" +
					$"</size>";
				Intercom icom = UnityEngine.Object.FindObjectOfType<Intercom>();
				if (icom.Network_state == Intercom.State.Ready)
				{
					icom.Network_intercomText = $"<size=20%>Готово к использованию</size>\n{stats}";
				}
				else if (icom.Network_state == Intercom.State.Transmitting)
				{
					string newValue2 = "";
					int num2 = Mathf.CeilToInt(icom.speechRemainingTime);
					if (10 >= num2) newValue2 = "<color=#ff0>█<color=#9c9c00>█████████</color></color>";
					else if (20 >= num2) newValue2 = "<color=#ff0>██<color=#9c9c00>████████</color></color>";
					else if (30 >= num2) newValue2 = "<color=#ff0>███<color=#9c9c00>███████</color></color>";
					else if (40 >= num2) newValue2 = "<color=#ff0>████<color=#9c9c00>██████</color></color>";
					else if (50 >= num2) newValue2 = "<color=#ff0>█████<color=#9c9c00>█████</color></color>";
					else if (60 >= num2) newValue2 = "<color=#ff0>██████<color=#9c9c00>████</color></color>";
					else if (70 >= num2) newValue2 = "<color=#ff0>███████<color=#9c9c00>███</color></color>";
					else if (80 >= num2) newValue2 = "<color=#ff0>████████<color=#9c9c00>██</color></color>";
					else if (90 >= num2) newValue2 = "<color=#ff0>█████████<color=#9c9c00>█</color></color>";
					else if (100 >= num2) newValue2 = "<color=#ff0>██████████<color=#9c9c00></color></color>"; 
					icom.Network_intercomText = $"<size=20%>Трансляция...</size>\n{newValue2}\n{stats}";
				}
				else if (icom.Network_state == Intercom.State.TransmittingBypass)
				{
					icom.Network_intercomText = $"<size=20%>Трансляция...</size>\n{stats}";
				}
				else if (icom.Network_state == Intercom.State.Restarting)
				{
					string newValue = "";
					int num = Mathf.CeilToInt(icom.remainingCooldown);
					if (10 >= num) newValue = "<color=#ff0>█████████<color=#9c9c00>█</color></color>";
					else if (15 >= num) newValue = "<color=#ff0>████████<color=#9c9c00>██</color></color>";
					else if (20 >= num) newValue = "<color=#ff0>███████<color=#9c9c00>███</color></color>";
					else if (25 >= num) newValue = "<color=#ff0>██████<color=#9c9c00>████</color></color>";
					else if (35 >= num) newValue = "<color=#ff0>█████<color=#9c9c00>█████</color></color>";
					else if (40 >= num) newValue = "<color=#ff0>████<color=#9c9c00>██████</color></color>";
					else if (50 >= num) newValue = "<color=#ff0>███<color=#9c9c00>███████</color></color>";
					else if (55 >= num) newValue = "<color=#ff0>██<color=#9c9c00>████████</color></color>";
					else if (60 >= num) newValue = "<color=#ff0>█<color=#9c9c00>█████████</color></color>";
					icom.Network_intercomText = $"<size=20%>Перезапуск...</size>\n{newValue}\n{stats}";
				}
				else if (icom.Network_state == Intercom.State.AdminSpeaking)
				{
					icom.Network_intercomText = $"<size=20%>Использует админ</size>\n{stats}";
				}
				else if (icom.Network_state == Intercom.State.Muted)
				{
					icom.Network_intercomText = $"<size=20%><color=red>Вы замьючены</color></size>\n{stats}";
				}
				yield return Timing.WaitForSeconds(1f);
			}
		}
	}
}
