using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClassicCore.Modules
{
	[Serializable]
	public class VecPos
	{
		public int sec = 0;
		public Vector3 Pos = new(0, 0, 0);
		public bool Alive { get; set; } = true;
	}
}