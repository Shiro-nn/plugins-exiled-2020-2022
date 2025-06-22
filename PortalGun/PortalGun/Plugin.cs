using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smod2;
using Smod2.Commands;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using Smod2.Attributes;
using UnityEngine;
using Mirror;
using System.Reflection;
using MEC;

namespace PortalGun
{
    [PluginDetails(
        author = "moseechev",
        name = "Portal Gun",
        description = "d",
        id = "mos.PortalGun",
        version = "0.3",
        SmodMajor = 3,
        SmodMinor = 2,
        SmodRevision = 1
    )]
    public class Plugin : Smod2.Plugin
    {
        public override void OnDisable()
        {

        }

        public override void OnEnable()
        {

        }

        public override void Register()
        {
            this.AddEventHandlers(new EventHandler());
        }
    }

    public class EventHandler : IEventHandler, IEventHandlerShoot, IEventHandlerPlayerPickupItem, IEventHandlerRoundStart
    {
        Vector portal1;
        Vector portal2;
        bool IsBlue = true;
        Smod2.API.Item portalgunitem;
        public static List<Smod2.API.Item> Portal1 = new List<Smod2.API.Item>();
        public static List<Smod2.API.Item> Portal2 = new List<Smod2.API.Item>();

        bool used = false;
        public static List<Smod2.API.Player> teleported = new List<Smod2.API.Player>();
        public void OnShoot(PlayerShootEvent ev)
        {
            ev.Player.PersonalClearBroadcasts();
            ev.Player.PersonalBroadcast(10, ev.Player.GetRotation().ToString(), false) ;
            if (ev.Weapon.WeaponType == WeaponType.COM15)
            {
                if (IsBlue)
                {
                    portal1 = new Vector(ev.TargetPosition.x, ev.TargetPosition.y + 1, ev.TargetPosition.z);
                    PluginManager.Manager.Server.Map.SpawnItem(Smod2.API.ItemType.MTF_COMMANDER_KEYCARD, new Vector(ev.TargetPosition.x, ev.TargetPosition.y + 1, ev.TargetPosition.z), new Vector(0, 0, 810));
                    foreach (Smod2.API.Item item in PluginManager.Manager.Server.Map.GetItems(Smod2.API.ItemType.MTF_COMMANDER_KEYCARD, false))
                    {
                        if (item.GetPosition().x < ev.TargetPosition.x + 5 && item.GetPosition().x > ev.TargetPosition.x - 5 /**/ && item.GetPosition().y < ev.TargetPosition.y + 5 && item.GetPosition().y > ev.TargetPosition.y - 5 /**/ && item.GetPosition().z < ev.TargetPosition.z + 5 && item.GetPosition().z > ev.TargetPosition.z - 5)
                        {
                            foreach (Smod2.API.Item item1 in Portal1)
                            {
                                item1.Remove();
                            }
                            Portal1.Clear();
                            item.SetKinematic(true);
                            Portal1.Add(item);
                            Timing.CallDelayed(0.01f, () => IsBlue = false);
                            foreach (Smod2.API.Item item1 in Portal1)
                            {
                                if (item1.GetComponent() is Pickup pickup)
                                {
                                    var gameObject = pickup.gameObject;
                                    Timing.CallDelayed(0.3f, () => Extension.Resize(gameObject, 15, 15, 15));
                                }
                            }
                        }
                    }
                }

                if (!IsBlue)
                {
                    portal2 = new Vector(ev.TargetPosition.x, ev.TargetPosition.y + 1, ev.TargetPosition.z);
                    PluginManager.Manager.Server.Map.SpawnItem(Smod2.API.ItemType.KEYCARD_SCIENTIST_MAJOR, new Vector(ev.TargetPosition.x, ev.TargetPosition.y + 1, ev.TargetPosition.z), new Vector(100, 0, 810));
                    foreach (Smod2.API.Item item in PluginManager.Manager.Server.Map.GetItems(Smod2.API.ItemType.KEYCARD_SCIENTIST_MAJOR, false))
                    {
                        if (item.GetPosition().x < ev.TargetPosition.x + 5 && item.GetPosition().x > ev.TargetPosition.x - 5 /**/ && item.GetPosition().y < ev.TargetPosition.y + 5 && item.GetPosition().y > ev.TargetPosition.y - 5 /**/ && item.GetPosition().z < ev.TargetPosition.z + 5 && item.GetPosition().z > ev.TargetPosition.z - 5)
                        {
                            foreach (Smod2.API.Item item1 in Portal2)
                            {
                                Timing.CallDelayed(0.1f, () => item1.Remove());
                            }
                            Portal2.Clear();
                            item.SetKinematic(true);
                            Portal2.Add(item);
                            Timing.CallDelayed(0.01f, () => IsBlue = true);
                            if (!used)
                            {
                                CheckPortals();
                                used = true;
                            }
                            foreach (Smod2.API.Item item1 in Portal2)
                            {
                                if (item1.GetComponent() is Pickup pickup)
                                {
                                    var gameObject = pickup.gameObject;
                                    Timing.CallDelayed(0.3f, () => Extension.Resize(gameObject, 15, 15, 15));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckPortals()
        {
            foreach (Smod2.API.Player player in PluginManager.Manager.Server.GetPlayers())
            {
                if (player.GetPosition().x < portal1.x + 0.5f && player.GetPosition().x > portal1.x - 3 /**/ && player.GetPosition().y < portal1.y + 2 && player.GetPosition().y > portal1.y - 2 /**/ && player.GetPosition().z < portal1.z + 3 && player.GetPosition().z > portal1.z - 0.5f)
                {
                    if (portal2 != null)
                    {
                        player.Teleport(portal2);
                    }
                }

                if (player.GetPosition().x < portal2.x + 1f && player.GetPosition().x > portal2.x - 1f /**/ && player.GetPosition().y < portal2.y + 3 && player.GetPosition().y > portal2.y - 3 /**/ && player.GetPosition().z < portal2.z + 1f && player.GetPosition().z > portal2.z - 1f)
                {
                    if (portal1 != null)
                    {
                        player.Teleport(portal1);
                    }
                }
            }
            Timing.CallDelayed(0.6f, () => CheckPortals());
        }
        bool takenUp;
        public void OnPlayerPickupItem(PlayerPickupItemEvent ev)
        {
            if (Portal1.Contains(ev.Item))
            {
                ev.Allow = false;
            }
            if (Portal2.Contains(ev.Item))
            {
                ev.Allow = false;
            }
            if (ev.Item.ItemType == Smod2.API.ItemType.GUN_COM15)
            {
                ev.Player.PersonalClearBroadcasts();
                ev.Player.PersonalBroadcast(10, "<color=aqua><b>Вы подобрали портал ган :)</b></color>", false);
            }
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            void ChangeToUSP()
            {
                foreach (Smod2.API.Door door in PluginManager.Manager.Server.Map.GetDoors())
                {
                    foreach (Smod2.API.Player player in PluginManager.Manager.Server.GetPlayers())
                    {
                        Vector vector = player.GetPosition();
                        Timing.CallDelayed(0.7f, () => player.Teleport(vector));
                    }
                    door.Locked = true;
                    door.Open = true;
                    Timing.CallDelayed(0.3f, () => door.Locked = false);
                    Timing.CallDelayed(0.3f, () => door.Open = false);
                    foreach (Smod2.API.Item item in PluginManager.Manager.Server.Map.GetItems(Smod2.API.ItemType.COM15, false))
                    {
                        PluginManager.Manager.Server.Map.SpawnItem(Smod2.API.ItemType.GUN_USP, item.GetPosition(), item.GetPosition());
                        item.Remove();
                    }
                }
            }
            Timing.CallDelayed(0.5f, () => ChangeToUSP());
        }
    }
    static class Extension
    {
        public static void Resize(GameObject target, float x, float y, float z)
        {
            try
            {
                NetworkIdentity identity = target.GetComponent<NetworkIdentity>();

                target.transform.localScale = new Vector3(1 * x, 1 * y, 1 * z);

                ObjectDestroyMessage destroyMessage = new ObjectDestroyMessage();
                destroyMessage.netId = identity.netId;
                foreach (GameObject player in PlayerManager.players)
                {
                    if (player == target)
                        continue;

                    NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;

                    playerCon.Send(destroyMessage, 0);

                    object[] parameters = new object[] { identity, playerCon };
                    typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
                }
            }
            catch (Exception e)
            {
                PluginManager.Manager.Logger.Error("ERROR", $"Set Scale error: {e}");
            }
        }

        public static void SetHitboxScale(GameObject target, float x, float y, float z)
        {
            try
            {
                NetworkIdentity identity = target.GetComponent<NetworkIdentity>();


                target.transform.localScale = new Vector3(1 * x, 1 * y, 1 * z);

                ObjectDestroyMessage destroyMessage = new ObjectDestroyMessage();
                destroyMessage.netId = identity.netId;


                foreach (Player pl in PluginManager.Manager.Server.GetPlayers())
                {
                    GameObject player = (GameObject)pl.GetGameObject();
                    NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;

                    if (player != target)
                        playerCon.Send(destroyMessage, 0);

                    object[] parameters = new object[] { identity, playerCon };
                    typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
                }
            }
            catch (Exception e)
            {
                PluginManager.Manager.Logger.Error("ERROR", $"Set Scale error: {e}");
            }
        }


        public static void InvokeStaticMethod(this Type type, string methodName, object[] param)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public;
            MethodInfo info = type.GetMethod(methodName, flags);
            info?.Invoke(null, param);
        }
    }

}
