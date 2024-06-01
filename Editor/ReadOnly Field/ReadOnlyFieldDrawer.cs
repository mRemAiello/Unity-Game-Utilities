using UnityEngine;
using UnityEditor;
using GameUtils;

namespace UnityEditor.GameUtils
{
    [CustomPropertyDrawer(typeof(ReadOnlyField))]
    public class ReadOnlyFieldDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // avoid the field being completely hidden from inspector
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}