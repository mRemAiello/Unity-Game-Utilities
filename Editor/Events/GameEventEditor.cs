using GameUtils;
using UnityEditor;

namespace UnityEditor.GameUtils
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 
            base.OnInspectorGUI();

            // 
            EditorGUILayout.LabelField("--- Listeners ---");

            // 
            var gameEvent = (target as GameEvent);
            if (gameEvent != null)
            {
                foreach (GameEventListener singleEvent in gameEvent.Events)
                {
                    EditorGUILayout.LabelField(singleEvent.gameObject.name);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}