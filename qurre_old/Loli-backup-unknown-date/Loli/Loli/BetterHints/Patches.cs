using HarmonyLib;
using Hints;
using Loli.Addons;
using MEC;
using Qurre.API;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Loli.BetterHints
{
    [HarmonyPatch(typeof(Player), nameof(Player.ShowHint), new[] { typeof(string), typeof(float), typeof(HintEffect[]) })]
    internal static class Patches
    {
        internal static bool Prefix(Player __instance, string text, float duration = 1f)
        {
            Manager.ShowHint(__instance, text, duration);
            return false;
        }
    }
    internal static class Manager
    {
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
            Dictionary<int, string> scp = new();
            try { scp = ScpText(); } catch { }
            string WaitText = "";
            string tps(int strings) => $"\n<align=left><voffset={-20 + strings}em><b><color=red><size=80%><pos=10%>TPS: {Plugin.TicksMinutes}" +
                $"</pos></size></color></b></voffset></align>";
            string HackerMenu(int strings)
            {
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
                    stances += $"<voffset={strings - i}em><align=right><pos=50%><b><size=70%>{ProgressBar(_p.Proccess, 100, 10, "█", true, color, cc)}</size></b></pos></align></voffset>\n";
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
                return $"<line-height=0%>\n<voffset={2 + strings}em><align=right><pos=50%><b><size=150%><color=#ff2222>Взлом комплекса</color></size></b></pos></align></voffset>\n\n" +
                $"<voffset={1 + strings}em><align=right><pos=50%><b><size=70%><color=#cf1500>Взлом станций безопасности:</color></size></b></pos></align></voffset>\n" +
                $"{stances}\n" +
                $"<voffset={-1 + strings - _count}em><align=right><pos=50%><b><size=70%><color=#ffae00>Взлом комнаты управления:</color></size></b></pos></align></voffset>\n" +
                $"<voffset={-2 + strings - _count}em><align=right><pos=50%><b><size=70%>{control_progress}</size></b></pos></align></voffset>\n\n" +
                $"<voffset={-4 + strings - _count}em><align=right><pos=50%><b><size=70%><color=#4169e1>Взлом основных серверов:</color></size></b></pos></align></voffset>\n" +
                $"<voffset={-5 + strings - _count}em><align=right><pos=50%><b><size=70%>{servers_progress}</size></b></pos></align></voffset>";
            }
            if (Round.Waiting)
            {
                try
                {
                    int pls = Player.List.Count();
                    string onemsg = $"<voffset=-19em><size=50><color=yellow><b>Раунд будет запущен через {GameCore.RoundStart.singleton.NetworkTimer} секунд</b></color></size></voffset>\n";
                    if (GameCore.RoundStart.singleton.NetworkTimer < 0) onemsg = $"";
                    WaitText = $"{onemsg}<size=40><i>{pls} игроков</i></size>";
                }
                catch { }
            }
            string ClansText = "";
            if (Plugin.ClansWars) ClansText = "<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#ff0000>Данный сервер будет использоваться" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "только для клановых войн</color>" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#00ff19>Есть идеи для карты?</color>" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#ffb500>Вы можете предложить идеи для карты" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "в <color=#0089c7>Discord</color> сервере</color>" +
                    "</b></size></pos></align>\n<align=left><pos=-20%><size=70%><alpha=#FF><b>" +
                    "<color=#f47fff>discord.gg/UCUBU2z</color>" +
                    "</b></size></pos></align>\n";
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
                    string str = "";
                    try { str += $"{ClansWars.Capturing.Map2.GetHint(pl)}"; } catch { }
                    try
                    {
                        if (!Round.Waiting && pl.Team != Team.RIP && pl.Team != Team.SCP && !pl.Tag.Contains(DataBase.Modules.Controllers.Beginner.Tag) &&
                            DataBase.Modules.Controllers.Priest.Get(pl) is null &&
                            DataBase.Modules.Controllers.Priest.List.Where(y => 5 >= pl.DistanceTo(y.pl)).Count() > 0)
                        {
                            str += $"<align=right><size=70%><alpha=#FF><b><color=#0089c7>.god</color> <color=#706e6e>(в консоли на [ё])</color></b></size></align>\n" +
                                $"<align=right><size=70%><alpha=#FF><b><color=red>Уверовать в истинного бога</color></b></size></align>\n";
                        }
                    }
                    catch { }
                    str += WaitText;
                    var chats = ChatText();
                    var fix = FixHints[pl];
                    bool scp_hint = pl.Role.GetTeam() == Team.SCP;
                    for (int i = 0; scp_hint && scp.ContainsKey(i); i++)
                    {
                        str += scp[i];
                        str += "\n";
                    }
                    str += ClansText;
                    for (int i = 0; chats.ContainsKey(i); i++)
                    {
                        str += chats[i];
                        str += "\n";
                    }
                    for (int i = 0; fix.Count > i; i++)
                    {
                        if (fix[i] != "")
                        {
                            str += fix[i];
                            str += "\n";
                        }
                    }
                    str += StatsText;
                    var leng = str.Split('\n').Length;
                    if (pl.ItsHacker() || pl.ItsSpyFacilityManager()) str += HackerMenu(leng);
                    str += tps(leng);
                    pl.HintDisplay.Show(new TextHint(str, new HintParameter[] { new StringHintParameter("") }, null, 1.1f));
                    Dictionary<int, string> ChatText()
                    {
                        Dictionary<int, string> text = new();
                        if (Chat.Disables.Contains(pl.UserId)) return text;
                        var msgs = Chat.messages.Where(x => (DateTime.Now - x.Date).TotalMinutes < 1 && ((x.Author == pl && x.Type != Chat.MessageType.Position) ||
                        x.Type == Chat.MessageType.Public ||
                        (x.Type == Chat.MessageType.Position && Vector3.Distance(x.Position, pl.Position) < 10) ||
                        (x.Type == Chat.MessageType.Team && x.Role.GetTeam() == pl.GetTeam()) ||
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
                                else if (msg.Role == Module.RoleType.ChaosConscript) color = "#58be58";
                                else if (msg.Role == Module.RoleType.ChaosMarauder) color = "#23be23";
                                else if (msg.Role == Module.RoleType.ChaosRepressor) color = "#38ac38";
                                else if (msg.Role == Module.RoleType.ChaosRifleman) color = "#1cac1c";
                                else if (msg.Role == Module.RoleType.FacilityGuard) color = "#afafa1";
                                else if (msg.Role == Module.RoleType.NtfPrivate) color = "#00a5ff";
                                else if (msg.Role == Module.RoleType.NtfCaptain) color = "#0200ff";
                                else if (msg.Role == Module.RoleType.NtfSergeant) color = "#0074ff";
                                else if (msg.Role == Module.RoleType.NtfSpecialist) color = "#1f7fff";
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
                                text.Add(text.Count, m);
                                text.Add(text.Count, $"<align=left><pos=-20%><size=70%><alpha=#FF><b><color=red>{msg.Text}</color></b></size></pos></align>");
                            }
                            catch { }
                        }
                        if (text.Count > 0)
                        {
                            text.Add(text.Count, $"<align=left><pos=-20%><size=70%><alpha=#FF><b><color=#0089c7>----------------------------------------------</color></b></size></pos></align>");
                            text.Add(text.Count, $"<align=left><pos=-20%><size=70%><alpha=#FF><b><color=red>.чат отключить - отключить чат</color></b></size></pos></align>");
                        }
                        return text;
                        bool Ally(Team team)
                        {
                            if (team == pl.GetTeam()) return true;
                            if ((team == Team.SCP || team == Team.TUT) && (pl.GetTeam() == Team.SCP || pl.GetTeam() == Team.TUT)) return true;
                            if ((team == Team.MTF || team == Team.RSC) && (pl.GetTeam() == Team.MTF || pl.GetTeam() == Team.RSC)) return true;
                            if ((team == Team.CHI || team == Team.CDP) && (pl.GetTeam() == Team.CHI || pl.GetTeam() == Team.CDP)) return true;
                            return false;
                        }
                    }
                }
                catch(Exception e) { Qurre.Log.Error(e); }
            }
            static Dictionary<int, string> ScpText()
            {
                Dictionary<int, string> text = new();
                var scps = Player.List.Where(x => x.GetTeam() == Team.SCP).OrderBy(o => o.Role);
                foreach (Player _ in scps) text.Add(text.Count,
                    $"<align=right><size=70%><alpha=#FF><b><color=#ff0000>" +
                    $"{_.Nickname} - {_.GetCustomRole()} ({Math.Round(_.Hp)}/{Math.Round((float)_.MaxHp)}HP)" +
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