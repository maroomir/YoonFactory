using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory
{
    public interface IYoonMatrix
    {
        int Length { get; }

        IYoonMatrix Clone();
        IYoonMatrix Inverse();
        IYoonMatrix Transpose();
        IYoonMatrix Unit();
        void CopyFrom(IYoonMatrix m);
    }

    public interface IYoonMatrix<T> : IYoonMatrix where T : IComparable, IComparable<T>
    {
        T[,] Array { get; set; }
        T Determinant { get; }  // 행렬식
        T Cofactor(int nRow, int nCol); // 여인자
        IYoonMatrix GetMinorMatrix(int nRow, int nCol); // 소행렬
        IYoonMatrix GetAdjointMatrix();    // 수반행렬
    }

    public interface IYoonMatrix2D<T> : IYoonMatrix<T> where T : IComparable, IComparable<T>
    {
        IYoonMatrix SetScaleUnit(T sx, T sy);
        IYoonMatrix SetMovementUnit(T dx, T dy);
        IYoonMatrix SetMovementUnit(IYoonVector2D<T> v);
        IYoonMatrix SetRotateUnit(double angle);
    }

    public interface IYoonMatrix3D<T> : IYoonMatrix<T> where T : IComparable, IComparable<T>
    {
        IYoonMatrix SetScaleUnit(T sx, T sy, T sz);
        IYoonMatrix SetMovementUnit(T dx, T dy, T dz);
        IYoonMatrix SetMovementUnit(IYoonVector3D<T> v);
        IYoonMatrix SetRotateXUnit(double angle);
        IYoonMatrix SetRotateYUnit(double angle);
        IYoonMatrix SetRotateZUnit(double angle);
    }

    public interface IYoonVector
    {
        int Count { get; }

        bool Equals(IYoonVector pVector);
        IYoonVector Clone();
        void CopyFrom(IYoonVector pVector);
        void Zero();
        IYoonVector Unit();
        double Length();
        double Distance(IYoonVector pVector);
    }

    public interface IYoonVector2D<T> : IYoonVector where T : IComparable, IComparable<T>
    {
        T W { get; set; }
        T X { get; set; }
        T Y { get; set; }
        T[] Array { get; set; }
        eYoonDir2D Direction { get; set; }

        IYoonCart<T> Cartesian { get; set; }
        IYoonVector Scale(T sx, T sy);
        IYoonVector Move(T dx, T dy);
        IYoonVector Move(IYoonVector v);
        IYoonVector Move(eYoonDir2D dir);
        IYoonVector Rotate(double angle);
        IYoonVector Rotate(IYoonVector center, double angle);
    }

    public interface IYoonVector3D<T> : IYoonVector where T : IComparable, IComparable<T>
    {
        T W { get; set; }
        T X { get; set; }
        T Y { get; set; }
        T Z { get; set; }
        T[] Array { get; set; }
        IYoonCart<T> Cartesian { get; set; }
        IYoonVector Scale(T sx, T sy, T sz);
        IYoonVector Move(T dx, T dy, T dz);
        IYoonVector Move(IYoonVector v);
        IYoonVector RotateX(double angle);
        IYoonVector RotateY(double angle);
        IYoonVector RotateZ(double angle);
    }

    public interface IYoonJoint
    {
        IYoonJoint Clone();
        void CopyFrom(IYoonJoint j);
    }

    public interface IYoonJoint<T> : IYoonJoint where T : IComparable, IComparable<T>
    {
        T J1 { get; set; }
        T J2 { get; set; }
        T J3 { get; set; }
        T J4 { get; set; }
        T J5 { get; set; }
        T J6 { get; set; }
        T[] ToArray { get; }

        void Zero();
    }

    public interface IYoonCart
    {
        IYoonCart Clone();
        void CopyFrom(IYoonCart c);
    }

    public interface IYoonCart<T> : IYoonCart where T : IComparable, IComparable<T>
    {
        T X { get; set; }
        T Y { get; set; }
        T Z { get; set; }
        T RX { get; set; }
        T RY { get; set; }
        T RZ { get; set; }
        T[] Array { get; set; }

        void Zero();
        void Unit();
    }

    public interface IYoonLine
    {
        IYoonLine Clone();
        void CopyFrom(IYoonLine pLine);
        bool Equals(IYoonLine pLine);
    }

    public interface IYoonLine2D<T> : IYoonLine where T : IComparable, IComparable<T>
    {
        IYoonVector2D<T> StartPos { get; set; }
        IYoonVector2D<T> EndPos { get; set; }
        IYoonVector2D<T> CenterPos { get; }

        T Length();
    }

    public interface IYoonRect
    {
        IYoonRect Clone();
        void CopyFrom(IYoonRect pRect);
        bool Equals(IYoonRect pRect);
    }

    public interface IYoonRect2D<T> : IYoonRect where T : IComparable, IComparable<T>
    {
        IYoonVector2D<T> CenterPos { get; set; }

        T Width { get; set; }
        T Height { get; set; }
        T Left { get; }
        T Top { get; }
        T Right { get; }
        T Bottom { get; }
        IYoonVector TopLeft { get; }
        IYoonVector TopRight { get; }
        IYoonVector BottomLeft { get; }
        IYoonVector BottomRight { get; }

        T Area();
    }

    public interface IYoonTriangle
    {
        IYoonTriangle Clone();
        void CopyFrom(IYoonTriangle t);
    }

    public interface IYoonTriangle2D<T> : IYoonTriangle where T : IComparable, IComparable<T>
    {
        T X { get; set; }
        T Y { get; set; }
        T Height { get; set; }
        T Area();
    }

    public interface IYoonMapping : IDisposable
    {
        void CopyFrom(IYoonMapping pMapping);
        IYoonMapping Clone();

        int Width { get; }
        int Height { get; }
        IYoonVector Offset { get; }
        List<IYoonVector> RealPoints { get; set; }
        List<IYoonVector> PixelPoints { get; set; }

        void SetReferencePosition(IYoonVector vecPixelPos, IYoonVector vecRealPos);
        IYoonVector GetPixelResolution(IYoonVector vecPixelPos);   // mm/pixel

        IYoonVector ToPixel(IYoonVector vecRealPos);
        IYoonVector ToReal(IYoonVector vecPixelPos);
    }

    public interface IYoonSection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        IEqualityComparer<TKey> Comparer { get; }
        TValue this[int nIndex] { get; set; }

        TKey KeyOf(int nIndex);
        int IndexOf(TKey pKey);
        int IndexOf(TKey pKey, int nStartIndex);
        int IndexOf(TKey pKey, int nStartIndex, int nCount);
        int LastIndexOf(TKey pKey);
        int LastIndexOf(TKey pKey, int nStartIndex);
        int LastIndexOf(TKey pKey, int nStartIndex, int nCount);
        void Insert(int nIndex, TKey pKey, TValue pValue);
        void InsertRange(int nIndex, IEnumerable<KeyValuePair<TKey, TValue>> pCollection);
        void RemoveAt(int nIndex);
        void RemoveRange(int nIndex, int nCount);
        void Reverse();
        void Reverse(int nIndex, int nCount);
        ICollection<TValue> GetOrderedValues();
    }
}
