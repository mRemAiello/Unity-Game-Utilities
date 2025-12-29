namespace GameUtils
{
    public interface IItem
    {
        ItemData Definition { get; }
        int Quantity { get; }
        string UniqueInstanceID { get; }

        void SetQuantity(int quantity);
        void AddQuantity(int amount);
        void RemoveQuantity(int amount);
    }
}
