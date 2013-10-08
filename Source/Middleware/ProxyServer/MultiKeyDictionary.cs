/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProxyServer
{
    /// <summary>
    /// Multi-Key Dictionary Class
    /// </summary>    
    /// <typeparam name="K">Primary Key Type</typeparam>
    /// <typeparam name="L">Sub Key Type</typeparam>
    /// <typeparam name="V">Value Type</typeparam>
    public class MultiKeyDictionary<K, L, V>
    {
        internal readonly Dictionary<K, V> baseDictionary = new Dictionary<K, V>();
        internal readonly Dictionary<L, K> subDictionary = new Dictionary<L, K>();
        internal readonly Dictionary<K, L> primaryToSubkeyMapping = new Dictionary<K, L>();

        ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        public V this[L subKey]
        {
            get
            {
                V item;
                if (TryGetValue(subKey, out item))
                    return item;

                throw new KeyNotFoundException("sub key not found: " + subKey.ToString());
            }
        }

        public V this[K primaryKey]
        {
            get
            {
                V item;
                if (TryGetValue(primaryKey, out item))
                    return item;

                throw new KeyNotFoundException("primary key not found: " + primaryKey.ToString());
            }
        }

        public void Associate(L subKey, K primaryKey)
        {
            readerWriterLock.EnterUpgradeableReadLock();

            try
            {
                if (!baseDictionary.ContainsKey(primaryKey))
                    throw new KeyNotFoundException(string.Format("The base dictionary does not contain the key '{0}'", primaryKey));

                if (primaryToSubkeyMapping.ContainsKey(primaryKey)) // Remove the old mapping first
                {
                    readerWriterLock.EnterWriteLock();

                    try
                    {
                        if (subDictionary.ContainsKey(primaryToSubkeyMapping[primaryKey]))
                        {
                            subDictionary.Remove(primaryToSubkeyMapping[primaryKey]);
                        }

                        primaryToSubkeyMapping.Remove(primaryKey);
                    }
                    finally
                    {
                        readerWriterLock.ExitWriteLock();
                    }
                }

                subDictionary[subKey] = primaryKey;
                primaryToSubkeyMapping[primaryKey] = subKey;
            }
            finally
            {
                readerWriterLock.ExitUpgradeableReadLock();
            }
        }

        public bool TryGetValue(L subKey, out V val)
        {
            val = default(V);

            K primaryKey;

            readerWriterLock.EnterReadLock();

            try
            {
                if (subDictionary.TryGetValue(subKey, out primaryKey))
                {
                    return baseDictionary.TryGetValue(primaryKey, out val);
                }
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }

            return false;
        }

        public bool TryGetValue(K primaryKey, out V val)
        {
            readerWriterLock.EnterReadLock();

            try
            {
                return baseDictionary.TryGetValue(primaryKey, out val);
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        public bool ContainsKey(L subKey)
        {
            V val;

            return TryGetValue(subKey, out val);
        }

        public bool ContainsKey(K primaryKey)
        {
            V val;

            return TryGetValue(primaryKey, out val);
        }

        public void Remove(K primaryKey)
        {
            readerWriterLock.EnterWriteLock();

            try
            {
                if (primaryToSubkeyMapping.ContainsKey(primaryKey))
                {
                    if (subDictionary.ContainsKey(primaryToSubkeyMapping[primaryKey]))
                    {
                        subDictionary.Remove(primaryToSubkeyMapping[primaryKey]);
                    }

                    primaryToSubkeyMapping.Remove(primaryKey);
                }

                baseDictionary.Remove(primaryKey);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public void Remove(L subKey)
        {
            readerWriterLock.EnterWriteLock();

            try
            {
                baseDictionary.Remove(subDictionary[subKey]);

                primaryToSubkeyMapping.Remove(subDictionary[subKey]);

                subDictionary.Remove(subKey);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public void Add(K primaryKey, V val)
        {
            readerWriterLock.EnterWriteLock();

            try
            {
                baseDictionary.Add(primaryKey, val);
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public void Add(K primaryKey, L subKey, V val)
        {
            Add(primaryKey, val);

            Associate(subKey, primaryKey);
        }

        public V[] CloneValues()
        {
            readerWriterLock.EnterReadLock();

            try
            {
                V[] values = new V[baseDictionary.Values.Count];

                baseDictionary.Values.CopyTo(values, 0);

                return values;
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        public List<V> Values
        {
            get
            {
                readerWriterLock.EnterReadLock();

                try
                {
                    return baseDictionary.Values.ToList();
                }
                finally
                {
                    readerWriterLock.ExitReadLock();
                }
            }
        }

        public K[] ClonePrimaryKeys()
        {
            readerWriterLock.EnterReadLock();

            try
            {
                K[] values = new K[baseDictionary.Keys.Count];

                baseDictionary.Keys.CopyTo(values, 0);

                return values;
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        public L[] CloneSubKeys()
        {
            readerWriterLock.EnterReadLock();

            try
            {
                L[] values = new L[subDictionary.Keys.Count];

                subDictionary.Keys.CopyTo(values, 0);

                return values;
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        public void Clear()
        {
            readerWriterLock.EnterWriteLock();

            try
            {
                baseDictionary.Clear();

                subDictionary.Clear();

                primaryToSubkeyMapping.Clear();
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        public int Count
        {
            get
            {
                readerWriterLock.EnterReadLock();

                try
                {
                    return baseDictionary.Count;
                }
                finally
                {
                    readerWriterLock.ExitReadLock();
                }
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            readerWriterLock.EnterReadLock();

            try
            {
                return baseDictionary.GetEnumerator();
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }
    }
}