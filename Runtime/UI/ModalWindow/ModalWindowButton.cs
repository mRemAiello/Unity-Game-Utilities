using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameUtils
{
    public class ModalWindowButton : MonoBehaviour
    {
        [Tab("References")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private ModalButtonType _type;
        [SerializeField, ReadOnly] private ModalWindowButtonEventAsset _buttonEvent;

        //
        public ModalButtonType Type => _type;

        public void Init(string text, ModalWindowButtonEventAsset buttonEvent, ModalButtonType type = ModalButtonType.Normal, UnityEvent onButtonClicked = null)
        {
            _text.text = text;
            _type = type;
            _buttonEvent = buttonEvent;

            //
            _button.onClick.AddListener(new UnityAction(Click));
            // Notify modal-level listeners when the button is clicked.
            _button.onClick.AddListener(() => InvokeButtonEvent(onButtonClicked));
        }

        private void Click()
        {
            _buttonEvent?.Invoke(this);
        }

        private void InvokeButtonEvent(UnityEvent onButtonClicked)
        {
            // Invoke the optional modal-level button event.
            onButtonClicked?.Invoke();
        }
    }
}
