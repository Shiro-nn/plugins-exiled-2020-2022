using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using GameCore;
using Grenades;
using MEC;
using Mirror;
using RemoteAdmin;
using UnityEngine;
using static DamageTypes;
using Log = EXILED.Log;
using Object = UnityEngine.Object;
using Harmony;

namespace TranquilizerGun {
    public class EventHandlers {

        public Plugin plugin;
        public bool testEnabled = false;
        public int gunCd = 10;
        public ItemType tgun;
        public Dictionary<ReferenceHub, int> scpShots;
        public List<ReferenceHub> tranquilized, protection, pepega;
        public readonly EXILED.ApiObjects.AmmoType _9mm = EXILED.ApiObjects.AmmoType.Dropped9;
        private static Vector3 alphaon = new Vector3(0, 1001, 8);
        public EventHandlers( Plugin plugin ) {
            this.plugin = plugin;
            tranquilized = new List<ReferenceHub>();
            protection = new List<ReferenceHub>();
            scpShots = new Dictionary<ReferenceHub, int>();
            try {
                Enum.TryParse(plugin.weapon, out tgun);
            } catch(Exception) {
                tgun = ItemType.GunUSP;
                Plugin.Config.SetString("tgun_weapon", ItemType.GunUSP.ToString());
                Log.Error($"Can't parse \"{plugin.weapon}\", using \"GunUSP\" by default.");
            }
        }

        public void PlayerLeaveEvent( PlayerLeaveEvent e ) {
            if(scpShots.ContainsKey(e.Player)) scpShots.Remove(e.Player);
            if(protection.Contains(e.Player)) protection.Remove(e.Player);
            if(tranquilized.Contains(e.Player)) tranquilized.Remove(e.Player);
        }

        public void OnRoundStart() {
            if(plugin.replaceComGun) Timing.RunCoroutine(DelayedReplace());
        }

