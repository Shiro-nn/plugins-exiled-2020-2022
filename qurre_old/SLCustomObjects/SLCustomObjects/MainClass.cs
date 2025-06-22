using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qurre;
using Qurre.API.Events;
using UnityEngine;
namespace SLCustomObjects
{
	public class MainClass : Plugin
	{
		public override string Developer { get; } = "Killers0992";
		public override string Name { get; } = "SLCustomObjects";
		public override Version NeededQurreVersion { get; } = new Version(1, 3, 0);
		public override void Enable()
		{
			this.schem = new Schematic();
			this.pluginDir = Path.Combine(PluginManager.PluginsDirectory, "SLCustomObjects");
			bool flag = !Directory.Exists(this.pluginDir);
			if (flag)
			{
				Directory.CreateDirectory(this.pluginDir);
			}
			bool flag2 = !Directory.Exists(Path.Combine(this.pluginDir, "schematics"));
			if (flag2)
			{
				Directory.CreateDirectory(Path.Combine(this.pluginDir, "schematics"));
			}
			Qurre.Events.Server.SendingRA += this.Server_SendingRemoteAdminCommand;
			Qurre.Events.Player.PickupItem += this.schem.PickupItem;
			Qurre.Events.Round.WaitingForPlayers += this.Server_WaitingForPlayers;
		}
        public override void Disable()
		{
			Qurre.Events.Server.SendingRA -= this.Server_SendingRemoteAdminCommand;
			Qurre.Events.Player.PickupItem -= this.schem.PickupItem;
			Qurre.Events.Round.WaitingForPlayers -= this.Server_WaitingForPlayers;
		}
		private void Server_WaitingForPlayers()
		{
			var AL = Config.GetString("schematic_auto_load", "");
			if(AL.Split(';').Length == 0) LoadAuto(AL);
			foreach (var set in AL.Split(';')) LoadAuto(set);
		}
		private void LoadAuto(string set)
		{
			var array = set.Split(':');
			var array2 = array[2].Split('$');
			var array3 = array[3].Split('$');
			if (Schematic.LoadSchematic(array[0], array[1], new Vector3(Convert.ToInt32(array2[0]), Convert.ToInt32(array2[1]), Convert.ToInt32(array2[2])),
				new Vector3(Convert.ToInt32(array3[0]), Convert.ToInt32(array3[1]), Convert.ToInt32(array3[2])), Vector3.zero))
			{
				Log.Info("Schematic loaded... " + Path.GetFileNameWithoutExtension(array[1]));
			}
			else
			{
				Log.Info("Schematic failed to load... " + Path.GetFileNameWithoutExtension(array[1]));
			}
		}
		private bool CheckPerms(string __)
		{
			string _ = Config.GetString("schematic_access_userid", "SERVER CONSOLE");
			string[] str = _.Split(',');
			List<string> strl = new List<string>();
			foreach (string st in str) strl.Add(st.Trim());
			return strl.Contains(__);
		}
		private void Server_SendingRemoteAdminCommand(SendingRAEvent ev)
		{
			switch (ev.Name.ToUpper())
			{
				case "SCHEMATIC":
					ev.Allowed = false;
					if (!CheckPerms(ev.CommandSender.SenderId))
					{
						ev.Player.RAMessage("No Permission", true, "SCHEMATIC");
						return;
					}
					if (ev.Args.Count() != 0)
					{
						switch (ev.Args[0].ToUpper())
						{
							case "LIST":
								string outstr = " Schematics: \n";
								foreach (var file in Directory.GetFiles(Path.Combine(pluginDir, "schematics")))
								{
									outstr += Path.GetFileNameWithoutExtension(file) + "\n";
								}
								ev.Player.RAMessage(outstr, true, "SCHEMATIC");
								break;
							case "LOAD":
								if (File.Exists(Path.Combine(pluginDir, "schematics", "schematic-" + ev.Args[2] + ".json")))
								{
									if (Schematic.LoadSchematic(ev.Args[1], Path.Combine(pluginDir, "schematics", "schematic-" + ev.Args[2] + ".json"), ev.Player.Position, Vector3.zero, Vector3.zero))
									{
										ev.Player.RAMessage($"Schematic {ev.Args[1]} loaded.", true, "SCHEMATIC");
										return;
									}
									else
									{
										ev.Player.RAMessage($"Failed loading schematic {ev.Args[1]}.", true, "SCHEMATIC");
									}
								}
								else
								{
									ev.Player.RAMessage("File not found", true, "SCHEMATIC");
								}
								break;
							case "UNLOAD":
								if (Schematic.UnloadSchematic("Schematic_" + ev.Args[1]))
								{
									ev.Player.RAMessage($"Schematic {ev.Args[1]} unloaded.", true, "SCHEMATIC");
									return;
								}
								ev.Player.RAMessage($"Schematic {ev.Args[1]} is not loaded.", true, "SCHEMATIC");
								break;
							case "BRINGSCHEMATIC":
								Schematic.BringSchematic("Schematic_" + ev.Args[1], ev.Player);
								break;
							case "FORCELOAD":
								Server_WaitingForPlayers();
								break;
						}
					}
					else
					{
						ev.Player.RAMessage(string.Concat("Commands: ",
							"LIST",
							"LOAD <name> <schematicName>",
							"UNLOAD <name>",
							"BRINGSCHEMATIC",
							"FORCELOAD"), true, "SCHEMATIC");
					}
					break;
			}
		}
		public string pluginDir;
		public Schematic schem;
	}
}