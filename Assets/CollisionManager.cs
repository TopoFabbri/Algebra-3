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
        meshColliders.Clear();
        meshColliders = GameObject.FindObjectsOfType<CustomMeshCollider>().ToList();
    }

    private void Update()
    {
        CheckCollisions();
    }

    void CheckCollisions()
    {
        foreach (var collider1 in meshColliders)
        {
            bool colliding = false;

            foreach (var collider2 in meshColliders)
            {
                if (collider1 == collider2)
                    continue;
                    
                if (collider1.CollidesWith(collider2.pointsInMesh))
                {
                    collider1.CollisionEnter(collider2);
                    colliding = true;
                }
            }
            
            if (!colliding)
                collider1.NoCollision();
        }
    }
}