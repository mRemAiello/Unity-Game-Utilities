using System;
using UnityEngine;

namespace GameUtils
{
    [Serializable]
    public class CurrencyConversionRate
    {
        [SerializeField] private CurrencyData _currencyData;
        [SerializeField] private float _conversionRate = 1.0f;

        //
        public CurrencyData CurrencyData => _currencyData;
        public float ConversionRate => _conversionRate;
    }
}