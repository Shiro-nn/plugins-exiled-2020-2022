using Qurre.API;
using Qurre.API.DataBase;
namespace ClassicCore.DataBase
{
	internal class Manager
	{
		internal readonly Init plugin;
		internal static Manager Static;
		internal readonly Modules.Admins Admins;
		internal readonly Modules.Data Data;
		internal readonly Modules.Donate Donate;
		internal readonly Modules.Loader Loader;
		internal readonly Modules.Stats Stats;
		internal readonly Modules.Updater Updater;
		public Manager(Init plugin)
		{
			this.plugin = plugin;
			Loader = new Modules.Loader(this);
			Updater = new Modules.Updater(this);
			Admins = new Modules.Admins(this);
			Data = new Modules.Data(this);
			Donate = new Modules.Donate(this);
			Stats = new Modules.Stats(this);
			Static = this;
		}
		private Database database;
		internal Database DataBase
		{
			get
			{
				if (database == null) database = Server.DataBase.GetDatabase("login");
				return database;
			}
		}
	}
}