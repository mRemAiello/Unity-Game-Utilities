using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class RuntimeInventory : MonoBehaviour, ILoggable
    {
        [SerializeField] private string _inventoryId;
        [SerializeField, Min(0)] private int _capacity = 20;
        [SerializeField] private List<InventorySlot> _slots = new();
        [SerializeField] private bool _allowAutoStacking = true;
        [SerializeField] private bool _allowOverflow;
        [SerializeField] private bool _logEnabled = true;

        public event Action<InventoryItem> OnItemAdded;
        public event Action<InventoryItem> OnItemRemoved;
        public event Action<int, int> OnSlotsSwapped;

        public bool LogEnabled => _logEnabled;
        public string InventoryId => _inventoryId;

        private void Awake()
        {
            EnsureSlots();
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_inventoryId))
            {
                _inventoryId = Guid.NewGuid().ToString();
            }
        }

        public bool AddItem(ItemDefinition definition, int quantity = 1)
        {
            if (definition == null || quantity <= 0)
            {
                return false;
            }

            EnsureSlots();

            int remaining = quantity;
            while (remaining > 0)
            {
                if (_allowAutoStacking && TryGetFirstStackableSlot(definition, out var stackableSlot))
                {
                    var stackableItem = stackableSlot.Item;
                    int canAdd = stackableItem.MaxStackSize - stackableItem.Quantity;
                    int toAdd = Mathf.Min(remaining, canAdd);
                    stackableItem.AddQuantity(toAdd);
                    remaining -= toAdd;
                    OnItemAdded?.Invoke(stackableItem);
                    continue;
                }

                if (TryGetFirstEmptySlot(out var emptySlot))
                {
                    var newItem = new InventoryItem(definition, 0);
                    emptySlot.SetItem(newItem);

                    int maxForNewItem = newItem.MaxStackSize > 0 ? newItem.MaxStackSize : remaining;
                    int toAdd = Mathf.Min(remaining, maxForNewItem);
                    newItem.SetQuantity(toAdd);
                    remaining -= toAdd;

                    OnItemAdded?.Invoke(newItem);
                    continue;
                }

                break;
            }

            bool success = remaining == 0;
            if (!success && !_allowOverflow)
            {
                this.LogWarning($"Not enough space to add {remaining} of {definition.InternalName}", this);
            }

            return success || _allowOverflow;
        }

        public bool RemoveItem(ItemDefinition definition, int quantity = 1)
        {
            if (definition == null || quantity <= 0)
            {
                return false;
            }

            EnsureSlots();

            int remaining = quantity;
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty || slot.Item.Definition != definition)
                {
                    continue;
                }

                var item = slot.Item;
                int toRemove = Mathf.Min(remaining, item.Quantity);
                item.RemoveQuantity(toRemove);
                remaining -= toRemove;
                OnItemRemoved?.Invoke(item);

                if (item.Quantity <= 0)
                {
                    slot.Clear();
                }

                if (remaining <= 0)
                {
                    break;
                }
            }

            bool success = remaining == 0;
            if (!success)
            {
                this.LogWarning($"Not enough quantity to remove {quantity} of {definition.InternalName}", this);
            }

            return success;
        }

        public bool ContainsItem(ItemDefinition definition, int quantity = 1)
        {
            return GetItemCount(definition) >= quantity;
        }

        public int GetItemCount(ItemDefinition definition)
        {
            if (definition == null)
            {
                return 0;
            }

            EnsureSlots();

            int count = 0;
            foreach (var slot in _slots)
            {
                if (!slot.IsEmpty && slot.Item.Definition == definition)
                {
                    count += slot.Item.Quantity;
                }
            }

            return count;
        }

        public InventorySlot GetSlot(int index)
        {
            EnsureSlots();
                return index >= 0 && index < _slots.Count ? _slots[index] : null;
        }

        public bool TryGetFirstEmptySlot(out InventorySlot slot)
        {
            EnsureSlots();
            foreach (var inventorySlot in _slots)
            {
                if (inventorySlot.IsEmpty)
                {
                    slot = inventorySlot;
                    return true;
                }
            }

            slot = null;
            return false;
        }

        public bool TryGetFirstStackableSlot(ItemDefinition definition, out InventorySlot slot)
        {
            EnsureSlots();
            foreach (var inventorySlot in _slots)
            {
                if (!inventorySlot.IsEmpty && inventorySlot.Item.Definition == definition)
                {
                    if (inventorySlot.Item.Quantity < inventorySlot.Item.MaxStackSize)
                    {
                        slot = inventorySlot;
                        return true;
                    }
                }
            }

            slot = null;
            return false;
        }

        public void SwapSlots(int indexA, int indexB)
        {
            EnsureSlots();
            if (indexA == indexB || !IsValidIndex(indexA) || !IsValidIndex(indexB))
            {
                return;
            }

            var temp = _slots[indexA].Item;
            _slots[indexA].SetItem(_slots[indexB].Item);
            _slots[indexB].SetItem(temp);

            OnSlotsSwapped?.Invoke(indexA, indexB);
        }

        public void MoveItem(int fromIndex, int toIndex)
        {
            EnsureSlots();
            if (!IsValidIndex(fromIndex) || !IsValidIndex(toIndex) || fromIndex == toIndex)
            {
                return;
            }

            var fromSlot = _slots[fromIndex];
            var toSlot = _slots[toIndex];

            if (fromSlot.IsEmpty)
            {
                return;
            }

            if (toSlot.IsEmpty)
            {
                toSlot.SetItem(fromSlot.Item);
                fromSlot.Clear();
                OnSlotsSwapped?.Invoke(fromIndex, toIndex);
                return;
            }

            SwapSlots(fromIndex, toIndex);
        }

        private void EnsureSlots()
        {
            if (_capacity < 0)
            {
                _capacity = 0;
            }

            while (_slots.Count < _capacity)
            {
                _slots.Add(new InventorySlot());
            }

            if (_slots.Count > _capacity)
            {
                _slots.RemoveRange(_capacity, _slots.Count - _capacity);
            }
        }

        private bool IsValidIndex(int index) => index >= 0 && index < _slots.Count;
    }
}
