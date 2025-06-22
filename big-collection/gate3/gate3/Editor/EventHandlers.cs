using EXILED;
using EXILED.Extensions;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
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
            Editor.PrepareMap();
        }
        public static void OnPlayerJoin(PlayerJoinEvent ev)
        {
            ev.Player.SetRole(RoleType.Tutorial);
            Thread.Sleep(500);
            ev.Player.SetPosition(new Vector3(-178, 990, -57));
            Editor.LoadMap(null, "gate3");
        }

        public static void RemoteAdminCommandEvent(ref RACommandEvent ev)
        {
            string[] args = ev.Command.Split(' ');

            ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" ? ReferenceHub.GetHub(PlayerManager.localPlayer) : Player.GetPlayer(ev.Sender.SenderId);

            switch (args[0].ToLower())
            {
                case "mapeditor":
                    ev.Allow = false;
                    if (!sender.CheckPermission("mapeditor"))
                    {
                        ev.Sender.RaReply("MapEditor#Отказано в доступе.", true, true, string.Empty);
                        return;
                    }
                    if (args.Length == 1)
                    {
                        ev.Sender.RaReply("MapEditor#Commands:", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR UNLOAD <NAME> - Unloads map.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR LOAD <NAME> - Load map.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR CREATE <NAME> - Create map.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR DELETE <NAME> - Delete map.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR CREATEOBJECT - Create object.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR DELETEOBJECT - Delete object.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR SETROOM <NAME> - Set object room.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR SELECTOBJECT - Select object.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR CLONEOBJECT - Clone object.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR CANCEL - Cancels map editing.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR EDIT <NAME> - Edits map.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR SAVE - Save map.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR SETPOS <X> <Y> <Z> - Set object position.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR SETROT <X> <Y> <Z> - Set object rotation.", true, true, string.Empty);
                        ev.Sender.RaReply("MapEditor# - MAPEDITOR SETSCALE <X> <Y> <Z> - Set object scale.", true, true, string.Empty);
                        return;
                    }
                    else if (args.Length > 1)
                    {
                        switch (args[1].ToLower())
                        {
                            case "unload":
                                Editor.UnloadMap(ev.Sender, args[2]);
                                break;
                            case "load":
                                Editor.LoadMap(ev.Sender, args[2]);
                                break;
                            case "create":
                                Editor.CreateMap(ev.Sender, args[2]);
                                break;
                            case "delete":
                                Editor.DeleteMap(ev.Sender, args[2]);
                                break;
                            case "setroom":
                                string fullMessage = ev.Command.Remove(0, 17);
                                Editor.SetRoomObject(ev.Sender, fullMessage);
                                break;
                            case "createobject":
                                Editor.CreateObject(ev.Sender);
                                break;
                            case "deleteobject":
                                Editor.DeleteObject(ev.Sender);
                                break;
                            case "selectobject":
                                Editor.SelectObject(ev.Sender);
                                break;
                            case "cancel":
                                Editor.StopMapEditing(ev.Sender);
                                break;
                            case "edit":
                                Editor.EditMap(ev.Sender, args[2]);
                                break;
                            case "save":
                                Editor.SaveMap(ev.Sender);
                                break;
                            case "setpos":
                                Editor.SetPositionObject(ev.Sender, float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4]));
                                break;
                            case "setrot":
                                Editor.SetRotationObject(ev.Sender, float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4]));
                                break;
                            case "setscale":
                                Editor.SetScaleObject(ev.Sender, float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4]));
                                break;
                            case "cloneobject":
                                Editor.CloneObject(ev.Sender);
                                break;
                        }
                        return;
                    }
                    break;
            }
        }

        public static void PlayerLeaveEvent(PlayerLeaveEvent ev)
        {
            //if (ev.Player.characterClassManager != null)
                //if (Editor.playerEditors.ContainsKey(ev.Player.characterClassManager.UserId))
                    //Editor.playerEditors.Remove(ev.Player.characterClassManager.UserId);
        }

        public static void RoundRestartEvent()
        {
            //Editor.playerEditors = new Dictionary<string, Editor.PlayerEditorStatus>();
        }
    }
}