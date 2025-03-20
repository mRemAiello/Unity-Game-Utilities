using GameUtils;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [CustomPropertyDrawer(typeof(Required))]
    public class RequiredPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox($"This field: {property.name} is required", MessageType.Error);
                GUILayout.Space(10);
            }
        }
    }
}