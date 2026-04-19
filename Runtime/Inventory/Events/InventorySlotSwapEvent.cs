using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Inventory/Slots Swapped Event")]
    public class InventorySlotSwapEvent : GameEventAsset<InventorySlotSwapEventArgs>
    {
    }
}
