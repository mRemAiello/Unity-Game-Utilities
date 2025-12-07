using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.SAVE_NAME + "Float")]
    public class FloatSettingData : BaseSettingData<float>
    {
        [Group("Setting"), SerializeField] protected Vector2 _valueMinMax = new(0f, 1f);

        // Public readonly fields
        public Vector2 ValueMinMax => _valueMinMax;
    }
}
