using System;

namespace Loli.DataBase
{
	[Serializable]
	public class UserData
	{
		public int lvl;
        
		public bool trainee;
		public bool helper;
		public bool mainhelper;
		public bool admin;
		public bool mainadmin;
		public bool control;
		public bool maincontrol;
		public bool it;

		public bool found;
		[Newtonsoft.Json.JsonIgnore]
		public DateTime entered = DateTime.Now;
		public string name = "[data deleted]";
		public int id;
		public string discord = "";
		public string login = "";
	}
	internal class DonateRoles
	{
		internal bool Rainbow => _rainbows > 0;

		internal int _rainbows { get; set; }
	}
    
    public class BDDonateRoles
    {
        public int owner { get; set; } = 0;
        public int id { get; set; } = 0;
        public int server { get; set; } = 0;
        public bool freezed { get; set; } = false;
    }
}