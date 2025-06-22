using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qurre;
using Qurre.API;
using Qurre.API.Events;
using SLCustomObjects.Args;
using UnityEngine;
namespace SLCustomObjects
{
    public class Schematic
    {
        public delegate void PickupInteract(PickupInteractEvent ev);
        public static event PickupInteract PickupInteractEvent;
        public static SchematicInit init = new SchematicPluginInit();

        public class SchematicPluginInit : SchematicInit
        {
            public override GameObject CreateObject(SchematicData data, string schematicName, ItemType force, ref bool isNetwork)
            {
                switch (data.ObjectType)
                {
                    case ObjectType.Item:
                        var dat = JsonConvert.DeserializeObject<SchematicItemData>(data.CustomData);
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ReferenceHub.HostHub.inventory.pickupPrefab);
                        gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        gameObject.AddComponent<DisablePickupInteract>();
                        gameObject.GetComponent<Pickup>().SetupPickup(force == ItemType.None ? (ItemType)dat.ItemID : force, -1f, ReferenceHub.HostHub.gameObject, new Pickup.WeaponModifiers(false, 0, 0, 0), gameObject.transform.position, gameObject.transform.rotation);
                        isNetwork = true;
                        if (dat.ExecuteEventOnPickup)
                        {
                            var p = gameObject.AddComponent<PickupEvent>();
                            p.SchematicName = schematicName;
                            p.EventName = dat.EventName;
                            p.pickup = gameObject.GetComponent<Pickup>();
                        }
                        return gameObject;
                    case ObjectType.Animation:
                        var dat2 = JsonConvert.DeserializeObject<SchematicAnimationData>(data.CustomData);
                        var gmb2 = new GameObject(data.Name);
                        if (dat2.rotateAnimation)
                        {
                            var ac = gmb2.AddComponent<AnimationRotate>();
                            ac.speed = dat2.rotateAnimationSpeed;
                        }
                        return gmb2;
                    case ObjectType.Collider:
                        var gmb = new GameObject(data.Name);
                        gmb.AddComponent<BoxCollider>();
                        return gmb;
                    default:

                        return new GameObject(data.Name);
                }
            }
        }

        public static bool LoadSchematic(string schematicName, string pathName, Vector3 position, Vector3 rotation, Vector3 forceScale, ItemType forceItem = ItemType.None, bool canForceScale = false)
        {
            try
            {
                SaveDataObjectList blocks = (SaveDataObjectList)JsonConvert.DeserializeObject<SaveDataObjectList>(File.ReadAllText(pathName));
                UnloadSchematic(schematicName);
                GameObject ob = new GameObject(schematicName);
                CreateRecursiveFromID(blocks.blocks[0].DataID, blocks, forceItem, ob.transform);
                ob.transform.position = position;
                ob.transform.rotation = Quaternion.Euler(rotation);
                if (!canForceScale)
                    return true;

                ob.transform.localScale = new Vector3(ob.transform.localScale.x * forceScale.x, ob.transform.localScale.y * forceScale.y, ob.transform.localScale.z * forceScale.z);
                foreach (var pick in ob.GetComponentsInChildren<Pickup>())
                {
                    NetworkServer.UnSpawn(pick.gameObject);
                    NetworkServer.Spawn(pick.gameObject);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }

        public static void CreateRecursiveFromID(int id, SaveDataObjectList blocks, ItemType force, Transform parentGameObject)
        {
            Transform childGameObjectTransform = CreateGameObjectFromGameObjectSaveData(blocks.BlockWithinstanceID(id), blocks.SchematicName, force, parentGameObject); // Create the object first before creating children.
            foreach (var block in blocks.BlocksWithParentID(id))
            {
                CreateRecursiveFromID(block.DataID, blocks, force, childGameObjectTransform); // The child now becomes the parent
            }
        }
        public static Transform CreateGameObjectFromGameObjectSaveData(SchematicData block, string schemName, ItemType force, Transform parentGameObject)
        {
            bool isNetwork = false;
            GameObject newGameObject = init.CreateObject(block, schemName, force, ref isNetwork);

            newGameObject.transform.parent = parentGameObject;
            newGameObject.transform.position = block.Position.GetJsonVector();
            newGameObject.transform.rotation = Quaternion.Euler(block.Rotation.GetJsonVector());
            newGameObject.transform.localScale = block.Scale.GetJsonVector();
            if (isNetwork)
            {
                NetworkServer.Spawn(newGameObject);
            }
            newGameObject.name = block.Name;
            return newGameObject.transform;
        }

        internal void PickupItem(PickupItemEvent ev)
        {
            if (ev.Pickup.gameObject.GetComponent<DisablePickupInteract>() != null)
            {
                ev.Allowed = false;
                if (ev.Pickup.gameObject.TryGetComponent(out PickupEvent ev2))
                {
                    PickupInteractEvent.Invoke(new PickupInteractEvent()
                    {
                        EventName = ev2.EventName,
                        SchematicName = ev2.SchematicName,
                        Pickup = ev.Pickup
                    });
                }
            }
        }

        public static bool UnloadSchematic(string schematicName)
        {
            if (IfSchematicLoaded(schematicName))
            {
                GameObject go = GameObject.Find($"{schematicName}");
                UnityEngine.Object.DestroyImmediate(go);
                return true;
            }
            return false;
        }

        public static bool BringSchematic(string schematicName, Player plr)
        {
            if (IfSchematicLoaded(schematicName))
            {
                GameObject go = GameObject.Find($"{schematicName}");
                go.transform.position = plr.Position;
                return true;
            }
            return false;
        }

        public static bool SetSchematicPosition(string schematicName, Vector3 position)
        {
            if (IfSchematicLoaded(schematicName))
            {
                GameObject go = GameObject.Find($"{schematicName}");
                go.transform.position = position;
                return true;
            }
            return false;
        }

        public static bool IfSchematicLoaded(string schematicName)
        {
            GameObject go = GameObject.Find($"{schematicName}");
            if (go == null)
                return false;
            else
                return true;
        }



    }
}