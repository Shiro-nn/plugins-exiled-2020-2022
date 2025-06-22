using Loli.Addons.RolePlay.CutScenes;
using Loli.Addons.RolePlay.Roles;
namespace Loli.Addons.RolePlay
{
    internal class Manager
    {
        //internal Scp173Scene Scp173Scene;
        //internal Scp106Scene Scp106Scene;
        internal Default Scene;//Delete later
        internal RealDClass RealDClass;
        internal Scientists Scientists;
        internal Modules Modules;
        internal void RegisterEvents()
        {/*
            Scp173Scene = new Scp173Scene();
            Qurre.Events.Round.Start += Scp173Scene.StartRound;
            Qurre.Events.Player.Spawn += Scp173Scene.FixTP;
            Qurre.Events.Player.DroppingItem += Scp173Scene.FixTP;
            Qurre.Events.Player.PickupItem += Scp173Scene.FixTP;
            Qurre.Events.Player.ScpAttack += Scp173Scene.FixTP;
            Qurre.Events.Player.Dies += Scp173Scene.FixTP;
            Qurre.Events.Player.Damage += Scp173Scene.FixTP;
            Qurre.Events.Scp106.Contain += Scp173Scene.FixTP;
            Qurre.Events.Player.InteractLocker += Scp173Scene.FixTP;

            Scp106Scene = new Scp106Scene();
            Qurre.Events.Round.Start += Scp106Scene.RoundStart;
            Qurre.Events.Player.Spawn += Scp106Scene.FixTP;
            Qurre.Events.Player.DroppingItem += Scp106Scene.FixTP;
            Qurre.Events.Player.PickupItem += Scp106Scene.FixTP;
            Qurre.Events.Player.ScpAttack += Scp106Scene.FixTP;
            Qurre.Events.Player.Dies += Scp106Scene.FixTP;
            Qurre.Events.Player.Damage += Scp106Scene.FixTP;
            Qurre.Events.Scp106.Contain += Scp106Scene.FixTP;
            Qurre.Events.Player.InteractLocker += Scp106Scene.FixTP;*/
            Scene = new Default();
            Qurre.Events.Round.Start += Scene.Start;

            RealDClass = new RealDClass();
            Qurre.Events.Player.Spawn += RealDClass.RealName;
            Qurre.Events.Player.Spawn += RealDClass.RealСharacter;
            Qurre.Events.Player.Spawn += RealDClass.FixTags;
            Qurre.Events.Player.RoleChange += RealDClass.FixTags;
            Qurre.Events.Player.Dead += RealDClass.FixTags;
            Qurre.Events.Player.Escape += RealDClass.FixTags;
            Qurre.Events.Player.PickupItem += RealDClass.PickReal;

            Scientists = new Scientists();
            Qurre.Events.Round.Start += Scientists.RoundStart;
            Qurre.Events.Player.Spawn += Scientists.Spawn;
            Qurre.Events.Player.Spawn += Scientists.FixTags;
            Qurre.Events.Player.RoleChange += Scientists.FixTags;
            Qurre.Events.Player.Dead += Scientists.FixTags;
            Qurre.Events.Player.PickupItem += Scientists.BetterHid;
            Qurre.Events.Player.DropItem += Scientists.BetterHid;
            Qurre.Events.Player.ItemChange += Scientists.BetterHid;

            Modules = new Modules();
            Qurre.Events.Round.Start += Modules.RealEscape;
            Qurre.Events.Round.Start += Modules.MainGuard;
            Qurre.Events.Player.RadioUsing += Modules.BetterRadio;
            Qurre.Events.Player.Escape += Modules.RealEscape;
            Qurre.Events.Round.Waiting += Modules.RealUnits;
            Qurre.Events.Player.ItemChange += Modules.RealHid;
            Qurre.Events.Player.DroppingItem += Modules.RealHid;
            Qurre.Events.Player.PickupItem += Modules.RealHid;
            Qurre.Events.Scp914.Upgrade += Modules.Real914;
            Qurre.Events.Player.InteractDoor += Modules.Door;
            Qurre.Events.Player.RechargeWeapon += Modules.FixRecharge;
            Qurre.Events.Player.RoleChange += Modules.Spawn;
            Qurre.Events.Player.Spawn += Modules.Spawn;
            Qurre.Events.Round.Start += Modules.RoundStart;
            Qurre.Events.Player.Dead += Modules.SweepFixTag;
            Qurre.Events.Player.RoleChange += Modules.SweepFixTag;
            Qurre.Events.Player.Spawn += Modules.SweepFixTag;

            Qurre.Events.Player.DamageProcess += FacilityManager.Damage;
            Qurre.Events.Player.Escape += FacilityManager.Escape;
            Qurre.Events.Player.Cuff += FacilityManager.Cuff;
        }
        internal void UnRegisterEvents()
        {/*
            Qurre.Events.Round.Start -= Scp173Scene.StartRound;
            Qurre.Events.Player.Spawn -= Scp173Scene.FixTP;
            Qurre.Events.Player.DroppingItem -= Scp173Scene.FixTP;
            Qurre.Events.Player.PickupItem -= Scp173Scene.FixTP;
            Qurre.Events.Player.ScpAttack -= Scp173Scene.FixTP;
            Qurre.Events.Player.Dies -= Scp173Scene.FixTP;
            Qurre.Events.Player.Damage -= Scp173Scene.FixTP;
            Qurre.Events.Scp106.Contain -= Scp173Scene.FixTP;
            Qurre.Events.Player.InteractLocker -= Scp173Scene.FixTP;
            Scp173Scene = null;

            Qurre.Events.Round.Start -= Scp106Scene.RoundStart;
            Qurre.Events.Player.Spawn -= Scp106Scene.FixTP;
            Qurre.Events.Player.DroppingItem -= Scp106Scene.FixTP;
            Qurre.Events.Player.PickupItem -= Scp106Scene.FixTP;
            Qurre.Events.Player.ScpAttack -= Scp106Scene.FixTP;
            Qurre.Events.Player.Dies -= Scp106Scene.FixTP;
            Qurre.Events.Player.Damage -= Scp106Scene.FixTP;
            Qurre.Events.Scp106.Contain -= Scp106Scene.FixTP;
            Qurre.Events.Player.InteractLocker -= Scp106Scene.FixTP;
            Scp106Scene = null;*/
            Qurre.Events.Round.Start -= Scene.Start;
            Scene = null;

            Qurre.Events.Player.Spawn -= RealDClass.RealName;
            Qurre.Events.Player.Spawn -= RealDClass.RealСharacter;
            Qurre.Events.Player.Spawn -= RealDClass.FixTags;
            Qurre.Events.Player.RoleChange -= RealDClass.FixTags;
            Qurre.Events.Player.Dead -= RealDClass.FixTags;
            Qurre.Events.Player.Escape -= RealDClass.FixTags;
            Qurre.Events.Player.PickupItem -= RealDClass.PickReal;
            RealDClass = null;

            Qurre.Events.Round.Start -= Scientists.RoundStart;
            Qurre.Events.Player.Spawn -= Scientists.Spawn;
            Qurre.Events.Player.Spawn -= Scientists.FixTags;
            Qurre.Events.Player.RoleChange -= Scientists.FixTags;
            Qurre.Events.Player.Dead -= Scientists.FixTags;
            Qurre.Events.Player.PickupItem -= Scientists.BetterHid;
            Qurre.Events.Player.DropItem -= Scientists.BetterHid;
            Qurre.Events.Player.ItemChange -= Scientists.BetterHid;
            Scientists = null;

            Qurre.Events.Round.Start -= Modules.RealEscape;
            Qurre.Events.Round.Start -= Modules.MainGuard;
            Qurre.Events.Player.RadioUsing -= Modules.BetterRadio;
            Qurre.Events.Player.Escape -= Modules.RealEscape;
            Qurre.Events.Round.Waiting -= Modules.RealUnits;
            Qurre.Events.Player.ItemChange -= Modules.RealHid;
            Qurre.Events.Player.DroppingItem -= Modules.RealHid;
            Qurre.Events.Player.PickupItem -= Modules.RealHid;
            Qurre.Events.Scp914.Upgrade -= Modules.Real914;
            Qurre.Events.Player.InteractDoor -= Modules.Door;
            Qurre.Events.Player.RechargeWeapon -= Modules.FixRecharge;
            Qurre.Events.Player.RoleChange -= Modules.Spawn;
            Qurre.Events.Player.Spawn -= Modules.Spawn;
            Qurre.Events.Round.Start -= Modules.RoundStart;
            Qurre.Events.Player.Dead -= Modules.SweepFixTag;
            Qurre.Events.Player.RoleChange -= Modules.SweepFixTag;
            Qurre.Events.Player.Spawn -= Modules.SweepFixTag;
            Modules = null;

            Qurre.Events.Player.DamageProcess -= FacilityManager.Damage;
            Qurre.Events.Player.Escape -= FacilityManager.Escape;
            Qurre.Events.Player.Cuff -= FacilityManager.Cuff;
        }
    }
}