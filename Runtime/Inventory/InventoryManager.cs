using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class InventoryManager : Singleton<InventoryManager>, ILoggable
    {
        [SerializeField] private RuntimeInventory _playerInventory;
        [SerializeField] private List<RuntimeInventory> _registeredInventories = new();
        [SerializeField] private ItemDefinitionManager _itemDatabase;
        [SerializeField] private bool _logEnabled = true;

        public bool LogEnabled => _logEnabled;
        public RuntimeInventory PlayerInventory => _playerInventory;

        protected override void OnPostAwake()
        {
            RegisterRuntimeInventory(_playerInventory);
        }

        public void RegisterInventory(RuntimeInventory inventory)
        {
            RegisterRuntimeInventory(inventory);
        }

        public void UnregisterInventory(RuntimeInventory inventory)
        {
            if (inventory == null)
            {
                return;
            }

            _registeredInventories.Remove(inventory);
        }

        public RuntimeInventory GetInventoryByID(string inventoryID)
        {
            if (string.IsNullOrWhiteSpace(inventoryID))
            {
                return null;
            }

            foreach (var inventory in _registeredInventories)
            {
                if (inventory != null && inventory.InventoryID == inventoryID)
                {
                    return inventory;
                }
            }

            this.LogWarning($"Inventory with ID {inventoryID} not found", this);
            return null;
        }

        public bool GiveItemToInventory(string inventoryID, string itemID, int quantity = 1)
        {
            var inventory = GetInventoryByID(inventoryID);
            var itemData = GetItemData(itemID);
            if (inventory == null || itemData == null)
            {
                return false;
            }

            return inventory.AddItem(itemData, quantity);
        }

        public bool TakeItemFromInventory(string inventoryID, string itemID, int quantity = 1)
        {
            var inventory = GetInventoryByID(inventoryID);
            var itemData = GetItemData(itemID);
            if (inventory == null || itemData == null)
            {
                return false;
            }

            return inventory.RemoveItem(itemData, quantity);
        }

        public bool TransferItem(RuntimeInventory from, RuntimeInventory to, ItemData itemData, int quantity = 1)
        {
            if (from == null || to == null || itemData == null || quantity <= 0)
            {
                return false;
            }

            if (!from.ContainsItem(itemData, quantity))
            {
                return false;
            }

            if (!to.AddItem(itemData, quantity))
            {
                return false;
            }

            return from.RemoveItem(itemData, quantity);
        }

        public void DropItemInWorld(ItemData itemData, int quantity, Vector3 position)
        {
            this.LogWarning($"DropItemInWorld not implemented. Attempted to drop {quantity} of {itemData?.InternalName} at {position}", this);
        }

        public ItemData GetItemData(string itemID)
        {
            if (string.IsNullOrWhiteSpace(itemID))
            {
                return null;
            }

            var db = _itemDatabase != null ? _itemDatabase : ItemDefinitionManager.Instance;
            if (db == null)
            {
                this.LogWarning("ItemDefinitionManager is not available", this);
                return null;
            }

            if (db.TrySearchByInternalName(itemID, out var itemData))
            {
                return itemData;
            }

            return db.SearchAssetByID(itemID);
        }

        private void RegisterRuntimeInventory(RuntimeInventory inventory)
        {
            if (inventory == null || _registeredInventories.Contains(inventory))
            {
                return;
            }

            _registeredInventories.Add(inventory);
        }
    }
}
