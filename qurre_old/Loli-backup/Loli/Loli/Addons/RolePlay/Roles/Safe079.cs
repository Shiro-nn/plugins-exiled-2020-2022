using System.Collections.Generic;
using Qurre.API.Controllers;
using Qurre.API.Events;
using Qurre.API.Objects;
namespace Loli.Addons.RolePlay.Roles
{
    internal static class Safe079
    {
        internal static List<Generator> ActGens = new();
        internal static void Waiting() => ActGens.Clear();
        internal static void GenAct(InteractGeneratorEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (ev.Status == GeneratorStatus.Activated) ActGens.Add(ev.Generator);
            else if (ev.Status == GeneratorStatus.Disabled && ActGens.Contains(ev.Generator)) ActGens.Remove(ev.Generator);
        }
        internal static void GenAct(GeneratorActivateEvent ev)
        {
            if (!Plugin.RolePlay) return;
            if (!ActGens.Contains(ev.Generator)) ev.Allowed = false;
        }
    }
}