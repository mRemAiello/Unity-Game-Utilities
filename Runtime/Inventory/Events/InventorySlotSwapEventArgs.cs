using System;

namespace GameUtils
{
    [Serializable]
    public class InventorySlotSwapEventArgs
    {
        public RuntimeInventory Inventory;
        public int FromIndex;
        public int ToIndex;

        public InventorySlotSwapEventArgs(RuntimeInventory inventory, int fromIndex, int toIndex)
        {
            Inventory = inventory;
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
    }
}
