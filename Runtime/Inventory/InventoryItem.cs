using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class InventoryItem : IItem, IStackable, IUsable, IEquippable
    {
        [SerializeField] private ItemData _itemData;
        [SerializeField] private int _quantity;
        [SerializeField] private string _uniqueInstanceID;

        public InventoryItem(ItemData definition, int quantity, string uniqueInstanceId = null)
        {
            _itemData = definition;
            _uniqueInstanceID = string.IsNullOrEmpty(uniqueInstanceId) ? Guid.NewGuid().ToString() : uniqueInstanceId;
            SetQuantity(quantity);
        }

        public ItemData Definition => _itemData;
        public int Quantity => _quantity;
        public string UniqueInstanceId => _uniqueInstanceID;
        public int MaxStackSize => _itemData != null ? _itemData.MaxStackSize : 0;
        public EquipmentSlot Slot => _itemData != null ? _itemData.EquipmentSlot : null;

        public void SetQuantity(int quantity)
        {
            int max = MaxStackSize > 0 ? MaxStackSize : int.MaxValue;
            _quantity = Mathf.Clamp(quantity, 0, max);
        }

        public void AddQuantity(int amount)
        {
            SetQuantity(_quantity + amount);
        }

        public void RemoveQuantity(int amount)
        {
            SetQuantity(_quantity - amount);
        }

        public bool CanStackWith(IItem other)
        {
            return other != null && other.Definition == _itemData && MaxStackSize > 1;
        }

        public bool CanUse()
        {
            return _itemData != null && _quantity > 0;
        }

        public void Use()
        {
            if (CanUse())
            {
                RemoveQuantity(1);
            }
        }

        public void Equip()
        {
        }

        public void Unequip()
        {
        }
    }
}
