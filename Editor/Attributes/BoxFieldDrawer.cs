using GameUtils;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    [CustomPropertyDrawer(typeof(BoxFieldAttribute))]
    public class BoxFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Prendi il BoxFieldAttribute per avere accesso al label
            BoxFieldAttribute boxField = (BoxFieldAttribute)attribute;

            // Inizio di un gruppo di box
            EditorGUI.BeginProperty(position, label, property);

            // Offset verticale per il margine superiore del box
            position.y += 5;

            // Stile per il box (con margini e padding)
            var boxStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(10, 10, 5, 5)
            };

            // Controlla se è stato passato un titolo per il box
            if (!string.IsNullOrEmpty(boxField.Label))
            {
                // Disegna una Label per il titolo del box
                EditorGUI.LabelField(position, boxField.Label, EditorStyles.boldLabel);
                position.y += EditorGUIUtility.singleLineHeight + 5; // spazio per la label
            }

            // Inizia la sezione del box
            GUI.BeginGroup(position, GUIContent.none, boxStyle);
            position.x += 10; // padding interno per il box
            position.width -= 20;

            // Disegna la proprietà all'interno del box
            EditorGUI.PropertyField(position, property, label, true);

            // Fine del gruppo di box
            GUI.EndGroup();

            // Aggiungi padding tra i vari campi
            position.y += EditorGUI.GetPropertyHeight(property, label, true) + 10;

            // Fine del box
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Altezza della proprietà più qualche pixel extra per il padding
            return EditorGUI.GetPropertyHeight(property, label, true) + 30; // 30 per padding e margini
        }
    }
}