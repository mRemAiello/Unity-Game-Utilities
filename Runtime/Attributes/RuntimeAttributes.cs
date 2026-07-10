using System.Collections.Generic;
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
            foreach (var pair in _attributeValuePair)
            {
                if (pair.Attribute != null)
                {
                    CreateRuntimeAttribute(null, pair.Attribute, pair.Value);
                }
            }
        }
    }
}