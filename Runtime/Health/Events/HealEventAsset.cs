using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Health/Heal Popup")]
    public class HealEventAsset : GameEventAsset<string, Transform>
    {
    }
}