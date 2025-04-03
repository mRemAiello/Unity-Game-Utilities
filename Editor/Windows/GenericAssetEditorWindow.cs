using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    public class GenericAssetEditorWindow<T> : EditorWindow where T : Object
    {
        private Vector2 _scrollPosition;
        private List<T> _assets;
        private T _selectedAsset;

        private void OnEnable()
        {
            //
            LoadAssets();
        }

        private void LoadAssets()
        {
            //
            string[] assetGuids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            _assets = new List<T>();

            //
            foreach (string guid in assetGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    _assets.Add(asset);
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            // 
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 3));
            GUILayout.Label($"List of {typeof(T).Name} assets", EditorStyles.boldLabel);

            //
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (T asset in _assets)
            {
                if (GUILayout.Button(asset.name))
                {
                    //
                    _selectedAsset = asset;
                    EditorGUIUtility.PingObject(asset);
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            // 
            EditorGUILayout.BeginVertical();
            GUILayout.Label("Inspector", EditorStyles.boldLabel);

            if (_selectedAsset != null)
            {
                //
                Editor editor = Editor.CreateEditor(_selectedAsset);
                editor.OnInspectorGUI();
            }
            else
            {
                GUILayout.Label("Select an asset from the list.");
            }

            //
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}