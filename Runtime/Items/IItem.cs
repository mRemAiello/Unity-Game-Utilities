namespace GameUtils
{
    public interface IItem
    {
        ItemDefinition Definition { get; }
        int Quantity { get; }
        string UniqueInstanceId { get; }

        void SetQuantity(int quantity);
        void AddQuantity(int amount);
        void RemoveQuantity(int amount);
    }
}
