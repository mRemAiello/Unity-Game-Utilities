using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Skill/Click")]
    public class ClickSkillEventAsset : GameEventAsset<RuntimeSkillNode>
    {
    }
}