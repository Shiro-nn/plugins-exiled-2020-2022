using ClassicCore.DataBase;
using Qurre.API;
using System;
using UnityEngine;
namespace ClassicCore
{
	public static class Extensions
	{
		public static float Difference(this float first, float second)
		{
			return Math.Abs(first - second);
		}
		public static void GetAmmo(this Player pl)
		{
			pl.Ammo12Gauge = 999;
			pl.Ammo44Cal = 999;
			pl.Ammo556 = 999;
			pl.Ammo762 = 999;
			pl.Ammo9 = 999;
		}
		public static float GetMaxHp(this Player pl)
		{
			float maxhp = pl.MaxHp;
			switch (pl.Role)
			{
				case RoleType.ClassD:
					maxhp = 105;
					break;
				case RoleType.Scientist:
					maxhp = 110;
					break;
				case RoleType.ChaosConscript or RoleType.ChaosMarauder or RoleType.ChaosRepressor or RoleType.ChaosRifleman:
					maxhp = 125;
					break;
				case RoleType.NtfPrivate:
					maxhp = 115;
					break;
				case RoleType.NtfSergeant:
					maxhp = 120;
					break;
				case RoleType.NtfCaptain:
					maxhp = 125;
					break;
				case RoleType.NtfSpecialist:
					maxhp = 125;
					break;
				case RoleType.FacilityGuard:
					maxhp = 130;
					break;
				case RoleType.Tutorial:
					maxhp = 125;
					break;
				case RoleType.Scp0492:
					maxhp = 750;
					break;
				case RoleType.Scp106:
					maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp106).maxHP;
					break;
				case RoleType.Scp049:
					maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp049).maxHP;
					break;
				case RoleType.Scp096:
					maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp096).maxHP;
					break;
				case RoleType.Scp93953 or RoleType.Scp93989:
					maxhp = pl.ClassManager.Classes.SafeGet(pl.Role).maxHP;
					break;
				case RoleType.Scp173:
					maxhp = pl.ClassManager.Classes.SafeGet(RoleType.Scp173).maxHP;
					break;
			}
			float cf = 1;
			if (Manager.Static.Data.Users.TryGetValue(pl.UserId, out var data))
			{
				if (Manager.Static.Data.Roles.TryGetValue(pl.UserId, out var roles) && roles.Prime) cf += 0.1f;
				if (DataBase.Modules.Data.Clans.TryGetValue(data.clan.ToUpper(), out var clan))
					foreach (int boost in clan.Boosts)
						if (boost == 3) cf += 0.05f;
			}
			return maxhp * cf;
		}
		public static void SetRank(this Player player, string rank, string color = "default")
		{
			player.RoleName = rank;
			player.RoleColor = color;
		}
	}
	public class RainbowTagController : MonoBehaviour
	{
		private ServerRoles _roles;
		private string _originalColor;

		private int _position = 0;
		private float _nextCycle = 0f;

		public static string[] Colors =
		{
			"pink",
			"red",
			"brown",
			"silver",
			"light_green",
			"crimson",
			"cyan",
			"aqua",
			"deep_pink",
			"tomato",
			"yellow",
			"magenta",
			"blue_green",
			"orange",
			"lime",
			"green",
			"emerald",
			"carmine",
			"nickel",
			"mint",
			"army_green",
			"pumpkin"
		};

		internal static float Interval { get; set; } = 0.5f;


		private void Start()
		{
			_roles = GetComponent<ServerRoles>();
			_nextCycle = Time.time;
			_originalColor = _roles.Network_myColor;
		}


		private void OnDisable()
		{
			_roles.Network_myColor = _originalColor;
		}


		private void Update()
		{
			if (Time.time < _nextCycle) return;
			_nextCycle += Interval;

			_roles.Network_myColor = Colors[_position];

			if (++_position >= Colors.Length)
				_position = 0;
		}
	}
}