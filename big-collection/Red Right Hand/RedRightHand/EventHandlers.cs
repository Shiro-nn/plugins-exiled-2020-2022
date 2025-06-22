using System;
using EXILED;
using System.Collections.Generic;
using System.Linq;
using MEC;
using UnityEngine;
using scp035.API;
using EXILED.Extensions;

namespace SerpentsHand
{
    public partial class EventHandlers
    {
        public static List<int> shPlayers = new List<int>();
        private List<int> shPocketPlayers = new List<int>();
        internal static ReferenceHub redc;
        internal static ReferenceHub redl;

        private bool isRoundStarted = false;

        private int respawnCount = 0;

        private static System.Random rand = new System.Random();

        private PlayerStats.HitInfo noDamage = new PlayerStats.HitInfo(0, "WORLD", DamageTypes.Nuke, 0);

        private static Vector3 shSpawnPos = new Vector3(0, 1001, 8);

        public void OnWaitingForPlayers()
        {
            Configs.ReloadConfigs();
        }

        public void OnRoundStart()
        {
            shPlayers.Clear();
            shPocketPlayers.Clear();
            isRoundStarted = true;
            respawnCount = 0;
        }

        public void OnRoundEnd()
        {
            isRoundStarted = false;
        }

        public void OnTeamRespawn(ref TeamRespawnEvent ev)
        {
            if (ev.IsChaos)
            {
                if (rand.Next(1, 101) <= Configs.spawnChance && Player.GetHubs().Count() > 0 && respawnCount >= Configs.respawnDelay)
                {
                    if(respawnCount == 0)
                    { 
                        List<ReferenceHub> SHPlayers = new List<ReferenceHub>();
                        List<ReferenceHub> CIPlayers = new List<ReferenceHub>(ev.ToRespawn);
                        ev.ToRespawn.Clear();

                        for (int i = 0; i < 3 && CIPlayers.Count > 2; i++)
                        {
                            ReferenceHub player = CIPlayers[rand.Next(CIPlayers.Count)];
                            SHPlayers.Add(player);
                            CIPlayers.Remove(player);
                        }
                        Timing.CallDelayed(0.1f, () => SpawnSquad(SHPlayers));
                    }
                }
            }
            respawnCount++;
        }

        public void OnPocketDimensionEnter(PocketDimEnterEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
            {
                shPocketPlayers.Add(ev.Player.queryProcessor.PlayerId);
            }
        }

        public void OnPocketDimensionDie(PocketDimDeathEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
            {
                if (!Configs.friendlyFire)
                {
                    ev.Allow = false;
                }
                if (Configs.teleportTo106)
                {
                    TeleportTo106(ev.Player);
                }
                shPocketPlayers.Remove(ev.Player.queryProcessor.PlayerId);
            }
        }

        public void OnPocketDimensionExit(PocketDimEscapedEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
            {
                ev.Allow = false;
                if (Configs.teleportTo106)
                {
                    TeleportTo106(ev.Player);
                }
                shPocketPlayers.Remove(ev.Player.queryProcessor.PlayerId);
            }
        }

        private ReferenceHub TryGet035()
        {
            return Scp035Data.GetScp035();
        }

        public void OnPlayerHurt(ref PlayerHurtEvent ev)
        {
            if (ev.Attacker.queryProcessor.PlayerId == 0 || !isRoundStarted) return;

            ReferenceHub scp035 = null;

            try
            {
                scp035 = TryGet035();
            } 
            catch (Exception x)
            {
                Log.Warn("SCP-035 not installed, ignoring API call.");
            }
            if (ev.Attacker.queryProcessor.PlayerId == redl?.queryProcessor.PlayerId && (ev.Player.GetTeam() == Team.SCP || (scp035 != null && ev.Attacker.queryProcessor.PlayerId == scp035.queryProcessor.PlayerId)))
            {
                ev.Amount = 100f;
            }
            if (ev.Attacker.queryProcessor.PlayerId == redc?.queryProcessor.PlayerId && (ev.Player.GetTeam() == Team.SCP || (scp035 != null && ev.Attacker.queryProcessor.PlayerId == scp035.queryProcessor.PlayerId)))
            {
                ev.Amount = 100f;
            }
        }

