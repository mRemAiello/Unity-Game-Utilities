using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.CONSTANTS_NAME + "Integer")]
    public class IntConstant : NumericGameConstant
    {
        [SerializeField] private int _value;

        //
        public override int IntValue => _value;
        public override float FloatValue => _value;
        public override string StringValue => _value.ToString();
    }
}