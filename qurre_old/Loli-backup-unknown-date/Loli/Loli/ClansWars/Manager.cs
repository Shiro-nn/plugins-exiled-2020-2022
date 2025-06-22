using Loli.ClansWars.Capturing;
namespace Loli.ClansWars
{
    internal class Manager
    {
        internal static Manager Static { get; private set; }
        internal Manager() => Static = this;
        internal Module Module;
        internal CapturingManager Capturing;
        internal void Initizialize()
        {
            Module = new Module();
            Qurre.Events.Round.Waiting += Module.Waiting;
            Qurre.Events.Round.Check += Module.Check;
            Qurre.Events.Player.Join += Module.Join;
            Qurre.Events.Player.Damage += Module.Damage;
            Capturing = new CapturingManager(this);
        }
        internal void UnRegister()
        {
            Module = new Module();
            Qurre.Events.Round.Waiting -= Module.Waiting;
            Qurre.Events.Round.Check -= Module.Check;
            Qurre.Events.Player.Join -= Module.Join;
            Qurre.Events.Player.Damage -= Module.Damage;
        }
    }
}