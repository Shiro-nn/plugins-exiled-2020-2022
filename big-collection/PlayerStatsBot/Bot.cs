// Decompiled with JetBrains decompiler
// Type: PlayerStatsBot.Bot
// Assembly: PlayerStatsBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 24C98175-4615-4B8E-8ED3-8F72265ADCF9
// Assembly location: I:\PlayerStats\PlayerStatsBot.exe

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace PlayerStatsBot
{
    public class Bot
    {
        private readonly Deserializer yamlDeserializer = new Deserializer();
        private static DiscordSocketClient _client;
        private readonly Program program;

        private static DiscordSocketClient Client
        {
            get
            {
                return Bot._client ?? (Bot._client = new DiscordSocketClient());
            }
        }

        public Bot(Program program)
        {
            this.program = program;
            this.InitBot().GetAwaiter().GetResult();
        }

        private async Task InitBot()
        {
            string status = "*help";
            await Bot.Client.SetStatusAsync(UserStatus.Idle);
            await Bot.Client.SetActivityAsync((IActivity)new Game(status, ActivityType.Playing));
            Bot.Client.Log += new Func<LogMessage, Task>(Program.Log);
            Bot.Client.MessageReceived += new Func<SocketMessage, Task>(this.OnMessageReceived);
            await Bot.Client.LoginAsync(Discord.TokenType.Bot, this.program.Config.BotToken, true);
            await Bot.Client.StartAsync();
            if (!System.IO.File.Exists(this.program.Config.SyncFile))
                System.IO.File.Create(this.program.Config.SyncFile).Close();
            await Task.Delay(-1);
        }

        private async Task OnMessageReceived(SocketMessage msg)
        {
            if (!msg.Content.StartsWith(this.program.Config.BotPrefix))
                return;
            CommandContext context = new CommandContext((IDiscordClient)Bot.Client, (IUserMessage)msg);
            this.HandleCommand((ICommandContext)context);
        }

        private async Task HandleCommand(ICommandContext context)
        {
            if (context.Message.Content.ToLower().Contains("hmm"))
            {
            }
            else if (context.Message.Content.ToLower().Contains("help"))
                await this.helpmsg(context);
            else if (context.Message.Content.ToLower().Contains("desync"))
                await this.DoDesyncAsync(context);
            else if (context.Message.Content.ToLower().Contains("sync"))
            {
                await this.DoSyncAsync(context);
            }
            else
            {
                if (!context.Message.Content.ToLower().Contains("stats"))
                    return;
                if (context.Message.Content.ToLower().Contains("tesla"))
                    await this.CheckTeslaStats(context);
                else
                    await this.CheckStats(context);
            }
        }

        private async Task DoSyncAsync(ICommandContext context)
        {
            string[] args = context.Message.Content.Split(' ');
            try
            {
                if (!string.IsNullOrEmpty(System.IO.File.ReadAllText(this.program.Config.SyncFile)))
                {
                    StreamReader reader = new StreamReader(this.program.Config.SyncFile);
                    StringReader input = new StringReader(reader.ReadToEnd());
                    reader.Close();
                    YamlStream yaml = new YamlStream();
                    yaml.Load((TextReader)input);
                    YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                    Sync[] syncs = mapping.Children.Select<KeyValuePair<YamlNode, YamlNode>, Sync>((Func<KeyValuePair<YamlNode, YamlNode>, Sync>)(entry => new Sync()
                    {
                        DiscordId = ulong.Parse(entry.Key.ToString()),
                        SteamId = entry.Value.ToString()
                    })).ToArray<Sync>();
                    if (syncs.Length != 0 && ((IEnumerable<Sync>)syncs).Any<Sync>((Func<Sync, bool>)(s => (long)s.DiscordId == (long)context.Message.Author.Id)))
                    {
                        IUserMessage userMessage1 = await context.Channel.SendMessageAsync("Ваш Discord уже синхронизирован!", false, (Embed)null, (RequestOptions)null);
                    }
                    else if (args.Length == 1 || !ulong.TryParse(args[1], out ulong _))
                    {
                        IUserMessage userMessage2 = await context.Channel.SendMessageAsync("Пожалуйста, предоставьте действительный SteamID.", false, (Embed)null, (RequestOptions)null);
                    }
                    else
                    {
                        string newSync = ((long)context.Message.Author.Id).ToString() + ": '" + args[1] + "@steam'";
                        System.IO.File.AppendAllText(this.program.Config.SyncFile, newSync + Environment.NewLine);
                        IUserMessage userMessage3 = await context.Channel.SendMessageAsync("@" + context.Message.Author?.ToString() + " Ваш Discord теперь синхронизируется с предоставленным SteamID!", false, (Embed)null, (RequestOptions)null);
                        newSync = (string)null;
                    }
                    reader = (StreamReader)null;
                    input = (StringReader)null;
                    yaml = (YamlStream)null;
                    mapping = (YamlMappingNode)null;
                    syncs = (Sync[])null;
                }
                else
                {
                    string newSync = ((long)context.Message.Author.Id).ToString() + ": '" + args[1] + "@steam'";
                    System.IO.File.AppendAllText(this.program.Config.SyncFile, newSync + Environment.NewLine);
                    IUserMessage userMessage = await context.Channel.SendMessageAsync("@" + context.Message.Author?.ToString() + " Ваш Discord теперь синхронизируется с предоставленным SteamID!", false, (Embed)null, (RequestOptions)null);
                    newSync = (string)null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
            }
        }

        private async Task DoDesyncAsync(ICommandContext context)
        {
            StreamReader reader = new StreamReader(this.program.Config.SyncFile);
            StringReader input = new StringReader(reader.ReadToEnd());
            reader.Close();
            YamlStream yaml = new YamlStream();
            yaml.Load((TextReader)input);
            YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            Sync[] syncs = mapping.Children.Select<KeyValuePair<YamlNode, YamlNode>, Sync>((Func<KeyValuePair<YamlNode, YamlNode>, Sync>)(entry => new Sync()
            {
                DiscordId = ulong.Parse(entry.Key.ToString()),
                SteamId = entry.Value.ToString()
            })).ToArray<Sync>();
            foreach (Sync sync in syncs)
            {
                Console.WriteLine(((long)sync.DiscordId).ToString() + ": " + sync.SteamId);
            }
            if (((IEnumerable<Sync>)syncs).All<Sync>((Func<Sync, bool>)(s => (long)s.DiscordId != (long)context.Message.Author.Id)))
            {
                IUserMessage userMessage1 = await context.Channel.SendMessageAsync("Ваш Discord не синхронизирован!", false, (Embed)null, (RequestOptions)null);
            }
            else
            {
                string[] oldLines = System.IO.File.ReadAllLines(this.program.Config.SyncFile);
                IEnumerable<string> newLines = ((IEnumerable<string>)oldLines).Where<string>((Func<string, bool>)(l => !l.Contains(context.Message.Author.Id.ToString())));
                System.IO.File.WriteAllLines(this.program.Config.SyncFile, newLines);
                IUserMessage userMessage2 = await context.Channel.SendMessageAsync("@" + context.Message.Author?.ToString() + " Ваш Discord теперь не синхронизирован!", false, (Embed)null, (RequestOptions)null);
                oldLines = (string[])null;
                newLines = (IEnumerable<string>)null;
            }
        }

        private async Task CheckTeslaStats(ICommandContext context)
        {
            string path = "/home/scp/scp_server/tesla.txt";
            if (!System.IO.File.Exists(path))
            {
                IUserMessage userMessage1 = await context.Channel.SendMessageAsync("Ошибка: файл статистики Tesla не найден!", false, (Embed)null, (RequestOptions)null);
            }
            else
            {
                string[] read = System.IO.File.ReadAllLines(path);
                Tesla tesla = new Tesla()
                {
                    Fires = int.Parse(read[0]),
                    Kills = int.Parse(read[1]),
                    Dboikills = int.Parse(read[2]),
                    NerdKills = int.Parse(read[3]),
                    CiKills = int.Parse(read[4]),
                    NtfKills = int.Parse(read[5]),
                    ScpKills = int.Parse(read[6])
                };
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithColor(Color.Blue);
                embed.WithFooter("fydne", (string)null);
                embed.WithCurrentTimestamp();
                embed.WithTitle("Статистика Tesla");
                embed.AddField("Активна", (object)tesla.Fires, true);
                embed.AddField("Всего убийств", (object)tesla.Kills, true);
                embed.AddField("Убийств D", (object)tesla.Dboikills, true);
                embed.AddField("Убийств ученых", (object)tesla.NerdKills, true);
                embed.AddField("Убийств Хаоситов", (object)tesla.CiKills, true);
                embed.AddField("Убийств МТФ", (object)tesla.NtfKills, true);
                embed.AddField("Убийств SCP", (object)tesla.ScpKills, true);
                IUserMessage userMessage2 = await context.Channel.SendMessageAsync((string)null, false, embed.Build(), (RequestOptions)null);
            }
        }

        private async Task helpmsg(ICommandContext context)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithColor(Color.Blue);
            embed.WithFooter("fydne", (string)null);
            embed.WithCurrentTimestamp();
            embed.WithTitle("Команды бота");
            embed.AddField("*help", "Текущая команда", true);
            embed.AddField("*sync", "Синхронизовать аккаунт Discord с аккаунтом Steam", true);
            embed.AddField("*desync", "Удалить синхронизацию", true);
            embed.AddField("*tesla", "Посмотреть статистику tesla", true);
            embed.AddField("*stats", "Посмотреть статистику игрока", true);
            embed.AddField("Например:", "*stats 76561198840787587 | *stats ||если сделали синхронизацию||", true);
            IUserMessage userMessage1 = await context.Channel.SendMessageAsync((string)null, false, embed.Build(), (RequestOptions)null);
        }

        private async Task CheckStats(ICommandContext context)
        {
            try
            {
                StreamReader reader = new StreamReader(this.program.Config.SyncFile);
                StringReader input = new StringReader(reader.ReadToEnd());
                reader.Close();
                YamlStream yaml = new YamlStream();
                yaml.Load((TextReader)input);
                YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                Sync[] syncs = mapping.Children.Select<KeyValuePair<YamlNode, YamlNode>, Sync>((Func<KeyValuePair<YamlNode, YamlNode>, Sync>)(entry => new Sync()
                {
                    DiscordId = ulong.Parse(entry.Key.ToString()),
                    SteamId = entry.Value.ToString()
                })).ToArray<Sync>();
                string[] args = context.Message.Content.Split(new string[1]
                {
          " "
                }, StringSplitOptions.None);
                Console.WriteLine(args.Length);
                Stats stats;
                EmbedBuilder embed;
                string steamId;
                string path;
                string steamName;
                ulong result1;
                if (args.Length > 1 && !ulong.TryParse(args[1], out result1))
                {
                    string name = args[1];
                    if (args[1].Contains("@"))
                    {
                        name = args[1].Replace("@", "");
                        name = name.Replace("<", "");
                        name = name.Replace(">", "");
                        if (name.Contains("!"))
                            name = name.Replace("!", "");
                    }
                    ulong result;
                    IEnumerable<IGuildUser> users = context.Guild.GetUsersAsync(CacheMode.CacheOnly, (RequestOptions)null).Result.Where<IGuildUser>((Func<IGuildUser, bool>)(u => !ulong.TryParse(name, out result) ? u.Username == name : (long)u.Id == (long)result));
                    IGuildUser user = users.OrderBy<IGuildUser, int>((Func<IGuildUser, int>)(u => u.Username.Length)).First<IGuildUser>();
                    if (user != null)
                    {
                        ulong id = user.Id;
                        if (((IEnumerable<Sync>)syncs).All<Sync>((Func<Sync, bool>)(s => (long)s.DiscordId != (long)id)))
                        {
                            IUserMessage userMessage1 = await context.Channel.SendMessageAsync("Пользователь не синхронизирован!", false, (Embed)null, (RequestOptions)null);
                        }
                        else
                        {
                            steamId = syncs[Array.FindIndex<Sync>(syncs, (Predicate<Sync>)(s => (long)s.DiscordId == (long)id))].SteamId;
                            path = this.program.Config.StatsFile + "/" + steamId + ".txt";
                            Console.WriteLine(path);
                            if (!System.IO.File.Exists(path))
                            {
                                IUserMessage userMessage2 = await context.Channel.SendMessageAsync("Статистика не найдена.", false, (Embed)null, (RequestOptions)null);
                            }
                            else
                            {
                                steamName = await Bot.GetSteamName(steamId.Replace("@steam", ""));
                                stats = Bot.DeserializeStats(path);
                                embed = new EmbedBuilder();
                                if (string.IsNullOrEmpty(stats.LastKiller))
                                    stats.LastKiller = "Отсутствует";
                                if (string.IsNullOrEmpty(stats.LastVictim))
                                    stats.LastVictim = "Отсутствует";
                                embed.WithTitle("Статистика: " + steamName + "(" + steamId + ")");
                                embed.AddField("Всего убийств", (object)stats.Kills, true);
                                embed.AddField("Убийств SCP", (object)stats.ScpKills, true);
                                embed.AddField("Смертей", (object)stats.Deaths, true);
                                embed.AddField("Самоубийств", (object)stats.Suicides, true);
                                embed.AddField("Выпито SCP-207", (object)stats.Scp207Uses, true);
                                embed.AddField("Кинуто SCP-018", (object)stats.Scp018Throws, true);
                                embed.AddField("Последний убийца", (object)stats.LastKiller, true);
                                embed.AddField("Последняя жертва", (object)stats.LastVictim, true);
                                embed.AddField("УС", (object)stats.Krd, true);
                                embed.AddField("Сбежал", (object)stats.Escapes, true);
                                embed.AddField("Раундов сыграно", (object)stats.RoundsPlayed, false);
                                embed.AddField("На серверах", (object)Bot.GetHRT(stats.SecondsPlayed), false);
                                embed.WithFooter((Action<EmbedFooterBuilder>)(f => f.Text = "Статистика fydne")).WithColor(Color.Blue).WithCurrentTimestamp();
                                IUserMessage userMessage3 = await context.Channel.SendMessageAsync((string)null, false, embed.Build(), (RequestOptions)null);
                            }
                        }
                    }
                    else
                    {
                        IUserMessage userMessage = await context.Channel.SendMessageAsync("Игрок не найден.", false, (Embed)null, (RequestOptions)null);
                    }
                    users = (IEnumerable<IGuildUser>)null;
                    user = (IGuildUser)null;
                }
                else if (args.Length > 1 && ulong.TryParse(args[1], out result1))
                {
                    steamId = args[1] + "@steam";
                    path = this.program.Config.StatsFile + "/" + steamId + ".txt";
                    Console.WriteLine(path);
                    if (!System.IO.File.Exists(path))
                    {
                        IUserMessage userMessage1 = await context.Channel.SendMessageAsync("Статистика не найдена.", false, (Embed)null, (RequestOptions)null);
                    }
                    else
                    {
                        steamName = await Bot.GetSteamName(steamId.Replace("@steam", ""));
                        stats = Bot.DeserializeStats(path);
                        embed = new EmbedBuilder();
                        if (string.IsNullOrEmpty(stats.LastKiller))
                            stats.LastKiller = "Отсутствует";
                        if (string.IsNullOrEmpty(stats.LastVictim))
                            stats.LastVictim = "Отсутствует";
                        embed.WithTitle("Статистика: " + steamName + "(" + steamId + ")");
                        embed.AddField("Всего убийств", (object)stats.Kills, true);
                        embed.AddField("Убийств SCP", (object)stats.ScpKills, true);
                        embed.AddField("Смертей", (object)stats.Deaths, true);
                        embed.AddField("Самоубийств", (object)stats.Suicides, true);
                        embed.AddField("Выпито SCP-207", (object)stats.Scp207Uses, true);
                        embed.AddField("Кинуто SCP-018", (object)stats.Scp018Throws, true);
                        embed.AddField("Последний убийца", (object)stats.LastKiller, true);
                        embed.AddField("Последняя жертва", (object)stats.LastVictim, true);
                        embed.AddField("УС", (object)stats.Krd, true);
                        embed.AddField("Сбежал", (object)stats.Escapes, true);
                        embed.AddField("Раундов сыграно", (object)stats.RoundsPlayed, false);
                        embed.AddField("На серверах", (object)Bot.GetHRT(stats.SecondsPlayed), false);
                        embed.WithFooter((Action<EmbedFooterBuilder>)(f => f.Text = "Статистика fydne")).WithColor(Color.Blue).WithCurrentTimestamp();
                        IUserMessage userMessage2 = await context.Channel.SendMessageAsync("", false, embed.Build(), (RequestOptions)null);
                    }
                }
                else if (((IEnumerable<Sync>)syncs).All<Sync>((Func<Sync, bool>)(s => (long)s.DiscordId != (long)context.Message.Author.Id)))
                {
                    IUserMessage userMessage4 = await context.Channel.SendMessageAsync("Ваш Discord не синхронизирован!", false, (Embed)null, (RequestOptions)null);
                }
                else
                {
                    steamId = syncs[Array.FindIndex<Sync>(syncs, (Predicate<Sync>)(s => (long)s.DiscordId == (long)context.Message.Author.Id))].SteamId;
                    path = this.program.Config.StatsFile + "/" + steamId + ".txt";
                    Console.WriteLine(path);
                    if (!System.IO.File.Exists(path))
                    {
                        IUserMessage userMessage1 = await context.Channel.SendMessageAsync("Статистика не найдена.", false, (Embed)null, (RequestOptions)null);
                    }
                    else
                    {
                        steamName = await Bot.GetSteamName(steamId.Replace("@steam", ""));
                        stats = Bot.DeserializeStats(path);
                        embed = new EmbedBuilder();
                        if (string.IsNullOrEmpty(stats.LastKiller))
                            stats.LastKiller = "Отсутствует";
                        if (string.IsNullOrEmpty(stats.LastVictim))
                            stats.LastVictim = "Отсутствует";
                        embed.WithTitle("Статистика: " + steamName + "(" + steamId + ")");
                        embed.AddField("Всего убийств", (object)stats.Kills, true);
                        embed.AddField("Убийств SCP", (object)stats.ScpKills, true);
                        embed.AddField("Смертей", (object)stats.Deaths, true);
                        embed.AddField("Самоубийств", (object)stats.Suicides, true);
                        embed.AddField("Выпито SCP-207", (object)stats.Scp207Uses, true);
                        embed.AddField("Кинуто SCP-018", (object)stats.Scp018Throws, true);
                        embed.AddField("Последний убийца", (object)stats.LastKiller, true);
                        embed.AddField("Последняя жертва", (object)stats.LastVictim, true);
                        embed.AddField("УС", (object)stats.Krd, true);
                        embed.AddField("Сбежал", (object)stats.Escapes, true);
                        embed.AddField("Раундов сыграно", (object)stats.RoundsPlayed, false);
                        embed.AddField("На серверах", (object)Bot.GetHRT(stats.SecondsPlayed), false);
                        embed.WithFooter((Action<EmbedFooterBuilder>)(f => f.Text = "Статистика fydne")).WithColor(Color.Blue).WithCurrentTimestamp();
                        IUserMessage userMessage2 = await context.Channel.SendMessageAsync("", false, embed.Build(), (RequestOptions)null);
                    }
                }
                reader = (StreamReader)null;
                input = (StringReader)null;
                yaml = (YamlStream)null;
                mapping = (YamlMappingNode)null;
                syncs = (Sync[])null;
                args = (string[])null;
                stats = (Stats)null;
                embed = (EmbedBuilder)null;
                steamId = (string)null;
                path = (string)null;
                steamName = (string)null;
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
            }
        }

        private static Stats DeserializeStats(string path)
        {
            string[] strArray = System.IO.File.ReadAllLines(path);
            return new Stats()
            {
                UserId = strArray[0],
                SecondsPlayed = float.Parse(strArray[1]),
                Kills = int.Parse(strArray[2]),
                ScpKills = int.Parse(strArray[3]),
                Deaths = int.Parse(strArray[4]),
                Suicides = int.Parse(strArray[5]),
                Scp207Uses = int.Parse(strArray[6]),
                Scp018Throws = int.Parse(strArray[7]),
                Krd = double.Parse(strArray[8]),
                LastKiller = strArray[9],
                LastVictim = strArray[10],
                Escapes = int.Parse(strArray[11]),
                RoundsPlayed = int.Parse(strArray[12])
            };
        }

        private static string GetHRT(float time)
        {
            time /= 60f;
            TimeSpan timeSpan = TimeSpan.FromMinutes((double)time);
            return string.Format("{0}д {1}ч {2}мин {3}сек.", (object)timeSpan.Days, (object)timeSpan.Hours, (object)timeSpan.Minutes, (object)timeSpan.Seconds);
        }

        private static async Task<string> GetSteamName(string steamId)
        {
            string apikey = "4C3DD54D9D92C18291F05FF2F59639A4";
            string result;
            await Bot.DownloadWebClient(out result, "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + apikey + "&steamids=" + steamId);
            RootObject players = JsonConvert.DeserializeObject<RootObject>(result);
            SteamUser player = players.response.players.FirstOrDefault<SteamUser>();
            return player?.personaname;
        }

        private static Task DownloadWebClient(out string result, string url)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                result = webClient.DownloadString(url);
            }
            return (Task)Task.FromResult<int>(1);
        }

    }
}
