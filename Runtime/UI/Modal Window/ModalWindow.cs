using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public abstract class ModalWindow : Singleton<ModalWindow>
    {
        [Tab("References")]
        [SerializeField] private TextMeshProUGUI _headerText;
        [SerializeField] private TextMeshProUGUI _questionText;
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
            }
        }

        public abstract bool Visible { get; set; }

        [Button]
        public virtual void SetHeaderText(string headerText)
        {
            _headerText.text = headerText;
        }

        [Button]
        public virtual void SetBodyText(string text)
        {
            _questionText.text = text;
        }

        [Button]
        public virtual void AddButton(GameObject buttonPrefab, string text, ModalWindowButtonEventAsset buttonEvent, ModalButtonType type)
        {
            if (!_buttonsRoot)
            {
                return;
            }

            // 
            var button = Instantiate(buttonPrefab, _buttonsRoot);
            if (button.TryGetComponent(out ModalWindowButton buttonScript))
            {
                buttonScript.Init(text, buttonEvent, type);
                _buttons.Add(buttonScript);
            }
        }

        [Button]
        public virtual void Show()
        {
            OnBeforeShow();

            //
            Visible = true;
            transform.SetAsLastSibling();
        }

        [Button]
        public virtual void Close()
        {
            Visible = false;
            
            //
            OnPostClose();
        }

        //
        protected virtual void OnBeforeShow() {}
        protected virtual void OnPostClose() {}
    }
}