using System;

namespace GameUtils
{
    [Serializable]
    public abstract class Modifier
    {
        public object Source;
        public float Amount;
        public float Duration;
        public ModifierType ModifierType;

        //
        public Modifier(object source, float amount = 0, float duration = 0, ModifierType modifierType = ModifierType.Neutral)
        {
            Source = source;
            Amount = amount;
            Duration = duration;
            ModifierType = modifierType;
        }

        //
        public virtual int Order => 0;
        public virtual float ApplyModifier(float value) => 0;
    }
}