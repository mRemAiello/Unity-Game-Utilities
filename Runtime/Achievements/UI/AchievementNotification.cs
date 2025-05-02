using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TriInspector;

namespace GameUtils
{
    public class AchievementNotification : MonoBehaviour
    {
        [Tab("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;

        [Tab("Settings")]
        [SerializeField] private float _showDuration = 5f;
        [SerializeField] private float _animationDuration = 1f;

        //
        public void Initialize(AchievementData achievement)
        {
            _icon.sprite = achievement.CompletedIcon;
            _title.text = achievement.Name;
            _description.text = achievement.Description;
        }

        public void Show()
        {
            gameObject.SetActive(true);

            //
            _rectTransform.sizeDelta = new Vector2(100, _rectTransform.sizeDelta.y);
            //_rectTransform.DOSizeDelta(new Vector2(400, _rectTransform.sizeDelta.y), _animationDuration).SetEase(Ease.OutBack);

            // Start coroutine to hide after duration
            StartCoroutine(HideAfterDuration());
        }

        private IEnumerator HideAfterDuration()
        {
            yield return new WaitForSeconds(_showDuration);
            Hide();
        }

        public void Hide()
        {
            Vector2 size = new(100, _rectTransform.sizeDelta.y);
            //_rectTransform.DOSizeDelta(size, _animationDuration).SetEase(Ease.InBack).OnComplete(() => Destroy(gameObject));
        }
    }
}