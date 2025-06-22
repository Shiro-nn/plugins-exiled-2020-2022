using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.Extensions;
using MEC;
using scp035.API;

namespace SerpentsHand
{
    partial class EventHandlers
    {
        internal static void SpawnPlayer(ReferenceHub player, bool full = true)
        {
            if (redc == null)
            {
                shPlayers.Add(player.queryProcessor.PlayerId);
                player.characterClassManager.SetClassID(RoleType.Tutorial);
                player.ammoBox.Networkamount = "999:999:999";
                player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, "<color=red>��</color> <color=#15ff00>MTF Alpha-1 Commander</color>\n<color=#00ffdc>�������������� ���������� �� `�`</color>", 10, false);
                player.characterClassManager.TargetConsolePrint(player.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n�� MTF Alpha-1 (Red Right Hand)\n�� ��������� ������� ������ �5\n���� ������ ����� ���� � ���������, �.� ����� �� ������ ����� ��� ���� ��������, � �.� SCP\n� ��� ���� ���������� ������ ������������, � ��� �� ������ � ���������� ������ �� SCP\n ----------------------------------------------------------- ", "red");
                player.characterClassManager.TargetConsolePrint(player.scp079PlayerScript.connectionToClient, "��-��, ������� fydne", "red");
                player.inventory.items.ToList().Clear();
                player.AddItem(ItemType.KeycardO5);
                player.AddItem(ItemType.GunE11SR);
                player.AddItem(ItemType.Radio);
                Timing.CallDelayed(1f, () => player.playerStats.health = 2000);
                Timing.CallDelayed(1f, () => player.playerStats.maxHP = 2000);
                Timing.CallDelayed(0.3f, () => player.plyMovementSync.OverridePosition(shSpawnPos, 0f));
                player.serverRoles.NetworkMyText = "MTF Alpha-1 Commander";
                player.serverRoles.NetworkMyColor = "red";
                redc = player;
                Timing.CallDelayed(0.3f, () => player.plyMovementSync.OverridePosition(shSpawnPos, 0f));
            }
            else if(redc != null)
            {
                SpawnPlayerl(player);
            }
        }
        internal static void SpawnPlaye(ReferenceHub player, bool full = true)
        {
            player.characterClassManager.SetClassID(RoleType.Tutorial);
            player.ammoBox.Networkamount = "999:999:999";
            player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, "<color=red>��</color> <color=#15ff00>MTF Alpha-1 Commander</color>\n<color=#00ffdc>�������������� ���������� �� `�`</color>", 10, false);
            player.characterClassManager.TargetConsolePrint(player.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n�� MTF Alpha-1 (Red Right Hand)\n�� ��������� ������� ������ �5\n���� ������ ����� ���� � ���������, �.� ����� �� ������ ����� ��� ���� ��������, � �.� SCP\n� ��� ���� ���������� ������ ������������, � ��� �� ������ � ���������� ������ �� SCP\n----------------------------------------------------------- ", "red");
            player.inventory.Clear();
            player.AddItem(ItemType.KeycardO5);
            player.AddItem(ItemType.GunE11SR);
            player.AddItem(ItemType.Radio);
            player.playerStats.health = 2000;
            player.serverRoles.NetworkMyText = "MTF Alpha-1 Commander";
            player.serverRoles.NetworkMyColor = "red";
            Timing.CallDelayed(1f, () => player.playerStats.health = 2000);
            Timing.CallDelayed(1f, () => player.playerStats.maxHP = 2000);
            Timing.CallDelayed(0.3f, () => player.plyMovementSync.OverridePosition(shSpawnPos, 0f));
            redc = player;
        }
        internal static void SpawnPlayerl(ReferenceHub player, bool full = true)
        {
            if (redc == null)
            {
                shPlayers.Add(player.queryProcessor.PlayerId);
                player.characterClassManager.SetClassID(RoleType.Tutorial);
                player.ammoBox.Networkamount = "999:999:999";
                player.GetComponent<Broadcast>().TargetAddElement(player.scp079PlayerScript.connectionToClient, "<color=red>��</color> <color=#15ff00>MTF Elite</color>\n<color=#00ffdc>�������������� ���������� �� `�`</color>", 10, false);
                player.characterClassManager.TargetConsolePrint(player.scp079PlayerScript.connectionToClient, "\n----------------------------------------------------------- \n�� MTF Alpha-1 (Red Right Hand)\n�� ��������� ������� ������ �5\n���� ������ ����� ���� � ���������, �.� ����� �� ������ ����� ��� ���� ��������, � �.� SCP\n� ��� ���� ���������� ������ ������������, � ��� �� ������ � ���������� ������ �� SCP\n ----------------------------------------------------------- ", "red");
                player.inventory.Clear();
                player.AddItem(ItemType.KeycardO5);
                player.AddItem(ItemType.GunE11SR);
                player.AddItem(ItemType.Medkit);
                player.AddItem(ItemType.Medkit);
                player.AddItem(ItemType.GrenadeFrag);
                player.AddItem(ItemType.Radio);
                Timing.CallDelayed(1f, () => player.playerStats.health = 1500);
                Timing.CallDelayed(1f, () => player.playerStats.maxHP = 1500);
                player.serverRoles.NetworkMyText = "MTF Elite";
                player.serverRoles.NetworkMyColor = "red";
                redc = player;

                Timing.CallDelayed(0.3f, () => player.plyMovementSync.OverridePosition(shSpawnPos, 0f));
            }
        }

        internal static void CreateSquad(int size)
        {
            List<ReferenceHub> spec = new List<ReferenceHub>();
            List<ReferenceHub> speca = new List<ReferenceHub>();
            List<ReferenceHub> pList = Player.GetHubs().ToList();

            foreach (ReferenceHub player in pList)
            {
                if (player.GetTeam() == Team.RIP)
                {
                    if (spec == null)
                    {
                        spec.Add(player);
                    }
                    else if(spec != null)
                    {
                        speca.Add(player);
                    }
                }
            }

            int spawnCount = 1;
            while (spec.Count > 0 && spawnCount <= size)
            {
                int index = rand.Next(0, spec.Count);
                if (spec[index] != null)
                {
                    SpawnPlayer(spec[index]);
                    spec.RemoveAt(index);
                    SpawnPlayerl(speca[index]);
                    speca.RemoveAt(index);
                    spawnCount++;
                }
            }
        }

        internal static void SpawnSquad(List<ReferenceHub> players)
        {
            foreach (ReferenceHub player in players)
            {
                SpawnPlayer(player);
            }

            Cassie.CassieMessage(Configs.entryAnnouncement, true, true);
        }

        private int CountRoles(Team team)
        {
            ReferenceHub scp035 = null;

            try
            {
                scp035 = Scp035Data.GetScp035();
            }
            catch (Exception x)
            {
                Log.Warn("SCP-035 not installed, ignoring API call.");
            }

            int count = 0;
            foreach (ReferenceHub pl in Player.GetHubs())
            {
                if (pl.GetTeam() == team)
                {
                    if (scp035 != null && pl.queryProcessor.PlayerId == scp035.queryProcessor.PlayerId) continue;
                    count++;
                }
            }
            return count;
        }

        private void TeleportTo106(ReferenceHub player)
        {
            ReferenceHub scp106 = Player.GetHubs().Where(x => x.characterClassManager.CurClass == RoleType.Scp106).FirstOrDefault();
            if (scp106 != null)
            {
                player.plyMovementSync.OverridePosition(scp106.transform.position, 0f);
            }
        }
    }
}