using System;

namespace GameUtils
{
    [Serializable]
    public struct SkillStateChangeData
    {
        public RuntimeSkillNode Node;
        public SkillNodeState NewState;
        public int Level;
    }
}