using System;

namespace RPGSystem
{
    [Serializable]
    public class ModifierFixed : Modifier
    {
        public ModifierFixed() : base()
        {
        }

        public override int Order
        {
            get
            {
                return 0;
            }
        }

        public override float ApplyModifier(float attribute_value)
        {
            return attribute_value + Amount;
        }
    }
}
