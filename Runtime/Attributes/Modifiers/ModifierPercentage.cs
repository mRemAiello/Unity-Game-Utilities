using System;

namespace GameUtils
{
    [Serializable]
    public class ModifierPercentage : Modifier
    {
        public ModifierPercentage(object source, float amount = 0, float duration = 0) : base(source, amount, duration)
        {
        }

        //
        public override int Order => 2;
        public override float ApplyModifier(float value) => value * (1 + (Amount / 100));
    }
}
