using TMPro;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class ClassTrackerUI : BaseTrackerUI
    {
        [SerializeField, Group("References")] private RuntimeClass _runtimeClass;
        [SerializeField, Group("References")] private TextMeshProUGUI _attributeTextLabel;
        [SerializeField, Group("Debug")] private bool _showModifiers = false;

        //
        protected override void RefreshUI()
        {
            foreach (AttributeValuePair attributeValuePair in _runtimeClass.ClassData.Attributes)
            {
                if (_runtimeClass.TryGetAttribute(attributeValuePair.Data, out RuntimeAttribute runtimeAttr))
                {
                    // Format the attribute information and set it to the text label.
                    string formattedText = FormatRuntimeAttribute(runtimeAttr, _showModifiers);
                    _attributeTextLabel.text = $"{formattedText}\n\n";
                }
            }
        }

        protected override bool ValidateReferences()
        {
            if (_runtimeClass == null)
            {
                this.LogError($"Target RuntimeClass reference is missing.", this);
                return false;
            }

            if (_attributeTextLabel == null)
            {
                this.LogError($"Attribute Text Label reference is missing.", this);
                return false;
            }

            return true;
        }
    }
}