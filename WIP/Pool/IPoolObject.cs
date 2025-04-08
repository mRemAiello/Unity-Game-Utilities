namespace GameUtils
{
    public interface IPoolObject
    {
        void OnObjectCreate() { }
        void OnObjectReuse() { }
        void OnObjectReturn() { }
        void OnObjectDestroy() { }
    }
}