using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Animations")]
    [CreateAssetMenu(menuName = GUConstants.HEALTH_NAME + "Popup Data")]
    public class DamagePopupData : ScriptableObject
    {
        [SerializeField, Group("Animations")] private GameObject _damagePopupPrefab;
        [SerializeField, Group("Animations")] private Color _textColor = Color.white;
        [SerializeField, Group("Animations")] private float _textSize = 36f;
        [SerializeField, Group("Animations")] private float _duration = 1f;
        [SerializeField, Group("Animations")] private float _speed = 50f;
        [SerializeField, Group("Animations")] private float _fadeStartTime = 0.7f;
        [SerializeField, Group("Animations")] private float _fadeDuration = 0.3f;
        [SerializeField, Group("Animations")] private float _arcWidth = 0.5f;

        //
        public GameObject DamagePopupPrefab => _damagePopupPrefab;
        public Color TextColor => _textColor;
        public float TextSize => _textSize;
        public float Duration => _duration;
        public float Speed => _speed;
        public float FadeStartTime => _fadeStartTime;
        public float FadeDuration => _fadeDuration;
        public float ArcWidth => _arcWidth;
    }
}