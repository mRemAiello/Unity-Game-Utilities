using UnityEngine;
using System.Collections.Generic;
using GameUtils;

namespace UnityEditor.GameUtils
{
    public class UniqueIDCheckerWindow : EditorWindow
    {
        private List<UniqueID> _uniqueIDObjects = new();
        private Dictionary<string, List<UniqueID>> _duplicateIDs = new();

        //
        [MenuItem("Tools/Game Utils/Unique ID Checker")]
        public static void ShowWindow()
        {
            GetWindow<UniqueIDCheckerWindow>("Unique ID Checker");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Search for Duplicate IDs"))
            {
                SearchForDuplicateIDs();
            }

            GUILayout.Space(20);

            if (_duplicateIDs.Count > 0)
            {
                GUILayout.Label("Duplicate IDs found:");

                foreach (var entry in _duplicateIDs)
                {
                    GUILayout.Label($"ID: {entry.Key} - Count: {entry.Value.Count}");
                    foreach (var uniqueID in entry.Value)
                    {
                        GUILayout.Label($"  - {AssetDatabase.GetAssetPath(uniqueID)}");
                    }
                }
            }
            else
            {
                GUILayout.Label("No duplicate IDs found.");
            }
        }

        private void SearchForDuplicateIDs()
        {
            _uniqueIDObjects.Clear();
            _duplicateIDs.Clear();

            // Find all ScriptableObjects of type UniqueID
            string[] guids = AssetDatabase.FindAssets("t:UniqueID");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UniqueID uniqueID = AssetDatabase.LoadAssetAtPath<UniqueID>(path);
                if (uniqueID != null)
                {
                    _uniqueIDObjects.Add(uniqueID);
                }
            }

            // Check for duplicates
            Dictionary<string, List<UniqueID>> idDictionary = new();
            foreach (UniqueID obj in _uniqueIDObjects)
            {
                if (!idDictionary.ContainsKey(obj.ID))
                {
                    idDictionary[obj.ID] = new List<UniqueID>();
                }
                idDictionary[obj.ID].Add(obj);
            }

            // Store duplicate IDs
            foreach (var entry in idDictionary)
            {
                if (entry.Value.Count > 1)
                {
                    _duplicateIDs[entry.Key] = entry.Value;
                }
            }

            // Refresh the window to display results
            Repaint();
        }
    }
}