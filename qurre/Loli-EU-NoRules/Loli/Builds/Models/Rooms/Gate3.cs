using System.IO;
using PlayerRoles;
using Qurre.API;
using Qurre.API.Attributes;
using Qurre.API.Objects;
using Qurre.Events;
using Qurre.Events.Structs;
using SchematicUnity.API.Objects;
using UnityEngine;

namespace Loli.Builds.Models.Rooms
{
    static class Gate3
    {
        static internal Vector3 ChaosSpawnPoint;
        static internal Vector3 DonateSpawnPoint;

        [EventMethod(PlayerEvents.Spawn, 666)]
        static void SpawnChangePos(SpawnEvent ev)
        {
            if (DonateSpawnPoint != default && ev.Player.Tag.Contains("DonateSpawnPoint"))
                ev.Position = DonateSpawnPoint;
            else if (ChaosSpawnPoint != default && ev.Role is RoleTypeId.ChaosConscript
                or RoleTypeId.ChaosMarauder or RoleTypeId.ChaosRepressor or RoleTypeId.ChaosRifleman)
                ev.Position = ChaosSpawnPoint;
        }

        [EventMethod(RoundEvents.Waiting)]
        static internal void Load()
        {
            new Lift(new(null, new(-137.86f, 992.69f, 54.93f), new(0, 90), true),
                new(null, new(-128.359f, 996.47f, -23.96f), new(0, 180), true), new Color32(59, 60, 62, 255));

            new Lift(new(null, new(-57.51f, 992.69f, 54.9f), new(0, 90), true),
                new(null, new(-135.45f, 996.47f, -24.04f), new(0, 180), true), new Color32(59, 60, 62, 255));

            var Scheme = SchematicUnity.API.SchematicManager.LoadSchematic(Path.Combine(Pathes.Plugins, "Schemes", "Gate3.json"), Vector3.zero, default);
            foreach (var _obj in Scheme.Objects)
                findObjects(_obj);
            static void findObjects(SObject obj)
            {
                if (obj is null)
                    return;

                switch (obj.Name)
                {
                    case "Steklo":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                prm.Color = new Color32(0, 133, 155, 150);
                            }
                            break;
                        }
                    case "ChaosSpawnPoint":
                        {
                            ChaosSpawnPoint = obj.Position;
                            break;
                        }
                    case "DonateSpawnPoint":
                        {
                            DonateSpawnPoint = obj.Position;
                            break;
                        }
                    case "DoorGate3Exit":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                var scr = prm.Base.Base.gameObject.AddComponent<OnDoorScript>();
                                scr._type = 1;
                            }
                            break;
                        }
                    case "DoorGate3Enter":
                        {
                            if (obj.Primitive != null)
                            {
                                PrimitiveParams prm = (PrimitiveParams)obj.Primitive;
                                var scr = prm.Base.Base.gameObject.AddComponent<OnDoorScript>();
                                scr._type = 2;
                            }
                            break;
                        }
                }

                if (obj.Childrens is null)
                    return;

                foreach (var _obj in obj.Childrens)
                    findObjects(_obj);
            }
            try
            {
                new Lamp(new(-135.0814f, 991.5962f, -6.769f), Vector3.zero);//1
                new Lamp(new(-135.08f, 991.5962f, -47.5f), Vector3.zero);//2
                new Lamp(new(-104.3564f, 991.8444f, -52.4819f), Vector3.zero);//3
                new Lamp(new(-104.3564f, 991.8444f, -33.6771f), Vector3.zero);//4
                new Lamp(new(-56.37352f, 991.4794f, -52.71039f), Vector3.zero, 0.5f);//5
                new Lamp(new(-44.4665f, 987.7709f, -38.0133f), Vector3.zero);//6
                new Lamp(new(-71.6298f, 987.7709f, -38.1228f), Vector3.zero);//7
                new Lamp(new(-8.630798f, 995.1732f, -11.799f), Vector3.zero);//8
                new Lamp(new(10.247f, 995.1732f, -3.732502f), Vector3.zero);//9
            }
            catch { }
        }

        [EventMethod(PlayerEvents.Damage)]
        internal static void AntiMachineDead(DamageEvent ev)
        {
            if (ev.DamageType == DamageTypes.Crushed &&
                ev.Target.MovementState.Position.y < 1000 &&
                ev.Target.MovementState.Position.y > 970)
                ev.Allowed = false;
        }

        static Vector3 DoorExit_Tp = new(-39.6f, 993, -36.04f);
        static Vector3 DoorEntrance_Tp = new(-58.345f, 988.723f, -36.184f);

        public class OnDoorScript : MonoBehaviour
        {
            internal int _type = 0;
            private void Start()
            {
                var collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
                collider.size = Vector3.one * 2f;
                collider.center = Vector3.up;
            }

            private void OnTriggerEnter(Collider other)
            {
                if (_type != 1 && _type != 2)
                    return;

                if (!other.gameObject.name.Contains("Player"))
                    return;

                Player pl = other.gameObject.GetPlayer();
                if (_type == 1)
                    pl.MovementState.Position = DoorExit_Tp;
                else if (_type == 2)
                    pl.MovementState.Position = DoorEntrance_Tp;
            }
        }
    }
}