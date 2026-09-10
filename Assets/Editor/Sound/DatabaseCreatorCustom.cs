using System;
using Sabanishi.MebuMekaFarm.Sound;
using UnityEditor;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Editor.Sound
{
    [CustomEditor(typeof(DatabaseCreator))]
    public class DatabaseCreatorCustom:UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Create"))
            {
                var creator = target as DatabaseCreator;
                if (creator == null) return;
                
                var type = creator.Type;
                var assetName = creator.AssetName;
                var savePath = AssetDatabase.GetAssetPath(creator.SavePathDirectory);
                
                // Type型のSoundDatabaseを作成
                var database = ScriptableObject.CreateInstance(type);
                
                // savePathがフォルダ出ない時
                if (!AssetDatabase.IsValidFolder(savePath))
                {
                    savePath = savePath.Substring(0, savePath.LastIndexOf("/", StringComparison.Ordinal));
                }

                var saveName = $"{savePath}/{assetName}.asset";
                
                // フォルダ内に同じ名前のファイルが存在する場合、警告ポップアップを出す
                if (AssetDatabase.LoadAssetAtPath(saveName, type) != null)
                {
                    // 警告ポップアップを開く
                    EditorUtility.DisplayDialog("Error", "同じ名前のファイルが存在します", "OK");
                    return;
                }
                
                AssetDatabase.CreateAsset(database, saveName);
                
                // 完了ポップアップを開く
                EditorUtility.DisplayDialog("Complete", $"データベースの作成が完了しました:{saveName}", "OK");
            }
        }
    }
}