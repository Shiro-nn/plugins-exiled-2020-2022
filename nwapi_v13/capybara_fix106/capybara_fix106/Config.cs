namespace capybara_fix106
{
    public sealed class Config
    {
        public string BroadCast { get; set; } = "Вы появитесь за SCP-106 через {0} секунд";
        public ushort BroadCastSeconds { get; set; } = 10;

        public byte RespawnSecs { get; set; } = 2;
    }
}