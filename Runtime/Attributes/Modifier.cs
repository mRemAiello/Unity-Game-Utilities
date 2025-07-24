namespace RPGSystem
{
    public abstract class Modifier
    {
        public object Source;
        public float Amount;
        public float Duration;

        public Modifier()
        {
            Source = null;
            Amount = 0;
            Duration = 0;
        }

        public virtual int Order
        {
            get
            {
                return 0;
            }
        }

        public virtual float ApplyModifier(float attribute_value)
        {
            return 0;
        }
    }
}
