using Cysharp.Threading.Tasks;
using System;

namespace Sabanishi.MebuMekaFarm.Sound
{
    public interface ISoundPlayer
    {
        
        /// <summary>
        /// T型のEnumに対応するSoundを流す準備を行う
        /// </summary>
        public void Setup<T>(SoundDatabase<T> database) where T : Enum;

        /// <summary>
        /// T型のEnumに対応するSoundの後片付けを行う
        /// </summary>
        public void Cleanup<T>() where T : Enum;
        
        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="clipEnum">識別子</param>
        /// <param name="pitch">ピッチ(省略可)</param>
        /// <param name="callerDepth">Debug用. 明らかにしたい呼び出し元クラスの深さ（PlayBase()から見た際の深さなので２以上）</param>
        public PlaybackUnit PlaySe<T>(T clipEnum, float pitch = 1.0f, int callerDepth = 2) where T : Enum;
        
        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="clipEnum">識別子</param>
        /// <param name="pitch">ピッチ(省略可)</param>
        /// <param name="callerDepth">Debug用. 明らかにしたい呼び出し元クラスの深さ（PlayBase()から見た際の深さなので２以上）</param>
        public PlaybackUnit PlayBgm<T>(T clipEnum, float pitch = 1.0f, int callerDepth = 2) where T : Enum;
        
        /// <summary>
        /// SEを流せるようになるまで待機した後に流す
        /// </summary>
        /// <param name="clipEnum">識別子</param>
        /// <param name="pitch">ピッチ(省略可)</param>
        /// <param name="callerDepth">Debug用. 明らかにしたい呼び出し元クラスの深さ（PlayBase()から見た際の深さなので２以上）</param>
        public UniTask<PlaybackUnit> PlaySeAsync<T>(T clipEnum, float pitch = 1.0f, int callerDepth = 2) where T : Enum;
        
        /// <summary>
        /// BGMを流せるようになるまで待機した後に流す
        /// </summary>
        /// <param name="clipEnum">識別子</param>
        /// <param name="pitch">ピッチ(省略可)</param>
        /// <param name="callerDepth">Debug用. 明らかにしたい呼び出し元クラスの深さ（PlayBase()から見た際の深さなので２以上）</param>
        public UniTask<PlaybackUnit> PlayBgmAsync<T>(T clipEnum, float pitch=1.0f, int callerDepth=2) where T : Enum;
        
        /// <summary>
        /// SEをループ再生する<br />
        /// 終了するためにはStop()を呼び出す
        /// </summary>
        /// <param name="clipEnum">識別子</param>
        /// <param name="crossFadeTime">フェードに掛かる時間(sec)</param>
        public PlaybackUnit PlayLoopSe<T>(T clipEnum, float crossFadeTime=0f) where T : Enum;
        
        /// <summary>
        /// BGMをループ再生する<br />
        /// 終了するためにはStop()を呼び出す
        /// </summary>
        /// <param name="clipEnum">識別子</param>
        /// <param name="crossFadeTime">フェードに掛かる時間(sec)</param>
        public PlaybackUnit PlayLoopBgm<T>(T clipEnum, float crossFadeTime=0f) where T : Enum;
        
        /// <summary>
        /// SE,BGMを停止させる
        /// </summary>
        /// <param name="playbackUnit">対象</param>
        /// <param name="fadeOutTime">フェードアウトにかかる時間</param>
        public void Stop(PlaybackUnit playbackUnit, float fadeOutTime=0f);
        
        /// <summary>
        /// ダッキング処理
        /// </summary>
        /// <param name="playbackUnit">ダッキング対象</param>
        /// <param name="fadeInTime">フェードイン(s)</param>
        /// <param name="duckingTime">フェードを含めた全体のダッキング時間(s)</param>
        /// <param name="fadeOutTime">フェードアウト(s)</param>
        public void Ducking(PlaybackUnit playbackUnit, float fadeInTime, float duckingTime, float fadeOutTime);
    }
}