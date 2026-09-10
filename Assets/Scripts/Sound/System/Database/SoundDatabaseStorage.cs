using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    /// <summary>
    /// SoundDatabaseの参照を保持するクラス
    /// </summary>
    public class SoundDatabaseStorage
    {
        private readonly Dictionary<Type, ScriptableObject> _databases　= new Dictionary<Type, ScriptableObject>();

        public void Add<T>(SoundDatabase<T> database) where T : Enum
        {
            _databases.Add(typeof(T), database);
        }
        
        public void Remove<T>() where T : Enum
        {
            _databases.Remove(typeof(T));
        }
        
        public bool TryGet<T>(out SoundDatabase<T> database) where T : Enum
        {
            if (_databases.TryGetValue(typeof(T), out var db))
            {
                database = db as SoundDatabase<T>;
                return true;
            }

            database = null;
            return false;
        }
        
        public List<Type> GetDatabaseTypes()
        {
            return new List<Type>(_databases.Keys);
        }
    }
}