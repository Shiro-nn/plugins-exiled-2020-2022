using System;
using EXILED;

namespace SCPList
{
	public class Plugin : EXILED.Plugin
	{
		//Instance variable for eventhandlers
		public EventHandlers EventHandlers;

		public bool displayBroadcast;
		
		public override void OnEnable()
		{
			if (!Config.GetBool("scplist_enable", true))
			{
				return;
			}

			displayBroadcast = Config.GetBool("scplist_broadcast", true);

			try
			{
				Log.Debug("Initializing event handlers..");
				//Set instance varible to a new instance, this should be nulled again in OnDisable
				EventHandlers = new EventHandlers(this);
				//Hook the events you will be using in the plugin. You should hook all events you will be using here, all events should be unhooked in OnDisabled
				Events.ConsoleCommandEvent += EventHandlers.OnConsoleCommand;
				Events.RoundStartEvent += EventHandlers.OnRoundStart;
				Log.Info($"SCPList loaded.");
			}
			catch (Exception e)
			{
				//This try catch is redundant, as EXILED will throw an error before this block can, but is here as an example of how to handle exceptions/errors
				Error($"There was an error loading the plugin: {e}");
			}
		}

		public override void OnDisable()
		{
			Events.ConsoleCommandEvent -= EventHandlers.OnConsoleCommand;
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;

			EventHandlers = null;
		}

		public override void OnReload()
		{
			//This is only fired when you use the EXILED reload command, the reload command will call OnDisable, OnReload, reload the plugin, then OnEnable in that order. There is no GAC bypass, so if you are updating a plugin, it must have a unique assembly name, and you need to remove the old version from the plugins folder
		}

		public override string getName { get; } = "SCPList";
	}
}