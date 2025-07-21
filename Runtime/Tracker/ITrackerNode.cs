namespace GameUtils
{
    public interface ITrackerNode
    {
        bool Changed { get; }
        void Check();
        void ForceInvoke();
    }

    //
    public interface ITrackerNode<T> : ITrackerNode
    {
        T PreviousValue { get; }
        T CurrentValue { get; }
    }
}