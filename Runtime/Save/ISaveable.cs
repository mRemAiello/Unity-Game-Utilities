namespace GameUtils
{
    public interface ISaveable
    {
        string SaveContext { get; }

        /// <summary>
        /// Trigger a save for this object.
        /// </summary>
        void Save();

        /// <summary>
        /// Trigger a load for this object.
        /// </summary>
        void Load();
    }
}