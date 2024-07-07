namespace GameUtils
{
    public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected new void Awake()
        {
            base.Awake();

            // 
            DontDestroyOnLoad(gameObject);
        }
    }
}