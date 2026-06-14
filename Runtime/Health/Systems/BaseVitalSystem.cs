using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("References")]
    [DeclareBoxGroup("Events")]
    [DeclareBoxGroup("Debug")]
    public abstract class BaseVitalSystem : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("References")] protected RuntimeClass _runtimeClass;
        [SerializeField, Group("References")] protected AttributeData _attributeData;

        [SerializeField, Group("Events")] protected FloatEventAsset _onValueChanged;
        [SerializeField, Group("Events")] protected VoidEventAsset _onMinReached;
        [SerializeField, Group("Events")] protected VoidEventAsset _onMaxReached;

        [SerializeField, Group("Debug")] protected bool _logEnabled = true;
        [SerializeField, ReadOnly, Group("Debug")] protected RuntimeVital _vital;

        // Public accessors
        public bool LogEnabled => _logEnabled;
        public RuntimeVital Vital => _vital;
        public AttributeData AttributeData => _attributeData;
        public RuntimeClass RuntimeClass => _runtimeClass;
        public float CurrentValue => _vital?.CurrentValue ?? 0f;
        public float MaxValue => _vital?.CurrentMaxValue ?? 0f;
        public float MinValue => _vital?.MinValue ?? 0f;
        public float PercentageValue => MaxValue > 0 ? Mathf.Clamp01(CurrentValue / MaxValue) : 0f;
        public bool IsAtMin => Mathf.Approximately(CurrentValue, MinValue);
        public bool IsAtMax => Mathf.Approximately(CurrentValue, MaxValue);

        //
        protected virtual void Start()
        {
            InitializeVital();
        }

        /// <summary>
        /// Initializes the vital reference by retrieving it from the RuntimeClass.
        /// </summary>
        protected virtual void InitializeVital()
        {
            if (_runtimeClass == null)
            {
                this.LogError($"[{nameof(BaseVitalSystem)}] Cannot initialize: RuntimeClass is null on {gameObject.name}.");
                return;
            }

            if (_attributeData == null)
            {
                this.LogError($"[{nameof(BaseVitalSystem)}] Cannot initialize: AttributeData is null on {gameObject.name}.");
                return;
            }

            // Validate that the attribute is marked as vital.
            if (!_attributeData.IsVital)
            {
                this.LogWarning($"[{nameof(BaseVitalSystem)}] AttributeData '{_attributeData.name}' is not marked as Vital. This may cause runtime issues.");
            }

            // Retrieve the runtime vital from the class.
            if (_runtimeClass.TryGetAttribute(_attributeData, out var attribute))
            {
                _vital = attribute as RuntimeVital;
                if (_vital == null)
                {
                    this.LogError($"[{nameof(BaseVitalSystem)}] Attribute '{_attributeData.name}' is not a RuntimeVital on {gameObject.name}.");
                    return;
                }

                this.Log($"[{nameof(BaseVitalSystem)}] Successfully initialized vital '{_attributeData.name}' on {gameObject.name}.");
                OnPostInit();
            }
            else
            {
                this.LogError($"[{nameof(BaseVitalSystem)}] Failed to retrieve attribute '{_attributeData.name}' from RuntimeClass on {gameObject.name}.");
            }
        }

        /// <summary>
        /// Sets the current value of the vital and triggers events.
        /// </summary>
        protected virtual void SetValue(float newValue)
        {
            if (_vital == null)
            {
                this.LogWarning($"[{nameof(BaseVitalSystem)}] Cannot set value: Vital not initialized on {gameObject.name}.");
                return;
            }

            float oldValue = CurrentValue;
            _vital.SetCurrentValue(newValue);
            float actualValue = CurrentValue;

            // Only trigger events if the value actually changed.
            if (!Mathf.Approximately(oldValue, actualValue))
            {
                OnValueChanged(oldValue, actualValue);
                _onValueChanged?.Invoke(actualValue);

                // Check for special threshold events.
                if (IsAtMin)
                {
                    OnMinReached();
                    _onMinReached?.Invoke();
                }
                else if (IsAtMax)
                {
                    OnMaxReached();
                    _onMaxReached?.Invoke();
                }
            }
        }

        /// <summary>
        /// Adds the specified amount to the current value.
        /// </summary>
        public virtual void AddValue(float amount)
        {
            SetValue(CurrentValue + amount);
        }

        /// <summary>
        /// Subtracts the specified amount from the current value.
        /// </summary>
        public virtual void SubtractValue(float amount)
        {
            SetValue(CurrentValue - amount);
        }

        /// <summary>
        /// Sets the vital to its minimum value.
        /// </summary>
        public virtual void SetToMin()
        {
            SetValue(MinValue);
        }

        /// <summary>
        /// Sets the vital to its maximum value.
        /// </summary>
        public virtual void SetToMax()
        {
            SetValue(MaxValue);
        }

        //
        protected virtual void OnPostInit() { }
        protected virtual void OnValueChanged(float oldValue, float newValue) { }
        protected virtual void OnMinReached() { }
        protected virtual void OnMaxReached() { }
    }
}
