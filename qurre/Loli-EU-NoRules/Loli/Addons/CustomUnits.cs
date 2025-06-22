using Qurre.API;
using Qurre.API.Attributes;
using Qurre.Events;
using System.Collections.Generic;

namespace Loli.Addons
{
    static class CustomUnits
    {
        static readonly List<Unit> _units = new();

        static internal void AddUnit(string unit, string color) => _units.Add(new(unit, color));

        static internal string GetAll(Player pl)
        {
            string str = "";
            int vf = 7;
            bool contains = false;
            foreach (var unit in _units)
            {
                string _unit = $"<color={unit.Color}>{unit.Name}</color>";
                if (!contains && pl.UserInfomation.CustomInfo.Contains(unit.Name))
                    _unit = $"> <u>{_unit}</u>";
                str += $"<align=right><voffset={vf}em><b><size=100%>{_unit}</size></color></b></voffset></align>\n";
                vf--;
            }
            return $"<color=#0074ff>{str}</color>";
        }


        [EventMethod(RoundEvents.Waiting)]
        static void Refresh()
        {
            _units.Clear();
        }

        [EventMethod(RoundEvents.Start)]
        static void Start()
        {
            _units.Add(new("Охрана", "#afafa1"));
        }

        internal struct Unit
        {
            internal string Name { get; }
            internal string Color { get; }

            internal Unit(string name, string color)
            {
                Name = name;
                Color = color;
            }
        }
    }
}