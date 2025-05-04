using System;

namespace GameUtils
{
    [Serializable]
    public class CurrencyChangeEventArgs
    {
        public CurrencyData CurrencyData;
        public int Amount;

        public CurrencyChangeEventArgs(CurrencyData currencyData, int amount)
        {
            CurrencyData = currencyData;
            Amount = amount;
        }
    }
}