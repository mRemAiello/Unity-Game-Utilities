using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Manages achievement data and runtime instances.
    /// </summary>
    public class AchievementManager : GenericDataManager<AchievementManager, AchievementData>
    {
        private readonly Dictionary<string, RuntimeAchievement> _runtimeAchievements = new();

        // TODO: When sdk of different platform (steam, xbox, etc) are implemented listen to those events
        public event Action<RuntimeAchievement> OnAchievementCompleted;
        public event Action<RuntimeAchievement> OnAchievementUncompleted;

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            foreach (var data in Items)
            {
                _runtimeAchievements[data.ID] = new RuntimeAchievement(data);
            }
        }

        public void OnAchievementEvent(string achievementEventName, int value)
        {
            foreach (var data in Items.Where(a => a.EventName == achievementEventName))
            {
                if (_runtimeAchievements.TryGetValue(data.ID, out var runtime))
                {
                    runtime.UpdateState(value);
                }
            }
        }

        public void AchievementCompleted(RuntimeAchievement achievement)
        {
            OnAchievementCompleted?.Invoke(achievement);
        }

        public void AchievementUncompleted(RuntimeAchievement achievement)
        {
            OnAchievementUncompleted?.Invoke(achievement);
        }
    }
}

