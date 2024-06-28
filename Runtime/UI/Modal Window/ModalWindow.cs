using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace GameUtils
{
    public abstract class ModalWindow : Singleton<ModalWindow>
    {
        [Tab("References")]
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _questionText;
        
        [Space]
        [SerializeField] private Button _backgroundButton;
        
        [Space]
        [SerializeField] private Transform _buttonsRoot;

        [Tab("Debug")]
        [SerializeField, ReadOnly] protected List<ModalWindowButton> _buttons = new();

        [SerializeField, ReadOnly] private bool _ignorable;

        public virtual bool Ignorable
        {
            get => _ignorable;
            protected set
            {
                _ignorable = value;

                if (_backgroundButton)
                    _backgroundButton.interactable = value;
            }
        }

        public abstract bool Visible { get; set; }

        protected override void OnPostAwake()
        {
            // 
            if (_backgroundButton)
                _backgroundButton.onClick.AddListener(new UnityEngine.Events.UnityAction(UI_IgnorePopup));
        }

        public virtual void SetHeaderText(string headerText)
        {
            _headerText.text = headerText;
        }

        public virtual void SetBodyText(string text)
        {
            _questionText.text = text;
        }

        public virtual void AddButton(string text, Action action = null, ModalButtonType type = ModalButtonType.Normal)
        {
            if (!_buttonsRoot)
            {
                return;
            }

            /*var button = Instantiate(GetButtonPrefab(type), buttonsRoot);
            button.Init(ButtonPressedCallback, text, action, type);
            m_Buttons.Add(button);*/
        }

        protected virtual void OnBeforeShow()
        {
            if (_buttons.Count == 0)
            {
                AddButton("Ok");
            }
        }

        public virtual void Show()
        {
            OnBeforeShow();

            Visible = true;
            transform.SetAsLastSibling();
        }

        public virtual void Close()
        {
            Visible = false;
            Destroy(gameObject, 1f);
        }

        public virtual void UI_IgnorePopup()
        {
            if (Ignorable)
            {
                Close();
            }
        }

        public virtual void ButtonPressedCallback(ModalWindowButton modalWindowButton)
        {
            Close();
        }
    }
}