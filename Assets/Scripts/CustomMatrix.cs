using System;
using System.Text;
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

    public float determinant => Determinant(this);

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

    public static CustomMatrix Inverse(CustomMatrix m)
    {
        
    }

    public void SetTRS(Vector3 pos, Quaternion q, Vector3 s)
    {
        // var rotation = Rotate(q);
        // var scale = Scale(s);
        // var translation = Translate(pos);
        //
        // var result = translation * rotation * scale;
        //
        // for (int i = 0; i < 16; i++)
        //     this[i] = result[i];
    }

    public Vector4 GetColumn(int index)
    {
        switch (index)
        {
            case 0: return new Vector4(m00, m10, m20, m30);
            case 1: return new Vector4(m01, m11, m21, m31);
            case 2: return new Vector4(m02, m12, m22, m32);
            case 3: return new Vector4(m03, m13, m23, m33);
            default: throw new IndexOutOfRangeException("Invalid column index!");
        }
    }

    public void SetColumn(int index, Vector4 column)
    {
        switch (index)
        {
            case 0:
                m00 = column.x;
                m10 = column.y;
                m20 = column.z;
                m30 = column.w;
                break;
            case 1:
                m01 = column.x;
                m11 = column.y;
                m21 = column.z;
                m31 = column.w;
                break;
            case 2:
                m02 = column.x;
                m12 = column.y;
                m22 = column.z;
                m32 = column.w;
                break;
            case 3:
                m03 = column.x;
                m13 = column.y;
                m23 = column.z;
                m33 = column.w;
                break;
            default: throw new IndexOutOfRangeException("Invalid column index!");
        }
    }

    public Vector4 GetRow(int index)
    {
        switch (index)
        {
            case 0: return new Vector4(m00, m01, m02, m03);
            case 1: return new Vector4(m10, m11, m12, m13);
            case 2: return new Vector4(m20, m21, m22, m23);
            case 3: return new Vector4(m30, m31, m32, m33);
            default: throw new IndexOutOfRangeException("Invalid row index!");
        }
    }

    public void SetRow(int index, Vector4 row)
    {
        switch (index)
        {
            case 0:
                m00 = row.x;
                m01 = row.y;
                m02 = row.z;
                m03 = row.w;
                break;
            case 1:
                m10 = row.x;
                m11 = row.y;
                m12 = row.z;
                m13 = row.w;
                break;
            case 2:
                m20 = row.x;
                m21 = row.y;
                m22 = row.z;
                m23 = row.w;
                break;
            case 3:
                m30 = row.x;
                m31 = row.y;
                m32 = row.z;
                m33 = row.w;
                break;
            default: throw new IndexOutOfRangeException("Invalid row index!");
        }
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