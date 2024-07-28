using UnityEngine;

namespace GameUtils
{
    public class ApplicationQuitManager : Singleton<ApplicationQuitManager>
    {
        [SerializeField] private VoidEventAsset _onApplicationQuit;

        void OnApplicationQuit()
        {
            _onApplicationQuit?.Invoke();    
        }
    }
}