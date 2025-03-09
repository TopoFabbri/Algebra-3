using CustomMath;
using UnityEngine;

public class CustomQuatTest : MonoBehaviour
{
    [Header("Values:")] 
    [SerializeField] private Transform reference;
    [SerializeField] private Quat q1;
    [SerializeField] private Quat q2;
    [SerializeField] private Vector3 v;
    [SerializeField] private float rotationAngle;
    [SerializeField] private float lerpValue;
    [SerializeField] private float rotateTowardsValue;

    [Header("Tests:")] 
    [SerializeField] private float dot;
    [SerializeField] private Vector3 euler;
    [SerializeField] private Quat product;
    [SerializeField] private Vector3 vectorProduct;
    [SerializeField] private Quat normalized;
    [SerializeField] private float angle;
    [SerializeField] private Quat eulerToQuat;
    [SerializeField] private Quat lerp;
    [SerializeField] private Quat rotateTowards;
    [SerializeField] private Quat lookRotation;

    private void OnDrawGizmosSelected()
    {
        q1 = reference.transform.rotation;
        
        q2 = Quat.AngleAxis(rotationAngle, v);
        dot = Quat.Dot(q1, q2);
        euler = q2.EulerAngles;
        product = q1 * q2;
        vectorProduct = q1 * v;
        normalized = q1.Normalized;
        angle = Quat.Angle(q1, q2);
        eulerToQuat = Quat.Euler(v);
        lerp = Quat.SlerpUnclamped(q1, q2, lerpValue);
        rotateTowards = Quat.RotateTowards(q1, q2, rotateTowardsValue);
        lookRotation = Quat.LookRotation(v, Vec3.Forward);
        
        Draw();
    }

    private void Draw()
    {
        float lineLength = 2f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position - v.normalized * lineLength, transform.position + v.normalized * lineLength);
    }
}