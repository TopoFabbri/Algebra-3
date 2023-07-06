using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using Unity.VisualScripting;
using UnityEngine;

public class CustomTransform : MonoBehaviour
{
    [SerializeField] private CustomQuat q1;
    [SerializeField] private CustomQuat q2;
    [SerializeField] private Vec3 v;
    [SerializeField] private float rotationAngle;
        
    [Header("Tests:")]
    [SerializeField] private float dot;
    [SerializeField] private Vec3 euler;
    [SerializeField] private CustomQuat product;
    [SerializeField] private Vec3 vectorProduct;
    [SerializeField] private CustomQuat normalized;
    [SerializeField] private float angle;
    [SerializeField] private CustomQuat eulerToQuat;

    private void OnDrawGizmosSelected()
    {
        q2 = CustomQuat.AngleAxis(rotationAngle, v);
        dot = CustomQuat.Dot(q1, q2);
        euler = q1.eulerAngles;
        product = q1 * q2;
        vectorProduct = q1 * v;
        normalized = q1.normalized;
        angle = CustomQuat.Dot(q1, q2);
        eulerToQuat = CustomQuat.Euler(v);

        transform.rotation = q2;
        
        Draw();
    }

    private void Draw()
    {
        float lineLength = 2f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine((Vec3)transform.position - v.normalized * lineLength, (Vec3)transform.position + v.normalized * lineLength);
    }
}