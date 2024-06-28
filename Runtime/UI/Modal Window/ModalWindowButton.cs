using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUtils
{
    public class ModalWindowButton : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Button button;

        public delegate void ButtonPressed(ModalWindowButton modalWindowButton);
        public event ButtonPressed OnButtonPressed;
        Action action;

        public void Init(ButtonPressed onButtonPressed, string _text, Action _action, ModalButtonType _type)
        {
            OnButtonPressed = onButtonPressed;
            text.text = _text;
            action = _action;

            button.onClick.AddListener(new UnityEngine.Events.UnityAction(Click));
        }

        public void Click()
        {
            action?.Invoke();
            OnButtonPressed?.Invoke(this);
        }
    }
}