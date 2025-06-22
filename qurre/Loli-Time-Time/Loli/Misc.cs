using Qurre.API.Addons.Models;
using Qurre.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loli
{
	internal enum HackMode : byte
	{
		Safe,
		Hacking,
		Hacked
	}
	public class FixPrimitiveSmoothing : MonoBehaviour
	{
		internal Model Model;
		private void Update()
		{
			if (Model is null) return;
			for (int i = 0; i < Model.Primitives.Count; i++)
			{
				var prim = Model.Primitives[i];
				prim.Primitive.Base.NetworkMovementSmoothing = prim.Primitive.MovementSmoothing;
				prim.Primitive.Base.NetworkRotation = new LowPrecisionQuaternion(prim.GameObject.transform.rotation);
				prim.Primitive.Base.NetworkPosition = prim.GameObject.transform.position;
			}
		}
	}
	public class FixOnePrimitiveSmoothing : MonoBehaviour
	{
		internal Primitive Primitive;
		private void Update()
		{
			if (Primitive is null) return;
			Primitive.Base.NetworkMovementSmoothing = Primitive.MovementSmoothing;
			Primitive.Base.NetworkRotation = new LowPrecisionQuaternion(Primitive.Rotation);
			Primitive.Base.NetworkPosition = Primitive.Position;
		}
	}
	public enum RoleType : sbyte
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