using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace GameUtils
{
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField] private List<AchievementData> _achievementData;

        private List<RuntimeAchievement> _achievements;
        private static AchievementManager _instance;

        public static AchievementManager Instance => _instance;

        // TODO: When sdk of different platform (steam, xbox, etc) are implemented listen to those events
        public static event Action<RuntimeAchievement> OnAchievementCompleted;
        public static event Action<RuntimeAchievement> OnAchievementUncompleted;

        private void Awake()
        {
            // Singleton
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            _achievements = _achievementData.Select(data => new RuntimeAchievement(data)).ToList();
        }

        private void OnValidate()
        {
            // Remove duplicates
            _achievementData = _achievementData.Distinct().ToList();
        }

        public void OnAchievementEvent(string achievementEventName, int value)
        {
            // Get all achievements with the same event name
            List<RuntimeAchievement> achievements = _achievements.FindAll(a => a.Data.EventName == achievementEventName);

            foreach (RuntimeAchievement achievement in achievements)
            {
                achievement.UpdateState(value);
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
