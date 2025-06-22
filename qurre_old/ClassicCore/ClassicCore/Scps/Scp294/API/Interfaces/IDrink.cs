using Qurre.API;
namespace ClassicCore.Scps.Scp294.API.Interfaces
{
    public interface IDrink
    {
        string Name { get; }
        string Description { get; }

        /// <returns>IsAllowed</returns>
        bool OnStartDrinking(Player player);
        void OnDrank(Player player);
    }
}