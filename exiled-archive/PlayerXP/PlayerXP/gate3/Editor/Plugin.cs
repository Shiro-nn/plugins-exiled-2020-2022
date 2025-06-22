using Mirror;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using static PlayerXP.gate3.editor.Editor;
using Exiled.API.Features;
using MEC;
using System;

namespace PlayerXP.gate3.editor
{
    public class gate3e
	{
		internal readonly Plugin plugin;
		public gate3e(Plugin plugin) => this.plugin = plugin;
		public static string pluginDir;
        public static MapEditorSettings settings;
		public void RegisterEvents()
		{
			Log.Info("Textures loaded.");
			Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayersEvent;

			Exiled.Events.Handlers.Server.SendingRemoteAdminCommand += EventHandlers.ra;
			Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;

			string appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			pluginDir = Path.Combine(appData, "Plugins", "Textures");
			if (!Directory.Exists(pluginDir))
				Directory.CreateDirectory(pluginDir);
			if (!Directory.Exists(Path.Combine(pluginDir, "maps")))
				Directory.CreateDirectory(Path.Combine(pluginDir, "maps"));
			if (!File.Exists(Path.Combine(pluginDir, "settings.yml")))
				File.WriteAllText(Path.Combine(pluginDir, "settings.yml"), JsonConvert.SerializeObject(new MapEditorSettings()
				{
					MapToLoad = new Dictionary<int, List<string>>()
					{
						{ 7777, new List<string>(){"gate3"} }
					}
				}));
			settings = JsonConvert.DeserializeObject<MapEditorSettings>(File.ReadAllText(Path.Combine(pluginDir, "settings.yml")));
			Timing.RunCoroutine(this.InfinityLoop());
		}
		internal void UnregisterEvents()
		{
			Log.Info("Textures unloaded.");
			Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.WaitingForPlayersEvent;

			Exiled.Events.Handlers.Server.SendingRemoteAdminCommand -= EventHandlers.ra;
			Exiled.Events.Handlers.Player.Joined -= EventHandlers.OnPlayerJoin;
			Timing.KillCoroutines();
		}
		public static GameObject GetWorkStationObject()
        {
            GameObject bench = UnityEngine.Object.Instantiate(NetworkManager.singleton.spawnPrefabs.Find(p => p.gameObject.name == "Work Station"));
            return bench;
		}
		public IEnumerator<float> InfinityLoop()
		{
			for (; ; )
			{
				yield return Timing.WaitForSeconds(1f);
				try
				{
					foreach (KeyValuePair<string, Editor.PlayerEditorStatus> ed in Editor.playerEditors)
					{
						ReferenceHub player = gate3.Extensions.GetPlayer(ed.Key);
						bool flag = player != null;
						if (flag)
						{
							bool flag2 = Editor.maps.ContainsKey(ed.Value.mapName);
							if (flag2)
							{
								string line = "";
								bool flag3 = ed.Value.selectedObject != null;
								if (flag3)
								{
									string str = ed.Value.isWorkstation ? string.Concat(new string[]
									{
										"<size=30>SelectedObject ID: ",
										ed.Value.selectedObject.name.Split(new char[]
										{
											'|'
										})[1],
										" MAP: ",
										ed.Value.selectedObject.name.Split(new char[]
										{
											'|'
										})[0],
										" ROOM: ",
										CommandEditor.GetObject(player).room,
										"</size>"
									}) : ("<size=30>Object Name: " + ed.Value.selectedObject.name + "</size>");
									string newLine = Environment.NewLine;
									string[] array = new string[19];
									array[0] = "<size=30>Position</size> <size=30>X: ";
									int num = 1;
									Vector3 vector = ed.Value.selectedObject.transform.position;
									array[num] = vector.x.ToString();
									array[2] = " Y: ";
									int num2 = 3;
									vector = ed.Value.selectedObject.transform.position;
									array[num2] = vector.y.ToString();
									array[4] = " Z: ";
									int num3 = 5;
									vector = ed.Value.selectedObject.transform.position;
									array[num3] = vector.z.ToString();
									array[6] = "</size> <color=red>|</color> <size=30>Rotation</size> <size=30>X: ";
									int num4 = 7;
									Quaternion rotation = ed.Value.selectedObject.transform.rotation;
									array[num4] = rotation.x.ToString();
									array[8] = " Y: ";
									int num5 = 9;
									rotation = ed.Value.selectedObject.transform.rotation;
									array[num5] = rotation.y.ToString();
									array[10] = " Z: ";
									int num6 = 11;
									rotation = ed.Value.selectedObject.transform.rotation;
									array[num6] = rotation.z.ToString();
									array[12] = "</size> <color=red>|</color> <size=30>Scale</size> <size=30>X: ";
									int num7 = 13;
									vector = ed.Value.selectedObject.transform.localScale;
									array[num7] = vector.x.ToString();
									array[14] = " Y: ";
									int num8 = 15;
									vector = ed.Value.selectedObject.transform.localScale;
									array[num8] = vector.y.ToString();
									array[16] = " Z: ";
									int num9 = 17;
									vector = ed.Value.selectedObject.transform.localScale;
									array[num9] = vector.z.ToString();
									array[18] = "</size>";
									line = str + newLine + string.Concat(array);
								}
								player.gameObject.GetComponent<Broadcast>().TargetClearElements(player.characterClassManager.connectionToClient);
								player.gameObject.GetComponent<Broadcast>().TargetAddElement(player.characterClassManager.connectionToClient, " <color=red>|</color> MapEditor <color=red>|</color> " + Environment.NewLine + line, 2, Broadcast.BroadcastFlags.Normal);
								line = null;
							}
							else
							{
								Editor.playerEditors.Remove(player.characterClassManager.UserId);
								player.gameObject.GetComponent<Broadcast>().TargetClearElements(player.characterClassManager.connectionToClient);
							}
						}
						player = null;
					}
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Log.Error(ex.ToString());
				}
			}
		}
	}
}