using UnityEngine;

namespace GameUtils
{
    [DefaultExecutionOrder(100)]
    public class ApplicationQuitManager : Singleton<ApplicationQuitManager>
    {
        [SerializeField] private VoidEventAsset _onApplicationQuit;

        void OnApplicationQuit()
        {
            _onApplicationQuit?.Invoke();    
        }
    }
}
