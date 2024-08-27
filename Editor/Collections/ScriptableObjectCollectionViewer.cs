using GameUtils;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    public class ScriptableObjectCollectionViewer : EditorWindow
    {
        private ScriptableObjectCollection _collection;
        private ScriptableObject _selectedElement;
        private Vector2 _scrollPosition;

        [MenuItem("Window/ScriptableCollection Editor")]
        public static void ShowWindow()
        {
            GetWindow<ScriptableObjectCollectionViewer>("ScriptableCollection Editor");
        }

        private void OnGUI()
        {
            _collection = (ScriptableObjectCollection)EditorGUILayout.ObjectField("File", _collection, typeof(ScriptableObjectCollection), false);

            //
            if (_collection == null)
                return;
            
            //
            EditorGUILayout.Space();

            //
            EditorGUILayout.BeginHorizontal();

            // 
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.3f));
            EditorGUILayout.LabelField("Elements", EditorStyles.boldLabel);

            //
            using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.Width(200)))
            {
                _scrollPosition = scrollView.scrollPosition;

                foreach (var elemento in _collection.Items)
                {
                    if (GUILayout.Button(elemento.name))
                    {
                        _selectedElement = elemento;
                    }
                }
            }

            //
            EditorGUILayout.EndVertical();

            //
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.7f));
            if (_selectedElement != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Edit Element", EditorStyles.boldLabel);
                Editor editor = Editor.CreateEditor(_selectedElement);
                editor.OnInspectorGUI();
            }
            else
            {
                EditorGUILayout.LabelField("Select an element.");
            }

            //
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}