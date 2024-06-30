using Mono.CSharp;
using QFSW.QC;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public abstract class UIAnimation : MonoBehaviour
    {
        [Tab("Events")]
        [SerializeField] private VoidGameEventAsset _onShowEvent;
        [SerializeField] private VoidGameEventAsset _onHideEvent;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private UIAnimationStatus _animationStatus = UIAnimationStatus.None;

        // 
        public VoidGameEventAsset OnShowEvent => _onShowEvent;
        public VoidGameEventAsset OnHideEvent => _onHideEvent;

        void Awake()
        {
            _animationStatus = UIAnimationStatus.None;
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

        public void Show(bool showAnimation = true)
        {
            //
            if (_animationStatus == UIAnimationStatus.Animating || _animationStatus == UIAnimationStatus.Showed)
                return;

            // 
            _animationStatus = UIAnimationStatus.Animating;

            //
            OnShow(showAnimation);
        }

        protected virtual void OnShow(bool showAnimation = true)
        {
        }

        public void Hide(bool showAnimation = true)
        {
            //
            if (_animationStatus == UIAnimationStatus.Animating || _animationStatus == UIAnimationStatus.Hided)
                return;

            // 
            _animationStatus = UIAnimationStatus.Animating;

            //
            OnHide(showAnimation);
        }

        protected virtual void OnHide(bool showAnimation = true)
        {          
        }

        protected void EndAnimation(UIAnimationStatus status)
        {
            //
            if (status == UIAnimationStatus.Hided)
            {
                _onHideEvent?.Invoke();
            }         
            else
            {
                _onShowEvent?.Invoke();
            }

            //
            _animationStatus = status;
        }
    }
}
