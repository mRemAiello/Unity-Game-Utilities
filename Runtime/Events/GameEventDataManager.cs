using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(-100)]
    public class GameEventDataManager : Singleton<GameEventDataManager>
    {
        [SerializeField, Group("Debug")] private string _dataFolderPath = "Assets/";
        [SerializeField, Group("Debug"), ReadOnly, TableList] private List<GameEventAssetBase> _dataList = new();

        //
        public IReadOnlyList<GameEventAssetBase> Events => _dataList;

        //
        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            LoadAssets();
            ResetAllEventData();
        }

        protected override void OnPostDestroy()
        {
            base.OnPostDestroy();

            //
            ResetAllEventData();
        }

        [Button(ButtonSizes.Medium)]
        protected void LoadAssets()
        {
#if UNITY_EDITOR
            var assetsGuid = AssetDatabase.FindAssets($"t:{typeof(GameEventAssetBase).Name}", new string[] { _dataFolderPath });
            var assetPaths = assetsGuid.Select(guid => AssetDatabase.GUIDToAssetPath(guid));

            //
            _dataList.Clear();
            foreach (var path in assetPaths)
            {
                _dataList.Add(AssetDatabase.LoadAssetAtPath<GameEventAssetBase>(path));
            }
#endif
        }

        [Button(ButtonSizes.Medium)]
        private void ResetAllEventData()
        {
            foreach (var item in Events)
            {
                item.ResetData();
            }
        }
    }
}
