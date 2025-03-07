using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace GameUtils
{
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField] private List<AchievementData> _achievements;

        // Private fields
        private static AchievementManager _instance;

        // Public readonly fields
        public static AchievementManager Instance => _instance;

        // Public events
        // TODO: When sdk of different platform (steam, xbox, etc) are implemented listen to those events
        public static event Action<AchievementData> OnAchievementCompleted;
        public static event Action<AchievementData> OnAchievementUncompleted;

        //
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
        }

        private void OnValidate()
        {
            // Remove duplicates
            _achievements = _achievements.Distinct().ToList();
        }

        public void OnAchievementEvent(string achievementEventName, int value)
        {
            // Get all achievements with the same event name
            List<AchievementData> achievements = _achievements.FindAll(a => a.EventName == achievementEventName);

            foreach (AchievementData achievement in achievements)
            {
                achievement.UpdateState(value);
            }
        }

        public void AchievementCompleted(AchievementData achievement)
        {
            OnAchievementCompleted?.Invoke(achievement);
        }

        public void AchievementUncompleted(AchievementData achievement)
        {
            OnAchievementUncompleted?.Invoke(achievement);
        }
    }
}