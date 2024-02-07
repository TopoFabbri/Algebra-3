using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomQuatTest : MonoBehaviour
{
    [Header("Values:")] 
    [SerializeField] private Transform reference;
    [SerializeField] private CustomQuat q1;
    [SerializeField] private CustomQuat q2;
    [SerializeField] private Vector3 v;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float lerpValue;
    [SerializeField] private float rotateTowardsValue;

    [Header("Tests:")] 
    [SerializeField] private float dot;
    [SerializeField] private Vector3 euler;
    [SerializeField] private CustomQuat product;
    [SerializeField] private Vector3 vectorProduct;
    [SerializeField] private CustomQuat normalized;
    [SerializeField] private float angle;
    [SerializeField] private CustomQuat eulerToQuat;
    [SerializeField] private CustomQuat lerp;
    [SerializeField] private CustomQuat rotateTowards;
    [SerializeField] private CustomQuat lookRotation;

    private void OnDrawGizmosSelected()
    {
        q1 = reference.transform.rotation;
        
        q2 = CustomQuat.AngleAxis(rotationAngle, v);
        dot = CustomQuat.Dot(q1, q2);
        euler = q2.EulerAngles;
        product = q1 * q2;
        vectorProduct = q1 * v;
        normalized = q1.Normalized;
        angle = CustomQuat.Angle(q1, q2);
        eulerToQuat = CustomQuat.Euler(v);
        lerp = CustomQuat.SlerpUnclamped(q1, q2, lerpValue);
        rotateTowards = CustomQuat.RotateTowards(q1, q2, rotateTowardsValue);
        lookRotation = CustomQuat.LookRotation(v, Vec3.Right);

        transform.rotation = q2;
        Draw();
    }

    private void Draw()
    {
        float lineLength = 2f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position - v.normalized * lineLength, transform.position + v.normalized * lineLength);
    }
}