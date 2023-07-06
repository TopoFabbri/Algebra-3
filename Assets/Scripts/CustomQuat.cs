using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;

[Serializable]
public struct CustomQuat : IEquatable<CustomQuat>, IFormattable
{
    public const float kEpsilon = 1E-06F;
    public float x;
    public float y;
    public float z;
    public float w;

    public CustomQuat(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public float this[int index]
    {
        get
        {
            if (index == 0) return x;
            if (index == 1) return y;
            if (index == 2) return z;
            if (index == 3) return w;
            throw new IndexOutOfRangeException("Invalid index");
        }
        set
        {
            if (index == 0) x = value;
            else if (index == 1) y = value;
            else if (index == 2) z = value;
            else if (index == 3) w = value;
            else throw new IndexOutOfRangeException("Invalid index");
        }
    }

    public static CustomQuat identity => new(0f, 0f, 0f, 1f);

    public Vec3 eulerAngles => EulerAngles(new CustomQuat(x, y, z, w));

    public CustomQuat normalized => Normalize(new CustomQuat(x, y, z, w));

    public static float Angle(CustomQuat a, CustomQuat b)
    {
        float dot = Mathf.Abs(Dot(a, b));
        return Mathf.Acos(dot) * 2f * Mathf.Rad2Deg;
    }

    public static CustomQuat AngleAxis(float angle, Vec3 axis)
    {
        axis.Normalize();
        axis *= Mathf.Sin(angle * .5f * Mathf.Deg2Rad);
        CustomQuat q = new CustomQuat(axis.x, axis.y, axis.z, Mathf.Cos(angle * .5f * Mathf.Deg2Rad));

        return q;
    }

    public static CustomQuat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
    {
        Vec3 axis = Vec3.Cross(fromDirection, toDirection);
        CustomQuat rotation = AngleAxis(Vec3.Angle(fromDirection, toDirection), axis);

        return rotation;
    }

    public static CustomQuat Inverse(CustomQuat rotation)
    {
        return new CustomQuat(-rotation.x, -rotation.y, -rotation.z, rotation.w);
    }

    public static CustomQuat LerpUnclamped(CustomQuat a, CustomQuat b, float t)
    {
        float tNeg = 1f - t;
        CustomQuat res;

        if (Dot(a, b) > 0f)
        {
            res.x = tNeg * a.x + t * b.x;
            res.y = tNeg * a.y + t * b.y;
            res.z = tNeg * a.z + t * b.z;
            res.w = tNeg * a.w + t * b.w;
        }
        else
        {
            res.x = tNeg * a.x - t * b.x;
            res.y = tNeg * a.y - t * b.y;
            res.z = tNeg * a.z - t * b.z;
            res.w = tNeg * a.w - t * b.w;
        }

        return res;
    }

    public static Vec3 EulerAngles(CustomQuat q)
    {
        float x = q.x;
        float y = q.y;
        float z = q.z;
        float w = q.w;
        Vec3 euler;

        euler.x = Mathf.Asin(2f * (x * z - w * y));
        euler.y = Mathf.Atan2(2f * (x * w + y * z), 1f - 2f * (z * z + w * w));
        euler.z = Mathf.Atan2(2f * (x * y + z * w), 1f - 2f * (y * y + z * z));

        return euler * Mathf.Rad2Deg;
    }

    public static float Dot(CustomQuat a, CustomQuat b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public static CustomQuat Euler(Vec3 euler)
    {
        CustomQuat x = new CustomQuat(
            Mathf.Sin(euler.x / 2f * Mathf.Deg2Rad),
            0f,
            0f,
            Mathf.Cos(euler.x / 2f * Mathf.Deg2Rad)
        );

        CustomQuat y = new CustomQuat(
            0f,
            Mathf.Sin(euler.y / 2f * Mathf.Deg2Rad),
            0f,
            Mathf.Cos(euler.y / 2f * Mathf.Deg2Rad)
        );

        CustomQuat z = new CustomQuat(
            0f,
            0f,
            Mathf.Sin(euler.x / 2f * Mathf.Deg2Rad),
            Mathf.Cos(euler.x / 2f * Mathf.Deg2Rad)
        );

        return x * y * z;
    }

    public static CustomQuat Euler(float x, float y, float z)
    {
        CustomQuat qx = new CustomQuat(
            Mathf.Sin(x / 2f * Mathf.Deg2Rad),
            0f,
            0f,
            Mathf.Cos(x / 2f * Mathf.Deg2Rad)
        );

        CustomQuat qy = new CustomQuat(
            0f,
            Mathf.Sin(y / 2f * Mathf.Deg2Rad),
            0f,
            Mathf.Cos(y / 2f * Mathf.Deg2Rad)
        );

        CustomQuat qz = new CustomQuat(
            0f,
            0f,
            Mathf.Sin(x / 2f * Mathf.Deg2Rad),
            Mathf.Cos(x / 2f * Mathf.Deg2Rad)
        );

        return qx * qy * qz;
    }

    public static CustomQuat Normalize(CustomQuat q)
    {
        float length = Mathf.Abs(Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z));
        return new CustomQuat(q.x / length, q.y / length, q.z / length, q.w / length);
    }

    public static CustomQuat operator *(CustomQuat q1, CustomQuat q2)
    {
        /*
        (q1w * q2w - q1x * q2x - q1y * q2y - q1z * q2z) + -> w
        (q1w * q2x + q1x * q2w + q1y * q2z - q1z * q2y)i + -> x
        (q1w * q2y - q1x * q2z + q1y * q2w + q1z * q2x)j + -> y
        (q1w * q2z + q1x * q2y - q1y * q2x + q1z * q2w)k -> z
        */

        return new CustomQuat(
            (float)(q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y),
            (float)(q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x),
            (float)(q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w),
            (float)(q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z)
        );
    }

    public static Vec3 operator *(CustomQuat rotation, Vec3 point)
    {
        CustomQuat q = rotation;
        Vec3 v = point;
        Vec3 res = Vec3.Zero;

        res.x = v.x * (q.x * q.x + q.w * q.w - q.y * q.y - q.z * q.z) + v.y * (2f * q.x * q.y - 2f * q.w * q.z) +
                v.z * (2f * q.x * q.z + 2f * q.w * q.y);
        res.y = v.x * (2f * q.w * q.z + 2f * q.x * q.y) + v.y * (q.w * q.w - q.x * q.x + q.y * q.y - q.z * q.z) +
                v.z * (-2f * q.w * q.x + 2f * q.y * q.z);
        res.z = v.x * (-2 * q.w * q.y + 2 * q.x * q.z) + v.y * (2 * q.w * q.x + 2 * q.y * q.z) +
                v.z * (q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z);
        return res;

        // CustomQuat p = new(point.x, point.y, point.z, 0f);
        // CustomQuat qConj = new(-rotation.x, -rotation.y, -rotation.z, rotation.w);
        // CustomQuat res = p * rotation * qConj;
        //
        // return new Vec3(res.x, res.y, res.z);
    }

    public static implicit operator CustomQuat(Quaternion q)
    {
        return new CustomQuat(q.x, q.y, q.z, q.w);
    }

    public static implicit operator Quaternion(CustomQuat q)
    {
        return new Quaternion(q.x, q.y, q.z, q.w);
    }

    public bool Equals(CustomQuat other)
    {
        return Dot(new CustomQuat(x, y, z, w), other) > 1 - kEpsilon;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"({w}, {x}, {y}, {z})";
    }
}