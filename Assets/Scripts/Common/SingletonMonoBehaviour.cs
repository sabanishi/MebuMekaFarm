using UnityEngine;

namespace Sabanishi.MebuMekaFarm
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = GetComponent<T>();
                OnAwakeInternal();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected void OnDestroy()
        {
            if (Instance == this)
            {
                OnDestroyInternal();
                Instance = null;
            }
        }
        
        /// <summary>
        /// Awake時に実行されるメソッド
        /// </summary>
        protected virtual void OnAwakeInternal() { }
        
        /// <summary>
        /// Destroy時に実行されるメソッド
        /// </summary>
        protected virtual void OnDestroyInternal() { }
    }
}