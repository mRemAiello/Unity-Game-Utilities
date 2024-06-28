using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class SimpleModalWindow : ModalWindow
    {
        [Tab("References")]
        private CanvasFader _canvasFader;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private bool _visible;

        public override bool Visible 
        { 
            get => _visible;
            set 
            {
                _visible = value;
                _canvasFader.ShowOrHide(value);
            }
        }
    }
}