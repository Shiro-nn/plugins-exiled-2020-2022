using System;
using System.Collections.Generic;
using EXILED;
using MEC;
using UnityEngine;
using EXILED.Extensions;
using Mirror;
namespace hideandseek
{
	public class Methods
	{
		private readonly Massacre plugin;
		public Methods(Massacre plugin) => this.plugin = plugin;

		public void EnableGamemode()
		{
			plugin.Ga = false;
			PlayerManager.localPlayer.GetComponent<Broadcast>().RpcAddElement("<color=red>�����</color> <color=aqua>������</color> <color=red>����� ������� � ��������� ������!</color>", 10, false);
		}
		public void DoSetup()
		{
			foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				if (door.DoorName.Contains("CHK") || door.DoorName.Contains("ARMORY") || door.DoorName.Contains("173") || door.DoorName.Contains("CHECKPOINT"))
				{
					door.Networklocked = true;
					door.NetworkisOpen = false;
				}
			foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
				item.Delete();
		}
		public void checkopen()
		{
			foreach (Door door in UnityEngine.Object.FindObjectsOfType<Door>())
				if (door.DoorName.Contains("CHECKPOINT"))
				{
					door.Networklocked = true;
					door.NetworkisOpen = true;
				}
			foreach (Pickup item in UnityEngine.Object.FindObjectsOfType<Pickup>())
				item.Delete();
		}
	}
}