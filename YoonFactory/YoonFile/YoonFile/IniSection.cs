// INI-FACTORY :  CJYOON @2020~
using System;
using System.Collections.Generic;
using System.Linq;

namespace YoonFactory.Files.Ini
{
    public struct IniValue
    {
        private static bool TryParseInt(string text, out int value)
        {
            int res;
            if (Int32.TryParse(text,
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture,
                out res))
            {
                value = res;
                return true;
            }
            value = 0;
            return false;
        }

        private static bool TryParseDouble(string text, out double value)
        {
            double res;
            if (Double.TryParse(text,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out res))
            {
                value = res;
                return true;
            }
            value = Double.NaN;
            return false;
        }

        private static bool TryParseEnum<T>(string text, out T value) where T : struct, IFormattable
        {
            T res;
            if (Enum.TryParse(text, out res))
            {
                value = res;
                return true;
            }
            value = default(T);
            return false;
        }

        private static bool TryParse<T>(string text, out T value) where T : IConvertible
        {
            try
            {
                value = (T)Convert.ChangeType(text, typeof(T));
                if (value == null)
                    return false;
                else
                    return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        public string Value;

        public IniValue(object value)
        {
            var formattable = value as IFormattable;
            if (formattable != null)
            {
                Value = formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                Value = value != null ? value.ToString() : null;
            }
        }

        public IniValue(string value)
        {
            Value = value;
        }

        public bool ToBool(bool valueIfInvalid = false)
        {
            bool res;
            if (TryConvertBool(out res))
            {
                return res;
            }
            return valueIfInvalid;
        }

        public bool TryConvertBool(out bool result)
        {
            if (Value == null)
            {
                result = default(bool);
                return false;
            }
            var boolStr = Value.Trim().ToLowerInvariant();
            if (boolStr == "true")
            {
                result = true;
                return true;
            }
            else if (boolStr == "false")
            {
                result = false;
                return true;
            }
            result = default(bool);
            return false;
        }

        public int ToInt(int valueIfInvalid = 0)
        {
            int res;
            if (TryConvertInt(out res))
            {
                return res;
            }
            return valueIfInvalid;
        }

        public bool TryConvertInt(out int result)
        {
            if (Value == null)
            {
                result = default(int);
                return false;
            }
            if (TryParseInt(Value.Trim(), out result))
            {
                return true;
            }
            return false;
        }

        public double ToDouble(double valueIfInvalid = 0)
        {
            double res;
            if (TryConvertDouble(out res))
            {
                return res;
            }
            return valueIfInvalid;
        }

        public bool TryConvertDouble(out double result)
        {
            if (Value == null)
            {
                result = default(double);
                return false; ;
            }
            if (TryParseDouble(Value.Trim(), out result))
            {
                return true;
            }
            return false;
        }

        public T ToEnum<T>(T valueIfInvalue) where T : struct, IFormattable
        {
            T res;
            if (TryConvertEnum(out res))
            {
                return res;
            }
            return valueIfInvalue;
        }

        public bool TryConvertEnum<T>(out T result) where T : struct, IFormattable
        {
            if (Value == null)
            {
                result = default(T);
                return false;
            }
            if (TryParseEnum<T>(Value.Trim(), out result))
            {
                return true;
            }
            return false;
        }

        public T To<T>(T valueIfInvalid) where T : IConvertible
        {
            T res;
            if (TryConvert(out res))
            {
                return res;
            }
            return valueIfInvalid;
        }

        public bool TryConvert<T>(out T result) where T : IConvertible
        {
            if (Value == null)
            {
                result = default(T);
                return false;
            }
            if (TryParse<T>(Value.Trim(), out result))
            {
                return true;
            }
            return false;
        }

        public string GetString()
        {
            return GetString(true, false);
        }

        public string GetString(bool preserveWhitespace)
        {
            return GetString(true, preserveWhitespace);
        }

        public string GetString(bool allowOuterQuotes, bool preserveWhitespace)
        {
            if (Value == null)
            {
                return "";
            }
            var trimmed = Value.Trim();
            if (allowOuterQuotes && trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[trimmed.Length - 1] == '"')
            {
                var inner = trimmed.Substring(1, trimmed.Length - 2);
                return preserveWhitespace ? inner : inner.Trim();
            }
            else
            {
                return preserveWhitespace ? Value : Value.Trim();
            }
        }

        public override string ToString()
        {
            return Value;
        }

        public string ToString(string valueIfInvalid)
        {
            if (String.IsNullOrEmpty(Value))
            {
                return valueIfInvalid;
            }
            return Value;
        }

        public static implicit operator IniValue(byte o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(short o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(int o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(sbyte o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(ushort o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(uint o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(float o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(double o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(bool o)
        {
            return new IniValue(o);
        }

        public static implicit operator IniValue(string o)
        {
            return new IniValue(o);
        }

        private static readonly IniValue _default = new IniValue();
        public static IniValue Default { get { return _default; } }
    }

    public class IniSection : IYoonSection<string, IniValue>
    {
        #region IDisposable Support
        ~IniSection()
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
            Clear();
            m_pDicIniValue = null;
            m_pListKeyOrdered = null;
            //// .NET Framework에 의하여 관리되지 않는 외부 리소스들을 여기서 정리합니다.
            this.disposed = true;
        }
        #endregion

        public string RootDirectory { get; set; }

        private Dictionary<string, IniValue> m_pDicIniValue;
        private List<string> m_pListKeyOrdered;

        public IEqualityComparer<string> Comparer { get { return m_pDicIniValue.Comparer; } }

        public IniValue this[int nIndex]
        {
            get
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index IniSection using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                return m_pDicIniValue[m_pListKeyOrdered[nIndex]];
            }
            set
            {
                if (m_pListKeyOrdered == null)
                {
                    throw new InvalidOperationException("Cannot index IniSection using integer key: section was not ordered.");
                }
                if (nIndex < 0 || nIndex >= m_pListKeyOrdered.Count)
                {
                    throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: index");
                }
                var key = m_pListKeyOrdered[nIndex];
                m_pDicIniValue[key] = value;
            }
        }

        public IniValue this[string strName]
        {
            get
            {
                IniValue pValue;
                if (m_pDicIniValue.TryGetValue(strName, out pValue))
                {
                    return pValue;
                }
                return IniValue.Default;
            }
            set
            {
                if (m_pListKeyOrdered != null && !m_pListKeyOrdered.Contains(strName, Comparer))
                {
                    m_pListKeyOrdered.Add(strName);
                }
                m_pDicIniValue[strName] = value;
            }
        }

        public IniSection()
            : this(YoonIni.DefaultComparer)
        {
        }

        public IniSection(IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicIniValue = new Dictionary<string, IniValue>(pStringComparer);
        }

        public IniSection(Dictionary<string, IniValue> pDic)
            : this(pDic, YoonIni.DefaultComparer)
        {
        }

        public IniSection(Dictionary<string, IniValue> pDic, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicIniValue = new Dictionary<string, IniValue>(pDic, pStringComparer);
        }

        public IniSection(IniSection pSection)
            : this(pSection, YoonIni.DefaultComparer)
        {
        }

        public IniSection(IniSection pSection, IEqualityComparer<string> pStringComparer)
        {
            this.m_pDicIniValue = new Dictionary<string, IniValue>(pSection.m_pDicIniValue, pStringComparer);
        }

        public void Clear()
        {
            if (m_pDicIniValue != null)
                m_pDicIniValue.Clear();
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Clear();
            }
        }

        public string KeyOf(int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call KeyOf(int) on IniSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            return m_pListKeyOrdered[nIndex];
        }

        public int IndexOf(string strKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string) on IniSection: section was not ordered.");
            }
            return IndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int IndexOf(string strKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int) on IniSection: section was not ordered.");
            }
            return IndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int IndexOf(string strKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call IndexOf(string, int, int) on IniSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name: nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            var end = nIndex + nCount;
            for (int i = nIndex; i < end; i++)
            {
                if (Comparer.Equals(m_pListKeyOrdered[i], strKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public int LastIndexOf(string strKey)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string) on IniSection: section was not ordered.");
            }
            return LastIndexOf(strKey, 0, m_pListKeyOrdered.Count);
        }

        public int LastIndexOf(string strKey, int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int) on IniSection: section was not ordered.");
            }
            return LastIndexOf(strKey, nIndex, m_pListKeyOrdered.Count - nIndex);
        }

        public int LastIndexOf(string strKey, int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call LastIndexOf(string, int, int) on IniSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name: nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            var end = nIndex + nCount;
            for (int i = end - 1; i >= nIndex; i--)
            {
                if (Comparer.Equals(m_pListKeyOrdered[i], strKey))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int nIndex, string strKey, IniValue pValue)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Insert(int, string, IniValue) on IniSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            m_pDicIniValue.Add(strKey, pValue);
            m_pListKeyOrdered.Insert(nIndex, strKey);
        }

        public void InsertRange(int nIndex, IEnumerable<KeyValuePair<string, IniValue>> pCollection)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call InsertRange(int, IEnumerable<KeyValuePair<string, IniValue>>) on IniSection: section was not ordered.");
            }
            if (pCollection == null)
            {
                throw new ArgumentNullException("Value cannot be null." + Environment.NewLine + "Parameter name: pCollection");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            foreach (var kvp in pCollection)
            {
                Insert(nIndex, kvp.Key, kvp.Value);
                nIndex++;
            }
        }

        public void RemoveAt(int nIndex)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call RemoveAt(int) on IniSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            var key = m_pListKeyOrdered[nIndex];
            m_pListKeyOrdered.RemoveAt(nIndex);
            m_pDicIniValue.Remove(key);
        }

        public void RemoveRange(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call RemoveRange(int, int) on IniSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name: nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            for (int i = 0; i < nCount; i++)
            {
                RemoveAt(nIndex);
            }
        }

        public void Reverse()
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse() on IniSection: section was not ordered.");
            }
            m_pListKeyOrdered.Reverse();
        }

