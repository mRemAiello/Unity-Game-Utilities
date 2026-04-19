using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GUConstants.EVENT_NAME + "Skill/Change State")]
    public class ChangeSkillStateEventAsset : GameEventAsset<RuntimeSkillNode, SkillNodeState, int>
    {
    }
}