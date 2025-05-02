using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class TransformScaler : UIAnimation
    {      
        [Tab("Fade In")]
        [SerializeField] private bool _useFadeInCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeInSpeed = 1.0f;
        [SerializeField] private AnimationCurve _fadeInCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        [Tab("Fade Out")]
        [SerializeField] private bool _useFadeOutCurve = false;
        [SerializeField, Range(0.1f, 100f)] private float _fadeOutSpeed = 1.0f;
        [SerializeField] private AnimationCurve _fadeOutCurve = AnimationCurve.Linear(0, 1, 1, 0);

        protected override void OnShow(bool showAnimation = true)
        {
            //
            transform.localScale = new Vector3(0, 0, 0);
            if (showAnimation)
            {
                if (_useFadeOutCurve)
                {
                    //transform.DOScale(Vector3.one, _fadeOutSpeed).SetEase(_fadeOutCurve).OnComplete(() => EndAnimation(UIAnimationStatus.Showed));
                }
                else
                {
                    //transform.DOScale(Vector3.one, _fadeOutSpeed).OnComplete(() => EndAnimation(UIAnimationStatus.Showed));
                }
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);

                //
                EndAnimation(UIAnimationStatus.Showed);
            }
        }

        protected override void OnHide(bool showAnimation = true)
        {
            // 
            transform.localScale = new Vector3(1, 1, 1);
            if (showAnimation)
            {
                if (_useFadeInCurve)
                {
                    //transform.DOScale(Vector3.zero, _fadeInSpeed).SetEase(_fadeInCurve).OnComplete(() => EndAnimation(UIAnimationStatus.Hided));
                }
                else
                {
                    //transform.DOScale(Vector3.zero, _fadeInSpeed).OnComplete(() => EndAnimation(UIAnimationStatus.Hided));
                }
            }
            else
            {
                transform.localScale = new Vector3(0, 0, 0);

                //
                EndAnimation(UIAnimationStatus.Hided);
            }
        }
    }
}
