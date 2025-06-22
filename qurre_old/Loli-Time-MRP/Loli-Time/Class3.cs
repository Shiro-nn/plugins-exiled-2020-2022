using System;

namespace Loli_Time
{
    [Serializable]
    public class UserData
    {
        public int money;
        public int xp;
        public int lvl;
        public int to;

        public bool donater = false;
        public bool trainee = false;
        public bool helper = false;
        public bool mainhelper = false;
        public bool admin = false;
        public bool mainadmin = false;
        public bool selection = false;
        public bool owner = false;
        public int warnings = 0;

        public string prefix = "";
        public string clan = "";
        public bool found = false;
        [Newtonsoft.Json.JsonIgnore]
        public bool anonym = false;
        [Newtonsoft.Json.JsonIgnore]
        public DateTime entered = DateTime.Now;
        public string name = "[data deleted]";
        public int id = 0;
        public string discord = "";
        public string login = "";
    }
}