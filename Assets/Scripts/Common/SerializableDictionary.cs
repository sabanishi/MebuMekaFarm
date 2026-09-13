using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Common
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> data = new List<SerializableKeyValuePair<TKey, TValue>>();

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Rebuild();
        }

        public void Rebuild()
        {
            Clear();

            foreach (var pair in data)
            {
                this[pair.Key] = pair.Value;
            }
        }
    }

    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        [SerializeField] public TKey Key;
        [SerializeField] public TValue Value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}