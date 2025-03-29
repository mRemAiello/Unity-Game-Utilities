using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GameUtils;

namespace UnityEditor.GameUtils
{
    [CustomEditor(typeof(QualityLevelToggle))]
    public class QualityLevelToggleEditor : Editor
    {
        private const string k_Label = "Enable on QualityLevel";

        public override VisualElement CreateInspectorGUI()
        {
            var qualityLevelNames = QualitySettings.names;
            var maskField = new MaskField(k_Label, qualityLevelNames.ToList(), -1)
            {
                bindingPath = "_mask"
            };

            //
            SerializedProperty maskProperty = serializedObject.FindProperty("_mask");
            maskField.TrackPropertyValue(maskProperty, SetValidLevels);
            maskField.BindProperty(maskProperty);

            //
            VisualElement myInspector = new();
            myInspector.Add(maskField);

            return myInspector;
        }


        void SetValidLevels(SerializedProperty maskProperty)
        {
            var levels = QualityLevelMaskToString(maskProperty.intValue);
            var array = maskProperty.serializedObject.FindProperty("_validLevels");
            array.arraySize = levels.Count;

            for (var i = 0; i < levels.Count; i++)
            {
                var element = array.GetArrayElementAtIndex(i);
                element.stringValue = levels[i];
            }

            maskProperty.serializedObject.ApplyModifiedProperties();
        }

        List<string> QualityLevelMaskToString(int mask)
        {
            List<string> levels = new();
            for (int i = 0; i < QualitySettings.count; i++)
            {
                if ((mask & (1 << i)) > 0)
                {
                    levels.Add(QualitySettings.names[i]);
                }
            }
            return levels;
        }
    }
}