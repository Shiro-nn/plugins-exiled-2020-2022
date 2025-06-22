using Newtonsoft.Json;

namespace HubServer
{
    internal class ReqReply
    {
        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("port")]
        public ushort Port { get; set; }
    }
}