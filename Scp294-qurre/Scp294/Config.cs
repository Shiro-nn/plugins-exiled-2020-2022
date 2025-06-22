using Qurre.API.Addons;

namespace Scp294
{
    public sealed class Config : IConfig
    {
        public string Name { get; set; } = "scp_294";

        public bool IsEnabled { get; set; } = true;
    }
}