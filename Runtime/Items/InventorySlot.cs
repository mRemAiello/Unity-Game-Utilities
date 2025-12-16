using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private InventoryItem _item;

        public InventoryItem Item => _item;
        public bool IsEmpty => _item == null || _item.Definition == null || _item.Quantity <= 0;

        public void SetItem(InventoryItem item)
        {
            _item = item;
        }

        public void Clear()
        {
            _item = null;
        }
    }
}
