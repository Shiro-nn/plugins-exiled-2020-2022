using Exiled.API.Features;
namespace RESURRECTION_OF_THE_DEAD
{
    public class RESURRECTION_OF_THE_DEAD : Plugin<Config>
    {
        public EventHandlers EventHandlers;
        public static RESURRECTION_OF_THE_DEAD plugin;
        public override void OnEnabled()
        {
            base.OnEnabled();
            plugin = this;
            EventHandlers = new EventHandlers();
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;
            Exiled.Events.Handlers.Player.DroppingItem += EventHandlers.OnDroppingItem;
        }
        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;
            Exiled.Events.Handlers.Player.DroppingItem -= EventHandlers.OnDroppingItem;
            EventHandlers = null;
        }
    }
}