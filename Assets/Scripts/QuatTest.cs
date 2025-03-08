using UnityEngine;

public class QuatTest : MonoBehaviour
{
    [Header("Values:")] 
    [SerializeField] private Transform reference;
    [SerializeField] private CustomQuat quat1;
    [SerializeField] private CustomQuat quat2;
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
        quat1 = reference.transform.rotation;
        Quaternion q1 = quat1;

        Quaternion q2 = Quaternion.AngleAxis(rotationAngle, v);
        dot = Quaternion.Dot(q1, q2);
        euler = q2.eulerAngles;
        product = q1 * q2;
        vectorProduct = q1 * v;
        normalized = q1.normalized;
        angle = Quaternion.Angle(q1, q2);
        quat2 = q2;
        eulerToQuat = Quaternion.Euler(v);
        lerp = Quaternion.SlerpUnclamped(q1, q2, lerpValue);
        rotateTowards = Quaternion.RotateTowards(q1, q2, rotateTowardsValue);
        lookRotation = Quaternion.LookRotation(v, Vector3.forward);
        
        Draw();
    }

    private void Draw()
    {
        float lineLength = 2f;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position - v.normalized * lineLength, transform.position + v.normalized * lineLength);
    }
}