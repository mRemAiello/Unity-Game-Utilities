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
            _headerText.text = headerText;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void SetBodyText(string text)
        {
            _questionText.text = text;
        }

        [Button(ButtonSizes.Medium)]
        public virtual void AddButton(GameObject buttonPrefab, string text, ModalWindowButtonEventAsset buttonEvent, ModalButtonType type)
        {
            // Validate the button prefab before instantiation.
            if (!buttonPrefab)
            {
                // Warn about a missing button prefab.
                Debug.LogWarning($"{nameof(ModalWindowBase)}: Button prefab is null.", this);
                return;
            }

            // Validate the button root before instantiation.
            if (!_buttonsRoot)
            {
                // Warn about a missing buttons root.
                Debug.LogWarning($"{nameof(ModalWindowBase)}: Buttons root is missing.", this);
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
            ClearButtons();

            //
            OnPostClose();
        }

        //
        [Button(ButtonSizes.Medium)]
        public virtual void ClearButtons()
        {
            // Destroy generated button game objects.
            foreach (var button in _buttons)
            {
                // Skip missing button references.
                if (!button)
                {
                    continue;
                }

                // Destroy the button game object.
                Destroy(button.gameObject);
            }

            // Clear the buttons list.
            _buttons.Clear();
        }

        //
        protected virtual void OnBeforeShow() { }
        protected virtual void OnPostClose() { }
    }
}
