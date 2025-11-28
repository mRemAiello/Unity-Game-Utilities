using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.STATUS_NAME + "StatusEffect")]
    public class StatusEffectEventAsset : GameEventAsset<RuntimeStatusEffect>
    {
    }
}