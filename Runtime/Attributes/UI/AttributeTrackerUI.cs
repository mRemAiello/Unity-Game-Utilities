using TMPro;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Debug")]
    [DeclareBoxGroup("References")]
    public class AttributeTrackerUI : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("References")] private RuntimeClass _targetRuntimeClass;
        [SerializeField, Group("References")] private AttributeData _targetAttributeData;
        [SerializeField, Group("References")] private TextMeshProUGUI _attributeTextLabel;
        [SerializeField, Group("Debug")] private bool _logEnabled;

        //
        public bool LogEnabled => _logEnabled;

        //
        private void Update()
        {
            if (!ValidateReferences())
                return;

            if (!_targetRuntimeClass.TryGetAttribute(_targetAttributeData, out RuntimeAttribute trackedAttribute))
            {
                this.LogWarning($"{_targetAttributeData.Name} Attribute '{_targetAttributeData.name}' not found.", this);
                return;
            }

            UpdateLabel(trackedAttribute);
        }

        private void UpdateLabel(RuntimeAttribute trackedAttribute)
        {
            _attributeTextLabel.text = $"{_targetAttributeData.Name}: {trackedAttribute.CurrentValue}";

            // If it's a vital attribute, also show the max value.
            if (_targetAttributeData.IsVital && trackedAttribute is RuntimeVital vitalAttr)
                _attributeTextLabel.text = $"{_targetAttributeData.Name}: {vitalAttr.CurrentValue}/{vitalAttr.CurrentMaxValue}";

            //
            this.Log($"{_targetAttributeData.Name} UI updated to {_attributeTextLabel.text}", this);
        }

        private bool ValidateReferences()
        {
            if (_targetRuntimeClass == null || _targetAttributeData == null || _attributeTextLabel == null)
            {
                this.LogWarning($"Missing references.", this);
                return false;
            }

            return true;
        }
    }
}