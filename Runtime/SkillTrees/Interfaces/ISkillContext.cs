namespace GameUtils
{
    public interface ISkillContext
    {
        T Get<T>() where T : class;
        bool TryGet<T>(out T capability) where T : class;
        bool Has<T>() where T : class;
    }
}