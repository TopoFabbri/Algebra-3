using System;
using System.Text;
using CustomMath;
using Unity.VisualScripting;
using UnityEngine;

public struct CustomMatrix : IEquatable<CustomMatrix>, IFormattable
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

    public CustomMatrix(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
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

    public float this[int row, int column]
    {
        get => this[row + column * 4];

        set => this[row + column * 4] = value;
    }

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

    public static CustomMatrix zero => new CustomMatrix(
        new Vector4(0, 0, 0, 0),
        new Vector4(0, 0, 0, 0),
        new Vector4(0, 0, 0, 0),
        new Vector4(0, 0, 0, 0)
    );

    public static CustomMatrix identity => new CustomMatrix(
        new Vector4(1f, 0f, 0f, 0f),
        new Vector4(0f, 1f, 0f, 0f),
        new Vector4(0f, 0f, 1f, 0f),
        new Vector4(0f, 0f, 0f, 1f)
    );

    public float determinant => Determinant(this);

    public CustomMatrix transpose => Transpose(this);

    #endregion 

    #region Functions

    public static float Determinant(CustomMatrix m)
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

    public static CustomMatrix Rotate(CustomQuat q)
    {
        float x = q.EulerAngles.x;
        float y = q.EulerAngles.y;
        float z = q.EulerAngles.z;

        CustomMatrix mX = identity;
        CustomMatrix mY = identity;
        CustomMatrix mZ = identity;

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

    public static CustomMatrix Scale(Vec3 vector)
    {
        CustomMatrix m = identity;

        m.m00 = vector.x;
        m.m11 = vector.y;
        m.m22 = vector.z;

        return m;
    }

    public static CustomMatrix Translate(Vec3 vector)
    {
        CustomMatrix m = identity;

        m.m03 = vector.x;
        m.m13 = vector.y;
        m.m23 = vector.y;

        return m;
    }

    public static CustomMatrix Transpose(CustomMatrix m)
    {
        Vector4 line1 = new Vector4(m.m00, m.m01, m.m02, m.m03);
        Vector4 line2 = new Vector4(m.m10, m.m11, m.m12, m.m13);
        Vector4 line3 = new Vector4(m.m20, m.m21, m.m22, m.m23);
        Vector4 line4 = new Vector4(m.m30, m.m31, m.m32, m.m33);

        return new CustomMatrix(line1, line2, line3, line4);
    }

    public static CustomMatrix TRS(Vector3 pos, CustomQuat q, Vector3 s)
    {
        CustomMatrix translation = Translate(pos);
        CustomMatrix rotation = Rotate(q);
        CustomMatrix scale = Scale(s);

        CustomMatrix result = translation * rotation * scale;

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

    public static CustomMatrix operator *(CustomMatrix lhs, CustomMatrix rhs)
    {
        CustomMatrix res = new CustomMatrix();
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

    public static Vector4 operator *(CustomMatrix lhs, Vector4 vector)
    {
        Vector4 res = Vector4.zero;

        res.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w;
        res.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w;
        res.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w;
        res.w = lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w;

        return res;
    }

    public static bool operator ==(CustomMatrix lhs, CustomMatrix rhs)
    {
        return lhs.m00 == rhs.m00 && lhs.m01 == rhs.m01 && lhs.m02 == rhs.m02 && lhs.m03 == rhs.m03 &&
               lhs.m10 == rhs.m10 && lhs.m11 == rhs.m11 && lhs.m12 == rhs.m12 && lhs.m13 == rhs.m13 &&
               lhs.m20 == rhs.m20 && lhs.m21 == rhs.m21 && lhs.m22 == rhs.m22 && lhs.m23 == rhs.m23 &&
               lhs.m30 == rhs.m30 && lhs.m31 == rhs.m31 && lhs.m32 == rhs.m32 && lhs.m33 == rhs.m33;
    }

    public static bool operator !=(CustomMatrix lhs, CustomMatrix rhs)
    {
        return !(lhs == rhs);
    }

    #endregion

    #region Other methods

    public bool Equals(CustomMatrix other)
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

    #endregion
}