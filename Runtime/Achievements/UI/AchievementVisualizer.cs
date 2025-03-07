using UnityEngine;

namespace GameUtils
{
    public class AchievementVisualizer : MonoBehaviour
    {
        [SerializeField] private AchievementNotification _notificationPrefab;

        //
        private void Awake()
        {
            // Subscribe to event
            AchievementManager.OnAchievementCompleted += OnAchievementUnlocked;
        }

        private void OnDestroy()
        {
            // Unsubscribe from event
            AchievementManager.OnAchievementCompleted -= OnAchievementUnlocked;
        }

        private void OnAchievementUnlocked(AchievementData achievement)
        {
            // Instantiate notification
            AchievementNotification notification = Instantiate(_notificationPrefab, transform);

            notification.Initialize(achievement);
            notification.Show();
        }
    }
}