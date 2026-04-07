using TMPro;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("References")]
    [DeclareBoxGroup("Debug")]
    public class AttributeTrackerUI : BaseTrackerUI
    {
        [SerializeField, Group("References")] private RuntimeClass _targetRuntimeClass;
        [SerializeField, Group("References")] private AttributeData _targetAttributeData;
        [SerializeField, Group("References")] private TextMeshProUGUI _attributeTextLabel;
        [SerializeField, Group("Debug")] private bool _showModifiers = false;

        //
        protected override bool ValidateReferences()
        {
            if (_targetRuntimeClass == null || _targetAttributeData == null || _attributeTextLabel == null)
            {
                this.LogWarning($"Missing references.", this);
                return false;
            }

            return true;
        }

        protected override void RefreshUI()
        {
            if (!_targetRuntimeClass.TryGetAttribute(_targetAttributeData, out RuntimeAttribute trackedAttribute))
            {
                this.LogWarning($"{_targetAttributeData.Name} Attribute '{_targetAttributeData.name}' not found.", this);
                return;
            }

            // Update the label text with the current attribute value.
            _attributeTextLabel.text = FormatRuntimeAttribute(trackedAttribute, _showModifiers);
        }
    }
}