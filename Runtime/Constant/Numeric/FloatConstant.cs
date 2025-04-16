using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.CONSTANTS_NAME + "Float")]
    public class FloatConstant : NumericGameConstant
    {
        [SerializeField] private float _value;

        //
        public override int IntValue => (int)_value;
        public override float FloatValue => _value;
        public override string StringValue => _value.ToString();
    }
}