using System;
using System.Collections.Generic;
using System.Linq;
using Doneref.Collectables.CollectableType;
using UnityEditor;
using UnityEngine;

namespace Doneref.Collectables
{
    /// <summary>
    /// Defines collectables management logic.
    /// </summary>
    /*public class CollectablesManager : Singleton<CollectablesManager>, ISaveable
    {
        #region Constants
        private const string COLLECTABLES_FOLDER_PATH = "Assets/Data/Collectables";
        #endregion

        #region Fields
        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private SerializableDictionary<string, int> _collectables;

        [SerializeField, FoldoutGroup("Debug"), ReadOnly]
        private List<CollectableTypeSO> _collectableTypes;
        #endregion

        #region Events
        private Dictionary<string, Action<int>> _events;
        #endregion

        #region Properties
        public Dictionary<string, Action<int>> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public string SaveContext => "Collectables";
        #endregion


        protected override void OnPostAwake()
        {
            base.OnPostAwake();
#if UNITY_EDITOR
            InitCollectable();
#endif
            InitCollectablesCollection();
        }

        void OnEnable()
        {
            InitCollectablesCollection();
            Collectable.OnCollectItem += OnCollectItem;
        }

        void OnDisable()
        {
            ClearCollectablesCollection();
            Collectable.OnCollectItem -= OnCollectItem;
        }

        /// <summary>
        /// Updates collectable counter value when a Collector gathers a collectable.
        /// </summary>
        /// <param name="incrementValue">Collectable counter increment value.</param>
        /// <param name="type">Collectable type.</param>
        private void OnCollectItem(int incrementValue, CollectableTypeSO type)
        {
            var collectableTypeName = type.name;
            _collectables[collectableTypeName] += incrementValue;

            // Persist collectable value
            GameSaveManager.Instance.Save(SaveContext, collectableTypeName, _collectables[collectableTypeName]);

            // Update UI text
            _events[collectableTypeName].Invoke(_collectables[collectableTypeName]);

            if (type.IsWinningCondition && _collectables[collectableTypeName] >= type.MaxLimit)
                BaseGameController.Instance.WinGame();
        }

        /// <summary>
        /// Dynamically initializes <see cref="_collectables"/> by getting created <see cref="CollectableTypeSO"/> objects from the <see cref="AssetDatabase"/>.
        /// </summary>
        private void InitCollectablesCollection()
        {
            ClearCollectablesCollection();

            _collectables = new SerializableDictionary<string, int>();
            _events = new Dictionary<string, Action<int>>();

            foreach (var type in _collectableTypes)
            {
                if (_collectables.ContainsKey(type.name))
                {
                    continue;
                }

                _collectables.Add(type.name, default);
                _events.Add(type.name, default);
            }
        }

        /// <summary>
        /// Resets counters in <see cref="_collectables"/> to their default value.
        /// </summary>
        private void ClearCollectablesCollection()
        {
            if (_collectables != null)
                _collectables.Clear();
        }

#if UNITY_EDITOR
        [Button]
        private void InitCollectable()
        {
            var assetsGuid = AssetDatabase.FindAssets(
                $"t:{typeof(CollectableTypeSO)}",
                new string[] { COLLECTABLES_FOLDER_PATH }
            );
            var assetPaths = assetsGuid.Select(guid => AssetDatabase.GUIDToAssetPath(guid));

            foreach (var path in assetPaths)
            {
                _collectableTypes.Add(AssetDatabase.LoadAssetAtPath<CollectableTypeSO>(path));
            }
        }
#endif
    }*/
}
