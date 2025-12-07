namespace GameUtils
{
    public interface ISaveable
    {
        string SaveContext { get; }

        //
        void Save();
        void Load();
    }
}