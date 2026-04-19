using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Inventory/Item Event")]
    public class InventoryItemEvent : GameEventAsset<InventoryItemEventArgs>
    {
    }
}
