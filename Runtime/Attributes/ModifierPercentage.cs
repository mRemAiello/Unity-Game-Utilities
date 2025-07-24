using System;

namespace RPGSystem
{
    [Serializable]
    public class ModifierPercentage : Modifier
    {
        public ModifierPercentage() : base()
        {
        }

        public override int Order
        {
            get
            {
                return 1;
            }
        }

        public override float ApplyModifier(float attribute_value)
        {
            return attribute_value * (1 + (Amount / 100));
        }
    }
}
