using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class InventoryManager : MonoBehaviour, ILoggable
    {
        [SerializeField] private RuntimeInventory _playerInventory;
        [SerializeField] private List<RuntimeInventory> _registeredInventories = new();
        [SerializeField] private ItemDefinitionManager _itemDatabase;
        [SerializeField] private bool _logEnabled = true;

        public static InventoryManager Instance { get; private set; }

        public bool LogEnabled => _logEnabled;
        public RuntimeInventory PlayerInventory => _playerInventory;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            RegisterRuntimeInventory(_playerInventory);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
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

        public RuntimeInventory GetInventoryById(string inventoryId)
        {
            if (string.IsNullOrWhiteSpace(inventoryId))
            {
                return null;
            }

            foreach (var inventory in _registeredInventories)
            {
                if (inventory != null && inventory.InventoryId == inventoryId)
                {
                    return inventory;
                }
            }

            this.LogWarning($"Inventory with id {inventoryId} not found", this);
            return null;
        }

        public bool GiveItemToInventory(string inventoryId, string itemId, int quantity = 1)
        {
            var inventory = GetInventoryById(inventoryId);
            var definition = GetItemDefinition(itemId);
            if (inventory == null || definition == null)
            {
                return false;
            }

            return inventory.AddItem(definition, quantity);
        }

        public bool TakeItemFromInventory(string inventoryId, string itemId, int quantity = 1)
        {
            var inventory = GetInventoryById(inventoryId);
            var definition = GetItemDefinition(itemId);
            if (inventory == null || definition == null)
            {
                return false;
            }

            return inventory.RemoveItem(definition, quantity);
        }

        public bool TransferItem(RuntimeInventory from, RuntimeInventory to, ItemData definition, int quantity = 1)
        {
            if (from == null || to == null || definition == null || quantity <= 0)
            {
                return false;
            }

            if (!from.ContainsItem(definition, quantity))
            {
                return false;
            }

            if (!to.AddItem(definition, quantity))
            {
                return false;
            }

            return from.RemoveItem(definition, quantity);
        }

        public void DropItemInWorld(ItemData definition, int quantity, Vector3 position)
        {
            this.LogWarning($"DropItemInWorld not implemented. Attempted to drop {quantity} of {definition?.InternalName} at {position}", this);
        }

        public ItemData GetItemDefinition(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                return null;
            }

            var db = _itemDatabase != null ? _itemDatabase : ItemDefinitionManager.Instance;
            if (db == null)
            {
                this.LogWarning("ItemDefinitionManager is not available", this);
                return null;
            }

            if (db.TrySearchByInternalName(itemId, out var definition))
            {
                return definition;
            }

            return db.SearchAssetByID(itemId);
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
