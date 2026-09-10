using System.Collections.Generic;
using Sabanishi.MebuMekaFarm.Sound;
using UnityEditor;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Editor.Sound
{
    public partial class SoundDebugWindow : DebugWindowBase<SoundDebugWindow>
    {
        [MenuItem("Tools/Sabanishi/MebuMekaFarm/Sound/DebugWindow")]
        public static void Open()
        {
            GetWindow<SoundDebugWindow>(ObjectNames.NicifyVariableName(nameof(SoundDebugWindow)));
        }

        protected override void OnEnableInternal()
        {
            var storage = SoundDebugDataStorage.Instance;
            var databases = SearchSoundDatabase();

            AddPanel(new SoundDebugPanel(storage, databases));

            maxSize = new Vector2(800, 800);
        }

        private List<(string, ISoundDatabase)> SearchSoundDatabase()
        {
            var guids = AssetDatabase.FindAssets("t:ScriptableObject");
            var databaseTuples = new List<(string, ISoundDatabase)>();

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (asset is ISoundDatabase database)
                {
                    databaseTuples.Add((path, database));
                }
            }

            return databaseTuples;
        }
    }
}