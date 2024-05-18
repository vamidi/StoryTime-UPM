
using System;
using System.Collections.Generic;
using System.Diagnostics;
using StoryTime.Domains.Extensions.Serialization;
using UnityEngine;

namespace StoryTime.Domains.Serialization
{
    using LookupTables;
    
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    { 
        /* [FormerlySerializedAs("_serializedList")]  */
        [SerializeField] internal List<SerializedKeyValuePair<TKey, TValue>> serializedList = new ();
        
#if UNITY_EDITOR
        internal IKeyable LookupTable
        {
            get
            {
                if (_lookupTable == null)
                    _lookupTable = new DictionaryLookupTable<TKey, TValue>(this);
                return _lookupTable;
            }
        }

        private DictionaryLookupTable<TKey, TValue> _lookupTable;
#endif
        
        public SerializableDictionary() {}

        public SerializableDictionary(SerializableDictionary<TKey, TValue> serializableDictionary) : base(serializableDictionary)
        {
#if UNITY_EDITOR
            foreach (var kvp in serializableDictionary.serializedList)
                serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
#endif
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
            SyncDictionaryToBackingField_Editor();
        }

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(
            dictionary, comparer)
        {
            SyncDictionaryToBackingField_Editor();
        }

        public SerializableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
        {
            SyncDictionaryToBackingField_Editor();
        }

        public SerializableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection,
            IEqualityComparer<TKey> comparer) : base(collection, comparer)
        {
            SyncDictionaryToBackingField_Editor();
        }
        public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public SerializableDictionary(int capacity) : base(capacity) { }
        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
        
        [Conditional("UNITY_EDITOR")]
        private void SyncDictionaryToBackingField_Editor()
        {
            foreach (var kvp in this)
                serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
        }

#if UNITY_EDITOR
        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                base[key] = value;
                bool anyEntryWasFound = false;
                for (int i = 0; i < serializedList.Count; i++)
                {
                    var kvp = serializedList[i];
                    if (!SerializedCollectionsUtility.KeysAreEqual(key, kvp.Key))
                        continue;
                    anyEntryWasFound = true;
                    kvp.Value = value;
                    serializedList[i] = kvp;
                }
                
                if (!anyEntryWasFound)
                    serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
            }
        }
        
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
        }

        public new void Clear()
        {
            base.Clear();
            serializedList.Clear();
        }

        public new bool Remove(TKey key)
        {
            if (TryGetValue(key, out var value))
            {
                base.Remove(key);
                serializedList.Remove(new SerializedKeyValuePair<TKey, TValue>(key, value));
                return true;
            }

            return false;
        }

        public new bool TryAdd(TKey key, TValue value)
        {
            if (base.TryAdd(key, value))
            {
                serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Only available in Editor. Add a key value pair, even if the key already exists in the dictionary.
        /// </summary>
        public void AddConflictAllowed(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                base.Add(key, value);
            serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
        }
#endif

        public void OnAfterDeserialize()
        {
            base.Clear();

            foreach (var kvp in serializedList)
            {
#if UNITY_EDITOR
                if (SerializedCollectionsUtility.IsValidKey(kvp.Key) && !ContainsKey(kvp.Key))
                    base.Add(kvp.Key, kvp.Value);
#else
                    Add(kvp.Key, kvp.Value);
#endif
            }

#if UNITY_EDITOR
            LookupTable.RecalculateOccurences();
#else
            _serializedList.Clear();
#endif
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                LookupTable.RemoveDuplicates();

            // TODO: is there a better way to check if the dictionary was deserialized with reflection?
            if (serializedList.Count == 0 && Count > 0)
                SyncDictionaryToBackingField_Editor();
#else
            _serializedList.Clear();
            foreach (var kvp in this)
                _serializedList.Add(new SerializedKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
#endif
        }
    }
}