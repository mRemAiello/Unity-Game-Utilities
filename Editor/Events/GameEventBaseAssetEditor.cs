using GameUtils;

namespace UnityEditor.GameUtils
{
    [CustomEditor(typeof(GameEventBaseAsset))]
    public class GameEventBaseAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            //
            serializedObject.Update();
            ShowListeners();
            serializedObject.ApplyModifiedProperties();            
        }

        private void ShowListeners()
        {
            // 
            var gameEventTarget = target as GameEventBaseAsset;
            
            //
            if (gameEventTarget == null)
                return;
            
            //
            if (gameEventTarget.Listeners == null)
                return;
            
            // 
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("--- Listeners ---");
            foreach (var listener in gameEventTarget.Listeners)
            {
                string str = string.Format("{0}: {1}", listener.Item1, listener.Item2);
                EditorGUILayout.LabelField(str);
            }
        }
    }
}