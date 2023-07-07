using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using UnityEngine;
using UnityEngine.Internal;

[Serializable]
public struct CustomQuat : IEquatable<CustomQuat>, IFormattable
{
    public const float kEpsilon = 1E-06F;

    /// <summary>
    ///   <para>X component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
    /// </summary>
    public float x;
    
    /// <summary>
    ///   <para>Y component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
    /// </summary>
    public float y;
    
    /// <summary>
    ///   <para>Z component of the Quaternion. Don't modify this directly unless you know quaternions inside out.</para>
    /// </summary>
    public float z;
    
    /// <summary>
    ///   <para>W component of the Quaternion. Do not directly modify quaternions.</para>
    /// </summary>
    public float w;

    /// <summary>
    ///   <para>Constructs new Quaternion with given x,y,z,w components.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    public CustomQuat(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    /// <summary>
    /// Set a variable by it's index
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="IndexOutOfRangeException"></exception>
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

    /// <summary>
    ///   <para>The identity rotation (Read Only).</para>
    /// </summary>
    public static CustomQuat identity => new(0f, 0f, 0f, 1f);
    
    /// <summary>
    ///   <para>Returns or sets the euler angle representation of the rotation.</para>
    /// </summary>
    public Vec3 eulerAngles
    {
        get
        {
            Vec3 euler;
            euler.x = Mathf.Asin(2f * (x * z - w * y));
            euler.y = Mathf.Atan2(2f * (x * w + y * z), 1f - 2f * (z * z + w * w));
            euler.z = Mathf.Atan2(2f * (x * y + z * w), 1f - 2f * (y * y + z * z));
            return euler * Mathf.Rad2Deg;
        }
        set => this = Euler(value);
    }
    
    /// <summary>
    ///   <para>Returns this quaternion with a magnitude of 1 (Read Only).</para>
    /// </summary>
    public CustomQuat normalized => Normalize(new CustomQuat(x, y, z, w));
    
    /// <summary>
    ///   <para>Returns the angle in degrees between two rotations a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static float Angle(CustomQuat a, CustomQuat b)
    {
        float dot = Mathf.Abs(Dot(a, b));
        return Mathf.Acos(dot) * 2f * Mathf.Rad2Deg;
    }

    /// <summary>
    ///   <para>Creates a rotation which rotates angle degrees around axis.</para>
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="axis"></param>
    public static CustomQuat AngleAxis(float angle, Vec3 axis)
    {
        axis.Normalize();
        axis *= Mathf.Sin(angle * .5f * Mathf.Deg2Rad);
        CustomQuat q = new CustomQuat(axis.x, axis.y, axis.z, Mathf.Cos(angle * .5f * Mathf.Deg2Rad));

        return q;
    }

    
    /// <summary>
    ///   <para>The dot product between two rotations.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static float Dot(CustomQuat a, CustomQuat b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    /// <summary>
    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis.</para>
    /// </summary>
    /// <param name="euler"></param>
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

    /// <summary>
    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis; applied in that order.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
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

    /// <summary>
    ///   <para>Creates a rotation which rotates from fromDirection to toDirection.</para>
    /// </summary>
    /// <param name="fromDirection"></param>
    /// <param name="toDirection"></param>
    public static CustomQuat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
    {
        Vec3 axis = Vec3.Cross(fromDirection, toDirection);
        CustomQuat rotation = AngleAxis(Vec3.Angle(fromDirection, toDirection), axis);

        return rotation;
    }

    /// <summary>
    ///   <para>Returns the Inverse of rotation.</para>
    /// </summary>
    /// <param name="rotation"></param>
    public static CustomQuat Inverse(CustomQuat rotation)
    {
        return new CustomQuat(-rotation.x, -rotation.y, -rotation.z, rotation.w);
    }

