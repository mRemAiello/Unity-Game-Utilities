using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Manages achievement data and runtime instances.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class AchievementManager : GenericDataManager<AchievementManager, AchievementData>, ISaveable
    {
        private readonly Dictionary<string, RuntimeAchievement> _runtimeAchievements = new();

        public string SaveContext => "Achievements";

        // TODO: When sdk of different platform (steam, xbox, etc) are implemented listen to those events
        public event Action<RuntimeAchievement> OnAchievementCompleted;
        public event Action<RuntimeAchievement> OnAchievementUncompleted;

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            InitializeRuntimeAchievements();
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

        public void Save()
        {
            if (!GameSaveManager.InstanceExists)
            {
                return;
            }

            var runtimeSaveData = _runtimeAchievements.Values
                .Select(RuntimeAchievementSaveData.FromRuntimeAchievement)
                .ToList();

            GameSaveManager.Instance.Save(SaveContext, nameof(_runtimeAchievements), runtimeSaveData);
        }

        public void Load()
        {
            InitializeRuntimeAchievements();

            if (!GameSaveManager.InstanceExists)
            {
                return;
            }

            if (GameSaveManager.Instance.TryLoad(SaveContext, nameof(_runtimeAchievements), out List<RuntimeAchievementSaveData> runtimeSaveData, new()))
            {
                foreach (var savedAchievement in runtimeSaveData)
                {
                    if (_runtimeAchievements.TryGetValue(savedAchievement.ID, out var runtimeAchievement))
                    {
                        runtimeAchievement.RestoreState(savedAchievement.CurrentValue, savedAchievement.IsCompleted);
                    }
                }
            }
        }

        private void InitializeRuntimeAchievements()
        {
            _runtimeAchievements.Clear();

            foreach (var data in Items)
            {
                _runtimeAchievements[data.ID] = new RuntimeAchievement(data);
            }
        }
    }

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

