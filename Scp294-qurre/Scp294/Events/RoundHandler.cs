using Qurre.API;
using Qurre.API.Objects;
using UnityEngine;

using RoundEvents = Qurre.Events.Round;

namespace Scp294.Events
{
    public sealed class RoundHandler
    {
        internal void RegisterEvents()
        {
            RoundEvents.Waiting += OnWaiting;
        }

        internal void UnregisterEvents()
        {
            RoundEvents.Waiting -= OnWaiting;
        }

        public void OnWaiting()
        {
            var roomTransform = RoomType.EzUpstairsPcs.GetRoom().Transform;
            var scp = new API.Scp294();

            scp.Spawn(Vector3.zero, Vector3.zero);

            scp.Transform.parent = roomTransform;
            scp.Transform.localPosition = new Vector3(9.73511f, 5.517578f, 3.399986f);
            scp.Transform.localRotation = Quaternion.Euler(Vector3.up * 180);
        }
    }
}