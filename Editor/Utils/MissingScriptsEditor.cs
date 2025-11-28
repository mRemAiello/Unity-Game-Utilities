using System.Collections.Generic;
using GameUtils;
using UnityEditor;
using UnityEngine;

namespace SimblendTools
{
    public class MissingScriptsEditor : EditorWindow
    {
        private Vector2 _scrollPosWindow = Vector2.zero;
        private Vector2 _scrollPosSceneMissingScripts = Vector2.zero;
        private Vector2 _scrollPosPrefabMissingScripts = Vector2.zero;
        private readonly List<GameObject> _prefabsWithMissingScripts = new();
        private readonly List<GameObject> _objectsWithMissingScripts = new();
        private bool _removedMissingScripts = false;

        //
        [MenuItem(GameUtilsMenuConstants.MENU_NAME + "Missing Scripts")]
        public static void ShowWindow()
        {
            GetWindow<MissingScriptsEditor>("Remove Missing Scripts");
        }

        private void OnGUI()
        {

            GUILayout.Label("Remove missing scripts from prefabs and scenes");
            GUILayout.Space(30);

            _scrollPosWindow = EditorGUILayout.BeginScrollView(_scrollPosWindow);

            if (GUILayout.Button("Find prefabs with Missing Scripts"))
            {
                FindObjectsWithMissingScriptsInPrefabs();
            }

            EditorGUILayout.Space();

            if (_prefabsWithMissingScripts.Count > 0)
            {
                _scrollPosPrefabMissingScripts = EditorGUILayout.BeginScrollView(_scrollPosPrefabMissingScripts, GUILayout.Height(150));
                List<GameObject> objectsToRemoveScripts = new List<GameObject>(_prefabsWithMissingScripts);

                foreach (var obj in objectsToRemoveScripts)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                    if (GUILayout.Button("Remove Missing Scripts"))
                    {
                        RemoveMissingScripts(obj);
                        _prefabsWithMissingScripts.Remove(obj);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            if (_prefabsWithMissingScripts.Count > 0)
            {
                if (GUILayout.Button("Remove All"))
                {
                    RemoveAllMissingScripts(_prefabsWithMissingScripts);
                }
            }

            EditorGUILayout.Space();

            GUILayout.Label("Make sure the scene is open");

            if (GUILayout.Button("Find missing scripts in this scene"))
            {
                FindObjectsWithMissingScriptsInScenes();
            }

            if (_objectsWithMissingScripts.Count > 0)
            {
                _scrollPosSceneMissingScripts = EditorGUILayout.BeginScrollView(_scrollPosSceneMissingScripts, GUILayout.Height(150));
                List<GameObject> objectsToRemoveScripts = new List<GameObject>(_objectsWithMissingScripts);

                foreach (var obj in objectsToRemoveScripts)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
                    if (GUILayout.Button("Remove Missing Scripts"))
                    {
                        RemoveMissingScripts(obj);
                        _objectsWithMissingScripts.Remove(obj);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.Space();
            if (_objectsWithMissingScripts.Count > 0)
            {
                if (GUILayout.Button("Remove All"))
                {
                    RemoveAllMissingScripts(_objectsWithMissingScripts);
                }
            }
            if (_removedMissingScripts)
            {
                GUILayout.Space(20);
                EditorGUILayout.HelpBox("Removed missing scripts", MessageType.Info);
            }
            GUILayout.Space(10);
            GUILayout.Label("Simblend");

            EditorGUILayout.EndScrollView();
        }

        private void FindObjectsWithMissingScriptsInPrefabs()
        {
            _removedMissingScripts = false;

            _prefabsWithMissingScripts.Clear();

            string[] allPrefabPaths = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in allPrefabPaths)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                FindMissingScriptsInPrefab(prefab);
            }
        }

        private void FindObjectsWithMissingScriptsInScenes()
        {
            _removedMissingScripts = false;

            _objectsWithMissingScripts.Clear();

            foreach (var gameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                FindMissingScriptsInGameObject(gameObject);
            }
        }

        private void FindMissingScriptsInPrefab(GameObject obj)
        {
            Component[] components = obj.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null)
                {
                    _prefabsWithMissingScripts.Add(obj);
                    break;
                }
            }

            foreach (Transform transform in obj.transform)
            {
                FindMissingScriptsInGameObject(transform.gameObject, isPrefab: true);
            }
        }

        private void FindMissingScriptsInGameObject(GameObject obj, bool isPrefab = false)
        {
            Component[] components = obj.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null)
                {
                    _objectsWithMissingScripts.Add(obj);
                    break;
                }
            }

            foreach (Transform transform in obj.transform)
            {
                FindMissingScriptsInGameObject(transform.gameObject);
            }
        }

        private void RemoveMissingScripts(GameObject obj)
        {
            if (GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj) != 0)
            {
                _removedMissingScripts = true;
            }

            foreach (Transform transform in obj.transform)
            {
                RemoveMissingScripts(transform.gameObject);
            }
        }

        private void RemoveAllMissingScripts(List<GameObject> objList)
        {
            List<GameObject> objectsToRemove = new List<GameObject>();
            foreach (var obj in objList)
            {
                if (obj != null)
                {
                    if (GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj) != 0)
                    {
                        _removedMissingScripts = true;
                    }

                    foreach (Transform transform in obj.transform)
                    {
                        RemoveMissingScripts(transform.gameObject);
                    }
                    objectsToRemove.Add(obj);
                }
            }

            if (objectsToRemove.Count > 0)
            {
                objectsToRemove.Clear();
                objList.Clear();
            }
        }
    }
}