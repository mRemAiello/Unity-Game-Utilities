using DG.Tweening;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class CanvasFader : UIAnimation
    {
        [Tab("Settings")]
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Tab("Fade In")]
        [SerializeField] private bool _useFadeInCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeInSpeed = 1.0f;
        
        [ShowIf("_useFadeInCurve")]
        [SerializeField] private AnimationCurve _fadeInCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [EndIf]
        
        [Tab("Fade Out")]
        [SerializeField] private bool _useFadeOutCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeOutSpeed = 1.0f;
        
        [ShowIf("_useFadeOutCurve")]
        [SerializeField] private AnimationCurve _fadeOutCurve = AnimationCurve.Linear(0, 1, 1, 0);
        [EndIf]

        protected override void OnShow(bool showAnimation = true)
        {
            //
            _canvasGroup.alpha = 0;
            if (showAnimation)
            {
                if (_useFadeOutCurve)
                {
                    _canvasGroup.DOFade(1, _fadeOutSpeed).SetEase(_fadeOutCurve).OnComplete(() => EndAnimation(UIAnimationStatus.Showed));
                }
                else
                {
                    _canvasGroup.DOFade(1, _fadeOutSpeed).OnComplete(() => EndAnimation(UIAnimationStatus.Showed));
                }
            }
            else
            {
                _canvasGroup.alpha = 1;

                //
                EndAnimation(UIAnimationStatus.Showed);
            }
        }

        protected override void OnHide(bool showAnimation = true)
        {
            // 
            _canvasGroup.alpha = 1;
            if (showAnimation)
            {
                if (_useFadeInCurve)
                {
                    _canvasGroup.DOFade(0, _fadeInSpeed).SetEase(_fadeInCurve).OnComplete(() => EndAnimation(UIAnimationStatus.Hided));
                }
                else
                {
                    _canvasGroup.DOFade(0, _fadeInSpeed).OnComplete(() =>  EndAnimation(UIAnimationStatus.Hided));
                }
            }
            else
            {
                _canvasGroup.alpha = 0;

                //
                EndAnimation(UIAnimationStatus.Hided);
            }
        }
    }
}