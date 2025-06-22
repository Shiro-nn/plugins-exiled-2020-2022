using System;
using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using Grenades;
using MEC;
using System.Linq;
using UnityEngine;
using Mirror;
using Log = EXILED.Log;
using System.Text;
using System.Text.RegularExpressions;
using scp228ruj.API;
namespace Shop
{
	public class EventHandlers
	{
		private readonly Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;
		public Dictionary<string, Stats> Stats = new Dictionary<string, Stats>();
		public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
		private readonly Regex regexSmartSiteReplacer = new Regex(@"#fydne");
		private static System.Random rand = new System.Random();
		public static bool tptovodka = false;
		public static Pickup shop1 = new Pickup();
		public static Pickup shop2 = new Pickup();
		public static Pickup shop3 = new Pickup();
		public static Pickup shop4 = new Pickup();
		public static Pickup shop5 = new Pickup();
		public static Pickup shop6 = new Pickup();
		public static Pickup shop7 = new Pickup();
		public static Pickup shop8 = new Pickup();
		public static Pickup shop9 = new Pickup();
		public static Pickup shop10 = new Pickup();
		public static Pickup shop11 = new Pickup();
		public static Pickup shop12 = new Pickup();
		public static Pickup shop13 = new Pickup();
		public static Pickup shop14 = new Pickup();
		public static Pickup shop15 = new Pickup();
		public static Pickup shop16 = new Pickup();
		public static Pickup shop17 = new Pickup();
		public static Pickup shop18 = new Pickup();
		public static Pickup shop19 = new Pickup();
		public static Pickup shop20 = new Pickup();
		public static Pickup shop21 = new Pickup();
		public static Pickup shop22 = new Pickup();
		public static Pickup shop23 = new Pickup();
		public static Pickup shop24 = new Pickup();
		public static Pickup shop25 = new Pickup();

		public void OnWaitingForPlayers()
		{
			Stats.Clear();
			foreach (CoroutineHandle handle in Coroutines)
				Timing.KillCoroutines(handle);
		}

		public void OnRoundStart()
		{
			foreach (Stats stats in Stats.Values)
				Coroutines.Add(Timing.RunCoroutine(SecondCounter()));
			shopspawn();
			tptovodka = false;
		}

