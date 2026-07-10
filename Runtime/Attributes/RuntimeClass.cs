using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Class")]
    public class RuntimeClass : RuntimeAttributesBase
    {
        [SerializeField, Group("Attributes"), ShowProperties] protected ClassData _classData;

        //
        public ClassData ClassData => _classData;

        // 
        protected override void Init()
        {
            base.Init();

            //
            if (_classData != null)
            {
                SetClass(_classData);
            }
        }

        [Button(ButtonSizes.Medium)]
        public virtual void SetClass(ClassData classData)
        {
            _classData = classData;
            _attributes = new List<RuntimeAttribute>();

            // Dictionary to store attribute data and their values.
            Dictionary<AttributeData, float> attributeValues = new();

            // If LoadAllAttributes is enabled, start by adding all attributes from AttributeDataManager.
            if (classData.LoadAllAttributes && AttributeDataManager.InstanceExists)
            {
                foreach (var data in AttributeDataManager.Instance.Items)
                {
                    if (data != null)
                    {
                        attributeValues[data] = data.MinValue;
                    }
                }
            }

            // Override with the specific values defined in classData.Attributes.
            foreach (var data in classData.Attributes)
            {
                if (data.Attribute != null)
                {
                    attributeValues[data.Attribute] = data.Value;
                }
            }

            // Create runtime instances for all collected attributes.
            foreach (var kvp in attributeValues)
            {
                var runtimeAttribute = CreateRuntimeAttribute(classData, kvp.Key, kvp.Value);
                if (runtimeAttribute != null)
                    _attributes.Add(runtimeAttribute);
            }
        }
    }
}
