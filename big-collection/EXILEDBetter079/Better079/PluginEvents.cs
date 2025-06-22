using EXILED;
using EXILED.ApiObjects;
using EXILED.Extensions;
using Grenades;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Better079
{
    public class PluginEvents
    {
        private Better079Plugin plugin;

        public PluginEvents(Better079Plugin better079Plugin)
        {
            this.plugin = better079Plugin;
        }

        internal void RoundStart()
        {
        }

        internal void PlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.GetRole() == RoleType.Scp079)
            {
                ev.Player.Broadcast(6, "<color=#00ff00>Напишите \".079 help\" в консоли для получения команд</color>", true);
            }
        }

        internal List<Camera079> GetSCPCameras()
        {
            var list = GameObject.FindObjectsOfType<Camera079>();
            List<Camera079> cams = new List<Camera079>();
            foreach (var ply in PlayerManager.players)
            {
                if (ply.GetPlayer().GetTeam() == Team.SCP && ply.GetPlayer().GetRole() != RoleType.Scp079)
                {
                    foreach (var cam in list)
                    {
                        if (Vector3.Distance(cam.transform.position, ply.transform.position) <= plugin.A1MinDist)
                        {
                            cams.Add(cam);
                        }
                    }
                }
            }
            return cams;
        }

        // https://github.com/galaxy119/EXILED/blob/master/EXILED_Main/Extensions/Player.cs 
        internal Room SCP079Room(ReferenceHub player)
        {
            Vector3 playerPos = player.scp079PlayerScript.currentCamera.transform.position;
            Vector3 end = playerPos - new Vector3(0f, 10f, 0f);
            bool flag = Physics.Linecast(playerPos, end, out RaycastHit raycastHit, -84058629);

            if (!flag || raycastHit.transform == null)
                return null;

            Transform transform = raycastHit.transform;

            while (transform.parent != null && transform.parent.parent != null)
                transform = transform.parent;

            foreach (Room room in Map.Rooms)
                if (room.Position == transform.position)
                    return room;

            return new Room
            {
                Name = transform.name,
                Position = transform.position,
                Transform = transform
            };
        }

        internal void ConsoleCmd(ConsoleCommandEvent ev)
        {
            if (ev.Player.GetRole() == RoleType.Scp079)
            {
                string[] args = ev.Command.Split(' ');
                if (args[0].Equals("079"))
                {
                    if (args.Length == 2)
                    {
                        if (args[1].ToLower().Equals("help") || args[1].ToLower().Equals("commands") || args[1].ToLower().Equals("?"))
                        {
                            ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Способности/Команды:\n" +
                                "\".079 a1\" - Телепортируйся к SCP -" + plugin.A1Power + " энергии, нужен " + (plugin.A1Tier + 1) + " уровень\n" +
                                "\".079 a2\" - Убить людей в комнате - " + plugin.A2Power + " энергии, нужен " + (plugin.A2Tier + 1) + " уровень\n" +
                                "\".079 a3\" - Отключает свет в комплексе - " + plugin.A3Power + " энергии, нужен " + (plugin.A3Tier + 1) + " уровень\n" +
                                "\".079 a4\" - Ослепить камерой - " + plugin.A4Power + " энергии, нужен " + (plugin.A4Tier + 1) + " уровень\n", "red");
                            return;
                        }

                        if (args[1].ToLower().Equals("a1"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < plugin.A1Tier)
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Необходим " + (plugin.A1Tier + 1) + " уровень", "red");
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana >= plugin.A1Power)
                            {
                                ev.Player.scp079PlayerScript.NetworkcurMana -= plugin.A1Power;
                            }
                            else
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Недостаточно энергии.", "red");
                                return;
                            }
                            var cams = GetSCPCameras();
                            if (cams.Count > 0)
                            {
                                Camera079 cam = cams[UnityEngine.Random.Range(0, cams.Count)];
                                ev.Player.scp079PlayerScript.CmdSwitchCamera(cam.cameraId, true);
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Телепортация...", "red");
                                return;
                            }
                            else
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "SCP не найдены.", "red");
                                ev.Player.scp079PlayerScript.NetworkcurMana += plugin.A1Power;
                                return;
                            }
                        }

                        if (args[1].ToLower().Equals("a2"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < plugin.A2Tier)
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Необходим " + (plugin.A2Tier + 1) + " уровень", "red");
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana >= plugin.A2Power)
                            {
                                ev.Player.scp079PlayerScript.NetworkcurMana -= plugin.A2Power;
                            }
                            else
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Недостаточно энергии.", "red");
                                return;
                            }
                            Room room = SCP079Room(ev.Player);
                            if (room == null)
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "MEMETIC KILL AGENT здесь не работает!", "red");
                                return;
                            }
                            foreach (var item in plugin.A2BlacklistRooms)
                            {
                                if (room.Name.ToLower().Contains(item.ToLower()))
                                {
                                    ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "MEMETIC KILL AGENT здесь не работает!", "red");
                                    return;
                                }
                            }
                            Timing.RunCoroutine(GasRoom(room, ev.Player));
                            ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Активация...", "red");
                            return;
                        }

                        if (args[1].ToLower().Equals("a3"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < plugin.A3Tier)
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Необходим " + (plugin.A3Tier + 1) + " уровень", "red");
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana >= plugin.A3Power)
                            {
                                ev.Player.scp079PlayerScript.NetworkcurMana -= plugin.A3Power;
                            }
                            else
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Недостаточно энергии.", "red");
                                return;
                            }
                            Generator079.generators[0].RpcCustomOverchargeForOurBeautifulModCreators(plugin.A3Timer, false);
                            ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Выключение...", "red");
                            return;
                        }

                        if (args[1].ToLower().Equals("a4"))
                        {
                            if (ev.Player.scp079PlayerScript.NetworkcurLvl < plugin.A4Tier)
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Необходим " + (plugin.A4Tier + 1) + " уровень", "red");
                                return;
                            }
                            if (ev.Player.scp079PlayerScript.NetworkcurMana >= plugin.A4Power)
                            {
                                ev.Player.scp079PlayerScript.NetworkcurMana -= plugin.A4Power;
                            }
                            else
                            {
                                ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Недостаточно энергии.", "red");
                                return;
                            }
                            var pos = ev.Player.scp079PlayerScript.currentCamera.transform.position;
                            GrenadeManager gm = ev.Player.GetComponent<GrenadeManager>();
                            GrenadeSettings settings = gm.availableGrenades.FirstOrDefault(g => g.inventoryID == ItemType.GrenadeFlash);
                            FlashGrenade flash = GameObject.Instantiate(settings.grenadeInstance).GetComponent<FlashGrenade>();
                            flash.fuseDuration = 0.5f;
                            flash.InitData(gm, Vector3.zero, Vector3.zero, 1f);
                            flash.transform.position = pos;
                            NetworkServer.Spawn(flash.gameObject);
                            ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Ослепление...", "red");
                            return;
                        }
                        ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Неправильно. Напишите \".079 help\" для помощи", "red");
                        return;
                    }
                    ev.Player.characterClassManager.TargetConsolePrint(ev.Player.scp079PlayerScript.connectionToClient, "Неправильно. Напишите \".079 help\" для помощи", "red");
                    return;
                }
            }
        }

        private IEnumerator<float> GasRoom(Room room, ReferenceHub scp)
        {
            string str = ".g4 ";
            for (int i = plugin.A2Timer; i > 0f; i--)
            {
                str += ". .g4 ";
            }
            PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(str, false, false);
            List<Door> doors = Map.Doors.FindAll((d) => Vector3.Distance(d.transform.position, room.Position) <= 20f);
            foreach (var item in doors)
            {
                item.NetworkisOpen = true;
                item.Networklocked = true;
            }
            for (int i = plugin.A2Timer; i > 0f; i--)
            {
                foreach (var ply in PlayerManager.players)
                {
                    var player = ply.GetPlayer();
                    if (player.GetTeam() != Team.SCP && player.GetCurrentRoom() != null && player.GetCurrentRoom().Transform == room.Transform)
                    {
                        player.ClearBroadcasts();
                        player.Broadcast(1, "<color=#ff0000>MEMETIC KILL AGENT активируется через " + i + " секунд.</color>", true);
                        PlayerManager.localPlayer.GetComponent<MTFRespawn>().RpcPlayCustomAnnouncement(".g3", false, false);
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
            foreach (var item in doors)
            {
                item.NetworkisOpen = false;
                item.Networklocked = true;
            }
            foreach (var ply in PlayerManager.players)
            {
                var player = ply.GetPlayer();
                if (player.GetTeam() != Team.SCP && player.GetCurrentRoom() != null && player.GetCurrentRoom().Transform == room.Transform)
                {
                    player.Broadcast(5, "<color=#ff0000>MEMETIC KILL AGENT АКТИВИРОВАН.</color>", true);
                }
            }
            for (int i = 0; i < plugin.A2TimerGas * 2; i++)
            {
                foreach (var ply in PlayerManager.players)
                {
                    var player = ply.GetPlayer();
                    if (player.GetTeam() != Team.SCP && player.GetRole() != RoleType.Spectator && player.GetCurrentRoom() != null && player.GetCurrentRoom().Transform == room.Transform)
                    {
                        player.playerStats.HurtPlayer(new PlayerStats.HitInfo(10f, "WORLD", DamageTypes.Decont, 0), player.gameObject);
                        if (player.GetRole() == RoleType.Spectator)
                        {
                            scp.scp079PlayerScript.AddExperience(plugin.A2Exp);
                        }
                    }
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
            foreach (var item in doors)
            {
                item.Networklocked = false;
            }
        }
    }
}