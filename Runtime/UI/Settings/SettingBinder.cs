using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Generic binder that synchronizes a <see cref="BaseSettingData{T}"/> with a UI component.
    /// Derived classes must implement how to read/write values to the specific UI element.
    /// </summary>
    /// <typeparam name="T">Type of the setting value.</typeparam>
    /// <typeparam name="TUI">Type of the UI component.</typeparam>
    public abstract class SettingBinder<T, TUI> : MonoBehaviour where TUI : Component
    {
        [SerializeField] protected BaseSettingData<T> _data;
        [SerializeField] protected TUI _uiComponent;

        //
        protected virtual void OnEnable()
        {
            if (_data != null)
            {
                _data.Load();
                _data.OnValueChanged += SetUIValue;
                SetUIValue(_data.CurrentValue);
            }

            if (_uiComponent != null)
            {
                AddUIListener();
            }
        }

        protected virtual void OnDisable()
        {
            if (_data != null)
            {
                _data.OnValueChanged -= SetUIValue;
            }

            if (_uiComponent != null)
            {
                RemoveUIListener();
            }
        }

        /// <summary>
        /// Subscribe to UI change events.
        /// Derived classes should link <see cref="OnUIValueChanged"/> to the UI callback.
        /// </summary>
        protected abstract void AddUIListener();

        /// <summary>
        /// Unsubscribe from UI change events.
        /// </summary>
        protected abstract void RemoveUIListener();

        /// <summary>
        /// Set the UI component to display the provided value.
        /// </summary>
        protected abstract void SetUIValue(T value);

        /// <summary>
        /// Retrieve the current value from the UI component.
        /// </summary>
        protected abstract T GetUIValue();

        /// <summary>
        /// Called when the UI component value changes.
        /// Updates the underlying setting data with the current UI value.
        /// </summary>
        protected void OnUIValueChanged()
        {
            if (_data != null)
            {
                _data.SetValue(GetUIValue());
            }
        }
    }
}
