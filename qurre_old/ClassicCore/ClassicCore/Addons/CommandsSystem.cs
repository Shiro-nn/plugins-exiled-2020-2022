using Qurre.API.Events;
using System;
using System.Collections.Generic;
namespace ClassicCore.Addons
{
    internal static class CommandsSystem
    {
        private static readonly Dictionary<string, Action<SendingConsoleEvent>> _consoles = new();
        private static readonly Dictionary<string, Action<SendingRAEvent>> _ras = new();
        internal static void RegisterConsole(string command, Action<SendingConsoleEvent> action)
        {
            if (_consoles.ContainsKey(command)) throw new Exception($"Console command \"${command}\" already exist");
            _consoles.Add(command, action);
        }
        internal static void RegisterRemoteAdmin(string command, Action<SendingRAEvent> action)
        {
            if (_ras.ContainsKey(command)) throw new Exception($"Console command \"${command}\" already exist");
            _ras.Add(command, action);
        }
        internal static void ConsoleInvoke(SendingConsoleEvent ev)
        {
            if (!_consoles.TryGetValue(ev.Name, out var action)) return;
            action(ev);
        }
        internal static void RAInvoke(SendingRAEvent ev)
        {
            if (!_ras.TryGetValue(ev.Name, out var action)) return;
            action(ev);
        }
    }
}