		private IEnumerator<float> SecondCounter()
		{
			for (; ; )
			{
				foreach (Stats stats in Stats.Values)

					yield return Timing.WaitForSeconds(1f);
			}
		}
		public static void shopspawn()
		{
			if (rand.Next(1, 100) < 25)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp049);
				Pickup q = Map.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Map.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Map.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Map.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Map.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Map.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Map.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Map.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Map.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Map.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Map.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Map.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Map.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Map.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Map.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Map.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Map.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Map.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Map.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Map.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Map.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Map.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Map.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Map.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Map.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast("<color=aqua>� ���� ������ ������� ��������� �� ������ �������</color>", 10);
				return;
			}
			if (rand.Next(1, 100) < 25)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp173);
				Pickup q = Map.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Map.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Map.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Map.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Map.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Map.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Map.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Map.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Map.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Map.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Map.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Map.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Map.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Map.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Map.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Map.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Map.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Map.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Map.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Map.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Map.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Map.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Map.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Map.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Map.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast("<color=aqua>� ���� ������ ������� ��������� �� ������ ��������</color>", 10);
				return;
			}
			if (rand.Next(1, 100) < 25)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp106);
				Pickup q = Map.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Map.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Map.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Map.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Map.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Map.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Map.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Map.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Map.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Map.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Map.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Map.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Map.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Map.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Map.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Map.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Map.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Map.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Map.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Map.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Map.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Map.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Map.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Map.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Map.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast("<color=aqua>� ���� ������ ������� ��������� �� ������ ����</color>", 10);
				foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
					if (door.DoorName.Contains("106_BOTTOM"))
					{
						door.Networklocked = true;
						door.NetworkisOpen = true;
					}
				return;
			}
			if (true)
			{
				Vector3 randomSP = Map.GetRandomSpawnPoint(RoleType.Scp93989);
				Pickup q = Map.SpawnItem(ItemType.KeycardContainmentEngineer, 100000, randomSP);
				Pickup w = Map.SpawnItem(ItemType.KeycardNTFLieutenant, 100000, randomSP + Vector3.left * 1f);
				Pickup e = Map.SpawnItem(ItemType.KeycardNTFCommander, 100000, randomSP + Vector3.left * 2f);
				Pickup r = Map.SpawnItem(ItemType.KeycardFacilityManager, 100000, randomSP + Vector3.right * 1f);
				Pickup t = Map.SpawnItem(ItemType.KeycardO5, 100000, randomSP + Vector3.right * 2f);
				Pickup y = Map.SpawnItem(ItemType.Radio, 100000, randomSP + Vector3.forward * 1f);
				Pickup u = Map.SpawnItem(ItemType.GunCOM15, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 1f);
				Pickup i = Map.SpawnItem(ItemType.GunProject90, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 2f);
				Pickup o = Map.SpawnItem(ItemType.GunE11SR, 100000, randomSP + Vector3.forward * 1f + Vector3.left * 3f);
				Pickup p = Map.SpawnItem(ItemType.GunMP7, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 1f);
				Pickup a = Map.SpawnItem(ItemType.GunLogicer, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 2f);
				Pickup s = Map.SpawnItem(ItemType.GunUSP, 100000, randomSP + Vector3.forward * 1f + Vector3.right * 3f);
				Pickup d = Map.SpawnItem(ItemType.Medkit, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 1f);
				Pickup f = Map.SpawnItem(ItemType.Adrenaline, 100000, randomSP + Vector3.forward * 2f + Vector3.right * 2f);
				Pickup g = Map.SpawnItem(ItemType.Painkillers, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 1f);
				Pickup h = Map.SpawnItem(ItemType.Flashlight, 100000, randomSP + Vector3.forward * 2f + Vector3.left * 2f);
				Pickup j = Map.SpawnItem(ItemType.MicroHID, 100000, randomSP + Vector3.back * 2f);
				Pickup k = Map.SpawnItem(ItemType.SCP500, 100000, randomSP + Vector3.back * 2f + Vector3.right * 1f);
				Pickup l = Map.SpawnItem(ItemType.SCP207, 100000, randomSP + Vector3.back * 2f + Vector3.right * 2f);
				Pickup z = Map.SpawnItem(ItemType.SCP018, 100000, randomSP + Vector3.back * 2f + Vector3.left * 1f);
				Pickup x = Map.SpawnItem(ItemType.SCP268, 100000, randomSP + Vector3.back * 2f + Vector3.left * 2f);
				Pickup c = Map.SpawnItem(ItemType.Disarmer, 100000, randomSP + Vector3.back * 1f + Vector3.right * 2f);
				Pickup v = Map.SpawnItem(ItemType.GrenadeFlash, 100000, randomSP + Vector3.back * 1f + Vector3.right * 1f);
				Pickup b = Map.SpawnItem(ItemType.GrenadeFrag, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				Pickup n = Map.SpawnItem(ItemType.WeaponManagerTablet, 100000, randomSP + Vector3.back * 1f + Vector3.left * 1f);
				shop1 = q;//10
				shop2 = w;//7
				shop3 = e;//10
				shop4 = r;//15
				shop5 = t;//20
				shop6 = y;//3
				shop7 = u;//7
				shop8 = i;//13
				shop9 = o;//15
				shop10 = p;//11
				shop11 = a;//17
				shop12 = s;//9
				shop13 = d;//3
				shop14 = f;//3
				shop15 = g;//2
				shop16 = h;//2
				shop17 = j;//20
				shop18 = k;//6
				shop19 = l;//5
				shop20 = z;//8
				shop21 = x;//13
				shop22 = c;//3
				shop23 = v;//4
				shop24 = b;//6
				shop25 = n;//3
				Map.Broadcast("<color=aqua>� ���� ������ ������� ��������� �� ������ ������</color>", 10);
				return;
			}
		}
		public void OnPickupItem(ref PickupItemEvent ev)
		{
			if (ev.Item.ItemId == ItemType.Coin)
			{
				ev.Allow = false;
				Stats[ev.Player.characterClassManager.UserId].money += 1;
				ev.Item.Delete();
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast("<color=aqua>+�������</color>", 3);
			}
			if (ev.Item == shop25)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 3)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 3 �������", 5);
					ev.Player.AddItem(ItemType.WeaponManagerTablet);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/3)", 5);
			}
			if (ev.Item == shop24)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 6)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 6;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 6 �����", 5);
					ev.Player.AddItem(ItemType.GrenadeFrag);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/6)", 5);
			}
			if (ev.Item == shop23)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 4)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 4;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 4 �������", 5);
					ev.Player.AddItem(ItemType.GrenadeFlash);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/5)", 5);
			}
			if (ev.Item == shop22)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 3)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 3 �������", 5);
					ev.Player.AddItem(ItemType.Disarmer);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/3)", 5);
			}
			if (ev.Item == shop21)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 13)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 13;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 13 �����", 5);
					ev.Player.AddItem(ItemType.SCP268);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/13)", 5);
			}
			if (ev.Item == shop20)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 8)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 8;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 8 �����", 5);
					ev.Player.AddItem(ItemType.SCP018);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/8)", 5);
			}
			if (ev.Item == shop19)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 5)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 5;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 5 �����", 5);
					ev.Player.AddItem(ItemType.SCP207);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/5)", 5);
			}
			if (ev.Item == shop18)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 6)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 6;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 6 �����", 5);
					ev.Player.AddItem(ItemType.SCP500);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/6)", 5);
			}
			if (ev.Item == shop17)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 20)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 20;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 20 �����", 5);
					ev.Player.AddItem(ItemType.MicroHID);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/20)", 5);
			}
			if (ev.Item == shop16)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 2)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 2;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 2 �������", 5);
					ev.Player.AddItem(ItemType.Flashlight);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/2)", 5);
			}
			if (ev.Item == shop15)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 2)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 2;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 2 �������", 5);
					ev.Player.AddItem(ItemType.Painkillers);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/2)", 5);
			}
			if (ev.Item == shop14)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 3)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 3 �������", 5);
					ev.Player.AddItem(ItemType.Adrenaline);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/3)", 5);
			}
			if (ev.Item == shop13)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 3)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 3 �������", 5);
					ev.Player.AddItem(ItemType.Medkit);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/3)", 5);
			}
			if (ev.Item == shop12)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 9)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 9;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 9 �����", 5);
					ev.Player.AddItem(ItemType.GunUSP);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/9)", 5);
			}
			if (ev.Item == shop11)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 17)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 17;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 17 �����", 5);
					ev.Player.AddItem(ItemType.GunLogicer);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/17)", 5);
			}
			if (ev.Item == shop10)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 11)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 11;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 11 �����", 5);
					ev.Player.AddItem(ItemType.GunMP7);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/11)", 5);
			}
			if (ev.Item == shop9)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 15)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 15;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 15 �����", 5);
					ev.Player.AddItem(ItemType.GunE11SR);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/15)", 5);
			}
			if (ev.Item == shop8)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 13)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 13;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 13 �����", 5);
					ev.Player.AddItem(ItemType.GunProject90);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/13)", 5);
			}
			if (ev.Item == shop1)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 10)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 10;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 10 �����", 5);
					ev.Player.AddItem(ItemType.KeycardContainmentEngineer);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/10)", 5);
			}
			if (ev.Item == shop2)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 7)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 7;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 7 �����", 5);
					ev.Player.AddItem(ItemType.KeycardNTFLieutenant);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/7)", 5);
			}
			if (ev.Item == shop3)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 10)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 10;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 10 �����", 5);
					ev.Player.AddItem(ItemType.KeycardNTFCommander);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/10)", 5);
			}
			if (ev.Item == shop4)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 15)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 15;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 15 �����", 5);
					ev.Player.AddItem(ItemType.KeycardFacilityManager);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/15)", 5);
			}
			if (ev.Item == shop5)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 20)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 20;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 20 �����", 5);
					ev.Player.AddItem(ItemType.KeycardO5);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/20)", 5);
			}
			if (ev.Item == shop6)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 3)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 3;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 3 �������", 5);
					ev.Player.AddItem(ItemType.Radio);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/3)", 5);
			}
			if (ev.Item == shop7)
			{
				ev.Allow = false;
				if (Stats[ev.Player.characterClassManager.UserId].money >= 7)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= 7;
					ev.Player.ClearBroadcasts();
					ev.Player.Broadcast("�� ������� ������ ���� ����� �� 7 �����", 5);
					ev.Player.AddItem(ItemType.GunCOM15);
					return;
				}
				ev.Player.ClearBroadcasts();
				ev.Player.Broadcast($"�� ������� �����({Stats[ev.Player.characterClassManager.UserId].money}/7)", 5);
			}
		}
		public void OnRoundEnd()
		{
			foreach (CoroutineHandle handle in Coroutines)
				Timing.KillCoroutines(handle);
			try
			{
				foreach (Stats stats in Stats.Values)
					Methods.SaveStats(stats);
			}
			catch (Exception)
			{
			}

			shop1 = null;
			shop2 = null;
			shop3 = null;
			shop4 = null;
			shop5 = null;
			shop6 = null;
			shop7 = null;
			shop8 = null;
			shop9 = null;
			shop10 = null;
			shop11 = null;
			shop12 = null;
			shop13 = null;
			shop14 = null;
			shop15 = null;
			shop16 = null;
			shop17 = null;
			shop18 = null;
			shop19 = null;
			shop20 = null;
			shop21 = null;
			shop22 = null;
			shop23 = null;
			shop24 = null;
			shop25 = null;
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			Timing.CallDelayed(70f, () => ev.Player.Broadcast("<color=red>���� �� �������� � ����</color> <color=#9bff00>#fydne</color>,\n<color=#fdffbb>�� �� ������ �������� � 2 ���� ������ �����</color>", 15));
			if (string.IsNullOrEmpty(ev.Player.characterClassManager.UserId) || ev.Player.characterClassManager.IsHost || ev.Player.nicknameSync.MyNick == "Dedicated Server")
				return;

			if (!Stats.ContainsKey(ev.Player.characterClassManager.UserId))
				Stats.Add(ev.Player.characterClassManager.UserId, Methods.LoadStats(ev.Player.characterClassManager.UserId));
		}
		public void OnCheckEscape(ref CheckEscapeEvent ev)
		{
			List<Team> pList = Player.GetHubs().Select(x => Player.GetTeam(x)).ToList();
			string nick1 = ev.Player.GetNickname();
			MatchCollection matches1 = regexSmartSiteReplacer.Matches(nick1);
			if (matches1.Count == 0)
			{
				ev.Player.ClearBroadcasts();
				if (ev.Player == pList.Contains(Team.RSC))
				{
					Stats[ev.Player.characterClassManager.UserId].money += 10;
					ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 10 ����� �� �����</color>", 10, true);
				}
				if (ev.Player == pList.Contains(Team.CDP))
				{
					Stats[ev.Player.characterClassManager.UserId].money += 10;
					ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 10 ����� �� �����</color>", 10, true);
				}
			}
			string nick = ev.Player.GetNickname();
			MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
			if (matches.Count > 0)
			{
				ev.Player.ClearBroadcasts();
				if (ev.Player == pList.Contains(Team.RSC))
				{
					Stats[ev.Player.characterClassManager.UserId].money += 20;
					ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 20 ����� �� �����</color>", 10, true);
				}
				if (ev.Player == pList.Contains(Team.CDP))
				{
					Stats[ev.Player.characterClassManager.UserId].money += 20;
					ev.Player.GetComponent<Broadcast>().TargetAddElement(ev.Player.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 20 ����� �� �����</color>", 10, true);
				}
			}
		}
		public void OnPlayerDeath(ref PlayerDeathEvent ev)
		{
			List<Team> pList = Player.GetHubs().Select(x => Player.GetTeam(x)).ToList();
			if (ev.Player == null || string.IsNullOrEmpty(ev.Player.characterClassManager.UserId))
				return;


			if (ev.Killer == null || string.IsNullOrEmpty(ev.Killer.characterClassManager.UserId))
				return;

			if (Stats.ContainsKey(ev.Killer.characterClassManager.UserId))
			{
				if (ev.Killer != ev.Player)
				{
					string nick1 = ev.Killer.GetNickname();
					MatchCollection matches1 = regexSmartSiteReplacer.Matches(nick1);
					if (matches1.Count == 0)
					{
						//ev.Killer.ClearBroadcasts();
						if (ev.Killer.GetTeam() == Team.CHI)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 5;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 5 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.MTF)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 5;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 5 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.TUT)
						{
							Stats[ev.Killer.characterClassManager.UserId].money += 3;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 3 ������� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.RSC)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 7;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 7 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.CDP)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 7;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 7 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.SCP)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 3;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 3 ������� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
					}
				}
				string nick = ev.Killer.GetNickname();
				MatchCollection matches = regexSmartSiteReplacer.Matches(nick);
				if (matches.Count > 0)
				{
					if (ev.Killer != ev.Player)
					{
						//ev.Killer.ClearBroadcasts();
						if (ev.Killer.GetTeam() == Team.CHI)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 10;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 10 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.MTF)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 10;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 10 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.TUT)
						{
							Stats[ev.Killer.characterClassManager.UserId].money += 6;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 6 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.RSC)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 14;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 14 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.CDP)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 14;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 14 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
						else if (ev.Killer.GetTeam() == Team.SCP)
						{
							Stats[ev.Player.characterClassManager.UserId].money += 6;
							ev.Killer.GetComponent<Broadcast>().TargetAddElement(ev.Killer.scp079PlayerScript.connectionToClient, "<color=#fdffbb>�� �������� 6 ����� �� ��������</color> <color=red>" + ev.Player.GetNickname() + "</color>", 5, true);
						}
					}
				}
			}
		}
		public static ReferenceHub TryGet228()
		{
			return Scp228Data.GetScp228();
		}
		private static string Tryvodkalocationbc()
		{
			return Scp228Data.vodkalocationbc();
		}
		private static string Tryvodkalocation()
		{
			return Scp228Data.vodkalocation();
		}
		private static string Tryvodkalocationcolor()
		{
			return Scp228Data.vodkalocationcolor();
		}
		private static Pickup Trygetvodka()
		{
			return Scp228Data.Getvodka();
		}
		public void OnPlayerSpawn(PlayerSpawnEvent ev)
		{
			if (ev.Player.queryProcessor.PlayerId == TryGet228()?.queryProcessor.PlayerId)
			{
				Timing.CallDelayed(6f, () => TryGet228().Broadcast("<color=cyan>������ �� ��������:</color>\n<color=red>.���� 228</color>", 5));
			}
		}
		public void OnConsoleCommand(ConsoleCommandEvent ev)
		{
			string[] args = ev.Command.Split(' ');
			string cmd = ev.Command.ToLower();
			if (cmd.StartsWith("money") || cmd.StartsWith("����") || cmd.StartsWith("������"))
			{
				if (args.Length < 2)
				{
					ev.ReturnMessage = "\n------------------------ \n������:\n" + Stats[ev.Player.characterClassManager.UserId].money + "\n------------------------";
					ev.Color = "green";
					return;
				}
				ReferenceHub rh = Player.GetPlayer(string.Join(" ", args.Skip(1)));
				if (rh == null)
				{
					ev.ReturnMessage = "����� �� ������.";
					ev.Color = "red";
					return;
				}
				ev.ReturnMessage = $"\n------------------------ \n������ {rh.GetNickname()}:\n{Stats[rh.characterClassManager.UserId].money}\n------------------------";
				ev.Color = "green";
				return;
			}
			if (cmd.StartsWith("pay") || cmd.StartsWith("���") || cmd.StartsWith("���"))
			{
				if (args.Length < 3)
				{
					ev.ReturnMessage = $"������! ������: {args[0]} hmm 10";
					ev.Color = "red";
					return;
				}
				if (!int.TryParse(args[2], out int result))
				{
					ev.ReturnMessage = "������� ���������� ���-�� �����";
					ev.Color = "red";
					return;
				}
				ReferenceHub rh = Player.GetPlayer(string.Join(" ", args.Skip(1)));
				if (rh == null)
				{
					ev.ReturnMessage = "����� �� ������.";
					ev.Color = "red";
					return;
				}
				if (Stats[ev.Player.characterClassManager.UserId].money >= result)
				{
					Stats[ev.Player.characterClassManager.UserId].money -= result;
					Stats[rh.characterClassManager.UserId].money += result;
					ev.ReturnMessage = $"�� ������� �������� {result} ����� ������ {rh.GetNickname()}.";
					ev.Color = "green";
					rh.ClearBroadcasts();
					rh.Broadcast($"<size=20><color=lime>{rh.GetNickname()} ������� ��� {result} �����</color></size>", 5);
					return;
				}
				ev.ReturnMessage = $"������������ �������({Stats[ev.Player.characterClassManager.UserId].money}/{result}).";
				ev.Color = "red";
				return;
			}
			if (cmd.StartsWith("help") || cmd.StartsWith("����") || cmd.StartsWith("����"))
			{
				if (args.Length < 2)
				{
					ev.ReturnMessage = $"\n.help-������� ������\n\n.money-���������� ������\n��������: .money hmm\n��������: .money\n\n.pay-�������� ������ ������\n��������: .pay hmm\n\n.����-������� ������\n\n.����-���������� ������\n��������: .���� hmm\n��������: .����\n\n.���-�������� ������ ������\n��������: .��� hmm\n\n.����-������� ������\n\n.������-���������� ������\n��������: .������ hmm\n��������: .������\n\n.���-�������� ������ ������\n��������: .��� hmm";
					ev.Color = "cyan";
					return;
				}
				if (args[1] == "228")
				{
					ev.ReturnMessage = "\n.vodka-���������� ��������������� �����(������ ��� SCP 228 RU J)\n\n.vodka tp-�� � �����(������ ��� SCP 228 RU J)\n\n.�����-���������� ��������������� �����(������ ��� SCP 228 RU J)\n\n.����� ��-�� � �����(������ ��� SCP 228 RU J)";
					ev.Color = "cyan";
					return;
				}
			}
			if (cmd.StartsWith("vodka") || cmd.StartsWith("�����"))
			{
				if (ev.Player.queryProcessor.PlayerId != TryGet228()?.queryProcessor.PlayerId)
				{
					ev.ReturnMessage = "�� �� SCP 228 RU J";
					ev.Color = "red";
					return;
				}
				if (args.Length < 2)
				{
					if (ev.Player.queryProcessor.PlayerId == TryGet228()?.queryProcessor.PlayerId)
					{
						if (Stats[ev.Player.characterClassManager.UserId].money >= 20)
						{
							Stats[ev.Player.characterClassManager.UserId].money -= 20;
							ev.ReturnMessage = Tryvodkalocation();
							ev.Color = Tryvodkalocationcolor();
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(Tryvodkalocationbc(), 5);
							tptovodka = true;
							return;
						}
						ev.ReturnMessage = $"������������ �������({Stats[ev.Player.characterClassManager.UserId].money}/20).";
						ev.Color = "red";
						return;
					}
				}
				if (args[1] == "tp" || args[1] == "��")
				{
					if (ev.Player.queryProcessor.PlayerId == TryGet228()?.queryProcessor.PlayerId)
					{
						if (tptovodka)
						{
							if (Stats[ev.Player.characterClassManager.UserId].money >= 50)
							{
								Stats[ev.Player.characterClassManager.UserId].money -= 50;
								ev.ReturnMessage = "�� ��������������� � �����...";
								ev.Color = "cyan";
								ev.Player.ClearBroadcasts();
								ev.Player.Broadcast("<color=aqua>�� ��������������� � �����...</color>", 5);
								ev.Player.SetPosition(Trygetvodka().Rb.position + Vector3.up * 2);
								return;
							}
							ev.ReturnMessage = $"������������ �������({Stats[ev.Player.characterClassManager.UserId].money}/50).";
							ev.Color = "red";
						}
						return;
					}
				}
			}
		}
	}
}