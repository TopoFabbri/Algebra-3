using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomTransform : MonoBehaviour
{
    [SerializeField] private CustomQuat q1;
    [SerializeField] private CustomQuat q2;
    [SerializeField] private Vec3 v;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float lerpValue;

    [Header("Tests:")]
    [SerializeField] private float dot;
    [SerializeField] private Vec3 euler;
    [SerializeField] private CustomQuat product;
    [SerializeField] private Vec3 vectorProduct;
    [SerializeField] private CustomQuat normalized;
    [SerializeField] private float angle;
    [SerializeField] private CustomQuat eulerToQuat;
    [SerializeField] private CustomQuat lerp;

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
        lerp = CustomQuat.Lerp(q1, q2, lerpValue);

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