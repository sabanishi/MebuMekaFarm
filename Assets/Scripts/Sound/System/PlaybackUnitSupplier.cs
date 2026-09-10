using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    /// <summary>
    /// PlaybackUnitの参照を保持するクラス
    /// </summary>
    public class PlaybackUnitSupplier:MonoBehaviour
    {
        private readonly List<PlaybackUnit> _audioSourceUnits = new List<PlaybackUnit>();
        
        private void Awake()
        {
            foreach (var source in this.gameObject.GetComponentsInChildren<PlaybackUnit>())
            {
                _audioSourceUnits.Add(source);
            }
        }
        
        /// <summary>
        /// ループに使用中でなく，再生中でもないPlaybackUnitを返す<br />
        /// 存在しない場合はnullを返す
        /// </summary>
        public PlaybackUnit Supply()
        {
            return _audioSourceUnits.FirstOrDefault(
                unit =>! unit.LoopUse && 
                       ! unit.AudioSource.isPlaying);
        }
    }
}