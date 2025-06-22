using System;

namespace MEC
{
	public struct CoroutineHandle : IEquatable<CoroutineHandle>
	{
		public byte Key
		{
			get
			{
				return (byte)(this._id & 15);
			}
		}

		public CoroutineHandle(byte ind)
		{
			if (ind > 15)
			{
				ind -= 15;
			}
			this._id = CoroutineHandle.NextIndex[(int)ind] + (int)ind;
			CoroutineHandle.NextIndex[(int)ind] += 16;
		}

		public CoroutineHandle(CoroutineHandle other)
		{
			this._id = other._id;
		}

		public bool Equals(CoroutineHandle other)
		{
			return this._id == other._id;
		}

		public override bool Equals(object other)
		{
			return other is CoroutineHandle && this.Equals((CoroutineHandle)other);
		}

		public static bool operator ==(CoroutineHandle a, CoroutineHandle b)
		{
			return a._id == b._id;
		}

		public static bool operator !=(CoroutineHandle a, CoroutineHandle b)
		{
			return a._id != b._id;
		}

		public override int GetHashCode()
		{
			return this._id;
		}

		public override string ToString()
		{
			if (Timing.GetTag(this) == null)
			{
				if (Timing.GetLayer(this) == null)
				{
					return Timing.GetDebugName(this);
				}
				return Timing.GetDebugName(this) + " Layer: " + Timing.GetLayer(this);
			}
			else
			{
				if (Timing.GetLayer(this) == null)
				{
					return Timing.GetDebugName(this) + " Tag: " + Timing.GetTag(this);
				}
				return string.Concat(new object[]
				{
					Timing.GetDebugName(this),
					" Tag: ",
					Timing.GetTag(this),
					" Layer: ",
					Timing.GetLayer(this)
				});
			}
		}

		public string Tag
		{
			get
			{
				return Timing.GetTag(this);
			}
			set
			{
				Timing.SetTag(this, value, true);
			}
		}

		public int? Layer
		{
			get
			{
				return Timing.GetLayer(this);
			}
			set
			{
				if (value == null)
				{
					Timing.RemoveLayer(this);
					return;
				}
				Timing.SetLayer(this, value.Value, true);
			}
		}

		public Segment Segment
		{
			get
			{
				return Timing.GetSegment(this);
			}
			set
			{
				Timing.SetSegment(this, value);
			}
		}

		public bool IsRunning
		{
			get
			{
				return Timing.IsRunning(this);
			}
			set
			{
				if (!value)
				{
					Timing.KillCoroutines(new CoroutineHandle[]
					{
						this
					});
				}
			}
		}

		public bool IsAliveAndPaused
		{
			get
			{
				return Timing.IsAliveAndPaused(this);
			}
			set
			{
				if (value)
				{
					Timing.PauseCoroutines(new CoroutineHandle[]
					{
						this
					});
					return;
				}
				Timing.ResumeCoroutines(new CoroutineHandle[]
				{
					this
				});
			}
		}

		public bool IsValid
		{
			get
			{
				return this.Key > 0;
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CoroutineHandle()
		{
			int[] array = new int[16];
			array[0] = 16;
			CoroutineHandle.NextIndex = array;
		}

		private const byte ReservedSpace = 15;

		private static readonly int[] NextIndex;

		private readonly int _id;
	}
}
