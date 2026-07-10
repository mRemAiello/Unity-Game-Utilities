using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class RuntimeAttributes : RuntimeAttributesBase
    {
        [SerializeField, TableList, PropertyOrder(0), Group("Attributes")] private List<AttributeValuePair> _attributeValuePair;

        //
        protected override void Init()
        {
            base.Init();

            // 
            if (_attributeValuePair == null)
            {
                this.LogWarning("AttributeValuePair list is null. No attributes to initialize.");
                return;
            }

            //
            foreach (var pair in _attributeValuePair)
            {
                if (pair.Attribute == null)
                    continue;

                //
                ReplaceOrAddAttribute(pair.Attribute, pair.Value);
            }
        }
    }
}