using TriInspector;
using UnityEngine;

namespace GameUtils
{

    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Currency/Currency Change Event")]
    public class CurrencyChangeEvent : GameEventAsset<CurrencyChangeEventArgs>
    {
        [Button(ButtonSizes.Medium)]
        public override void Invoke(CurrencyChangeEventArgs param)
        {
            // Inoltra l'invocazione all'implementazione base.
            base.Invoke(param);
        }
    }
}