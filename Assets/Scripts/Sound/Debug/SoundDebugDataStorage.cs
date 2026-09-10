using System.Collections.Generic;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Sound
{
    public class SoundDebugDataStorage
    {
        // SE,BGMの2つ
        private const int DataTypeCount = 2;
        
        private static SoundDebugDataStorage _instance;

        public static SoundDebugDataStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SoundDebugDataStorage();
                    for (int i = 0; i < DataTypeCount; i++)
                    {
                        _instance._playBackUnitDataList.Add(new List<PlaybackUnitDebugData>());
                    }
                }

                return _instance;
            }
        }
        
        private readonly List<List<PlaybackUnitDebugData>> _playBackUnitDataList = new List<List<PlaybackUnitDebugData>>(DataTypeCount);
        private readonly List<VariableData> _variableDataList = new List<VariableData>();
        public IReadOnlyList<PlaybackUnitDebugData> GetPlaybackUnitDataList(int category)
        {
            if (category >= DataTypeCount)
            {
                Debug.LogError($"categoryが不正です:{category}");
                return null;
            }

            return _playBackUnitDataList[category];
        }
        
        public IReadOnlyList<VariableData> GetVariableDataList()
        {
            return _variableDataList;
        }

        /// <summary>
        /// PlaybackUnitの情報を更新する
        /// </summary>
        public void UpdatePlaybackUnitData(int category, int id, string name, string clipName, float playingTimeRatio,
            bool playing, float volume, bool loopUse, string scriptName)
        {
            if (category >= DataTypeCount)
            {
                Debug.LogError($"categoryが不正です:{category}");
                return;
            }

            //リスト内のデータが足りない場合は追加
            var categoryList = _playBackUnitDataList[category];
            while (categoryList.Count <= id)
            {
                categoryList.Add(new PlaybackUnitDebugData());
            }
            
            var data = categoryList[id];
            data.name = name;
            data.clipName = clipName;
            data.playingTimeRatio = playingTimeRatio;
            data.playing = playing;
            data.volume = volume;
            data.loopUse = loopUse;
            data.scriptName = scriptName;
        }
        
        /// <summary>
        /// 変数を視覚化するための情報を更新する<br />
        /// 変数をPrintDebugしたい時に使ってください
        /// </summary>
        public void UpdateVariableData(int index, string name, string value)
        {
            //リスト内のデータが足りない場合は追加
            while (_variableDataList.Count <= index)
            {
                _variableDataList.Add(new VariableData());
            }
            
            var data = _variableDataList[index];
            data.name = name;
            data.value = value;
        }
    }
}