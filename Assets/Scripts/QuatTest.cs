using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatTest : MonoBehaviour
{
    [SerializeField] private CustomQuat quat1;
    [SerializeField] private CustomQuat quat2;
    [SerializeField] private Vector3 v;
    [SerializeField] private float rotationAngle;

    [Header("Tests:")]
    [SerializeField] private float dot;
    [SerializeField] private Vector3 euler;
    [SerializeField] private CustomQuat product;
    [SerializeField] private Vector3 vectorProduct;
    [SerializeField] private CustomQuat normalized;
    [SerializeField] private float angle;
    [SerializeField] private CustomQuat eulerToQuat;
    
    private void OnDrawGizmosSelected()
    {
        Quaternion q1 = quat1;
        Quaternion q2 = Quaternion.AngleAxis(rotationAngle, v);
        
        dot = Quaternion.Dot(q1, q2);
        euler = q1.eulerAngles;
        product = q1 * q2;
        vectorProduct = q1 * v;
        normalized = q1.normalized;
        angle = Quaternion.Dot(q1, q2);

        quat2 = q2;
        transform.rotation = q2;
        eulerToQuat = Quaternion.Euler(v);
        
        Draw();
    }

    private void Draw()
    {
        float lineLength = 2f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position - v.normalized * lineLength, transform.position + v.normalized * lineLength);
    }
}
