using Hints;
using Loli.Addons;
using MEC;
using PlayerRoles;
using Qurre.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli.BetterHints
{
    internal static class Manager
    {
        internal static readonly Dictionary<Player, List<HintStruct>> Hints = new();
        internal static readonly Dictionary<Player, List<string>> FixHints = new();
        internal static void ShowHint(Player pl, string text, float dur)
        {
            if (!FixHints.ContainsKey(pl)) FixHints.Add(pl, new List<string>());
            var list = text.Trim().Split('\n');
            foreach (var str in list)
            {
                string _ = str.Replace("\n", "").Trim();
                if (_ == "") continue;
                if (!FixHints.TryGetValue(pl, out var _data)) continue;
                _data.Add(_);
                Timing.CallDelayed(dur, () => { if (_data.Contains(_)) _data.Remove(_); });
            }
        }
        internal static void Cycle()
        {
            List<string> scp = new();
            try { scp = ScpText(); } catch { }
            string WaitText = "";
            string _hackMenu = "";
            string ProjectLogo = $"\n<align=center><voffset=-20em><b><size=120%>" +
                $"<color=#ff00c0>⭐</color>" +
                $"<color=#ff00a0>f</color>" +
                $"<color=#ff0080>y</color>" +
                $"<color=#ff0060>d</color>" +
                $"<color=#ff0040>n</color>" +
                $"<color=#ff0020>e</color>" +
                $"<color=#ff0000>⭐</color>" +
                $"</size></color></b></voffset></align>";
            string RecClan = "";
            try { RecClan = ClansRecommendation.GetText(); } catch { }
            string HackerMenu()
            {
                if (!_hackMenu.IsEmpty()) return _hackMenu;
                string stances = "";
                var _ps = Textures.Models.Panel.Panels.Select(x => x.Value);
                int _count = _ps.Count();
                for (int i = 0; i < _count; i++)
                {
                    var _p = _ps.ElementAt(i);
                    string color = "#00ff19";
                    string cc = "#009c0f";
                    if (_p.Status == HackMode.Hacking)
                    {
                        color = "#ffe200";
                        cc = "#998700";
                    }
                    else if (_p.Status == HackMode.Hacked)
                    {
                        color = "#ff0000";
                        cc = "#880000";
                    }
                    stances += $"<voffset=-{i}em><align=right><pos=50%><b><size=70%>{ProgressBar(_p.Proccess, 100, 10, "█", true, color, cc)}</size></b></pos></align></voffset>\n";
                }
                string control_progress = "<size=55%><color=#d60000>Взломайте все станции безопасности</color></size>";
                if (Textures.Models.Panel.AllHacked)
                {
                    var _stts = Textures.Models.Rooms.Control.Status;
                    var _prcs = Textures.Models.Rooms.Control.Proccess;
                    string color = "#00ff19";
                    string cc = "#009c0f";
                    if (_stts == HackMode.Hacking)
                    {
                        color = "#ffe200";
                        cc = "#998700";
                    }
                    else if (_stts == HackMode.Hacked)
                    {
                        color = "#ff0000";
                        cc = "#880000";
                    }
                    control_progress = ProgressBar(_prcs, 100, 10, "█", true, color, cc);
                }
                string servers_progress = "<size=55%><color=#d60000>Взломайте комнату управления</color></size>";
                if (Textures.Models.Rooms.Control.Proccess >= 100)
                {
                    var _stts = Textures.Models.Rooms.ServersManager.Status;
                    var _prcs = Textures.Models.Rooms.ServersManager.Proccess;
                    string color = "#00ff19";
                    string cc = "#009c0f";
                    if (_stts == HackMode.Hacking)
                    {
                        color = "#ffe200";
                        cc = "#998700";
                    }
                    else if (_stts == HackMode.Hacked)
                    {
                        color = "#ff0000";
                        cc = "#880000";
                    }
                    servers_progress = ProgressBar(_prcs, 100, 10, "█", true, color, cc);
                }
                _hackMenu = $"<line-height=0%>\n<voffset={2}em><align=right><pos=50%><b><size=150%><color=#ff2222>Взлом комплекса</color></size></b></pos></align></voffset>\n\n" +
                $"<voffset={1}em><align=right><pos=50%><b><size=70%><color=#cf1500>Взлом станций безопасности:</color></size></b></pos></align></voffset>\n" +
                $"{stances}\n" +
                $"<voffset={-1 - _count}em><align=right><pos=50%><b><size=70%><color=#ffae00>Взлом комнаты управления:</color></size></b></pos></align></voffset>\n" +
                $"<voffset={-2 - _count}em><align=right><pos=50%><b><size=70%>{control_progress}</size></b></pos></align></voffset>\n\n" +
                $"<voffset={-4 - _count}em><align=right><pos=50%><b><size=70%><color=#4169e1>Взлом основных серверов:</color></size></b></pos></align></voffset>\n" +
                $"<voffset={-5 - _count}em><align=right><pos=50%><b><size=70%>{servers_progress}</size></b></pos></align></voffset>";
                return _hackMenu;
            }
            if (Round.Waiting)
            {
                try
                {
                    int pls = Player.List.Count();
                    string onemsg = $"<voffset=-14em><size=50><color=yellow><b>Раунд будет запущен через {GameCore.RoundStart.singleton.NetworkTimer} секунд</b></color></size></voffset>\n";
                    if (GameCore.RoundStart.singleton.NetworkTimer < 0) onemsg = $"";
                    WaitText = $"{onemsg}<voffset=-12.5em><size=40><i>{pls} игроков</i></size></voffset>";
                }
                catch { }
            }
            string StatsText = "";
            if (Round.Ended)
            {
                StatsText += "\n";
                if (EndStats.Escapes.Count > 0)
                {
                    StatsText += "<align=left><pos=-10%>Самые быстрые побеги:</align></pos>\n";
                    int escapes = 0;
                    foreach (var data in EndStats.Escapes.Take(3))
                    {
                        escapes++;
                        StatsText += $"<align=left><pos=-10%><size=30><i>[<color=#00FFE4>{escapes}</color>] " +
                         $"<color=yellow><b>{data.Key}</b></color> сбежал за <color=#FF9303><b>{data.Value.Minutes}:{data.Value.Seconds}</b></color></i></size></pos></align>\n";
                    }
                }
                if (EndStats.Kills.Count > 0)
                {
                    StatsText += "<align=left><pos=-10%>Больше всего убийств:</align></pos>\n";
                    int _count = 0;
                    foreach (var data in EndStats.Kills.OrderByDescending(x => x.Value).Take(5))
                    {
                        _count++;
                        StatsText += $"<align=left><pos=-10%><size=30><i>[<color=#00FFE4>{_count}</color>] " +
                         $"<color=yellow><b>{data.Key}</b></color> - <color=#ff3400><b>{data.Value}</b></color></i></size></pos></align>\n";
                    }
                }
                if (EndStats.ScpKills.Count > 0)
                {
                    StatsText += "<align=left><pos=-10%>Больше всего убийств за SCP:</align></pos>\n";
                    int _count = 0;
                    foreach (var data in EndStats.ScpKills.OrderByDescending(x => x.Value).Take(5))
                    {
                        _count++;
                        StatsText += $"<align=left><pos=-10%><size=30><i>[<color=#00FFE4>{_count}</color>] " +
                         $"<color=yellow><b>{data.Key}</b></color> - <color=#ff1f00><b>{data.Value}</b></color></i></size></pos></align>\n";
                    }
                }
                if (EndStats.Damages.Count > 0)
                {
                    StatsText += "<align=left><pos=-10%>Больше всего нанесенного урона:</align></pos>\n";
                    int _count = 0;
                    foreach (var data in EndStats.Damages.OrderByDescending(x => x.Value).Take(5))
                    {
                        _count++;
                        StatsText += $"<align=left><pos=-10%><size=30><i>[<color=#00FFE4>{_count}</color>] " +
                         $"<color=yellow><b>{data.Key}</b></color> - <color=#ff1f00><b>{data.Value}HP</b></color></i></size></pos></align>\n";
                    }
                }
            }
            foreach (Player pl in Player.List)
            {
                try
                {
                    int _count = 7;
                    string FormateArr(string str)
                    {
                        string ret = "";
                        var sta = str.Split('\n');
                        foreach (string st in sta)
                        {
                            if (!string.IsNullOrEmpty(st)) ret += $"<voffset={_count + GetVOffset(st)}em>{st}</voffset>";
                            _count -= 1;
                            if (_count < -20) _count = 7;
                        }
                        return ret + "\n";
                    }
                    float GetVOffset(string text)
                    {
                        if (text.Contains("<voffset="))
                        {
                            foreach (string spl in text.Split('>'))
                            {
                                if (spl.Contains("<voffset="))
                                {
                                    return float.Parse(spl.Substring(spl.IndexOf("<voffset=") + 9, spl.Contains("em") ? spl.Length - 11 : spl.Length - 9), System.Globalization.CultureInfo.InvariantCulture);
                                }
                            }
                        }
                        return 0;
                    }
                    string str = "<line-height=0%>";
                    try
                    {
                        if (!Round.Waiting && pl.RoleInfomation.IsHuman && !pl.Tag.Contains(DataBase.Modules.Controllers.Beginner.Tag) &&
                            DataBase.Modules.Controllers.Priest.Get(pl) is null &&
                            DataBase.Modules.Controllers.Priest.List.Where(y => 5 >= pl.DistanceTo(y.pl)).Count() > 0)
                        {
                            str += FormateArr($"<align=right><size=70%><alpha=#FF><b><color=#0089c7>.god</color> <color=#706e6e>(в консоли на [ё])</color></b></size></align>\n" +
                                $"<align=right><size=70%><alpha=#FF><b><color=red>Уверовать в истинного Бога</color></b></size></align>\n");
                        }
                    }
                    catch { }
                    str += WaitText;
                    bool scp_hint = pl.RoleInfomation.IsScp;
                    if (scp_hint)
                    {
                        _count = 7;
                        foreach (var _t in scp) str += FormateArr(_t);
                    }
                    var chats = ChatText();
                    if (chats.Count > 0)
                    {
                        _count = 7;
                        foreach (var _t in chats) str += FormateArr(_t);
                    }
                    List<string> fix = new();
                    try { fix.AddRange(FixHints[pl]); } catch { }
                    if (fix.Count > 0)
                    {
                        _count = 7;
                        for (int i = 0; fix.Count > i; i++)
                            if (fix[i] != "")
                                str += FormateArr(fix[i]);
                    }
                    List<HintStruct> _hs = new();
                    try { _hs.AddRange(Hints[pl]); } catch { }
                    if (_hs.Count > 0)
                    {
                        _count = 0;
                        foreach (var _h in _hs)
                        {
                            str += $"<voffset={(_h.Static ? _h.Voffset : _count + _h.Voffset)}em><pos={_h.Position}%>{_h.Message}</pos></voffset>";
                            _count -= 1;
                            if (_count < -20) _count = 0;
                        }
                    }
                    _count = 7;
                    str += FormateArr(StatsText);
                    if (pl.ItsHacker()) str += HackerMenu();
                    if (pl.RoleInfomation.Role is RoleTypeId.Spectator) str += RecClan;
                    //if (false) str += ClosingSoon(leng);
                    try
                    {
                        if (Scps.Scp294.API.DrinksManager.Drinks.TryGetValue(pl.ItemInHand.Serial, out var drink))
                        {
                            str += $"\n<align=left><voffset=-19><b><color=#F13D3D><size=80%><pos=10%>Предмет в руках: {drink.Name}</pos></size></color></b></voffset></align>";
                        }
                    }
                    catch { }
                    str += ProjectLogo;
                    pl.Client.HintDisplay.Show(new TextHint(str, new HintParameter[] { new StringHintParameter("") }, null, 1.1f));
                    List<string> ChatText()
                    {
                        List<string> text = new();

                        if (Chat.Disables.Contains(pl.UserInfomation.UserId))
                            return text;

                        var msgs = Chat.messages.Where(x => (DateTime.Now - x.Date).TotalMinutes < 1 && ((x.Author == pl && x.Type != Chat.MessageType.Position) ||
                        x.Type == Chat.MessageType.Public ||
                        (x.Type == Chat.MessageType.Position && Vector3.Distance(x.Position, pl.MovementState.Position) < 10) ||
                        (x.Type == Chat.MessageType.Team && x.Role.GetTeam() == pl.GetTeam()) ||
                        (x.Type == Chat.MessageType.Ally && Ally(x.Role.GetTeam())) ||
                        (x.Type == Chat.MessageType.Private && x.PrivateTo == pl)
                        ));

                        foreach (var msg in msgs)
                        {
                            try
                            {
                                string m = "<align=left><pos=-20%><size=70%><alpha=#FF><b>";
                                string color = "white";

                                switch (msg.Role)
                                {
                                    case RoleType.ClassD: color = "#ff9900"; break;
                                    case RoleType.Scientist: color = "#e2e26d"; break;

                                    case RoleType.Tutorial: color = "#00ff00"; break;

                                    case RoleType.ChaosConscript: color = "#58be58"; break;
                                    case RoleType.ChaosMarauder: color = "#23be23"; break;
                                    case RoleType.ChaosRepressor: color = "#38ac38"; break;
                                    case RoleType.ChaosRifleman: color = "#1cac1c"; break;

                                    case RoleType.FacilityGuard: color = "#afafa1"; break;
                                    case RoleType.NtfPrivate: color = "#00a5ff"; break;
                                    case RoleType.NtfSergeant: color = "#0074ff"; break;
                                    case RoleType.NtfCaptain: color = "#0200ff"; break;
                                    case RoleType.NtfSpecialist: color = "#1f7fff"; break;

                                    default:
                                        {
                                            if (msg.Role.GetTeam() == Team.SCPs)
                                                color = "#ff0000";
                                            break;
                                        }
                                }

                                if (msg.Type == Chat.MessageType.Public)
                                {
                                    m += $"<color=#f47fff>[Публичный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.UserInfomation.Nickname}" +
                                        $"{((string.IsNullOrEmpty(msg.Author.UserInfomation.DisplayName) || msg.Author.UserInfomation.DisplayName == msg.Author.UserInfomation.Nickname) ? "" : $" ({msg.Author.UserInfomation.DisplayName})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Team)
                                {
                                    m += $"<color=#009267>[Командный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.UserInfomation.Nickname}" +
                                        $"{((string.IsNullOrEmpty(msg.Author.UserInfomation.DisplayName) || msg.Author.UserInfomation.DisplayName == msg.Author.UserInfomation.Nickname) ? "" : $" ({msg.Author.UserInfomation.DisplayName})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Ally)
                                {
                                    m += $"<color=#00ff88>[Союзный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.UserInfomation.Nickname}" +
                                        $"{((string.IsNullOrEmpty(msg.Author.UserInfomation.DisplayName) || msg.Author.UserInfomation.DisplayName == msg.Author.UserInfomation.Nickname) ? "" : $" ({msg.Author.UserInfomation.DisplayName})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Private)
                                {
                                    m += $"<color=#ffae00>[Личный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.UserInfomation.Nickname}" +
                                        $"{((string.IsNullOrEmpty(msg.Author.UserInfomation.DisplayName) || msg.Author.UserInfomation.DisplayName == msg.Author.UserInfomation.Nickname) ? "" : $" ({msg.Author.UserInfomation.DisplayName})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Position)
                                {
                                    m += $"<color=#0089c7>[Ближний]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.UserInfomation.Nickname}" +
                                        $"{((string.IsNullOrEmpty(msg.Author.UserInfomation.DisplayName) || msg.Author.UserInfomation.DisplayName == msg.Author.UserInfomation.Nickname) ? "" : $" ({msg.Author.UserInfomation.DisplayName})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                m += "</b></size></pos></align>";
                                text.Add(m);
                                text.Add($"<align=left><pos=-20%><size=70%><alpha=#FF><b><color=red>{msg.Text}</color></b></size></pos></align>");
                            }
                            catch { }
                        }
                        if (text.Count > 0)
                        {
                            text.Add("<align=left><pos=-20%><size=70%><alpha=#FF><b><color=#0089c7>----------------------------------------------</color></b></size></pos></align>");
                            text.Add("<align=left><pos=-20%><size=70%><alpha=#FF><b><color=red>.чат отключить - отключить чат</color></b></size></pos></align>");
                            text.Add("<align=center></align>");
                        }
                        return text;

                        bool Ally(Team team)
                        {
                            var pt = pl.GetTeam();

                            if (team == pt)
                                return true;
                            if (team.GetFaction() == pt.GetFaction())
                                return true;

                            return false;
                        }
                    }
                }
                catch (Exception e) { Log.Error(e); }
            }
            static List<string> ScpText()
            {
                List<string> text = new();
                var scps = Player.List.Where(x => x.GetTeam() == Team.SCPs).OrderBy(o => o.RoleInfomation.Role);
                foreach (Player _ in scps) text.Add($"<align=right><size=70%><alpha=#FF><b><color=#ff0000>" +
                    $"{_.UserInfomation.Nickname} - {_.GetCustomRole()} ({Math.Round(_.HealthInfomation.Hp)}/{Math.Round((float)_.HealthInfomation.MaxHp)}HP)" +
                    $"</color></b></size></align>");
                return text;
            }
        }
        private static string ProgressBar(long progress, long total, int chunks = 30, string symbol = "■", bool showPercent = true, string completeColor = "green", string remainingColor = "red")
        {
            if (progress > total) progress = total;
            string str = "[";
            float chunk = (float)chunks / total;

            int complete = (int)Math.Ceiling((double)chunk * progress);
            int remaining = chunks - complete;

            if (0 > remaining) remaining = 0;
            if (0 > complete) complete = 0;

            if (complete > chunks) complete = chunks;
            if (remaining > chunks) remaining = chunks;

            str += $"<color={completeColor}>{Repeat(symbol, complete)}</color>";
            str += $"<color={remainingColor}>{Repeat(symbol, remaining)}</color>";

            str += "]";
            if (showPercent)
            {
                int percent = (int)((float)progress / (float)total * 100);
                if (10 > percent) str += "    ";
                else if (100 > percent) str += "  ";
                str += $" {percent}%";
            }
            return str;
            static string Repeat(string str, int times) => string.Concat(Enumerable.Repeat(str, times));
        }
    }
}