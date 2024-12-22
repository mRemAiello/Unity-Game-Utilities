using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class StatusEffect
    {
        public string ID = "";
        public GameObject Source = null;
        public GameObject Target = null;
        public StatusEffectData Data = null;
        public int Duration = 0;
        public int Intensity = 0;
    }
}