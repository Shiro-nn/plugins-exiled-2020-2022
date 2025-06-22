using Qurre.API.Addons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loli
{
	internal enum HackMode : byte
	{
		Safe,
		Hacking,
		Hacked
	}
	class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>
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
		internal int Counts => _dates.Count(x => (DateTime.Now - x).TotalMinutes < 1);
		internal void Add() => _dates.Add(DateTime.Now);
		internal void Clear() => _dates.Clear();
	}
	class FixPrimitiveSmoothing : MonoBehaviour
	{
		readonly float _interval = 0.1f;
		float _nextCycle = 0f;

		internal Model Model;

		void Start()
		{
			_nextCycle = Time.time;
		}
		void Update()
		{
			if (Model is null)
				return;

			if (Time.time < _nextCycle)
				return;

			_nextCycle += _interval;

			for (int i = 0; i < Model.Primitives.Count; i++)
			{
				var prim = Model.Primitives[i];
				prim.Primitive.Base.NetworkMovementSmoothing = prim.Primitive.MovementSmoothing;
				prim.Primitive.Base.NetworkRotation = new LowPrecisionQuaternion(prim.GameObject.transform.rotation);
				prim.Primitive.Base.NetworkPosition = prim.GameObject.transform.position;
			}
		}
	}
	enum RoleType : sbyte
	{
		None = -1,
		Scp173,
		ClassD,
		Spectator,
		Scp106,
		NtfSpecialist,
		Scp049,
		Scientist,
		Scp079,
		ChaosConscript,
		Scp096,
		Scp0492,
		NtfSergeant,
		NtfCaptain,
		NtfPrivate,
		Tutorial,
		FacilityGuard,
		Scp939,
		CustomRole,
		ChaosRifleman,
		ChaosRepressor,
		ChaosMarauder,
		Overwatch,
		Scp035
	}
	class RainbowTagController : MonoBehaviour
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