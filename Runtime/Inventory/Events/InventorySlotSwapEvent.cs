using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Inventory/Slots Swapped Event")]
    public class InventorySlotSwapEvent : GameEventAsset<InventorySlotSwapEventArgs>
    {
    }
}
