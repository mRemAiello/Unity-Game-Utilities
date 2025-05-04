using UnityEngine;

namespace GameUtils
{

    [CreateAssetMenu(menuName = Constant.EVENT_NAME + "Currency/Currency Change Event")]
    public class CurrencyChangeEvent : GameEventAsset<CurrencyChangeEventArgs>
    {
    }
}