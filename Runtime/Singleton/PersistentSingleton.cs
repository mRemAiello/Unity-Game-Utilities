namespace Arbinty.Utils
{
    public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected override void OnPostAwake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}