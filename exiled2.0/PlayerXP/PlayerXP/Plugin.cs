using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using HarmonyLib;
using MongoDB.Driver;

namespace PlayerXP
{
    public class Plugin : Plugin<Config>
    {
        internal Config config;
        internal static Config sconfig;
        public EventHandlers EventHandlers;
        internal static MongoClient Client;
        #region override
        public override PluginPriority Priority { get; } = PluginPriority.Last;
        public override string Author { get; } = "fydne";
        public override void OnEnabled()
        {
            Client = new MongoClient(Config.MongoURL);
        }
        public override void OnDisabled() => base.OnDisabled();
        public override void OnRegisteringCommands()
        {
            base.OnRegisteringCommands();
            cfg1();
            RegisterEvents();
        }
        public override void OnUnregisteringCommands()
        {
            base.OnUnregisteringCommands();
            UnregisterEvents();
        }
        #endregion
        #region cfg
        internal void cfg1()
        {
            config = base.Config;
            sconfig = base.Config;
        }
        #endregion
        private Harmony hInstance;


        private void RegisterEvents()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += EventHandlers.OnConsoleCommand;

            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.Escaping += EventHandlers.OnCheckEscape;
            Exiled.Events.Handlers.Player.Dying += EventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension += EventHandlers.OnPocketDimensionDie;
            hInstance = new Harmony("fydne.playerxp");
            hInstance.PatchAll();
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= EventHandlers.OnConsoleCommand;

            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnPlayerJoin;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnPlayerSpawn;
            Exiled.Events.Handlers.Player.Escaping -= EventHandlers.OnCheckEscape;
            Exiled.Events.Handlers.Player.Dying -= EventHandlers.OnPlayerDeath;
            Exiled.Events.Handlers.Player.FailingEscapePocketDimension -= EventHandlers.OnPocketDimensionDie;
            EventHandlers = null;
            hInstance.UnpatchAll(null);
        }
    }
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string MongoURL { get; set; } = "";
        public string Lvl { get; set; } = "LVL";
        /// <summary>
        /// �������� �������.
        /// </summary>
        [Description("�������� �������")]
        public string Pn { get; set; } = "�������";
        /// <summary>
        /// Join msg.
        /// </summary>
        [Description("Join msg")]
        public string Jm { get; set; } = "<i>���� �� �������� � ����</i> <color=orange><b>#�������</b></color>\n<i>�� �� ������ �������� � 2 ���� ������ �����!</i>";
        /// <summary>
        /// lvl up bc.
        /// </summary>
        [Description("lvl up bc")]
        public string Lvlup { get; set; } = "<i>�� ��������</i> <color=orange><b>%lvl% �������!</b></color> <i>�� ���������� ������ ��� �� �������</i> <color=orange><b>%to.xp% XP</b></color>";
        /// <summary>
        /// Escape bc.
        /// </summary>
        [Description("Escape bc")]
        public string Eb { get; set; } = "<i> �� ��������</i> <b><color=orange>%xp%xp</color></b> <i>�� �����</i>";
        /// <summary>
        /// kill bc.
        /// </summary>
        [Description("kill bc")]
        public string Kb { get; set; } = "<i> �� ��������</i> <b><color=orange>%xp%xp</color></b> <i>�� ��������</i> <b><color=red>%player%</color></b>";
        /// <summary>
        /// console command.
        /// </summary>
        [Description("console command")]
        public string Cc { get; set; } = "level";
        /// <summary>
        /// Prefixs.
        /// </summary>
        [Description("Prefixs")]
        public string Prefixs { get; set; } = "1:������,2:������ �� �����,3:���-�������,4:��� �����,5:����,6:�����,7:���� �����,8:�������� �����,9:�������� �����,10:������ �����,11:������ �����,12:������ �����,13:������,14:������,15:���������,16:���������,17:����� ��������,18:����� ��������,19:����� ��������,20:������,21:������,22:������,23:������,24:������,25:��������,26:��������,27:��������,28:��������,29:��������,30:����,31:����,32:����,33:����,34:����,35:������ �����,36:������ �����,37:������ �����,38:������ �����,39:������ �����,40:��������,41:��������,42:��������,43:��������,44:��������,45:���� ������,46:���� ������,47:���� ������,48:���� ������,49:���� ������,50:������� �����,51:������� �����,52:������� �����,53:������� �����,54:������� �����,55:���������� ������,56:���������� ������,57:���������� ������,58:���������� ������,59:���������� ������,60:����������,61:����������,62:����������,63:����������,64:����������,65:����������,66:����������,67:����������,68:����������,69:����������,70:�������� ���,71:�������� ���,72:�������� ���,73:�������� ���,74:�������� ���,75:�������� ���,76:�������� ���,77:�������� ���,78:�������� ���,79:�������� ���,80:�������,81:�������,82:�������,83:�������,84:�������,85:�������,86:�������,87:�������,88:�������,89:�������,90:����������,91:����������,92:����������,93:����������,94:����������,95:����������,96:����������,97:����������,98:����������,99:����������,100:���";
    }
}