using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VInspector;

namespace GameUtils
{
    public class ModalWindowButton : MonoBehaviour
    {
        [Tab("References")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private ModalButtonType _type;
        [SerializeField, ReadOnly] private ModalWindowButtonEvent _buttonEvent;

        //
        public ModalButtonType Type => _type;

        public void Init(string text, ModalWindowButtonEvent buttonEvent, ModalButtonType type = ModalButtonType.Normal)
        {
            _text.text = text;
            _type = type;
            _buttonEvent = buttonEvent;

            //
            _button.onClick.AddListener(new UnityAction(Click));
        }

        private void Click()
        {
            _buttonEvent?.Invoke(this);
        }
    }
}