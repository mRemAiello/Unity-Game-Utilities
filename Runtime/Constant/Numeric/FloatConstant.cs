using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.CONSTANTS_NAME + "Float")]
    public class FloatConstant : NumericGameConstant
    {
        [SerializeField] private float _value;

        //
        public override int IntValue => (int)_value;
        public override float FloatValue => _value;
        public override string StringValue => _value.ToString();
    }
}