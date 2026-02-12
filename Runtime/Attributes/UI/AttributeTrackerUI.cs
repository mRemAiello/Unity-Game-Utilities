using TMPro;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("references", Title = "References")]
    public class AttributeTrackerUI : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("references")] private RuntimeClass _targetRuntimeClass;
        [SerializeField, Group("references")] private AttributeData _targetAttributeData;
        [SerializeField, Group("references")] private TextMeshProUGUI _attributeTextLabel;
        [SerializeField, Group("debug")] private bool _logEnabled;

        //
        public bool LogEnabled => _logEnabled;

        //
        private void Update()
        {
            if (!ValidateReferences())
                return;

            if (!_targetRuntimeClass.TryGetAttribute(_targetAttributeData, out RuntimeAttribute trackedAttribute))
            {
                this.LogWarning($"[AttributeTrackerUI] Attribute '{_targetAttributeData.name}' not found.", this);
                return;
            }

            UpdateLabel(trackedAttribute);
        }

        private void UpdateLabel(RuntimeAttribute trackedAttribute)
        {
            if (_targetAttributeData.IsVital)
            {
                _attributeTextLabel.text = $"{_targetAttributeData.Name}: {trackedAttribute.CurrentValue}/{trackedAttribute.MaxValue}";
            }
            else
            {
                _attributeTextLabel.text = $"{_targetAttributeData.Name}: {trackedAttribute.CurrentValue}";
            }

            //
            this.Log($"[AttributeTrackerUI] UI updated -> {_attributeTextLabel.text}", this);
        }

        private bool ValidateReferences()
        {
            if (_targetRuntimeClass == null || _targetAttributeData == null || _attributeTextLabel == null)
            {
                this.LogWarning("[AttributeTrackerUI] Missing references.", this);
                return false;
            }

            return true;
        }
    }
}