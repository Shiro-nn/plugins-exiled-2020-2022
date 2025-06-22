using Hints;
using ClassicCore.Addons;
using MEC;
using Qurre.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ClassicCore.BetterHints
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
            string ProjectLogo = $"\n<align=center><voffset=-20em><b><size=120%>" +
                $"<color=#ff00c0>⭐</color>" +
                $"<color=#ff00a0>f</color>" +
                $"<color=#ff0080>y</color>" +
                $"<color=#ff0060>d</color>" +
                $"<color=#ff0040>n</color>" +
                $"<color=#ff0020>e</color>" +
                $"<color=#ff0000>⭐</color>" +
                $"</size></color></b></voffset></align>";
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
                    str += WaitText;
                    if (pl.Team == Team.SCP)
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
                    try
                    {
                        if(Scps.Scp294.API.DrinksManager.Drinks.TryGetValue(pl.ItemInHand.Serial, out var drink))
                        {
                            str += $"\n<align=left><voffset=-19><b><color=#F13D3D><size=80%><pos=10%>Предмет в руках: {drink.Name}</pos></size></color></b></voffset></align>";
                        }
                    }
                    catch { }
                    str += ProjectLogo;
                    pl.HintDisplay.Show(new TextHint(str, new HintParameter[] { new StringHintParameter("") }, null, 1.1f));
                    List<string> ChatText()
                    {
                        List<string> text = new();
                        if (Chat.Disables.Contains(pl.UserId)) return text;
                        var msgs = Chat.messages.Where(x => (DateTime.Now - x.Date).TotalMinutes < 1 && ((x.Author == pl && x.Type != Chat.MessageType.Position) ||
                        x.Type == Chat.MessageType.Public ||
                        (x.Type == Chat.MessageType.Position && Vector3.Distance(x.Position, pl.Position) < 10) ||
                        (x.Type == Chat.MessageType.Team && x.Role.GetTeam() == pl.Team) ||
                        (x.Type == Chat.MessageType.Ally && Ally(x.Role.GetTeam())) ||
                        (x.Type == Chat.MessageType.Private && x.PrivateTo == pl))).Skip(Math.Max(0, Chat.messages.Count - 10));
                        foreach (var msg in msgs)
                        {
                            try
                            {
                                string m = "<align=left><pos=-20%><size=70%><alpha=#FF><b>";
                                string color = "white";
                                if (msg.Role.GetTeam() == Team.CDP) color = "#ff9900";
                                else if (msg.Role.GetTeam() == Team.SCP) color = "#ff0000";
                                else if (msg.Role.GetTeam() == Team.RSC) color = "#e2e26d";
                                else if (msg.Role.GetTeam() == Team.TUT) color = "#00ff00";
                                else if (msg.Role == RoleType.ChaosConscript) color = "#58be58";
                                else if (msg.Role == RoleType.ChaosMarauder) color = "#23be23";
                                else if (msg.Role == RoleType.ChaosRepressor) color = "#38ac38";
                                else if (msg.Role == RoleType.ChaosRifleman) color = "#1cac1c";
                                else if (msg.Role == RoleType.FacilityGuard) color = "#afafa1";
                                else if (msg.Role == RoleType.NtfPrivate) color = "#00a5ff";
                                else if (msg.Role == RoleType.NtfCaptain) color = "#0200ff";
                                else if (msg.Role == RoleType.NtfSergeant) color = "#0074ff";
                                else if (msg.Role == RoleType.NtfSpecialist) color = "#1f7fff";
                                if (msg.Type == Chat.MessageType.Public)
                                {
                                    m += $"<color=#f47fff>[Публичный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.Nickname}" +
                                        $"{((msg.Author.DisplayNickname == null || msg.Author.DisplayNickname == "" || msg.Author.DisplayNickname == msg.Author.Nickname) ? "" : $" ({msg.Author.DisplayNickname})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Team)
                                {
                                    m += $"<color=#009267>[Командный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.Nickname}" +
                                        $"{((msg.Author.DisplayNickname == null || msg.Author.DisplayNickname == "" || msg.Author.DisplayNickname == msg.Author.Nickname) ? "" : $" ({msg.Author.DisplayNickname})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Ally)
                                {
                                    m += $"<color=#00ff88>[Союзный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.Nickname}" +
                                        $"{((msg.Author.DisplayNickname == null || msg.Author.DisplayNickname == "" || msg.Author.DisplayNickname == msg.Author.Nickname) ? "" : $" ({msg.Author.DisplayNickname})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Private)
                                {
                                    m += $"<color=#ffae00>[Личный]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.Nickname}" +
                                        $"{((msg.Author.DisplayNickname == null || msg.Author.DisplayNickname == "" || msg.Author.DisplayNickname == msg.Author.Nickname) ? "" : $" ({msg.Author.DisplayNickname})")} " +
                                        $"- {msg.Role}</color>";
                                }
                                else if (msg.Type == Chat.MessageType.Position)
                                {
                                    m += $"<color=#0089c7>[Ближний]</color> <color=red>[{msg.Date:mm:ss}]</color> <color={color}>{msg.Author.Nickname}" +
                                        $"{((msg.Author.DisplayNickname == null || msg.Author.DisplayNickname == "" || msg.Author.DisplayNickname == msg.Author.Nickname) ? "" : $" ({msg.Author.DisplayNickname})")} " +
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
                        }
                        return text;
                        bool Ally(Team team)
                        {
                            var pt = pl.Team;
                            if (team == pt) return true;
                            if ((team == Team.SCP || team == Team.TUT) && (pt == Team.SCP || pt == Team.TUT)) return true;
                            if ((team == Team.MTF || team == Team.RSC) && (pt == Team.MTF || pt == Team.RSC)) return true;
                            if ((team == Team.CHI || team == Team.CDP) && (pt == Team.CHI || pt == Team.CDP)) return true;
                            return false;
                        }
                    }
                }
                catch (Exception e) { Qurre.Log.Error(e); }
            }
            static List<string> ScpText()
            {
                List<string> text = new();
                var scps = Player.List.Where(x => x.Team == Team.SCP).OrderBy(o => o.Role);
                foreach (Player _ in scps) text.Add($"<align=right><size=70%><alpha=#FF><b><color=#ff0000>" +
                    $"{_.Nickname} - {_.Role} ({Math.Round(_.Hp)}/{Math.Round((float)_.MaxHp)}HP)" +
                    $"</color></b></size></align>");
                return text;
            }
        }
    }
}