        public void Reverse(int nIndex, int nCount)
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call Reverse(int, int) on IniSection: section was not ordered.");
            }
            if (nIndex < 0 || nIndex > m_pListKeyOrdered.Count)
            {
                throw new IndexOutOfRangeException("Index must be within the bounds." + Environment.NewLine + "Parameter name: nIndex");
            }
            if (nCount < 0)
            {
                throw new IndexOutOfRangeException("Count cannot be less than zero." + Environment.NewLine + "Parameter name: nCount");
            }
            if (nIndex + nCount > m_pListKeyOrdered.Count)
            {
                throw new ArgumentException("Index and count were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
            }
            m_pListKeyOrdered.Reverse(nIndex, nCount);
        }

        public ICollection<IniValue> GetOrderedValues()
        {
            if (m_pListKeyOrdered == null)
            {
                throw new InvalidOperationException("Cannot call GetOrderedValues() on IniSection: section was not ordered.");
            }
            var list = new List<IniValue>();
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                list.Add(m_pDicIniValue[m_pListKeyOrdered[i]]);
            }
            return list;
        }

        public void Add(string strKey, IniValue pValue)
        {
            m_pDicIniValue.Add(strKey, pValue);
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Add(strKey);
            }
        }

        public bool ContainsKey(string strKey)
        {
            return m_pDicIniValue.ContainsKey(strKey);
        }

        /// <summary>
        /// Returns this IniSection's collection of keys. If the IniSection is ordered, the keys will be returned in order.
        /// </summary>
        public ICollection<string> Keys
        {
            get { return (m_pListKeyOrdered != null) ? (ICollection<string>)m_pListKeyOrdered : m_pDicIniValue.Keys; }
        }

        public bool Remove(string strKey)
        {
            var ret = m_pDicIniValue.Remove(strKey);
            if (m_pListKeyOrdered != null && ret)
            {
                for (int i = 0; i < m_pListKeyOrdered.Count; i++)
                {
                    if (Comparer.Equals(m_pListKeyOrdered[i], strKey))
                    {
                        m_pListKeyOrdered.RemoveAt(i);
                        break;
                    }
                }
            }
            return ret;
        }

        public bool TryGetValue(string strKey, out IniValue pValue)
        {
            return m_pDicIniValue.TryGetValue(strKey, out pValue);
        }

        /// <summary>
        /// Returns the values in this IniSection. These values are always out of order. To get ordered values from an IniSection call GetOrderedValues instead.
        /// </summary>
        public ICollection<IniValue> Values
        {
            get
            {
                return m_pDicIniValue.Values;
            }
        }

        void ICollection<KeyValuePair<string, IniValue>>.Add(KeyValuePair<string, IniValue> pCollection)
        {
            ((IDictionary<string, IniValue>)m_pDicIniValue).Add(pCollection);
            if (m_pListKeyOrdered != null)
            {
                m_pListKeyOrdered.Add(pCollection.Key);
            }
        }

        bool ICollection<KeyValuePair<string, IniValue>>.Contains(KeyValuePair<string, IniValue> pCollection)
        {
            return ((IDictionary<string, IniValue>)m_pDicIniValue).Contains(pCollection);
        }

        void ICollection<KeyValuePair<string, IniValue>>.CopyTo(KeyValuePair<string, IniValue>[] pArray, int nIndexArray)
        {
            ((IDictionary<string, IniValue>)m_pDicIniValue).CopyTo(pArray, nIndexArray);
        }

        public int Count
        {
            get { return m_pDicIniValue.Count; }
        }

        bool ICollection<KeyValuePair<string, IniValue>>.IsReadOnly
        {
            get { return ((IDictionary<string, IniValue>)m_pDicIniValue).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<string, IniValue>>.Remove(KeyValuePair<string, IniValue> pCollection)
        {
            var ret = ((IDictionary<string, IniValue>)m_pDicIniValue).Remove(pCollection);
            if (m_pListKeyOrdered != null && ret)
            {
                for (int i = 0; i < m_pListKeyOrdered.Count; i++)
                {
                    if (Comparer.Equals(m_pListKeyOrdered[i], pCollection.Key))
                    {
                        m_pListKeyOrdered.RemoveAt(i);
                        break;
                    }
                }
            }
            return ret;
        }

        public IEnumerator<KeyValuePair<string, IniValue>> GetEnumerator()
        {
            if (m_pListKeyOrdered != null)
            {
                return GetOrderedEnumerator();
            }
            else
            {
                return m_pDicIniValue.GetEnumerator();
            }
        }

        private IEnumerator<KeyValuePair<string, IniValue>> GetOrderedEnumerator()
        {
            for (int i = 0; i < m_pListKeyOrdered.Count; i++)
            {
                yield return new KeyValuePair<string, IniValue>(m_pListKeyOrdered[i], m_pDicIniValue[m_pListKeyOrdered[i]]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator IniSection(Dictionary<string, IniValue> pDic)
        {
            return new IniSection(pDic);
        }

        public static explicit operator Dictionary<string, IniValue>(IniSection pSection)
        {
            return pSection.m_pDicIniValue;
        }
    }
}