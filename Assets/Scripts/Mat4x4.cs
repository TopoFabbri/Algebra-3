using System;
using System.Text;
using CustomMath;
using UnityEngine;

[Serializable]
public struct Mat4x4 : IEquatable<Mat4x4>, IFormattable
{
    #region Variables

    public float m00;
    public float m10;
    public float m20;
    public float m30;
    public float m01;
    public float m11;
    public float m21;
    public float m31;
    public float m02;
    public float m12;
    public float m22;
    public float m32;
    public float m03;
    public float m13;
    public float m23;
    public float m33;

    #endregion

    #region Constructor

    public Mat4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
    {
        m00 = column0.x;
        m10 = column0.y;
        m20 = column0.z;
        m30 = column0.w;

        m01 = column1.x;
        m11 = column1.y;
        m21 = column1.z;
        m31 = column1.w;

        m02 = column2.x;
        m12 = column2.y;
        m22 = column2.z;
        m32 = column2.w;

        m03 = column3.x;
        m13 = column3.y;
        m23 = column3.z;
        m33 = column3.w;
    }

    #endregion

    #region Properties

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return m00;
                case 1: return m10;
                case 2: return m20;
                case 3: return m30;
                case 4: return m01;
                case 5: return m11;
                case 6: return m21;
                case 7: return m31;
                case 8: return m02;
                case 9: return m12;
                case 10: return m22;
                case 11: return m32;
                case 12: return m03;
                case 13: return m13;
                case 14: return m23;
                case 15: return m33;
                default: throw new IndexOutOfRangeException("Invalid Matrix index!");
            }
        }

        set
        {
            switch (index)
            {
                case 0:
                    m00 = value;
                    break;
                case 1:
                    m10 = value;
                    break;
                case 2:
                    m20 = value;
                    break;
                case 3:
                    m30 = value;
                    break;
                case 4:
                    m01 = value;
                    break;
                case 5:
                    m11 = value;
                    break;
                case 6:
                    m21 = value;
                    break;
                case 7:
                    m31 = value;
                    break;
                case 8:
                    m02 = value;
                    break;
                case 9:
                    m12 = value;
                    break;
                case 10:
                    m22 = value;
                    break;
                case 11:
                    m32 = value;
                    break;
                case 12:
                    m03 = value;
                    break;
                case 13:
                    m13 = value;
                    break;
                case 14:
                    m23 = value;
                    break;
                case 15:
                    m33 = value;
                    break;
                default: throw new IndexOutOfRangeException("Invalid Matrix index!");
            }
        }
    }

    public float this[int row, int column]
    {
        get => this[row + column * 4];

        set => this[row + column * 4] = value;
    }

    public static Mat4x4 zero => new(
        new Vector4(0, 0, 0, 0),
        new Vector4(0, 0, 0, 0),
        new Vector4(0, 0, 0, 0),
        new Vector4(0, 0, 0, 0)
    );

    public static Mat4x4 identity => new(
        new Vector4(1f, 0f, 0f, 0f),
        new Vector4(0f, 1f, 0f, 0f),
        new Vector4(0f, 0f, 1f, 0f),
        new Vector4(0f, 0f, 0f, 1f)
    );

    public Quat Rotation => GetRotation();

    public Vec3 LossyScale => GetLossyScale();

    public bool IsIdentity => this == identity;

    public float Determinant => GetDeterminant(this);

    public Mat4x4 Transpose => GetTranspose(this);

    #endregion

    #region Functions

    private Quat GetRotation()
    {
        float trace = m00 + m11 + m22;

        float qw, qx, qy, qz;

        if (trace > 0)
        {
            float s = 0.5f / Mathf.Sqrt(trace + 1.0f);
            qw = 0.25f / s;
            qx = (m21 - m12) * s;
            qy = (m02 - m20) * s;
            qz = (m10 - m01) * s;
        }
        else
        {
            if (m00 > m11 && m00 > m22)
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + m00 - m11 - m22);
                qw = (m21 - m12) / s;
                qx = 0.25f * s;
                qy = (m01 + m10) / s;
                qz = (m02 + m20) / s;
            }
            else if (m11 > m22)
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + m11 - m00 - m22);
                qw = (m02 - m20) / s;
                qx = (m01 + m10) / s;
                qy = 0.25f * s;
                qz = (m12 + m21) / s;
            }
            else
            {
                float s = 2.0f * Mathf.Sqrt(1.0f + m22 - m00 - m11);
                qw = (m10 - m01) / s;
                qx = (m02 + m20) / s;
                qy = (m12 + m21) / s;
                qz = 0.25f * s;
            }
        }

        return new Quat(qx, qy, qz, qw);
    }

    public Vector3 GetLossyScale()
    {
        // Extract the basis vectors from the matrix
        Vector3 scaleX = new(m00, m10, m20);
        Vector3 scaleY = new(m01, m11, m21);
        Vector3 scaleZ = new(m02, m12, m22);

        // Compute the magnitudes (lengths) of the basis vectors
        float scaleXLength = scaleX.magnitude;
        float scaleYLength = scaleY.magnitude;
        float scaleZLength = scaleZ.magnitude;

        // Return the scale as a Vector3
        return new Vector3(scaleXLength, scaleYLength, scaleZLength);
    }

    public static float GetDeterminant(Mat4x4 m)
    {
        float det1 = m.m00 * (m.m11 * m.m22 * m.m33 + m.m12 * m.m23 * m.m31 + m.m13 * m.m21 * m.m32
                              - m.m13 * m.m22 * m.m31 - m.m12 * m.m21 * m.m33 - m.m11 * m.m23 * m.m32);

        float det2 = m.m01 * (m.m10 * m.m22 * m.m33 + m.m12 * m.m23 * m.m30 + m.m13 * m.m20 * m.m32
                              - m.m13 * m.m22 * m.m30 - m.m12 * m.m20 * m.m33 - m.m10 * m.m23 * m.m32);

        float det3 = m.m02 * (m.m10 * m.m21 * m.m33 + m.m11 * m.m23 * m.m30 + m.m13 * m.m20 * m.m31
                              - m.m13 * m.m21 * m.m30 - m.m11 * m.m20 * m.m33 - m.m10 * m.m23 * m.m31);

        float det4 = m.m03 * (m.m10 * m.m21 * m.m32 + m.m11 * m.m22 * m.m30 + m.m12 * m.m20 * m.m31
                              - m.m12 * m.m21 * m.m30 - m.m11 * m.m20 * m.m32 - m.m10 * m.m22 * m.m31);

        return det1 - det2 + det3 - det4;
    }

    public static Mat4x4 Inverse(Mat4x4 m)
    {
        float determinant = m.Determinant;

        if (Mathf.Abs(determinant) < Mathf.Epsilon)
            throw new InvalidOperationException("Matrix is not invertible.");

        Mat4x4 inv = new()
        {
            // Calculate the inverse matrix using adjoint method
            m00 = m.m11 * m.m22 * m.m33 - m.m11 * m.m23 * m.m32 - m.m21 * m.m12 * m.m33 + m.m21 * m.m13 * m.m32 +
                m.m31 * m.m12 * m.m23 - m.m31 * m.m13 * m.m22,
            m01 = -m.m01 * m.m22 * m.m33 + m.m01 * m.m23 * m.m32 + m.m21 * m.m02 * m.m33 - m.m21 * m.m03 * m.m32 -
                m.m31 * m.m02 * m.m23 + m.m31 * m.m03 * m.m22,
            m02 = m.m01 * m.m12 * m.m33 - m.m01 * m.m13 * m.m32 - m.m11 * m.m02 * m.m33 + m.m11 * m.m03 * m.m32 +
                m.m31 * m.m02 * m.m13 - m.m31 * m.m03 * m.m12,
            m03 = -m.m01 * m.m12 * m.m23 + m.m01 * m.m13 * m.m22 + m.m11 * m.m02 * m.m23 - m.m11 * m.m03 * m.m22 -
                m.m21 * m.m02 * m.m13 + m.m21 * m.m03 * m.m12,
            
                m10 = -m.m10 * m.m22 * m.m33 + m.m10 * m.m23 * m.m32 + m.m20 * m.m12 * m.m33 - m.m20 * m.m13 * m.m32 -
                m.m30 * m.m12 * m.m23 + m.m30 * m.m13 * m.m22,
            m11 = m.m00 * m.m22 * m.m33 - m.m00 * m.m23 * m.m32 - m.m20 * m.m02 * m.m33 + m.m20 * m.m03 * m.m32 +
                m.m30 * m.m02 * m.m23 - m.m30 * m.m03 * m.m22,
            m12 = -m.m00 * m.m12 * m.m33 + m.m00 * m.m13 * m.m32 + m.m10 * m.m02 * m.m33 - m.m10 * m.m03 * m.m32 -
                m.m30 * m.m02 * m.m13 + m.m30 * m.m03 * m.m12,
            m13 = m.m00 * m.m12 * m.m23 - m.m00 * m.m13 * m.m22 - m.m10 * m.m02 * m.m23 + m.m10 * m.m03 * m.m22 +
                m.m20 * m.m02 * m.m13 - m.m20 * m.m03 * m.m12,
            
                m20 = m.m10 * m.m21 * m.m33 - m.m10 * m.m23 * m.m31 - m.m20 * m.m11 * m.m33 + m.m20 * m.m13 * m.m31 +
                m.m30 * m.m11 * m.m23 - m.m30 * m.m13 * m.m21,
            m21 = -m.m00 * m.m21 * m.m33 + m.m00 * m.m23 * m.m31 + m.m20 * m.m01 * m.m33 - m.m20 * m.m03 * m.m31 -
                m.m30 * m.m01 * m.m23 + m.m30 * m.m03 * m.m21,
            m22 = m.m00 * m.m11 * m.m33 - m.m00 * m.m13 * m.m31 - m.m10 * m.m01 * m.m33 + m.m10 * m.m03 * m.m31 +
                m.m30 * m.m01 * m.m13 - m.m30 * m.m03 * m.m11,
            m23 = -m.m00 * m.m11 * m.m23 + m.m00 * m.m13 * m.m21 + m.m10 * m.m01 * m.m23 - m.m10 * m.m03 * m.m21 -
                m.m20 * m.m01 * m.m13 + m.m20 * m.m03 * m.m11,
            
                m30 = -m.m10 * m.m21 * m.m32 + m.m10 * m.m22 * m.m31 + m.m20 * m.m11 * m.m32 - m.m20 * m.m12 * m.m31 -
                m.m30 * m.m11 * m.m22 + m.m30 * m.m12 * m.m21,
            m31 = m.m00 * m.m21 * m.m32 - m.m00 * m.m22 * m.m31 - m.m20 * m.m01 * m.m32 + m.m20 * m.m02 * m.m31 +
                m.m30 * m.m01 * m.m22 - m.m30 * m.m02 * m.m21,
            m32 = -m.m00 * m.m11 * m.m32 + m.m00 * m.m12 * m.m31 + m.m10 * m.m01 * m.m32 - m.m10 * m.m02 * m.m31 -
                m.m30 * m.m01 * m.m12 + m.m30 * m.m02 * m.m11,
            m33 = m.m00 * m.m11 * m.m22 - m.m00 * m.m12 * m.m21 - m.m10 * m.m01 * m.m22 + m.m10 * m.m02 * m.m21 +
                m.m20 * m.m01 * m.m12 - m.m20 * m.m02 * m.m11
        };

        return inv;
    }
    
    public static Mat4x4 Rotate(Quat q)
    {
        float x = q.EulerAngles.x;
        float y = q.EulerAngles.y;
        float z = q.EulerAngles.z;

        Mat4x4 mX = identity;
        Mat4x4 mY = identity;
        Mat4x4 mZ = identity;

        mX.m11 = Mathf.Cos(x);
        mX.m12 = -Mathf.Sin(x);
        mX.m21 = Mathf.Sin(x);
        mX.m22 = Mathf.Cos(x);

        mY.m00 = Mathf.Cos(y);
        mY.m02 = Mathf.Sin(y);
        mY.m20 = -Mathf.Sin(y);
        mY.m22 = Mathf.Cos(y);

        mZ.m00 = Mathf.Cos(z);
        mZ.m01 = -Mathf.Sin(z);
        mZ.m10 = Mathf.Sin(z);
        mZ.m11 = Mathf.Cos(z);

        return mZ * mX * mY;
    }

    public static Mat4x4 Scale(Vec3 vector)
    {
        Mat4x4 m = identity;

        m.m00 = vector.x;
        m.m11 = vector.y;
        m.m22 = vector.z;

        return m;
    }

    public static Mat4x4 Translate(Vec3 vector)
    {
        Mat4x4 m = identity;

        m.m03 = vector.x;
        m.m13 = vector.y;
        m.m23 = vector.y;

        return m;
    }

    public static Mat4x4 GetTranspose(Mat4x4 m)
    {
        Vector4 line1 = new Vector4(m.m00, m.m01, m.m02, m.m03);
        Vector4 line2 = new Vector4(m.m10, m.m11, m.m12, m.m13);
        Vector4 line3 = new Vector4(m.m20, m.m21, m.m22, m.m23);
        Vector4 line4 = new Vector4(m.m30, m.m31, m.m32, m.m33);

        return new Mat4x4(line1, line2, line3, line4);
    }

    public static Mat4x4 TRS(Vector3 pos, Quat q, Vector3 s)
    {
        Mat4x4 translation = Translate(pos);
        Mat4x4 rotation = Rotate(q);
        Mat4x4 scale = Scale(s);

        Mat4x4 result = translation * rotation * scale;

        return result;
    }

    public Vector4 GetColumn(int index)
    {
        return new Vector4(this[0, index], this[1, index], this[2, index], this[3, index]);
    }

    public Vec3 GetPosition()
    {
        if (ValidTRS())
            return new Vector3(this.m03, this.m13, this.m23);
        else
            return Vec3.Zero;
    }

    public Vector4 GetRow(int index)
    {
        return new Vector4(this[index, 0], this[index, 1], this[index, 2], this[index, 3]);
    }

    public Vec3 MultiplyPoint(Vec3 point)
    {
        Vec3 res;
        res.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
        res.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
        res.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;
        float num = m30 * point.x + m31 * point.y + m32 * point.z + m33;

        res.x /= num;
        res.y /= num;
        res.z /= num;

        return res;
    }

    public Vec3 MultiplyPoint3x4(Vec3 point)
    {
        if (!ValidTRS())
            return MultiplyPoint(point);

        Vec3 res;

        res.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
        res.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
        res.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;

        return res;
    }

    public Vec3 MultiplyVector(Vec3 vector)
    {
        Vector3 res;
        res.x = m00 * vector.x + m01 * vector.y + m02 * vector.z;
        res.y = m10 * vector.x + m11 * vector.y + m12 * vector.z;
        res.z = m20 * vector.x + m21 * vector.y + m22 * vector.z;
        return res;
    }

    public void SetColumn(int index, Vector4 column)
    {
        this[0, index] = column[0];
        this[1, index] = column[1];
        this[2, index] = column[2];
        this[3, index] = column[3];

        if (index > 3)
            throw new IndexOutOfRangeException("Invalid row index!");
    }

    public void SetRow(int index, Vector4 row)
    {
        this[index, 0] = row[0];
        this[index, 1] = row[1];
        this[index, 2] = row[2];
        this[index, 3] = row[3];

        if (index > 3)
            throw new IndexOutOfRangeException("Invalid row index!");
    }

    public void SetTRS(Vector3 pos, Quaternion q, Vector3 s) => this = TRS(pos, q, s);

    public bool ValidTRS()
    {
        if (GetRow(3) != new Vector4(0, 0, 0, 1))
            return false;

        if (m00 < 0 || m11 < 0 || m22 < 0)
            return false;

        Vec3 column0 = new Vec3(m00, m10, m20);
        Vec3 column1 = new Vec3(m01, m11, m21);
        Vec3 column2 = new Vec3(m02, m12, m22);

        return Mathf.Approximately(Vec3.Dot(column0, column1), 0) &&
               Mathf.Approximately(Vec3.Dot(column0, column2), 0) &&
               Mathf.Approximately(Vec3.Dot(column1, column2), 0);
    }

    #endregion

    #region Operators

    public static Mat4x4 operator *(Mat4x4 lhs, Mat4x4 rhs)
    {
        Mat4x4 res = new Mat4x4();
        res.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
        res.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
        res.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
        res.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;

        res.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
        res.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
        res.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
        res.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;

        res.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
        res.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
        res.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
        res.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;

        res.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
        res.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
        res.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
        res.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;

        return res;
    }

    public static Vector4 operator *(Mat4x4 lhs, Vector4 vector)
    {
        Vector4 res = Vector4.zero;

        res.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w;
        res.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w;
        res.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w;
        res.w = lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w;

        return res;
    }

    public static bool operator ==(Mat4x4 lhs, Mat4x4 rhs)
    {
        return lhs.m00 == rhs.m00 && lhs.m01 == rhs.m01 && lhs.m02 == rhs.m02 && lhs.m03 == rhs.m03 &&
               lhs.m10 == rhs.m10 && lhs.m11 == rhs.m11 && lhs.m12 == rhs.m12 && lhs.m13 == rhs.m13 &&
               lhs.m20 == rhs.m20 && lhs.m21 == rhs.m21 && lhs.m22 == rhs.m22 && lhs.m23 == rhs.m23 &&
               lhs.m30 == rhs.m30 && lhs.m31 == rhs.m31 && lhs.m32 == rhs.m32 && lhs.m33 == rhs.m33;
    }

    public static bool operator !=(Mat4x4 lhs, Mat4x4 rhs)
    {
        return !(lhs == rhs);
    }

    public static implicit operator Mat4x4(Matrix4x4 mat)
    {
        return new Mat4x4
        {
            m00 = mat.m00,
            m01 = mat.m01,
            m02 = mat.m02,
            m03 = mat.m03,
            m10 = mat.m10,
            m11 = mat.m11,
            m12 = mat.m12,
            m13 = mat.m13,
            m20 = mat.m20,
            m21 = mat.m21,
            m22 = mat.m22,
            m23 = mat.m23,
            m30 = mat.m30,
            m31 = mat.m31,
            m32 = mat.m32,
            m33 = mat.m33
        };
    }

    public static implicit operator Matrix4x4(Mat4x4 mat)
    {
        return new Matrix4x4
        {
            m00 = mat.m00,
            m01 = mat.m01,
            m02 = mat.m02,
            m03 = mat.m03,
            m10 = mat.m10,
            m11 = mat.m11,
            m12 = mat.m12,
            m13 = mat.m13,
            m20 = mat.m20,
            m21 = mat.m21,
            m22 = mat.m22,
            m23 = mat.m23,
            m30 = mat.m30,
            m31 = mat.m31,
            m32 = mat.m32,
            m33 = mat.m33
        };
    }

    #endregion

    #region Other methods

    public bool Equals(Mat4x4 other)
    {
        return this == other;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(
            $"{m00.ToString(format, formatProvider)} {m01.ToString(format, formatProvider)} {m02.ToString(format, formatProvider)} {m03.ToString(format, formatProvider)}");
        sb.AppendLine(
            $"{m10.ToString(format, formatProvider)} {m11.ToString(format, formatProvider)} {m12.ToString(format, formatProvider)} {m13.ToString(format, formatProvider)}");
        sb.AppendLine(
            $"{m20.ToString(format, formatProvider)} {m21.ToString(format, formatProvider)} {m22.ToString(format, formatProvider)} {m23.ToString(format, formatProvider)}");
        sb.AppendLine(
            $"{m30.ToString(format, formatProvider)} {m31.ToString(format, formatProvider)} {m32.ToString(format, formatProvider)} {m33.ToString(format, formatProvider)}");

        return sb.ToString();
    }

    public override bool Equals(object obj)
    {
        return obj is Mat4x4 other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();
        hashCode.Add(m00);
        hashCode.Add(m10);
        hashCode.Add(m20);
        hashCode.Add(m30);
        hashCode.Add(m01);
        hashCode.Add(m11);
        hashCode.Add(m21);
        hashCode.Add(m31);
        hashCode.Add(m02);
        hashCode.Add(m12);
        hashCode.Add(m22);
        hashCode.Add(m32);
        hashCode.Add(m03);
        hashCode.Add(m13);
        hashCode.Add(m23);
        hashCode.Add(m33);
        return hashCode.ToHashCode();
    }

    #endregion
}