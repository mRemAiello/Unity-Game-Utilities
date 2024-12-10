using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class StatusEffect
    {
        public string ID;
        public GameObject Source;
        public GameObject Target;
        public StatusEffectData Data;
        public int Amount;
    }
}