using UnityEngine;

namespace Doneref.Collectables.CollectableType
{
    /// <summary>
    /// Defines the various types of collectables.
    /// </summary>
    [CreateAssetMenu(menuName = "Doneref/Collectables/New Collectable Type")]
    public class CollectableTypeSO : ScriptableObject
    {
        [SerializeField]
        private int _maxLimit;

        [SerializeField]
        private bool _isWinningCondition;

        public int MaxLimit => _maxLimit;
        public bool IsWinningCondition => _isWinningCondition;
    }
}
