namespace GameUtils
{
    public readonly struct TimedEventHandle
    {
        internal TimedEventHandle(int id) => ID = id;
        internal int ID { get; }
    }
}
