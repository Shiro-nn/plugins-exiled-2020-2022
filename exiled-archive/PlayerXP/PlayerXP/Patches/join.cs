
using System;
using Exiled.Events.EventArgs;
using Exiled.Loader.Features;
using Exiled.API.Features;
using HarmonyLib;
using MEC;
using UnityEngine;
using Player = Exiled.API.Features.Player;
namespace PlayerXP.Patches
{

    /// <summary>
    /// Patches <see cref="PlayerManager.AddPlayer(GameObject)"/>.
    /// Adds the <see cref="Player.Joined"/> event.
    /// </summary>
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.NetworkIsVerified), MethodType.Setter)]
    internal static class Joined
    {
        private static void Prefix(CharacterClassManager __instance, bool value)
        {
                // UserId will always be empty/null if it's not in online mode
                if (!value || (string.IsNullOrEmpty(__instance.UserId) && CharacterClassManager.OnlineMode))
                    return;

                if (!Player.Dictionary.TryGetValue(__instance.gameObject, out Player player))
                {
                    player = new Player(ReferenceHub.GetHub(__instance.gameObject));

                    Player.Dictionary.Add(__instance.gameObject, player);
                }

                Log.SendRaw($"Player {player?.Nickname} ({player?.UserId}) ({player?.Id}) connected with the IP: {player?.IPAddress}", ConsoleColor.Green);

                if (PlayerManager.players.Count >= CustomNetworkManager.slots)
                    MultiAdminFeatures.CallEvent(MultiAdminFeatures.EventType.SERVER_FULL);

                Timing.CallDelayed(0.25f, () =>
                {
                    if (player?.IsMuted == true)
                        player.ReferenceHub.characterClassManager.SetDirtyBit(2UL);
                });

                var ev = new JoinedEventArgs(Player.Get(__instance.gameObject));
        }
    }
}
