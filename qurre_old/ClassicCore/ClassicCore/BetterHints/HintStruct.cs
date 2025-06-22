namespace ClassicCore.BetterHints
{
    public class HintStruct
    {
        public int Position { get; private set; }
        public int Voffset { get; private set; }
        public string Message { get; set; }
        public int Duration { get; private set; }
        public bool Static { get; private set; }

        internal HintStruct(int pos, int vf, string msg, int dur, bool stat)
        {
            Position = pos;
            Voffset = vf;
            Message = msg;
            Duration = dur;
            Static = stat;
        }
    }
}