    /// <summary>
    ///   <para>Interpolates between a and b by t and normalizes the result afterwards. The parameter t is clamped to the range [0, 1].</para>
    /// </summary>
    /// <param name="a">Start value, returned when t = 0.</param>
    /// <param name="b">End value, returned when t = 1.</param>
    /// <param name="t">Interpolation ratio.</param>
    /// <returns>
    ///   <para>A quaternion interpolated between quaternions a and b.</para>
    /// </returns>
    public static CustomQuat Lerp(CustomQuat a, CustomQuat b, float t)
    {
        t = Mathf.Clamp01(t);

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

        return res.normalized;
    }

    /// <summary>
    ///   <para>Interpolates between a and b by t and normalizes the result afterwards. The parameter t is not clamped.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
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

        return res.normalized;
    }

    /// <summary>
    ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
    /// </summary>
    /// <param name="forward">The direction to look in.</param>
    /// <param name="upwards">The vector that defines in which direction up is.</param>
    public static CustomQuat LookRotation(Vec3 forward, [DefaultValue("Vec3.up")] Vec3 upwards)
    {
        forward = Vec3.Normalize(forward);
        Vec3 right = Vec3.Normalize(Vec3.Cross(upwards, forward));

        float[,] m = new[,]
        {
            { right.x, right.y, right.z },
            { upwards.x, upwards.y, upwards.z },
            { forward.x, forward.y, forward.z }
        };

        float diags = m[0, 0] + m[1, 1] + m[2, 2];
        CustomQuat q;

        if (diags > 0f)
        {
            q.x = (m[1, 2] - m[2, 1]) * (0.5f / Mathf.Sqrt(diags + 1f));
            q.y = (m[2, 0] - m[0, 2]) * (0.5f / Mathf.Sqrt(diags + 1f));
            q.z = (m[0, 1] - m[1, 0]) * (0.5f / Mathf.Sqrt(diags + 1f));
            q.w = Mathf.Sqrt(diags + 1f) / 2f;
        }
        else if (m[0, 0] >= m[1, 1] && m[0, 0] >= m[2, 2])
        {
            q.x = Mathf.Sqrt(1f + m[0, 0] - m[1, 1] - m[2, 2]) / 2f;
            q.y = (m[0, 1] + m[1, 0]) * (0.5f / Mathf.Sqrt(1f + m[0, 0] - m[1, 1] - m[2, 2]));
            q.z = (m[2, 0] + m[0, 2]) * (0.5f / Mathf.Sqrt(1f + m[0, 0] - m[1, 1] - m[2, 2]));
            q.w = (m[1, 2] - m[2, 1]) * (0.5f / Mathf.Sqrt(1f + m[0, 0] - m[1, 1] - m[2, 2]));
        }
        else if (m[1, 1] > m[2, 2])
        {
            q.x = (m[1, 0] + m[0, 1]) * 0.5f / Mathf.Sqrt(1f + m[1, 1] - m[0, 0] - m[2, 2]);
            q.y = Mathf.Sqrt(1f + m[1, 1] - m[0, 0] - m[2, 2]) / 2f;
            q.z = (m[2, 1] + m[1, 2]) * 0.5f / Mathf.Sqrt(1f + m[1, 1] - m[0, 0] - m[2, 2]);
            q.w = (m[2, 0] + m[0, 2]) * 0.5f / Mathf.Sqrt(1f + m[1, 1] - m[0, 0] - m[2, 2]);
        }
        else
        {
            q.x = (m[2, 0] + m[0, 2]) * 0.5f / Mathf.Sqrt(1f + m[2, 2] - m[0, 0] - m[1, 1]);
            q.y = (m[2, 1] + m[1, 2]) * 0.5f / Mathf.Sqrt(1f + m[2, 2] - m[0, 0] - m[1, 1]);
            q.z = Mathf.Sqrt(1f + m[2, 2] - m[0, 0] - m[1, 1]) / 2f;
            q.w = (m[0, 1] + m[1, 0]) * 0.5f / Mathf.Sqrt(1f + m[2, 2] - m[0, 0] - m[1, 1]);
        }

        return q;
    }

    /// <summary>
    ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
    /// </summary>
    /// <param name="forward">The direction to look in.</param>
    /// <param name="upwards">The vector that defines in which direction up is.</param>
    public static CustomQuat LookRotation(Vec3 forward) => LookRotation(forward, Vec3.Up);

    /// <summary>
    ///   <para>Converts this quaternion to one with the same orientation but with a magnitude of 1.</para>
    /// </summary>
    /// <param name="q"></param>
    public static CustomQuat Normalize(CustomQuat q)
    {
        float length = Mathf.Abs(Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z));
        return new CustomQuat(q.x / length, q.y / length, q.z / length, q.w / length);
    }

    /// <summary>
    ///   <para>Spherically interpolates between quaternions a and b by ratio t. The parameter t is clamped to the range [0, 1].</para>
    /// </summary>
    /// <param name="a">Start value, returned when t = 0.</param>
    /// <param name="b">End value, returned when t = 1.</param>
    /// <param name="t">Interpolation ratio.</param>
    /// <returns>
    ///   <para>A quaternion spherically interpolated between quaternions a and b.</para>
    /// </returns>
    public static CustomQuat Slerp(CustomQuat a, CustomQuat b, float t)
    {
        t = Mathf.Clamp01(t);
        
        float cosTheta = a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        float angle = Mathf.Acos(cosTheta);
        float sinTheta = Mathf.Sqrt(1f - cosTheta * cosTheta);
        float wa = Mathf.Sin((1f - t) * angle) / sinTheta;
        float wb = Mathf.Sin(t * angle) / sinTheta;

        return new CustomQuat(
            wa * a.x + wb * b.x,
            wa * a.y + wb * b.y,
            wa * a.z + wb * b.z,
            wa * a.w + wb * b.w
        );
    }
    
    /// <summary>
    ///   <para>Set x, y, z and w components of an existing Quaternion.</para>
    /// </summary>
    /// <param name="newX"></param>
    /// <param name="newY"></param>
    /// <param name="newZ"></param>
    /// <param name="newW"></param>
    public void Set(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
    }

    /// <summary>
    ///   <para>Creates a rotation which rotates from fromDirection to toDirection.</para>
    /// </summary>
    /// <param name="fromDirection"></param>
    /// <param name="toDirection"></param>
    public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection) =>
        this = FromToRotation(fromDirection, toDirection);

    /// <summary>
    ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
    /// </summary>
    /// <param name="view">The direction to look in.</param>
    /// <param name="up">The vector that defines in which direction up is.</param>
    public void SetLookRotation(Vec3 view) => this = LookRotation(view);

    /// <summary>
    ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
    /// </summary>
    /// <param name="view">The direction to look in.</param>
    /// <param name="up">The vector that defines in which direction up is.</param>
    public void SetLookRotation(Vec3 view, [DefaultValue("Vec3.up")] Vec3 up) => this = LookRotation(view, Vec3.Up);

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

    public static bool operator ==(CustomQuat lhs, CustomQuat rhs)
    {
        return Dot(new CustomQuat(lhs.x, lhs.y, lhs.z, lhs.w), rhs) > 1f - kEpsilon;
    }

    public static bool operator !=(CustomQuat lhs, CustomQuat rhs) => !(lhs == rhs);

    public static implicit operator CustomQuat(Quaternion q)
    {
        return new CustomQuat(q.x, q.y, q.z, q.w);
    }

    public static implicit operator Quaternion(CustomQuat q)
    {
        return new Quaternion(q.x, q.y, q.z, q.w);
    }

    /// <summary>
    ///   <para>Returns a formatted string for this quaternion.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"({w}, {x}, {y}, {z})";
    }

    public bool Equals(CustomQuat other)
    {
        return new CustomQuat(x, y, z, w) == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }
}