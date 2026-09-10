using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    /// <summary>
    /// PlaybackUnitの情報をSoundDebugDataStorageへと送信するクラス<br />
    /// エディタ上でのみ使用される
    /// </summary>
    [RequireComponent(typeof(PlaybackUnit))]
    public class PlaybackUnitDebugObserver:MonoBehaviour
    {
        private PlaybackUnit _playbackUnit;
        private int _categoryNumber;
        private int _id;

        private void Awake()
        {
            _playbackUnit = GetComponent<PlaybackUnit>();
        }

        public void Initialize(int categoryNumber, int id)
        {
            _categoryNumber = categoryNumber;
            _id = id;
        }

        private void Update()
        {
#if UNITY_EDITOR
            var storage = SoundDebugDataStorage.Instance;

            storage?.UpdatePlaybackUnitData(
                category:_categoryNumber,
                id:_id,
                name:_playbackUnit.Name,
                clipName:_playbackUnit.ClipName,
                playingTimeRatio:_playbackUnit.PlayingTimeRatio,
                playing:_playbackUnit.IsPlaying,
                volume:_playbackUnit.Volume,
                loopUse:_playbackUnit.LoopUse,
                scriptName:_playbackUnit.ScriptName);
#endif
        }
    }
}