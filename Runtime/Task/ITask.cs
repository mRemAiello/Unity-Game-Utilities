namespace GameUtils
{
    public interface ITask
    {
        double TimeElapsed
        {
            get;
            set;
        }

        string ShortName
        {
            get;
        }

        void Execute(ref object context, ref object data);
    }
}