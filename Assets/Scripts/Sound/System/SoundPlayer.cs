using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Sabanishi.MebuMekaFarm.Sound
{
    /// <summary>
    /// ISoundPlayerの具象クラス
    /// </summary>
    public class SoundPlayer:MonoBehaviour,ISoundPlayer
    {
        [SerializeField] private PlaybackUnitSupplier bgmPlaybackUnitSupplier;
        [SerializeField] private PlaybackUnitSupplier sePlaybackUnitSupplier;
        
        private AudioSource _audioSource;
        private AudioClip _audioClip;
        
        private SoundDatabaseStorage _databaseStorage;

        private void Awake()
        {
            _databaseStorage = new SoundDatabaseStorage();
        }

        #region Public Method
        
        public void Setup<T>(SoundDatabase<T> database) where T : Enum
        {
            _databaseStorage.Add(database);
        }
        
        public void Cleanup<T>() where T : Enum
        {
            _databaseStorage.Remove<T>();
        }
        
        public PlaybackUnit PlaySe<T>(T clipEnum,float pitch = 1.0f, int callerDepth = 2) where T : Enum
        {
            return Play(clipEnum, true, pitch, callerDepth);
        }
        
        public PlaybackUnit PlayBgm<T>(T clipEnum,float pitch = 1.0f, int callerDepth = 2) where T : Enum
        {
            return Play(clipEnum, false, pitch, callerDepth);
        }

        public async UniTask<PlaybackUnit> PlaySeAsync<T>(T clipEnum,float pitch=1.0f, int callerDepth=2) where T : Enum
        {
            return await PlayAsync(clipEnum, true, pitch, callerDepth);
        }

        public async UniTask<PlaybackUnit> PlayBgmAsync<T>(T clipEnum, float pitch=1.0f, int callerDepth=2) where T : Enum
        {
            return await PlayAsync(clipEnum, false, pitch, callerDepth);
        }
        
        public PlaybackUnit PlayLoopSe<T>(T clipEnum,float crossFadeTime=0f) where T : Enum
        {
            return PlayLoop(clipEnum,true, crossFadeTime);
        }
        
        public PlaybackUnit PlayLoopBgm<T>(T clipEnum,float crossFadeTime=0f) where T : Enum
        {
            return PlayLoop(clipEnum,false, crossFadeTime);
        }
        
        public void Stop(PlaybackUnit playbackUnit, float fadeOutTime=0f)
        {
            if (!playbackUnit)
            {
                Debug.Log($"Stop対象がNullになっています");
                return;
            }
            var audioSource = playbackUnit.AudioSource;
            // フェードアウトしてから再生停止
            DOTween.To((() => audioSource.volume), (value) => audioSource.volume = value, 0f, fadeOutTime)
                .SetEase(Ease.InOutSine)
                .OnComplete((() =>
                {
                    playbackUnit.Stop();
                    audioSource.volume = 1.0f;
                    playbackUnit.Cancel();
                }));
        }
        
        public void Ducking(PlaybackUnit playbackUnit, float fadeInTime, float duckingTime, float fadeOutTime)
        {
            if (playbackUnit == null)
            {
                Debug.LogWarning($"PlaybackUnitが参照できません");
                return;
            }
            
            var audioSource = playbackUnit.AudioSource;
            var sequence = DOTween.Sequence();
            sequence
                .Append(
                    DOTween
                        .To((() => audioSource.volume), value => audioSource.volume = value, 0f, fadeInTime)
                        .SetEase(Ease.InCubic)
                )
                .AppendInterval(duckingTime - (fadeInTime + fadeOutTime))
                .Append(
                    DOTween
                        .To((() => audioSource.volume), value => audioSource.volume = value, 1f, fadeOutTime)
                        .SetEase(Ease.InCubic)
                );
        }
        
        #endregion

        #region Private Method
        
        private PlaybackUnit Play<T>(T clipEnum, bool isSe,float pitch = 1.0f, int callerDepth = 2) where T : Enum
        {
            var playbackUnit = RequirePlayBackUnit(isSe);
            
            if (!TryGetAudioClip<T>(clipEnum, out var clip))
            {
                return null;
            }
            _audioClip = clip;
            PlayBase(playbackUnit, _audioClip, pitch, callerDepth);
            return playbackUnit;
        }
        
        private async UniTask<PlaybackUnit> PlayAsync<T>(T clipEnum,bool isSe,float pitch, int callerDepth) where T : Enum
        {
            var token = this.GetCancellationTokenOnDestroy();
            PlaybackUnit playbackUnit = null;
            while (true)
            {
                playbackUnit = RequirePlayBackUnit(isSe);
                if(playbackUnit != null) break;
                await UniTask.DelayFrame(1,cancellationToken:token);
            }
            
            if (!TryGetAudioClip<T>(clipEnum, out var clip))
            {
                return null;
            }
            _audioClip = clip;
            
            PlayBase(playbackUnit,clip, pitch, callerDepth);
            return playbackUnit;
        }
        
        private PlaybackUnit PlayLoop<T>(T clipEnum, bool isSe,float crossFadeTime) where T : Enum
        {
            if (!TryGetAudioClip<T>(clipEnum, out var clip))
            {
                return null;
            }
            _audioClip = clip;
            
            var playbackUnit = RequirePlayBackUnit(isSe);
            if (playbackUnit==null)
            {
                Debug.Log($"{clipEnum} のループ再生を中止");
                return null;
            }
            playbackUnit.LoopUse = true; // サブをサプライする前にloopUseをtrueにする（メインと同じインスタンスを返してしまうため）
            var playbackUnitSub = RequirePlayBackUnit(isSe);
            if (playbackUnitSub==null)
            {
                Debug.Log($"Loop用のサブユニットが見つかりませんでした");
                return null;
            }
            playbackUnitSub.LoopUse = true;
            
            playbackUnit.RegisterSubUnit(playbackUnitSub);
            
            // _playbackUnitをStop時の参照に使うため，Forgetで次の戻り値をすぐに取得可能にする
            PlayLoopOrder(playbackUnit, playbackUnitSub, crossFadeTime).Forget();

            return playbackUnit;
        }
        
        private async UniTask PlayLoopOrder(PlaybackUnit main,PlaybackUnit sub, float crossFadeTime)
        {
            AudioClip audioClip = _audioClip;
            Queue<PlaybackUnit> interchange = new Queue<PlaybackUnit>();
            interchange.Enqueue(sub);

            PlaybackUnit nowPlayer = main;
            PlaybackUnit fadeoutPlayer;
            PlaybackUnit fadeInPlayer;
            PlayBase(main, audioClip, 1.0f);

            while (main.LoopUse)
            {
                var cacheTime = 0f;
                while (true)
                {
                    // AudioClipを再生し終わる(crossFadeTime秒だけ残して再生し終わる)まで待機
                    // 一回目ループだとfadeoutPlayer，二回目以降のループだとfadeInPlayerが参照されている
                    if (nowPlayer.AudioSource.time >= audioClip.length - crossFadeTime)
                    {
                        break;
                    }

                    // 前のフレームには再生していたのに、今フレームでは再生されていない時、再生が終了したと判断する
                    // crossFadeTime=0の時、上の条件が永遠に成立しないことがあるため、この条件も必要
                    if (cacheTime > 0 && nowPlayer.AudioSource.time == 0)
                    {
                        break;
                    }
                    
                    cacheTime = nowPlayer.AudioSource.time;
                    await UniTask.DelayFrame(1);
                }

                fadeoutPlayer = nowPlayer;
                // Fade out.
                DOVirtual.Float(1.0f, 0.0f, crossFadeTime,
                        value => fadeoutPlayer.AudioSource.volume = value)
                    .OnComplete((() => fadeoutPlayer.AudioSource.volume = 1.0f)); // ボリュームを元に戻す
                // 待機列に入れなおす
                interchange.Enqueue(fadeoutPlayer);
                // 次のPlaybackUnitを取得
                nowPlayer = interchange.Dequeue();
                fadeInPlayer = nowPlayer;
                // フェードインの前に一度判定しておく
                if (!main.LoopUse) continue;
                // Fade in.
                DOVirtual.Float(0.0f, 1.0f, crossFadeTime, 
                    value => fadeInPlayer.AudioSource.volume = value);
                PlayBase(fadeInPlayer, audioClip, 1.0f, 3);
            }
        }
        
        /// <summary>
        /// オーディオ再生の基底メソッド
        /// </summary>
        /// <param name="playbackUnit"></param>
        /// <param name="audioClip"></param>
        /// <param name="pitch"></param>
        /// <param name="callerDepth">明らかにしたい呼び出し元クラスの深さ（Play()等を挟むので２以上）</param>
        private void PlayBase(PlaybackUnit playbackUnit, AudioClip audioClip, float pitch, int callerDepth = 2)
        {
            if (playbackUnit == null)
            {
                Debug.LogWarning($"PlaybackUnitが参照できません");
                return;
            }

            var audioSource = playbackUnit.AudioSource;
            if (audioSource==null)
            {
                Debug.Log("[" + audioClip.name + " ] の再生をスキップします");
                return;
            }
            
            if(audioClip.loadState != AudioDataLoadState.Loaded) Debug.Log($"{audioClip.name} は {audioClip.loadState} 中に再生がリクエストされました");

            audioSource.volume = 1.0f;
            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();
#if UNITY_EDITOR
            // メソッドの呼び出し元がどこなのかを取得（skipFlameを2とすることで二つ前の呼び出し元に戻る）
            playbackUnit.ScriptName = GetCallerName(callerDepth);
#endif
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// メソッドの呼び出し元がどこなのかを取得（skipFlameを2とすることで二つ前の呼び出し元に戻る）
        /// </summary>
        private static string GetCallerName(int hierarchyIndex)
        {
            var caller = new StackFrame(hierarchyIndex + 1, false);
            var s = caller.GetMethod().DeclaringType?.FullName;
            var splitCodes = s.Split(".");
            s = splitCodes[splitCodes.Length - 1];
            return s;
        }
#endif
        
        private bool TryGetAudioClip<T>(T clipEnum, out AudioClip clip) where T : Enum
        {
            clip = null;
            if (!_databaseStorage.TryGet<T>(out var database))
            {
                Debug.LogError($"{typeof(T)} に対応するSoundDatabaseが見つかりませんでした");
                return false;
            }

            if (!database.TryGetClip(clipEnum, out clip))
            {
                Debug.LogError($"{clipEnum} に対応するAudioClipが見つかりませんでした");
                return false;
            }
            
            return true;
        }
        
        private PlaybackUnit RequirePlayBackUnit(bool isSe)
        {
            return isSe 
                ? sePlaybackUnitSupplier.Supply() 
                : bgmPlaybackUnitSupplier.Supply();
        }
        
        #endregion
    }
}