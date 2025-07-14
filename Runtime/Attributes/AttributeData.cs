
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("graphics", Title = "Graphics")]
    [DeclareBoxGroup("progress", Title = "Progress")]
    [CreateAssetMenu(menuName = Constant.ATTRIBUTES_NAME + "Attribute")]
    public class AttributeData : ItemAssetBase
    {
        public Sprite icon;
        public float minValue;
        public float maxValue;
        public string description;
        public bool isVital;
        //public AttributeClampType clampType;
        //public List<CustomValueAll> customValues;
    }
}