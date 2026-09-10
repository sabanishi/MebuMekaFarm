using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    public class PlaybackUnitDebugObserverParent:MonoBehaviour
    {
        [SerializeField] private int categoryNumber;

        private void Awake()
        {
#if UNITY_EDITOR
            int index = 0;
            foreach(var observer in GetComponentsInChildren<PlaybackUnitDebugObserver>())
            {
                observer.Initialize(categoryNumber, index);
                index++;
            }
#endif
        }
    }
}