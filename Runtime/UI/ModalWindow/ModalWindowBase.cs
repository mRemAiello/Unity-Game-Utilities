using System.Collections.Generic;
using TMPro;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("references", Title = "References")]
    public abstract class ModalWindowBase : Singleton<ModalWindowBase>, ILoggable
    {
        [SerializeField, Group("references")] private TextMeshProUGUI _headerText;
        [SerializeField, Group("references")] private TextMeshProUGUI _questionText;
        [SerializeField, Group("references")] private Transform _buttonsRoot;
        [SerializeField, Group("debug")] private bool _logEnabled = false;
        [SerializeField, ReadOnly, HideInEditMode, Group("debug")] protected List<ModalWindowButton> _buttons = new();
        [SerializeField, ReadOnly, HideInEditMode, Group("debug")] private bool _ignorable;

        //
        public virtual bool Ignorable
        {
            get => _ignorable;
            protected set => _ignorable = value;
        }
        public abstract bool Visible { get; set; }
        public bool LogEnabled => _logEnabled;

        //
        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            _buttons = new();
            _ignorable = false;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void SetHeaderText(string headerText)
        {
            // Guard against missing header reference to avoid null access.
            if (!_headerText)
            {
                this.LogWarning("[ModalWindowBase] Header text reference is null.", this);
                return;
            }

            _headerText.text = headerText;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void SetBodyText(string text)
        {
            // Guard against missing body reference to avoid null access.
            if (!_questionText)
            {
                this.LogWarning("[ModalWindowBase] Body text reference is null.", this);
                return;
            }

            _questionText.text = text;
        }

        [Button(ButtonSizes.Medium)]
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

        [Button(ButtonSizes.Medium)]
        public void Show()
        {
            OnBeforeShow();

            //
            Visible = true;
            transform.SetAsLastSibling();
        }

        [Button(ButtonSizes.Medium)]
        public void Close()
        {
            Visible = false;

            //
            OnPostClose();
        }

        //
        protected virtual void OnBeforeShow() { }
        protected virtual void OnPostClose() { }
    }
}
