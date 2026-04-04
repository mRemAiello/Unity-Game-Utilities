using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Item")]
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.INVENTORY_NAME + "Item Data")]
    public class ItemData : ItemVisualData
    {
        [SerializeField, Group("Item")] private ItemType _itemType;
        [SerializeField, Group("Item")] private Rarity _rarity;
        [SerializeField, Group("Item"), Min(1)] private int _maxStackSize = 1;
        [SerializeField, Group("Item"), Min(0)] private int _baseValue;
        [SerializeField, Group("Item")] private bool _isUnique;
        [SerializeField, Group("Item")] private EquipmentSlot _equipmentSlot;

        //
        public ItemType ItemType => _itemType;
        public Rarity Rarity => _rarity;
        public int MaxStackSize => _maxStackSize;
        public int BaseValue => _baseValue;
        public bool IsUnique => _isUnique;
        public EquipmentSlot EquipmentSlot => _equipmentSlot;
    }
}
