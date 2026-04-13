using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameUtils.Editor
{
    public class DuplicateItemIdCheckerWindow : EditorWindow
    {
        private struct DuplicateGroup
        {
            public string ID;
            public List<ItemIdentifierData> Assets;
        }

        private List<DuplicateGroup> _duplicates = new();
        private Vector2 _scrollPos;
        private bool _hasScanned;

        [MenuItem("Tools/Unity Game Utilities/Check Duplicate Item IDs")]
        public static void Open()
        {
            var window = GetWindow<DuplicateItemIdCheckerWindow>("Duplicate Item IDs");
            window.minSize = new Vector2(420, 300);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(6);

            if (GUILayout.Button("Scan Project", GUILayout.Height(30)))
            {
                Scan();
            }

            EditorGUILayout.Space(4);

            if (!_hasScanned)
            {
                EditorGUILayout.HelpBox("Press \"Scan Project\" to search for duplicate IDs.", MessageType.Info);
                return;
            }

            if (_duplicates.Count == 0)
            {
                EditorGUILayout.HelpBox("No duplicate IDs found.", MessageType.Info);
                return;
            }

            EditorGUILayout.LabelField(
                $"Found {_duplicates.Count} duplicate ID group(s):",
                EditorStyles.boldLabel);

            EditorGUILayout.Space(4);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            foreach (var group in _duplicates)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField("ID:", group.ID, EditorStyles.miniLabel);

                foreach (var asset in group.Assets)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(asset, typeof(ItemIdentifierData), false);
                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("Select", GUILayout.Width(55)))
                    {
                        Selection.activeObject = asset;
                        EditorGUIUtility.PingObject(asset);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }

            EditorGUILayout.EndScrollView();
        }

        private void Scan()
        {
            _duplicates = new List<DuplicateGroup>();
            _hasScanned = true;

            var idMap = new Dictionary<string, List<ItemIdentifierData>>();

            string[] guids = AssetDatabase.FindAssets("t:ItemIdentifierData");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ItemIdentifierData>(path);

                if (asset == null)
                    continue;

                string id = asset.ID;

                if (string.IsNullOrEmpty(id))
                    id = "<empty>";

                if (!idMap.TryGetValue(id, out var list))
                {
                    list = new List<ItemIdentifierData>();
                    idMap[id] = list;
                }

                list.Add(asset);
            }

            foreach (var kvp in idMap)
            {
                if (kvp.Value.Count > 1)
                {
                    _duplicates.Add(new DuplicateGroup
                    {
                        ID = kvp.Key,
                        Assets = kvp.Value
                    });
                }
            }

            Repaint();
        }
    }
}
