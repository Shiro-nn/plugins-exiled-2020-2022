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
        internal bool Recruitment { get; private set; } = false;
        internal Priest(Player pl)
        {
            List.Add(this);
            this.pl = pl;
            pl.Tag += Tag;
            var color = new Color32(255, 242, 122, 255);
            Glow = new Glow(pl, color);
            Nimb = new Nimb(pl);
        }
        internal void Update()
        {
            if (Player.List.Where(x => x.DistanceTo(pl) < 7).Count() != 0) Recruitment = true;
            else Recruitment = false;
        }
        internal void StartRecruitment(Player target)
        {
            pl.ShowHint($"<b><color=#00ff19>'{target.Nickname}' уверовал в истинного бога,</color></b>\n" +
                "<b><color=#00ff88>благодаря Вам</color></b>", 10);
            Timing.RunCoroutine(MethodDo());
            IEnumerator<float> MethodDo()
            {
                target.ShowHint("<b><color=#00ff19>Вы начали обряд уверования</color></b>", 5);
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
                target.ShowHint("<b><color=#00ff19>Вы уверовали в истинного бога</color></b>\n" +
                    "<b><color=#f47fff>Также, теперь, вы можете участвовать в призыве истинного бога</color></b>", 10);
                lg.Destroy();
                yield break;
            }
        }
        internal void StartCalling()
        {
            var room = pl.Room;
            var rt = room.Type;
            var pos = room.Position + Vector3.up * 2;
            if (!(rt == RoomType.Lcz914 || rt == RoomType.LczCrossing || rt == RoomType.LczGr18 || rt == RoomType.LczTCross ||
                rt == RoomType.HczCrossing || rt == RoomType.HczTCross))
            {
                pl.ShowHint("Данная комната не подходит для призыва бога");
                return;
            }
            pl.EnableEffect(EffectType.Ensnared);
            room.LightsOff(120);
            var color = new Color32(255, 242, 122, 255);
            var lg = new Light(pos + Vector3.up * 10, color, 1, 10);
            var prim = new Primitive(PrimitiveType.Sphere, lg.Position, new Color32(0, 0, 0, 150), default, new Vector3(0.6f, 0.6f, 0.6f));
            prim.Base.transform.parent = lg.Base.transform;
            prim.Base.transform.localPosition = new Vector3(0, 0, 0);
            Timing.RunCoroutine(MethodDo());
            IEnumerator<float> MethodDo()
            {
                for (int i = 100; i > 0; i--)
                {
                    try { lg.Position = pos + Vector3.up * (0.1f * i); } catch { }
                    yield return Timing.WaitForSeconds(0.1f);
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
    }
}