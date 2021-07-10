using System;
using System.Collections.Generic;

namespace YoonFactory
{
    public class YoonMatrix2X2Int : IYoonMatrix, IEquatable<YoonMatrix2X2Int>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as YoonMatrix2X2Int);
        }

        public bool Equals(YoonMatrix2X2Int other)
        {
            return other != null &&
                   Length == other.Length &&
                   matrix_11 == other.matrix_11 &&
                   matrix_12 == other.matrix_12 &&
                   matrix_21 == other.matrix_21 &&
                   matrix_22 == other.matrix_22 &&
                   EqualityComparer<int[,]>.Default.Equals(Array, other.Array) &&
                   Determinant == other.Determinant;
        }

        public override int GetHashCode()
        {
            int hashCode = 915136901;
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_11.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_12.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_21.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_22.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int[,]>.Default.GetHashCode(Array);
            hashCode = hashCode * -1521134295 + Determinant.GetHashCode();
            return hashCode;
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

        public static bool operator ==(YoonMatrix2X2Int a, YoonMatrix2X2Int b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return false;
                }
            }
            return true;
        }

        public static bool operator !=(YoonMatrix2X2Int a, YoonMatrix2X2Int b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return true;
                }
            }
            return false;
        }
    }

    public class YoonMatrix2X2Double : IYoonMatrix, IEquatable<YoonMatrix2X2Double>
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
            matrix_12 = -m.matrix_12 / m.Determinant;
            matrix_21 = -m.matrix_21 / m.Determinant;
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

        public override bool Equals(object obj)
        {
            return Equals(obj as YoonMatrix2X2Double);
        }

        public bool Equals(YoonMatrix2X2Double other)
        {
            return other != null &&
                   Length == other.Length &&
                   matrix_11 == other.matrix_11 &&
                   matrix_12 == other.matrix_12 &&
                   matrix_21 == other.matrix_21 &&
                   matrix_22 == other.matrix_22 &&
                   EqualityComparer<double[,]>.Default.Equals(Array, other.Array) &&
                   Determinant == other.Determinant;
        }

        public override int GetHashCode()
        {
            int hashCode = 915136901;
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_11.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_12.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_21.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_22.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double[,]>.Default.GetHashCode(Array);
            hashCode = hashCode * -1521134295 + Determinant.GetHashCode();
            return hashCode;
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

        public static bool operator ==(YoonMatrix2X2Double a, YoonMatrix2X2Double b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return false;
                }
            }
            return true;
        }

        public static bool operator !=(YoonMatrix2X2Double a, YoonMatrix2X2Double b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return true;
                }
            }
            return false;
        }
    }

    public class YoonMatrix3X3Int : IYoonMatrix, IYoonMatrix<int>, IEquatable<YoonMatrix3X3Int>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as YoonMatrix3X3Int);
        }

        public bool Equals(YoonMatrix3X3Int other)
        {
            return other != null &&
                   Length == other.Length &&
                   matrix_11 == other.matrix_11 &&
                   matrix_12 == other.matrix_12 &&
                   matrix_13 == other.matrix_13 &&
                   matrix_21 == other.matrix_21 &&
                   matrix_22 == other.matrix_22 &&
                   matrix_23 == other.matrix_23 &&
                   matrix_31 == other.matrix_31 &&
                   matrix_32 == other.matrix_32 &&
                   matrix_33 == other.matrix_33 &&
                   EqualityComparer<int[,]>.Default.Equals(Array, other.Array) &&
                   Determinant == other.Determinant;
        }

        public override int GetHashCode()
        {
            int hashCode = -1889127308;
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_11.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_12.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_13.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_21.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_22.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_23.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_31.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_32.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_33.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int[,]>.Default.GetHashCode(Array);
            hashCode = hashCode * -1521134295 + Determinant.GetHashCode();
            return hashCode;
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

        public static bool operator ==(YoonMatrix3X3Int a, YoonMatrix3X3Int b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return false;
                }
            }
            return true;
        }

        public static bool operator !=(YoonMatrix3X3Int a, YoonMatrix3X3Int b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return true;
                }
            }
            return false;
        }
    }

    public class YoonMatrix3X3Double : IYoonMatrix, IYoonMatrix<double>, IEquatable<YoonMatrix3X3Double>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as YoonMatrix3X3Double);
        }

        public bool Equals(YoonMatrix3X3Double other)
        {
            return other != null &&
                   Length == other.Length &&
                   matrix_11 == other.matrix_11 &&
                   matrix_12 == other.matrix_12 &&
                   matrix_13 == other.matrix_13 &&
                   matrix_21 == other.matrix_21 &&
                   matrix_22 == other.matrix_22 &&
                   matrix_23 == other.matrix_23 &&
                   matrix_31 == other.matrix_31 &&
                   matrix_32 == other.matrix_32 &&
                   matrix_33 == other.matrix_33 &&
                   EqualityComparer<double[,]>.Default.Equals(Array, other.Array) &&
                   Determinant == other.Determinant;
        }

        public override int GetHashCode()
        {
            int hashCode = -1889127308;
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_11.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_12.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_13.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_21.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_22.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_23.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_31.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_32.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_33.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double[,]>.Default.GetHashCode(Array);
            hashCode = hashCode * -1521134295 + Determinant.GetHashCode();
            return hashCode;
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

        public static bool operator ==(YoonMatrix3X3Double a, YoonMatrix3X3Double b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return false;
                }
            }
            return true;
        }

        public static bool operator !=(YoonMatrix3X3Double a, YoonMatrix3X3Double b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return true;
                }
            }
            return false;
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

    public class YoonMatrix4X4Double : IYoonMatrix, IYoonMatrix<double>, IEquatable<YoonMatrix4X4Double>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as YoonMatrix4X4Double);
        }

        public bool Equals(YoonMatrix4X4Double other)
        {
            return other != null &&
                   Length == other.Length &&
                   matrix_11 == other.matrix_11 &&
                   matrix_12 == other.matrix_12 &&
                   matrix_13 == other.matrix_13 &&
                   matrix_14 == other.matrix_14 &&
                   matrix_21 == other.matrix_21 &&
                   matrix_22 == other.matrix_22 &&
                   matrix_23 == other.matrix_23 &&
                   matrix_24 == other.matrix_24 &&
                   matrix_31 == other.matrix_31 &&
                   matrix_32 == other.matrix_32 &&
                   matrix_33 == other.matrix_33 &&
                   matrix_34 == other.matrix_34 &&
                   matrix_41 == other.matrix_41 &&
                   matrix_42 == other.matrix_42 &&
                   matrix_43 == other.matrix_43 &&
                   matrix_44 == other.matrix_44 &&
                   EqualityComparer<double[,]>.Default.Equals(Array, other.Array) &&
                   Determinant == other.Determinant;
        }

        public override int GetHashCode()
        {
            int hashCode = -1235618589;
            hashCode = hashCode * -1521134295 + Length.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_11.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_12.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_13.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_14.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_21.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_22.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_23.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_24.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_31.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_32.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_33.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_34.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_41.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_42.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_43.GetHashCode();
            hashCode = hashCode * -1521134295 + matrix_44.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double[,]>.Default.GetHashCode(Array);
            hashCode = hashCode * -1521134295 + Determinant.GetHashCode();
            return hashCode;
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

        public static bool operator ==(YoonMatrix4X4Double a, YoonMatrix4X4Double b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return false;
                }
            }
            return true;
        }

        public static bool operator !=(YoonMatrix4X4Double a, YoonMatrix4X4Double b)
        {
            for (int i = 0; i < a?.Length; i++)
            {
                for (int j = 0; j < a?.Length; j++)
                {
                    if (a?.Array[i, j] != b?.Array[i, j])
                        return true;
                }
            }
            return false;
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

}
