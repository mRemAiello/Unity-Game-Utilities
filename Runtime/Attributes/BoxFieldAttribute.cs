using UnityEngine;

namespace GameUtils
{
    public class BoxFieldAttribute : PropertyAttribute
    {
        public string Label { get; }

        public BoxFieldAttribute(string label = "")
        {
            Label = label;
        }
    }
}