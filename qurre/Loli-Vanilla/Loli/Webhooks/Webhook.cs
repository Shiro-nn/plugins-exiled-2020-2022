using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Loli.Webhooks
{
    [JsonObject]
    public class Dishook
    {
        private readonly HttpClient _httpClient;
        private readonly string _webhookUrl;

        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("tts")]
        public bool IsTTS { get; set; }
        [JsonProperty("embeds")]
        public List<Embed> Embeds { get; set; } = new();

        public Dishook(string webhookUrl)
        {
            _httpClient = new HttpClient();
            _webhookUrl = webhookUrl;
        }

        public Dishook(ulong id, string token) : this($"https://discord.com/api/webhooks/{id}/{token}") { }
        public void Send(string content, string username = null, string avatarUrl = null, bool isTTS = false, IEnumerable<Embed> embeds = null)
        {
            Content = content;
            Username = username;
            AvatarUrl = avatarUrl;
            IsTTS = isTTS;
            Embeds.Clear();
            if (embeds is not null) Embeds.AddRange(embeds);

            var contentdata = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");

            if (Core.UseProxy)
            {
                new Thread(async () =>
                {
                    var proxy = new WebProxy
                    {
                        Address = new Uri($"http://soundcloud-proxy.fydne.dev:3128"),
                        BypassProxyOnLocal = false,
                        UseDefaultCredentials = false,

                        Credentials = new NetworkCredential(userName: "fydne", password: "NuIZ2iFLf4Lk61MakQlcUhD6Dkv4DF2sEDbAwLL1Pb")
                    };

                    var httpClientHandler = new HttpClientHandler { Proxy = proxy };
                    var _client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
                    var msg = await _client.PostAsync(_webhookUrl, contentdata);
                }).Start();
            }
            else _httpClient.PostAsync(_webhookUrl, contentdata);
        }
    }
}