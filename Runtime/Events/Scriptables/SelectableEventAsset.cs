using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.EVENT_NAME + "UI/Selectable")]
    public class SelectableEventAsset : GameEventAsset<ISelectable>
    {
    }
}