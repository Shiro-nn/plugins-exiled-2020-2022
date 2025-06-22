using EXILED;

namespace RemoteKeycard
{
    public class RemoteKeycard : Plugin
    {
        internal static RemoteKeycard plugin { get; private set; }

        public override string getName => "RemoteKeycard";

        public const string Version = "V1.0.0";
        public override void OnDisable()
        {
            if (!enabled)
            {
                return;
            }
            Events.DoorInteractEvent -= LocalEvents.OnDoorAccess;
        }
        private EventHandlers LocalEvents;
        private bool enabled = false;
        public override void OnEnable()
        {
            enabled = Config.GetBool("rkc_enable", true);
            if (!enabled)
            {
                return;
            }
            plugin = this;

            Plugin.Info("Registering events...");
            LocalEvents = new EventHandlers();
            Events.DoorInteractEvent += LocalEvents.OnDoorAccess;

            ConfigManagers.Manager.ReloadConfig();
        }

        /*
        private void RegisterConfigs()
        {
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_card_list", "0,1,2,3,4,5,6,7,8,9,10,11", true, "CList settings")); // CL
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_door_list", string.Empty, true, "DList setings")); // DL
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_card_access", string.Empty, true, "Customized permissions to card")); // CA
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_door_access", string.Empty, true, "Customized permissions to door")); // DA
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_mode", 1, true, "Work mode"));
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_remote", true, true, "Remote door opening"));
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_default_if_none", false, true, "Use default settings if they are not specified, for example you want to change only one door"));
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_permission", false, true, "Permission mode"));
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_disable", false, true, "Disable this pluign"));
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_info", true, true, "Usage information"));
            this.AddConfig(new Smod2.Config.ConfigSetting("rpc_debug", false, true, "Debug this plugin"));
        }
        */
        public override void OnReload()
        {
            // Un-used, since I'm not doing the commands.
        }
    }
}
