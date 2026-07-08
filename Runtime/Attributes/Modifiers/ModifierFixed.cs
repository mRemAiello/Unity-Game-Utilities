using System;

namespace GameUtils
{
    /// <summary>
    /// Modifier that adds a fixed amount to the base value.
    /// </summary>
    [Serializable]
    public class ModifierFixed : Modifier
    {
        /// <summary>
        /// Creates a fixed modifier with the specified amount, duration, and permanence.
        /// </summary>
        public ModifierFixed(object source, float amount = 0, float duration = 0, bool isPermanent = false)
            : base(source, amount, duration, isPermanent)
        {
        }

        /// <summary>
        /// Applies after permanent modifiers and before percentage ones.
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// Applies a fixed delta to the incoming value.
        /// </summary>
        public override float ApplyModifier(float value) => value + Amount;

        /// <summary>
        /// Returns a string representation of the modifier.
        /// </summary>
        public override string ToString()
        {
            string sign = Amount >= 0 ? "+" : "-";
            string duration = IsPermanent ? "permanent" : $"{Duration} seconds";
            return $"{sign}{Amount} from {Source} with {duration} duration";
        }
    }
}
