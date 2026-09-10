using System.Threading;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaybackUnit:MonoBehaviour
    {
        private string _clipName;
        private float _playingTimeRatio;
        private bool _isPlaying;
        private float _volume;
        private bool _loopUse;
        private string _scriptName;

        private CancellationTokenSource _cts;
        private AudioSource _audioSource;
        private PlaybackUnit _subUnit;
        
        public string Name => gameObject.name;
        public string ClipName => _clipName;
        public float PlayingTimeRatio => _playingTimeRatio;
        public bool IsPlaying => _isPlaying;
        public float Volume => _volume;
        public bool LoopUse
        {
            get => _loopUse;
            set => _loopUse = value;
        }
        public string ScriptName{
            get => _scriptName;
            set => _scriptName = value;
        }
        
        public AudioSource AudioSource => _audioSource;

        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _cts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cts.Dispose();
        }

        private void Update()
        {
            if (_isPlaying != _audioSource.isPlaying)
            {
                if (_audioSource.isPlaying)
                {
                    //再生中の時
                    _clipName = _audioSource.clip.name;
                }
                else
                {
                    //再生が終了した時
                    _clipName = string.Empty;
                    _playingTimeRatio = 0f;
                    _scriptName = string.Empty;
                }
                
                _isPlaying = _audioSource.isPlaying;
            }

            if (_audioSource.isPlaying)
            {
                _volume = _audioSource.volume;
                _playingTimeRatio = _audioSource.time / _audioSource.clip.length;
            }
        }

        public void RegisterSubUnit(PlaybackUnit sub)
        {
            _subUnit = sub;
        }

        public void Stop()
        {
            _audioSource.Stop();
            _loopUse = false;
            if (_subUnit != null)
            {
                _subUnit.Stop();
                _subUnit = null;
            }
        }

        public void Cancel()
        {
            _cts.Cancel();
        }
    }
}