namespace auto_donate
{
    internal static class Configs
	{
		internal static string ServerName { get; set; } = "NoRules";
		internal static string Donates { get; set; } = "1:test,2:admin";
		internal static string MongoURL { get; set; } = "";
		internal static string WebIp { get; set; } = "localhost";
		internal static int ServerID { get; set; } = 0;
		internal static void Reload()
        {
			Plugin.Config.Reload();
			ServerName = Plugin.Config.GetString("AutoDonate_ServerName", ServerName, "Название сервера");
			Donates = Plugin.Config.GetString("AutoDonate_Donates", Donates, "список донатов > id:роль");
			MongoURL = Plugin.Config.GetString("AutoDonate_MongoURL", MongoURL, "ссылка-подключение к mongodb");
			WebIp = Plugin.Config.GetString("AutoDonate_WebIp", WebIp, "ip сайта, указанный в конфиге");
			ServerID = Plugin.Config.GetInt("AutoDonate_ServerID", ServerID, "ID сервера / должен совпадать с id на сайте");
		}
	}
}