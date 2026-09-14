using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    public class SoundLocator:SingletonMonoBehaviour<SoundLocator>
    {
        [SerializeField] private SoundPlayer soundPlayer;
        
        private ISoundPlayer _soundPlayer;
        public ISoundPlayer SoundPlayer => _soundPlayer;

        protected override void OnAwakeInternal()
        {
            base.OnAwakeInternal();
            _soundPlayer = soundPlayer;
        }
    }
}