using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YoonFactory.Image;

namespace YoonFactory
{
    public class YoonObject<T> : IYoonObject, IEquatable<YoonObject<T>> where T : IYoonFigure
    {
        #region Supported IDisposable Pattern
        ~YoonObject()
        {
            this.Dispose(false);
        }

        private bool _disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing)
            {
                ObjectImage.Dispose();
            }
            this._disposed = true;
        }
        #endregion

        private const int DEFAULT_LABEL = 0;
        private const int DEFAULT_PIX_COUNT = 0;
        private const double DEFAULT_SCORE = 0.0;

        public int Label { get; set; }
        public double Score { get; set; }
        public T Object { get; set; }
        public IYoonVector ReferencePosition { get; set; }
        public YoonImage ObjectImage { get; set; }
        public int PixelCount { get; set; }

        public YoonObject()
        {
            Label = DEFAULT_LABEL;
            Score = DEFAULT_SCORE;
            PixelCount = DEFAULT_PIX_COUNT;
            switch (Object)
            {
                case YoonRect2N pRect2N:
                    pRect2N = new YoonRect2N();
                    ReferencePosition = new YoonVector2N();
                    break;
                case YoonRect2D pRect2D:
                    pRect2D = new YoonRect2D();
                    ReferencePosition = new YoonVector2D();
                    break;
                case YoonRectAffine2D pRectAffine2D:
                    pRectAffine2D = new YoonRectAffine2D();
                    ReferencePosition = new YoonVector2D();
                    break;
                case YoonLine2N pLine2N:
                    pLine2N = new YoonLine2N();
                    ReferencePosition = new YoonVector2N();
                    break;
                case YoonLine2D pLine2D:
                    pLine2D = new YoonLine2D();
                    ReferencePosition = new YoonVector2D();
                    break;
                case YoonVector2N pVector2N:
                    pVector2N = new YoonVector2N();
                    ReferencePosition = new YoonVector2N();
                    break;
                case YoonVector2D pVector2D:
                    pVector2D = new YoonVector2D();
                    ReferencePosition = new YoonVector2D();
                    break;
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
            ObjectImage = new YoonImage();
        }

        public YoonObject(int nLabel, T pObject, YoonImage pObjectImage)
        {
            Label = nLabel;
            Score = DEFAULT_SCORE;
            PixelCount = DEFAULT_PIX_COUNT;
            switch (pObject)
            {
                case YoonRect2N pRect2N:
                    Object = (T)pRect2N.Clone();
                    ReferencePosition = pRect2N.CenterPos.Clone();
                    break;
                case YoonRect2D pRect2D:
                    Object = (T)pRect2D.Clone();
                    ReferencePosition = pRect2D.CenterPos.Clone();
                    break;
                case YoonRectAffine2D pRectAffine2D:
                    Object = (T)pRectAffine2D.Clone();
                    ReferencePosition = pRectAffine2D.CenterPos.Clone();
                    break;
                case YoonLine2N pLine2N:
                    Object = (T)pLine2N.Clone();
                    ReferencePosition = pLine2N.CenterPos.Clone();
                    break;
                case YoonLine2D pLine2D:
                    Object = (T)pLine2D.Clone();
                    ReferencePosition = pLine2D.CenterPos.Clone();
                    break;
                case YoonVector2N pVector2N:
                    Object = (T)pVector2N.Clone();
                    ReferencePosition = pVector2N.Clone();
                    break;
                case YoonVector2D pVector2D:
                    Object = (T)pVector2D.Clone();
                    ReferencePosition = pVector2D.Clone();
                    break;
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
            ObjectImage = pObjectImage.Clone() as YoonImage;
        }

        public YoonObject(int nLabel, T pObject, IYoonVector pPosReference, YoonImage pObjectImage)
        {
            Label = nLabel;
            Score = DEFAULT_SCORE;
            PixelCount = DEFAULT_PIX_COUNT;
            switch (pObject)
            {
                case IYoonRect pRect:
                    Object = (T)pRect.Clone();
                    break;
                case IYoonLine pLine:
                    Object = (T)pLine.Clone();
                    break;
                case IYoonVector pVector:
                    Object = (T)pVector.Clone();
                    break;
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
            ReferencePosition = pPosReference.Clone();
            ObjectImage = pObjectImage.Clone() as YoonImage;
        }

        public YoonObject(int nLabel, T pObject, YoonImage pObjectImage, int nCount)
        {
            Label = nLabel;
            Score = DEFAULT_SCORE;
            PixelCount = nCount;
            switch (pObject)
            {
                case YoonRect2N pRect2N:
                    Object = (T)pRect2N.Clone();
                    ReferencePosition = pRect2N.CenterPos.Clone();
                    break;
                case YoonRect2D pRect2D:
                    Object = (T)pRect2D.Clone();
                    ReferencePosition = pRect2D.CenterPos.Clone();
                    break;
                case YoonRectAffine2D pRectAffine2D:
                    Object = (T)pRectAffine2D.Clone();
                    ReferencePosition = pRectAffine2D.CenterPos.Clone();
                    break;
                case YoonLine2N pLine2N:
                    Object = (T)pLine2N.Clone();
                    ReferencePosition = pLine2N.CenterPos.Clone();
                    break;
                case YoonLine2D pLine2D:
                    Object = (T)pLine2D.Clone();
                    ReferencePosition = pLine2D.CenterPos.Clone();
                    break;
                case YoonVector2N pVector2N:
                    Object = (T)pVector2N.Clone();
                    ReferencePosition = pVector2N.Clone();
                    break;
                case YoonVector2D pVector2D:
                    Object = (T)pVector2D.Clone();
                    ReferencePosition = pVector2D.Clone();
                    break;
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
            ObjectImage = pObjectImage.Clone() as YoonImage;
        }

        public YoonObject(int nLabel, T pObject, IYoonVector pPosReference, YoonImage pObjectImage, int nCount)
        {
            Label = nLabel;
            Score = DEFAULT_SCORE;
            PixelCount = nCount;
            switch (pObject)
            {
                case IYoonRect pRect:
                    Object = (T)pRect.Clone();
                    break;
                case IYoonLine pLine:
                    Object = (T)pLine.Clone();
                    break;
                case IYoonVector pVector:
                    Object = (T)pVector.Clone();
                    break;
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
            ReferencePosition = pPosReference.Clone();
            ObjectImage = pObjectImage.Clone() as YoonImage;
        }

        public YoonObject(int nLabel, T pObject, YoonImage pObjectImage, double dScore, int nCount)
        {
            Label = nLabel;
            Score = dScore;
            PixelCount = nCount;
            switch (pObject)
            {
                case YoonRect2N pRect2N:
                    Object = (T)pRect2N.Clone();
                    ReferencePosition = pRect2N.CenterPos.Clone();
                    break;
                case YoonRect2D pRect2D:
                    Object = (T)pRect2D.Clone();
                    ReferencePosition = pRect2D.CenterPos.Clone();
                    break;
                case YoonRectAffine2D pRectAffine2D:
                    Object = (T)pRectAffine2D.Clone();
                    ReferencePosition = pRectAffine2D.CenterPos.Clone();
                    break;
                case YoonLine2N pLine2N:
                    Object = (T)pLine2N.Clone();
                    ReferencePosition = pLine2N.CenterPos.Clone();
                    break;
                case YoonLine2D pLine2D:
                    Object = (T)pLine2D.Clone();
                    ReferencePosition = pLine2D.CenterPos.Clone();
                    break;
                case YoonVector2N pVector2N:
                    Object = (T)pVector2N.Clone();
                    ReferencePosition = pVector2N.Clone();
                    break;
                case YoonVector2D pVector2D:
                    Object = (T)pVector2D.Clone();
                    ReferencePosition = pVector2D.Clone();
                    break;
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
            ObjectImage = pObjectImage.Clone() as YoonImage;
        }

        public YoonObject(int nLabel, T pObject, IYoonVector pPosReference, YoonImage pObjectImage, double dScore, int nCount)
        {
            Label = nLabel;
            Score = dScore;
            PixelCount = nCount;
            switch (pObject)
            {
                case IYoonRect pRect:
                    Object = (T)pRect.Clone();
                    break;
                case IYoonLine pLine:
                    Object = (T)pLine.Clone();
                    break;
                case IYoonVector pVector:
                    Object = (T)pVector.Clone();
                    break;
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
            ReferencePosition = pPosReference.Clone();
            ObjectImage = pObjectImage.Clone() as YoonImage;
        }

        public void CopyFrom(IYoonObject pObject)
        {
            if (pObject is YoonObject<T> pYoonObject)
            {
                Label = pYoonObject.Label;
                Score = pYoonObject.Score;
                PixelCount = pYoonObject.PixelCount;
                ObjectImage = pYoonObject.ObjectImage;
                switch (pYoonObject.Object)
                {
                    case IYoonRect pRect:
                        Object = (T)pRect.Clone();
                        break;
                    case IYoonLine pLine:
                        Object = (T)pLine.Clone();
                        break;
                    case IYoonVector pVector:
                        Object = (T)pVector.Clone();
                        break;
                    default:
                        throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
                }
                ReferencePosition = pYoonObject.ReferencePosition.Clone();
            }
        }

        public IYoonObject Clone()
        {
            switch (Object)
            {
                case IYoonRect pRect:
                    return new YoonObject<T>(Label, (T)pRect.Clone(), ReferencePosition, (YoonImage)ObjectImage.Clone(), Score, PixelCount);
                case IYoonLine pLine:
                    return new YoonObject<T>(Label, (T)pLine.Clone(), ReferencePosition, (YoonImage)ObjectImage.Clone(), Score, PixelCount);
                case IYoonVector pVector:
                    return new YoonObject<T>(Label, (T)pVector.Clone(), ReferencePosition, (YoonImage)ObjectImage.Clone(), Score, PixelCount);
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
        }


        public bool Equals(IYoonObject pObject)
        {
            if (pObject is YoonObject<T> pYoonObject)
            {
                switch (pYoonObject.Object)
                {
                    case IYoonRect pRect:
                        if (pYoonObject.Label == Label &&
                            pYoonObject.Score == Score &&
                            pRect.Equals(Object) &&
                            pYoonObject.ReferencePosition.Equals(ReferencePosition) &&
                            pYoonObject.ObjectImage == ObjectImage &&
                            pYoonObject.PixelCount == PixelCount)
                            return true;
                        break;
                    case IYoonLine pLine:
                        if (pYoonObject.Label == Label &&
                            pYoonObject.Score == Score &&
                            pLine.Equals(Object) &&
                            pYoonObject.ReferencePosition.Equals(ReferencePosition) &&
                            pYoonObject.ObjectImage == ObjectImage &&
                            pYoonObject.PixelCount == PixelCount)
                            return true;
                        break;
                    case IYoonVector pVector:
                        if (pYoonObject.Label == Label &&
                            pYoonObject.Score == Score &&
                            pVector.Equals(Object) &&
                            pYoonObject.ReferencePosition.Equals(ReferencePosition) &&
                            pYoonObject.ObjectImage == ObjectImage &&
                            pYoonObject.PixelCount == PixelCount)
                            return true;
                        break;
                    default:
                        throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 258276020;
            hashCode = hashCode * -1521134295 + _disposed.GetHashCode();
            hashCode = hashCode * -1521134295 + Label.GetHashCode();
            hashCode = hashCode * -1521134295 + Score.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Object);
            hashCode = hashCode * -1521134295 + EqualityComparer<IYoonVector>.Default.GetHashCode(ReferencePosition);
            hashCode = hashCode * -1521134295 + EqualityComparer<YoonImage>.Default.GetHashCode(ObjectImage);
            hashCode = hashCode * -1521134295 + PixelCount.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is YoonObject<T> @object &&
                   _disposed == @object._disposed &&
                   Label == @object.Label &&
                   Score == @object.Score &&
                   EqualityComparer<T>.Default.Equals(Object, @object.Object) &&
                   EqualityComparer<IYoonVector>.Default.Equals(ReferencePosition, @object.ReferencePosition) &&
                   EqualityComparer<YoonImage>.Default.Equals(ObjectImage, @object.ObjectImage) &&
                   PixelCount == @object.PixelCount;
        }

        public bool Equals(YoonObject<T> other)
        {
            return Equals(other);
        }

        public static bool operator ==(YoonObject<T> pObjectSource, YoonObject<T> pObjectOther)
        {
            return pObjectSource?.Equals(pObjectOther) == true;
        }

        public static bool operator !=(YoonObject<T> pObjectSource, YoonObject<T> pObjectOther)
        {
            return !(pObjectSource == pObjectOther);
        }
    }
}
