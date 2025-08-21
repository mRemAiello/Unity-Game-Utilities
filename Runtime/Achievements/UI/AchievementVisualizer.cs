using UnityEngine;

namespace GameUtils
{
    public class AchievementVisualizer : MonoBehaviour
    {
        [SerializeField] private AchievementNotification _notificationPrefab;

        private void OnEnable()
        {
            if (AchievementManager.InstanceExists)
                AchievementManager.Instance.OnAchievementCompleted += OnAchievementUnlocked;
        }

        private void OnDisable()
        {
            if (AchievementManager.InstanceExists)
                AchievementManager.Instance.OnAchievementCompleted -= OnAchievementUnlocked;
        }

        private void OnAchievementUnlocked(RuntimeAchievement achievement)
        {
            // Instantiate notification
            AchievementNotification notification = Instantiate(_notificationPrefab, transform);

            notification.Initialize(achievement);
            notification.Show();
        }
    }
}
