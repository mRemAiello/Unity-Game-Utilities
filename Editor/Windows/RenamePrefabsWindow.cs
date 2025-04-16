using UnityEngine;
using System.Collections.Generic;
using GameUtils;

namespace UnityEditor.GameUtils
{
    public class RenamePrefabsWindow : EditorWindow
    {
        private string _baseName = "Prefab";

        //
        private SerializedObject _serializedObject;
        private SerializedProperty _prefabsProperty;
        [SerializeField] private List<Object> _prefabs = new();

        //
        [MenuItem(Constant.MENU_NAME + "Prefab Renamer")]
        public static void ShowWindow()
        {
            GetWindow<RenamePrefabsWindow>("Rename Prefabs");
        }

        private void OnEnable()
        {
            _prefabs ??= new List<Object>();

            //
            _serializedObject = new SerializedObject(this);
            _prefabsProperty = _serializedObject.FindProperty("_prefabs");
        }

        private void OnGUI()
        {
            //
            _baseName = EditorGUILayout.TextField("Base Name", _baseName);

            //
            EditorGUILayout.Separator();

            // 
            _serializedObject.Update();
            EditorGUILayout.PropertyField(_prefabsProperty, new GUIContent("Prefabs List"), true);
            _serializedObject.ApplyModifiedProperties();

            //
            EditorGUILayout.Space();

            //
            if (GUILayout.Button("Rename Prefabs"))
            {
                RenamePrefabs();
            }
        }

        private void RenamePrefabs()
        {
            if (string.IsNullOrEmpty(_baseName))
            {
                Debug.LogError("Base name cannot be empty.");
                return;
            }

            for (int i = 0; i < _prefabs.Count; i++)
            {
                if (_prefabs[i] != null)
                {
                    string newName = _baseName + " " + (i + 1);
                    string assetPath = AssetDatabase.GetAssetPath(_prefabs[i]);
                    AssetDatabase.RenameAsset(assetPath, newName);
                    Debug.Log($"Renamed {_prefabs[i].name} to {newName}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}