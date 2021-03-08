using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YoonFactory
{
    public class YoonObject<T> : IYoonObject where T : IYoonFigure
    {
        #region Supported IDisposable Pattern
        ~YoonObject()
        {
            this.Dispose(false);
        }

        private bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                ////  .Net Framework에 의해 관리되는 리소스를 여기서 정리합니다.
            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        private const int DEFAULT_PIX_COUNT = 0;
        private const double DEFAULT_SCORE = 0.0;

        public int Label { get; set; }
        public double Score { get; set; }
        public T Object { get; set; }
        public int PixelCount { get; set; }

        public YoonObject(int nLabel, T pObject)
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
        }

        public YoonObject(int nLabel, T pObject, int nCount)
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
        }

        public YoonObject(int nLabel, T pObject, double dScore, int nCount)
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
                            pYoonObject.PixelCount == PixelCount)
                            return true;
                        break;
                    case IYoonLine pLine:
                        if (pYoonObject.Label == Label &&
                            pYoonObject.Score == Score &&
                            pLine.Equals(Object) &&
                            pYoonObject.PixelCount == PixelCount)
                            return true;
                        break;
                    case IYoonVector pVector:
                        if (pYoonObject.Label == Label &&
                            pYoonObject.Score == Score &&
                            pVector.Equals(Object) &&
                            pYoonObject.PixelCount == PixelCount)
                            return true;
                        break;
                    default:
                        throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
                }
            }
            return false;
        }

        public void CopyFrom(IYoonObject pObject)
        {
            if (pObject is YoonObject<T> pYoonObject)
            {
                Label = pYoonObject.Label;
                Score = pYoonObject.Score;
                PixelCount = pYoonObject.PixelCount;
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
            }
        }

        public IYoonObject Clone()
        {
            switch (Object)
            {
                case IYoonRect pRect:
                    return new YoonObject<T>(Label, (T)pRect.Clone(), Score, PixelCount);
                case IYoonLine pLine:
                    return new YoonObject<T>(Label, (T)pLine.Clone(), Score, PixelCount);
                case IYoonVector pVector:
                    return new YoonObject<T>(Label, (T)pVector.Clone(), Score, PixelCount);
                default:
                    throw new FormatException("[YOONIMAGE EXCEPTION] Object format is not correct");
            }
        }
    }
}
