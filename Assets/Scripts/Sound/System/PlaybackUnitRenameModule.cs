using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sabanishi.MebuMekaFarm.Sound
{
    public class PlaybackUnitRenameModule: MonoBehaviour
    {
        [SerializeField, Header("連番名")] private string rename;
        [SerializeField, Header("プレフィックス")] private string prefix;

        public void Rename()
        {
#if UNITY_EDITOR
            int i = 0;
            
            foreach (Transform child in transform.GetComponentsInChildren<Transform>())
            {
                if (i == 0)
                {
                    i++;
                    continue;
                }
                Undo.RecordObject(child.gameObject, "child rename");
                child.gameObject.name = rename + prefix + i;
                i++;
            }
#endif
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(PlaybackUnitRenameModule))]
    public class RenameModuleEditor : UnityEditor.Editor
    {
        private PlaybackUnitRenameModule _targetScript;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_targetScript == null)
            {
                _targetScript = target as PlaybackUnitRenameModule;
            }

            if (GUILayout.Button("子オブジェクトを全リネーム"))
            {
                if (_targetScript == null) return;
                _targetScript.Rename();
            }
        }
    }
#endif
}