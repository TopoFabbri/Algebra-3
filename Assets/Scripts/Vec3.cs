﻿using System;
using UnityEngine;

namespace CustomMath
{
    [Serializable]
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Variables

        public float x;
        public float y;
        public float z;

        public float sqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        public Vec3 normalized
        {
            get
            {
                float mag = this.magnitude;
                if (mag > 0)
                {
                    return new Vec3(x / mag, y / mag, z / mag);
                }

                return new Vec3(0, 0, 0);
            }
        }

        public float magnitude
        {
            get { return Mathf.Sqrt(sqrMagnitude); }
        }

        #endregion

        #region constants

        public const float epsilon = 1e-05f;

        #endregion

        #region Default Values

        public static Vec3 Zero
        {
            get { return new Vec3(0.0f, 0.0f, 0.0f); }
        }

        public static Vec3 One
        {
            get { return new Vec3(1.0f, 1.0f, 1.0f); }
        }

        public static Vec3 Forward
        {
            get { return new Vec3(0.0f, 0.0f, 1.0f); }
        }

        public static Vec3 Back
        {
            get { return new Vec3(0.0f, 0.0f, -1.0f); }
        }

        public static Vec3 Right
        {
            get { return new Vec3(1.0f, 0.0f, 0.0f); }
        }

        public static Vec3 Left
        {
            get { return new Vec3(-1.0f, 0.0f, 0.0f); }
        }

        public static Vec3 Up
        {
            get { return new Vec3(0.0f, 1.0f, 0.0f); }
        }

        public static Vec3 Down
        {
            get { return new Vec3(0.0f, -1.0f, 0.0f); }
        }

        public static Vec3 PositiveInfinity
        {
            get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); }
        }

        public static Vec3 NegativeInfinity
        {
            get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); }
        }

        #endregion

        #region Constructors

        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector2 v2)
        {
            this.x = v2.x;
            this.y = v2.y;
            this.z = 0.0f;
        }

        #endregion

        #region Operators

        public static bool operator ==(Vec3 left, Vec3 right)
        {
            float diffX = left.x - right.x;
            float diffY = left.y - right.y;
            float diffZ = left.z - right.z;
            
            float sqrmag = diffX * diffX + diffY * diffY + diffZ * diffZ;
            
            return sqrmag < epsilon * epsilon;
        }

        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        public static Vec3 operator +(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x + rightV3.x, leftV3.y + rightV3.y, leftV3.z + rightV3.z);
        }

        public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x - rightV3.x, leftV3.y - rightV3.y, leftV3.z - rightV3.z);
        }

        public static Vec3 operator -(Vec3 v3)
        {
            return new Vec3(-v3.x, -v3.y, -v3.z);
        }

        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }

        public static Vec3 operator *(float scalar, Vec3 v3)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }

        public static Vec3 operator /(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x / scalar, v3.y / scalar, v3.z / scalar);
        }

        public static implicit operator Vector3(Vec3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }

        public static implicit operator Vector2(Vec3 v2)
        {
            return new Vector2(v2.x, v2.y);
        }

        public static implicit operator Vec3(Vector3 v3)
        {
            return new Vec3(v3.x, v3.y, v3.z);
        }

        public static implicit operator Vec3(Vector2 v3)
        {
            return new Vec3(v3.x, v3.y, 0f);
        }

        #endregion

        #region Functions

        public override string ToString()
        {
            return "X = " + x.ToString() + "   Y = " + y.ToString() + "   Z = " + z.ToString();
        }

        public static float Angle(Vec3 from, Vec3 to)
        {
            float dot = Vector3.Dot(from.normalized, to.normalized);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            return angle;
        }

        public static Vec3 ClampMagnitude(Vec3 vector, float maxLength)
        {
            float sqrMagnitude = vector.sqrMagnitude;

            if (sqrMagnitude > maxLength * maxLength)
            {
                float magnitude = Mathf.Sqrt(sqrMagnitude);
                vector *= maxLength / magnitude;
            }

            return vector;
        }

        public static float Magnitude(Vec3 vector)
        {
            return Mathf.Sqrt(Vector3.Dot(vector, vector));
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            Vec3 cross;
            
            cross.x = a.y * b.z - a.z * b.y;
            cross.y = a.z * b.x - a.x * b.z;
            cross.z = a.x * b.y - a.y * b.x;

            return cross;
        }

        public static float Distance(Vec3 a, Vec3 b)
        {
            return Mathf.Sqrt((b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y) + (b.z - a.z) * (b.z - a.z));
        }

        public static float Dot(Vec3 a, Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vec3(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t
            );
        }

        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
        {
            return new Vec3(
                a.x + (b.x - a.x) * t,
                a.y + (b.y - a.y) * t,
                a.z + (b.z - a.z) * t
            );
        }

        public static Vec3 Max(Vec3 a, Vec3 b)
        {
            float newX = a.x > b.x ? a.x : b.x;
            float newY = a.y > b.y ? a.y : b.y;
            float newZ = a.z > b.z ? a.z : b.z;

            return new Vec3(newX, newY, newZ);
        }

        public static Vec3 Min(Vec3 a, Vec3 b)
        {
            float newX = a.x < b.x ? a.x : b.x;
            float newY = a.y < b.y ? a.y : b.y;
            float newZ = a.z < b.z ? a.z : b.z;

            return new Vec3(newX, newY, newZ);
        }

        public static float SqrMagnitude(Vec3 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        public static Vec3 Project(Vec3 vector, Vec3 onNormal)
        {
            float sqrMag = onNormal.sqrMagnitude;
            if (sqrMag < epsilon)
                return new Vec3(0f, 0f, 0f);
            else
                return onNormal * Dot(vector, onNormal) / sqrMag;
        }

        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal)
        {
            return inDirection - 2f * Dot(inDirection, inNormal) * inNormal;
        }

        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        public void Scale(Vec3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public static Vec3 Normalize(Vec3 vec)
        {
            float mag = vec.magnitude;
            if (mag > epsilon)
            {
                vec.x /= mag;
                vec.y /= mag;
                vec.z /= mag;
            }
            else
            {
                vec.x = 0f;
                vec.y = 0f;
                vec.z = 0f;
            }

            return vec;
        }
        
        public void Normalize()
        {
            float mag = magnitude;
            if (mag > epsilon)
            {
                x /= mag;
                y /= mag;
                z /= mag;
            }
            else
            {
                x = 0f;
                y = 0f;
                z = 0f;
            }
        }
        
        #endregion

        #region Internals

        public override bool Equals(object other)
        {
            if (!(other is Vec3)) return false;
            return Equals((Vec3)other);
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        #endregion
    }
}