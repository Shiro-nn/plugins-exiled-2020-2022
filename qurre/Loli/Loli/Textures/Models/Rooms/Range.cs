using Interactables.Interobjects.DoorUtils;
using Loli.DataBase.Modules;
using Loli.Discord;
using MEC;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Attributes;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using Qurre.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KeycardPermissions = Interactables.Interobjects.DoorUtils.KeycardPermissions;
using Primitive = Qurre.API.Controllers.Primitive;

namespace Loli.Textures.Models.Rooms
{
    internal class Range
    {
        internal const string DoorName = "Range_Armory_Door";//-309.5 -644 -1111.5   |   -285.5 -640 -1088.5

        public static List<CustomDoor> CustomDoors = new();
        internal static Range Static;
        internal static readonly Color32 Steklo = new(0, 0, 0, 100);
        public CustomRoom Model;
        public static Transform SpawnPoint;
        private readonly ModelPrimitive DestroyLater;
        private readonly Model DestroyLater2;
        internal static readonly List<string> Cheaters = new();
        internal static void GetCheaters()
        {
            if (!Round.Waiting) return;
            foreach (var pl in Player.List)
            {
                if (Cheaters.Contains(pl.UserInfomation.UserId)) continue;
                try
                {
                    if (Data.Users.TryGetValue(pl.UserInfomation.UserId, out var _data) &&
                        (_data.mainhelper || _data.admin || _data.mainadmin || _data.owner)) continue;
                }
                catch { }
                Vector3 pos = pl.MovementState.Position;
                if (pos.x < -285.5f && pos.x > -309.5f && pos.y < -640 && pos.y > -650 && pos.z < -1088.5f && pos.z > -1111.5f)
                {
                    Cheaters.Add(pl.UserInfomation.UserId);
                    SendHook(pl);
                }
            }
            void SendHook(Player pl)
            {
                new System.Threading.Thread(() =>
                {
                    string hook = "https://discord.com/api/webhooks";
                    Webhook webhk = new(hook);
                    List<Embed> listEmbed = new();
                    Embed embed = new()
                    {
                        Title = "Обнаружен NoClip",
                        Color = 1,
                        Description = $"{pl.UserInfomation.Nickname} - {pl.UserInfomation.UserId} ({pl.UserInfomation.Ip})",
                        TimeStamp = System.DateTimeOffset.Now
                    };
                    listEmbed.Add(embed);
                    webhk.Send("Лишь теоритически, возможно, просто совпадение", Core.ServerName, null, false, embeds: listEmbed);
                }).Start();
            }
        }
        internal Range()
        {
            Static = this;
            Model = new CustomRoom("Range", new(-300, -650, -1100), Vector3.zero);
            SpawnPoint = new Model("SpawnPoint", new(-16, 2), Vector3.zero, Model).GameObject.transform;

            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(20.3f, 16.26f, 8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(20.3f, 16.26f, -8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(8.25f, 14, 8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(8.25f, 14, -8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(-4.02f, 14, 8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(-4.02f, 14, -8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(-17.57f, 14, 8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(-17.57f, 14, -8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(-32.23f, 14, 8.5f), 0.5f, 40, false));
            Model.AddPart(new ModelLight(Model, Load.WhiteColor, new(-32.23f, 14, -8.5f), 0.5f, 40, false));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 0), Vector3.zero, new(50, 0.1f, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-5, 18.9f), Vector3.zero, new(60, 0.1f, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 0.744f), Vector3.zero, new(0.4f, 1.4f, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 4), Vector3.zero, new(0.4f, 1.4f, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 9.52f, 12), Vector3.zero, new(0.4f, 19, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 9.52f, -12), Vector3.zero, new(0.4f, 19, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 11.42f, 8), Vector3.zero, new(0.4f, 15, 2)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 11.42f, -8), Vector3.zero, new(0.4f, 15, 2)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 11.42f, 3), Vector3.zero, new(0.4f, 15, 2)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 11.42f, -3), Vector3.zero, new(0.4f, 15, 2)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 17.1f), new(-45.02f, 0), new(0.4f, 20, 20)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 9.45f, -12.5f), new(90, 0), new(50, 0.1f, 19)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-10, 9.45f, 12.5f), new(90, 0), new(50, 0.1f, 19)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Steklo, new(-10, 9.6f, 0), new(0, 0), new(0.3f, 10, 25)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(15, 3.58f), Vector3.zero, new(0.4f, 7, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(19.87f, 5.73f), Vector3.zero, new(10, 0.1f, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(15, 10.5f, -11), Vector3.zero, new(0.4f, 7, 3)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(15, 10.5f, 11), Vector3.zero, new(0.4f, 7, 3)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(15, 16.43f), Vector3.zero, new(0.4f, 5, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Steklo, new(15, 10.5f), Vector3.zero, new(0.39f, 7, 19)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(19.87f, 14.65f, -12.5f), new(90, 0), new(10, 0.1f, 8.6f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(23.62f, 7.928f, -12.5f), new(90, 0), new(2.5f, 0.1f, 4.9f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(15.976f, 7.928f, -12.5f), new(90, 0), new(2.5f, 0.1f, 4.9f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(23.62f, 7.928f, 12.52f), new(90, 0), new(2.5f, 0.1f, 4.9f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(15.976f, 7.928f, 12.52f), new(90, 0), new(2.5f, 0.1f, 4.9f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(19.87f, 14.65f, 12.52f), new(90, 0), new(10, 0.1f, 8.6f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(24.82f, 12.3f), Vector3.zero, new(0.4f, 13.2f, 25)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-25.2f, 11.35f), Vector3.zero, new(0.4f, 16, 25)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-25.2f, 2.02f, -0.05f), Vector3.zero, new(0.4f, 4, 3)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-25.2f, 2.02f, -11.5f), Vector3.zero, new(0.4f, 4, 2)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-25.2f, 2.02f, -6.03f), Vector3.zero, new(0.4f, 4, 5)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Load.WhiteColor, new(-34.84f, 9.45f), Vector3.zero, new(0.4f, 19, 25)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Load.WhiteColor, new(-32.82f, 2.489f, 5.504f), new(58, 0), new(3.7f, 9.3f, 1)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Quad, Load.WhiteColor, new(-32.82f, 4.955f, 10.94f), new(90, 0), new(3.7f, 3, 1)));

            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Steklo, new(-30.07f, 4.935f, -5.59f), Vector3.zero, new(9.5f, 0.05f, 14)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Steklo, new(-32.82f, 11.95f, 1.385f), Vector3.zero, new(3.7f, 14, 0.05f)));
            Model.AddPart(new ModelPrimitive(Model, PrimitiveType.Cube, Steklo, new(-30.9695f, 11.95f, 5.36f), new(0, 90), new(8, 14, 0.05f)));

            DestroyLater2 = new Model("DestroyLater2", Vector3.zero, Vector3.zero, Model);
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Sport, new(5.21f, 0.05f, 7.71f), new(0, -50), Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Sport, new(5.21f, 0.05f, -7.71f), new(0, 50), Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Sport, new(-1.3f, 0.05f, 10.36f), new(0, -50), Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Sport, new(-1.3f, 0.05f, -10.36f), new(0, 50), Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Binary, new(9, 0.05f, 5), new(0, -30), Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Binary, new(9, 0.05f, -5), new(0, 30), Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Dboy, new(13.74f, 0.05f), Vector3.zero, Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Dboy, new(2.15f, 0.05f, -9.34f), new(0, 50), Vector3.one));
            DestroyLater2.AddPart(new ModelTarget(DestroyLater2, TargetPrefabs.Dboy, new(2.15f, 0.05f, 9.34f), new(0, -50), Vector3.one));
            Timing.CallDelayed(5f, () => DestroyLater2.AddPart(new ModelWorkStation(DestroyLater2, new(-16.466f, 0.05f, -12.352f), Vector3.zero, Vector3.one)));