        public void OnCommand( ref RACommandEvent ev ) {
            try {
                if(ev.Command.Contains("REQUEST_DATA PLAYER_LIST SILENT")) return;
                string[] args = ev.Command.ToLower().Split(' ');
                ReferenceHub sender = ev.Sender.SenderId == "SERVER CONSOLE" || ev.Sender.SenderId == "GAME CONSOLE" ? Player.GetPlayer(PlayerManager.localPlayer) : Player.GetPlayer(ev.Sender.SenderId);
                if(args[0] == "tg" || args[0] == "tgun" || args[0] == "tranquilizergun" || args[0] == "tranquilizer") {
                    ev.Allow = false;
                    if(!CheckPermission(sender, "tg")) {
                        ev.Sender.RAMessage(plugin.accessDenied);
                        return;
                    }
                    if(args.Length >= 2) {
                        if(args[1] == "protect") {
                            if(!CheckPermission(sender, "protect")) {
                                ev.Sender.RAMessage(plugin.accessDenied);
                                return;
                            }
                            if(protection.Contains(sender)) {
                                protection.Remove(sender);
                                ev.Sender.RAMessage($"<color=green>You've lost your protection against tranquilizers!</color>");
                            } else {
                                protection.Add(sender);
                                ev.Sender.RAMessage($"<color=green>You've gained protection against tranquilizers!</color>");
                            }
                            return;
                        } else if(args[1] == "toggle") {
                            if(!CheckPermission(sender, "toggle")) {
                                ev.Sender.RAMessage(plugin.accessDenied);
                                return;
                            }
                            plugin.enabled = !plugin.enabled;
                            Plugin.Config.SetString("tgun_enable", plugin.enabled.ToString());
                            ev.Sender.RAMessage($"<color=green>Tranquilizers are now: {plugin.enabled}.</color>");
                            if(plugin.enabled) plugin.StartEvents();
                            else if(!plugin.enabled) plugin.StopEvents();
                            return;
                        } else if(args[1] == "replaceguns") {
                            if(!CheckPermission(sender, "replaceguns")) {
                                ev.Sender.RAMessage(plugin.accessDenied);
                                return;
                            }
                            Timing.RunCoroutine(DelayedReplace());
                            ev.Sender.RAMessage($"<color=green>Replaced all COM15 pistols with {tgun.ToString()}.</color>");
                            return;
                        } else if(args[1] == "addgun" ) {
                            if(!CheckPermission(sender, "addgun")) {
                                ev.Sender.RAMessage(plugin.accessDenied);
                                return;
                            }
                            sender.AddItem(tgun);
                            ev.Sender.RAMessage($"<color=green>You received a tranquilizing gun!</color>");
                            return;
                        } else if(args[1] == "setgun") {
                            if(!CheckPermission(sender, "setgun")) {
                                ev.Sender.RAMessage(plugin.accessDenied);
                                return;
                            }
                            ItemType newGun = sender.GetCurrentItem().id;
                            if(!newGun.IsWeapon()) {
                                ev.Sender.RAMessage($"<color=red>Can't set Tranquilizer gun to: {sender.GetCurrentItem().id}</color>");
                                return;
                            }
                            Plugin.Config.SetString("tgun_weapon", newGun.ToString());
                            tgun = newGun;
                            ev.Sender.RAMessage($"<color=green>Tranquilizer gun has been set to: {newGun.ToString()}.</color>");
                            return;
                        } else if(args[1] == "reload") {
                            if(!CheckPermission(sender, "reload")) {
                                ev.Sender.RAMessage(plugin.accessDenied);
                                return;
                            }
                            plugin.ReloadConfig();
                            ev.Sender.RAMessage($"<color=green>Configuration variables have been reloaded.</color>");
                            return;
                        } else if(args[1] == "sleep") {
                            if(!CheckPermission(sender, "sleep")) {
                                ev.Sender.RAMessage(plugin.accessDenied);
                                return;
                            } 

                            if(args.Length == 2) {
                                ev.Sender.RAMessage($"<color=red>Please try: \"{args[0]} <sleep> <player>.</color>");
                                return;
                            }

                            if(args[2] == "all" || args[2] == "*") {
                                int i = 0;
                                foreach(ReferenceHub player in Player.GetHubs()) {
                                    if(player.characterClassManager.CurClass != RoleType.Spectator || player.characterClassManager.CurClass != RoleType.None) {
                                        i++;
                                        GoSleepySleepy(player);
                                    }
                                }
                                ev.Sender.RAMessage($"<color=green>Made {i} players fall asleep.</color>");
                            } else {
                                ReferenceHub player;
                                try {
                                    player = Player.GetPlayer(args[2]);
                                } catch(Exception) {
                                    ev.Sender.RAMessage($"<color=red>Couldn't find player: {args[2]}.</color>");
                                    return;
                                }

                                ev.Sender.RAMessage($"<color=green>Putting {player.GetNickname()} to sleep.</color>");
                                GoSleepySleepy(player);
                            }
                            return;
                        } else if(args[1] == "version") {
                            ev.Sender.RAMessage("You're currently using " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                            return;
                        }
                    }
                    ev.Sender.RAMessage($"<color=red>Try using: \"{args[0]} <argument>\"</color>" +
                        $"\n<color=red>Possible arguments: reload / protect / replaceguns / toggle / sleep / version / setgun / addgun</color>");
                }
            } catch(Exception e) {
                Log.Error("" + e.StackTrace);
            }
            return;
        }

        public void OnPickupEvent( ref PickupItemEvent ev ) {
            if(tranquilized.Contains(ev.Player)) {
                ev.Allow = false;
                return;
            }

            if(ev.Item.ItemId == tgun && plugin.youPickedupDuration > 0) {
                if(plugin.clearBroadcasts) ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(plugin.youPickedupDuration, plugin.youPickedupText);
            }
        }

        public void OnShootEvent( ref ShootEvent ev ) {
            ReferenceHub hub = ev.Shooter;
            if((plugin.requiresPermission && !ev.Shooter.CheckPermission("use"))) return;
            if(ev.Shooter.inventory.NetworkcurItem == tgun) {
                if(ev.Shooter.inventory.GetItemInHand().durability < plugin.tranqAmmo) {
                    if(plugin.noAmmoDuration > 0) {
                        if(plugin.clearBroadcasts) ev.Shooter.ClearBroadcasts();
                        ev.Shooter.Broadcast(plugin.noAmmoDuration, plugin.noAmmoText);
                    }
                    int savedAmmo = (int) ev.Shooter.inventory.GetItemInHand().durability;
                    ev.Shooter.SetWeaponAmmo(0);
                    Timing.CallDelayed(0.2f, () => { hub.SetWeaponAmmo(savedAmmo); });
                    ev.Allow = false;
                    return;
                }
                ev.Shooter.RemoveWeaponAmmo(plugin.tranqAmmo);
            }
        }

        public void OnPlayerHurt( ref PlayerHurtEvent ev ) {
            if(tranquilized.Contains(ev.Player) && ev.DamageType == DamageTypes.Decont) { 
                ev.Amount = 0;
                return;
            }

            if(IsThisFrustrating(ev.DamageType) && ThisIsMoreFrustrating(ev.DamageType) == tgun && !protection.Contains(ev.Player) && ev.Player != ev.Attacker) {
                ev.Amount = plugin.tranqDamage;
                if(ev.Player.characterClassManager.CurClass == RoleType.Scp173 && plugin.blacklist173) return; 
                if(ev.Player.GetTeam() == Team.SCP && plugin.ScpShotsNeeded > 1) {
                    if(!scpShots.ContainsKey(ev.Player)) scpShots.Add(ev.Player, 0);
                    scpShots[ev.Player] += 1;
                    if(scpShots[ev.Player] >= plugin.ScpShotsNeeded ) {
                        GoSleepySleepy(ev.Player);
                        scpShots[ev.Player] = 0;
                    }
                    return;
                }
                GoSleepySleepy(ev.Player);
            }
        }

        public void GoSleepySleepy( ReferenceHub player ) {
            int IdkHowToCode = (int) player.characterClassManager.CurClass;
            Vector3 UglyCopy = player.GetPosition();
            List<Inventory.SyncItemInfo> items = player.inventory.items.ToList();

            if(player.characterClassManager.CurClass == RoleType.Tutorial) IdkHowToCode = 15;
            // Tutorial = 15 ;
            if(plugin.warningTime > 0) {
                if(plugin.clearBroadcasts) player.ClearBroadcasts();
                player.Broadcast(plugin.warningTime, plugin.warningText);
            }    
            player.gameObject.GetComponent<RagdollManager>().SpawnRagdoll(player.gameObject.transform.position, Quaternion.identity, IdkHowToCode,
                new PlayerStats.HitInfo(1000f, player.characterClassManager.UserId, DamageTypes.Usp, player.queryProcessor.PlayerId), false,
                player.GetNickname(), player.GetNickname(), 0);
            tranquilized.Add(player);
            player.ClearInventory();
            EventPlugin.GhostedIds.Add(player.queryProcessor.PlayerId);
            //if(!plugin.doStun) player.SetPosition(2, -2, 3);
            if(!plugin.doStun) player.SetPosition(Vector3.up * 9999f);
            //else {
            // 10.0 update stun
            //}
            Timing.RunCoroutine(WakeTheFuckUpSamurai(player, items, UglyCopy, Extensions.GenerateRandomNumber(plugin.sleepDurationMin, plugin.sleepDurationMax)));
        }
        public bool IsThisFrustrating( DamageType type ) {
            return ((type == DamageTypes.Usp && tgun == ItemType.GunUSP) ||
                (type == DamageTypes.Com15 && tgun == ItemType.GunCOM15) ||
                (type == DamageTypes.E11StandardRifle && tgun == ItemType.GunE11SR) ||
                (type == DamageTypes.Logicer && tgun == ItemType.GunLogicer) ||
                (type == DamageTypes.MicroHid && tgun == ItemType.MicroHID) ||
                (type == DamageTypes.Mp7 && tgun == ItemType.GunMP7) ||
                (type == DamageTypes.P90 && tgun == ItemType.GunProject90));
        }

        public ItemType ThisIsMoreFrustrating( DamageType type ) {
            if(type == DamageTypes.Usp) return ItemType.GunUSP;
            else if(type == DamageTypes.Com15) return ItemType.GunCOM15;
            else if(type == DamageTypes.E11StandardRifle) return ItemType.GunE11SR;
            else if(type == DamageTypes.Logicer) return ItemType.GunLogicer;
            else if(type == DamageTypes.MicroHid) return ItemType.MicroHID;
            else if(type == DamageTypes.Mp7) return ItemType.GunMP7;
            else if(type == DamageTypes.P90) return ItemType.GunProject90;
            return ItemType.GunUSP;
        }

        public IEnumerator<float> DelayedReplace() {
            yield return Timing.WaitForSeconds(2);
            foreach(Pickup item in Object.FindObjectsOfType<Pickup>()) {
                if(item.ItemId == ItemType.GunCOM15) {
                    item.ItemId = tgun;
                    item.RefreshDurability(true, true);
                }
            }
        }

        public bool CheckPermission( ReferenceHub sender, string perm ) {
            if(!sender.CheckPermission("tgun.*") || !sender.CheckPermission("tgun." + perm)) return false;
            return true;
        }

        public IEnumerator<float> WakeTheFuckUpSamurai( ReferenceHub player, List<Inventory.SyncItemInfo> items, Vector3 pos, float time ) {
            yield return Timing.WaitForSeconds(time);
            player.plyMovementSync.OverridePosition(pos, 0f, false);
            tranquilized.Remove(player);
            player.SetInventory(items);
            EventPlugin.GhostedIds.Remove(player.queryProcessor.PlayerId);
            foreach(Ragdoll doll in Object.FindObjectsOfType<Ragdoll>()) {
                if(doll.owner.ownerHLAPI_id == player.GetNickname()) {
                    NetworkServer.Destroy(doll.gameObject);
                }
            }
            if (Map.IsNukeDetonated)
            {
                if (player.GetCurrentRoom().Zone != EXILED.ApiObjects.ZoneType.Surface) player.Kill();
                if (Vector3.Distance(player.GetPosition(), alphaon) <= 3.6f) player.Kill();
            }
        }
    }
}