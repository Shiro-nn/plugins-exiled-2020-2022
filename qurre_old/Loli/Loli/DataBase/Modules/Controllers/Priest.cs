using Loli.BetterHints;
using MEC;
using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Light = Qurre.API.Controllers.Light;
namespace Loli.DataBase.Modules.Controllers
{
    internal class Priest
    {
        internal static readonly List<Priest> List = new();
        internal const string Tag = "PriestBetterTag";
        private readonly Glow Glow;
        private readonly Nimb Nimb;
        internal readonly Player pl;
        internal static bool Recruitment { get; private set; } = false;
        internal static Room RecRoom { get; private set; }
        internal Priest(Player pl)
        {
            List.Add(this);
            this.pl = pl;
            pl.Tag += Tag;
            Glow = new Glow(pl, new Color32(255, 242, 122, 255));
            Nimb = new Nimb(pl);
        }
        internal void StartRecruitment(Player target)
        {
            pl.ShowHint($"<b><color=#00ff19>'{target.Nickname}' уверовал в истинного Бога,</color></b>\n" +
                "<b><color=#00ff88>благодаря Вам</color></b>", 10);
            Timing.RunCoroutine(MethodDo());
            IEnumerator<float> MethodDo()
            {
                target.ShowHint("<b><color=#00ff19>Вы начали обряд уверования</color></b>", 5);
                target.ShowHint("<b><color=#00ff19>Вы не можете двигаться в процессе уверования</color></b>", 5);
                target.EnableEffect(EffectType.Ensnared);
                var pos = target.Position;
                var color = new Color32(68, 255, 0, 255);
                var lg = new Light(pos + Vector3.up * 10, color, 1, 10);
                for (int i = 100; i > 0; i--)
                {
                    try { lg.Position = pos + Vector3.up * (0.1f * i); } catch { }
                    yield return Timing.WaitForSeconds(0.1f);
                }
                new Beginner(target);
                target.DisableEffect(EffectType.Ensnared);
                target.ShowHint("<b><color=#00ff19>Вы уверовали в истинного Бога</color></b>\n" +
                    "<b><color=#f47fff>Также, теперь, вы можете участвовать в призыве истинного Бога</color></b>", 10);
                lg.Destroy();
                yield break;
            }
        }
        internal void StartCalling()
        {
            if (Recruitment)
            {
                pl.Hint(new(0, 2, "<color=#00dd15>Истинный Бог уже призван</color>", 10, false));
                return;
            }
            var room = pl.Room;
            var rt = room.Type;
            var pos = room.Position + Vector3.up * 2;
            if (!(rt == RoomType.Lcz914 || rt == RoomType.LczCrossing || rt == RoomType.LczGr18 || rt == RoomType.LczTCross ||
                rt == RoomType.HczCrossing || rt == RoomType.HczTCross || rt == RoomType.EzCrossing))
            {
                pl.Hint(new(0, 2, "<color=#ff0000>Данная комната не подходит для призыва бога</color>", 10, false));
                return;
            }
            var bgns = Beginner.List.Where(x => Vector3.Distance(x.pl.Position, room.Position) < 11);
            if (bgns.Count() < 5)
            {
                pl.Hint(new(0, 2, "<color=#ff0000>Для призыва Истинного Бога, необходимо собрать 5 последователей</color>", 10, false));
                pl.Hint(new(0, 2, "<color=#ff0000>в радиусе 10-ти метров от центра комнаты призыва</color>", 10, false));
                pl.Hint(new(0, 2, $"<color=#ff0000>({bgns.Count()}/5)</color>", 10, false));
                return;
            }
            float time = Mathf.Round(120 * 5 / bgns.Count());
            Color32 color = new(255, 242, 122, 255);
            room.LightsOff(time);
            room.Lights.Color = color;
            room.Lights.LockChange = true;
            foreach (var bg in bgns)
            {
                bg.pl.Hint(new(0, 2, $"<color=#ffe000>Начат призыв Истинного Бога, он продлится {time} секунд</color>", 10, false));
                bg.pl.EnableEffect(EffectType.Ensnared);
            }
            pl.Hint(new(0, 2, $"<color=#ffe000>Начат призыв Истинного Бога, он продлится {time} секунд</color>", 10, false));
            pl.EnableEffect(EffectType.Ensnared);
            Recruitment = true;
            RecRoom = room;
            var lg = new Light(pos + Vector3.up * 10, color, 1, 10);
            var prim = new Primitive(PrimitiveType.Sphere, lg.Position, new Color32(0, 0, 0, 150), default, new Vector3(0.6f, 0.6f, 0.6f));
            prim.Base.transform.parent = lg.Base.transform;
            prim.Base.transform.localPosition = new Vector3(0, 0, 0);
            Timing.KillCoroutines("GodPriestCor");
            Timing.RunCoroutine(MethodDo(), "GodPriestCor");
            IEnumerator<float> MethodDo()
            {
                int round = Round.CurrentRound;
                yield return Timing.WaitForSeconds(Mathf.Max(time - 11, 0));
                for (int i = 100; i > 0; i--)
                {
                    try { lg.Position = pos + Vector3.up * (0.1f * i); } catch { }
                    yield return Timing.WaitForSeconds(0.1f);
                }
                try { pl.DisableEffect(EffectType.Ensnared); } catch { }
                try { pl.Hint(new(0, 2, "<color=#00dd15>Истинный Бог призван</color>", 20, false)); } catch { }
                try { pl.Hint(new(0, 2, "<color=#00dd15>Вы не получаете урон около него</color>", 20, false)); } catch { }
                try { pl.Hint(new(0, 2, "<color=#00dd15>А также он дарует Вам свое благословение</color>", 20, false)); } catch { }
                try { pl.Hint(new(0, 2, "<color=#00dd15>(хилит около себя, если у вас ничего в руках нету)</color>", 20, false)); } catch { }
                try { pl.Hint(new(0, 2, "<color=#00dd15>(*Бог - это игрок и шарик бога)</color>", 20, false)); } catch { }
                foreach (var bg in bgns)
                {
                    try { bg.pl.DisableEffect(EffectType.Ensnared); } catch { }
                    try { bg.pl.Hint(new(0, 2, "<color=#00dd15>Истинный Бог призван</color>", 20, false)); } catch { }
                    try { bg.pl.Hint(new(0, 2, "<color=#00dd15>Вы не получаете урон около него</color>", 20, false)); } catch { }
                    try { bg.pl.Hint(new(0, 2, "<color=#00dd15>А также он дарует Вам свое благословение</color>", 20, false)); } catch { }
                    try { bg.pl.Hint(new(0, 2, "<color=#00dd15>(хилит около себя, если у вас ничего в руках нету)</color>", 20, false)); } catch { }
                    try { bg.pl.Hint(new(0, 2, "<color=#00dd15>(*Бог - это игрок и шарик бога)</color>", 20, false)); } catch { }
                }
                try { Scps.God.SpawnPriest(pl); } catch { }
                while (Round.CurrentRound == round)
                {
                    try
                    {
                        foreach (var dat in List)
                            if (Vector3.Distance(dat.pl.Position, RecRoom.Position) < 6)
                                dat.pl.Heal(2, false);
                    }
                    catch { }
                    try
                    {
                        foreach (var dat in Beginner.List)
                            if (Vector3.Distance(dat.pl.Position, RecRoom.Position) < 6)
                                dat.pl.Heal(1, false);
                    }
                    catch { }
                    try
                    {
                        if (Scps.God.GodPlayer != null)
                        {
                            var pos = Scps.God.GodPlayer.Position;
                            try
                            {
                                foreach (var dat in List)
                                    if (Vector3.Distance(dat.pl.Position, pos) < 6)
                                        dat.pl.Heal(2, false);
                            }
                            catch { }
                            try
                            {
                                foreach (var dat in Beginner.List)
                                    if (Vector3.Distance(dat.pl.Position, pos) < 6)
                                        dat.pl.Heal(1, false);
                            }
                            catch { }
                        }
                    }
                    catch { }
                    yield return Timing.WaitForSeconds(1f);
                }
                yield break;
            }
        }
        internal void Break()
        {
            if (List.Contains(this)) List.Remove(this);
            try { pl.Tag = pl.Tag.Replace(Tag, ""); } catch { }
            try { Glow.Destroy(); } catch { }
            try { Nimb.Destroy(); } catch { }
        }
        internal static Priest Get(Player pl)
        {
            var _list = List.Where(x => x.pl == pl);
            if (_list.Count() > 0) return _list.First();
            return null;
        }

