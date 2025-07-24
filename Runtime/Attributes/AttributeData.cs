using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    public class AttributeData : Datablock
    {
        public Sprite icon;
        public float minValue;
        public float maxValue;
        public string description;
        public bool isVital;
        public AttributeClampType clampType;
        public List<CustomValueAll> customValues;

        public AttributeData() : base()
        {
            icon = default;
            minValue = 0;
            maxValue = 0;
            description = "";
            isVital = false;
            clampType = AttributeClampType.Floor;
            customValues = new List<CustomValueAll>();
        }
    }
}

