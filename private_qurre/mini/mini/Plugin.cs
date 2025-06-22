using Qurre.API;
using Qurre.API.Events;
using Qurre.API.Objects;
using Respawning;
using Respawning.NamingRules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mini
{
    public class Plugin : Qurre.Plugin
    {
        #region override
        public override int Priority { get; } = -9999999;
        public override string Developer { get; } = "fydne";
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Qurre.Events.Round.WaitingForPlayers += WaitingForPlayers;
            Qurre.Events.Player.Escape += Escape;
            Qurre.Events.Player.RoleChange += SPawn;
            Qurre.Events.Player.InteractLift += Elevator;
            MEC.Timing.RunCoroutine(checkescape());
        }
        private void UnregisterEvents()
        {
            Qurre.Events.Round.WaitingForPlayers -= WaitingForPlayers;
            Qurre.Events.Player.Escape -= Escape;
            Qurre.Events.Player.RoleChange -= SPawn;
            Qurre.Events.Player.InteractLift -= Elevator;
        }
        #endregion
        #region events
        public void WaitingForPlayers()
        {
            RespawnManager.Singleton.NamingManager.AllUnitNames.Clear();
            var txt1 = Config.GetString("mini_unit", "<color=#07D9D5>S</color><color=#02DE23>i</color><color=orange>m</color><color=yellow>e</color>" +
                $"<color=#990ECC>t</color><color=red>r</color><color=#09E07F>i</color><color=#0983E0>a</color> <color=#E0D209>P" +
                $"</color><color=#F73B99>r</color><color=#0CB009>o</color><color=#9307D9>j</color><color=#05E657>e</color><color=#FF3D51>c</color><color=#FC8F00>t</color>");
            var txt2 = Config.GetString("mini_unit2", "<size=-5%><color=#00FF22>discord.me/simetriaProject</color></size>");
            Round.AddUnit(TeamUnitType.NineTailedFox, "<color=#9b9b9b>Охрана</color>");
            Round.AddUnit(TeamUnitType.NineTailedFox, txt1);
            Round.AddUnit(TeamUnitType.NineTailedFox, txt2);
            Round.AddUnit(TeamUnitType.ChaosInsurgency, txt1);
            Round.AddUnit(TeamUnitType.ChaosInsurgency, txt2);
            Round.AddUnit(TeamUnitType.ClassD, txt1);
            Round.AddUnit(TeamUnitType.ClassD, txt2);
            Round.AddUnit(TeamUnitType.Scientist, txt1);
            Round.AddUnit(TeamUnitType.Scientist, txt2);
            Round.AddUnit(TeamUnitType.Scp, txt1);
            Round.AddUnit(TeamUnitType.Scp, txt2);
            Round.AddUnit(TeamUnitType.Tutorial, txt1);
            Round.AddUnit(TeamUnitType.Tutorial, txt2);
        }
        private void Elevator(InteractLiftEvent ev) => ev.Lift.MovingSpeed = Config.GetInt("mini_lift_speed", 5);
        private void SPawn(RoleChangeEvent ev)
        {
            if (ev.NewRole == RoleType.FacilityGuard) ev.Player.UnitName = $"<color=#9b9b9b>Охрана</color>";
        }
        internal void Escape(EscapeEvent ev)
        {
            if (ev.Player.Role == RoleType.ClassD && ev.Player.Cuffed) ev.NewRole = RoleType.NtfScientist;
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
                        if (!player.Cuffed)
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
}