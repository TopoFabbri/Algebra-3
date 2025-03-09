using System;
using CustomMath;
using UnityEngine;
using UnityEngine.Internal;

[Serializable]
public struct Quat : IEquatable<Quat>, IFormattable
{
    public override bool Equals(object obj)
    {
        return obj is Quat other && Equals(other);
    }

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
    public Quat(float x, float y, float z, float w)
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
            return index switch
            {
                0 => x,
                1 => y,
                2 => z,
                3 => w,
                _ => throw new IndexOutOfRangeException("Invalid index")
            };
        }
        set
        {
            switch (index)
            {
                case 0:
                    x = value;
                    break;
                case 1:
                    y = value;
                    break;
                case 2:
                    z = value;
                    break;
                case 3:
                    w = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid index");
            }
        }
    }

    /// <summary>
    ///   <para>The identity rotation (Read Only).</para>
    /// </summary>
    public static Quat Identity => new(0f, 0f, 0f, 1f);

    /// <summary>
    ///   <para>Returns or sets the euler angle representation of the rotation.</para>
    /// </summary>
    public Vec3 EulerAngles
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
    public Quat Normalized => Normalize(new Quat(x, y, z, w));

    #endregion

    #region Functions

    /// <summary>
    ///   <para>Returns the angle in degrees between two rotations a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static float Angle(Quat a, Quat b)
    {
        float dot = Mathf.Abs(Dot(a, b));
        return Mathf.Acos(dot) * 2f * Mathf.Rad2Deg;
    }

    /// <summary>
    ///   <para>Creates a rotation which rotates angle degrees around axis.</para>
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="axis"></param>
    public static Quat AngleAxis(float angle, Vec3 axis)
    {
        axis.Normalize();
        axis *= Mathf.Sin(angle * .5f * Mathf.Deg2Rad);
        Quat q = new(axis.x, axis.y, axis.z, Mathf.Cos(angle * .5f * Mathf.Deg2Rad));

        return q;
    }

    /// <summary>
    ///   <para>The dot product between two rotations.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static float Dot(Quat a, Quat b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    /// <summary>
    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis.</para>
    /// </summary>
    /// <param name="euler"></param>
    public static Quat Euler(Vec3 euler)
    {
        float xRad = euler.x * Mathf.Deg2Rad;
        float yRad = euler.y * Mathf.Deg2Rad;
        float zRad = euler.z * Mathf.Deg2Rad;

        float cX = Mathf.Cos(xRad / 2f);
        float cY = Mathf.Cos(yRad / 2f);
        float cZ = Mathf.Cos(zRad / 2f);
        float sX = Mathf.Sin(xRad / 2f);
        float sY = Mathf.Sin(yRad / 2f);
        float sZ = Mathf.Sin(zRad / 2f);

        return new Quat(
            sX * cY * cZ + cX * sY * sZ,
            cX * sY * cZ - sX * cY * sZ,
            cX * cY * sZ + sX * sY * cZ,
            cX * cY * cZ - sX * sY * sZ
        );
    }

    /// <summary>
    ///   <para>Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis; applied in that order.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static Quat Euler(float x, float y, float z) => Euler(new Vec3(x, y, z)); 

    /// <summary>
    ///   <para>Creates a rotation which rotates from fromDirection to toDirection.</para>
    /// </summary>
    /// <param name="fromDirection"></param>
    /// <param name="toDirection"></param>
    public static Quat FromToRotation(Vec3 fromDirection, Vec3 toDirection)
    {
        Vec3 axis = Vec3.Cross(fromDirection, toDirection);
        Quat rotation = AngleAxis(Vec3.Angle(fromDirection, toDirection), axis);

        return rotation;
    }

    /// <summary>
    ///   <para>Returns the Inverse of rotation.</para>
    /// </summary>
    /// <param name="rotation"></param>
    public static Quat Inverse(Quat rotation)
    {
        return new Quat(-rotation.x, -rotation.y, -rotation.z, rotation.w);
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
    public static Quat Lerp(Quat a, Quat b, float t)
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
    public static Quat LerpUnclamped(Quat a, Quat b, float t)
    {
        float tNeg = 1f - t;
        Quat res;

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

        return res.Normalized;
    }

    /// <summary>
    ///   <para>Creates a rotation with the specified forward and upwards directions.</para>
    /// </summary>
    /// <param name="forward">The direction to look in.</param>
    /// <param name="upwards">The vector that defines in which direction up is.</param>
    public static Quat LookRotation(Vec3 forward, [DefaultValue("Vec3.up")] Vec3 upwards)
    {
        forward.Normalize();
        Vec3 right = Vec3.Cross(upwards, forward).normalized;
        Vec3 u = Vec3.Cross(forward, right);
        
        // Build a rotation matrix with r, u, and f as the columns.
        //
        //    | r.x   u.x   f.x |
        //    | r.y   u.y   f.y |
        //    | r.z   u.z   f.z |
        
        float m00 = right.x, m01 = u.x, m02 = forward.x;
        float m10 = right.y, m11 = u.y, m12 = forward.y;
        float m20 = right.z, m21 = u.z, m22 = forward.z;
        
        // Compute the trace of the matrix.
        float trace = m00 + m11 + m22;
        float qw, qx, qy, qz;
        
        if (trace > 0)
        {
            float s = 0.5f / MathF.Sqrt(trace + 1.0f);
            qw = 0.25f / s;
            qx = (m21 - m12) * s;
            qy = (m02 - m20) * s;
            qz = (m10 - m01) * s;
        }
        else
        {
            if (m00 > m11 && m00 > m22)
            {
                float s = 2.0f * MathF.Sqrt(1.0f + m00 - m11 - m22);
                qw = (m21 - m12) / s;
                qx = 0.25f * s;
                qy = (m01 + m10) / s;
                qz = (m02 + m20) / s;
            }
            else if (m11 > m22)
            {
                float s = 2.0f * MathF.Sqrt(1.0f + m11 - m00 - m22);
                qw = (m02 - m20) / s;
                qx = (m01 + m10) / s;
                qy = 0.25f * s;
                qz = (m12 + m21) / s;
            }
            else
            {
                float s = 2.0f * MathF.Sqrt(1.0f + m22 - m00 - m11);
                qw = (m10 - m01) / s;
                qx = (m02 + m20) / s;
                qy = (m12 + m21) / s;
                qz = 0.25f * s;
            }
        }
        
        return new Quat(qx, qy, qz, qw);
    }

    /// <summary>
    ///   <para>Converts this quaternion to one with the same orientation but with a magnitude of 1.</para>
    /// </summary>
    /// <param name="q"></param>
    public static Quat Normalize(Quat q)
    {
        float length = Mathf.Abs(Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z));
        return new Quat(q.x / length, q.y / length, q.z / length, q.w / length);
    }

    /// <summary>
    ///   <para>Rotates a rotation from towards to.</para>
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="maxDegreesDelta"></param>
    public static Quat RotateTowards(Quat from, Quat to, float maxDegreesDelta)
    {
        return Slerp(from, to, maxDegreesDelta / Angle(from, to) * Mathf.Rad2Deg);
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
    public static Quat Slerp(Quat a, Quat b, float t)
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
    public static Quat SlerpUnclamped(Quat a, Quat b, float t)
    {
        float angle = Angle(a, b);

        float tNeg = 1f - t;
        float s1 = (float)Math.Sin(tNeg * angle) * (1f / Mathf.Sin(angle));
        float s2 = (float)Math.Sin(t * angle) * (1f / Mathf.Sin(angle));

        Quat ans;

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
    public void SetLookRotation(Vec3 view, [DefaultValue("Vec3.up")] Vec3 up) => this = LookRotation(view, up);
    
    #endregion

    #region Operators

    public static Quat operator *(Quat q1, Quat q2)
    {
        /*
        (q1w * q2w - q1x * q2x - q1y * q2y - q1z * q2z) + -> w
        (q1w * q2x + q1x * q2w + q1y * q2z - q1z * q2y)i + -> x
        (q1w * q2y - q1x * q2z + q1y * q2w + q1z * q2x)j + -> y
        (q1w * q2z + q1x * q2y - q1y * q2x + q1z * q2w)k -> z
        */

        return new Quat(
            q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
            q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x,
            q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w,
            q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
        );
    }

    public static Vec3 operator *(Quat rotation, Vec3 point)
    {
        Quat q = rotation;
        Vec3 v = point;
        Vec3 res = Vec3.Zero;

        // v = q * q(v) * q'

        Quat qv = new(v.x, v.y, v.z, 0);
        qv = q * qv * Inverse(q);
        res = new Vec3(qv.x, qv.y, qv.z);

        return res;
    }

    public static bool operator ==(Quat lhs, Quat rhs)
    {
        return Dot(new Quat(lhs.x, lhs.y, lhs.z, lhs.w), rhs) > 1f - KEpsilon;
    }

    public static bool operator !=(Quat lhs, Quat rhs) => !(lhs == rhs);

    public static implicit operator Quat(Quaternion q)
    {
        return new Quat(q.x, q.y, q.z, q.w);
    }

    public static implicit operator Quaternion(Quat q)
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

    public bool Equals(Quat other)
    {
        return new Quat(x, y, z, w) == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }

    #endregion
}