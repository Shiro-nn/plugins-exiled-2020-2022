using Qurre.API;
using Qurre.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClassicCore.Modules
{
    static internal class Main
    {
        static internal void Waiting()
        {
            try { GameObject.Find("StartRound").transform.localScale = Vector3.zero; } catch { }
        }
        static internal void AutoEndZeroPlayers()
        {
            if (Round.Started && !Round.Ended && !Player.List.Any(x => true)) Round.Restart();
        }
    }
}