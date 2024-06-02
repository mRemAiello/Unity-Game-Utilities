using GameUtils;
using UnityEditor;

namespace UnityEditor.GameUtils
{
    [CustomEditor(typeof(GameEventBaseAsset))]
    public class GameEventBaseAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 
            base.OnInspectorGUI();

            // 
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("--- Listeners ---");

            // 
            var listeners = (target as GameEventBaseAsset);
            if (listeners != null)
            {
                foreach (var listener in listeners.Listeners)
                {
                    string str = string.Format("{0}: {1}", listener.Item1, listener.Item2);
                    EditorGUILayout.LabelField(str);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}