        static internal void Damage(Qurre.API.Events.DamageProcessEvent ev)
        {
            if (!ev.Allowed) return;
            if (!Recruitment) return;
            if (!List.Any(x => x.pl.UserId == ev.Target.UserId) || !Beginner.List.Any(x => x.pl.UserId == ev.Target.UserId)) return;
            if (Vector3.Distance(ev.Target.Position, RecRoom.Position) > 11) return;
            ev.Amount /= 10;
        }
        static internal void NoPriestTk(Qurre.API.Events.DamageProcessEvent ev)
        {
            if (!ev.Allowed) return;
            if (!List.Any(x => x.pl.UserId == ev.Target.UserId) || !Beginner.List.Any(x => x.pl.UserId == ev.Attacker.UserId)) return;
            ev.Amount /= 3;
        }
        static internal void No106(Qurre.API.Events.PocketEnterEvent ev)
        {
            if (!ev.Allowed) return;
            if (!Recruitment) return;
            if (!List.Any(x => x.pl.UserId == ev.Player.UserId) || !Beginner.List.Any(x => x.pl.UserId == ev.Player.UserId)) return;
            if (Vector3.Distance(ev.Player.Position, RecRoom.Position) > 11) return;
            ev.Allowed = false;
        }
        static internal void NoSCPs(Qurre.API.Events.ScpAttackEvent ev)
        {
            if (!ev.Allowed) return;
            if (!Recruitment) return;
            if (!List.Any(x => x.pl.UserId == ev.Target.UserId) || !Beginner.List.Any(x => x.pl.UserId == ev.Target.UserId)) return;
            if (Vector3.Distance(ev.Target.Position, RecRoom.Position) > 11) return;
            int rand = Random.Range(0, 100);
            if (rand < 10) ev.Allowed = false;
        }
    }
}