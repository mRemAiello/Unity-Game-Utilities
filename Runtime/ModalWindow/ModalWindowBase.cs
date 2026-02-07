using System.Collections.Generic;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameUtils
{
    [DeclareBoxGroup("references", Title = "References")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("animations", Title = "Animations")]
    public abstract class ModalWindowBase : Singleton<ModalWindowBase>, ILoggable
    {
        [SerializeField, Group("references")] private TextMeshProUGUI _headerText;
        [SerializeField, Group("references")] private TextMeshProUGUI _questionText;
        [SerializeField, Group("references")] private Transform _buttonsRoot;
        [SerializeField, Group("animations")] private Animator _animator;
        [SerializeField, Group("debug")] private bool _logEnabled = false;
        [SerializeField, ReadOnly, HideInEditMode, Group("debug")] protected List<ModalWindowButton> _buttons = new();
        [SerializeField, ReadOnly, HideInEditMode, Group("debug")] private bool _ignorable;

        //
        public virtual bool Ignorable
        {
            get => _ignorable;
            protected set => _ignorable = value;
        }

        //
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
        public virtual void AddButton(GameObject buttonPrefab, string text, UnityAction onButtonClicked)
        {
            // Validate the button prefab before instantiation.
            if (!buttonPrefab)
            {
                // Warn about a missing button prefab.
                this.LogWarning("[ModalWindowBase] Button prefab is null. Cannot create modal button.", this);
                return;
            }

            // Validate the button root before instantiation.
            if (!_buttonsRoot)
            {
                // Warn about a missing buttons root.
                this.LogWarning("[ModalWindowBase] Buttons root is missing. Cannot parent modal button.", this);
                return;
            }

            // 
            var button = Instantiate(buttonPrefab, _buttonsRoot);
            if (button.TryGetComponent(out ModalWindowButton buttonScript))
            {
                // Provide the modal-level button event to the button initialization.
                buttonScript.Init(text, onButtonClicked);
                _buttons.Add(buttonScript);
                // Exit after successful button initialization.
                return;
            }

            // Warn when the prefab lacks the expected button component.
            this.LogWarning("[ModalWindowBase] Button prefab is missing ModalWindowButton. Destroying instance.", this);

            // Destroy the invalid button instance.
            Destroy(button);
        }

        [Button(ButtonSizes.Medium)]
        public void Show()
        {
            OnBeforeShow();

            //
            Visible = true;
            transform.SetAsLastSibling();

            //
            if (_animator == null)
            {
                // Warn when the animator reference is missing.
                this.LogWarning("[ModalWindowBase] Animator reference is null. Open trigger skipped.", this);
                return;
            }

            //
            _animator.SetTrigger("Open");
        }

        [Button(ButtonSizes.Medium)]
        public void Close()
        {
            Visible = false;

            //
            if (_animator == null)
            {
                // Warn when the animator reference is missing.
                this.LogWarning("[ModalWindowBase] Animator reference is null. Close trigger skipped.", this);
                return;
            }

            //
            _animator.SetTrigger("Close");

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
