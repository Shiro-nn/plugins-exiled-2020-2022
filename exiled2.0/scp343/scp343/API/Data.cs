using Exiled.API.Features;
namespace scp343.API
{
    public class Data
    {
        public static Player scp343()
        {
            return Player.Get(EventHandlers.scp343);
        }
    }
}