        public void OnPlayerDie(ref PlayerDeathEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
            {
                shPlayers.Remove(ev.Player.queryProcessor.PlayerId);
            }

            if (ev.Player.characterClassManager.CurClass == RoleType.Scp106 && !Configs.friendlyFire)
            {
                foreach (ReferenceHub player in Player.GetHubs().Where(x => shPocketPlayers.Contains(x.queryProcessor.PlayerId)))
                {
                    player.playerStats.HurtPlayer(new PlayerStats.HitInfo(50000, "WORLD", ev.Info.GetDamageType(), player.queryProcessor.PlayerId), player.gameObject);
                }
            }
            if (ev.Player.queryProcessor.PlayerId == redl?.queryProcessor.PlayerId)
            {
                ev.Player.serverRoles.NetworkMyText = "";
                ev.Player.serverRoles.NetworkMyColor = "default";
                ev.Player.serverRoles.HiddenBadge = null;
                ev.Player.serverRoles.RpcResetFixed();
                ev.Player.serverRoles.RefreshPermissions(true);
                ev.Player.serverRoles.HiddenBadge = ev.Player.serverRoles.MyText;
                ev.Player.serverRoles.NetworkGlobalBadge = null;
                ev.Player.serverRoles.SetText(null);
                ev.Player.serverRoles.SetColor(null);
                ev.Player.serverRoles.GlobalSet = false;
                ev.Player.serverRoles.RefreshHiddenTag();
                ev.Player.inventory.Clear();
            }
            if (ev.Player.queryProcessor.PlayerId == redc?.queryProcessor.PlayerId)
            {
                ev.Player.serverRoles.NetworkMyText = "";
                ev.Player.serverRoles.NetworkMyColor = "default";
                ev.Player.serverRoles.HiddenBadge = null;
                ev.Player.serverRoles.RpcResetFixed();
                ev.Player.serverRoles.RefreshPermissions(true);
                ev.Player.serverRoles.HiddenBadge = ev.Player.serverRoles.MyText;
                ev.Player.serverRoles.NetworkGlobalBadge = null;
                ev.Player.serverRoles.SetText(null);
                ev.Player.serverRoles.SetColor(null);
                ev.Player.serverRoles.GlobalSet = false;
                ev.Player.serverRoles.RefreshHiddenTag();
                ev.Player.inventory.Clear();
                redc = null;
            }
        }

        public void OnCheckRoundEnd(ref CheckRoundEndEvent ev)
        {
            ReferenceHub scp035 = null;

            try
            {
                scp035 = TryGet035();
            }
            catch (Exception x)
            {
                Log.Debug("SCP-035 not installed, ignoring API call.");
            }

            bool MTFAlive = CountRoles(Team.MTF) > 0;
            bool CiAlive = CountRoles(Team.CHI) > 0;
            bool ScpAlive = CountRoles(Team.SCP) + (scp035 != null && scp035.characterClassManager.CurClass != RoleType.Spectator ? 1 : 0) > 0;
            bool DClassAlive = CountRoles(Team.CDP) > 0;
            bool ScientistsAlive = CountRoles(Team.RSC) > 0;
            bool SHAlive = shPlayers.Count > 0;

            if (SHAlive && ((CiAlive && !Configs.scpsWinWithChaos) || DClassAlive || MTFAlive || ScientistsAlive))
            {
                ev.Allow = false;
            }
            else if (SHAlive && ScpAlive && !MTFAlive && !DClassAlive && !ScientistsAlive)
            {
                if (!Configs.scpsWinWithChaos)
                {
                    if (!CiAlive)
                    {
                        ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
                        ev.Allow = true;
                        ev.ForceEnd = true;
                    }
                }
                else
                {
                    ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
                    ev.Allow = true;
                    ev.ForceEnd = true;
                }
            }
        }

        public void OnSetRole(SetClassEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId))
            {
                if (ev.Player.GetTeam() != Team.TUT)
                { 
                    shPlayers.Remove(ev.Player.queryProcessor.PlayerId);
                }
            }
        }

        public void OnDisconnect(PlayerLeaveEvent ev)
        {
            Timing.CallDelayed(1f, () =>
            {
                int[] curPlayers = Player.GetHubs().Select(x => x.queryProcessor.PlayerId).ToArray();
                shPlayers.RemoveAll(x => !curPlayers.Contains(x));
            });
        }

        public void OnContain106(Scp106ContainEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && !Configs.friendlyFire)
            {
                ev.Allow = false;
            }
        }

        public void OnRACommand(ref RACommandEvent RACom)
        {
            string command = RACom.Command;
            ReferenceHub sender = RACom.Sender.SenderId == "SERVER CONSOLE" || RACom.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(RACom.Sender.SenderId);

            switch (command.ToLower())
            {
                case "redsquad":
                    {
                        SpawnPlaye(sender);
                        Cassie.CassieMessage(Configs.entryAnnouncement, true, true);
                        RACom.Sender.RAMessage("�������.", true);
                    }
                    break;
            }
        }

        public void OnGeneratorInsert(ref GeneratorInsertTabletEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && !Configs.friendlyFire)
            {
                ev.Allow = false;
            }
        }

        public void OnFemurEnter(FemurEnterEvent ev)
        {
            if (shPlayers.Contains(ev.Player.queryProcessor.PlayerId) && !Configs.friendlyFire)
            {
                ev.Allow = false;
            }
        }
    }
}