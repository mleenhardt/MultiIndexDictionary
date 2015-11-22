using System;
using System.Collections.Generic;

namespace MultiIndexDictionary
{
    /// <summary>
    /// A generic <see cref="IDictionary{TKey, TValue}"/> that accepts multiple look up indices
    /// that have string keys and are built based on <typeparamref name="TValue"/> values that exist
    /// in the dictionary.
    /// </summary>
    public interface IMultiIndexDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets the number of indices contained in this instance.
        /// </summary>
        int IndexCount { get; }
        /// <summary>
        /// Gets the <see cref="IDictionary{TKey, TValue}"/> associated to the specified index and index key.
        /// </summary>
        IDictionary<TKey, TValue> this[string indexName, string indexKey] { get; }
        /// <summary>
        /// Adds a new index to the dictionary.
        /// </summary>
        void AddIndex(string indexName, Func<TValue, string> indexKeyFactory);
        /// <summary>
        /// Removes an index from the dictionary.
        /// </summary>
        bool RemoveIndex(string indexName);
        /// <summary>
        /// Determines whether the dictionary contains an index with the specified name.
        /// </summary>
        bool ContainsIndex(string indexName);
        /// <summary>
        /// Determines whether the dictionary contains an index with the specified name that has an element with the specified key.
        /// </summary>
        bool ContainsIndexKey(string indexName, string indexKey);
        /// <summary>
        /// Gets the keys associated with specified index.
        /// </summary>
        ICollection<string> GetIndexKeys(string indexName);
        /// <summary>
        /// Gets the values associated with the specified index.
        /// </summary>
        ICollection<IDictionary<TKey, TValue>> GetIndexValues(string indexName);
        /// <summary>
        /// Gets the values associated with the specified index.
        /// </summary>
        /// <returns>true if the dictionary contains an index with the specified name, false otherwise.</returns>
        bool TryGetIndexValues(string indexName, out ICollection<IDictionary<TKey, TValue>> values);
        /// <summary>
        /// Gets the values associated with the specified index and index key.
        /// </summary>
        /// <returns>
        /// true if the dictionary contains an index with the specified name
        /// and within that index, an element with the specified key, false otherwise.
        /// </returns>
        bool TryGetIndexValues(string indexName, string indexKey, out IDictionary<TKey, TValue> values);
    }
}