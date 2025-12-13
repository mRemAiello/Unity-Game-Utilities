using System;
using System.Collections.Generic;

namespace GameUtils
{
    [Serializable]
    public class StatusEffectManagerSaveData
    {
        public List<StatusEffectSaveData> Effects = new();
        public List<TagSaveData> Tags = new();
        public List<TagSaveData> Immunities = new();
    }
}