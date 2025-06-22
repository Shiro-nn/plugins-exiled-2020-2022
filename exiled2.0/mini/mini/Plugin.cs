using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs;
using Respawning;
using Respawning.NamingRules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mini
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.First;
        public override string Author { get; } = "fydne";
        public override void OnEnabled() => base.OnEnabled();
        public override void OnDisabled() => base.OnDisabled();
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();
            cfg1();
            RegisterEvents();
        }
        public override void OnUnregisteringCommands()
        {
            base.OnUnregisteringCommands();
            UnregisterEvents();
        }
        #endregion
        #region cfg
        internal void cfg1() => config = base.Config;
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += WaitingForPlayers;
            Exiled.Events.Handlers.Server.RespawningTeam += CISpawn;
            Exiled.Events.Handlers.Player.Escaping += Escape;
            Exiled.Events.Handlers.Player.ChangingRole += SPawn;
            Exiled.Events.Handlers.Player.InteractingElevator += Elevator;
            MEC.Timing.RunCoroutine(checkescape());
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= WaitingForPlayers;
            Exiled.Events.Handlers.Server.RespawningTeam -= CISpawn;
            Exiled.Events.Handlers.Player.Escaping -= Escape;
            Exiled.Events.Handlers.Player.ChangingRole -= SPawn;
            Exiled.Events.Handlers.Player.InteractingElevator -= Elevator;
        }
        #endregion
        #region events
        public void WaitingForPlayers()
        {
            cfg1();
            RespawnManager.Singleton.NamingManager.AllUnitNames.Clear();
            RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
            {
                SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
                UnitName = $"<color=#9b9b9b>Охрана</color>"
            });
            RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
            {
                SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
                UnitName = $"<color=#07D9D5>S</color><color=#02DE23>i</color><color=orange>m</color><color=yellow>e</color><color=#990ECC>t</color><color=red>r</color><color=#09E07F>i</color><color=#0983E0>a</color> <color=#E0D209>P</color><color=#F73B99>r</color><color=#0CB009>o</color><color=#9307D9>j</color><color=#05E657>e</color><color=#FF3D51>c</color><color=#FC8F00>t</color>"
            });
            RespawnManager.Singleton.NamingManager.AllUnitNames.Add(new SyncUnit
            {
                SpawnableTeam = (byte)SpawnableTeamType.NineTailedFox,
                UnitName = $"<size=-5%><color=#00FF22>discord.me/simetriaProject</color></size>"
            });
        }
        private void CISpawn(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency) Map.Broadcast(config.ciBcTime, config.ciBc);
        }
        private void Elevator(InteractingElevatorEventArgs ev) => ev.Lift.movingSpeed = config.LiftSpeed;
        private void SPawn(ChangingRoleEventArgs ev)
        {
            if(ev.NewRole == RoleType.FacilityGuard) ev.Player.ReferenceHub.characterClassManager.NetworkCurUnitName = $"<color=#9b9b9b>Охрана</color>";
        }
        internal void Escape(EscapingEventArgs ev)
        {
            if (ev.Player.Role == RoleType.ClassD && ev.Player.IsCuffed) ev.NewRole = RoleType.NtfScientist;
        }
        public IEnumerator<float> checkescape()
        {
            for (; ; )
            {
                Vector3 escape1 = new Vector3(174, 989, 32);
                Vector3 escape2 = new Vector3(166, 980, 25);
                List<Player> playerssl = Player.List.ToList();
                foreach (Player player in playerssl)
                {
                    if (player.ReferenceHub.transform.position.x <= escape1.x &&
                        player.ReferenceHub.transform.position.x >= escape2.x &&
                        player.ReferenceHub.transform.position.y <= escape1.y &&
                        player.ReferenceHub.transform.position.y >= escape2.y &&
                        player.ReferenceHub.transform.position.z <= escape1.z &&
                        player.ReferenceHub.transform.position.z >= escape2.z)
                    {
                        if (!player.IsCuffed)
                        {
                            if (player.Role == RoleType.FacilityGuard)
                            {
                                player.Position = Map.GetRandomSpawnPoint(RoleType.NtfLieutenant);
                                player.Inventory.ServerDropAll();
                                player.Role = RoleType.NtfLieutenant;
                            }
                        }
                        else
                        {
                            if (player.Role == RoleType.ChaosInsurgency)
                            {
                                player.ReferenceHub.characterClassManager.SetClassID(RoleType.NtfLieutenant);
                            }
                            else if (player.Team == Team.MTF)
                            {
                                player.ReferenceHub.characterClassManager.SetClassID(RoleType.ChaosInsurgency);
                            }
                        }
                    }
                }
                yield return MEC.Timing.WaitForSeconds(1f);
            }
        }
        #endregion
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float LiftSpeed { get; set; } = 5;
        public ushort ciBcTime { get; set; } = 10;
        public string ciBc { get; set; } = "Приехал отряд хаоса.";
    }
}