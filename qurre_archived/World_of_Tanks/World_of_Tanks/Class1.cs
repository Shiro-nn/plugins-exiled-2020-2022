using System.IO;
using UnityEngine;
namespace World_of_Tanks
{
    public class Class1 : Qurre.Plugin
    {
        public override void Enable()
        {
            Qurre.Events.Round.Waiting += Waiting;
        }
        public override void Disable()
        {
            Qurre.Events.Round.Waiting -= Waiting;
        }
        private void Waiting()
        {
            Qurre.Log.Info("Za Наших");
            string loc = Path.Combine(Qurre.PluginManager.PluginsDirectory, "Schemes", "Tank.json");
            SchematicUnity.API.SchematicManager.LoadSchematic(loc, new Vector3(189.84f, 992.4335f, -75.58f), Quaternion.Euler(new Vector3(0, -97)));
        }
    }
}