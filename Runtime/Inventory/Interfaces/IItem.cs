namespace GameUtils
{
    public interface IItem
    {
        ItemData Definition { get; }
        int Quantity { get; }
        string UniqueInstanceId { get; }

        void SetQuantity(int quantity);
        void AddQuantity(int amount);
        void RemoveQuantity(int amount);
    }
}
