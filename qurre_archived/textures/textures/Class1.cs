using Qurre.API;
using Qurre.API.Events;
using System;
using System.Linq;
using UnityEngine;

namespace textures
{
    public class Plugin : Qurre.Plugin
    {
        #region override
        public override string Developer => "fydne";
        public override string Name => "textures";
        public override Version Version => new Version(1, 0, 0);
        public override Version NeededQurreVersion => new Version(1, 0, 0);
        public override void Enable() => RegisterEvents();
        public override void Disable() => UnregisterEvents();
        public override void Reload() { }
        #endregion
        #region Events
        private void RegisterEvents()
        {
            Qurre.Events.Round.WaitingForPlayers += WaitingForPlayers;
            Qurre.Events.Server.SendingRA += Ra;
        }
        private void UnregisterEvents()
        {
            Qurre.Events.Round.WaitingForPlayers -= WaitingForPlayers;
            Qurre.Events.Server.SendingRA -= Ra;
        }
        #endregion
        private void Ra(SendingRAEvent ev)
        {
            if(ev.Name == "texture")
            {
                //GameObject gm = ev.Player.GameObject;
                GameObject gm = Map.SpawnBot(ev.Player.Role, "umm", 100f, ev.Player.Position, ev.Player.Rotation, ev.Player.Scale);
                var obj = gm.GetComponentsInChildren<Renderer>().FirstOrDefault().material.mainTexture;
                string url = "https://scpsl.store/img/kirigiri/kirigiri.png";
                var tex = new Texture2D(obj.width, obj.height);
                WWW www = new WWW(url);
                www.LoadImageIntoTexture(tex);
                Color[] cols = tex.GetPixels(0, 0, tex.width, tex.height);
                float[,] heights = new float[tex.width, tex.height];
                for (int x = 0; x < tex.width; x++)
                {
                    for (int y = 0; y < tex.height; y++)
                    {
                        heights[x, y] = cols[y * tex.width + x].grayscale;
                        //tex.GetPixel(x, y).grayscale;
                        //Debug.Log(tex.GetPixel(x, y).grayscale);
                        //ter.heightmap.Add(hgt);
                    }
                }
                gm.GetComponent<UnityEngine.Terrain>().terrainData.SetHeights(0, 0, heights);
            }
        }
        private void WaitingForPlayers()
        {
        }
    }
}