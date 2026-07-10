using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
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

            // Override with the specific values defined in classData.Attributes.
            foreach (var pair in _classData.Attributes)
            {
                if (pair.Attribute == null)
                    continue;

                //
                ReplaceOrAddAttribute(pair.Attribute, pair.Value);
            }
        }
    }
}