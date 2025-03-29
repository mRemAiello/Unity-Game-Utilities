using GameUtils;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>))]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        private const float KeyValueFieldHeight = 18f;
        private const float Spacing = 2f;
        private const float ButtonWidth = 20f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty keysProperty = property.FindPropertyRelative("keys");
            float size = (keysProperty.arraySize + 2) * (KeyValueFieldHeight + Spacing);
            return size + Spacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Disegna il nome del dizionario (etichetta)
            position.height = KeyValueFieldHeight;
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Recupera le proprietà delle chiavi e dei valori
            SerializedProperty keysProperty = property.FindPropertyRelative("keys");
            SerializedProperty valuesProperty = property.FindPropertyRelative("values");

            position.y += KeyValueFieldHeight + Spacing;

            for (int i = 0; i < keysProperty.arraySize; i++)
            {
                SerializedProperty key = keysProperty.GetArrayElementAtIndex(i);
                SerializedProperty value = valuesProperty.GetArrayElementAtIndex(i);

                // Rettangoli per chiavi, valori e bottone X
                EditorGUILayout.BeginHorizontal();
                Rect keyRect = new(position.x, position.y, (position.width - ButtonWidth) / 2 - 5, KeyValueFieldHeight);
                Rect valueRect = new(position.x + (position.width / 2), position.y, (position.width - ButtonWidth) / 2 - 5, KeyValueFieldHeight);
                Rect buttonRect = new(position.x + position.width - ButtonWidth, position.y, ButtonWidth, KeyValueFieldHeight);

                // Disegna i campi chiave e valore
                EditorGUI.PropertyField(keyRect, key, GUIContent.none);
                EditorGUI.PropertyField(valueRect, value, GUIContent.none);
                EditorGUILayout.EndHorizontal();

                // Disegna il bottone X per eliminare la coppia
                if (GUI.Button(buttonRect, "X"))
                {
                    keysProperty.DeleteArrayElementAtIndex(i);
                    valuesProperty.DeleteArrayElementAtIndex(i);
                    break;
                }

                position.y += KeyValueFieldHeight + Spacing;
            }

            //
            position.y += Spacing;
            if (GUI.Button(new Rect(position.x, position.y, position.width, position.height), "Add Entry"))
            {
                keysProperty.arraySize++;
                valuesProperty.arraySize++;
            }

            //
            EditorGUI.EndProperty();
        }
    }
}