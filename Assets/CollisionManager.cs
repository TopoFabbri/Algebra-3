using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private List<CustomMeshCollider> meshColliders = new List<CustomMeshCollider>();
    
    private void OnValidate()
    {
        if (meshColliders.Count > 0)
        {
            meshColliders = GameObject.FindObjectsOfType<CustomMeshCollider>().ToList();
        }
    }
}
