using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = Constant.CONSTANTS_NAME + "String")]
    public class StringConstant : GameConstant
    {
        [SerializeField] private string _value;

        //
        public override string StringValue => _value;
    }
}