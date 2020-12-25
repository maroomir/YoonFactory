using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YoonFactory
{
    public class YoonMatrix2X2Int : IYoonMatrix
    {
        public int Length { get; } = 2;

        public int matrix_11
        {
            get => Array[0, 0];

            set
            {
                Array[0, 0] = value;
            }
        }

        public int matrix_12
        {
            get => Array[0, 1];

            set
            {
                Array[0, 1] = value;
            }
        }

        public int matrix_21
        {
            get => Array[1, 0];

            set
            {
                Array[1, 0] = value;
            }
        }

        public int matrix_22
        {
            get => Array[1, 1];

            set
            {
                Array[1, 1] = value;
            }
        }

        public YoonMatrix2X2Int()
        {
            matrix_11 = matrix_22 = 1;
            matrix_12 = matrix_21 = 0;
        }

        public YoonMatrix2X2Int(IYoonMatrix m)
        {
            this.CopyFrom(m);
        }

        public int[,] Array { get; set; } = new int[2, 2];

        public int Determinant
        {
            get => matrix_11 * matrix_22 - matrix_12 * matrix_21;
        }

        public IYoonMatrix Clone()
        {
            YoonMatrix2X2Int pMatrix = new YoonMatrix2X2Int();
            pMatrix.matrix_11 = matrix_11;
            pMatrix.matrix_12 = matrix_12;
            pMatrix.matrix_21 = matrix_21;
            pMatrix.matrix_22 = matrix_22;
            return pMatrix;
        }

        public void CopyFrom(IYoonMatrix m)
        {
            if (m is YoonMatrix2X2Int pMatrix)
            {
                matrix_11 = pMatrix.matrix_11;
                matrix_12 = pMatrix.matrix_12;
                matrix_21 = pMatrix.matrix_21;
                matrix_22 = pMatrix.matrix_22;
            }
        }

        public IYoonMatrix Inverse()
        {
            if (Determinant == 0) return Unit();

            YoonMatrix2X2Int m = new YoonMatrix2X2Int(this);
            matrix_11 = m.matrix_22 / m.Determinant;
            matrix_12 = -m.matrix_12 / m.Determinant;
            matrix_21 = -m.matrix_21 / m.Determinant;
            matrix_22 = m.matrix_11 / m.Determinant;
            return this;
        }

        public IYoonMatrix Transpose()
        {
            YoonMatrix2X2Int m = new YoonMatrix2X2Int(this);
            matrix_12 = m.matrix_21;
            matrix_21 = m.matrix_12;
            return this;
        }

        public IYoonMatrix Unit()
        {
            matrix_11 = matrix_22 = 1;
            matrix_12 = matrix_21 = 0;
            return this;
        }
        
        public static YoonMatrix2X2Int operator *(int a, YoonMatrix2X2Int b)
        {
            YoonMatrix2X2Int m = new YoonMatrix2X2Int();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a * b.Array[i, j];
                }
            }
            return m;
        }

        public static YoonMatrix2X2Int operator /(YoonMatrix2X2Int a, int b)
        {
            YoonMatrix2X2Int m = new YoonMatrix2X2Int();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a.Array[i, j] / b;
                }
            }
            return m;
        }

        public static YoonMatrix2X2Int operator *(YoonMatrix2X2Int a, YoonMatrix2X2Int b)
        {
            YoonMatrix2X2Int m = new YoonMatrix2X2Int();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = 0;
                    for (int kValue = 0; kValue < m.Length; kValue++)
                        m.Array[i, j] += (a.Array[i, kValue] * b.Array[kValue, j]);
                }
            }
            return m;
        }
    }
    
    public class YoonMatrix2X2Double : IYoonMatrix
    {
        public int Length { get; } = 2;

        public double matrix_11
        {
            get => Array[0, 0];

            set
            {
                Array[0, 0] = value;
            }
        }

        public double matrix_12
        {
            get => Array[0, 1];

            set
            {
                Array[0, 1] = value;
            }
        }

        public double matrix_21
        {
            get => Array[1, 0];

            set
            {
                Array[1, 0] = value;
            }
        }

        public double matrix_22
        {
            get => Array[1, 1];

            set
            {
                Array[1, 1] = value;
            }
        }

        public YoonMatrix2X2Double()
        {
            matrix_11 = matrix_22 = 1;
            matrix_12 = matrix_21 = 0;
        }

        public YoonMatrix2X2Double(IYoonMatrix m)
        {
            this.CopyFrom(m);
        }

        public double[,] Array { get; set; } = new double[2, 2];

        public double Determinant
        {
            get => matrix_11 * matrix_22 - matrix_12 * matrix_21;
        }

        public IYoonMatrix Clone()
        {
            YoonMatrix2X2Double pMatrix = new YoonMatrix2X2Double();
            pMatrix.matrix_11 = matrix_11;
            pMatrix.matrix_12 = matrix_12;
            pMatrix.matrix_21 = matrix_21;
            pMatrix.matrix_22 = matrix_22;
            return pMatrix;
        }

        public void CopyFrom(IYoonMatrix m)
        {
            if (m is YoonMatrix2X2Double pMatrix)
            {
                matrix_11 = pMatrix.matrix_11;
                matrix_12 = pMatrix.matrix_12;
                matrix_21 = pMatrix.matrix_21;
                matrix_22 = pMatrix.matrix_22;
            }
        }

        public IYoonMatrix Inverse()
        {
            if (Determinant == 0) return Unit();

            YoonMatrix2X2Double m = new YoonMatrix2X2Double(this);
            matrix_11 = m.matrix_22 / m.Determinant;
            matrix_12 = - m.matrix_12 / m.Determinant;
            matrix_21 = - m.matrix_21 / m.Determinant;
            matrix_22 = m.matrix_11 / m.Determinant;
            return this;
        }

        public IYoonMatrix Transpose()
        {
            YoonMatrix2X2Double m = new YoonMatrix2X2Double(this);
            matrix_12 = m.matrix_21;
            matrix_21 = m.matrix_12;
            return this;
        }

        public IYoonMatrix Unit()
        {
            matrix_11 = matrix_22 = 1;
            matrix_12 = matrix_21 = 0;
            return this;
        }

        public static YoonMatrix2X2Double operator *(double a, YoonMatrix2X2Double b)
        {
            YoonMatrix2X2Double m = new YoonMatrix2X2Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a * b.Array[i, j];
                }
            }
            return m;
        }

        public static YoonMatrix2X2Double operator /(YoonMatrix2X2Double a, double b)
        {
            YoonMatrix2X2Double m = new YoonMatrix2X2Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a.Array[i, j] / b;
                }
            }
            return m;
        }

        public static YoonMatrix2X2Double operator *(YoonMatrix2X2Double a, YoonMatrix2X2Double b)
        {
            YoonMatrix2X2Double m = new YoonMatrix2X2Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = 0;
                    for (int kValue = 0; kValue < m.Length; kValue++)
                        m.Array[i, j] += (a.Array[i, kValue] * b.Array[kValue, j]);
                }
            }
            return m;
        }
    }

    public class YoonMatrix3X3Int : IYoonMatrix, IYoonMatrix<int>
    {
        public int Length { get; } = 3;

        public int matrix_11
        {
            get => Array[0, 0];

            set
            {
                Array[0, 0] = value;
            }
        }

        public int matrix_12
        {
            get => Array[0, 1];

            set
            {
                Array[0, 1] = value;
            }
        }

        public int matrix_13
        {
            get => Array[0, 2];

            set
            {
                Array[0, 2] = value;
            }
        }

        public int matrix_21
        {
            get => Array[1, 0];

            set
            {
                Array[1, 0] = value;
            }
        }

        public int matrix_22
        {
            get => Array[1, 1];

            set
            {
                Array[1, 1] = value;
            }
        }

        public int matrix_23
        {
            get => Array[1, 2];

            set
            {
                Array[1, 2] = value;
            }
        }

        public int matrix_31
        {
            get => Array[2, 0];

            set
            {
                Array[2, 0] = value;
            }
        }

        public int matrix_32
        {
            get => Array[2, 1];

            set
            {
                Array[2, 1] = value;
            }
        }

        public int matrix_33
        {
            get => Array[2, 2];

            set
            {
                Array[2, 2] = value;
            }
        }

        public YoonMatrix3X3Int()
        {
            matrix_11 = matrix_22 = matrix_33 = 1;
            matrix_12 = matrix_13 = matrix_21 = matrix_23 = matrix_31 = matrix_32 = 0;
        }

        public YoonMatrix3X3Int(IYoonMatrix m)
        {
            this.CopyFrom(m);
        }

        public int[,] Array { get; set; } = new int[3, 3];

        public int Determinant
        {
            /*
                   matrix_11 * matrix_22 * matrix_33 + matrix_21 * matrix_32 * matrix_13 + matrix_31 * matrix_12 * matrix_23
                 - matrix_11 * matrix_32 * matrix_23 - matrix_31 * matrix_22 * matrix_13 - matrix_21 * matrix_12 * matrix_33;
            */
            get
            {
                int nSum = 0;
                for (int i = 0; i < Length; i++)
                    nSum += Array[0, i] * Cofactor(0, i);
                return nSum;
            }
        }

        public IYoonMatrix Clone()
        {
            YoonMatrix3X3Int pMatrix = new YoonMatrix3X3Int();
            pMatrix.matrix_11 = matrix_11;
            pMatrix.matrix_12 = matrix_12;
            pMatrix.matrix_13 = matrix_13;
            pMatrix.matrix_21 = matrix_21;
            pMatrix.matrix_22 = matrix_22;
            pMatrix.matrix_23 = matrix_23;
            pMatrix.matrix_31 = matrix_31;
            pMatrix.matrix_32 = matrix_32;
            pMatrix.matrix_33 = matrix_33;
            return pMatrix;
        }

        public void CopyFrom(IYoonMatrix m)
        {
            if (m is YoonMatrix3X3Int pMatrix)
            {
                matrix_11 = pMatrix.matrix_11;
                matrix_12 = pMatrix.matrix_12;
                matrix_13 = pMatrix.matrix_13;
                matrix_21 = pMatrix.matrix_21;
                matrix_22 = pMatrix.matrix_22;
                matrix_23 = pMatrix.matrix_23;
                matrix_31 = pMatrix.matrix_31;
                matrix_32 = pMatrix.matrix_32;
                matrix_33 = pMatrix.matrix_33;
            }
        }

        public int Cofactor(int nRow, int nCol)
        {
            return (int)(Math.Pow(-1, nRow + nCol) * (GetMinorMatrix(nRow, nCol) as YoonMatrix2X2Int).Determinant);
        }

        public IYoonMatrix GetMinorMatrix(int nRow, int nCol)
        {
            if (nRow < 0 || nRow >= Length || nCol < 0 || nCol >= Length)
                return new YoonMatrix2X2Int();

            int[,] pArray = new int[2, 2];
            int iCount = 0;
            int jCount = 0;
            for (int i = 0; i < Length; i++)
            {
                if (i == nRow) continue;
                for (int j = 0; j < Length; j++)
                {
                    if (j == nCol) continue;
                    pArray[iCount, jCount] = Array[i, j];
                    jCount++;
                }
                iCount++;
            }
            YoonMatrix2X2Int pMatrix = new YoonMatrix2X2Int();
            pMatrix.Array = pArray;
            return pMatrix;
        }

        public IYoonMatrix GetAdjointMatrix()
        {
            int[,] pArray = new int[3, 3];
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    pArray[i, j] = Cofactor(i, j);
                }
            }
            YoonMatrix3X3Int pMatrix = new YoonMatrix3X3Int();
            pMatrix.Array = pArray;
            return pMatrix.Transpose();
        }

        public IYoonMatrix Inverse()
        {
            if (Determinant == 0) return Unit();

            YoonMatrix3X3Int m = new YoonMatrix3X3Int(this);
            CopyFrom((m.GetAdjointMatrix() as YoonMatrix3X3Int) / m.Determinant);
            return this;
        }

        public IYoonMatrix Transpose()
        {
            YoonMatrix3X3Int m = new YoonMatrix3X3Int(this);
            matrix_12 = m.matrix_21;
            matrix_13 = m.matrix_31;
            matrix_21 = m.matrix_12;
            matrix_23 = m.matrix_32;
            matrix_31 = m.matrix_13;
            matrix_32 = m.matrix_23;
            return this;
        }

        public IYoonMatrix Unit()
        {
            matrix_11 = matrix_22 = matrix_33 = 1;
            matrix_12 = matrix_13 = matrix_21 = matrix_23 = matrix_31 = matrix_32 = 0;
            return this;
        }

        public static YoonMatrix3X3Int operator *(int a, YoonMatrix3X3Int b)
        {
            YoonMatrix3X3Int m = new YoonMatrix3X3Int();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a * b.Array[i, j];
                }
            }
            return m;
        }

        public static YoonMatrix3X3Int operator /(YoonMatrix3X3Int a, int b)
        {
            YoonMatrix3X3Int m = new YoonMatrix3X3Int();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a.Array[i, j] / b;
                }
            }
            return m;
        }

        public static YoonMatrix3X3Int operator *(YoonMatrix3X3Int a, YoonMatrix3X3Int b)
        {
            YoonMatrix3X3Int m = new YoonMatrix3X3Int();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = 0;
                    for (int kValue = 0; kValue < m.Length; kValue++)
                        m.Array[i, j] += (a.Array[i, kValue] * b.Array[kValue, j]);
                }
            }
            return m;
        }
    }
    
    public class YoonMatrix3X3Double : IYoonMatrix, IYoonMatrix<double>
    {
        public int Length { get; } = 3;

        public double matrix_11
        {
            get => Array[0, 0];

            set
            {
                Array[0, 0] = value;
            }
        }

        public double matrix_12
        {
            get => Array[0, 1];

            set
            {
                Array[0, 1] = value;
            }
        }

        public double matrix_13
        {
            get => Array[0, 2];

            set
            {
                Array[0, 2] = value;
            }
        }

        public double matrix_21
        {
            get => Array[1, 0];

            set
            {
                Array[1, 0] = value;
            }
        }

        public double matrix_22
        {
            get => Array[1, 1];

            set
            {
                Array[1, 1] = value;
            }
        }

        public double matrix_23
        {
            get => Array[1, 2];

            set
            {
                Array[1, 2] = value;
            }
        }

        public double matrix_31
        {
            get => Array[2, 0];

            set
            {
                Array[2, 0] = value;
            }
        }

        public double matrix_32
        {
            get => Array[2, 1];

            set
            {
                Array[2, 1] = value;
            }
        }

        public double matrix_33
        {
            get => Array[2, 2];

            set
            {
                Array[2, 2] = value;
            }
        }

        public YoonMatrix3X3Double()
        {
            matrix_11 = matrix_22 = matrix_33 = 1;
            matrix_12 = matrix_13 = matrix_21 = matrix_23 = matrix_31 = matrix_32 = 0;
        }

        public YoonMatrix3X3Double(IYoonMatrix m)
        {
            this.CopyFrom(m);
        }

        public double[,] Array { get; set; } = new double[3, 3];

        public double Determinant
        {
            /*
                   matrix_11 * matrix_22 * matrix_33 + matrix_21 * matrix_32 * matrix_13 + matrix_31 * matrix_12 * matrix_23
                 - matrix_11 * matrix_32 * matrix_23 - matrix_31 * matrix_22 * matrix_13 - matrix_21 * matrix_12 * matrix_33;
            */
            get
            {
                double dSum = 0;
                for (int i = 0; i < Length; i++)
                    dSum += Array[0, i] * Cofactor(0, i);
                return dSum;
            }
        }

        public IYoonMatrix Clone()
        {
            YoonMatrix3X3Double pMatrix = new YoonMatrix3X3Double();
            pMatrix.matrix_11 = matrix_11;
            pMatrix.matrix_12 = matrix_12;
            pMatrix.matrix_13 = matrix_13;
            pMatrix.matrix_21 = matrix_21;
            pMatrix.matrix_22 = matrix_22;
            pMatrix.matrix_23 = matrix_23;
            pMatrix.matrix_31 = matrix_31;
            pMatrix.matrix_32 = matrix_32;
            pMatrix.matrix_33 = matrix_33;
            return pMatrix;
        }

        public void CopyFrom(IYoonMatrix m)
        {
            if (m is YoonMatrix3X3Double pMatrix)
            {
                matrix_11 = pMatrix.matrix_11;
                matrix_12 = pMatrix.matrix_12;
                matrix_13 = pMatrix.matrix_13;
                matrix_21 = pMatrix.matrix_21;
                matrix_22 = pMatrix.matrix_22;
                matrix_23 = pMatrix.matrix_23;
                matrix_31 = pMatrix.matrix_31;
                matrix_32 = pMatrix.matrix_32;
                matrix_33 = pMatrix.matrix_33;
            }
        }

        public double Cofactor(int nRow, int nCol)
        {
            return Math.Pow(-1, nRow + nCol) * (GetMinorMatrix(nRow, nCol) as YoonMatrix2X2Double).Determinant;
        }

        public IYoonMatrix GetMinorMatrix(int nRow, int nCol)
        {
            if (nRow < 0 || nRow >= Length || nCol < 0 || nCol >= Length)
                return new YoonMatrix2X2Double();

            double[,] pArray = new double[2, 2];
            int iCount = 0;
            int jCount = 0;
            for (int i = 0; i < Length; i++)
            {
                if (i == nRow) continue;
                for (int j = 0; j < Length; j++)
                {
                    if (j == nCol) continue;
                    pArray[iCount, jCount] = Array[i, j];
                    jCount++;
                }
                iCount++;
            }
            YoonMatrix2X2Double pMatrix = new YoonMatrix2X2Double();
            pMatrix.Array = pArray;
            return pMatrix;
        }

        public IYoonMatrix GetAdjointMatrix()
        {
            double[,] pArray = new double[3, 3];
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    pArray[i, j] = Cofactor(i, j);
                }
            }
            YoonMatrix3X3Double pMatrix = new YoonMatrix3X3Double();
            pMatrix.Array = pArray;
            return pMatrix.Transpose();
        }

        public IYoonMatrix Inverse()
        {
            if (Determinant == 0) return Unit();

            YoonMatrix3X3Double m = new YoonMatrix3X3Double(this);
            CopyFrom((m.GetAdjointMatrix() as YoonMatrix3X3Double) / m.Determinant);
            return this;
        }

        public IYoonMatrix Transpose()
        {
            YoonMatrix3X3Double m = new YoonMatrix3X3Double(this);
            matrix_12 = m.matrix_21;
            matrix_13 = m.matrix_31;
            matrix_21 = m.matrix_12;
            matrix_23 = m.matrix_32;
            matrix_31 = m.matrix_13;
            matrix_32 = m.matrix_23;
            return this;
        }

        public IYoonMatrix Unit()
        {
            matrix_11 = matrix_22 = matrix_33 = 1;
            matrix_12 = matrix_13 = matrix_21 = matrix_23 = matrix_31 = matrix_32 = 0;
            return this;
        }

        public static YoonMatrix3X3Double operator *(double a, YoonMatrix3X3Double b)
        {
            YoonMatrix3X3Double m = new YoonMatrix3X3Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a * b.Array[i, j];
                }
            }
            return m;
        }

        public static YoonMatrix3X3Double operator /(YoonMatrix3X3Double a, double b)
        {
            YoonMatrix3X3Double m = new YoonMatrix3X3Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a.Array[i, j] / b;
                }
            }
            return m;
        }

        public static YoonMatrix3X3Double operator *(YoonMatrix3X3Double a, YoonMatrix3X3Double b)
        {
            YoonMatrix3X3Double m = new YoonMatrix3X3Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = 0;
                    for (int kValue = 0; kValue < m.Length; kValue++)
                        m.Array[i, j] += (a.Array[i, kValue] * b.Array[kValue, j]);
                }
            }
            return m;
        }
    }

    /// <summary>
    /// 2차원 동차변환 행렬 (int, 연산이 있으므로 제너릭 사용 불가)
    /// </summary>
    public class YoonMatrix2N : YoonMatrix3X3Int, IYoonMatrix2D<int>
    {
        public IYoonMatrix SetScaleUnit(int sx, int sy)
        {
            Unit();
            matrix_11 *= sx;
            matrix_22 *= sy;
            return this;
        }

        public IYoonMatrix SetMovementUnit(int dx, int dy)
        {
            Unit();
            matrix_13 += dx;
            matrix_23 += dy;
            return this;
        }

        public IYoonMatrix SetMovementUnit(IYoonVector2D<int> v)
        {
            Unit();
            matrix_13 += v.X;
            matrix_23 += v.Y;
            return this;
        }

        public IYoonMatrix SetRotateUnit(double angle)
        {
            Unit();
            int cosT = (int)Math.Cos(angle);
            int sinT = (int)Math.Sin(angle);
            matrix_11 = cosT;
            matrix_12 = -sinT;
            matrix_21 = sinT;
            matrix_22 = cosT;
            return this;
        }
    }
    
    /// <summary>
    /// 2차원 동차변환 행렬 (double, 연산이 있으므로 제너릭 사용 불가)
    /// </summary>
    public class YoonMatrix2D : YoonMatrix3X3Double, IYoonMatrix2D<double>
    {

        public IYoonMatrix SetScaleUnit(double sx, double sy)
        {
            Unit();
            matrix_11 *= sx;
            matrix_22 *= sy;
            return this;
        }

        public IYoonMatrix SetMovementUnit(double dx, double dy)
        {
            Unit();
            matrix_13 += dx;
            matrix_23 += dy;
            return this;
        }

        public IYoonMatrix SetMovementUnit(IYoonVector2D<double> v)
        {
            Unit();
            matrix_13 += v.X;
            matrix_23 += v.Y;
            return this;
        }

        public IYoonMatrix SetRotateUnit(double angle)
        {
            Unit();
            double cosT = Math.Cos(angle);
            double sinT = Math.Sin(angle);
            matrix_11 = cosT;
            matrix_12 = -sinT;
            matrix_21 = sinT;
            matrix_22 = cosT;
            return this;
        }

    }

    public class YoonMatrix4X4Double : IYoonMatrix, IYoonMatrix<double>
    {
        public int Length { get; set; } = 4;

        public double matrix_11
        {
            get => Array[0, 0];

            set
            {
                Array[0, 0] = value;
            }
        }

        public double matrix_12
        {
            get => Array[0, 1];

            set
            {
                Array[0, 1] = value;
            }
        }

        public double matrix_13
        {
            get => Array[0, 2];

            set
            {
                Array[0, 2] = value;
            }
        }

        public double matrix_14
        {
            get => Array[0, 3];

            set
            {
                Array[0, 3] = value;
            }
        }

        public double matrix_21
        {
            get => Array[1, 0];

            set
            {
                Array[1, 0] = value;
            }
        }

        public double matrix_22
        {
            get => Array[1, 1];

            set
            {
                Array[1, 1] = value;
            }
        }

        public double matrix_23
        {
            get => Array[1, 2];

            set
            {
                Array[1, 2] = value;
            }
        }

        public double matrix_24
        {
            get => Array[1, 3];

            set
            {
                Array[1, 3] = value;
            }
        }

        public double matrix_31
        {
            get => Array[2, 0];

            set
            {
                Array[2, 0] = value;
            }
        }

        public double matrix_32
        {
            get => Array[2, 1];

            set
            {
                Array[2, 1] = value;
            }
        }

        public double matrix_33
        {
            get => Array[2, 2];

            set
            {
                Array[2, 2] = value;
            }
        }

        public double matrix_34
        {
            get => Array[2, 3];

            set
            {
                Array[2, 3] = value;
            }
        }


        public double matrix_41
        {
            get => Array[3, 0];

            set
            {
                Array[3, 0] = value;
            }
        }

        public double matrix_42
        {
            get => Array[3, 1];

            set
            {
                Array[3, 1] = value;
            }
        }

        public double matrix_43
        {
            get => Array[3, 2];

            set
            {
                Array[3, 2] = value;
            }
        }

        public double matrix_44
        {
            get => Array[3, 3];

            set
            {
                Array[3, 3] = value;
            }
        }

        public YoonMatrix4X4Double()
        {
            matrix_11 = matrix_22 = matrix_33 = matrix_44 = 1;
            matrix_12 = matrix_13 = matrix_14 = matrix_21 = matrix_23 = matrix_24 = matrix_31 = matrix_32 = matrix_34 = matrix_41 = matrix_42 = matrix_43 = 0;
        }

        public YoonMatrix4X4Double(IYoonMatrix m)
        {
            this.CopyFrom(m);
        }

        public double[,] Array { get; set; } = new double[4, 4];

        public double Determinant
        {
            /*
                   matrix_11 * matrix_22 * matrix_33 * matrix_44 + matrix_11 * matrix_23 * matrix_34 * matrix_42 + matrix_11 * matrix_24 * matrix_32 * matrix_43
                 + matrix_12 * matrix_21 * matrix_34 * matrix_43 + matrix_12 * matrix_23 * matrix_31 * matrix_44 + matrix_12 * matrix_24 * matrix_33 * matrix_41
                 + matrix_13 * matrix_21 * matrix_32 * matrix_44 + matrix_13 * matrix_22 * matrix_34 * matrix_41 + matrix_13 * matrix_24 * matrix_31 * matrix_42
                 + matrix_14 * matrix_21 * matrix_33 * matrix_42 + matrix_14 * matrix_22 * matrix_31 * matrix_43 + matrix_14 * matrix_23 * matrix_32 * matrix_41
                 - matrix_11 * matrix_22 * matrix_34 * matrix_43 - matrix_11 * matrix_23 * matrix_32 * matrix_44 - matrix_11 * matrix_24 * matrix_33 * matrix_42
                 - matrix_12 * matrix_21 * matrix_33 * matrix_44 - matrix_12 * matrix_23 * matrix_34 * matrix_41 - matrix_12 * matrix_24 * matrix_31 * matrix_43
                 - matrix_13 * matrix_21 * matrix_34 * matrix_42 - matrix_13 * matrix_22 * matrix_31 * matrix_44 - matrix_13 * matrix_24 * matrix_32 * matrix_41
                 - matrix_14 * matrix_21 * matrix_32 * matrix_43 - matrix_14 * matrix_22 * matrix_33 * matrix_41 - matrix_14 * matrix_23 * matrix_31 * matrix_42;
            */
            get
            {
                double dSum = 0;
                for (int i = 0; i < Length; i++)
                    dSum += Array[0, i] * Cofactor(0, i);
                return dSum;
            }
        }

        public IYoonMatrix Clone()
        {
            YoonMatrix4X4Double pMatrix = new YoonMatrix4X4Double();
            pMatrix.matrix_11 = matrix_11;
            pMatrix.matrix_12 = matrix_12;
            pMatrix.matrix_13 = matrix_13;
            pMatrix.matrix_14 = matrix_14;
            pMatrix.matrix_21 = matrix_21;
            pMatrix.matrix_22 = matrix_22;
            pMatrix.matrix_23 = matrix_23;
            pMatrix.matrix_24 = matrix_24;
            pMatrix.matrix_31 = matrix_31;
            pMatrix.matrix_32 = matrix_32;
            pMatrix.matrix_33 = matrix_33;
            pMatrix.matrix_34 = matrix_34;
            pMatrix.matrix_41 = matrix_41;
            pMatrix.matrix_42 = matrix_42;
            pMatrix.matrix_43 = matrix_43;
            pMatrix.matrix_44 = matrix_44;
            return pMatrix;
        }

        public void CopyFrom(IYoonMatrix m)
        {
            if (m is YoonMatrix4X4Double pMatrix)
            {
                matrix_11 = pMatrix.matrix_11;
                matrix_12 = pMatrix.matrix_12;
                matrix_13 = pMatrix.matrix_13;
                matrix_14 = pMatrix.matrix_14;
                matrix_21 = pMatrix.matrix_21;
                matrix_22 = pMatrix.matrix_22;
                matrix_23 = pMatrix.matrix_23;
                matrix_24 = pMatrix.matrix_24;
                matrix_31 = pMatrix.matrix_31;
                matrix_32 = pMatrix.matrix_32;
                matrix_33 = pMatrix.matrix_33;
                matrix_34 = pMatrix.matrix_34;
                matrix_41 = pMatrix.matrix_41;
                matrix_42 = pMatrix.matrix_42;
                matrix_43 = pMatrix.matrix_43;
                matrix_44 = pMatrix.matrix_44;
            }
        }

        public double Cofactor(int nRow, int nCol)
        {
            return Math.Pow(-1, nRow + nCol) * (GetMinorMatrix(nRow, nCol) as YoonMatrix3X3Double).Determinant;
        }

        public IYoonMatrix GetMinorMatrix(int nRow, int nCol)
        {
            if (nRow < 0 || nRow >= Length || nCol < 0 || nCol >= Length)
                return new YoonMatrix3X3Double();

            double[,] pArray = new double[3, 3];
            int iCount = 0;
            int jCount = 0;
            for (int i = 0; i < Length; i++)
            {
                if (i == nRow) continue;
                for (int j = 0; j < Length; j++)
                {
                    if (j == nCol) continue;
                    pArray[iCount, jCount] = Array[i, j];
                    jCount++;
                }
                iCount++;
            }

            YoonMatrix3X3Double pMatrix = new YoonMatrix3X3Double();
            pMatrix.Array = pArray;
            return pMatrix;
        }

        public IYoonMatrix GetAdjointMatrix()
        {
            double[,] pArray = new double[3, 3];
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    pArray[i, j] = Cofactor(i, j);
                }
            }
            YoonMatrix4X4Double pMatrix = new YoonMatrix4X4Double();
            pMatrix.Array = pArray;
            return pMatrix.Transpose();
        }

        public IYoonMatrix Inverse()
        {
            if (Determinant == 0) return Unit();

            YoonMatrix4X4Double m = new YoonMatrix4X4Double(this);
            CopyFrom((m.GetAdjointMatrix() as YoonMatrix4X4Double) / m.Determinant);
            return this;
        }

        public IYoonMatrix Transpose()
        {
            YoonMatrix4X4Double m = new YoonMatrix4X4Double(this);
            matrix_12 = m.matrix_21;
            matrix_13 = m.matrix_31;
            matrix_14 = m.matrix_41;
            matrix_21 = m.matrix_12;
            matrix_23 = m.matrix_32;
            matrix_24 = m.matrix_42;
            matrix_31 = m.matrix_13;
            matrix_32 = m.matrix_23;
            matrix_34 = m.matrix_43;
            matrix_41 = m.matrix_14;
            matrix_42 = m.matrix_24;
            matrix_43 = m.matrix_34;
            return this;
        }

        public IYoonMatrix Unit()
        {
            matrix_11 = matrix_22 = matrix_33 = matrix_44 = 1;
            matrix_12 = matrix_13 = matrix_14 = matrix_21 = matrix_23 = matrix_24 = matrix_31 = matrix_32 = matrix_34 = matrix_41 = matrix_42 = matrix_43 = 0;
            return this;
        }

        public static YoonMatrix4X4Double operator *(double a, YoonMatrix4X4Double b)
        {
            YoonMatrix4X4Double m = new YoonMatrix4X4Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a * b.Array[i, j];
                }
            }
            return m;
        }

        public static YoonMatrix4X4Double operator /(YoonMatrix4X4Double a, double b)
        {
            YoonMatrix4X4Double m = new YoonMatrix4X4Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = a.Array[i, j] / b;
                }
            }
            return m;
        }

        public static YoonMatrix4X4Double operator *(YoonMatrix4X4Double a, YoonMatrix4X4Double b)
        {
            YoonMatrix4X4Double m = new YoonMatrix4X4Double();
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    m.Array[i, j] = 0;
                    for (int kValue = 0; kValue < m.Length; kValue++)
                        m.Array[i, j] += (a.Array[i, kValue] * b.Array[kValue, j]);
                }
            }
            return m;
        }
    }
    
    /// <summary>
    /// 3차원 동차변환 행렬 (double, 연산이 있으므로 제너릭 사용 불가)
    /// </summary>
    public class YoonMatrix3D : YoonMatrix4X4Double, IYoonMatrix3D<double>
    {
        public IYoonMatrix SetScaleUnit(double sx, double sy, double sz)
        {
            Unit();
            matrix_11 *= sx;
            matrix_22 *= sy;
            matrix_33 *= sz;
            return this;
        }

        public IYoonMatrix SetMovementUnit(double dx, double dy, double dz)
        {
            Unit();
            matrix_14 += dx;
            matrix_24 += dy;
            matrix_34 += dz;
            return this;
        }

        public IYoonMatrix SetMovementUnit(IYoonVector3D<double> v)
        {
            Unit();
            matrix_14 += v.X;
            matrix_24 += v.Y;
            matrix_34 += v.Z;
            return this;
        }

        public IYoonMatrix SetRotateXUnit(double angle)
        {
            Unit();
            double cosT = Math.Cos(angle);
            double sinT = Math.Sin(angle);
            matrix_22 = cosT;
            matrix_23 = -sinT;
            matrix_32 = sinT;
            matrix_33 = cosT;
            return this;
        }

        public IYoonMatrix SetRotateYUnit(double angle)
        {
            Unit();
            double cosT = Math.Cos(angle);
            double sinT = Math.Sin(angle);
            matrix_11 = cosT;
            matrix_13 = sinT;
            matrix_31 = -sinT;
            matrix_33 = cosT;
            return this;
        }

        public IYoonMatrix SetRotateZUnit(double angle)
        {
            Unit();
            double cosT = Math.Cos(angle);
            double sinT = Math.Sin(angle);
            matrix_11 = cosT;
            matrix_12 = -sinT;
            matrix_21 = sinT;
            matrix_22 = cosT;
            return this;
        }

    }

    /// <summary>
    /// 2차원 동차변환 벡터
    /// </summary>
    public class YoonVector2N : IYoonVector, IYoonVector2D<int>
    {
        public int Count { get; } = 3;

        public void CopyFrom(IYoonVector v)
        {
            if (v is YoonVector2N vec)
            {
                X = vec.X;
                Y = vec.Y;
                W = 1;
            }
        }

        public IYoonVector Clone()
        {
            YoonVector2N v = new YoonVector2N();
            v.X = this.X;
            v.Y = this.Y;
            v.W = this.W;
            return v;
        }
        [XmlIgnore]
        private static double DELTA = 0.0000000000001;
        [XmlIgnore]
        public int W
        {
            get => Array[2];

            set
            {
                Array[2] = value;
            }
        }
        [XmlAttribute]
        public int X
        {
            get => Array[0];

            set
            {
                Array[0] = value;
            }
        }
        [XmlAttribute]
        public int Y
        {
            get => Array[1];

            set
            {
                Array[1] = value;
            }
        }
        public IYoonCart<int> Cartesian
        {
            get => new YoonCartN(X, Y, 0, 0, 0, 0);
            set
            {
                try
                {
                    X = value.X;
                    Y = value.Y;
                }
                catch
                {
                    //
                }
            }
        }
        public int[] Array { get; set; } = new int[3];

        public YoonVector2N()
        {
            X = 0;
            Y = 0;
            W = 1;
        }
        public YoonVector2N(IYoonVector p)
        {
            CopyFrom(p);
        }
        public YoonVector2N(int dx, int dy)
        {
            X = dx;
            Y = dy;
            W = 1;
        }
        public void Zero()
        {
            X = 0;
            Y = 0;
            W = 1;
        }
        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public IYoonVector Unit()
        {
            double len = this.Length();
            if (len > DELTA)
            {
                len = 1.0 / len;
                X *= (int)len;
                Y *= (int)len;
            }
            return this;
        }
        public double Distance(IYoonVector p)
        {
            if (p is YoonVector2N vec)
            {
                double dx = this.X - vec.X;
                double dy = this.Y - vec.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
                return 0.0;
        }

        public IYoonVector Scale(int sx, int sy)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetScaleUnit(sx, sy);
            YoonVector2N v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            return this;
        }

        public IYoonVector Move(int dx, int dy)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetMovementUnit(dx, dy);
            YoonVector2N v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            return this;
        }

        public IYoonVector Move(IYoonVector pv)
        {
            if(pv is YoonVector2N pVector)
            {
                YoonMatrix2N pMatrix = new YoonMatrix2N();
                pMatrix.SetMovementUnit(pVector);
                YoonVector2N v = pMatrix * this;
                this.X = v.X;
                this.Y = v.Y;
            }
            return this;
        }

        public IYoonVector Rotate(double angle)
        {
            YoonMatrix2N pMatrix = new YoonMatrix2N();
            pMatrix.SetRotateUnit(angle);
            YoonVector2N v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            return this;
        }

        public IYoonVector Rotate(IYoonVector center, double angle)
        {
            if (center is YoonVector2N pVecCenter)
            {
                Move(-pVecCenter);
                Rotate(angle);
                Move(pVecCenter);
            }
            return this;
        }

        public static YoonVector2N operator *(YoonMatrix3X3Int m, YoonVector2N v)
        {
            YoonVector2N pVector = new YoonVector2N();
            for (int i = 0; i < v.Count; i++)
            {
                pVector.Array[i] = 0;
                for (int k = 0; k < m.Length; k++)
                    pVector.Array[i] += m.Array[i, k] * v.Array[k];
            }
            return pVector;
        }
        public static YoonVector2N operator *(YoonVector2N v, int a)
        {
            return new YoonVector2N(v.X * a, v.Y * a);
        }
        public static YoonVector2N operator +(YoonVector2N v1, YoonVector2N v2)
        {
            return new YoonVector2N(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static YoonVector2N operator -(YoonVector2N v1, YoonVector2N v2)
        {
            return new YoonVector2N(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static YoonVector2N operator -(YoonVector2N v)
        {
            return new YoonVector2N(-v.X, -v.Y);
        }
        public static YoonVector2N operator /(YoonVector2N v, int a)
        {
            return new YoonVector2N(v.X / a, v.Y / a);
        }
        public static int operator *(YoonVector2N v1, YoonVector2N v2) // dot product
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
    }

    /// <summary>
    /// 2차원 동차변환 벡터
    /// </summary>
    public class YoonVector2D : IYoonVector, IYoonVector2D<double>
    {
        public int Count { get; } = 3;

        public void CopyFrom(IYoonVector v)
        {
            if (v is YoonVector2D vec)
            {
                X = vec.X;
                Y = vec.Y;
                W = 1;
            }
        }

        public IYoonVector Clone()
        {
            YoonVector2D v = new YoonVector2D();
            v.X = this.X;
            v.Y = this.Y;
            v.W = this.W;
            return v;
        }
        [XmlIgnore]
        private static double DELTA = 0.0000000000001;
        [XmlIgnore]
        public double W
        {
            get => Array[2];

            set
            {
                Array[2] = value;
            }
        }
        [XmlAttribute]
        public double X
        {
            get => Array[0];

            set
            {
                Array[0] = value;
            }
        }
        [XmlAttribute]
        public double Y
        {
            get => Array[1];

            set
            {
                Array[1] = value;
            }
        }
        public IYoonCart<double> Cartesian
        {
            get => new YoonCartD(X, Y, 0, 0, 0, 0);
            set
            {
                try
                {
                    X = value.X;
                    Y = value.Y;
                }
                catch
                {
                    //
                }
            }
        }
        public double[] Array { get; set; } = new double[3];

        public YoonVector2D()
        {
            X = 0;
            Y = 0;
            W = 1;
        }
        public YoonVector2D(IYoonVector p)
        {
            CopyFrom(p);
        }
        public YoonVector2D(double dx, double dy)
        {
            X = dx;
            Y = dy;
            W = 1;
        }
        public void Zero()
        {
            X = 0;
            Y = 0;
            W = 1;
        }
        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public IYoonVector Unit()
        {
            double len = this.Length();
            if (len > DELTA)
            {
                len = 1.0 / len;
                X *= len;
                Y *= len;
            }
            return this;
        }
        public double Distance(IYoonVector p)
        {
            if (p is YoonVector2D vec)
            {
                double dx = this.X - vec.X;
                double dy = this.Y - vec.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
                return 0.0;
        }

        public IYoonVector Scale(double sx, double sy)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetScaleUnit(sx, sy);
            YoonVector2D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            return this;
        }

        public IYoonVector Move(double dx, double dy)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetMovementUnit(dx, dy);
            YoonVector2D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            return this;
        }

        public IYoonVector Move(IYoonVector pv)
        {
            if (pv is YoonVector2D pVector)
            {
                YoonMatrix2D pMatrix = new YoonMatrix2D();
                pMatrix.SetMovementUnit(pVector);
                YoonVector2D v = pMatrix * this;
                this.X = v.X;
                this.Y = v.Y;
            }
            return this;
        }

        public IYoonVector Rotate(double angle)
        {
            YoonMatrix2D pMatrix = new YoonMatrix2D();
            pMatrix.SetRotateUnit(angle);
            YoonVector2D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            return this;
        }

        public IYoonVector Rotate(IYoonVector center, double angle)
        {
            if (center is YoonVector2D pVecCenter)
            {
                Move(-pVecCenter);
                Rotate(angle);
                Move(pVecCenter);
            }
            return this;
        }

        public static YoonVector2D operator *(YoonMatrix3X3Double m, YoonVector2D v)
        {
            YoonVector2D pVector = new YoonVector2D();
            for (int i = 0; i < v.Count; i++)
            {
                pVector.Array[i] = 0;
                for (int k = 0; k < m.Length; k++)
                    pVector.Array[i] += m.Array[i, k] * v.Array[k];
            }
            return pVector;
        }
        public static YoonVector2D operator *(YoonVector2D v, double a)
        {
            return new YoonVector2D(v.X * a, v.Y * a);
        }
        public static YoonVector2D operator +(YoonVector2D v1, YoonVector2D v2)
        {
            return new YoonVector2D(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static YoonVector2D operator +(YoonVector2D v, YoonCartD c)
        {
            return new YoonVector2D(v.X + c.X, v.Y + c.Y);
        }
        public static YoonVector2D operator -(YoonVector2D v1, YoonVector2D v2)
        {
            return new YoonVector2D(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static YoonVector2D operator -(YoonVector2D v)
        {
            return new YoonVector2D(-v.X, -v.Y);
        }
        public static YoonVector2D operator -(YoonVector2D v, YoonCartD c)
        {
            return new YoonVector2D(v.X - c.X, v.Y - c.Y);
        }
        public static YoonVector2D operator /(YoonVector2D v, double a)
        {
            return new YoonVector2D(v.X / a, v.Y / a);
        }
        public static double operator *(YoonVector2D v1, YoonVector2D v2) // dot product
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
    }

    /// <summary>
    /// 2차원 동차변환 벡터
    /// </summary>
    public class YoonVector3D : IYoonVector, IYoonVector3D<double>
    {
        public int Count { get; } = 4;

        public void CopyFrom(IYoonVector v)
        {
            if (v is YoonVector2D vec)
            {
                X = vec.X;
                Y = vec.Y;
                W = 1;
            }
        }

        public IYoonVector Clone()
        {
            YoonVector3D v = new YoonVector3D();
            v.X = this.X;
            v.Y = this.Y;
            v.Z = this.Z;
            v.W = this.W;
            return v;
        }

        [XmlIgnore]
        private static double DELTA = 0.0000000000001;
        [XmlIgnore]
        public double W
        {
            get => Array[3];

            set
            {
                Array[3] = value;
            }
        }
        [XmlAttribute]
        public double X
        {
            get => Array[0];

            set
            {
                Array[0] = value;
            }
        }
        [XmlAttribute]
        public double Y
        {
            get => Array[1];

            set
            {
                Array[1] = value;
            }
        }
        [XmlAttribute]
        public double Z
        {
            get => Array[2];

            set
            {
                Array[2] = value;
            }
        }

        public IYoonCart<double> Cartesian
        {
            get => new YoonCartD(X, Y, Z, 0, 0, 0);
            set
            {
                try
                {
                    X = value.X;
                    Y = value.Y;
                    Z = value.Z;
                }
                catch
                {
                    //
                }
            }
        }

        public double[] Array { get; set; } = new double[4];

        public YoonVector3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public YoonVector3D(IYoonVector p)
        {
            CopyFrom(p);
        }

        public YoonVector3D(double dx, double dy, double dz)
        {
            X = dx;
            Y = dy;
            Z = dz;
            W = 1;
        }

        public void Zero()
        {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public IYoonVector Unit()
        {
            double len = this.Length();
            if (len > DELTA)
            {
                len = 1.0 / len;
                X *= len;
                Y *= len;
                Z *= len;
            }
            return this;
        }

        public double Distance(IYoonVector p)
        {
            if (p is YoonVector3D vec)
            {
                double dx = this.X - vec.X;
                double dy = this.Y - vec.Y;
                double dz = this.Z - vec.Z;
                return Math.Sqrt(dx * dx + dy * dy);
            }
            else
                return 0.0;
        }

        public IYoonVector Scale(double sx, double sy, double sz)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetScaleUnit(sx, sy, sz);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            return this;
        }

        public IYoonVector Move(double dx, double dy, double dz)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetMovementUnit(dx, dy, dz);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            return this;
        }

        public IYoonVector Move(IYoonVector pv)
        {
            if (pv is YoonVector3D pVector)
            {
                YoonMatrix3D pMatrix = new YoonMatrix3D();
                pMatrix.SetMovementUnit(pVector);
                YoonVector3D v = pMatrix * this;
                this.X = v.X;
                this.Y = v.Y;
                this.Z = v.Z;
            }
            return this;
        }

        public IYoonVector RotateX(double angle)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetRotateXUnit(angle);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            return this;
        }

        public IYoonVector RotateY(double angle)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetRotateYUnit(angle);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            return this;
        }

        public IYoonVector RotateZ(double angle)
        {
            YoonMatrix3D pMatrix = new YoonMatrix3D();
            pMatrix.SetRotateZUnit(angle);
            YoonVector3D v = pMatrix * this;
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            return this;
        }

        public static YoonVector3D operator *(YoonMatrix4X4Double m, YoonVector3D v)
        {
            YoonVector3D pVector = new YoonVector3D();
            for (int i = 0; i < v.Count; i++)
            {
                pVector.Array[i] = 0;
                for (int k = 0; k < m.Length; k++)
                    pVector.Array[i] += m.Array[i, k] * v.Array[k];
            }
            return pVector;
        }
        public static YoonVector3D operator *(YoonVector3D v, double a)
        {
            return new YoonVector3D(v.X * a, v.Y * a, v.Z * a);
        }
        public static YoonVector3D operator +(YoonVector3D v1, YoonVector3D v2)
        {
            return new YoonVector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static YoonVector3D operator +(YoonVector3D v, YoonCartD c)
        {
            return new YoonVector3D(v.X + c.X, v.Y + c.Y, v.Z + c.Z);
        }
        public static YoonVector3D operator -(YoonVector3D v1, YoonVector3D v2)
        {
            return new YoonVector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static YoonVector3D operator -(YoonVector3D v)
        {
            return new YoonVector3D(-v.X, -v.Y, -v.Z);
        }
        public static YoonVector3D operator -(YoonVector3D v, YoonCartD c)
        {
            return new YoonVector3D(v.X - c.X, v.Y - c.Y, v.Z - c.Z);
        }
        public static YoonVector3D operator /(YoonVector3D v, double a)
        {
            return new YoonVector3D(v.X / a, v.Y / a, v.Z / a);
        }
        public static double operator *(YoonVector3D v1, YoonVector3D v2) // dot product
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
    }

    public class YoonJointD : IYoonJoint, IYoonJoint<double>
    {
        public IYoonJoint Clone()
        {
            YoonJointD j = new YoonJointD();
            j.J1 = J1;
            j.J2 = J2;
            j.J3 = J3;
            j.J4 = J4;
            j.J5 = J5;
            j.J6 = J6;
            return j;
        }

        public void CopyFrom(IYoonJoint j)
        {
            if(j is YoonJointD joint)
            {
                J1 = joint.J1;
                J2 = joint.J2;
                J3 = joint.J3;
                J4 = joint.J4;
                J5 = joint.J5;
                J6 = joint.J6;
            }
        }

        [XmlIgnore]
        public double J1 { get; set; }
        [XmlAttribute]
        public double J2 { get; set; }
        [XmlAttribute]
        public double J3 { get; set; }
        [XmlIgnore]
        public double J4 { get; set; }
        [XmlAttribute]
        public double J5 { get; set; }
        [XmlAttribute]
        public double J6 { get; set; }

        public double[] ToArray
        {
            get => new double[6] { J1, J2, J3, J4, J5, J6 };
        }

        public YoonJointD()
        {
            Zero();
        }
        public YoonJointD(IYoonJoint j)
        {
            CopyFrom(j);
        }
        public YoonJointD(double dJ1, double dJ2, double dJ3, double dJ4, double dJ5, double dJ6)
        {
            J1 = dJ1;
            J2 = dJ2;
            J3 = dJ3;
            J4 = dJ4;
            J5 = dJ5;
            J6 = dJ6;
        }

        public void Zero()
        {
            J1 = J2 = J3 = J4 = J5 = J6 = 0;
        }
        public static YoonJointD operator *(YoonJointD j, double a)
        {
            return new YoonJointD(j.J1 * a, j.J2 * a, j.J3 * a, j.J4 * a, j.J5 * a, j.J6 * a);
        }
        public static YoonJointD operator +(YoonJointD j1, YoonJointD j2)
        {
            return new YoonJointD(j1.J1 + j2.J1, j1.J2 + j2.J2, j1.J3 + j2.J3, j1.J4 + j2.J4, j1.J5 + j2.J5, j1.J6 + j2.J6);
        }
        public static YoonJointD operator -(YoonJointD j1, YoonJointD j2)
        {
            return new YoonJointD(j1.J1 - j2.J1, j1.J2 - j2.J2, j1.J3 - j2.J3, j1.J4 - j2.J4, j1.J5 - j2.J5, j1.J6 - j2.J6);
        }
        public static YoonJointD operator /(YoonJointD j, double a)
        {
            return new YoonJointD(j.J1 / a, j.J2 / a, j.J3 / a, j.J4 / a, j.J5 / a, j.J6 / a);
        }
    }

    public class YoonCartN : IYoonCart, IYoonCart<int>
    {
        public IYoonCart Clone()
        {
            YoonCartN c = new YoonCartN();
            c.X = X;
            c.Y = Y;
            c.Z = Z;
            c.RX = RX;
            c.RY = RY;
            c.RZ = RZ;
            return c;
        }

        public void CopyFrom(IYoonCart c)
        {
            if (c is YoonCartN cart)
            {
                X = cart.X;
                Y = cart.Y;
                Z = cart.Z;
                RX = cart.RX;
                RY = cart.RY;
                RZ = cart.RZ;
            }
        }

        [XmlIgnore]
        public int X { get; set; }
        [XmlAttribute]
        public int Y { get; set; }
        [XmlAttribute]
        public int Z { get; set; }
        [XmlIgnore]
        public int RX { get; set; }
        [XmlAttribute]
        public int RY { get; set; }
        [XmlAttribute]
        public int RZ { get; set; }

        public int[] Array
        {
            get => new int[6] { X, Y, Z, RX, RY, RZ };
            set
            {
                try
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                    RX = value[3];
                    RY = value[4];
                    RZ = value[5];
                }
                catch
                {
                    //
                }
            }
        }

        public YoonCartN()
        {
            Unit();
        }
        public YoonCartN(IYoonCart c)
        {
            CopyFrom(c);
        }
        public YoonCartN(int dX, int dY, int dZ, int dRX, int dRY, int dRZ)
        {
            X = dX;
            Y = dY;
            Z = dZ;
            RX = dRX;
            RY = dRY;
            RZ = dRZ;
        }

        public void Zero()
        {
            X = Y = Z = RX = RY = RZ = 0;
        }
        public void Unit()
        {
            X = Y = Z = 1;
            RX = RY = RZ = 0;
        }
        public static YoonCartN operator *(YoonCartN c, int a)
        {
            return new YoonCartN(c.X * a, c.Y * a, c.Z * a, c.RX, c.RY, c.RZ);
        }
        public static YoonCartN operator +(YoonCartN c1, YoonCartN c2)
        {
            return new YoonCartN(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z, c1.RX + c2.RX, c1.RY + c2.RY, c1.RZ + c2.RZ);
        }
        public static YoonCartN operator +(YoonCartN c, YoonVector2N v)
        {
            return new YoonCartN(c.X + v.X, c.Y + v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartN operator -(YoonCartN c1, YoonCartN c2)
        {
            return new YoonCartN(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z, c1.RX - c2.RX, c1.RY - c2.RY, c1.RZ - c2.RZ);
        }
        public static YoonCartN operator -(YoonCartN c, YoonVector2N v)
        {
            return new YoonCartN(c.X - v.X, c.Y - v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartN operator /(YoonCartN c, int a)
        {
            return new YoonCartN(c.X / a, c.Y / a, c.Z / a, c.RX, c.RY, c.RZ);
        }
    }
    
    public class YoonCartD : IYoonCart, IYoonCart<double>
    {
        public IYoonCart Clone()
        {
            YoonCartD c = new YoonCartD();
            c.X = X;
            c.Y = Y;
            c.Z = Z;
            c.RX = RX;
            c.RY = RY;
            c.RZ = RZ;
            return c;
        }

        public void CopyFrom(IYoonCart c)
        {
            if (c is YoonCartD cart)
            {
                X = cart.X;
                Y = cart.Y;
                Z = cart.Z;
                RX = cart.RX;
                RY = cart.RY;
                RZ = cart.RZ;
            }
        }

        [XmlIgnore]
        public double X { get; set; }
        [XmlAttribute]
        public double Y { get; set; }
        [XmlAttribute]
        public double Z { get; set; }
        [XmlIgnore]
        public double RX { get; set; }
        [XmlAttribute]
        public double RY { get; set; }
        [XmlAttribute]
        public double RZ { get; set; }

        public double[] Array
        {
            get => new double[6] { X, Y, Z, RX, RY, RZ };
            set
            {
                try
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                    RX = value[3];
                    RY = value[4];
                    RZ = value[5];
                }
                catch
                {
                    //
                }
            }
        }

        public YoonCartD()
        {
            Unit();
        }
        public YoonCartD(IYoonCart c)
        {
            CopyFrom(c);
        }
        public YoonCartD(double dX, double dY, double dZ, double dRX, double dRY, double dRZ)
        {
            X = dX;
            Y = dY;
            Z = dZ;
            RX = dRX;
            RY = dRY;
            RZ = dRZ;
        }

        public void Zero()
        {
            X = Y = Z = RX = RY = RZ = 0;
        }
        public void Unit()
        {
            X = Y = Z = 1;
            RX = RY = RZ = 0;
        }
        public static YoonCartD operator *(YoonCartD c, double a)
        {
            return new YoonCartD(c.X * a, c.Y * a, c.Z * a, c.RX, c.RY, c.RZ);
        }
        public static YoonCartD operator +(YoonCartD c1, YoonCartD c2)
        {
            return new YoonCartD(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z, c1.RX + c2.RX, c1.RY + c2.RY, c1.RZ + c2.RZ);
        }
        public static YoonCartD operator +(YoonCartD c, YoonVector2D v)
        {
            return new YoonCartD(c.X + v.X, c.Y + v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartD operator -(YoonCartD c1, YoonCartD c2)
        {
            return new YoonCartD(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z, c1.RX - c2.RX, c1.RY - c2.RY, c1.RZ - c2.RZ);
        }
        public static YoonCartD operator -(YoonCartD c, YoonVector2D v)
        {
            return new YoonCartD(c.X - v.X, c.Y - v.Y, c.Z, c.RX, c.RY, c.RZ);
        }
        public static YoonCartD operator /(YoonCartD c, double a)
        {
            return new YoonCartD(c.X / a, c.Y / a, c.Z / a, c.RX, c.RY, c.RZ);
        }
    }

    public class YoonTriangle2N : IYoonTriangle, IYoonTriangle2D<int>
    {
        public IYoonTriangle Clone()
        {
            YoonTriangle2N t = new YoonTriangle2N();
            t.X = this.X;
            t.Y = this.Y;
            t.Height = this.Height;
            return t;
        }
        public void CopyFrom(IYoonTriangle t)
        {
            if (t is YoonTriangle2N trg)
            {
                X = trg.X;
                Y = trg.Y;
                Height = trg.Height;
            }
        }

        [XmlAttribute]
        public int X { get; set; }
        [XmlAttribute]
        public int Y { get; set; }
        [XmlAttribute]
        public int Height { get; set; }

        public YoonTriangle2N()
        {
            X = 0;
            Y = 0;
            Height = 0;
        }
        public YoonTriangle2N(int dx, int dy, int dh)
        {
            X = dx;
            Y = dy;
            Height = dh;
        }
        public int Area()
        {
            return X * Y / 2;
        }
    }

    public class YoonTriangle2D : IYoonTriangle, IYoonTriangle2D<double>
    {
        public IYoonTriangle Clone()
        {
            YoonTriangle2D t = new YoonTriangle2D();
            t.X = this.X;
            t.Y = this.Y;
            t.Height = this.Height;
            return t;
        }
        public void CopyFrom(IYoonTriangle t)
        {
            if (t is YoonTriangle2D trg)
            {
                X = trg.X;
                Y = trg.Y;
                Height = trg.Height;
            }
        }

        [XmlAttribute]
        public double X { get; set; }
        [XmlAttribute]
        public double Y { get; set; }
        [XmlAttribute]
        public double Height { get; set; }

        public YoonTriangle2D()
        {
            X = 0;
            Y = 0;
            Height = 0;
        }
        public YoonTriangle2D(double dx, double dy, double dh)
        {
            X = dx;
            Y = dy;
            Height = dh;
        }

        public double Area()
        {
            return 0.5 * X * Y;
        }
    }

    /// <summary>
    /// 사각형 대응 변수 (기준좌표계 : Pixel과 동일/ 좌+상)
    /// </summary>
    public class YoonRect2N : IYoonRect, IYoonRect2D<int>
    {
        public IYoonRect Clone()
        {
            YoonRect2N r = new YoonRect2N();
            r.CenterPos = this.CenterPos.Clone() as YoonVector2N;
            r.Width = this.Width;
            r.Height = this.Height;
            return r;
        }
        public void CopyFrom(IYoonRect r)
        {
            if (r is YoonRect2N rect)
            {
                CenterPos = rect.CenterPos.Clone() as YoonVector2N;
                Width = rect.Width;
                Height = rect.Height;
            }
        }

        [XmlAttribute]
        public IYoonVector2D<int> CenterPos { get; set; }
        [XmlAttribute]
        public int Width { get; set; }
        [XmlAttribute]
        public int Height { get; set; }

        public int Left
        {
            get => (CenterPos as YoonVector2N).X - Width / 2;
        }

        public int Top
        {
            get => (CenterPos as YoonVector2N).Y - Height / 2;
        }

        public int Right
        {
            get => (CenterPos as YoonVector2N).X + Width / 2;
        }
        public int Bottom
        {
            get => (CenterPos as YoonVector2N).Y + Height / 2;
        }

        public IYoonVector TopLeft
        {
            get => new YoonVector2N(CenterPos.X - Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector TopRight
        {
            get => new YoonVector2N(CenterPos.X + Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector BottomLeft
        {
            get => new YoonVector2N(CenterPos.X - Width / 2, CenterPos.Y + Height / 2);
        }

        public IYoonVector BottomRight
        {
            get => new YoonVector2N(CenterPos.X + Width / 2, CenterPos.Y + Height / 2);
        }

        public YoonRect2N()
        {
            CenterPos = new YoonVector2N();
            CenterPos.X = 0;
            CenterPos.Y = 0;
            Width = 0;
            Height = 0;
        }
        public YoonRect2N(int dx, int dy, int dw, int dh)
        {
            CenterPos = new YoonVector2N();
            CenterPos.X = dx;
            CenterPos.Y = dy;
            Width = dw;
            Height = dh;
        }
        public int Area()
        {
            return Width * Height;
        }
    }

    /// <summary>
    /// 사각형 대응 변수
    /// </summary>
    public class YoonRect2D : IYoonRect, IYoonRect2D<double>
    {
        public IYoonRect Clone()
        {
            YoonRect2D r = new YoonRect2D();
            r.CenterPos = this.CenterPos.Clone() as YoonVector2D;
            r.Width = this.Width;
            r.Height = this.Height;
            return r;
        }
        public void CopyFrom(IYoonRect r)
        {
            if (r is YoonRect2D rect)
            {
                CenterPos = rect.CenterPos.Clone() as YoonVector2D;
                Width = rect.Width;
                Height = rect.Height;
            }
        }

        [XmlAttribute]
        public IYoonVector2D<double> CenterPos { get; set; }
        [XmlAttribute]
        public double Width { get; set; }
        [XmlAttribute]
        public double Height { get; set; }

        public double Left
        {
            get => CenterPos.X - Width / 2;
        }

        public double Top
        {
            get => CenterPos.Y - Height / 2;
        }

        public double Right
        {
            get => CenterPos.X + Width / 2;
        }

        public double Bottom
        {
            get => CenterPos.Y + Height / 2;
        }

        public IYoonVector TopLeft
        {
            get => new YoonVector2D(CenterPos.X - Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector TopRight
        {
            get => new YoonVector2D(CenterPos.X + Width / 2, CenterPos.Y - Height / 2);
        }

        public IYoonVector BottomLeft
        {
            get => new YoonVector2D(CenterPos.X - Width / 2, CenterPos.Y + Height / 2);
        }

        public IYoonVector BottomRight
        {
            get => new YoonVector2D(CenterPos.X + Width / 2, CenterPos.Y + Height / 2);
        }

        public YoonRect2D()
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = 0;
            CenterPos.Y = 0;
            Width = 0;
            Height = 0;
        }
        public YoonRect2D(double dx, double dy, double dw, double dh)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = dx;
            CenterPos.Y = dy;
            Width = dw;
            Height = dh;
        }
        public double Area()
        {
            return Width * Height;
        }
    }

    public class YoonRectAffine2D : IYoonRect, IYoonRect2D<double>
    {
        public IYoonRect Clone()
        {
            YoonRectAffine2D r = new YoonRectAffine2D(0.0, 0.0, 0.0);
            r.CenterPos = this.CenterPos.Clone() as YoonVector2D;
            r.Width = this.Width;
            r.Height = this.Height;
            r.Rotation = this.Rotation;
            return r;
        }
        public void CopyFrom(IYoonRect r)
        {
            if (r is YoonRectAffine2D rect)
            {
                CenterPos = rect.CenterPos.Clone() as YoonVector2D;
                Width = rect.Width;
                Height = rect.Height;
                Rotation = rect.Rotation;
            }
        }

        [XmlAttribute]
        public IYoonVector2D<double> CenterPos
        {
            get => m_vecCenter;
            set
            {
                m_vecCenter = value as YoonVector2D;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);
            }
        }
        [XmlAttribute]
        public double Width
        {
            get => m_dWidth;
            set
            {
                m_dWidth = value;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);
            }
        }
        [XmlAttribute]
        public double Height
        {
            get => m_dHeight;
            set
            {
                m_dHeight = value;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);
            }
        }
        [XmlAttribute]
        public double Rotation
        {
            get => m_dRotation;
            set
            {
                m_dRotation = value;
                InitRectOrigin(m_vecCenter as YoonVector2D, m_dWidth, m_dHeight);

                m_vecCornerRotate_TopLeft = m_vecCornerOrigin_TopLeft.Rotate(m_vecCenter, m_dRotation) as YoonVector2D;
                m_vecCornerRotate_BottomLeft = m_vecCornerOrigin_BottomLeft.Rotate(m_vecCenter, m_dRotation) as YoonVector2D;
                m_vecCornerRotate_TopRight = m_vecCornerOrigin_TopRight.Rotate(m_vecCenter, m_dRotation) as YoonVector2D;
                m_vecCornerRotate_BottomRight = m_vecCornerOrigin_BottomRight.Rotate(m_vecCenter, m_dRotation) as YoonVector2D;
            }
        }

        private void InitRectOrigin(YoonVector2D vecCenter, double dWidth, double dHeight)
        {
            m_vecCornerOrigin_TopLeft.X = vecCenter.X - dWidth / 2;
            m_vecCornerOrigin_TopLeft.Y = vecCenter.Y - dHeight / 2;
            m_vecCornerOrigin_TopRight.X = vecCenter.X + dWidth / 2;
            m_vecCornerOrigin_TopRight.Y = vecCenter.Y - dHeight / 2;
            m_vecCornerOrigin_BottomLeft.X = vecCenter.X - dWidth / 2;
            m_vecCornerOrigin_BottomLeft.Y = vecCenter.Y + dHeight / 2;
            m_vecCornerOrigin_BottomRight.X = vecCenter.X + dWidth / 2;
            m_vecCornerOrigin_BottomRight.Y = vecCenter.Y + dHeight / 2;
        }

        #region 내부 변수
        private YoonVector2D m_vecCornerOrigin_TopLeft = new YoonVector2D();
        private YoonVector2D m_vecCornerOrigin_BottomLeft = new YoonVector2D();
        private YoonVector2D m_vecCornerOrigin_TopRight = new YoonVector2D();
        private YoonVector2D m_vecCornerOrigin_BottomRight = new YoonVector2D();
        private YoonVector2D m_vecCornerRotate_TopLeft = null;
        private YoonVector2D m_vecCornerRotate_BottomLeft = null;
        private YoonVector2D m_vecCornerRotate_TopRight = null;
        private YoonVector2D m_vecCornerRotate_BottomRight = null;
        private YoonVector2D m_vecCenter = new YoonVector2D(); // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        private double m_dWidth = 0.0; // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        private double m_dHeight = 0.0; // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        private double m_dRotation = 0.0; // Set 내 InitOrigin과 충돌에 따른 StackOverflow 방지용
        #endregion

        public double Left
        {
            get => CenterPos.X - Width / 2;
        }

        public double Top
        {
            get => CenterPos.Y - Height / 2;
        }

        public double Right
        {
            get => CenterPos.X + Width / 2;
        }

        public double Bottom
        {
            get => CenterPos.Y + Height / 2;
        }

        public IYoonVector TopLeft
        {
            get => m_vecCornerRotate_TopLeft;

        }

        public IYoonVector TopRight
        {
            get => m_vecCornerRotate_TopRight;
        }

        public IYoonVector BottomLeft
        {
            get => m_vecCornerRotate_BottomLeft;
        }

        public IYoonVector BottomRight
        {
            get => m_vecCornerRotate_BottomRight;
        }

        public YoonRectAffine2D(double dWidth, double dHeight, double dTheta)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = 0;
            CenterPos.Y = 0;
            Width = dWidth;
            Height = dHeight;

            Rotation = dTheta;
        }

        public YoonRectAffine2D(double dX, double dY, double dWidth, double dHeight, double dTheta)
        {
            CenterPos = new YoonVector2D();
            CenterPos.X = dX;
            CenterPos.Y = dY;
            Width = dWidth;
            Height = dHeight;

            Rotation = dTheta;
        }

        public YoonRectAffine2D(YoonVector2D vecPos, double dWidth, double dHeight, double dTheta)
        {
            CenterPos = vecPos.Clone() as YoonVector2D;
            Width = dWidth;
            Height = dHeight;
            Rotation = dTheta;
        }

        public double Area()
        {
            return Width * Height;
        }
    }

    public class YoonObjectRect : IYoonObject
    {
        public int LabelNo { get; set; }
        public IYoonVector FeaturePos { get; set; }
        public IYoonRect PickArea { get; set; }
        public int PixelCount { get; set; }
        public double Score { get; set; }

        public IYoonObject Clone()
        {
            YoonObjectRect pObject = new YoonObjectRect();
            pObject.LabelNo = LabelNo;
            pObject.FeaturePos = FeaturePos.Clone();
            pObject.PickArea = PickArea.Clone();
            pObject.PixelCount = PixelCount;
            return pObject;
        }

        public void CopyFrom(IYoonObject pObject)
        {
            if (pObject == null) return;

            if(pObject is YoonObjectRect pObjectRect)
            {
                LabelNo = pObjectRect.LabelNo;
                FeaturePos = pObjectRect.FeaturePos.Clone();
                PickArea = pObjectRect.PickArea.Clone();
                PixelCount = pObjectRect.PixelCount;
            }
        }
    }

    public class YoonObjectLine : IYoonObject
    {
        public int LabelNo { get; set; }
        public IYoonVector StartPos { get; set; }
        public IYoonVector EndPos { get; set; }
        public double Score { get; set; }

        public IYoonObject Clone()
        {
            YoonObjectLine pObject = new YoonObjectLine();
            pObject.LabelNo = LabelNo;
            pObject.StartPos = StartPos.Clone();
            pObject.EndPos = EndPos.Clone();
            return pObject;
        }

        public void CopyFrom(IYoonObject pObject)
        {
            if (pObject == null) return;

            if (pObject is YoonObjectLine pObjectLine)
            {
                LabelNo = pObjectLine.LabelNo;
                StartPos = pObjectLine.StartPos.Clone();
                EndPos = pObjectLine.EndPos.Clone();
            }
        }
    }
}