            {
                Map.Rooms.TryFind(out var room, x => x.Type == RoomType.EzVent);
                new Lift(new(room.Transform, new(0, 2.5f, 0.45f), Vector3.zero),
                    new(Model.GameObject.transform, new(19.92f, 8.22f, -12.65f), Vector3.zero, true), Load.WhiteColor);
                Primitive prim = new(PrimitiveType.Cube) { Color = Load.WhiteColor };
                var gobj = prim.Base.gameObject;
                gobj.transform.parent = room.Transform;
                gobj.transform.localPosition = new Vector3(0, 6.046f, 0.6f);
                gobj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gobj.transform.localScale = new Vector3(5, 2.2f, 0.1f);
                for (int i = 0; i < room.Transform.childCount; i++)
                {
                    Transform child = room.Transform.GetChild(i);
                    if (child.tag == "SP_GUARD") child.localPosition = new(0, 1.32f, 5);
                }
            }

            DestroyLater = new ModelPrimitive(Model, PrimitiveType.Cube, Color.gray, new(-23.97f, 9.45f), Vector3.zero, new(0.4f, 19, 25));
        }
        internal static void ClearWaitInv()
        {
            if (!Round.Waiting) return;
            short timer = GameCore.RoundStart.singleton.NetworkTimer;
            if (timer < 0) return;
            if (timer > 1) return;
            foreach (var pl in Player.List)
            {
                try
                {
                    pl.Inventory.Clear();
                    pl.Inventory.Base.UserInventory.ReserveAmmo.Clear();
                    pl.Inventory.Base.SendAmmoNextFrame = true;

                    pl.RoleInfomation.SetNew(PlayerRoles.RoleTypeId.Spectator, PlayerRoles.RoleChangeReason.Respawn);
                }
                catch { }
            }
            try { Static.DestroyLater2.Destroy(); } catch { }
        }

        [EventMethod(RoundEvents.Start)]
        internal static void RoundStart()
        {
            try { Static.DestroyLater2.Destroy(); } catch { }
            Timing.CallDelayed(3.5f, () =>
            {
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Sport, new(5.21f, 0.05f, 7.71f), new(0, -50), Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Sport, new(5.21f, 0.05f, -7.71f), new(0, 50), Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Sport, new(-1.3f, 0.05f, 10.36f), new(0, -50), Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Sport, new(-1.3f, 0.05f, -10.36f), new(0, 50), Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Binary, new(9, 0.05f, 5), new(0, -30), Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Binary, new(9, 0.05f, -5), new(0, 30), Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Dboy, new(13.74f, 0.05f), Vector3.zero, Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Dboy, new(2.15f, 0.05f, -9.34f), new(0, 50), Vector3.one));
                Static.Model.AddPart(new ModelTarget(Static.Model, TargetPrefabs.Dboy, new(2.15f, 0.05f, 9.34f), new(0, -50), Vector3.one));
                try { Static.DestroyLater.Primitive.Destroy(); } catch { }
                {
                    new Lift(new(Static.Model.GameObject.transform, new(19.71f, 8.22f, 12.67f), new(0, 180), true),
                        new(Static.Model.GameObject.transform, new(-25.2f, 2.51f, 4.18f), new(0, 90), true), Load.WhiteColor);
                }
                {
                    {
                        Color32 color = new(150, 150, 150, 255);
                        CreateDoor(new(-25.2f, 0.02f, -2.53f), new(0, 90));
                        CreateDoor(new(-25.2f, 0.02f, -9.522f), new(0, 90));
                        void CreateDoor(Vector3 pos, Vector3 rot)
                        {
                            ModelDoor obj = new(Static.Model, DoorPrefabs.DoorEZ, pos, rot, Vector3.one);
                            Static.Model.AddPart(obj);
                            obj.Door.Name = DoorName;
                            if (obj.Door.DoorVariant.TryGetComponent<DoorNametagExtension>(out var dnametag)) dnametag.UpdateName(DoorName);
                            obj.Door.Permissions.RequiredPermissions = KeycardPermissions.ArmoryLevelTwo;
                            var DoArmoryDoor = new Model("DoArmoryDoor", pos + Vector3.up * 2.5f, rot, Static.Model);
                            var ArmoryDoor = new Model("ArmoryDoor", Vector3.zero, Vector3.zero, DoArmoryDoor);
                            ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, color, new(0, -1.56f), Vector3.zero, new(2, 2, 0.1f)));
                            ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, color, new(0, 0.466f), Vector3.zero, new(2, 0.8f, 0.1f)));
                            ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, color, new(0.914f, -0.244f), Vector3.zero, new(0.2f, 0.7f, 0.1f)));
                            ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, color, new(-0.914f, -0.244f), Vector3.zero, new(0.2f, 0.7f, 0.1f)));
                            ArmoryDoor.AddPart(new ModelPrimitive(ArmoryDoor, PrimitiveType.Cube, Steklo, new(0, -0.244f), Vector3.zero, new(1.7f, 0.7f, 0.08f)));
                            CreateButton(new(-1.277f, -0.713f, 0.209f), Vector3.zero);
                            CreateButton(new(1.267f, -0.713f, -0.21f), new(0, 180));
                            void CreateButton(Vector3 pos, Vector3 rot)
                            {
                                Model Button = new("Button", pos, rot, DoArmoryDoor);
                                Button.AddPart(new ModelPrimitive(Button, PrimitiveType.Cube, new Color32(60, 60, 60, 255), Vector3.zero, Vector3.zero, new(0.25f, 0.25f, 0.1f)));
                                Button.AddPart(new ModelPrimitive(Button, PrimitiveType.Cylinder, Color.red, new(0, 0, 0.05f), new(90, 0), new(0.15f, 0.01f, 0.15f)));
                            }
                            CustomDoors.Add(new(obj.Door.DoorVariant, ArmoryDoor.GameObject.transform));
                        }
                    }
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.RifleRack, new(-28.17f, 0.5f, 1.415f), new(0, 180), Vector3.one));
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.Pedestal500, new(-25.756f, 0.05f, -12.065f), new(0, 310), Vector3.one));
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.LargeGun, new(-25.389f, 0.05f, -5.98f), new(0, 270), Vector3.one));
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.AdrenalineMedkit, new(-25.4f, 4.98f, -11.621f), new(0, -90), Vector3.one));
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.AdrenalineMedkit, new(-34.643f, 4.98f, -11.621f), new(0, 90), Vector3.one));
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.RifleRack, new(-30, 5.139f, -12.45f), Vector3.zero, Vector3.one));
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.RifleRack, new(-25.4f, 5.139f, -4.22f), new(0, -90), Vector3.one));
                    Static.Model.AddPart(new ModelLocker(Static.Model, LockerPrefabs.RifleRack, new(-34.64f, 5.139f, -4.22f), new(0, 90), Vector3.one));

                    {
                        GameObject obj = new();
                        var tr = obj.transform;
                        tr.parent = Static.Model.GameObject.transform;
                        tr.localPosition = new(-25.455f, 4.95f, -9.37f);
                        tr.localRotation = Quaternion.Euler(new(0, -90));
                        new WorkStation(tr.position, tr.rotation.eulerAngles, Vector3.one);
                    }
                    {
                        GameObject obj = new();
                        var tr = obj.transform;
                        tr.parent = Static.Model.GameObject.transform;
                        tr.localPosition = new(-34.6f, 4.95f, -9.37f);
                        tr.localRotation = Quaternion.Euler(new(0, 90));
                        new WorkStation(tr.position, tr.rotation.eulerAngles, Vector3.one);
                    }
                }
            });
        }

        [EventMethod()]
        internal static void DoorEvents(DoorDamageEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not DoorName) return;
            ev.Allowed = false;
        }

        [EventMethod()]
        internal static void DoorEvents(DoorLockEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not DoorName) return;
            ev.Allowed = false;
        }

        [EventMethod()]
        internal static void DoorEvents(DoorOpenEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not DoorName) return;
            ev.Allowed = false;
        }

        [EventMethod()]
        internal static void DoorEvents(Scp079InteractDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not DoorName) return;
            ev.Allowed = false;
        }

        [EventMethod()]
        internal static void DoorEvents(Scp079LockDoorEvent ev)
        {
            if (ev.Door is null) return;
            if (ev.Door.Name is not DoorName) return;
            ev.Allowed = false;
        }
        internal class CustomDoor
        {
            internal readonly DoorVariant Door;
            internal readonly Transform Custom;
            internal CustomDoor(DoorVariant door, Transform custom)
            {
                Door = door;
                Custom = custom;
            }
            private bool InUse = false;
            internal bool Interact(bool open)
            {
                if (InUse) return false;
                InUse = true;
                Timing.RunCoroutine(Update());
                return true;
                IEnumerator<float> Update()
                {
                    for (float i = 0; 20 >= i; i++)
                    {
                        Custom.localPosition = new Vector3((open ? 0 : 2) + (open ? 0.1f : -0.1f) * i, 0, 0);
                        yield return Timing.WaitForSeconds(0.05f);
                    }
                    Custom.localPosition = new Vector3(open ? 2.01f : 0, 0, 0);
                    InUse = false;
                    yield break;
                }
            }
        }
    }
}