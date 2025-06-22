// Decompiled with JetBrains decompiler
// Type: PlayerStatsBot.Program
// Assembly: PlayerStatsBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 24C98175-4615-4B8E-8ED3-8F72265ADCF9
// Assembly location: I:\PlayerStats\PlayerStatsBot.exe

using Discord;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStatsBot
{
  public class Program
  {
    private readonly Bot bot;
    private const string kCfgFile = "PlayerStatsBot.json";
    private Config config;

    public Config Config
    {
      get
      {
        return this.config ?? (this.config = Program.GetConfig());
      }
    }

    public static void Main()
    {
      Program program = new Program();
    }

    private Program()
    {
      this.bot = new Bot(this);
    }

    public static Task Log(LogMessage msg)
    {
      Console.WriteLine(msg.ToString((StringBuilder) null, true, true, DateTimeKind.Local, new int?(11)));
      return Task.CompletedTask;
    }

    private static Config GetConfig()
    {
      if (File.Exists("PlayerStatsBot.json"))
        return JsonConvert.DeserializeObject<Config>(File.ReadAllText("PlayerStatsBot.json"));
      File.WriteAllText("PlayerStatsBot.json", JsonConvert.SerializeObject((object) Config.Default, Formatting.Indented));
      return Config.Default;
    }
  }
}
