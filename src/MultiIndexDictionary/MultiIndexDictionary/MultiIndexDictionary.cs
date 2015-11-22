using System;
using System.Collections;
using System.Collections.Generic;

namespace MultiIndexDictionary
{
    public class MultiIndexDictionary<TKey, TValue> : IMultiIndexDictionary<TKey, TValue>
    {
        /// <summary>
        /// Represents a dictionary index.
        /// This class stores <see cref="Dictionary{TKey, TValue}"/> alongside their index key and
        /// provides a factory to build unique index keys from a <typeparamref name="{TValue}"/>.
        /// </summary>
        private sealed class Index : Dictionary<string, Dictionary<TKey, TValue>>
        {
            /// <summary>
            /// The index name.
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// A factory that returns a string index key provided with a <typeparamref name="{TValue}"/>.
            /// </summary>
            public Func<TValue, string> KeyFactory { get; private set; }

            public Index(string name, Func<TValue, string> keyFactory)
            {
                if (String.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException("name");
                }
                if (keyFactory == null)
                {
                    throw new ArgumentNullException("keyFactory");
                }

                Name = name;
                KeyFactory = keyFactory;
            }
        }

        /// <summary>
        /// Main storage for <see cref="KeyValuePair{TKey,TValue}"/> that
        /// are added to the <see cref="MultiIndexDictionary{TKey, TValue}"/>.
        /// </summary>
        private readonly Dictionary<TKey, TValue> _data;
        /// <summary>
        /// Underlying storage for the indices, keyed by <see cref="Index.Name"/>.
        /// </summary>
        private readonly Dictionary<string, Index> _indices;

        public int Count { get { return _data.Count; } }
        public int IndexCount { get { return _indices.Count; } }
        public bool IsReadOnly { get { return false; } }
        public ICollection<TKey> Keys { get { return _data.Keys; } }
        public ICollection<TValue> Values { get { return _data.Values; } }

        public TValue this[TKey key]
        {
            get { return _data[key]; }
            set { Add(key, value); }
        }

        public IDictionary<TKey, TValue> this[string indexName, string indexKey]
        {
            get
            {
                Index index;
                if (_indices.TryGetValue(indexName, out index))
                {
                    return index[indexKey];
                }
                else
                {
                    throw new KeyNotFoundException("Index doesn't exist.");
                }
            }
        }

        public MultiIndexDictionary()
        {
            _data = new Dictionary<TKey, TValue>();
            _indices = new Dictionary<string, Index>();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            // Adding to main dictionary.
            _data[key] = value;
            // Adding to existing indices.
            foreach (Index index in _indices.Values)
            {
                string indexKey = index.KeyFactory(value);
                Dictionary<TKey, TValue> indexData;
                if (!index.TryGetValue(indexKey, out indexData))
                {
                    // Adding an entry for the current index key in case it doesn't exist.
                    indexData = new Dictionary<TKey, TValue>();
                    index.Add(indexKey, indexData);
                }
                indexData[key] = value;
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Remove(TKey key)
        {
            TValue value;
            if (!_data.TryGetValue(key, out value))
            {
                return false;
            }
            // Removing value from main dictionary.
            _data.Remove(key);
            // Removing value from existing indices.
            foreach (Index index in _indices.Values)
            {
                index[index.KeyFactory(value)].Remove(key);
            }
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public void Clear()
        {
            // Clearing main dictionary.
            _data.Clear();
            // Clearing existing indices.
            foreach (Index index in _indices.Values)
            {
                index.Clear();
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _data.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_data).CopyTo(array, arrayIndex);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _data.TryGetValue(key, out value);
        }

        public void AddIndex(string indexName, Func<TValue, string> indexKeyFactory)
        {
            if (_indices.ContainsKey(indexName))
            {
                throw new ArgumentException("An index with this name already exists.", indexName);
            }

            var index = new Index(indexName, indexKeyFactory);
            // Building the index with existing values.
            foreach (KeyValuePair<TKey, TValue> kvp in _data)
            {
                string indexKey = index.KeyFactory(kvp.Value);
                Dictionary<TKey, TValue> indexData;
                if (!index.TryGetValue(indexKey, out indexData))
                {
                    indexData = new Dictionary<TKey, TValue>();
                    index.Add(indexKey, indexData);
                }
                indexData.Add(kvp.Key, kvp.Value);
            }
            _indices.Add(index.Name, index);
        }

        public bool RemoveIndex(string indexName)
        {
            return _indices.Remove(indexName);
        }

        public bool ContainsIndex(string indexName)
        {
            return _indices.ContainsKey(indexName);
        }

        public bool ContainsIndexKey(string indexName, string indexKey)
        {
            Index index;
            if (_indices.TryGetValue(indexName, out index))
            {
                return index.ContainsKey(indexKey);
            }
            else
            {
                throw new KeyNotFoundException("Index doesn't exist.");
            }
        }

        public ICollection<string> GetIndexKeys(string indexName)
        {
            Index index;
            if (_indices.TryGetValue(indexName, out index))
            {
                return index.Keys;
            }
            else
            {
                throw new KeyNotFoundException("Index doesn't exist.");
            }
        }

        public ICollection<IDictionary<TKey, TValue>> GetIndexValues(string indexName)
        {
            Index index;
            if (_indices.TryGetValue(indexName, out index))
            {
                return new List<IDictionary<TKey, TValue>>(index.Values);
            }
            else
            {
                throw new KeyNotFoundException("Index doesn't exist.");
            }
        }

        public bool TryGetIndexValues(string indexName, out ICollection<IDictionary<TKey, TValue>> values)
        {
            Index index;
            if (_indices.TryGetValue(indexName, out index))
            {
                values = new List<IDictionary<TKey, TValue>>(index.Values);
                return true;
            }
            else
            {
                values = null;
                return false;
            }
        }

        public bool TryGetIndexValues(string indexName, string indexKey, out IDictionary<TKey, TValue> values)
        {
            Index index;
            Dictionary<TKey, TValue> outValues;
            if (_indices.TryGetValue(indexName, out index) && index.TryGetValue(indexKey, out outValues))
            {
                values = outValues;
                return true;
            }
            else
            {
                values = null;
                return false;
            }
        }
    }
}