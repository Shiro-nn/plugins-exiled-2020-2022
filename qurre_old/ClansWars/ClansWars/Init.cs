using System.IO;
using Qurre;
using Qurre.API;
namespace ClansWars
{
    public class Init : Plugin
    {
        internal static readonly string MapsPath = Path.Combine(PluginManager.PluginsDirectory, "ClansMaps");

        public override string Developer => "fydne";
        public override string Name => "Clans-Wars Core";
        public override int Priority => int.MaxValue;
        public override void Disable() => Server.Restart();
        public override void Enable()
        {
            Objects.Initializator.Enable();
            Qurre.Events.Round.Start += Maps.CapturePoints.Stock.Load;
            Qurre.Events.Scp079.GeneratorActivate += Maps.CapturePoints.Stock.Gen;
            Qurre.Events.Map.DoorDamage += Maps.CapturePoints.Stock.DoorEvents;
        }
    }
}