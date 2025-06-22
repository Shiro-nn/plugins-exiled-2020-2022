using UnityEngine;
using System.Collections;
public class SmoothStep : MonoBehaviour
{
    public Transform target;
    private float startPos;
    private float endPos;
    void Start()
    {
        startPos = transform.position.z;
        endPos = target.position.z;
    }
    void Update()
    {
        float z = Mathf.SmoothStep(startPos, endPos, Time.time / 5);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}