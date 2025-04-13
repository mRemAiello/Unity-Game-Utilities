using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.CURRENCY_NAME + "Currency")]
    [DeclareBoxGroup("currency", Title = "Currency")]
    public class CurrencyData : ItemAssetBase
    {
        // TODO: Inserire un sistema di conversione (isConvertible e poi lista di converter)
        [SerializeField, Group("currency")] private CurrencyType _currencyType;
        [SerializeField, Group("currency")] private int _maxAmount;
        [SerializeField, Group("currency")] private AudioClip _collectSound;

        //
        public CurrencyType CurrencyType => _currencyType;
        public int MaxAmount => _maxAmount;
        public AudioClip CollectSound => _collectSound;
    }
}