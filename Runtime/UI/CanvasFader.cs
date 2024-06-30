using DG.Tweening;
using QFSW.QC;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class CanvasFader : Singleton<CanvasFader>
    {
        [Tab("Settings")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _showOnEnable = false;
        [SerializeField] private bool _hideOnDisable = false;
        
        [Tab("Fade In")]
        [SerializeField] private bool _useFadeInCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeInSpeed = 1.0f;
        [SerializeField, ShowIf("_useFadeInCurve")] private AnimationCurve _fadeInCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        [Tab("Fade Out")]
        [SerializeField] private bool _useFadeOutCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeOutSpeed = 1.0f;
        [SerializeField, ShowIf("_useFadeOutCurve")] private AnimationCurve _fadeOutCurve = AnimationCurve.Linear(0, 1, 1, 0);

        [Tab("Events")]
        [SerializeField] private VoidGameEventAsset _onShowEvent;
        [SerializeField] private VoidGameEventAsset _onHideEvent;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private bool _isAnimating = false;

        void OnEnable()
        {
            if (_showOnEnable)
            {
                Show();   
            }
        }

        void OnDisable()
        {
            if (_hideOnDisable)
            {
                Hide();
            }
        }

        public void ShowOrHide(bool value, bool showAnimation = true)
        {
            if (value)
            {
                Show(showAnimation);
            }
            else
            {
                Hide(showAnimation);
            }
        }

        [Button, Command]
        public void Show(bool showAnimation = true)
        {
            //
            if (_isAnimating)
                return;

            // 
            _isAnimating = true;

            // 
            _canvasGroup.alpha = 0;
            if (showAnimation)
            {
                if (_useFadeInCurve)
                {
                    _canvasGroup.DOFade(1, _fadeInSpeed).SetEase(_fadeInCurve).OnComplete(() => EndAnimation());
                }
                else
                {
                    _canvasGroup.DOFade(1, _fadeInSpeed).OnComplete(() => EndAnimation());
                }
            }
            else
            {
                _canvasGroup.alpha = 1;

                //
                EndAnimation();
            }
        }

        [Button, Command]
        public void Hide(bool showAnimation = true)
        {
            //
            if (_isAnimating)
                return;

            // 
            _isAnimating = true;

            //
            _canvasGroup.alpha = 1;
            if (showAnimation)
            {
                if (_useFadeOutCurve)
                {
                    _canvasGroup.DOFade(0, _fadeOutSpeed).SetEase(_fadeOutCurve).OnComplete(() => EndAnimation());
                }
                else
                {
                    _canvasGroup.DOFade(0, _fadeOutSpeed).OnComplete(() => EndAnimation());
                }
            }
            else
            {
                _canvasGroup.alpha = 0;

                //
                EndAnimation();
            }
        }

        private void EndAnimation()
        {
            //
            if (_canvasGroup.alpha == 0)
            {
                _onHideEvent?.Invoke();
            }         
            else
            {
                _onShowEvent?.Invoke();
            }   

            //
            _isAnimating = false;
        }
    }
}