using Loli.DataBase;
using Newtonsoft.Json;
using Qurre.API;
using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli
{
	public class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public CustomDictionary() : base() { }

		new public bool TryGetValue(TKey key, out TValue value)
		{
			try
			{
				return base.TryGetValue(key, out value);
			}
			catch
			{
				value = default;
				return false;
			}
		}

		new public bool ContainsKey(TKey key)
		{
			try
			{
				return base.ContainsKey(key);
			}
			catch
			{
				return false;
			}
		}

		new public void Add(TKey key, TValue value)
		{
			try
			{
				base.Add(key, value);
			}
			catch { }
		}

		new public bool Remove(TKey key)
		{
			try
			{
				return base.Remove(key);
			}
			catch
			{
				return false;
			}
		}
	}
	internal class BansCounts
	{
		readonly List<DateTime> _dates = new();
		internal int Counts => _dates.Count(x => (DateTime.Now - x).TotalSeconds < 30);
		internal void Add() => _dates.Add(DateTime.Now);
		internal void Clear() => _dates.Clear();
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


	[Serializable]
	class SteamMainInfoApi
	{
		[JsonProperty("personaname")]
		public string Name { get; set; }

		[JsonProperty("avatar")]
		public string Avatar { get; set; }

		[JsonProperty("avatarfull")]
		public string AvatarFull { get; set; }
	}

	[Serializable]
	class DoubfulData
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("setuped")]
		public bool IsSetup { get; set; }

		[JsonProperty("banned")]
		public bool Banned { get; set; }

		[JsonProperty("days_since_last_ban")]
		public int DaysSinceLastBan { get; set; }

		[JsonProperty("level")]
		public int Level { get; set; }

		[JsonProperty("game_hours")]
		public int GameHours { get; set; }

		[JsonProperty("created")]
		public long Created { get; set; }

		[JsonProperty("created_formatted")]
		public string CreatedFormatted { get; set; }
	}

	[Serializable]
	class GeoIP
	{
		[JsonProperty("ip")]
		public string Ip { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("region")]
		public string Region { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("loc")]
		public string Loc { get; set; }

		[JsonProperty("org")]
		public string Org { get; set; }

		[JsonProperty("postal")]
		public string Postal { get; set; }

		[JsonProperty("timezone")]
		public string Timezone { get; set; }
	}


	[Serializable]
	class GetCheaterByUserid
	{
		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("ipAddress")]
		public string[] IpArray { get; set; }

		[JsonProperty("reports")]
		public CheaterReport[] Reports { get; set; }
	}

	[Serializable]
	class GetCheaterByIp
	{
		[JsonProperty("ipAddress")]
		public string IpAddress { get; set; }

		[JsonProperty("userIds")]
		public string[] UserIds { get; set; }

		[JsonProperty("reports")]
		public CheaterReport[] Reports { get; set; }
	}

	[Serializable]
	class CheaterReport
	{
		[JsonProperty("projectName")]
		public string Project { get; set; }

		[JsonProperty("isConfirmed")]
		public bool Verified { get; set; }

		[JsonProperty("reason")]
		public string Reason { get; set; }

		[JsonProperty("evidence")]
		public string Evidence { get; set; }
	}
}