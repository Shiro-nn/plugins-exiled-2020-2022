using Mirror;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using gate3;
using System;
namespace MapEditor
{
    public class EventHandlers
    {
        public static bool loaded = false;
        public static void WaitingForPlayersEvent()
        {
            if (!loaded)
            {
                loaded = true;
                foreach (KeyValuePair<int, List<string>> map in MainClass.settings.MapToLoad)
                {
                    if (map.Key == ServerConsole.Port)
                    {
                        foreach (string str in map.Value)
                        {
                            Editor.LoadMap(null, str);
                        }
                    }
                }
            }
        }
        public static void OnPlayerJoin(JoinedEventArgs ev)
        {
            ev.Player.SetRole(RoleType.Tutorial);
            ev.Player.ReferenceHub.SetPosition(new Vector3(-178, 990, -57));
            Editor.LoadMap(null, "gate3");
        }
    }
}