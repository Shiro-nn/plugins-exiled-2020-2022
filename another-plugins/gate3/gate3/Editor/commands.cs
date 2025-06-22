using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mirror;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Exiled.API.Features;
namespace MongoDB.gate3.editor
{
	// Token: 0x02000002 RID: 2
	public class CommandEditor
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void UnloadMap(CommandSender sender, string Name)
		{
			Editor.maps.Remove(Name);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
		public static void CreateMap(CommandSender sender, string Name)
		{
			string path = Path.Combine(gate3e.pluginDir, "maps", Name + ".yml");
			bool flag = File.Exists(path);
			if (flag)
			{
				sender.RaReply("MapEditor#File " + Name + ".yml does exist.", true, true, string.Empty);
			}
			else
			{
				ISerializer serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
				File.WriteAllText(path, serializer.Serialize(new Editor.YML
				{
					objects = new List<Editor.MapObject>()
				}));
				sender.RaReply("MapEditor#Map " + Name + ".yml created.", true, true, string.Empty);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002108 File Offset: 0x00000308
		public static void DeleteMap(CommandSender sender, string Name)
		{
			string path = Path.Combine(gate3e.pluginDir, "maps", Name + ".yml");
			bool flag = !File.Exists(path);
			if (flag)
			{
				sender.RaReply("MapEditor#File " + Name + ".yml does not exist.", true, true, string.Empty);
			}
			else
			{
				bool flag2 = Editor.maps.ContainsKey(Name);
				if (flag2)
				{
					Editor.UnloadMap(sender, Name);
				}
				File.Delete(path);
				sender.RaReply("MapEditor#Map " + Name + ".yml deleted.", true, true, string.Empty);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021A0 File Offset: 0x000003A0
		public static void PrepareMap()
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, Editor.Map> keyValuePair in Editor.maps)
			{
				list.Add(keyValuePair.Value.Name);
			}
			Editor.maps.Clear();
			foreach (string text in list)
			{
				string path = Path.Combine(gate3e.pluginDir, "maps", text + ".yml");
				bool flag = !File.Exists(path);
				if (!flag)
				{
					string text2 = File.ReadAllText(path);
					IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
					List<Editor.MapObject> objects = deserializer.Deserialize<Editor.YML>(text2).objects;
					Editor.Map map = new Editor.Map();
					map.Name = text;
					foreach (Editor.MapObject mapObject in objects)
					{
						Editor.MapObjectLoaded mapObjectLoaded = new Editor.MapObjectLoaded();
						mapObjectLoaded.workStation = gate3e.GetWorkStationObject();
						List<int> list2 = new List<int>();
						foreach (Editor.MapObjectLoaded mapObjectLoaded2 in map.objects)
						{
							list2.Add(mapObjectLoaded2.id);
						}
						mapObjectLoaded.id = Enumerable.Range(1, int.MaxValue).Except(list2).First<int>();
						mapObjectLoaded.name = text;
						mapObjectLoaded.room = mapObject.room;
						mapObjectLoaded.workStation.name = text + "|" + mapObjectLoaded.id.ToString();
						Offset networkposition = default(Offset);
						bool flag2 = mapObject.room != "none";
						if (flag2)
						{
							foreach (Room room in Exiled.API.Features.Map.Rooms)
							{
								bool flag3 = room.Name == mapObject.room;
								if (flag3)
								{
									Transform transform = room.Transform;
									Vector3 position = new Vector3(mapObject.position.x, mapObject.position.y, mapObject.position.z);
									Vector3 vector = new Vector3(mapObject.rotation.x, mapObject.rotation.y, mapObject.rotation.z);
									mapObjectLoaded.position = transform.TransformPoint(position);
									Quaternion quaternion = new Quaternion(transform.rotation.x + vector.x, transform.rotation.y + vector.y, transform.rotation.z + vector.z, transform.rotation.z + vector.z);
									mapObjectLoaded.rotation = quaternion.eulerAngles;
									mapObjectLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(quaternion.eulerAngles);
								}
							}
						}
						else
						{
							mapObjectLoaded.position = new Vector3(mapObject.position.x, mapObject.position.y, mapObject.position.z);
							mapObjectLoaded.rotation = new Vector3(mapObject.rotation.x, mapObject.rotation.y, mapObject.rotation.z);
							mapObjectLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(mapObjectLoaded.rotation);
						}
						mapObjectLoaded.scale = new Vector3(mapObject.scale.x, mapObject.scale.y, mapObject.scale.z);
						networkposition.position = mapObjectLoaded.position;
						networkposition.rotation = mapObjectLoaded.rotation;
						networkposition.scale = Vector3.one;
						mapObjectLoaded.workStation.gameObject.transform.localScale = mapObjectLoaded.scale;
						mapObjectLoaded.workStation.AddComponent<WorkStation>();
						NetworkServer.Spawn(mapObjectLoaded.workStation);
						mapObjectLoaded.workStation.GetComponent<WorkStation>().Networkposition = networkposition;
						map.objects.Add(mapObjectLoaded);
					}
					Editor.maps.Add(text, map);
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000026E4 File Offset: 0x000008E4
		public static Vector3 GetOffset(Vector3 euler, float forward, float right)
		{
			float y = euler.y;
			float num = y;
			Vector3 result;
			if (num != 90f)
			{
				if (num != 180f)
				{
					if (num != 270f)
					{
						result = new Vector3(-forward, 0f, right);
					}
					else
					{
						result = new Vector3(-right, 0f, forward);
					}
				}
				else
				{
					result = new Vector3(forward, 0f, right);
				}
			}
			else
			{
				result = new Vector3(right, 0f, forward);
			}
			return result;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002754 File Offset: 0x00000954
		public static void LoadMap(CommandSender sender, string Name)
		{
			try
			{
				string path = Path.Combine(gate3e.pluginDir, "maps", Name + ".yml");
				bool flag = !File.Exists(path);
				if (flag)
				{
					bool flag2 = sender != null;
					if (flag2)
					{
						sender.RaReply("MapEditor#File " + Name + ".yml does not exist.", true, true, string.Empty);
					}
				}
				else
				{
					bool flag3 = Editor.maps.ContainsKey(Name);
					if (flag3)
					{
						Editor.UnloadMap(sender, Name);
					}
					string text = File.ReadAllText(path);
					IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
					List<Editor.MapObject> objects = deserializer.Deserialize<Editor.YML>(text).objects;
					Editor.Map map = new Editor.Map();
					map.Name = Name;
					foreach (Editor.MapObject mapObject in objects)
					{
						Editor.MapObjectLoaded mapObjectLoaded = new Editor.MapObjectLoaded();
						mapObjectLoaded.workStation = gate3e.GetWorkStationObject();
						List<int> list = new List<int>();
						foreach (Editor.MapObjectLoaded mapObjectLoaded2 in map.objects)
						{
							list.Add(mapObjectLoaded2.id);
						}
						mapObjectLoaded.id = Enumerable.Range(1, int.MaxValue).Except(list).First<int>();
						mapObjectLoaded.name = Name;
						mapObjectLoaded.room = mapObject.room;
						mapObjectLoaded.workStation.name = Name + "|" + mapObjectLoaded.id.ToString();
						Offset networkposition = default(Offset);
						bool flag4 = mapObject.room != "none";
						if (flag4)
						{
							foreach (Room room in Exiled.API.Features.Map.Rooms)
							{
								bool flag5 = room.Name == mapObject.room;
								if (flag5)
								{
									Transform transform = room.Transform;
									Vector3 position = new Vector3(mapObject.position.x, mapObject.position.y, mapObject.position.z);
									Vector3 vector = new Vector3(mapObject.rotation.x, mapObject.rotation.y, mapObject.rotation.z);
									Vector3 euler = transform.TransformDirection(vector);
									Quaternion rotation = Quaternion.Euler(euler);
									mapObjectLoaded.position = transform.TransformPoint(position);
									Quaternion quaternion = new Quaternion(transform.rotation.x + vector.x, transform.rotation.y + vector.y, transform.rotation.z + vector.z, transform.rotation.z + vector.z);
									mapObjectLoaded.rotation = quaternion.eulerAngles;
									mapObjectLoaded.workStation.gameObject.transform.rotation = rotation;
								}
							}
						}
						else
						{
							mapObjectLoaded.position = new Vector3(mapObject.position.x, mapObject.position.y, mapObject.position.z);
							mapObjectLoaded.rotation = new Vector3(mapObject.rotation.x, mapObject.rotation.y, mapObject.rotation.z);
							mapObjectLoaded.workStation.gameObject.transform.rotation = Quaternion.Euler(mapObjectLoaded.rotation);
						}
						mapObjectLoaded.scale = new Vector3(mapObject.scale.x, mapObject.scale.y, mapObject.scale.z);
						networkposition.position = mapObjectLoaded.position;
						networkposition.rotation = mapObjectLoaded.rotation;
						networkposition.scale = Vector3.one;
						mapObjectLoaded.workStation.gameObject.transform.localScale = mapObjectLoaded.scale;
						mapObjectLoaded.workStation.AddComponent<WorkStation>();
						NetworkServer.Spawn(mapObjectLoaded.workStation);
						mapObjectLoaded.workStation.GetComponent<WorkStation>().Networkposition = networkposition;
						map.objects.Add(mapObjectLoaded);
					}
					Editor.maps.Add(Name, map);
					bool flag6 = sender != null;
					if (flag6)
					{
						sender.RaReply(string.Concat(new string[]
						{
							"MapEditor#Map ",
							Name,
							" loaded with ",
							map.objects.Count.ToString(),
							" objects."
						}), true, true, string.Empty);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public static Editor.MapObjectLoaded GetObject(ReferenceHub hub)
		{
			foreach (Editor.MapObjectLoaded mapObjectLoaded in Editor.maps[Editor.playerEditors[hub.characterClassManager.UserId].mapName].objects)
			{
				bool flag = mapObjectLoaded.workStation == Editor.playerEditors[hub.characterClassManager.UserId].selectedObject;
				if (flag)
				{
					return mapObjectLoaded;
				}
			}
			return null;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002D3C File Offset: 0x00000F3C
		public static void SetPositionObject(CommandSender sender, float x, float y, float z)
		{
			bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
			if (!flag)
			{
				bool flag2 = Editor.playerEditors[sender.SenderId].selectedObject == null;
				if (!flag2)
				{
					bool isWorkstation = Editor.playerEditors[sender.SenderId].isWorkstation;
					if (isWorkstation)
					{
						foreach (Editor.MapObjectLoaded mapObjectLoaded in Editor.maps[Editor.playerEditors[sender.SenderId].mapName].objects)
						{
							bool flag3 = mapObjectLoaded.workStation == Editor.playerEditors[sender.SenderId].selectedObject;
							if (flag3)
							{
								Offset networkposition = mapObjectLoaded.workStation.GetComponent<WorkStation>().Networkposition;
								NetworkServer.UnSpawn(mapObjectLoaded.workStation);
								mapObjectLoaded.workStation.transform.rotation = Quaternion.Euler(mapObjectLoaded.rotation);
								networkposition.position = new Vector3(x, y, z);
								mapObjectLoaded.position = networkposition.position;
								NetworkServer.Spawn(mapObjectLoaded.workStation);
								mapObjectLoaded.workStation.GetComponent<WorkStation>().Networkposition = networkposition;
							}
						}
					}
					else
					{
						NetworkServer.UnSpawn(Editor.playerEditors[sender.SenderId].selectedObject);
						Editor.playerEditors[sender.SenderId].selectedObject.transform.position = new Vector3(x, y, z);
						NetworkServer.Spawn(Editor.playerEditors[sender.SenderId].selectedObject);
					}
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000304C File Offset: 0x0000124C
		public static void SetRotationObject(CommandSender sender, float x, float y, float z)
		{
			bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
			if (!flag)
			{
				bool flag2 = Editor.playerEditors[sender.SenderId].selectedObject == null;
				if (!flag2)
				{
					bool isWorkstation = Editor.playerEditors[sender.SenderId].isWorkstation;
					if (isWorkstation)
					{
						foreach (Editor.MapObjectLoaded mapObjectLoaded in Editor.maps[Editor.playerEditors[sender.SenderId].mapName].objects)
						{
							bool flag3 = mapObjectLoaded.workStation == Editor.playerEditors[sender.SenderId].selectedObject;
							if (flag3)
							{
								Offset networkposition = mapObjectLoaded.workStation.GetComponent<WorkStation>().Networkposition;
								NetworkServer.UnSpawn(mapObjectLoaded.workStation);
								networkposition.rotation = new Vector3(x, y, z);
								mapObjectLoaded.workStation.transform.rotation = Quaternion.Euler(x, y, z);
								mapObjectLoaded.rotation = new Vector3(x, y, z);
								networkposition.rotation = mapObjectLoaded.rotation;
								NetworkServer.Spawn(mapObjectLoaded.workStation);
								mapObjectLoaded.workStation.GetComponent<WorkStation>().Networkposition = new Offset
								{
									position = networkposition.position,
									rotation = networkposition.rotation,
									scale = networkposition.scale
								};
							}
						}
					}
					else
					{
						NetworkServer.UnSpawn(Editor.playerEditors[sender.SenderId].selectedObject);
						Editor.playerEditors[sender.SenderId].selectedObject.transform.rotation = Quaternion.Euler(x, y, z);
						NetworkServer.Spawn(Editor.playerEditors[sender.SenderId].selectedObject);
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00003274 File Offset: 0x00001474
		public static void DeleteObject(CommandSender sender)
		{
			bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
			if (!flag)
			{
				bool flag2 = Editor.playerEditors[sender.SenderId].selectedObject == null;
				if (!flag2)
				{
					bool isWorkstation = Editor.playerEditors[sender.SenderId].isWorkstation;
					if (isWorkstation)
					{
						foreach (Editor.MapObjectLoaded mapObjectLoaded in Editor.maps[Editor.playerEditors[sender.SenderId].mapName].objects)
						{
							bool flag3 = mapObjectLoaded.workStation == Editor.playerEditors[sender.SenderId].selectedObject;
							if (flag3)
							{
								UnityEngine.Object.DestroyImmediate(mapObjectLoaded.workStation, true);
								Editor.maps[Editor.playerEditors[sender.SenderId].mapName].objects.Remove(mapObjectLoaded);
							}
						}
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(Editor.playerEditors[sender.SenderId].selectedObject, true);
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000033C4 File Offset: 0x000015C4
		public static void SetScaleObject(CommandSender sender, float x, float y, float z)
		{
			bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
			if (!flag)
			{
				bool flag2 = Editor.playerEditors[sender.SenderId].selectedObject == null;
				if (!flag2)
				{
					bool isWorkstation = Editor.playerEditors[sender.SenderId].isWorkstation;
					if (isWorkstation)
					{
						foreach (Editor.MapObjectLoaded mapObjectLoaded in Editor.maps[Editor.playerEditors[sender.SenderId].mapName].objects)
						{
							bool flag3 = mapObjectLoaded.workStation == Editor.playerEditors[sender.SenderId].selectedObject;
							if (flag3)
							{
								NetworkServer.UnSpawn(mapObjectLoaded.workStation);
								mapObjectLoaded.workStation.transform.localScale = new Vector3(x, y, z);
								mapObjectLoaded.scale = new Vector3(x, y, z);
								NetworkServer.Spawn(mapObjectLoaded.workStation);
							}
						}
					}
					else
					{
						NetworkServer.UnSpawn(Editor.playerEditors[sender.SenderId].selectedObject);
						Editor.playerEditors[sender.SenderId].selectedObject.transform.localScale = new Vector3(x, y, z);
						NetworkServer.Spawn(Editor.playerEditors[sender.SenderId].selectedObject);
					}
				}
			}
		}
		// Token: 0x0600000D RID: 13 RVA: 0x00003564 File Offset: 0x00001764
		public static void CreateObject(CommandSender sender)
		{
			try
			{
				bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
				if (!flag)
				{
					ReferenceHub player = gate3.Extensions.GetPlayer(sender.SenderId);
					Editor.PlayerEditorStatus playerEditorStatus;
					Editor.playerEditors.TryGetValue(sender.SenderId, out playerEditorStatus);
					Editor.Map map = Editor.maps[playerEditorStatus.mapName];
					Editor.MapObjectLoaded mapObjectLoaded = new Editor.MapObjectLoaded();
					List<int> list = new List<int>();
					foreach (Editor.MapObjectLoaded mapObjectLoaded2 in map.objects)
					{
						list.Add(mapObjectLoaded2.id);
					}
					mapObjectLoaded.id = Enumerable.Range(1, int.MaxValue).Except(list).First<int>();
					mapObjectLoaded.workStation = gate3e.GetWorkStationObject();
					mapObjectLoaded.name = playerEditorStatus.mapName;
					mapObjectLoaded.workStation.name = playerEditorStatus.mapName + "|" + mapObjectLoaded.id.ToString();
					Offset networkposition = default(Offset);
					Scp049_2PlayerScript component = player.gameObject.GetComponent<Scp049_2PlayerScript>();
					Scp106PlayerScript component2 = player.gameObject.GetComponent<Scp106PlayerScript>();
					Vector3 forward = component.plyCam.transform.forward;
					RaycastHit raycastHit;
					Physics.Raycast(component.plyCam.transform.position, forward, out raycastHit, 40f, component2.teleportPlacementMask);
					Vector3 rotation = new Vector3(-forward.x, forward.y, -forward.z);
					Vector3 position = CommandEditor.Vec3ToVector3(raycastHit.point) + Vector3.up * 0.1f;
					mapObjectLoaded.position = position;
					mapObjectLoaded.rotation = rotation;
					mapObjectLoaded.scale = mapObjectLoaded.workStation.transform.localScale;
					networkposition.position = position;
					networkposition.rotation = rotation;
					networkposition.scale = mapObjectLoaded.workStation.transform.localScale;
					mapObjectLoaded.workStation.AddComponent<WorkStation>();
					NetworkServer.Spawn(mapObjectLoaded.workStation);
					mapObjectLoaded.workStation.GetComponent<WorkStation>().Networkposition = networkposition;
					map.objects.Add(mapObjectLoaded);
					Editor.playerEditors[sender.SenderId].selectedObject = mapObjectLoaded.workStation;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003804 File Offset: 0x00001A04
		public static void CloneObject(CommandSender sender)
		{
			try
			{
				bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
				if (!flag)
				{
					bool flag2 = Editor.playerEditors[sender.SenderId].selectedObject == null;
					if (!flag2)
					{
						ReferenceHub player = gate3.Extensions.GetPlayer(sender.SenderId);
						Editor.PlayerEditorStatus playerEditorStatus;
						Editor.playerEditors.TryGetValue(sender.SenderId, out playerEditorStatus);
						bool isWorkstation = playerEditorStatus.isWorkstation;
						if (isWorkstation)
						{
							Editor.Map map = Editor.maps[playerEditorStatus.mapName];
							string room = "none";
							foreach (Editor.MapObjectLoaded mapObjectLoaded in map.objects)
							{
								bool flag3 = Editor.playerEditors[sender.SenderId].selectedObject == mapObjectLoaded.workStation;
								if (flag3)
								{
									room = mapObjectLoaded.room;
								}
							}
							Editor.MapObjectLoaded mapObjectLoaded2 = new Editor.MapObjectLoaded();
							List<int> list = new List<int>();
							foreach (Editor.MapObjectLoaded mapObjectLoaded3 in map.objects)
							{
								list.Add(mapObjectLoaded3.id);
							}
							mapObjectLoaded2.id = Enumerable.Range(1, int.MaxValue).Except(list).First<int>();
							mapObjectLoaded2.workStation = gate3e.GetWorkStationObject();
							mapObjectLoaded2.name = playerEditorStatus.mapName;
							mapObjectLoaded2.room = room;
							mapObjectLoaded2.workStation.name = playerEditorStatus.mapName + "|" + mapObjectLoaded2.id.ToString();
							Offset networkposition = default(Offset);
							Vector3 position = Editor.playerEditors[sender.SenderId].selectedObject.transform.position;
							Vector3 eulerAngles = Editor.playerEditors[sender.SenderId].selectedObject.transform.rotation.eulerAngles;
							mapObjectLoaded2.position = position;
							mapObjectLoaded2.rotation = eulerAngles;
							mapObjectLoaded2.scale = Editor.playerEditors[sender.SenderId].selectedObject.transform.localScale;
							networkposition.position = position;
							networkposition.rotation = eulerAngles;
							networkposition.scale = Editor.playerEditors[sender.SenderId].selectedObject.transform.localScale;
							mapObjectLoaded2.workStation.transform.localScale = Editor.playerEditors[sender.SenderId].selectedObject.transform.localScale;
							NetworkServer.Spawn(mapObjectLoaded2.workStation);
							mapObjectLoaded2.workStation.GetComponent<WorkStation>().Networkposition = networkposition;
							mapObjectLoaded2.workStation.AddComponent<WorkStation>();
							map.objects.Add(mapObjectLoaded2);
							Editor.playerEditors[sender.SenderId].selectedObject = mapObjectLoaded2.workStation;
						}
						else
						{
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(playerEditorStatus.selectedObject);
							gameObject.transform.localScale = playerEditorStatus.selectedObject.transform.localScale;
							gameObject.transform.rotation = playerEditorStatus.selectedObject.transform.rotation;
							gameObject.transform.position = playerEditorStatus.selectedObject.transform.position;
							gameObject.name += UnityEngine.Random.Range(0, 99999).ToString();
							NetworkServer.Spawn(gameObject);
							Editor.playerEditors[sender.SenderId].isWorkstation = false;
							Editor.playerEditors[sender.SenderId].selectedObject = gameObject;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003EEC File Offset: 0x000020EC
		public static void SelectObject(CommandSender sender)
		{
			bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
			if (flag)
			{
				sender.RaReply("MapEditor#Enable editing mode.", true, true, string.Empty);
			}
			else
			{
				ReferenceHub player = gate3.Extensions.GetPlayer(sender.SenderId);
				Scp049_2PlayerScript component = player.gameObject.GetComponent<Scp049_2PlayerScript>();
				Vector3 forward = component.plyCam.transform.forward;
				RaycastHit raycastHit;
				Physics.Raycast(component.plyCam.transform.position, forward, out raycastHit, 40f);
				bool flag2 = raycastHit.point.Equals(Vector3.zero);
				if (!flag2)
				{
					bool flag3 = raycastHit.transform.gameObject.name.Split(new char[]
					{
						'|'
					})[0] == Editor.playerEditors[sender.SenderId].mapName;
					if (flag3)
					{
						Editor.playerEditors[sender.SenderId].isWorkstation = true;
						Editor.playerEditors[sender.SenderId].selectedObject = raycastHit.transform.gameObject;
					}
					else
					{
						bool flag4 = raycastHit.transform.parent.gameObject != null;
						if (flag4)
						{
							bool flag5 = raycastHit.transform.parent.gameObject.name.Contains("|");
							if (flag5)
							{
								bool flag6 = raycastHit.transform.parent.gameObject.name.Split(new char[]
								{
									'|'
								})[0] == Editor.playerEditors[sender.SenderId].mapName;
								if (flag6)
								{
									Editor.playerEditors[sender.SenderId].isWorkstation = true;
									Editor.playerEditors[sender.SenderId].selectedObject = raycastHit.transform.parent.gameObject;
									return;
								}
							}
						}
						bool flag7 = raycastHit.transform.parent.transform.parent != null;
						if (flag7)
						{
							bool flag8 = raycastHit.transform.parent.transform.parent.gameObject.name.Contains("|");
							if (flag8)
							{
								bool flag9 = raycastHit.transform.parent.transform.parent.gameObject.name.Split(new char[]
								{
									'|'
								})[0] == Editor.playerEditors[sender.SenderId].mapName;
								if (flag9)
								{
									Editor.playerEditors[sender.SenderId].isWorkstation = true;
									Editor.playerEditors[sender.SenderId].selectedObject = raycastHit.transform.parent.transform.parent.gameObject;
									return;
								}
							}
						}
						bool flag10 = raycastHit.transform.gameObject.GetComponent<NetworkIdentity>() != null;
						if (flag10)
						{
							Editor.playerEditors[sender.SenderId].isWorkstation = false;
							Editor.playerEditors[sender.SenderId].selectedObject = raycastHit.transform.gameObject;
						}
					}
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00004244 File Offset: 0x00002444
		public static Vector3 Vec3ToVector3(Vector3 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00004270 File Offset: 0x00002470
		public static void StopMapEditing(CommandSender sender)
		{
			bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
			if (flag)
			{
				sender.RaReply("MapEditor#You are not currently editing any maps.", true, true, string.Empty);
			}
			else
			{
				Editor.playerEditors.Remove(sender.SenderId);
				sender.RaReply("MapEditor#Editing map finished", true, true, string.Empty);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000042D0 File Offset: 0x000024D0
		public static void SaveMap(CommandSender sender)
		{
			bool flag = !Editor.playerEditors.ContainsKey(sender.SenderId);
			if (flag)
			{
				sender.RaReply("MapEditor#Enable editing mode.", true, true, string.Empty);
			}
			else
			{
				string mapName = Editor.playerEditors[sender.SenderId].mapName;
				ISerializer serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
				List<Editor.MapObject> list = new List<Editor.MapObject>();
				foreach (Editor.MapObjectLoaded mapObjectLoaded in Editor.maps[mapName].objects)
				{
					Editor.ObjectPosition position = new Editor.ObjectPosition
					{
						x = mapObjectLoaded.position.x,
						y = mapObjectLoaded.position.y,
						z = mapObjectLoaded.position.z
					};
					Editor.ObjectPosition rotation = new Editor.ObjectPosition
					{
						x = mapObjectLoaded.rotation.x,
						y = mapObjectLoaded.rotation.y,
						z = mapObjectLoaded.rotation.z
					};
					Editor.ObjectPosition scale = new Editor.ObjectPosition
					{
						x = mapObjectLoaded.scale.x,
						y = mapObjectLoaded.scale.y,
						z = mapObjectLoaded.scale.z
					};
					bool flag2 = mapObjectLoaded.room != "none";
					if (flag2)
					{
						foreach (Room room in Exiled.API.Features.Map.Rooms)
						{
							bool flag3 = room.Name == mapObjectLoaded.room;
							if (flag3)
							{
								Vector3 relativePosition = GetRelativePosition(room, mapObjectLoaded.workStation.transform.position);
								Vector3 relativeRotation = GetRelativeRotation(room, mapObjectLoaded.workStation.transform.rotation.eulerAngles);
								rotation = new Editor.ObjectPosition
								{
									x = relativeRotation.x,
									y = relativeRotation.y,
									z = relativeRotation.z
								};
								position = new Editor.ObjectPosition
								{
									x = relativePosition.x,
									y = relativePosition.y,
									z = relativePosition.z
								};
							}
						}
					}
					list.Add(new Editor.MapObject
					{
						id = mapObjectLoaded.id,
						room = mapObjectLoaded.room,
						position = position,
						rotation = rotation,
						scale = scale
					});
				}
				string path = Path.Combine(gate3e.pluginDir, "maps", mapName + ".yml");
				File.WriteAllText(path, serializer.Serialize(new Editor.YML
				{
					objects = list
				}));
				sender.RaReply("MapEditor#Map " + mapName + " saved.", true, true, string.Empty);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00004624 File Offset: 0x00002824
		public static void EditMap(CommandSender sender, string Name)
		{
			bool flag = !Editor.maps.ContainsKey(Name);
			if (flag)
			{
				sender.RaReply("MapEditor#Map " + Name + " not loaded.", true, true, string.Empty);
			}
			else
			{
				bool flag2 = Editor.playerEditors.ContainsKey(sender.SenderId);
				if (flag2)
				{
					Editor.PlayerEditorStatus playerEditorStatus;
					Editor.playerEditors.TryGetValue(sender.SenderId, out playerEditorStatus);
					sender.RaReply("MapEditor#You are currently editing the <color=green>" + playerEditorStatus.mapName + "</color> map, to stop editing type <color=green>mapeditor cancel</color>", true, true, string.Empty);
				}
				else
				{
					Editor.playerEditors.Add(sender.SenderId, new Editor.PlayerEditorStatus
					{
						editingMap = true,
						mapName = Name,
						selectedObject = null
					});
				}
			}
		}
		public static Vector3 GetRelativeRotation(Room room, Vector3 rotation)
		{
			return room.Transform.InverseTransformDirection(rotation);
		}
		public static Vector3 GetRelativePosition(Room room, Vector3 position)
		{
			return Vec3ToVector3(room.Transform.InverseTransformPoint(Vector3To3(position)));
		}
		public static Vector3 Vector3To3(Vector3 v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x04000001 RID: 1
		public static Dictionary<string, Editor.Map> maps = new Dictionary<string, Editor.Map>();

		// Token: 0x04000002 RID: 2
		public static Dictionary<string, Editor.PlayerEditorStatus> playerEditors = new Dictionary<string, Editor.PlayerEditorStatus>();

		// Token: 0x0200000B RID: 11
		public class MapEditorSettings
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000051 RID: 81 RVA: 0x000060D2 File Offset: 0x000042D2
			// (set) Token: 0x06000052 RID: 82 RVA: 0x000060DA File Offset: 0x000042DA
			public Dictionary<int, List<string>> MapToLoad { get; set; } = new Dictionary<int, List<string>>();
		}

		// Token: 0x0200000C RID: 12
		public class PlayerEditorStatus
		{
			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000054 RID: 84 RVA: 0x000060F7 File Offset: 0x000042F7
			// (set) Token: 0x06000055 RID: 85 RVA: 0x000060FF File Offset: 0x000042FF
			public bool editingMap { get; set; } = false;

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000056 RID: 86 RVA: 0x00006108 File Offset: 0x00004308
			// (set) Token: 0x06000057 RID: 87 RVA: 0x00006110 File Offset: 0x00004310
			public string mapName { get; set; } = "";

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000058 RID: 88 RVA: 0x00006119 File Offset: 0x00004319
			// (set) Token: 0x06000059 RID: 89 RVA: 0x00006121 File Offset: 0x00004321
			public bool isWorkstation { get; set; } = true;

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x0600005A RID: 90 RVA: 0x0000612A File Offset: 0x0000432A
			// (set) Token: 0x0600005B RID: 91 RVA: 0x00006132 File Offset: 0x00004332
			public GameObject selectedObject { get; set; } = null;
		}

		// Token: 0x0200000D RID: 13
		public class MapObject
		{
			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600005D RID: 93 RVA: 0x00006164 File Offset: 0x00004364
			// (set) Token: 0x0600005E RID: 94 RVA: 0x0000616C File Offset: 0x0000436C
			public int id { get; set; } = 0;

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x0600005F RID: 95 RVA: 0x00006175 File Offset: 0x00004375
			// (set) Token: 0x06000060 RID: 96 RVA: 0x0000617D File Offset: 0x0000437D
			public string room { get; set; } = "none";

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000061 RID: 97 RVA: 0x00006186 File Offset: 0x00004386
			// (set) Token: 0x06000062 RID: 98 RVA: 0x0000618E File Offset: 0x0000438E
			public Editor.ObjectPosition position { get; set; } = new Editor.ObjectPosition();

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000063 RID: 99 RVA: 0x00006197 File Offset: 0x00004397
			// (set) Token: 0x06000064 RID: 100 RVA: 0x0000619F File Offset: 0x0000439F
			public Editor.ObjectPosition scale { get; set; } = new Editor.ObjectPosition();

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000065 RID: 101 RVA: 0x000061A8 File Offset: 0x000043A8
			// (set) Token: 0x06000066 RID: 102 RVA: 0x000061B0 File Offset: 0x000043B0
			public Editor.ObjectPosition rotation { get; set; } = new Editor.ObjectPosition();
		}

		// Token: 0x0200000E RID: 14
		public class ObjectPosition
		{
			// Token: 0x1700000E RID: 14
			// (get) Token: 0x06000068 RID: 104 RVA: 0x000061F5 File Offset: 0x000043F5
			// (set) Token: 0x06000069 RID: 105 RVA: 0x000061FD File Offset: 0x000043FD
			public float x { get; set; } = 0f;

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600006A RID: 106 RVA: 0x00006206 File Offset: 0x00004406
			// (set) Token: 0x0600006B RID: 107 RVA: 0x0000620E File Offset: 0x0000440E
			public float y { get; set; } = 0f;

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600006C RID: 108 RVA: 0x00006217 File Offset: 0x00004417
			// (set) Token: 0x0600006D RID: 109 RVA: 0x0000621F File Offset: 0x0000441F
			public float z { get; set; } = 0f;
		}

		// Token: 0x0200000F RID: 15
		public class MapObjectLoaded
		{
			// Token: 0x17000011 RID: 17
			// (get) Token: 0x0600006F RID: 111 RVA: 0x00006252 File Offset: 0x00004452
			// (set) Token: 0x06000070 RID: 112 RVA: 0x0000625A File Offset: 0x0000445A
			public int id { get; set; } = 0;

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x06000071 RID: 113 RVA: 0x00006263 File Offset: 0x00004463
			// (set) Token: 0x06000072 RID: 114 RVA: 0x0000626B File Offset: 0x0000446B
			public string room { get; set; } = "none";

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x06000073 RID: 115 RVA: 0x00006274 File Offset: 0x00004474
			// (set) Token: 0x06000074 RID: 116 RVA: 0x0000627C File Offset: 0x0000447C
			public string name { get; set; } = "";

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000075 RID: 117 RVA: 0x00006285 File Offset: 0x00004485
			// (set) Token: 0x06000076 RID: 118 RVA: 0x0000628D File Offset: 0x0000448D
			public Vector3 position { get; set; } = default(Vector3);

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000077 RID: 119 RVA: 0x00006296 File Offset: 0x00004496
			// (set) Token: 0x06000078 RID: 120 RVA: 0x0000629E File Offset: 0x0000449E
			public Vector3 scale { get; set; } = default(Vector3);

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000079 RID: 121 RVA: 0x000062A7 File Offset: 0x000044A7
			// (set) Token: 0x0600007A RID: 122 RVA: 0x000062AF File Offset: 0x000044AF
			public Vector3 rotation { get; set; } = default(Vector3);

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x0600007B RID: 123 RVA: 0x000062B8 File Offset: 0x000044B8
			// (set) Token: 0x0600007C RID: 124 RVA: 0x000062C0 File Offset: 0x000044C0
			public GameObject workStation { get; set; } = null;
		}

		// Token: 0x02000010 RID: 16
		public class Map
		{
			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600007E RID: 126 RVA: 0x00006328 File Offset: 0x00004528
			// (set) Token: 0x0600007F RID: 127 RVA: 0x00006330 File Offset: 0x00004530
			public string Name { get; set; } = "";

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x06000080 RID: 128 RVA: 0x00006339 File Offset: 0x00004539
			// (set) Token: 0x06000081 RID: 129 RVA: 0x00006341 File Offset: 0x00004541
			public List<Editor.MapObjectLoaded> objects { get; set; } = new List<Editor.MapObjectLoaded>();
		}

		// Token: 0x02000011 RID: 17
		public class YML
		{
			// Token: 0x1700001A RID: 26
			// (get) Token: 0x06000083 RID: 131 RVA: 0x00006369 File Offset: 0x00004569
			// (set) Token: 0x06000084 RID: 132 RVA: 0x00006371 File Offset: 0x00004571
			public List<Editor.MapObject> objects { get; set; } = new List<Editor.MapObject>();
		}
	}
}
