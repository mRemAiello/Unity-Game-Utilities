using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.ATTRIBUTES_NAME + "Class")]
    [DeclareBoxGroup("class", Title = "Class")]
    public class ClassData : ItemAssetBase
    {
        [SerializeField, Group("class"), TableList] private List<AttributeValuePair> _attributes;

        //
        public List<AttributeValuePair> Attributes => _attributes;
    }
}