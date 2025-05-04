using System;
using Doneref.Collectables.CollectableType;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Defines collecting logic (item side).
    /// </summary>
    public class Collectable : MonoBehaviour
    {
        /*[SerializeField, BoxGroup("Settings")]
        private int _incrementValue;

        [SerializeField, BoxGroup("Settings"), Required]
        private CollectableTypeSO _type;

        [SerializeField, FoldoutGroup("Debug")]
        private bool _debugEnabled = false;

        // public fields
        public int IncrementValue => _incrementValue;
        public CollectableTypeSO Type => _type;
        public bool LogEnabled => _debugEnabled;
        public static event Action<int, CollectableTypeSO> OnCollectItem;

        /// <summary>
        /// Defines what happens when a collectable is collected by a character.
        /// </summary>
        public void Collect()
        {
            this.Log($"{_type} collected!");
            OnCollectItem.Invoke(_incrementValue, _type);

            Destroy(gameObject);
        }*/
    }
}
