using System;
using TriInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GameUtils
{
    [DisallowMultipleComponent]
    [DeclareBoxGroup("Debug")]
    public sealed class PersistentID : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("Debug")] private bool _logEnabled = false;
        [SerializeField, ReadOnly, Group("Debug")] private string _id;

        // Expose ID as read-only property
        public string ID => _id;
        public bool LogEnabled => _logEnabled;

        //
        private void Awake()
        {
            if (string.IsNullOrEmpty(_id))
            {
                this.LogWarning($"PersistentID is missing an ID from {name}. This should never happen. Please generate a new ID in the editor.");
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            // Do not generate IDs on prefab assets
            if (PrefabUtility.IsPartOfPrefabAsset(this))
                return;

            // Generate if missing
            if (string.IsNullOrEmpty(_id))
            {
                GenerateNewID();
                EditorUtility.SetDirty(this);
            }
            else
            {
                EnsureUnique();
            }
        }

        private void GenerateNewID()
        {
            _id = Guid.NewGuid().ToString();
        }

        private void EnsureUnique()
        {
            PersistentID[] all = FindObjectsByType<PersistentID>();
            foreach (PersistentID pid in all)
            {
                if (pid == this)
                    continue;

                // If we find a duplicate ID, generate a new one and mark the scene dirty for saving.
                if (pid.ID == _id)
                {
                    this.LogWarning($"Duplicate ID detected on {name}. Regenerating.");

                    // Generate a new ID and mark the scene dirty for saving.
                    GenerateNewID();
                    EditorUtility.SetDirty(this);
                    EditorSceneManager.MarkSceneDirty(gameObject.scene);
                    break;
                }
            }
        }
#endif
    }
}