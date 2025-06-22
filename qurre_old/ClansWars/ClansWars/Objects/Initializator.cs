namespace ClansWars.Objects
{
    internal static class Initializator
    {
        internal static void Enable()
        {
            Qurre.Events.Round.Start += Lift.SpawnDoors;
            Qurre.Events.Map.DoorDamage += Lift.DoorEvents;
            Qurre.Events.Map.DoorLock += Lift.DoorEvents;
            Qurre.Events.Map.DoorOpen += Lift.DoorEvents;
            Qurre.Events.Scp079.InteractDoor += Lift.DoorEvents;
            Qurre.Events.Scp079.LockDoor += Lift.DoorEvents;
            Qurre.Events.Player.InteractDoor += Lift.DoorEvents;
        }
    }
}