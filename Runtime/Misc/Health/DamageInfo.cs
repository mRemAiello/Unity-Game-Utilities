using UnityEngine;

namespace GameUtils
{
    public struct DamageInfo
    {
        public float Amount;
        public bool IsCrit;
        public GameObject Instigator;
        public GameObject Source;
        public GameObject Victim;
        public object AdditionalArgs;

        public DamageInfo(float amount, bool isCrit, GameObject instigator, GameObject source, GameObject victim)
        {
            Amount = amount;
            IsCrit = isCrit;
            Instigator = instigator;
            Source = source;
            Victim = victim;
            AdditionalArgs = null;
        }

        public DamageInfo(float amount, bool isCrit, GameObject instigator, GameObject source, GameObject victim, object additionalArgs)
        {
            Amount = amount;
            IsCrit = isCrit;
            Instigator = instigator;
            Source = source;
            Victim = victim;
            AdditionalArgs = additionalArgs;
        }
    }
}