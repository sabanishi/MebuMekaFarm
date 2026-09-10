using Sabanishi.MebuMekaFarm.Sound;
using UnityEditor;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.Editor.Sound
{
    /// <summary>
    /// SoundDatabaseを自動生成するためのウィンドウ
    /// </summary>
    public class DatabaseCreatorWindow:EditorWindow
    {
        [MenuItem("Tools/Sabanishi/MebuMekaFarm/Sound/Create Database")]
        private static void Open()
        {
            var window = GetWindow<DatabaseCreatorWindow>();
            window.titleContent = new GUIContent("Create Sound Database");
            window.Show();
        }
        
        private UnityEditor.Editor _editor;
        private ScriptableObject _target;

        private void OnGUI()
        {
            if (EditorApplication.isPlaying) return;
            
            if (_editor == null || _target == null)
            {
                _target = ScriptableObject.CreateInstance<DatabaseCreator>();
                _editor = UnityEditor.Editor.CreateEditor(_target);
            }
            
            if(_editor != null)
            {
                _editor.OnInspectorGUI();
            }
        }
    }
}