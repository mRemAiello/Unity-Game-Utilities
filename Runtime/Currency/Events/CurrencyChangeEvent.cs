using UnityEngine;

namespace GameUtils
{

    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Currency/Currency Change Event")]
    public class CurrencyChangeEvent : GameEventAsset<CurrencyChangeEventArgs>
    {
    }
}