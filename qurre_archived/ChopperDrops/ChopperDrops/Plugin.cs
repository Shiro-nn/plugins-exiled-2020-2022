using System;
namespace ChopperDrops
{
	public class ChopperDrops : Qurre.Plugin
	{
		public override string Name => "ChopperDrops";
		public override Version Version => new Version(1, 0, 1);
		public override Version NeededQurreVersion => new Version(1, 7, 0);
		public override void Enable()
		{
			if (!Configs.Enabled) return;
			EventHandlers = new EventHandlers();
			Qurre.Events.Round.Start += EventHandlers.RoundStart;
			Qurre.Events.Round.Waiting += EventHandlers.WaitingForPlayers;
		}
		public override void Disable()
		{
			if (!Configs.Enabled) return;
			Qurre.Events.Round.Start -= EventHandlers.RoundStart;
			Qurre.Events.Round.Waiting -= EventHandlers.WaitingForPlayers;
			EventHandlers = null;
		}
		private EventHandlers EventHandlers;
	}
	public static class Configs
	{
		public static bool Enabled = Qurre.Plugin.Config.GetBool("cd_enable", true);
		public static int DropDelay = Qurre.Plugin.Config.GetInt("cd_drop_delay", 600);
		public static string DropText = Qurre.Plugin.Config.GetString("cd_drop_text", "<color=lime>A supply drop is at the surface!</color>");
		public static ushort DropTextTime = Qurre.Plugin.Config.GetUShort("cd_drop_text_time", 10);
		public static string AllowedItems = Qurre.Plugin.Config.GetString("cd_allowed_items", "GunE11SR:1,Medkit:3,Adrenaline:2,Ammo762:2");
	}
}