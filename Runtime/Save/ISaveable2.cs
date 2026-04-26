namespace GameUtils
{
    public interface ISaveable2
    {
        string ID { get; }

        //
        object Save();
        void Load(object state);
    }
}