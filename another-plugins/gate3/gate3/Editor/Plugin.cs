using Mirror;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using static MapEditor.Editor;
using Exiled.API.Interfaces;
using Exiled.API.Features;
using Exiled.API.Enums;
using MEC;
using System;

namespace MapEditor
{
	public class MainClass : Plugin<Config>
	{
		public static string pluginDir;
		public static MapEditorSettings settings;
		public override PluginPriority Priority { get; } = PluginPriority.Medium;
		public override void OnEnabled()
		{
			base.OnEnabled();
			Log.Info("Textures loaded.");
			Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.WaitingForPlayersEvent;

			Exiled.Events.Handlers.Player.Joined += EventHandlers.OnPlayerJoin;

			var f = new gate3.Plugin();
			f.RegisterEvents();
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
		}

		public static GameObject GetWorkStationObject()
		{
			GameObject bench = UnityEngine.Object.Instantiate(NetworkManager.singleton.spawnPrefabs.Find(p => p.gameObject.name == "Work Station"));
			return bench;
		}
	}
	public sealed class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
	}
}