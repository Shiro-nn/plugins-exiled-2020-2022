using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Loli.Discord
{
    [JsonObject]
    class Webhook
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

        public Webhook(string webhookUrl)
        {
            _httpClient = new HttpClient();
            _webhookUrl = webhookUrl;
        }

        public Webhook(ulong id, string token) : this($"https://discord.com/api/webhooks/{id}/{token}") { }
        public void Send(string content, string username = null, string avatarUrl = null, bool isTTS = false, IEnumerable<Embed> embeds = null)
        {
            Content = content;
            Username = username;
            AvatarUrl = avatarUrl;
            IsTTS = isTTS;
            Embeds.Clear();
            if (embeds is not null) Embeds.AddRange(embeds);

            var contentdata = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");

            _httpClient.PostAsync(_webhookUrl, contentdata);
        }
    }
}