using System;

namespace GameUtils
{
    [Serializable]
    public struct RuntimeAchievementSaveData
    {
        public string ID;
        public int CurrentValue;
        public bool IsCompleted;

        public static RuntimeAchievementSaveData FromRuntimeAchievement(RuntimeAchievement runtimeAchievement)
        {
            return new RuntimeAchievementSaveData
            {
                ID = runtimeAchievement.Data.ID,
                CurrentValue = runtimeAchievement.CurrentValue,
                IsCompleted = runtimeAchievement.IsCompleted
            };
        }
    }
}
