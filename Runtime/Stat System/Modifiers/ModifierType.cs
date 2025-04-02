namespace GameUtils
{
    public enum ModifierType
    {
        /// <summary>A flat modifier that adds a fixed value to the current value.</summary>
        Flat = 100,

        /// <summary>An additive modifier that adds a percentage of the base value to the current value.</summary>
        Additive = 200,

        /// <summary>A multiplicative modifier that adds a percentage of the current value to the current value.</summary>
        Multiplicative = 300
    }
}