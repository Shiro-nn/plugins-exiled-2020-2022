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
        internal static void ra(SendingRemoteAdminCommandEventArgs ev)
        {
            if (ev.Name == "mapeditor")
            {
                if (ev.Sender.UserId == "-@steam")
                {
                    ev.IsAllowed = false;
                    if (ev.Arguments.Count == 0)
                    {
                        ev.ReplyMessage = "Commands:" +
                            "mapeditor unload <NAME> - Unloads map.\n" +
                            "mapeditor load <NAME> - Load map.\n" +
                            "mapeditor create <NAME> - Create map.\n" +
                            "mapeditor DELETE <NAME> - Delete map.\n" +
                            "mapeditor createobject - Create object.\n" +
                            "mapeditor deleteobject - Delete object.\n" +
                            "mapeditor SETROOM <NAME> - Set object room.\n" +
                            "mapeditor SELECTOBJECT - Select object.\n" +
                            "mapeditor cloneobject - Clone object.\n" +
                            "mapeditor cancel - Cancels map editing.\n" +
                            "mapeditor edit <NAME> - Edits map.\n" +
                            "mapeditor save - Save map.\n" +
                            "mapeditor setpos <X> <Y> <Z> - Set object position.\n" +
                            "mapeditor setrot <X> <Y> <Z> - Set object rotation.\n" +
                            "mapeditor setscale <X> <Y> <Z> - Set object scale.\n";
                    }
                    if (ev.Arguments[0] == "createobject")
                    {
                        CommandEditor.CreateObject(ev.CommandSender);
                    }
                    else if (ev.Arguments[0] == "cancel")
                    {
                        CommandEditor.StopMapEditing(ev.CommandSender);
                    }
                    else if (ev.Arguments[0] == "setpos")
                    {
                        CommandEditor.SetPositionObject(ev.CommandSender, float.Parse(ev.Arguments[1]), float.Parse(ev.Arguments[2]), float.Parse(ev.Arguments[3]));
                    }
                    else if (ev.Arguments[0] == "create")
                    {
                        CommandEditor.CreateMap(ev.CommandSender, ev.Arguments[1]);
                    }
                    else if (ev.Arguments[0] == "setrot")
                    {
                        CommandEditor.SetRotationObject(ev.CommandSender, float.Parse(ev.Arguments[1]), float.Parse(ev.Arguments[2]), float.Parse(ev.Arguments[3]));
                    }
                    else if (ev.Arguments[0] == "cloneobject")
                    {
                        CommandEditor.CloneObject(ev.CommandSender);
                    }
                    else if (ev.Arguments[0] == "unload")
                    {
                        CommandEditor.UnloadMap(ev.CommandSender, ev.Arguments[1]);
                    }
                    else if (ev.Arguments[0] == "edit")
                    {
                        CommandEditor.EditMap(ev.CommandSender, ev.Arguments[1]);
                    }
                    else if (ev.Arguments[0] == "deleteobject")
                    {
                        CommandEditor.DeleteObject(ev.CommandSender);
                    }
                    else if (ev.Arguments[0] == "save")
                    {
                        CommandEditor.SaveMap(ev.CommandSender);
                    }
                    else if (ev.Arguments[0] == "load")
                    {
                        CommandEditor.LoadMap(ev.CommandSender, ev.Arguments[1]);
                    }
                    else if (ev.Arguments[0] == "setscale")
                    {
                        CommandEditor.SetScaleObject(ev.CommandSender, float.Parse(ev.Arguments[1]), float.Parse(ev.Arguments[2]), float.Parse(ev.Arguments[3]));
                    }
                }
            }
        }
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
            //Editor.PrepareMap();
        }
        public static void OnPlayerJoin(JoinedEventArgs ev)
        {
            ev.Player.SetRole(RoleType.Tutorial);
            ev.Player.ReferenceHub.SetPosition(new Vector3(152, 1019.5f, -17));
            Editor.LoadMap(null, "gate3");
        }
    }
}