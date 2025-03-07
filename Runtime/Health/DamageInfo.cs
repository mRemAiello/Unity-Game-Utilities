using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class DamageInfo
    {
        public GameObject Source;
        public GameObject Target;
        public float Amount;
        public bool IsCrit;
        public object AdditionalArgs;

        public DamageInfo(GameObject source, GameObject target, float amount, bool isCrit, object additionalArgs = null)
        {
            Amount = amount;
            IsCrit = isCrit;
            Source = source;
            Target = target;
            AdditionalArgs = additionalArgs;
        }
    }
}