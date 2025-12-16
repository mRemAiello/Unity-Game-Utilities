using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    public abstract class GenericDatabase<TAsset> : ScriptableObject, ILoggable where TAsset : UniqueID
    {
        [SerializeField, Group("debug")] private bool _logEnabled = true;
        [SerializeField, Group("debug"), TableList] private List<TAsset> _items = new();

        private Dictionary<string, TAsset> _itemsById = new();

        //
        public bool LogEnabled => _logEnabled;
        public IReadOnlyList<TAsset> Items => _items;

        protected virtual void OnEnable()
        {
            RefreshLookups();
        }

        protected virtual void OnValidate()
        {
            RefreshLookups();
        }

        private void RefreshLookups()
        {
            _itemsById = _items
                .Where(item => item != null)
                .GroupBy(item => item.ID)
                .ToDictionary(group => group.Key, group => group.First());

            OnPostRefresh();
        }

        protected virtual void OnPostRefresh() { }

        public bool TrySearchByID(string id, out TAsset result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            return _itemsById.TryGetValue(id, out result);
        }

        public TAsset SearchByID(string id)
        {
            if (TrySearchByID(id, out var result))
            {
                return result;
            }

            this.LogError($"Asset with ID '{id}' not found.");
            return null;
        }
    }
}
