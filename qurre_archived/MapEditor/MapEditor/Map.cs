using System.Collections.Generic;
namespace MapEditor
{
    public class Map
    {
        public string MapName { get; set; } = "None";
        public List<ushort> LoadOnStart { get; set; } = new List<ushort>();
        public List<MapObject> objects { get; set; } = new List<MapObject>();
    }
}