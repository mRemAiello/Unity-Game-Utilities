using System;

namespace GameUtils
{
    /// <summary>
    /// Base class for attribute modifiers applied at runtime.
    /// </summary>
    [Serializable]
    public abstract class Modifier
    {
        // Public fields to simplify serialization and inspector usage.
        public object Source;
        public float Amount;
        public float Duration;
        public ModifierType ModifierType;
        public bool IsPermanent;

        /// <summary>
        /// Creates a modifier with the specified source, amount, duration, type, and permanence.
        /// </summary>
        public Modifier(object source, float amount = 0, float duration = 0, ModifierType modifierType = ModifierType.Neutral, bool isPermanent = false)
        {
            Source = source;
            Amount = amount;
            Duration = duration;
            ModifierType = modifierType;
            IsPermanent = isPermanent;
        }

        /// <summary>
        /// Determines the application order among modifiers (lower is applied first).
        /// </summary>
        public virtual int Order => 0;

        /// <summary>
        /// Applies the modifier effect to the incoming value.
        /// </summary>
        public virtual float ApplyModifier(float value) => 0;
    }
}
