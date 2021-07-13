using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Image
{
    public class YoonDataset : IDisposable, IList<YoonObject>
    {
        #region IDisposable Support
        ~YoonDataset()
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
                Clear();

            _pListLabel = null;
            _pListScore = null;
            _pListFeature = null;
            _pListReference = null;
            _pListImage = null;
            _pListPixels = null;
            _disposed = true;
        }
        #endregion

        protected List<int> _pListLabel = new List<int>();
        protected List<double> _pListScore = new List<double>();
        protected List<IYoonFigure> _pListFeature = new List<IYoonFigure>();
        protected List<IYoonVector> _pListReference = new List<IYoonVector>();
        protected List<YoonImage> _pListImage = new List<YoonImage>();
        protected List<int> _pListPixels = new List<int>();

        public YoonObject this[int index]
        {
            get
            {
                if (_pListLabel == null || _pListScore == null || _pListFeature == null || _pListReference == null ||
                    _pListImage == null || _pListPixels == null)
                    throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
                YoonObject pObject = new YoonObject();
                if (_pListLabel.Count > 0 && index >= 0 && index < _pListLabel.Count)
                    pObject.Label = _pListLabel[index];
                if (_pListScore.Count > 0 && index >= 0 && index < _pListScore.Count)
                    pObject.Score = _pListScore[index];
                if (_pListFeature.Count > 0 && index >= 0 && index < _pListFeature.Count)
                    pObject.Feature = _pListFeature[index];
                if (_pListReference.Count > 0 && index >= 0 && index < _pListReference.Count)
                    pObject.ReferencePosition = _pListReference[index];
                if (_pListImage.Count > 0 && index >= 0 && index < _pListImage.Count)
                    pObject.ObjectImage = (YoonImage) _pListImage[index];
                if (_pListPixels.Count > 0 && index >= 0 && index < _pListPixels.Count)
                    pObject.PixelCount = _pListPixels[index];
                return pObject;
            }
            set
            {
                if (_pListLabel == null || _pListScore == null || _pListFeature == null || _pListReference == null ||
                    _pListImage == null || _pListPixels == null)
                    throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
                if (_pListLabel.Count > 0 && index >= 0 && index < _pListLabel.Count)
                    _pListLabel[index] = value.Label;
                if (_pListScore.Count > 0 && index >= 0 && index < _pListScore.Count)
                    _pListScore[index] = value.Score;
                if (_pListFeature.Count > 0 && index >= 0 && index < _pListFeature.Count)
                    _pListFeature[index] = value.Feature.Clone();
                if (_pListReference.Count > 0 && index >= 0 && index < _pListReference.Count)
                    _pListReference[index] = value.ReferencePosition.Clone();
                if (_pListImage.Count > 0 && index >= 0 && index < _pListImage.Count)
                    _pListImage[index] = (YoonImage) value.ObjectImage.Clone();
                if (_pListPixels.Count > 0 && index >= 0 && index < _pListPixels.Count)
                    _pListPixels[index] = value.PixelCount;
            }
        }

        public int Count => _pListFeature.Count;

        public bool IsReadOnly => ((ICollection<YoonObject>)_pListFeature).IsReadOnly;

        public void Add(YoonObject item)
        {
            if (_pListLabel == null || _pListScore == null || _pListFeature == null || _pListReference == null ||
                _pListImage == null || _pListPixels == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            _pListLabel.Add(item.Label);
            _pListImage.Add(item.ObjectImage);
            _pListFeature.Add(item.Feature);
            _pListPixels.Add(item.PixelCount);
            _pListReference.Add(item.ReferencePosition);
            _pListScore.Add(item.Score);
        }

        public void Clear()
        {
            _pListLabel?.Clear();
            _pListScore?.Clear();
            _pListFeature?.Clear();
            _pListReference?.Clear();
            _pListImage?.Clear();
            _pListPixels?.Clear();
        }

        public bool Contains(YoonObject item)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return _pListFeature.Contains(item.Feature) &&
                   _pListImage.Contains(item.ObjectImage) &&
                   _pListScore.Contains(item.Score) &&
                   _pListReference.Contains(item.ReferencePosition) &&
                   _pListLabel.Contains(item.Label) &&
                   _pListPixels.Contains(item.PixelCount);
        }

        public void CopyTo(YoonObject<T>[] array, int arrayIndex)
        {
            if (_pListFeature == null || array == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            _pListFeature.CopyTo(array, arrayIndex);
        }

        public YoonDataset<T> Clone()
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            YoonDataset<T> pList = new YoonDataset<T>();
            pList._pListFeature = new List<YoonObject<T>>(_pListFeature);
            return pList;
        }

        public void CopyFrom(YoonDataset<T> pList)
        {
            _pListFeature = pList._pListFeature;
        }

        public IEnumerator<YoonObject<T>> GetEnumerator()
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return _pListFeature.GetEnumerator();
        }

        public int IndexOf(YoonObject<T> item)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return _pListFeature.IndexOf(item);
        }

        public YoonObject<T> Search(int nLabel)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            YoonObject<T> pYoonObject = null;
            foreach (YoonObject<T> pValue in _pListFeature)
            {
                if (pValue.Label == nLabel)
                {
                    pYoonObject = pValue.Clone() as YoonObject<T>;
                    break;
                }
            }
            if (pYoonObject == null)
                throw new NullReferenceException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            else
                return pYoonObject;
        }

        public void Swap(int index1, int index2)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (index1 < 0 || index1 >= _pListFeature.Count ||
                index2 < 0 || index2 >= _pListFeature.Count)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            YoonObject<T> pObject1 = _pListFeature[index1].Clone() as YoonObject<T>;
            YoonObject<T> pObject2 = _pListFeature[index2].Clone() as YoonObject<T>;
            _pListFeature[index2] = pObject1;
            _pListFeature[index1] = pObject2;
        }

        public void Insert(int index, YoonObject<T> item)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (index < 0 || index >= _pListFeature.Count)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            _pListFeature.Insert(index, item);
        }

        public bool Remove(YoonObject<T> item)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return _pListFeature.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (index < 0 || index >= _pListFeature.Count)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _pListFeature.GetEnumerator();
        }

        public void SortLabels(eYoonDir2DMode nMode)
        {
            if (_pListFeature == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            int iSearch;
            switch (nMode)
            {
                case eYoonDir2DMode.Increase:
                    int minLabel;
                    for (int i = 0; i < _pListFeature.Count - 1; i++)
                    {
                        iSearch = i;
                        for (int j = i + 1; j < _pListFeature.Count; j++)
                        {
                            minLabel = _pListFeature[iSearch].Label;
                            if (_pListFeature[j].Label < minLabel)
                                iSearch = j;
                        }
                        if (iSearch == i) continue;
                        Swap(i, iSearch);
                    }
                    break;
                case eYoonDir2DMode.Decrease:
                    int maxLabel;
                    for (int i = 0; i < _pListFeature.Count - 1; i++)
                    {
                        iSearch = i;
                        for (int j = i + 1; j < _pListFeature.Count; j++)
                        {
                            maxLabel = _pListFeature[iSearch].Label;
                            if (_pListFeature[j].Label > maxLabel)
                                iSearch = j;
                        }
                        if (iSearch == i) continue;
                        Swap(i, iSearch);
                    }
                    break;
                default:
                    throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Sorting mode is not correct");
            }
        }

        public void SortObjects(eYoonDir2D nDir)
        {
            if (_pListFeature == null || _pListFeature.Count == 0)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (_pListFeature[0].Object is YoonRect2N)
                SortRect2N(nDir);
            if (_pListFeature[0].Object is YoonRect2D)
                SortRect2D(nDir);
            if (_pListFeature[0].Object is YoonRectAffine2D)
                SortRectAffine2D(nDir);
            else
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] This type is not supported");
        }

        private void SortRect2N(eYoonDir2D nDir)
        {
            YoonRect2N pRectMin, pRectCurr;
            int iSearch = 0;
            int nDiff = 0;
            int nHeight = 0;
            for (int i = 0; i < _pListFeature.Count - 1; i++)
            {
                iSearch = i;
                for (int j = i + 1; j < _pListFeature.Count; j++)
                {
                    pRectMin = _pListFeature[iSearch].Object as YoonRect2N;
                    pRectCurr = _pListFeature[j] as YoonRect2N;
                    switch (nDir)
                    {
                        case eYoonDir2D.TopLeft:
                            nDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            nHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 왼쪽 우선.
                            if (nDiff <= nHeight / 2)
                            {
                                if (pRectCurr.Left < pRectMin.Left)
                                    iSearch = j;
                            }
                            else
                            {
                                if (pRectCurr.Top < pRectMin.Top)
                                    iSearch = j;
                            }
                            break;
                        case eYoonDir2D.TopRight:
                            nDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            nHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 오른쪽 우선.
                            if (nDiff <= nHeight / 2)
                            {
                                if (pRectCurr.Right > pRectMin.Right)
                                    iSearch = j;
                            }
                            else
                            {
                                if (pRectCurr.Top < pRectMin.Top)
                                    iSearch = j;
                            }
                            break;
                        case eYoonDir2D.Left:
                            if (pRectCurr.Left < pRectMin.Left)
                                iSearch = j;
                            break;
                        case eYoonDir2D.Right:
                            if (pRectCurr.Right > pRectMin.Right)
                                iSearch = j;
                            break;
                        default:  // 좌상측 정렬과 같음.
                            nDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            nHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 왼쪽 우선.
                            if (nDiff <= nHeight / 2)
                                continue;
                            if (pRectCurr.Left < pRectMin.Left)
                                iSearch = j;
                            break;
                    }
                }
                if (iSearch == i) continue;
                Swap(i, iSearch);
            }
        }

        private void SortRect2D(eYoonDir2D nDir)
        {
            YoonRect2D pRectMin, pRectCurr;
            int iSearch = 0;
            double dDiff = 0;
            double dHeight = 0;
            for (int i = 0; i < _pListFeature.Count - 1; i++)
            {
                iSearch = i;
                for (int j = i + 1; j < _pListFeature.Count; j++)
                {
                    pRectMin = _pListFeature[iSearch].Object as YoonRect2D;
                    pRectCurr = _pListFeature[j] as YoonRect2D;
                    switch (nDir)
                    {
                        case eYoonDir2D.TopLeft:
                            dDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            dHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 왼쪽 우선.
                            if (dDiff <= dHeight / 2)
                            {
                                if (pRectCurr.Left < pRectMin.Left)
                                    iSearch = j;
                            }
                            else
                            {
                                if (pRectCurr.Top < pRectMin.Top)
                                    iSearch = j;
                            }
                            break;
                        case eYoonDir2D.TopRight:
                            dDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            dHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 오른쪽 우선.
                            if (dDiff <= dHeight / 2)
                            {
                                if (pRectCurr.Right > pRectMin.Right)
                                    iSearch = j;
                            }
                            else
                            {
                                if (pRectCurr.Top < pRectMin.Top)
                                    iSearch = j;
                            }
                            break;
                        case eYoonDir2D.Left:
                            if (pRectCurr.Left < pRectMin.Left)
                                iSearch = j;
                            break;
                        case eYoonDir2D.Right:
                            if (pRectCurr.Right > pRectMin.Right)
                                iSearch = j;
                            break;
                        default:  // 좌상측 정렬과 같음.
                            dDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            dHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 왼쪽 우선.
                            if (dDiff <= dHeight / 2)
                                continue;
                            if (pRectCurr.Left < pRectMin.Left)
                                iSearch = j;
                            break;
                    }
                }
                if (iSearch == i) continue;
                Swap(i, iSearch);
            }
        }

        private void SortRectAffine2D(eYoonDir2D nDir)
        {
            YoonRectAffine2D pRectMin, pRectCurr;
            int iSearch = 0;
            double dDiff = 0;
            double dHeight = 0;
            for (int i = 0; i < _pListFeature.Count - 1; i++)
            {
                iSearch = i;
                for (int j = i + 1; j < _pListFeature.Count; j++)
                {
                    pRectMin = _pListFeature[iSearch].Object as YoonRectAffine2D;
                    pRectCurr = _pListFeature[j] as YoonRectAffine2D;
                    switch (nDir)
                    {
                        case eYoonDir2D.TopLeft:
                            dDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            dHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 왼쪽 우선.
                            if (dDiff <= dHeight / 2)
                            {
                                if (pRectCurr.Left < pRectMin.Left)
                                    iSearch = j;
                            }
                            else
                            {
                                if (pRectCurr.Top < pRectMin.Top)
                                    iSearch = j;
                            }
                            break;
                        case eYoonDir2D.TopRight:
                            dDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            dHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 오른쪽 우선.
                            if (dDiff <= dHeight / 2)
                            {
                                if (pRectCurr.Right > pRectMin.Right)
                                    iSearch = j;
                            }
                            else
                            {
                                if (pRectCurr.Top < pRectMin.Top)
                                    iSearch = j;
                            }
                            break;
                        case eYoonDir2D.Left:
                            if (pRectCurr.Left < pRectMin.Left)
                                iSearch = j;
                            break;
                        case eYoonDir2D.Right:
                            if (pRectCurr.Right > pRectMin.Right)
                                iSearch = j;
                            break;
                        default:  // 좌상측 정렬과 같음.
                            dDiff = Math.Abs(pRectMin.Top - pRectCurr.Top);
                            dHeight = pRectMin.Bottom - pRectMin.Top;
                            //////  높이차가 있는 경우 Top 우선, 없는 경우 왼쪽 우선.
                            if (dDiff <= dHeight / 2)
                                continue;
                            if (pRectCurr.Left < pRectMin.Left)
                                iSearch = j;
                            break;
                    }
                }
                if (iSearch == i) continue;
                Swap(i, iSearch);
            }
        }

        public void CombineObjects()
        {
            if (_pListFeature == null || _pListFeature.Count == 0)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (_pListFeature[0].Object is YoonRect2N)
                CombineRect2N();
            if (_pListFeature[0].Object is YoonRect2D)
                CombineRect2D();
            if (_pListFeature[0].Object is YoonRectAffine2D)
                CombineRectAffine2D();
            else
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] This type is not supported");
        }

        private void CombineRect2N()
        {
            YoonRect2N pRect1, pRect2;
            YoonRect2N combineRect;
            bool isCombine = false;
            pRect1 = new YoonRect2N();
            ////  원본(rect1)을 복사(rect2)해서 List에 넣은 後 삭제한다.
            List<YoonObject<YoonRect2N>> pListTemp = new List<YoonObject<YoonRect2N>>();
            foreach (YoonObject<T> pObject in _pListFeature)
            {
                pListTemp.Add(pObject.Clone() as YoonObject<YoonRect2N>);
            }
            _pListFeature.Clear();
            ////  모든 사각형들을 전수 조사해가며 서로 겹치는 사각형이 있는지 찾는다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                pRect1 = pListTemp[i].Feature;
                combineRect = new YoonRect2N(0, 0, 0, 0);
                if (pRect1.Width == 0)
                    continue;
                isCombine = false;
                for (int j = 0; j < pListTemp.Count; j++)
                {
                    if (i == j) continue;
                    pRect2 = pListTemp[j].Feature;
                    if (pRect2.Width == 0)
                        continue;
                    //////  Rect1와 Rect2가 겹치거나 속해지는 경우...
                    if ((pRect1.Left > pRect2.Left) && (pRect1.Left < pRect2.Right))
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    if (pRect1.Right > pRect2.Left && pRect1.Right < pRect2.Right)
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    if ((pRect1.Left <= pRect2.Left) && (pRect1.Right >= pRect2.Right))
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    //////  Rect들이 겹쳐지는 경우, 결합 Rect는 둘을 모두 포함한다.
                    if (isCombine)
                    {
                        combineRect.CenterPos.X = (pRect1.Left < pRect2.Left) ? pRect1.CenterPos.X : pRect2.CenterPos.X;
                        combineRect.Width = (pRect1.Right > pRect2.Right) ? pRect1.Right - combineRect.Left : pRect2.Right - combineRect.Left;
                        combineRect.CenterPos.Y = (pRect1.Top < pRect2.Top) ? pRect1.CenterPos.Y : pRect2.CenterPos.Y;
                        combineRect.Height = (pRect1.Bottom > pRect2.Bottom) ? pRect1.Bottom - combineRect.Top : pRect2.Bottom - combineRect.Top;
                        pListTemp[i].Feature = new YoonRect2N(0, 0, 0, 0);
                        pListTemp[j].Feature = combineRect;
                        break;
                    }
                }
            }
            ////  정렬된 사각형들 中 유효한 사각형들만 재정렬시킨다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                T pObject = (T)pListTemp[i].Feature.Clone();
                if (pObject is YoonRect2N pRect)
                {
                    if (pRect.Right != 0)
                    {
                        _pListFeature.Add(new YoonObject<T>(pListTemp[i].Label, pObject, (YoonImage)pListTemp[i].ObjectImage.Clone(), pListTemp[i].Score, pListTemp[i].PixelCount));
                    }
                }
                else
                    break;
            }
            pListTemp.Clear();
        }

        private void CombineRect2D()
        {
            YoonRect2D pRect1, pRect2;
            YoonRect2D combineRect;
            bool isCombine = false;
            pRect1 = new YoonRect2D();
            ////  원본(rect1)을 복사(rect2)해서 List에 넣은 後 삭제한다.
            List<YoonObject<YoonRect2D>> pListTemp = new List<YoonObject<YoonRect2D>>();
            foreach (YoonObject<T> pObject in _pListFeature)
            {
                pListTemp.Add(pObject.Clone() as YoonObject<YoonRect2D>);
            }
            _pListFeature.Clear();
            ////  모든 사각형들을 전수 조사해가며 서로 겹치는 사각형이 있는지 찾는다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                pRect1 = pListTemp[i].Feature;
                combineRect = new YoonRect2D(0, 0, 0, 0);
                if (pRect1.Width == 0)
                    continue;
                isCombine = false;
                for (int j = 0; j < pListTemp.Count; j++)
                {
                    if (i == j) continue;
                    pRect2 = pListTemp[j].Feature;
                    if (pRect2.Width == 0)
                        continue;
                    //////  Rect1와 Rect2가 겹치거나 속해지는 경우...
                    if ((pRect1.Left > pRect2.Left) && (pRect1.Left < pRect2.Right))
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    if (pRect1.Right > pRect2.Left && pRect1.Right < pRect2.Right)
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    if ((pRect1.Left <= pRect2.Left) && (pRect1.Right >= pRect2.Right))
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    //////  Rect들이 겹쳐지는 경우, 결합 Rect는 둘을 모두 포함한다.
                    if (isCombine)
                    {
                        combineRect.CenterPos.X = (pRect1.Left < pRect2.Left) ? pRect1.CenterPos.X : pRect2.CenterPos.X;
                        combineRect.Width = (pRect1.Right > pRect2.Right) ? pRect1.Right - combineRect.Left : pRect2.Right - combineRect.Left;
                        combineRect.CenterPos.Y = (pRect1.Top < pRect2.Top) ? pRect1.CenterPos.Y : pRect2.CenterPos.Y;
                        combineRect.Height = (pRect1.Bottom > pRect2.Bottom) ? pRect1.Bottom - combineRect.Top : pRect2.Bottom - combineRect.Top;
                        pListTemp[i].Feature = new YoonRect2D(0, 0, 0, 0);
                        pListTemp[j].Feature = combineRect;
                        break;
                    }
                }
            }
            ////  정렬된 사각형들 中 유효한 사각형들만 재정렬시킨다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                T pObject = (T)pListTemp[i].Feature.Clone();
                if (pObject is YoonRect2D pRect)
                {
                    if (pRect.Right != 0)
                    {
                        _pListFeature.Add(new YoonObject<T>(pListTemp[i].Label, pObject, (YoonImage)pListTemp[i].ObjectImage.Clone(), pListTemp[i].Score, pListTemp[i].PixelCount));
                    }
                }
                else
                    break;
            }
            pListTemp.Clear();
        }

        private void CombineRectAffine2D()
        {
            YoonRectAffine2D pRect1, pRect2;
            YoonRectAffine2D combineRect;
            bool isCombine = false;
            ////  원본(rect1)을 복사(rect2)해서 List에 넣은 後 삭제한다.
            List<YoonObject<YoonRectAffine2D>> pListTemp = new List<YoonObject<YoonRectAffine2D>>();
            foreach (YoonObject<T> pObject in _pListFeature)
            {
                pListTemp.Add(pObject.Clone() as YoonObject<YoonRectAffine2D>);
            }
            _pListFeature.Clear();
            ////  모든 사각형들을 전수 조사해가며 서로 겹치는 사각형이 있는지 찾는다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                pRect1 = pListTemp[i].Feature;
                combineRect = new YoonRectAffine2D(0, 0, 0);
                if (pRect1.Width == 0)
                    continue;
                isCombine = false;
                for (int j = 0; j < pListTemp.Count; j++)
                {
                    if (i == j) continue;
                    pRect2 = pListTemp[j].Feature;
                    if (pRect2.Width == 0)
                        continue;
                    //////  Rect1와 Rect2가 겹치거나 속해지는 경우...
                    if ((pRect1.Left > pRect2.Left) && (pRect1.Left < pRect2.Right))
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    if (pRect1.Right > pRect2.Left && pRect1.Right < pRect2.Right)
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    if ((pRect1.Left <= pRect2.Left) && (pRect1.Right >= pRect2.Right))
                    {
                        if ((pRect1.Top >= pRect2.Top) && (pRect1.Top <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Bottom >= pRect2.Top) && (pRect1.Bottom <= pRect2.Bottom))
                            isCombine = true;
                        if ((pRect1.Top <= pRect2.Top) && (pRect1.Bottom >= pRect2.Bottom))
                            isCombine = true;
                    }
                    //////  Rect들이 겹쳐지는 경우, 결합 Rect는 둘을 모두 포함한다.
                    if (isCombine)
                    {
                        combineRect.CenterPos.X = (pRect1.Left < pRect2.Left) ? pRect1.CenterPos.X : pRect2.CenterPos.X;
                        combineRect.Width = (pRect1.Right > pRect2.Right) ? pRect1.Right - combineRect.Left : pRect2.Right - combineRect.Left;
                        combineRect.CenterPos.Y = (pRect1.Top < pRect2.Top) ? pRect1.CenterPos.Y : pRect2.CenterPos.Y;
                        combineRect.Height = (pRect1.Bottom > pRect2.Bottom) ? pRect1.Bottom - combineRect.Top : pRect2.Bottom - combineRect.Top;
                        pListTemp[i].Feature = new YoonRectAffine2D(0, 0, 0);
                        pListTemp[j].Feature = combineRect;
                        break;
                    }
                }
            }
            ////  정렬된 사각형들 中 유효한 사각형들만 재정렬시킨다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                T pObject = (T)pListTemp[i].Feature.Clone();
                if (pObject is YoonRectAffine2D pRect)
                {
                    if (pRect.Right != 0)
                    {
                        _pListFeature.Add(new YoonObject<T>(pListTemp[i].Label, pObject, (YoonImage)pListTemp[i].ObjectImage.Clone(), pListTemp[i].Score, pListTemp[i].PixelCount));
                    }
                }
                else
                    break;
            }
            pListTemp.Clear();
        }
    }
}
