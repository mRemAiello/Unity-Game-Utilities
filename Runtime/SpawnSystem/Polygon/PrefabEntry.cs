using UnityEngine;

namespace GameUtils
{
    [System.Serializable]
    public class PrefabEntry
    {
        public GameObject prefab;
        [Min(0f)] public float weight = 1f;
    }
}