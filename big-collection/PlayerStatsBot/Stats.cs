// Decompiled with JetBrains decompiler
// Type: PlayerStatsBot.Stats
// Assembly: PlayerStatsBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 24C98175-4615-4B8E-8ED3-8F72265ADCF9
// Assembly location: I:\PlayerStats\PlayerStatsBot.exe

using System;

namespace PlayerStatsBot
{
  [Serializable]
  public class Stats
  {
    public string UserId;
    public float SecondsPlayed;
    public int Kills;
    public int ScpKills;
    public int Deaths;
    public int Suicides;
    public int Scp207Uses;
    public int Scp018Throws;
    public double Krd;
    public string LastKiller;
    public string LastVictim;
    public int Escapes;
    public int RoundsPlayed;
  }
}
