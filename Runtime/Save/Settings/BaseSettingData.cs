using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Settings")]
    [DeclareBoxGroup("debug", Title = "Debug")]
    public abstract class BaseSettingData<T> : ScriptableObject, ISaveable, ILoggable
    {
        [Group("Setting"), SerializeField] protected string _settingName;
        [Group("Setting"), SerializeField] protected T _defaultValue;
        [Group("debug"), PropertyOrder(99), SerializeField, ReadOnly] protected T _currentValue;
        [Group("debug"), PropertyOrder(100), SerializeField, ReadOnly] protected bool _logEnabled = true;

        /// <summary>
        /// Invoked whenever the setting value changes. UI binders can subscribe to
        /// this event to stay synchronized with the underlying data.
        /// </summary>
        public event Action<T> OnValueChanged;

        //
        public string SettingName => _settingName;
        public T DefaultValue => _defaultValue;
        public T CurrentValue => _currentValue;
        public string SaveContext => "Settings";
        public bool LogEnabled => _logEnabled;

        //
        [Button]
        public virtual void SetValue(T newValue)
        {
            if (!GameSaveManager.InstanceExists)
            {
                this.Log("GameSaveManager instance does not exist. Cannot save setting.");
            }
            else
            {
                GameSaveManager.Instance.Save(this, _settingName, newValue);
            }

            _currentValue = newValue;
            OnValueChanged?.Invoke(_currentValue);
        }

        public virtual T GetValue()
        {
            if (GameSaveManager.InstanceExists && GameSaveManager.Instance.TryLoad(this, _settingName, out T value))
            {
                return value;
            }

            //
            return _defaultValue;
        }

        [Button]
        public virtual void Load()
        {
            if (GameSaveManager.InstanceExists && GameSaveManager.Instance.TryLoad(this, _settingName, out T value))
            {
                SetValue(value);
                return;
            }

            SetValue(_defaultValue);
        }

        public void Save()
        {
            if (!GameSaveManager.InstanceExists)
            {
                this.Log("GameSaveManager instance does not exist. Cannot save setting.");
                return;
            }

            GameSaveManager.Instance.Save(this, _settingName, _currentValue);
        }
    }
}
