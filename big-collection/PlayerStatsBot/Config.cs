// Decompiled with JetBrains decompiler
// Type: PlayerStatsBot.Config
// Assembly: PlayerStatsBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 24C98175-4615-4B8E-8ED3-8F72265ADCF9
// Assembly location: I:\PlayerStats\PlayerStatsBot.exe

namespace PlayerStatsBot
{
  public class Config
  {
    public static readonly Config Default = new Config()
    {
      BotPrefix = "*",
      BotToken = "",
      SyncFile = "DiscordSteamSyncs.yml",
      StatsFile = "C:/Users/User/AppData/Roaming/Plugins/PlayerStats",
      StaffID = 0
    };

    public string BotPrefix { get; set; }

    public string BotToken { get; set; }

    public string SyncFile { get; set; }

    public string StatsFile { get; set; }

    public ulong StaffID { get; set; }
  }
}
