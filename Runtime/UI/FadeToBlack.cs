using DG.Tweening;
using QFSW.QC;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class FadeToBlack : Singleton<FadeToBlack>
    {
        [Tab("Settings")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
        [SerializeField] private bool _useFadeInCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeInSpeed = 1.0f;
        [SerializeField, ShowIf("_useFadeInCurve")] private AnimationCurve _fadeInCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [Space]
        [SerializeField] private bool _useFadeOutCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeOutSpeed = 1.0f;
        [SerializeField, ShowIf("_useFadeOutCurve")] private AnimationCurve _fadeOutCurve = AnimationCurve.Linear(0, 1, 1, 0);


        [Tab("Events")]
        [SerializeField] private VoidGameEventAsset _onPreAnimationEvent;
        [SerializeField] private VoidGameEventAsset _onMiddleAnimationEvent;
        [SerializeField] private VoidGameEventAsset _onEndAnimationEvent;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private bool _isAnimating = false;

        [Button, Command]
        public void StartFade()
        {
            //
            if (_isAnimating)
            {
                return;
            }

            // 
            _isAnimating = true;

            // 
            _onPreAnimationEvent.Invoke();

            //
            FadeIn();
        }

        private void FadeIn()
        {
            if (_canvasGroup == null)
            {
                return;
            }

            // Start fade in
            _canvasGroup.alpha = 0;
            if (_useFadeInCurve)
            {
                _canvasGroup.DOFade(1, _fadeInSpeed).SetEase(_fadeInCurve).OnComplete(() => FadeOut());
            }
            else
            {
                _canvasGroup.DOFade(1, _fadeInSpeed).OnComplete(() => FadeOut());
            }
        }

        private void FadeOut()
        {
            //
            _onMiddleAnimationEvent.Invoke();

            // Start fade out
            if (_useFadeOutCurve)
            {
                _canvasGroup.DOFade(1, _fadeOutSpeed).SetEase(_fadeOutCurve).OnComplete(() => EndAnimation());
            }
            else
            {
                _canvasGroup.DOFade(1, _fadeOutSpeed).OnComplete(() => EndAnimation());
            }
        }

        private void EndAnimation()
        {
            //
            _onEndAnimationEvent.Invoke();

            //
            _isAnimating = false;
        }
    }
}