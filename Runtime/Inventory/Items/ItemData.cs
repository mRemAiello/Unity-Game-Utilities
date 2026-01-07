using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("item", Title = "Item")]
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.INVENTORY_NAME + "Item Data")]
    public class ItemData : ItemAssetBase
    {
        [SerializeField, Group("item")] private ItemType _itemType;
        [SerializeField, Group("item")] private Rarity _rarity;
        [SerializeField, Group("item"), Min(1)] private int _maxStackSize = 1;
        [SerializeField, Group("item"), Min(0)] private int _baseValue;
        [SerializeField, Group("item")] private bool _isUnique;
        [SerializeField, Group("item")] private EquipmentSlot _equipmentSlot;
        [SerializeField, Group("item")] private List<GameTag> _tags = new();

        //
        public ItemType ItemType => _itemType;
        public Rarity Rarity => _rarity;
        public int MaxStackSize => _maxStackSize;
        public int BaseValue => _baseValue;
        public bool IsUnique => _isUnique;
        public EquipmentSlot EquipmentSlot => _equipmentSlot;
        public override IReadOnlyList<GameTag> Tags => _tags;
    }
}
