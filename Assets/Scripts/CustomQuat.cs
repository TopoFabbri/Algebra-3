using System;
using System.Collections;
using System.Collections.Generic;
using CustomMath;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Internal;

[Serializable]
public struct CustomQuat : IEquatable<CustomQuat>, IFormattable
{
    #region Variables

    public const float KEpsilon = 1E-06F;

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

    #endregion

    #region Constructor

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

    #endregion

    #region Properties

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

            euler.z = Mathf.Atan2(2f * (w * z + x * y), 1f - 2f * (z * z + x * x));
            euler.x = -Mathf.PI / 2f + 2f * Mathf.Atan2(Mathf.Sqrt(1f + 2f * (w * x - y * z)),
                Mathf.Sqrt(1f - 2f * (w * x - y * z)));
            euler.y = Mathf.Atan2(2f * (w * y + x * z), 1f - 2f * (y * y + x * x));

            return euler * Mathf.Rad2Deg;
        }
        set => this = Euler(value);
    }

    /// <summary>
    ///   <para>Returns this quaternion with a magnitude of 1 (Read Only).</para>
    /// </summary>
    public CustomQuat normalized => Normalize(new CustomQuat(x, y, z, w));

    #endregion

    #region Functions

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
        return LerpUnclamped(a, b, t);
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
        forward = forward.normalized;
        upwards = upwards.normalized;
        
        CustomQuat q1 = FromToRotation(Vec3.Forward, forward);
        CustomQuat q2 = FromToRotation(Vec3.Up, upwards);

        return Inverse(q1 * q2);
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
    ///   <para>Rotates a rotation from towards to.</para>
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="maxDegreesDelta"></param>
    public static CustomQuat RotateTowards(CustomQuat from, CustomQuat to, float maxDegreesDelta)
    {
        if (Dot(from, to) > 1f)
        {
            to = Normalize(to);
            from = Normalize(from);
        }

        if (maxDegreesDelta < 0)
            to = Inverse(to);

        float angle = Angle(from, to);
        Mathf.Abs(maxDegreesDelta);
        maxDegreesDelta = maxDegreesDelta / angle;

        if (angle > 1f - KEpsilon)
            return to;

        return Slerp(from, to, maxDegreesDelta);
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
        return SlerpUnclamped(a, b, t);
    }

    /// <summary>
    ///   <para>Spherically interpolates between a and b by t. The parameter t is not clamped.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    public static CustomQuat SlerpUnclamped(CustomQuat a, CustomQuat b, float t)
    {
        float angle = Angle(a, b);

        float tNeg = 1f - t;
        float s1 = (float)Math.Sin(tNeg * angle) * (1f / Mathf.Sin(angle));
        float s2 = (float)Math.Sin(t * angle) * (1f / Mathf.Sin(angle));

        CustomQuat ans;

        ans.x = s1 * a.x + s2 * b.x;
        ans.y = s1 * a.y + s2 * b.y;
        ans.z = s1 * a.z + s2 * b.z;
        ans.w = s1 * a.w + s2 * b.w;

        return ans;
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

    public void ToAngleAxis(out float angle, out Vec3 axis)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Operators

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

        // v = q * q(v) * q*

        CustomQuat qv = new(v.x, v.y, v.z, 0);
        qv = q * qv * Inverse(q);
        res = new Vec3(qv.x, qv.y, qv.z);

        return res;
    }

    public static bool operator ==(CustomQuat lhs, CustomQuat rhs)
    {
        return Dot(new CustomQuat(lhs.x, lhs.y, lhs.z, lhs.w), rhs) > 1f - KEpsilon;
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

    #endregion

    #region Needed methods

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

    #endregion
}