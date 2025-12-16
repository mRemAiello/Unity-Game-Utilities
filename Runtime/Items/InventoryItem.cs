using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class InventoryItem : IItem, IStackable, IUsable, IEquippable
    {
        [SerializeField] private ItemDefinition _definition;
        [SerializeField] private int _quantity;
        [SerializeField] private string _uniqueInstanceId;

        public InventoryItem(ItemDefinition definition, int quantity, string uniqueInstanceId = null)
        {
            _definition = definition;
            _uniqueInstanceId = string.IsNullOrEmpty(uniqueInstanceId) ? Guid.NewGuid().ToString() : uniqueInstanceId;
            SetQuantity(quantity);
        }

        public ItemDefinition Definition => _definition;
        public int Quantity => _quantity;
        public string UniqueInstanceId => _uniqueInstanceId;
        public int MaxStackSize => _definition != null ? _definition.MaxStackSize : 0;
        public EquipmentSlot Slot => _definition != null ? _definition.EquipmentSlot : null;

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
            return other != null && other.Definition == _definition && MaxStackSize > 1;
        }

        public bool CanUse()
        {
            return _definition != null && _quantity > 0;
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
