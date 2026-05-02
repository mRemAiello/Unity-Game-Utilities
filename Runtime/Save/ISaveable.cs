namespace GameUtils
{
    public interface ISaveable
    {
        string SaveContext { get; }

        //
        object CaptureState();
        void RestoreState(object state);
    }
}