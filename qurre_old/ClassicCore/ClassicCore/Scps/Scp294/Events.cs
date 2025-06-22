using InventorySystem;
using InventorySystem.Items;
using Qurre.API.Objects;
using Qurre.API.Events;
using Qurre.API;
using UnityEngine;
using ClassicCore.Scps.Scp294.API;
using ClassicCore.BetterHints;

using PlayerEvents = Qurre.Events.Player;
using RoundEvents = Qurre.Events.Round;

namespace ClassicCore.Scps.Scp294
{
    static internal class Events
    {
        static internal void Init()
        {
            DrinksManager.Init();
            PlayerEvents.ItemUsing += ItemUsing;
            PlayerEvents.ItemUsed += ItemUsed;
            RoundEvents.Waiting += Waiting;
        }

        static public void Waiting()
        {
            var roomTransform = RoomType.EzUpstairsPcs.GetRoom().Transform;
            var scp = new API.Scp294();

            scp.Spawn(Vector3.zero, Vector3.zero);

            scp.Transform.parent = roomTransform;
            scp.Transform.localPosition = new Vector3(9.73511f, 5.517578f, 3.399986f);
            scp.Transform.localRotation = Quaternion.Euler(Vector3.up * 180);
        }

        static public void Interact(Player pl)
        {
            if (pl.ItemTypeInHand != ItemType.Coin)
            {
                pl.Hint(new(-20, 6, "<align=left><color=#F13D3D>Возьмите монетку в руку</color></align>", 3, false));
                return;
            }


            if (!DrinksManager.TryGetRandomDrink(out var drink))
            {
                pl.Hint(new(-20, 6, "<align=left><color=#F13D3D>Напиток не найден</color></align>", 3, false));
                return;
            }

            pl.Inventory.ServerRemoveItem(pl.CurrentItem.SerialNumber, null);

            var serial = ItemSerialGenerator.GenerateNext();

            pl.Inventory.ServerAddItem(ItemType.SCP207, serial, null);
            DrinksManager.Drinks.Add(serial, drink);
            MEC.Timing.CallDelayed(0.5f, () => pl.Inventory.ServerSelectItem(serial));

            pl.Hint(new(0, -2, $"<size=30><color=#F12E7C><b>Вы купили \"{drink.Name}\" за 15 монет</b></color></size>", 7, false));
            pl.SendConsoleMessage($"Вы купили \"{drink.Name}\"\nОписание:\n{drink.Description}", "white");
        }

        static public void ItemUsing(ItemUsingEvent ev)
        {
            if (!ev.Allowed) return;
            if (ev.Item.TryGetDrink(out var drink))
            {
                ev.Allowed = drink.OnStartDrinking(ev.Player);
            }
        }

        static public void ItemUsed(ItemUsedEvent ev)
        {
            if (ev.Item.TryGetDrink(out var drink))
            {
                drink.OnDrank(ev.Player);
            }
        }
    }
}