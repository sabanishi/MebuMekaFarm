using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    [Serializable]
    public class SoundDatabase<T>: ScriptableObject,ISoundDatabase where T:Enum
    {
        [Serializable]
        public class AudioData
        {
            public T tag;
            public AudioClip audioClip;
        }
        
        [SerializeField] private AudioData[] audioDataList;
        
        public bool TryGetClip(T clipEnum,out AudioClip clip)
        {
            clip = null;
            foreach (var audioData in audioDataList)
            {
                if (audioData.tag.Equals(clipEnum))
                {
                    clip = audioData.audioClip;
                    return true;
                }
            }
            return false;
        }

        public void Setup()
        {
            // 自身をSoundPlayerに登録
            SoundLocator.Instance.SoundPlayer.Setup<T>(this);
        }

        public void Cleanup()
        {
            // 自身をSoundPlayerから削除
            SoundLocator.Instance.SoundPlayer.Cleanup<T>();
        }

        /// <summary>
        /// AudioClipの参照が外れているAudioDataのtag名を取得
        /// </summary>
        public string[] LookupReferenceMissingClipNames()
        {
            var missingClipNames = new List<string>();
            foreach (var audioData in audioDataList)
            {
                if (audioData.audioClip == null)
                {
                    missingClipNames.Add(Enum.GetName(audioData.tag.GetType(), audioData.tag));
                }
            }
            return missingClipNames.ToArray();
        }
    }
}