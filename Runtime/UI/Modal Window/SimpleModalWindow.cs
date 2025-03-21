using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class SimpleModalWindow : ModalWindow
    {
        [Tab("Debug")]
        [SerializeField, ReadOnly] private List<UIAnimation> _uiAnimations;
        [SerializeField, ReadOnly] private bool _visible;

        public override bool Visible 
        { 
            get => _visible;
            set 
            {
                _visible = value;

                //
                foreach (var animation in _uiAnimations)
                {
                    animation.ShowOrHide(value, true);
                }
            }
        }

        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            _visible = false;

            //
            _uiAnimations = GetComponentsInChildren<UIAnimation>().ToList();
            foreach (var animation in _uiAnimations)
            {
                animation.Hide(false);
            }
        }

        protected override void OnBeforeShow()
        {
            //
            foreach (var animation in _uiAnimations)
            {
                animation.Show(true);
            }
        }

        protected override void OnPostClose()
        {
            //
            foreach (var animation in _uiAnimations)
            {
                animation.Hide(true);
            }
        }
    }
}