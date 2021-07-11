using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Image
{
    public class ObjectList<T> : IDisposable, IList<YoonObject<T>> where T : IYoonFigure
    {
        #region IDisposable Support
        ~ObjectList()
        {
            this.Dispose(false);
        }

        private bool disposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed) return;
            if (disposing)
            {
                ////  .Net Framework에 의해 관리되는 리소스를 여기서 정리합니다.
            }
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            Clear();
            m_pListObject = null;
            disposed = true;
        }
        #endregion

        protected List<YoonObject<T>> m_pListObject = new List<YoonObject<T>>();

        public YoonObject<T> this[int index]
        {
            get
            {
                if (m_pListObject == null)
                    throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
                if (index < 0 || index >= m_pListObject.Count)
                    throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
                return m_pListObject[index];
            }
            set
            {
                if (m_pListObject == null)
                    throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
                if (index < 0 || index >= m_pListObject.Count)
                    throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
                m_pListObject[index] = value;
            }
        }

        public int Count => m_pListObject.Count;

        public bool IsReadOnly => ((ICollection<YoonObject<T>>)m_pListObject).IsReadOnly;

        public void Add(YoonObject<T> item)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            m_pListObject.Add(item);
        }

        public void Clear()
        {
            if (m_pListObject != null)
                m_pListObject.Clear();
        }

        public bool Contains(YoonObject<T> item)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return m_pListObject.Contains(item);
        }

        public void CopyTo(YoonObject<T>[] array, int arrayIndex)
        {
            if (m_pListObject == null || array == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            m_pListObject.CopyTo(array, arrayIndex);
        }

        public ObjectList<T> Clone()
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            ObjectList<T> pList = new ObjectList<T>();
            pList.m_pListObject = new List<YoonObject<T>>(m_pListObject);
            return pList;
        }

        public void CopyFrom(ObjectList<T> pList)
        {
            m_pListObject = pList.m_pListObject;
        }

        public IEnumerator<YoonObject<T>> GetEnumerator()
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return m_pListObject.GetEnumerator();
        }

        public int IndexOf(YoonObject<T> item)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return m_pListObject.IndexOf(item);
        }

        public YoonObject<T> Search(int nLabel)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            YoonObject<T> pYoonObject = null;
            foreach (YoonObject<T> pValue in m_pListObject)
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
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (index1 < 0 || index1 >= m_pListObject.Count ||
                index2 < 0 || index2 >= m_pListObject.Count)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            YoonObject<T> pObject1 = m_pListObject[index1].Clone() as YoonObject<T>;
            YoonObject<T> pObject2 = m_pListObject[index2].Clone() as YoonObject<T>;
            m_pListObject[index2] = pObject1;
            m_pListObject[index1] = pObject2;
        }

        public void Insert(int index, YoonObject<T> item)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (index < 0 || index >= m_pListObject.Count)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            m_pListObject.Insert(index, item);
        }

        public bool Remove(YoonObject<T> item)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            return m_pListObject.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (index < 0 || index >= m_pListObject.Count)
                throw new IndexOutOfRangeException("[YOONIMAGE EXCEPTION] Index must be within the bounds");
            RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_pListObject.GetEnumerator();
        }

        public void SortLabels(eYoonDir2DMode nMode)
        {
            if (m_pListObject == null)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            int iSearch;
            switch (nMode)
            {
                case eYoonDir2DMode.Increase:
                    int minLabel;
                    for (int i = 0; i < m_pListObject.Count - 1; i++)
                    {
                        iSearch = i;
                        for (int j = i + 1; j < m_pListObject.Count; j++)
                        {
                            minLabel = m_pListObject[iSearch].Label;
                            if (m_pListObject[j].Label < minLabel)
                                iSearch = j;
                        }
                        if (iSearch == i) continue;
                        Swap(i, iSearch);
                    }
                    break;
                case eYoonDir2DMode.Decrease:
                    int maxLabel;
                    for (int i = 0; i < m_pListObject.Count - 1; i++)
                    {
                        iSearch = i;
                        for (int j = i + 1; j < m_pListObject.Count; j++)
                        {
                            maxLabel = m_pListObject[iSearch].Label;
                            if (m_pListObject[j].Label > maxLabel)
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
            if (m_pListObject == null || m_pListObject.Count == 0)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (m_pListObject[0].Object is YoonRect2N)
                SortRect2N(nDir);
            if (m_pListObject[0].Object is YoonRect2D)
                SortRect2D(nDir);
            if (m_pListObject[0].Object is YoonRectAffine2D)
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
            for (int i = 0; i < m_pListObject.Count - 1; i++)
            {
                iSearch = i;
                for (int j = i + 1; j < m_pListObject.Count; j++)
                {
                    pRectMin = m_pListObject[iSearch].Object as YoonRect2N;
                    pRectCurr = m_pListObject[j] as YoonRect2N;
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
            for (int i = 0; i < m_pListObject.Count - 1; i++)
            {
                iSearch = i;
                for (int j = i + 1; j < m_pListObject.Count; j++)
                {
                    pRectMin = m_pListObject[iSearch].Object as YoonRect2D;
                    pRectCurr = m_pListObject[j] as YoonRect2D;
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
            for (int i = 0; i < m_pListObject.Count - 1; i++)
            {
                iSearch = i;
                for (int j = i + 1; j < m_pListObject.Count; j++)
                {
                    pRectMin = m_pListObject[iSearch].Object as YoonRectAffine2D;
                    pRectCurr = m_pListObject[j] as YoonRectAffine2D;
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
            if (m_pListObject == null || m_pListObject.Count == 0)
                throw new InvalidOperationException("[YOONIMAGE EXCEPTION] Objects was not ordered");
            if (m_pListObject[0].Object is YoonRect2N)
                CombineRect2N();
            if (m_pListObject[0].Object is YoonRect2D)
                CombineRect2D();
            if (m_pListObject[0].Object is YoonRectAffine2D)
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
            foreach (YoonObject<T> pObject in m_pListObject)
            {
                pListTemp.Add(pObject.Clone() as YoonObject<YoonRect2N>);
            }
            m_pListObject.Clear();
            ////  모든 사각형들을 전수 조사해가며 서로 겹치는 사각형이 있는지 찾는다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                pRect1 = pListTemp[i].Object;
                combineRect = new YoonRect2N(0, 0, 0, 0);
                if (pRect1.Width == 0)
                    continue;
                isCombine = false;
                for (int j = 0; j < pListTemp.Count; j++)
                {
                    if (i == j) continue;
                    pRect2 = pListTemp[j].Object;
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
                        pListTemp[i].Object = new YoonRect2N(0, 0, 0, 0);
                        pListTemp[j].Object = combineRect;
                        break;
                    }
                }
            }
            ////  정렬된 사각형들 中 유효한 사각형들만 재정렬시킨다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                T pObject = (T)pListTemp[i].Object.Clone();
                if (pObject is YoonRect2N pRect)
                {
                    if (pRect.Right != 0)
                    {
                        m_pListObject.Add(new YoonObject<T>(pListTemp[i].Label, pObject, (YoonImage)pListTemp[i].ObjectImage.Clone(), pListTemp[i].Score, pListTemp[i].PixelCount));
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
            foreach (YoonObject<T> pObject in m_pListObject)
            {
                pListTemp.Add(pObject.Clone() as YoonObject<YoonRect2D>);
            }
            m_pListObject.Clear();
            ////  모든 사각형들을 전수 조사해가며 서로 겹치는 사각형이 있는지 찾는다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                pRect1 = pListTemp[i].Object;
                combineRect = new YoonRect2D(0, 0, 0, 0);
                if (pRect1.Width == 0)
                    continue;
                isCombine = false;
                for (int j = 0; j < pListTemp.Count; j++)
                {
                    if (i == j) continue;
                    pRect2 = pListTemp[j].Object;
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
                        pListTemp[i].Object = new YoonRect2D(0, 0, 0, 0);
                        pListTemp[j].Object = combineRect;
                        break;
                    }
                }
            }
            ////  정렬된 사각형들 中 유효한 사각형들만 재정렬시킨다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                T pObject = (T)pListTemp[i].Object.Clone();
                if (pObject is YoonRect2D pRect)
                {
                    if (pRect.Right != 0)
                    {
                        m_pListObject.Add(new YoonObject<T>(pListTemp[i].Label, pObject, (YoonImage)pListTemp[i].ObjectImage.Clone(), pListTemp[i].Score, pListTemp[i].PixelCount));
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
            foreach (YoonObject<T> pObject in m_pListObject)
            {
                pListTemp.Add(pObject.Clone() as YoonObject<YoonRectAffine2D>);
            }
            m_pListObject.Clear();
            ////  모든 사각형들을 전수 조사해가며 서로 겹치는 사각형이 있는지 찾는다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                pRect1 = pListTemp[i].Object;
                combineRect = new YoonRectAffine2D(0, 0, 0);
                if (pRect1.Width == 0)
                    continue;
                isCombine = false;
                for (int j = 0; j < pListTemp.Count; j++)
                {
                    if (i == j) continue;
                    pRect2 = pListTemp[j].Object;
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
                        pListTemp[i].Object = new YoonRectAffine2D(0, 0, 0);
                        pListTemp[j].Object = combineRect;
                        break;
                    }
                }
            }
            ////  정렬된 사각형들 中 유효한 사각형들만 재정렬시킨다.
            for (int i = 0; i < pListTemp.Count; i++)
            {
                T pObject = (T)pListTemp[i].Object.Clone();
                if (pObject is YoonRectAffine2D pRect)
                {
                    if (pRect.Right != 0)
                    {
                        m_pListObject.Add(new YoonObject<T>(pListTemp[i].Label, pObject, (YoonImage)pListTemp[i].ObjectImage.Clone(), pListTemp[i].Score, pListTemp[i].PixelCount));
                    }
                }
                else
                    break;
            }
            pListTemp.Clear();
        }
    }
}
