/*using Loli.Addons.RolePlay.Roles;
using Loli.Items;
using MEC;
using Mirror;
using PlayerStatsSystem;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.Addons.RolePlay.CutScenes
{
    internal class Scp106Scene
    {
        internal void FixTP(SpawnEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom106Pos")) ev.Position = Map.GetRandomSpawnPoint(RoleType.Scp106);
        }
        internal void FixTP(DroppingItemEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom106Pos")) ev.Allowed = false;
        }
        internal void FixTP(PickupItemEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom106Pos")) ev.Allowed = false;
        }
        internal void FixTP(ScpAttackEvent ev)
        {
            if (Plugin.RolePlay && ev.Target.Tag.Contains("Custom106Pos")) ev.Allowed = false;
        }
        internal void FixTP(DiesEvent ev)
        {
            if (Plugin.RolePlay && ev.Target.Tag.Contains("Custom106Pos")) ev.Allowed = false;
        }
        internal void FixTP(DamageEvent ev)
        {
            if (Plugin.RolePlay && ev.Target.Tag.Contains("Custom106Pos")) ev.Allowed = false;
        }
        internal void FixTP(ContainEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom106Pos")) ev.Allowed = false;
        }
        internal void FixTP(InteractLockerEvent ev)
        {
            if (Plugin.RolePlay && ev.Player.Tag.Contains("Custom106Pos")) ev.Allowed = false;
        }
        internal void RoundStart()
        {
            if (!Plugin.RolePlay) return;
            int round = Round.CurrentRound;
            var pl106 = Player.List.Where(x => x.Role == RoleType.Scp106).FirstOrDefault();
            if (pl106 != null)
            {
                pl106.Tag += "Custom106Pos";
                pl106.Invisible = true;
                pl106.Role = RoleType.Tutorial;
                pl106.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=red>SCP 106</color>\n" +
                    "После катсцены, вы появитесь за SCP 106.</color></size>", 10, true);
                pl106.AddItem(ItemType.Flashlight);
            }
            Teleport();
            void Teleport()
            {
                var list = Player.List.Where(x => x.Role == RoleType.Scientist).ToList();
                if (list.Count == 0) return;
                list.Shuffle();
                var major = list.First();
                See(major);
                Timing.CallDelayed(28f, () =>
                {
                    major.Invisible = false;
                    major.Tag = major.Tag.Replace("Custom106Pos", "");
                    Scientists.SpawnMajor(major);
                });
                foreach (var pl in list.Skip(1)) BackSpawn(pl);
                void BackSpawn(Player pl)
                {
                    See(pl);
                    Timing.CallDelayed(28f, () =>
                    {
                        if (round != Round.CurrentRound) return;
                        pl.Invisible = false;
                        pl.Tag = pl.Tag.Replace("Custom106Pos", "");
                        pl.Role = RoleType.Scientist;
                    });
                }
                void See(Player pl)
                {
                    pl.Tag += "Custom106Pos";
                    pl.Invisible = true;
                    pl.Role = RoleType.Tutorial;
                    pl.Broadcast("<size=30%><color=#6f6f6f>Вы - <color=#e2e26d>Научный сотрудник</color>\n" +
                        "После катсцены, вы появитесь в лайт-зоне.</color></size>", 10, true);
                    pl.AddItem(ItemType.Flashlight);
                }
            }
            var door = Qurre.API.Extensions.GetDoor(DoorType.HCZ_106_Bottom);
            var room = Qurre.API.Extensions.GetRoom(RoomType.Hcz106);
            var rot = door.Rotation.eulerAngles;
            if (rot.y.Difference(0) < 1)
            {
                DoSpawn(new Vector3(door.Position.x - 11, door.Position.y + 1.8f, door.Position.z - 5.5f), new Vector3(280, rot.y, 0),
                    new Vector3(door.Position.x - 18, door.Position.y - 0.5f, door.Position.z + 6.5f),
                    new Vector3(door.Position.x + 7.5f, door.Position.y + 2.5f, door.Position.z - 3.6f));
            }
            else if (rot.y.Difference(90) < 1)
            {
                DoSpawn(new Vector3(door.Position.x - 5.5f, door.Position.y + 1.8f, door.Position.z + 11), new Vector3(280, rot.y, 0),
                    new Vector3(door.Position.x + 6.5f, door.Position.y - 0.5f, door.Position.z + 18),
                    new Vector3(door.Position.x - 3.6f, door.Position.y + 2.5f, door.Position.z - 7.5f));
            }
            else if (rot.y.Difference(180) < 1)
            {
                DoSpawn(new Vector3(door.Position.x + 11, door.Position.y + 1.8f, door.Position.z + 5.5f), new Vector3(280, rot.y, 0),
                    new Vector3(door.Position.x + 18, door.Position.y - 0.5f, door.Position.z - 6.5f),
                    new Vector3(door.Position.x - 7.5f, door.Position.y + 2.5f, door.Position.z + 3.6f));
            }
            else if (rot.y.Difference(270) < 1)
            {
                DoSpawn(new Vector3(door.Position.x + 5.5f, door.Position.y + 1.8f, door.Position.z - 11), new Vector3(280, rot.y, 0),
                    new Vector3(door.Position.x - 6.5f, door.Position.y - 0.5f, door.Position.z - 18),
                    new Vector3(door.Position.x + 3.6f, door.Position.y + 2.5f, door.Position.z + 7.5f));
            }
            void DoSpawn(Vector3 sciPos, Vector3 sciRot, Vector3 scpPos, Vector3 portalPos)
            {
                var botSci = Bot.Create(sciPos, sciRot, RoleType.Scientist, "Инженер камер содержания", "80 уровень", "lime");
                botSci.ItemInHand = ItemType.Flashlight;
                botSci.Direction = MovementDirection.Left;
                var bot106 = Bot.Create(Vector3.zero, new Vector3(rot.x, rot.y + 110, rot.z), RoleType.Scp106, "Scp 106", "1 уровень", "green");
                bot106.Despawn();
                Timing.CallDelayed(9f, () =>
                {
                    if (round != Round.CurrentRound) return;
                    Timing.RunCoroutine(DoRun106());
                    IEnumerator<float> DoRun106()
                    {
                        room.LightsOff(1);
                        bot106.Player.Scp106Controller.PortalPosition = scpPos;
                        yield return Timing.WaitForSeconds(0.5f);
                        bot106.Spawn();
                        room.LightsOff(1);
                        yield return Timing.WaitForSeconds(0.5f);
                        room.LightColor = Color.red;
                        Timing.RunCoroutine(DoRotation());
                        bot106.Direction = MovementDirection.Stop;
                        bot106.Player.Scp106Controller.UsePortal();
                        yield return Timing.WaitForSeconds(7f);
                        if (round != Round.CurrentRound) yield break;
                        bot106.Movement = PlayerMovementState.Walking;
                        bot106.Direction = MovementDirection.Forward;
                        yield return Timing.WaitForSeconds(0.5f);
                        GameObject gameObject = GameObject.Find("SCP106_PORTAL");
                        NetworkServer.UnSpawn(gameObject);
                        gameObject.transform.localScale = new Vector3(4, 4, 4);
                        NetworkServer.Spawn(gameObject);
                        bot106.Player.Scp106Controller.PortalPosition = portalPos;
                        yield return Timing.WaitForSeconds(2f);
                        if (round != Round.CurrentRound) yield break;
                        bot106.Movement = PlayerMovementState.Sneaking;
                        yield return Timing.WaitForSeconds(3f);
                        if (round != Round.CurrentRound) yield break;
                        bot106.Direction = MovementDirection.Stop;
                        NetworkServer.UnSpawn(gameObject);
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                        NetworkServer.Spawn(gameObject);
                        yield return Timing.WaitForSeconds(0.5f);
                        bot106.Player.Scp106Controller.PortalPosition = Qurre.API.Extensions.GetRoom(RoomType.HczEzCheckpoint).Position;
                        yield return Timing.WaitForSeconds(0.5f);
                        bot106.Player.Scp106Controller.UsePortal();
                        if (pl106 == null)
                        {
                            yield return Timing.WaitForSeconds(4f);
                            bot106.Destroy();
                            yield break;
                        }
                        else
                        {
                            yield return Timing.WaitForSeconds(7f);
                            try
                            {
                                pl106.Tag = pl106.Tag.Replace("Custom106Pos", "");
                                pl106.Role = RoleType.Scp106;
                            }
                            catch
                            {
                                bot106.Destroy();
                                yield break;
                            }
                            yield return Timing.WaitForSeconds(0.5f);
                            try
                            {
                                pl106.Scp106Controller.PortalPosition = bot106.Player.Scp106Controller.PortalPosition;
                                pl106.Position = bot106.Position;
                                pl106.Invisible = false;
                                bot106.Destroy();
                            }
                            catch
                            {
                                bot106.Destroy();
                                yield break;
                            }
                            yield break;
                        }
                    }
                });
                Timing.CallDelayed(14f, () =>
                {
                    if (round != Round.CurrentRound) return;
                    Timing.RunCoroutine(DoRunSci());
                    Timing.RunCoroutine(CheckSinkHole());
                    IEnumerator<float> DoRunSci()
                    {
                        float dx = botSci.Rotation.y;
                        for (float y = dx; y < dx + 75; y+=2)
                        {
                            botSci.Rotation = new Vector2(botSci.Rotation.x, y);
                            yield return Timing.WaitForSeconds(0.01f);
                        }
                        botSci.Direction = MovementDirection.Forward;
                        botSci.Movement = PlayerMovementState.Sprinting;
                        yield return Timing.WaitForSeconds(1f);
                        botSci.ItemInHand = ItemType.KeycardContainmentEngineer;
                        yield return Timing.WaitForSeconds(1.5f);
                        door.Open = true;
                        yield return Timing.WaitForSeconds(0.5f);
                        botSci.Direction = MovementDirection.Forward;
                        yield return Timing.WaitForSeconds(1.5f);
                        botSci.Direction = MovementDirection.Stop;
                        float dx2 = botSci.Rotation.y;
                        for (float y = dx2; y < dx2 + 107; y += 5)
                        {
                            botSci.Rotation = new Vector2(botSci.Rotation.x, y);
                            yield return Timing.WaitForSeconds(0.01f);
                        }
                        botSci.Direction = MovementDirection.Forward;
                        yield return Timing.WaitForSeconds(0.5f);
                        botSci.Player.EnableEffect(EffectType.SinkHole);
                        yield break;
                    }
                    IEnumerator<float> CheckSinkHole()
                    {
                        for (; ;)
                        {
                            if (Vector3.Distance(botSci.Position, portalPos) < 2f)
                            {
                                botSci.Player.DisableEffect(EffectType.SinkHole);
                                botSci.Position = Vector3.down * 1998.5f;
                                botSci.Player.EnableEffect(EffectType.Corroding);
                                yield return Timing.WaitForSeconds(1f);
                                Spawner.EngineerPos = botSci.Position;
                                Qurre.API.Controllers.Ragdoll.Create(RoleType.Scientist, botSci.Position, Quaternion.Euler(botSci.Rotation),
                                    new UniversalDamageHandler(-1, DeathTranslations.PocketDecay), "Инженер камер содержания", 0);
                                botSci.Destroy();
                                Spawner.SpawnInLure();
                                new Item(ItemType.Flashlight).Spawn(botSci.Position + Vector3.back * 0.5f);
                                yield break;
                            }
                            else if (Vector3.Distance(botSci.Position, portalPos) < 5)
                                botSci.Player.EnableEffect(EffectType.SinkHole);
                            else botSci.Player.DisableEffect(EffectType.SinkHole);
                            yield return Timing.WaitForSeconds(0.1f);
                        }
                    }
                });
                IEnumerator<float> DoRotation()
                {
                    botSci.Direction = MovementDirection.Stop;
                    for (float x = sciRot.x; x < 360; x++)
                    {
                        botSci.Rotation = new Vector2(x, sciRot.y);
                        yield return Timing.WaitForSeconds(0.01f);
                    }
                    yield break;
                }
            }
        }
    }
}*/