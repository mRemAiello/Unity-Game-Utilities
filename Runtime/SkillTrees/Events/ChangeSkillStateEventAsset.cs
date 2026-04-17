using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.EVENT_NAME + "Skill/Change State")]
    public class ChangeSkillStateEventAsset : GameEventAsset<RuntimeSkillNode, SkillNodeState>
    {
    }
}