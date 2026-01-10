using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.CURRENCY_NAME + "Currency")]
    [DeclareBoxGroup("currency", Title = "Currency")]
    public class CurrencyData : ItemVisualData
    {
        [SerializeField, Group("currency")] private CurrencyType _currencyType;
        [SerializeField, Group("currency")] private int _maxAmount;
        [SerializeField, Group("currency")] private bool _isConvertible;
        [SerializeField, Group("currency"), ShowIf(nameof(_isConvertible), true)] private List<CurrencyConversionRate > _currencyConversions;

        //
        public CurrencyType CurrencyType => _currencyType;
        public int MaxAmount => _maxAmount;
        public bool IsConvertible => _isConvertible;
        public IReadOnlyList<CurrencyConversionRate > CurrencyConversions => _currencyConversions;

        //
        public override bool Equals(object other)
        {
            if (other is CurrencyData otherCurrency)
            {
                return ID == otherCurrency.ID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool TryGetCurrencyConversionRate(CurrencyData currency, out CurrencyConversionRate conversionRate)
        {
            conversionRate = null;
            foreach (var conversion in _currencyConversions)
            {
                if (conversion.CurrencyData.Equals(currency))
                {
                    conversionRate = conversion;
                    return true;
                }
            }

            return false;
        }
    }
}