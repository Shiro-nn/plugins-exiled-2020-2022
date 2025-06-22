using MEC;
using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
namespace WaitAndChill
{
    public class Plugin : Qurre.Plugin
    {
        public override string Developer => "fydne";
        public override string Name => "WaitAndChill";
        public override Version Version => new Version(2, 0, 4);
        public override Version NeededQurreVersion => new Version(1, 10, 0);
        public override void Enable()
        {
            Cfg.Reload();
            Qurre.Events.Round.Waiting += Waiting;
            Qurre.Events.Round.Start += Kill;
            Qurre.Events.Player.Join += Join;
            Qurre.Events.Player.Damage += Damage;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Waiting -= Waiting;
            Qurre.Events.Round.Start -= Kill;
            Qurre.Events.Player.Join -= Join;
            Qurre.Events.Player.Damage -= Damage;
        }
        private void Waiting() => Timing.RunCoroutine(Сycle(), "WaitAndChill_ShowHint");
        private void Kill() => Timing.KillCoroutines("WaitAndChill_ShowHint");
        private IEnumerator<float> Сycle()
        {
            Cfg.Reload();
            try { GameObject.Find("StartRound").transform.localScale = Vector3.zero; } catch { }
            while (!Round.Started)
            {
                try
                {
                    string onemsg = Cfg.Hint1.Replace("%seconds%", $"{GameCore.RoundStart.singleton.NetworkTimer}");
                    if (GameCore.RoundStart.singleton.NetworkTimer < 0) onemsg = "";
                    string msg = Cfg.Hint2.Replace("%players%", $"{Player.List.Count()}");
                    Map.ShowHint(onemsg + "\n" + msg, 1);
                }
                catch { }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        private void Join(JoinEvent ev)
        {
            Timing.CallDelayed(1f, () =>
            {
                if (!Round.Started && Round.ElapsedTime.Minutes == 0)
                {
                    ev.Player.Role = Cfg.Role;
                    Timing.CallDelayed(0.5f, () =>
                    {
                        ev.Player.Hp = ev.Player.ClassManager.Classes.SafeGet(Cfg.Role).maxHP;
                        Timing.CallDelayed(0.3f, () => ev.Player.Position = Cfg.Position);
                    });
                }
            });

        }
        private void Damage(DamageEvent ev)
        {
            if (Round.Started || Round.Ended) return;
            ev.Amount = 0;
            ev.Allowed = false;
        }
    }
    internal static class Cfg
    {
        internal static Vector3 Position = new Vector3();
        internal static RoleType Role = RoleType.Tutorial;
        internal static string Hint1 = "";
        internal static string Hint2 = "";
        internal static void Reload()
        {
            Qurre.Plugin.Config.Reload();
            Position = GetVector3(Qurre.Plugin.Config.GetString("WaitAndChill_Position", "1,1,1", "The position to teleport to while waiting for players. Ex: '1,1,1'/'1'"));
            Hint1 = Qurre.Plugin.Config.GetString("WaitAndChill_Hint_ToStart", "<size=50><color=yellow><b>The round will start in %seconds% seconds</b></color></size>");
            Hint2 = Qurre.Plugin.Config.GetString("WaitAndChill_Hint_Players", "<size=40><i>%players% players</i></size>");
            int DoRole = Qurre.Plugin.Config.GetInt("WaitAndChill_RoleType", 14);
            if (DoRole < 0 || DoRole > 17) Role = RoleType.Tutorial;
            else Role = (RoleType)DoRole;
            Vector3 GetVector3(string data)
            {
                float x = 1, y = 1, z = 1;
                if (data.Contains(','))
                {
                    var array = data.Split(',');
                    for (int i = 0; i < array.Count(); i++)
                    {
                        float _ = 1;
                        try { _ = float.Parse(array[i].Trim(), CultureInfo.InvariantCulture); } catch { }
                        if (i == 0) x = _;
                        else if (i == 1) y = _;
                        else if (i == 2) z = _;
                    }
                }
                else
                {
                    float _ = 1;
                    try { _ = float.Parse(data.Trim(), CultureInfo.InvariantCulture); } catch { }
                    x = _;
                    y = _;
                    z = _;
                }
                return new Vector3(x, y, z);
            }
        }
    }
}