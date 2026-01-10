using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DefaultExecutionOrder(-100)]
    public abstract class GenericDataManager<T1, T2> : MonoBehaviour, ILoggable where T1 : GenericDataManager<T1, T2> where T2 : ItemIdentifierData
    {
        [SerializeField, Group("debug")] private bool _logEnabled = true;
        [SerializeField, Group("debug")] private string _dataFolderPath = "Assets/";
        [SerializeField, Group("debug"), ReadOnly, TableList] private List<T2> _dataList = new();

        //
        public bool LogEnabled => _logEnabled;

        //
        public static T1 Instance { get; protected set; }
        public static bool InstanceExists => Instance != null;
        public IReadOnlyList<T2> Items => _dataList;

        //
        protected void Awake()
        {
            if (InstanceExists)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = (T1)this;
                LoadAssets();
                OnPostAwake();
            }
        }

        protected void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                OnPostDestroy();
            }
        }

        public bool TrySearchAssetByID(string id, out T2 result)
        {
            result = default;
            foreach (var element in _dataList)
            {
                if (element.ID.Equals(id))
                {
                    result = element;
                    return true;
                }
            }

            return false;
        }

        public T2 SearchAssetByID(string id)
        {
            if (TrySearchAssetByID(id, out var result))
            {
                return result;
            }

            this.LogError($"Asset with ID '{id}' not found.");
            return null;
        }

        public T SearchAsset<T>() where T : T2
        {
            foreach (var item in _dataList)
            {
                if (item is T data)
                {
                    return data;
                }
            }

            this.LogError($"No asset of type {typeof(T).Name} found.");
            return null;
        }

        [Button]
        protected void LoadAssets()
        {
#if UNITY_EDITOR
            var assetsGuid = AssetDatabase.FindAssets($"t:{typeof(T2)}", new string[] { _dataFolderPath });
            var assetPaths = assetsGuid.Select(guid => AssetDatabase.GUIDToAssetPath(guid));

            //
            _dataList.Clear();
            foreach (var path in assetPaths)
            {
                _dataList.Add(AssetDatabase.LoadAssetAtPath<T2>(path));
            }
#endif
        }

        //
        public bool HasAsset<T>() where T : T2 => _dataList.Any(x => x is T);
        protected virtual void OnPostAwake() { }
        protected virtual void OnPostDestroy() { }
    }
}