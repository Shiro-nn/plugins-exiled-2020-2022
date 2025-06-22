using MEC;
using Qurre.API.Controllers;
using SchematicUnity.API.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Loli.Addons.RealSpawn
{
    internal static class Chaos
    {
        internal static Scheme Car { get; private set; }
        internal static Scheme Gate { get; private set; }
        internal static SObject LeftGate { get; private set; }
        internal static SObject RightGate { get; private set; }
        internal static Transform SpawnPoint { get; private set; }
        internal static bool AlreadyAnimation { get; private set; } = false;
        internal static bool AllowSpawn { get; private set; } = false;
        internal static void Load()
        {
            Car = SchematicUnity.API.SchematicManager.LoadSchematic(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Schemes", "ChaosCar.json"),
                new(-192, 988.984f, -13.53454f), Quaternion.identity);
            Gate = SchematicUnity.API.SchematicManager.LoadSchematic(Path.Combine(Qurre.PluginManager.PluginsDirectory, "Schemes", "Gate.json"),
                new(-189.81f, 987.47f, -13.19484f), Quaternion.Euler(Vector3.up * 180));
            foreach (var v in Gate.Objects[0].Childrens)
            {
                if (v.Transform.localPosition.z < -0.5f) LeftGate = v;
                else if (v.Transform.localPosition.z > 0.5f) RightGate = v;
            }
            foreach (var v in LeftGate.Childrens)
            {
                if (v.Primitive is not PrimitiveParams prs) continue;
                prs.MovementSmoothing = 64;
                prs.Parent.Transform.gameObject.AddComponent<FixPosSmoothing>().Primitive = prs;
            }
            foreach (var v in RightGate.Childrens)
            {
                if (v.Primitive is not PrimitiveParams prs) continue;
                prs.MovementSmoothing = 64;
                prs.Parent.Transform.gameObject.AddComponent<FixPosSmoothing>().Primitive = prs;
            }
            foreach (var v in Car.Objects[0].Childrens)
            {
                if (v.Primitive is not PrimitiveParams prs) continue;
                prs.MovementSmoothing = 10;
                prs.Parent.Transform.gameObject.AddComponent<FixPosSmoothing>().Primitive = prs;
            }
            {
                var sp = new GameObject("ChaosOneTimeSpawnPoint");
                sp.transform.parent = Car.Objects[0].Transform;
                sp.transform.localPosition = new(-6.64f, 0.5f);
                sp.transform.localRotation = Quaternion.Euler(0, 90, 0);
                SpawnPoint = sp.transform;
            }
        }
        internal static void OpenGate(Action action)
        {
            if (AlreadyAnimation) return;
            AlreadyAnimation = true;
            AllowSpawn = true;
            Timing.RunCoroutine(DoRun());
            IEnumerator<float> DoRun()
            {
                Car.Transform.position = new(-192, 988.984f, -13.53454f);
                Car.Transform.rotation = Quaternion.identity;

                var _car = Car.Objects[0].Transform;
                _car.localPosition = Vector3.zero;
                _car.localRotation = Quaternion.identity;

                LeftGate.Transform.localPosition = new(0, 0, -3.375f);
                RightGate.Transform.localPosition = new(0, 0, 3.375f);

                for (float i = 3.3f; i < 9; i += 0.1f)
                {
                    LeftGate.Transform.localPosition = new(0, 0, -i);
                    RightGate.Transform.localPosition = new(0, 0, i);
                    yield return Timing.WaitForSeconds(0.05f);
                }
                LeftGate.Transform.localPosition = new(0, 0, -9);
                RightGate.Transform.localPosition = new(0, 0, 9);


                for (float i = 0; i < 7.7f; i += 0.1f)
                {
                    _car.localPosition = new(i, 0, 0);
                    yield return Timing.WaitForSeconds(0.02f);
                }
                for (float i = 0; i < 9; i += 0.05f)
                {
                    Car.Transform.rotation = Quaternion.Euler(new(0, i * 10));
                    _car.localPosition = new(7.7f + i, 0, i);
                    yield return Timing.WaitForSeconds(0.02f);
                }

                for (float i = 17f; i < 38.3f; i += 0.1f)
                {
                    _car.localPosition = new(i, 0, _car.localPosition.z);
                    yield return Time.deltaTime;
                }
                for (float i = 0; i < 9; i += 0.1f)
                {
                    _car.localRotation = Quaternion.Euler(new(0, -(i * 10)));
                    _car.localPosition = new(38.3f + i, 0, 9 + i);
                    yield return Time.deltaTime / 10;
                }
                yield return Timing.WaitForSeconds(1f);
                AllowSpawn = false;
                yield return Timing.WaitForSeconds(1f);
                try { action(); } catch(Exception e) { Qurre.Log.Error(e); }

                yield return Timing.WaitForSeconds(3f);

                for (float i = 0; i < 4.5f; i += 0.1f)
                {
                    _car.localRotation = Quaternion.Euler(new(0, -90 - (i * 10)));
                    _car.localPosition = new(47.3f - i, 0, 18 + i);
                    yield return Timing.WaitForSeconds(0.05f);
                }
                yield return Timing.WaitForSeconds(0.5f);
                for (float i = 0; i < 4.5f; i += 0.1f)
                {
                    _car.localRotation = Quaternion.Euler(new(0, -135 - (i * 10)));
                    _car.localPosition = new(42.8f - i, 0, 22.5f - (i * 3));
                    yield return Timing.WaitForSeconds(0.05f);
                }
                yield return Timing.WaitForSeconds(0.5f);

                for (float i = 38.3f; i > 9; i -= 0.1f)
                {
                    _car.localPosition = new(i, 0, 9);
                    yield return Time.deltaTime;
                }

                for (float i = 0; i < 9; i += 0.05f)
                {
                    Car.Transform.rotation = Quaternion.Euler(new(0, 90 - (i * 10)));
                    _car.localPosition = new(9 - i, 0, 9 - i);
                    yield return Timing.WaitForSeconds(0.02f);
                }
                yield return Timing.WaitForSeconds(0.5f);
                for (float i = 0; i < 11; i += 0.1f)
                {
                    _car.localPosition = new(-i, 0, 0);
                    yield return Timing.WaitForSeconds(0.02f);
                }

                LeftGate.Transform.localPosition = new(0, 0, -9);
                RightGate.Transform.localPosition = new(0, 0, 9);

                for (float i = 9; i > 3.4f; i -= 0.1f)
                {
                    LeftGate.Transform.localPosition = new(0, 0, -i);
                    RightGate.Transform.localPosition = new(0, 0, i);
                    yield return Timing.WaitForSeconds(0.05f);
                }
                LeftGate.Transform.localPosition = new(0, 0, -3.375f);
                RightGate.Transform.localPosition = new(0, 0, 3.375f);

                for (float i = 0; i < 11; i += 0.1f)
                {
                    _car.localPosition = new(-11 + i, 0, 0);
                    _car.localRotation = Quaternion.Euler(0, 180 - (180 / 11 * i), 0);
                    yield return Timing.WaitForSeconds(0.01f);
                }
                AlreadyAnimation = false;
                yield break;
            }
        }
        public class FixPosSmoothing : MonoBehaviour
        {
            internal PrimitiveParams Primitive;
            private void Update()
            {
                Primitive.Base.Base.NetworkMovementSmoothing = Primitive.Base.MovementSmoothing;
                Primitive.Base.Base.NetworkRotation = new LowPrecisionQuaternion(Primitive.Parent.Transform.rotation);
                Primitive.Base.Base.NetworkPosition = Primitive.Parent.Transform.position;
            }
        }
    }
}