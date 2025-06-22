using UnityEngine;
using System.Collections;
var highLod : Mesh;
var lowLod : Mesh;
var distance = 10.0;

function Update()
{
    var campos = Camera.main.transform.position;
    var meshFilter : MeshFilter = GetComponent(MeshFilter);
    if ((transform.position - campos).sqrMagnitude <
        distance * distance)
    {
        if (meshFilter.sharedMesh != highLod)
            meshFilter.sharedMesh = highLod;
    }
    else
    {
        if (meshFilter.sharedMesh != lowLod)
            meshFilter.sharedMesh = lowLod;
    }
}