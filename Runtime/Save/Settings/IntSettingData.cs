using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = "Doneref/Settings/New Int")]
    public class IntSettingData : BaseSettingData<int>
    {
        [Group("Setting"), SerializeField] protected Vector2Int _valueMinMax = new(0, 1);

        // Public readonly fields
        public Vector2Int ValueMinMax => _valueMinMax;
    }
}
