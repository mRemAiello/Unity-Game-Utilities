using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class Pool
    {
        public GameObject Prefab;
        public string Tag;
        public int Size;
    }
}