using System;
using System.Collections;
using System.Collections.Generic;

namespace MultiIndexDictionary
{
    public class MultiIndexDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Represents a dictionary index.
        /// This class stores <see cref="List{TValue}"/> alongside their index key and
        /// provides a factory to build unique index keys from a <typeparamref name="{TValue}"/>.
        /// </summary>
        private sealed class Index : Dictionary<string, List<TValue>>
        {
            /// <summary>
            /// The index name.
            /// </summary>
            public string Name { get; private set; }
            //public Dictionary<string, List<TValue>> Data { get; private set; }
            /// <summary>
            /// A factory that returns a string index key provided with a <typeparamref name="{TValue}"/>.
            /// </summary>
            public Func<TValue, string> KeyFactory { get; private set; }

            public Index(string name, Func<TValue, string> keyFactory)
            {
                Name = name;
                //Data = new Dictionary<string, List<TValue>>();
                KeyFactory = keyFactory;
            }
        }

        /// <summary>
        /// Underlying storage for <see cref="KeyValuePair{TKey,TValue}"/> that
        /// will be added to the <see cref="MultiIndexDictionary{TKey, TValue}"/>.
        /// </summary>
        private readonly Dictionary<TKey, TValue> _data;
        /// <summary>
        /// Underlying storage for the indices, keyed by <see cref="Index.Name"/>.
        /// </summary>
        private readonly Dictionary<string, Index> _indices;

        public MultiIndexDictionary()
        {
            _data = new Dictionary<TKey, TValue>();
            _indices = new Dictionary<string, Index>();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }
        public bool ContainsKey(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new System.NotImplementedException();
        }

        public TValue this[TKey key] { get { throw new System.NotImplementedException(); } set { throw new System.NotImplementedException(); } }

        public ICollection<TKey> Keys { get; private set; }
        public ICollection<TValue> Values { get; private set; }
    }
}