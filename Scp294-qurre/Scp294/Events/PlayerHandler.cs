using InventorySystem;
using InventorySystem.Items;
using Qurre.API.Events;
using Scp294.API;

using PlayerEvents = Qurre.Events.Player;

namespace Scp294.Events
{
    public sealed class PlayerHandler
    {
        internal void RegisterEvents()
        {
            PlayerEvents.PickupItem += OnPickupItem;
            PlayerEvents.ItemUsing += OnItemUsing;
            PlayerEvents.ItemUsed += OnItemUsed;
        }

        internal void UnregisterEvents()
        {
            PlayerEvents.PickupItem -= OnPickupItem;
            PlayerEvents.ItemUsing -= OnItemUsing;
            PlayerEvents.ItemUsed -= OnItemUsed;
        }

        public void OnPickupItem(PickupItemEvent ev)
        {
            if (!ev.Pickup.IsScp294())
                return;

            ev.Allowed = false;

            if (!DrinksManager.TryGetDrinkByItemType(ev.Player.ItemTypeInHand, out var drink))
            {
                ev.Player.ShowHint($"<size=30><color=#F13D3D><b>Отказано</b></color></size>", 2);
                return;
            }

            var serial = ItemSerialGenerator.GenerateNext();

            ev.Player.Inventory.ServerRemoveItem(ev.Player.CurrentItem.SerialNumber, null);
            ev.Player.Inventory.ServerAddItem(ItemType.SCP207, serial, null);
            DrinksManager.Drinks.Add(serial, drink);

            ev.Player.ShowHint($"<size=30><color=#F12E7C><b>Вы получили \"{drink.Name}\".</b></color></size>", 8);
            ev.Player.SendConsoleMessage($"Вы получили \"{drink.Name}\".\nОписание:\n{drink.Description}", "FFB051");
        }

        public void OnItemUsing(ItemUsingEvent ev)
        {
            if (ev.Item.TryGetDrink(out var drink))
            {
                ev.Allowed = drink.OnStartDrinking(ev.Player);
            }
        }

        public void OnItemUsed(ItemUsedEvent ev)
        {
            if (ev.Item.TryGetDrink(out var drink))
            {
                drink.OnDrank(ev.Player);
            }
        }
    }
}