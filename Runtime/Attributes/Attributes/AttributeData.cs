using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.ATTRIBUTES_NAME + "Attribute")]
    [DeclareBoxGroup("attribute", Title = "Attribute")]
    public class AttributeData : ItemAssetBase
    {
        [SerializeField, Group("attribute")] private float _minValue = 0;
        [SerializeField, Group("attribute")] private float _maxValue = 99;
        [SerializeField, Group("attribute")] private bool _isVital;
        [SerializeField, Group("attribute")] private AttributeClampType _clampType;

        //
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public bool IsVital => _isVital;
        public AttributeClampType ClampType => _clampType;
    }
}