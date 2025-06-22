using UnityEngine;
namespace SLCustomObjects
{
    public class AnimationRotate : MonoBehaviour
    {
        public float speed = 3f;
        public void Update()
        {
            this.transform.Rotate(Vector3.left, Time.deltaTime * speed);
        }
    }
}