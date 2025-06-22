using Scp294.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

using Random = UnityEngine.Random;

namespace Scp294.API
{
    internal static class DrinksManager
    {
        internal static Dictionary<ushort, IDrink> Drinks;

        private static Dictionary<ItemType, List<IDrink>> _drinksSortedByItemType;

        internal static void Init()
        {
            Drinks = new Dictionary<ushort, IDrink>();
            _drinksSortedByItemType = new Dictionary<ItemType, List<IDrink>>();

            foreach (var type in Assembly.GetCallingAssembly().GetTypes())
            {
                try
                {
                    if (type.GetInterface("IDrink") != typeof(IDrink))
                        continue;

                    var drink = Activator.CreateInstance(type) as IDrink;

                    foreach (var itemType in drink.Items)
                    {
                        if (!_drinksSortedByItemType.ContainsKey(itemType))
                        {
                            _drinksSortedByItemType.Add(itemType, new List<IDrink>() { drink });
                        }
                        else if (!_drinksSortedByItemType[itemType].Contains(drink))
                        {
                            _drinksSortedByItemType[itemType].Add(drink);
                        }
                    }
                }
                catch { }
            }
        }

        internal static void Reset()
        {
            Drinks = null;
            _drinksSortedByItemType = null;
        }

        internal static bool TryGetDrinkByItemType(ItemType item, out IDrink drink)
        {
            if (_drinksSortedByItemType.TryGetValue(item, out var drinksList))
            {
                drink = drinksList[Random.Range(0, drinksList.Count - 1)];
                return true;
            }

            drink = null;
            return false;
        }
    }
}