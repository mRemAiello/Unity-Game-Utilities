using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "Game Utils/Tags/Game Tag")]
    public class GameTag : UniqueID
    {
        [SerializeField] private string _internalName;

        // private field
        public string InternalName => _internalName;
    }
}
