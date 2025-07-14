using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class RuntimeStatusEffect
    {
        public string ID = "";
        public GameObject Source = null;
        public GameObject Target = null;
        public StatusEffectData Data = null;
        public int Duration = 0;
        public int Intensity = 0;

        //
        public RuntimeStatusEffect(string id, GameObject source, GameObject target, StatusEffectData data)
        {
            ID = id;
            Source = source;
            Target = target;
            Data = data;
        }
    }
}