using UnityEngine;
using Qurre.API;
using Qurre.API.Controllers;
using System.Linq;
using Qurre.API.Objects;
namespace Loli.Items
{
	public class Spawner
	{
		public void RoundStart()
		{
			SpawnInLure();
			{
				Room room = Map.Rooms.Where(x => x.Type == RoomType.LczCafe).First();
				if (room.Rotation.eulerAngles == new Vector3(0, 270, 0))
				{// + -
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 5, room.Position.y + 3, room.Position.z + 1.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 5.5f, room.Position.y + 3, room.Position.z + 10.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 6, room.Position.y + 3, room.Position.z - 3.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 3, room.Position.y + 3, room.Position.z + 6)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				else if (room.Rotation.eulerAngles == new Vector3(0, 90, 0))
				{// - +
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 5, room.Position.y + 3, room.Position.z - 1.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 5.5f, room.Position.y + 3, room.Position.z - 10.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 6, room.Position.y + 3, room.Position.z + 3.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 3, room.Position.y + 3, room.Position.z - 6)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				else if (room.Rotation.eulerAngles == new Vector3(0, 180, 0)) // + +
				{
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 1.5f, room.Position.y + 3, room.Position.z - 5)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 10.5f, room.Position.y + 3, room.Position.z - 5.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 3.5f, room.Position.y + 3, room.Position.z + 6)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 6, room.Position.y + 3, room.Position.z + 3)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				else if (room.Rotation.eulerAngles == new Vector3(0, 0, 0))// - -
				{
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 1.5f, room.Position.y + 3, room.Position.z + 5)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 10.5f, room.Position.y + 3, room.Position.z + 5.5f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 3.5f, room.Position.y + 3, room.Position.z - 6)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 6, room.Position.y + 3, room.Position.z - 3)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
			}
			{
				Room room = Map.Rooms.Where(x => x.Type == RoomType.Hcz049).First();
				var door = Map.Doors.Find(x => x.Type == DoorType.HCZ_049_Armory);
				if (room.Rotation.eulerAngles == new Vector3(0, 270, 0))
				{
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 1f, door.Position.y + 3.5f, door.Position.z - 3.9f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 1f, door.Position.y + 3.5f, door.Position.z - 3.2f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 1.25f, door.Position.y + 2.5f, door.Position.z - 3.9f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 1.25f, door.Position.y + 2, door.Position.z - 3.6f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 1.25f, door.Position.y + 1.5f, door.Position.z - 3.2f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				else if (room.Rotation.eulerAngles == new Vector3(0, 90, 0))
				{
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 1f, door.Position.y + 3.5f, door.Position.z + 3.9f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 1f, door.Position.y + 3.5f, door.Position.z + 3.2f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 1.25f, door.Position.y + 2.5f, door.Position.z + 3.2f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 1.25f, door.Position.y + 2, door.Position.z + 3.6f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 1.25f, door.Position.y + 1.5f, door.Position.z + 3.9f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				else if (room.Rotation.eulerAngles == new Vector3(0, 180, 0))
				{
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 3.9f, door.Position.y + 3.5f, door.Position.z + 1f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 3.2f, door.Position.y + 3.5f, door.Position.z + 1f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 3.9f, door.Position.y + 2.5f, door.Position.z + 1.25f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 3.6f, door.Position.y + 2, door.Position.z + 1.25f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x + 3.2f, door.Position.y + 1.5f, door.Position.z + 1.25f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				else if (room.Rotation.eulerAngles == new Vector3(0, 0, 0))
				{
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 3.9f, door.Position.y + 3.5f, door.Position.z - 1f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					{
						var _ = new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 3.2f, door.Position.y + 3.5f, door.Position.z - 1f));
						_.Rotation = Quaternion.Euler(door.Rotation.eulerAngles);
						_.Scale = new Vector3(0.5f, 0.5f, 0.5f);
					}
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 3.2f, door.Position.y + 2.5f, door.Position.z - 1.25f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 3.6f, door.Position.y + 2, door.Position.z - 1.25f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(door.Position.x - 3.9f, door.Position.y + 1.5f, door.Position.z - 1.25f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
			}
			{
				Room room = Map.Rooms.Where(x => x.Type == RoomType.EzDownstairsPcs).First();
				Door door = Map.Doors.Where(x => Vector3.Distance(room.Position, x.Position) < 11).First();
				if (room.Position.x.Difference(door.Position.x) < 1)
				{
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 3, room.Position.y + 0.5f, room.Position.z + 0.1f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 3, room.Position.y + 0.5f, room.Position.z + 0.1f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 9, room.Position.y + 0.5f, room.Position.z + 4.6f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 9, room.Position.y + 0.5f, room.Position.z + 4.6f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
				else if (room.Position.z.Difference(door.Position.z) < 1)
				{
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 0.1f, room.Position.y + 0.5f, room.Position.z - 3)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 0.1f, room.Position.y + 0.5f, room.Position.z + 3)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 4.6f, room.Position.y + 0.5f, room.Position.z - 9)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 4.6f, room.Position.y + 0.5f, room.Position.z + 9)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				}
			}
			{
				Room room = Map.Rooms.Where(x => x.Type == RoomType.EzUpstairsPcs).First();
				Door door = Map.Doors.Where(x => Vector3.Distance(room.Position, x.Position) < 11).First();
				if (room.Position.x.Difference(door.Position.x) < 1)
				{
					var p1 = new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 5.3f, room.Position.y + 3, room.Position.z + 0.1f));
					var p2 = new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 5.3f, room.Position.y + 3, room.Position.z + 0.1f));
					p1.Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					p2.Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					MEC.Timing.CallDelayed(1f, () =>
					{
						if (0.2f >= p1.Position.y.Difference(room.Position.y)) p1.Destroy();
						else if (0.2f >= p2.Position.y.Difference(room.Position.y)) p2.Destroy();
					});
				}
				else if (room.Position.z.Difference(door.Position.z) < 1)
				{
					var p1 = new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 0.1f, room.Position.y + 3, room.Position.z - 5.3f));
					var p2 = new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 0.1f, room.Position.y + 3, room.Position.z + 5.3f));
					p1.Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					p2.Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
					MEC.Timing.CallDelayed(1f, () =>
					{
						if (0.2f >= p1.Position.y.Difference(room.Position.y)) p1.Destroy();
						else if (0.2f >= p2.Position.y.Difference(room.Position.y)) p2.Destroy();
					});
				}
			}
			{
				Room room = Map.Rooms.Where(x => x.Type == RoomType.EzPcs).First();
				Door door = Map.Doors.Where(x => Vector3.Distance(room.Position, x.Position) < 11).First();
				if (room.Position.x.Difference(door.Position.x) < 1)
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x - 2.8f, room.Position.y + 3, room.Position.z + 0.1f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));
				if (room.Position.z.Difference(door.Position.z) < 1)
					new Item(ItemType.Coin).Spawn(new Vector3(room.Position.x + 0.1f, room.Position.y + 3, room.Position.z - 2.8f)).Rotation = Quaternion.Euler(new Vector3(90, 0, 0));

			}
		}
		internal static Vector3 EngineerPos = Vector3.one;
		public static void SpawnInLure()
		{
			if (Extensions.Random.Next(0, 100) > 50)
			{
				if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardFacilityManager).Spawn(new Vector3(-6.3f, -1995, 6.5f));
				else if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardFacilityManager).Spawn(new Vector3(-5.5f, -1995, 5.5f));
				else if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardFacilityManager).Spawn(new Vector3(4, -1995, 4));
				else if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardFacilityManager).Spawn(new Vector3(7, -1995, 7));
				else
					new Item(ItemType.KeycardFacilityManager).Spawn(new Vector3(0, -1995, 0));
			}
			else
			{
				if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardContainmentEngineer).Spawn(new Vector3(-5.5f, -1995, 5.5f));
				else if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardContainmentEngineer).Spawn(new Vector3(-6.3f, -1995, 6.5f));
				else if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardContainmentEngineer).Spawn(new Vector3(4, -1995, 4));
				else if (Extensions.Random.Next(0, 100) > 50)
					new Item(ItemType.KeycardContainmentEngineer).Spawn(new Vector3(7, -1995, 7));
				else
					new Item(ItemType.KeycardContainmentEngineer).Spawn(new Vector3(0, -1995, 0));
			}
		}
	}
}