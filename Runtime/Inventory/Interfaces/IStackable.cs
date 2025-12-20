namespace GameUtils
{
    public interface IStackable
    {
        int MaxStackSize { get; }

        bool CanStackWith(IItem other);
    }
}
