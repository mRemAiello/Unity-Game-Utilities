using System;

namespace GameUtils
{
    [Serializable]
    public class InventoryItemEventArgs
    {
        public RuntimeInventory Inventory;
        public InventoryItem Item;
        public int Quantity;

        public InventoryItemEventArgs(RuntimeInventory inventory, InventoryItem item, int quantity)
        {
            Inventory = inventory;
            Item = item;
            Quantity = quantity;
        }
    }
}
