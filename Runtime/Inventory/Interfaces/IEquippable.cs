namespace GameUtils
{
    public interface IEquippable
    {
        EquipmentSlot Slot { get; }

        void Equip();
        void Unequip();
    }
}
