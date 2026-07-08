using System;

namespace GameUtils
{
    /// <summary>
    /// Modifier that applies a percentage multiplier to the base value.
    /// </summary>
    [Serializable]
    public class ModifierPercentage : Modifier
    {
        /// <summary>
        /// Creates a percentage modifier with the specified amount, duration, and permanence.
        /// </summary>
        public ModifierPercentage(object source, float amount = 0, float duration = 0, bool isPermanent = false)
            : base(source, amount, duration, isPermanent)
        {
        }

        /// <summary>
        /// Applies after fixed modifiers.
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// Applies a percentage delta to the incoming value.
        /// </summary>
        public override float ApplyModifier(float value) => value * (1 + (Amount / 100));

        /// <summary>
        /// Returns a string representation of the modifier.
        /// </summary>
        public override string ToString()
        {
            string sign = Amount >= 0 ? "+" : "-";
            string duration = IsPermanent ? "permanent" : $"{Duration} seconds";
            return $"{sign}{Amount}% from {Source} with {duration} duration";
        }
    }
}
