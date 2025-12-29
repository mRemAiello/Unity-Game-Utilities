using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("events", Title = "Events")]
    [DeclareBoxGroup("settings", Title = "Settings")]
    public class RuntimeInventory : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("settings")] private string _inventoryID;
        [SerializeField, Min(0), Group("settings")] private int _capacity = 20;
        [SerializeField, Group("settings")] private bool _allowAutoStacking = true;
        [SerializeField, Group("settings")] private bool _allowOverflow;
        [SerializeField, Group("events")] private InventoryItemEvent _onItemAddedEvent;
        [SerializeField, Group("events")] private InventoryItemEvent _onItemRemovedEvent;
        [SerializeField, Group("events")] private InventorySlotSwapEvent _onSlotsSwappedEvent;
        [SerializeField, Group("debug")] private bool _logEnabled = true;
        [SerializeField, Group("debug"), ReadOnly] private List<InventorySlot> _slots = new();

        //
        public bool LogEnabled => _logEnabled;
        public string InventoryID => _inventoryID;

        //
        private void Awake()
        {
            EnsureSlots();
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(_inventoryID))
            {
                _inventoryID = Guid.NewGuid().ToString();
            }
        }

        public bool AddItem(ItemData definition, int quantity = 1)
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
                    _onItemAddedEvent?.Invoke(new InventoryItemEventArgs(this, stackableItem, toAdd));
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

                    _onItemAddedEvent?.Invoke(new InventoryItemEventArgs(this, newItem, toAdd));
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

        public bool RemoveItem(ItemData definition, int quantity = 1)
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
                _onItemRemovedEvent?.Invoke(new InventoryItemEventArgs(this, item, toRemove));

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

        public bool ContainsItem(ItemData definition, int quantity = 1)
        {
            return GetItemCount(definition) >= quantity;
        }

        public int GetItemCount(ItemData definition)
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

        public bool TryGetFirstStackableSlot(ItemData definition, out InventorySlot slot)
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

            _onSlotsSwappedEvent?.Invoke(new InventorySlotSwapEventArgs(this, indexA, indexB));
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
                _onSlotsSwappedEvent?.Invoke(new InventorySlotSwapEventArgs(this, fromIndex, toIndex));
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
