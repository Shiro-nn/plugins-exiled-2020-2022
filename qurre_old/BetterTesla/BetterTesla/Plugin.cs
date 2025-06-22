using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
namespace BetterTesla
{
    public class Plugin : Qurre.Plugin
    {
        public override string Developer => "fydne";
        public override string Name => "BetterTesla";
        public override Version Version => new();
        public override Version NeededQurreVersion => new(1, 14, 0);
        public override void Enable()
        {
            Cfg.Reload();
            Qurre.Events.Round.Waiting += Waiting;
            Qurre.Events.Player.TeslaTrigger += Trigger;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Waiting -= Waiting;
            Qurre.Events.Player.TeslaTrigger -= Trigger;
        }
        private void Waiting()
        {
            Cfg.Reload();
            List<Tesla> list = Map.Teslas;
            foreach (var tesla in list)
            {
                if (Cfg.Delete) tesla.Destroy();
                else tesla.Scale = Cfg.Scale;
            }
        }
        private void Trigger(TeslaTriggerEvent ev)
        {
            if (Cfg.IgnoreRoles.Contains((int)ev.Player.Role)) ev.Allowed = false;
        }
    }
    public static class Cfg
    {
        public static List<int> IgnoreRoles = new();
        public static Vector3 Scale = new();
        public static bool Delete = false;
        public static void Reload()
        {
            Plugin.Config.Reload();
            IgnoreRoles = GetList(Plugin.Config.GetString("BetterTesla_IgnoreRoles", "2,14", "Roles for which Tesla is disabled"));
            Scale = GetVector3(Plugin.Config.GetString("BetterTesla_Scale", "1,1,1", "Tesla scale. Ex: '1,1,1'/'1'"));
            Delete = Plugin.Config.GetBool("BetterTesla_Delete", false, "Remove Tesla?");
        }
        private static List<int> GetList(string data)
        {
            List<int> list = new();
            if (data.Contains(','))
            {
                var array = data.Split(',');
                foreach (string txt in array)
                {
                    try { list.Add(int.Parse(txt.Trim())); } catch { }
                }
            }
            else
            {
                try { list.Add(int.Parse(data.Trim())); } catch { }
            }
            return list;
        }
        private static Vector3 GetVector3(string data)
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
            return new(x, y, z);
        }
    }
}