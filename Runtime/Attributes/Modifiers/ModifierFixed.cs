using System;

namespace GameUtils
{
    [Serializable]
    public class ModifierFixed : Modifier
    {
        /// <summary>
        /// Creates a fixed modifier with the specified amount, duration, and permanence.
        /// </summary>
        public ModifierFixed(object source, float amount = 0, float duration = 0, bool isPermanent = false)
            : base(source, amount, duration, ModifierType.Neutral, isPermanent)
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
    }
}
