using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoonFactory.Image
{
    public class ObjectList<T> : IDisposable, IList<YoonObject<T>>
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
    }
}
