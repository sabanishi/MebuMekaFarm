using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Editor.Sound
{
    /// <summary>
    /// AudioClipを指定するためのEnumを自動生成するためのクラス
    /// </summary>
    public class SoundTypeCreatorWindow:EditorWindow
    {
        private const string FolderPath = "Assets/Scripts/Sound/Type";
        
        [MenuItem("Tools/Sabanishi/MebuMekaFarm/Sound/Create SoundType Enum")]
        private static void Open()
        {
            var window = GetWindow<SoundTypeCreatorWindow>();
            window.titleContent = new GUIContent("SoundTypeCreatorWindow");
            window.Show();
        }

        private string _enumName = "Default";
        
        private void OnGUI()
        {
            _enumName = EditorGUILayout.TextField("Enum Name", _enumName);
            
            if(GUILayout.Button("Create"))
            {
                CreateScript();
            }
        }

        private void CreateScript()
        {
            // 命名をキャメルケースに変換
            var enumName = _enumName.Substring(0, 1).ToUpper() + _enumName.Substring(1);
            
            // enumNameの末尾に拡張子があれば削除
            if (enumName.Contains(".cs"))
            {
                enumName = enumName.Replace(".cs", "");
            }
            
            // フォルダ内に同名のファイルが存在するか確認
            if (File.Exists($"{FolderPath}/{enumName}.cs"))
            {
                // 警告ポップアップを出す
                EditorUtility.DisplayDialog("Error", "同名のファイルが既に存在します", "OK");
                return;
            }
            
            // フォルダが存在しない場合は作成
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            
            // ファイルを作成
            var builder = CreateText(enumName);
            File.WriteAllText($"{FolderPath}/{enumName}Type.cs", builder.ToString());

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            
            // 完了ポップアップを出す
            EditorUtility.DisplayDialog("Complete", "ファイルの作成が完了しました", "OK");
        }

        private StringBuilder CreateText(string enumName)
        {
            var builder = new StringBuilder();
            builder.AppendLine("namespace Sabanishi.MebuMekaFarm.Sound");
            builder.AppendLine("{");
            
            builder.Append("\t").Append($"public enum {enumName}Type").AppendLine();
            builder.Append("\t").Append("{").AppendLine();
            builder.Append("\t").Append("}").AppendLine();

            builder.Append("\t").AppendLine();
            
            builder.Append("\t").Append($"public class {enumName}SoundDatabase:SoundDatabase<{enumName}Type>").AppendLine();
            builder.Append("\t").Append("{").AppendLine();
            builder.Append("\t").Append("}").AppendLine();

            builder.Append("}");
            
            return builder;
        }
    }
}