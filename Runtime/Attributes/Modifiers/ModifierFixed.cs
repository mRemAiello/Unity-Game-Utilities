using System;

namespace GameUtils
{
    [Serializable]
    public class ModifierFixed : Modifier
    {
        public ModifierFixed(object source, float amount = 0, float duration = 0) : base(source, amount, duration)
        {
        }

        //
        public override int Order => 1;
        public override float ApplyModifier(float value) => value + Amount;
    }
}