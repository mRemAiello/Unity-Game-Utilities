using UnityEngine;

namespace GameUtils
{
    // Ensures this singleton initializes much earlier than standard MonoBehaviours.
    [DefaultExecutionOrder(-10000)]
    public class ApplicationQuitManager : Singleton<ApplicationQuitManager>
    {
        [SerializeField] private VoidEventAsset _onApplicationQuit;

        void OnApplicationQuit()
        {
            _onApplicationQuit?.Invoke();    
        }
    }
}
