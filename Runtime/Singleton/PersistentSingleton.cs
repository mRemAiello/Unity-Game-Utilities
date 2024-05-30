namespace GameUtils
{
    public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        new void Awake()
        {
            base.Awake();

            // 
            DontDestroyOnLoad(gameObject);
        }
    }
}