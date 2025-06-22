using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loli_Time
{
    public class SendingRAEvent : EventArgs
    {
        private string returnMessage;
        public SendingRAEvent(CommandSender commandSender, string command, string name, string[] args, string prefix = "", bool allowed = true)
        {
            CommandSender = commandSender;
            Command = command;
            Name = name;
            Args = args;
            Allowed = allowed;
            if (prefix == "") pref = Assembly.GetCallingAssembly().GetName().Name;
            else pref = prefix;
        }
        public CommandSender CommandSender { get; }
        public string Command { get; }
        public string Name { get; }
        public string[] Args { get; }
        public string pref;
        public string ReplyMessage
        {
            get => returnMessage;
            set
            {
                if (pref == "") pref = Assembly.GetCallingAssembly().GetName().Name;
                returnMessage = $"{pref}#{value}";
            }
        }
        public string Prefix
        {
            get => pref;
            set => pref = value;
        }
        public bool Success { get; set; } = true;
        public bool Allowed { get; set; }
    }
    public class EffectsSender : CommandSender
    {
        public override void RaReply(string text, bool success, bool logToConsole, string overrideDisplay)
        {
        }

        public override void Print(string text)
        {
        }

        public string Name;
        public string Command;
        public EffectsSender(string name, string com)
        {
            Name = name;
            Command = com;
        }
        public override string SenderId => "Effects Controller";
        public override string Nickname => Name;
        public override ulong Permissions => ServerStatic.GetPermissionsHandler().FullPerm;
        public override byte KickPower => byte.MinValue;
        public override bool FullPermissions => true;
    }
    internal class DonateRoles
    {
        internal bool Rainbow => _rainbows > 0;
        internal bool Prime => _primes > 0;
        internal bool Priest => _priests > 0;
        internal bool Mage => _mages > 0;
        internal bool Sage => _sages > 0;
        internal bool Star => _stars > 0;

        internal int _rainbows { get; set; } = 0;
        internal int _primes { get; set; } = 0;
        internal int _priests { get; set; } = 0;
        internal int _mages { get; set; } = 0;
        internal int _sages { get; set; } = 0;
        internal int _stars { get; set; } = 0;
    }
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
    public class BDDonateRoles
    {
        public int owner { get; set; } = 0;
        public int id { get; set; } = 0;
        public int server { get; set; } = 0;
        public bool freezed { get; set; } = false;
    }
    public class DonateRA
    {
        public bool Force { get; internal set; } = false;
        public bool Give { get; internal set; } = false;
        public bool Effects { get; internal set; } = false;
        public bool ViewRoles { get; internal set; } = false;
    }
    public class BDDonateRA
    {
        public bool force { get; set; } = false;
        public bool give { get; set; } = false;
        public bool effects { get; set; } = false;
        public bool players_roles { get; set; } = false;
        public string prefix { get; set; } = "";
        public string color { get; set; } = "";
    }
    public class RainbowTagController : MonoBehaviour
    {
        private ServerRoles _roles;
        private string _originalColor;

        private int _position = 0;
        private float _nextCycle = 0f;

        public static string[] Colors =
        {
            "pink",
            "red",
            "brown",
            "silver",
            "light_green",
            "crimson",
            "cyan",
            "aqua",
            "deep_pink",
            "tomato",
            "yellow",
            "magenta",
            "blue_green",
            "orange",
            "lime",
            "green",
            "emerald",
            "carmine",
            "nickel",
            "mint",
            "army_green",
            "pumpkin"
        };

        internal static float Interval { get; set; } = 0.5f;


        private void Start()
        {
            _roles = GetComponent<ServerRoles>();
            _nextCycle = Time.time;
            _originalColor = _roles.Network_myColor;
        }


        private void OnDisable()
        {
            _roles.Network_myColor = _originalColor;
        }


        private void Update()
        {
            if (Time.time < _nextCycle) return;
            _nextCycle += Interval;

            _roles.Network_myColor = Colors[_position];

            if (++_position >= Colors.Length)
                _position = 0;
        }
    }
}